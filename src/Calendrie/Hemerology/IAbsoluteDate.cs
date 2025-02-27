﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

using Calendrie.Core.Utilities;

using static Calendrie.Core.CalendricalConstants;

// L'interface IAbsoluteDate<T> est prévue pour les dates fonctionnant avec une
// __seule espèce de calendrier__, d'où le fait d'avoir choisi des propriétés et
// méthodes statiques :
// - IMinMaxValue<T>
// - Calendar
// - FromDayNumber()
//
// Pour des types date partagés par plusieurs calendriers, on utilisera plutôt
// une propriété non-statique Calendar et on ajoutera une méthode
// WithCalendar(newCalendar) pour l'interconversion.

#region Developer Notes

// TL;DR: Despite its flaws we'll use the CRTP.
//
// Problems
// ========
//
// "Covariant return type"
// -----------------------
//
// > interface IFoo {
// >   THIS Xyz(args);
// > }
//
// where "THIS" would be the type implementing the interface.
// This is not possible in C#. Furthermore, the problem is NOT well-defined:
// > class Foo : IFoo { Bar Xyz() { ... } }
// > class Bar : Foo { }
// As we can see, Bar does implement IFoo but Xyz() returns a Foo, not a Bar.
// Of course, if Foo is sealed, this is no longer a problem.
//
// "Contravariant parameter type"
// ------------------------------
//
// Similar problem but this time with a parameter type
// > interface IFoo {
// >   Bar Xyz(THIS, ...);
// > }
//
// And what about?
// > interface IFoo {
// >   THIS Xyz(THIS, ...);
// > }
//
// Solutions?
// ==========
//
// There is no perfect answer, the type system is simply not rich enough.
//
// The simplest one? Avoid the problem by removing the culprit methods from
// the interface... Not what we really want to achieve but sometimes we have
// to admit that there is simply no good answer.
// > interface IFooHelper<T> where T : IFoo {
// >   T Xyz(T foo, args);
// > }
// > class FooHelper<Foo> : IFooHelper<Foo> {
// >   Foo Xyz(Foo foo, args) { ... }
// > }
// This works fine in both covariant and contravariant cases.
// Another way to avoid the problem is to use static abstract methods. In
// fact, this is what we end up doing with the operators.
//
// "Covariant return type"
// -----------------------
//
// First possibility: return an IFoo.
// > interface IFoo {
// >   IFoo Xyz(args);
// > }
// It's not semantically equivalent as a class Foo implementing this
// interface may return anything, not just a Foo, as long as it is an IFoo.
// If covariant return types were supported with interfaces, we could write:
// > class Foo : IFoo {
// >   Foo Xyz(args)  { .. }
// > }
// Still not perfect because one cannot force all implementers to do the
// "right" thing.
//
// Another possibility is the CRTP (Curiously Recurring Template Pattern).
// > interface IFoo<TSelf> where TSelf : IFoo<TSelf> {
// >   TSelf Xyz();
// > }
// It seems to work but it does not:
// > class Alpha : IFoo<Alpha> {}
// > class Beta : IFoo<Alpha> {}
// The type constraint is not strong enough: because Alpha implements IFoo<>
// Beta can implement IFoo<> while TSelf != "Beta".
// If IFoo contains "regular" methods/props, better to split the interface:
// > interface IFoo {
// >   int Klm();
// > }
// > interface IFoo<TSelf> where TSelf : IFoo, IFoo<TSelf> {
// >   TSelf Xyz();
// > }
// Awkward but it does what it's supposed to do.
//
// Variant of the first possibility:
// > interface IFoo {
// >   IFoo Xyz();
// > }
// > class Foo : IFoo {
// >   Foo Xyz(args)  { .. }
// >   IFoo IFoo.Xyz(args) => Xyz(args);
// > }
//
// Another variant of the first possibility:
// > interface IFoo {
// >   IFoo Xyz(args);
// > }
// > interface IFoo<T> : IFoo where TSelf : IFoo<TSelf> {
// >   new TSelf Xyz(args);
// > }
// > class Foo : IFoo<Foo> {
// >   Foo Xyz(args)  { .. }
// >   IFoo IFoo.Xyz(args) => Xyz(args);
// > }
//
// "Contravariant parameter type"
// ------------------------------
//
// Use a separate interface?
// > interface IXyzable<TOther> {
// >   Bar Xyz(TOther);
// > }
// where TOther is not necessary an IFoo.
//
// Final remarks
// =============
//
// Problem?
// > Also, there is a perf consideration here because this is byref (or
// > inref for readonly struct) which can hurt inlining and other
// > optimizations done by the JIT.
// https://github.com/dotnet/designs/pull/205#discussion_r619273410
//
// Proposal related to the CRTP.
// https://github.com/dotnet/csharplang/blob/main/proposals/self-constraint.md

#endregion

