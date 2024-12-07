﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Calendrie.Specialized;

using Calendrie.Core.Validation;
using Calendrie.Hemerology;

/// <summary>
/// Provides common adjusters for <see cref="GregorianDate"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class GregorianAdjuster : SpecialAdjuster<GregorianDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianAdjuster"/> class.
    /// </summary>
    internal GregorianAdjuster(GregorianCalendar calendar) : base(calendar) { }

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private protected sealed override GregorianDate NewDate(int daysSinceZero) => new(daysSinceZero);
}

/// <summary>
/// Represents the Gregorian date.
/// <para><see cref="GregorianDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct GregorianDate :
    IDate<GregorianDate, GregorianCalendar>,
    IAdjustable<GregorianDate>
{ }

public partial struct GregorianDate // Factories
{
    /// <inheritdoc />
    [Pure]
    public static GregorianDate FromDayNumber(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        return new(dayNumber.DaysSinceZero);
    }
}

public partial struct GregorianDate // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() => Schema.CountDaysInYearBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() => Schema.CountDaysInYearAfter(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => Schema.CountDaysInMonthBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() => Schema.CountDaysInMonthAfter(_daysSinceZero);
}

public partial struct GregorianDate // Adjustments
{
    /// <inheritdoc />
    /// <remarks>See also <seealso cref="Adjuster"/>.</remarks>
    [Pure]
    public GregorianDate Adjust(Func<GregorianDate, GregorianDate> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        return adjuster.Invoke(this);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return new(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return new(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        s_Domain.CheckOverflow(dayNumber);
        return new(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return new(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return new(dayNumber.DaysSinceZero);
    }
}

public partial struct GregorianDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero == right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator !=(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero != right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public bool Equals(GregorianDate other) => _daysSinceZero == other._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is GregorianDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceZero;
}

public partial struct GregorianDate // IComparable
{
    /// <inheritdoc />
    public static bool operator <(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero < right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator <=(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero <= right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator >(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero > right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator >=(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero >= right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public static GregorianDate Min(GregorianDate x, GregorianDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static GregorianDate Max(GregorianDate x, GregorianDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(GregorianDate other) => _daysSinceZero.CompareTo(other._daysSinceZero);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is GregorianDate date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(GregorianDate), obj);
}

public partial struct GregorianDate // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    public static int operator -(GregorianDate left, GregorianDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static GregorianDate operator +(GregorianDate value, int days) => value.AddDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static GregorianDate operator -(GregorianDate value, int days) => value.AddDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    public static GregorianDate operator ++(GregorianDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    public static GregorianDate operator --(GregorianDate value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(GregorianDate other) =>
        // No need to use a checked context here.
        _daysSinceZero - other._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public GregorianDate AddDays(int days)
    {
        int daysSinceZero = checked(_daysSinceZero + days);

        // Don't write (the addition may also overflow...):
        // > s_Domain.CheckOverflow(Epoch + daysSinceZero);
        if (daysSinceZero < s_MinDaysSinceZero || daysSinceZero > s_MaxDaysSinceZero)
            ThrowHelpers.ThrowDateOverflow();

        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate NextDay()
    {
        if (this == s_MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceZero + 1);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate PreviousDay()
    {
        if (this == s_MinValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceZero - 1);
    }
}
