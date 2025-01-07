﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

#region CivilDate

/// <summary>
/// Represents the Civil date.
/// <para><i>All</i> dates within the range [1..9999] of years are supported.
/// </para>
/// <para><see cref="CivilDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct CivilDate :
    IDate<CivilDate>,
    ICalendarBound<CivilCalendar>,
    IUnsafeFactory<CivilDate>,
    ISubtractionOperators<CivilDate, CivilDate, int>
{ }

public partial struct CivilDate // Factories & conversions
{
    /// <inheritdoc />
    [Pure]
    public static CivilDate FromDayNumber(DayNumber dayNumber)
    {
        int daysSinceZero = dayNumber.DaysSinceZero;

        if (unchecked((uint)daysSinceZero) > MaxDaysSinceZero)
            throw new ArgumentOutOfRangeException(nameof(dayNumber));

        return new CivilDate(daysSinceZero);
    }

    [Pure]
    static CivilDate IUnsafeFactory<CivilDate>.UnsafeCreate(int daysSinceZero) =>
        new(daysSinceZero);
}

public partial struct CivilDate // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() => Calendar.Schema.CountDaysInYearBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() => Calendar.Schema.CountDaysInYearAfter(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => Calendar.Schema.CountDaysInMonthBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() => Calendar.Schema.CountDaysInMonthAfter(_daysSinceZero);
}

public partial struct CivilDate // Find close by day of the week
{
    /// <inheritdoc />
    [Pure]
    public CivilDate Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceZero = _daysSinceZero + (δ >= 0 ? δ - DaysInWeek : δ);
        if (daysSinceZero < 0) ThrowHelpers.ThrowDateOverflow();
        return new CivilDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceZero = _daysSinceZero + (δ > 0 ? δ - DaysInWeek : δ);
        if (daysSinceZero < 0) ThrowHelpers.ThrowDateOverflow();
        return new CivilDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate Nearest(DayOfWeek dayOfWeek)
    {
        int daysSinceZero = DayNumber.Nearest(dayOfWeek).DaysSinceZero;
        if (unchecked((uint)daysSinceZero) > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new CivilDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate NextOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceZero = _daysSinceZero + (δ < 0 ? δ + DaysInWeek : δ);
        if (daysSinceZero > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new CivilDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceZero = _daysSinceZero + (δ <= 0 ? δ + DaysInWeek : δ);
        if (daysSinceZero > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new CivilDate(daysSinceZero);
    }
}

public partial struct CivilDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(CivilDate left, CivilDate right) =>
        left._daysSinceZero == right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator !=(CivilDate left, CivilDate right) =>
        left._daysSinceZero != right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public bool Equals(CivilDate other) => _daysSinceZero == other._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CivilDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceZero;
}

public partial struct CivilDate // IComparable
{
    /// <summary>
    /// Compares the two specified dates to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(CivilDate left, CivilDate right) =>
        left._daysSinceZero < right._daysSinceZero;

    /// <summary>
    /// Compares the two specified dates to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(CivilDate left, CivilDate right) =>
        left._daysSinceZero <= right._daysSinceZero;

    /// <summary>
    /// Compares the two specified dates to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(CivilDate left, CivilDate right) =>
        left._daysSinceZero > right._daysSinceZero;

    /// <summary>
    /// Compares the two specified dates to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(CivilDate left, CivilDate right) =>
        left._daysSinceZero >= right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public static CivilDate Min(CivilDate x, CivilDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static CivilDate Max(CivilDate x, CivilDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(CivilDate other) => _daysSinceZero.CompareTo(other._daysSinceZero);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is CivilDate date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(CivilDate), obj);
}

public partial struct CivilDate // Standard math ops
{
    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountDaysSince()")]
    public static int operator -(CivilDate left, CivilDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static CivilDate operator +(CivilDate value, int days) => value.PlusDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static CivilDate operator -(CivilDate value, int days) => value.PlusDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextDay()")]
    public static CivilDate operator ++(CivilDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousDay()")]
    public static CivilDate operator --(CivilDate value) => value.PreviousDay();

    /// <summary>
    /// Counts the number of days elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountDaysSince(CivilDate other) =>
        // No need to use a checked context here. Indeed, the absolute value of
        // the result is at most equal to MaxDaysSinceZero.
        _daysSinceZero - other._daysSinceZero;

    /// <summary>
    /// Adds a number of days to the current instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public CivilDate PlusDays(int days)
    {
        int daysSinceZero = checked(_daysSinceZero + days);
        if (unchecked((uint)daysSinceZero) > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new CivilDate(daysSinceZero);
    }

    /// <summary>
    /// Obtains the date after the current instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [Pure]
    public CivilDate NextDay()
    {
        if (_daysSinceZero == MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new CivilDate(_daysSinceZero + 1);
    }

    /// <summary>
    /// Obtains the date before the current instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [Pure]
    public CivilDate PreviousDay()
    {
        if (_daysSinceZero == 0) ThrowHelpers.ThrowDateOverflow();
        return new CivilDate(_daysSinceZero - 1);
    }

    //
    // Math operations based on the week unit
    //

    /// <summary>
    /// Counts the number of weeks elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountWeeksSince(CivilDate other) => MathZ.Divide(CountDaysSince(other), DaysInWeek);

    /// <summary>
    /// Adds a number of weeks to the current instance, yielding a new value.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public CivilDate AddWeeks(int weeks) => PlusDays(DaysInWeek * weeks);

    /// <summary>
    /// Obtains the date after the current instance falling on the same day of
    /// the week, yielding a new date.
    /// </summary>
    [Pure]
    public CivilDate NextWeek() => PlusDays(DaysInWeek);

    /// <summary>
    /// Obtains the date before the current instance falling on the same day of
    /// the week, yielding a new date.
    /// </summary>
    [Pure]
    public CivilDate PreviousWeek() => PlusDays(-DaysInWeek);
}

#endregion