/// <summary>
/// Defines an absolute date.
/// <para>A date is said to be <i>absolute</i> if it's attached to a global
/// timeline. In this project, it means that it can be mapped to a
/// <see cref="Calendrie.DayNumber"/>.
/// </para>
/// </summary>
public interface IAbsoluteDate
{
    /// <summary>
    /// Gets the day number.
    /// </summary>
    DayNumber DayNumber { get; }

    /// <summary>
    /// Gets the count of consecutive days since the epoch of the calendar to
    /// which belongs the current instance.
    /// </summary>
    int DaysSinceEpoch { get; }

    /// <summary>
    /// Gets the day of the week.
    /// </summary>
    DayOfWeek DayOfWeek { get; }

    //
    // Static helpers
    //

    /// <summary>
    /// Obtains the date strictly before the specified value that falls on the
    /// specified day of the week.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfWeek"/>
    /// is not a valid day of the week.</exception>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported days.</exception>
    [Pure]
    public static TDate Previous<TDate>(TDate date, DayOfWeek dayOfWeek)
        where TDate : IAbsoluteDate, IAdditionOperators<TDate, int, TDate>
    {
        ArgumentNullException.ThrowIfNull(date);
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - date.DayOfWeek;
        return date + (δ >= 0 ? δ - DaysPerWeek : δ);
    }

    /// <summary>
    /// Obtains the date on or before the specified value that falls on the
    /// specified day of the week.
    /// <para>If the date already falls on the given day of the week, returns
    /// the same instance.</para>
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfWeek"/>
    /// is not a valid day of the week.</exception>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported days.</exception>
    [Pure]
    public static TDate PreviousOrSame<TDate>(TDate date, DayOfWeek dayOfWeek)
        where TDate : IAbsoluteDate, IAdditionOperators<TDate, int, TDate>
    {
        ArgumentNullException.ThrowIfNull(date);
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - date.DayOfWeek;
        return δ == 0 ? date : date + (δ > 0 ? δ - DaysPerWeek : δ);
    }

    /// <summary>
    /// Obtains the nearest day that falls on the specified day of the week.
    /// <para>If the day already falls on the given day of the week, returns the
    /// current instance.</para>
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfWeek"/>
    /// is not a valid day of the week - or - the result would be outside the
    /// range of supported days.</exception>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported days.</exception>
    [Pure]
    public static TDate Nearest<TDate>(TDate date, DayOfWeek dayOfWeek)
        where TDate : IAbsoluteDate<TDate>
    {
        var nearest = date.DayNumber.Nearest(dayOfWeek);
        return TDate.FromDayNumber(nearest);
    }

    /// <summary>
    /// Obtains the date on or after the specified value that falls on the
    /// specified day of the week.
    /// <para>If the date already falls on the given day of the week, returns
    /// the same instance.</para>
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfWeek"/>
    /// is not a valid day of the week.</exception>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported days.</exception>
    [Pure]
    public static TDate NextOrSame<TDate>(TDate date, DayOfWeek dayOfWeek)
        where TDate : IAbsoluteDate, IAdditionOperators<TDate, int, TDate>
    {
        ArgumentNullException.ThrowIfNull(date);
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - date.DayOfWeek;
        return δ == 0 ? date : date + (δ < 0 ? δ + DaysPerWeek : δ);
    }

    /// <summary>
    /// Obtains the date strictly after the specified value that falls on the
    /// specified day of the week.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfWeek"/>
    /// is not a valid day of the week.</exception>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported days.</exception>
    [Pure]
    public static TDate Next<TDate>(TDate date, DayOfWeek dayOfWeek)
        where TDate : IAbsoluteDate, IAdditionOperators<TDate, int, TDate>
    {
        ArgumentNullException.ThrowIfNull(date);
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - date.DayOfWeek;
        return date + (δ <= 0 ? δ + DaysPerWeek : δ);
    }
}

/// <summary>
/// Defines an absolute date type.
/// <para>A date is said to be <i>absolute</i> if it's attached to a global
/// timeline. In this project, it means that it can be mapped to a
/// <see cref="DayNumber"/>.</para>
/// <para>This interface SHOULD NOT be implemented by types participating in a
/// poly-calendar system; see <see cref="IAbsoluteDateBase{TSelf}"/> for a more
/// suitable interface.</para>
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IAbsoluteDate<TSelf> :
    IAbsoluteDateBase<TSelf>,
    IMinMaxValue<TSelf>
    where TSelf : IAbsoluteDate<TSelf>
{
    /// <summary>
    /// Creates a new instance of the <typeparamref name="TSelf"/> struct from
    /// the specified <see cref="DayNumber"/> value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayNumber"/>
    /// is outside the range of supported values.</exception>
    [Pure] static abstract TSelf FromDayNumber(DayNumber dayNumber);
}
