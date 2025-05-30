﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable IDE0002 // Simplify Member Access (Style) ✓

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
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
    IUnsafeFactory<CivilDate>,
    ISubtractionOperators<CivilDate, CivilDate, int>
{ }

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

public partial struct CivilDate // Find a close by day of the week
{
    /// <inheritdoc />
    [Pure]
    public CivilDate Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceZero = _daysSinceZero + (δ >= 0 ? δ - DaysPerWeek : δ);
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
        int daysSinceZero = _daysSinceZero + (δ > 0 ? δ - DaysPerWeek : δ);
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
        int daysSinceZero = _daysSinceZero + (δ < 0 ? δ + DaysPerWeek : δ);
        if (daysSinceZero > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new CivilDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceZero = _daysSinceZero + (δ <= 0 ? δ + DaysPerWeek : δ);
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
    /// Counts the number of whole days from <paramref name="other"/> to this
    /// date instance.
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
    /// Counts the number of whole weeks from <paramref name="other"/> to this
    /// date instance.
    /// </summary>
    [Pure]
    public int CountWeeksSince(CivilDate other) => MathZ.Divide(CountDaysSince(other), DaysPerWeek);

    /// <summary>
    /// Adds a number of weeks to the current instance, yielding a new value.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public CivilDate PlusWeeks(int weeks) => PlusDays(DaysPerWeek * weeks);

    /// <summary>
    /// Obtains the date after the current instance falling on the same day of
    /// the week, yielding a new date.
    /// </summary>
    [Pure]
    public CivilDate NextWeek() => PlusDays(DaysPerWeek);

    /// <summary>
    /// Obtains the date before the current instance falling on the same day of
    /// the week, yielding a new date.
    /// </summary>
    [Pure]
    public CivilDate PreviousWeek() => PlusDays(-DaysPerWeek);
}

#endregion

#region CivilMonth

/// <summary>
/// Represents the Civil month.
/// <para><i>All</i> months within the range [1..9999] of years are supported.
/// </para>
/// <para><see cref="CivilMonth"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct CivilMonth :
    IMonth<CivilMonth>,
    IUnsafeFactory<CivilMonth>,
    // A month viewed as a finite sequence of days
    IDaySegment<CivilDate>,
    ISetMembership<CivilDate>,
    // Arithmetic
    ISubtractionOperators<CivilMonth, CivilMonth, int>
{ }

public partial struct CivilMonth // Factories
{
    /// <inheritdoc />
    [Pure]
    public static CivilMonth Create(int year, int month) => new(year, month);

    /// <summary>
    /// Attempts to create a new instance of the <see cref="CivilMonth"/>
    /// struct from the specified month components.
    /// </summary>
    [Pure]
    public static CivilMonth? TryCreate(int year, int month)
    {
        // The calendar being regular, no need to use the PreValidator.
        if (year < CivilScope.MinYear || year > CivilScope.MaxYear
            || month < 1 || month > CivilSchema.MonthsPerYear)
        {
            return null;
        }

        return UnsafeCreate(year, month);
    }

    // Explicit implementation: CivilMonth being a value type, better
    // to use the other TryCreate().
    [Pure]
    static bool IMonth<CivilMonth>.TryCreate(int year, int month, out CivilMonth result)
    {
        var monthValue = TryCreate(year, month);
        result = monthValue ?? default;
        return monthValue.HasValue;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CivilMonth"/> struct
    /// from the specified month components.
    /// <para>This method does NOT validate its parameters.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static CivilMonth UnsafeCreate(int year, int month)
    {
        int monthsSinceEpoch = CountMonthsSinceEpoch(year, month);
        return new CivilMonth(monthsSinceEpoch);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CivilMonth"/> struct
    /// from the specified count of consecutive months since the epoch.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static CivilMonth UnsafeCreate(int monthsSinceEpoch) => new(monthsSinceEpoch);

    [Pure]
    static CivilMonth IUnsafeFactory<CivilMonth>.UnsafeCreate(int monthsSinceEpoch) =>
        UnsafeCreate(monthsSinceEpoch);

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CountMonthsSinceEpoch(int y, int m) =>
        // See RegularSchema.CountMonthsSinceEpoch().
        CivilSchema.MonthsPerYear * (y - 1) + m - 1;
}

public partial struct CivilMonth // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedMonthsInYear() => Month - 1;

    /// <inheritdoc />
    [Pure]
    public int CountRemainingMonthsInYear() => CivilSchema.MonthsPerYear - Month;

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear()
    {
        var (y, m) = this;
        return Calendar.Schema.CountDaysInYearBeforeMonth(y, m);
    }

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear()
    {
        var (y, m) = this;
        return Calendar.Schema.CountDaysInYearAfterMonth(y, m);
    }
}

public partial struct CivilMonth // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public CivilMonth WithYear(int newYear)
    {
        int m = Month;

        // Even when "newYear" is valid, we should re-check "m", but the calendar
        // being regular this is not needed here.
        // The calendar being regular, no need to use the Scope:
        // > Calendar.Scope.ValidateYearMonth(newYear, m, nameof(newYear));
        if (newYear < CivilScope.MinYear || newYear > CivilScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(newYear, nameof(newYear));

        return UnsafeCreate(newYear, m);
    }

    /// <inheritdoc />
    [Pure]
    public CivilMonth WithMonth(int newMonth)
    {
        int y = Year;

        // We already know that "y" is valid, we only need to check "newMonth".
        // The calendar being regular, no need to use the Scope:
        // > Calendar.Scope.PreValidator.ValidateMonth(y, newMonth, nameof(newMonth));
        if (newMonth < 1 || newMonth > CivilSchema.MonthsPerYear)
            ThrowHelpers.ThrowMonthOutOfRange(newMonth, nameof(newMonth));

        return UnsafeCreate(y, newMonth);
    }
}

public partial struct CivilMonth // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(CivilMonth left, CivilMonth right) =>
        left._monthsSinceEpoch == right._monthsSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(CivilMonth left, CivilMonth right) =>
        left._monthsSinceEpoch != right._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(CivilMonth other) => _monthsSinceEpoch == other._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CivilMonth month && Equals(month);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _monthsSinceEpoch;
}

public partial struct CivilMonth // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(CivilMonth left, CivilMonth right) =>
        left._monthsSinceEpoch < right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(CivilMonth left, CivilMonth right) =>
        left._monthsSinceEpoch <= right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(CivilMonth left, CivilMonth right) =>
        left._monthsSinceEpoch > right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(CivilMonth left, CivilMonth right) =>
        left._monthsSinceEpoch >= right._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static CivilMonth Min(CivilMonth x, CivilMonth y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static CivilMonth Max(CivilMonth x, CivilMonth y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(CivilMonth other) => _monthsSinceEpoch.CompareTo(other._monthsSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is CivilMonth month ? CompareTo(month)
        : ThrowHelpers.ThrowNonComparable(typeof(CivilMonth), obj);
}

public partial struct CivilMonth // Standard math ops
{
    /// <summary>
    /// Subtracts the two specified months and returns the number of months
    /// between them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountMonthsSince()")]
    public static int operator -(CivilMonth left, CivilMonth right) => left.CountMonthsSince(right);

    /// <summary>
    /// Adds a number of months to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported months.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusMonths()")]
    public static CivilMonth operator +(CivilMonth value, int months) => value.PlusMonths(months);

    /// <summary>
    /// Subtracts a number of months to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported months.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusMonths()")]
    public static CivilMonth operator -(CivilMonth value, int months) => value.PlusMonths(-months);

    /// <summary>
    /// Adds one month to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported month.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextMonth()")]
    public static CivilMonth operator ++(CivilMonth value) => value.NextMonth();

    /// <summary>
    /// Subtracts one month to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported month.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousMonth()")]
    public static CivilMonth operator --(CivilMonth value) => value.PreviousMonth();

    /// <summary>
    /// Counts the number of whole months elapsed since the specified month.
    /// </summary>
    [Pure]
    public int CountMonthsSince(CivilMonth other) =>
        // No need to use a checked context here. Indeed, the absolute value of
        // the result is at most equal to MaxMonthsSinceEpoch.
        _monthsSinceEpoch - other._monthsSinceEpoch;

    /// <summary>
    /// Adds a number of months to the current instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported months.
    /// </exception>
    [Pure]
    public CivilMonth PlusMonths(int months)
    {
        int monthsSinceEpoch = checked(_monthsSinceEpoch + months);
        if (unchecked((uint)monthsSinceEpoch) > MaxMonthsSinceEpoch)
            ThrowHelpers.ThrowMonthOverflow();
        return new CivilMonth(monthsSinceEpoch);
    }

    /// <summary>
    /// Obtains the month after the current instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported month.</exception>
    [Pure]
    public CivilMonth NextMonth()
    {
        if (_monthsSinceEpoch == MaxMonthsSinceEpoch) ThrowHelpers.ThrowMonthOverflow();
        return new CivilMonth(_monthsSinceEpoch + 1);
    }

    /// <summary>
    /// Obtains the month before the current instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported month.</exception>
    [Pure]
    public CivilMonth PreviousMonth()
    {
        if (_monthsSinceEpoch == 0) ThrowHelpers.ThrowMonthOverflow();
        return new CivilMonth(_monthsSinceEpoch - 1);
    }
}

public partial struct CivilMonth // Non-standard math ops
{
    /// <summary>
    /// Adds the specified number of years to the year part of this month
    /// instance, yielding a new date.
    /// <para>The underlying calendar being regular, this operation is <i>always</i>
    /// exact.</para>
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported months.</exception>
    [Pure]
    public CivilMonth PlusYears(int years)
    {
        var (y, m) = this;
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < CivilScope.MinYear || newY > CivilScope.MaxYear)
            ThrowHelpers.ThrowMonthOverflow();

        return UnsafeCreate(newY, m);
    }

    [Pure]
    CivilMonth IMonthBase<CivilMonth>.PlusYears(int years, out int roundoff)
    {
        roundoff = 0;
        return PlusYears(years);
    }

    /// <summary>
    /// Counts the number of whole years from <paramref name="other"/> to this
    /// month instance.
    /// </summary>
    [Pure]
    public int CountYearsSince(CivilMonth other)
    {
        // Exact difference between two calendar years.
        int years = Year - other.Year;

        var newStart = other.PlusYears(years);
        if (other < this)
        {
            if (newStart > this) years--;
        }
        else
        {
            if (newStart < this) years++;
        }

        return years;
    }
}

#endregion

#region CivilYear

/// <summary>
/// Represents the Civil year.
/// <para><i>All</i> years within the range [1..9999] of years are supported.
/// </para>
/// <para><see cref="CivilYear"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct CivilYear :
    IYear<CivilYear>,
    // A year viewed as a finite sequence of months
    IMonthSegment<CivilMonth>,
    ISetMembership<CivilMonth>,
    // A year viewed as a finite sequence of days
    IDaySegment<CivilDate>,
    ISetMembership<CivilDate>,
    // Arithmetic
    ISubtractionOperators<CivilYear, CivilYear, int>
{ }

public partial struct CivilYear // Factories
{
    /// <inheritdoc />
    [Pure]
    public static CivilYear Create(int year) => new(year);

    /// <summary>
    /// Attempts to create a new instance of the <see cref="CivilYear"/>
    /// struct from the specified year.
    /// </summary>
    [Pure]
    public static CivilYear? TryCreate(int year)
    {
        bool ok = year >= CivilScope.MinYear && year <= CivilScope.MaxYear;
        return ok ? UnsafeCreate(year) : null;
    }

    // Explicit implementation: CivilYear being a value type, better
    // to use the other TryCreate().
    [Pure]
    static bool IYear<CivilYear>.TryCreate(int year, out CivilYear result)
    {
        var yearValue = TryCreate(year);
        result = yearValue ?? default;
        return yearValue.HasValue;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CivilYear"/> struct
    /// from the specified year.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static CivilYear UnsafeCreate(int year) => new((ushort)(year - 1));
}

public partial struct CivilYear // IMonthSegment
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthCount = CivilSchema.MonthsPerYear;

    /// <inheritdoc />
    public CivilMonth MinMonth => CivilMonth.UnsafeCreate(Year, 1);

    /// <inheritdoc />
    public CivilMonth MaxMonth => CivilMonth.UnsafeCreate(Year, MonthCount);

    /// <inheritdoc />
    [Pure]
    int IMonthSegment<CivilMonth>.CountMonths() => MonthCount;

    /// <inheritdoc />
    [Pure]
    public Segment<CivilMonth> ToMonthRange() => Segment.StartingAt(MinMonth, MonthCount);

    /// <inheritdoc />
    [Pure]
    public IEnumerable<CivilMonth> EnumerateMonths()
    {
        int startOfYear = CivilMonth.UnsafeCreate(Year, 1).MonthsSinceEpoch;

        return from monthsSinceEpoch
               in Enumerable.Range(startOfYear, MonthCount)
               select CivilMonth.UnsafeCreate(monthsSinceEpoch);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the current year instance contains
    /// the specified month; otherwise returns <see langword="false"/>.
    /// </summary>
    [Pure]
    public bool Contains(CivilMonth month) => month.Year == Year;

    /// <summary>
    /// Obtains the month corresponding to the specified month of this year
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="month"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public CivilMonth GetMonthOfYear(int month)
    {
        // We already know that "y" is valid, we only need to check "month".
        // The calendar being regular, no need to use the Scope:
        // > Calendar.Scope.PreValidator.ValidateMonth(Year, month);
        if (month < 1 || month > CivilSchema.MonthsPerYear)
            ThrowHelpers.ThrowMonthOutOfRange(month);

        return CivilMonth.UnsafeCreate(Year, month);
    }
}

public partial struct CivilYear // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(CivilYear left, CivilYear right) =>
        left._yearsSinceEpoch == right._yearsSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(CivilYear left, CivilYear right) =>
        left._yearsSinceEpoch != right._yearsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(CivilYear other) => _yearsSinceEpoch == other._yearsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CivilYear year && Equals(year);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _yearsSinceEpoch;
}

public partial struct CivilYear // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(CivilYear left, CivilYear right) =>
        left._yearsSinceEpoch < right._yearsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(CivilYear left, CivilYear right) =>
        left._yearsSinceEpoch <= right._yearsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(CivilYear left, CivilYear right) =>
        left._yearsSinceEpoch > right._yearsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(CivilYear left, CivilYear right) =>
        left._yearsSinceEpoch >= right._yearsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static CivilYear Min(CivilYear x, CivilYear y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static CivilYear Max(CivilYear x, CivilYear y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(CivilYear other) =>
        _yearsSinceEpoch.CompareTo(other._yearsSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is CivilYear year ? CompareTo(year)
        : ThrowHelpers.ThrowNonComparable(typeof(CivilYear), obj);
}

public partial struct CivilYear // Math ops
{
    /// <summary>
    /// Subtracts the two specified years and returns the number of years between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountYearsSince()")]
    public static int operator -(CivilYear left, CivilYear right) => left.CountYearsSince(right);

    /// <summary>
    /// Adds a number of years to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported years.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusYears()")]
    public static CivilYear operator +(CivilYear value, int years) => value.PlusYears(years);

    /// <summary>
    /// Subtracts a number of years to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range
    /// of supported years.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusYears()")]
    public static CivilYear operator -(CivilYear value, int years) => value.PlusYears(-years);

    /// <summary>
    /// Adds one year to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported year.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextYear()")]
    public static CivilYear operator ++(CivilYear value) => value.NextYear();

    /// <summary>
    /// Subtracts one year to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported year.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousYear()")]
    public static CivilYear operator --(CivilYear value) => value.PreviousYear();

    /// <summary>
    /// Counts the number of whole years elapsed since the specified year.
    /// </summary>
    [Pure]
    public int CountYearsSince(CivilYear other) =>
        // No need to use a checked context here. Indeed, the absolute value of
        // the result is at most equal to (MaxYear - 1).
        _yearsSinceEpoch - other._yearsSinceEpoch;

    /// <summary>
    /// Adds a number of years to the current instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported years.
    /// </exception>
    [Pure]
    public CivilYear PlusYears(int years)
    {
        int yearsSinceEpoch = checked(_yearsSinceEpoch + years);
        if (unchecked((uint)yearsSinceEpoch) > MaxYearsSinceEpoch) ThrowHelpers.ThrowYearOverflow();
        return new CivilYear((ushort)yearsSinceEpoch);
    }

    /// <summary>
    /// Obtains the year after the current instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported year.</exception>
    [Pure]
    public CivilYear NextYear()
    {
        if (_yearsSinceEpoch == MaxYearsSinceEpoch) ThrowHelpers.ThrowYearOverflow();
        return new CivilYear((ushort)(_yearsSinceEpoch + 1));
    }

    /// <summary>
    /// Obtains the year before the current instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported year.</exception>
    [Pure]
    public CivilYear PreviousYear()
    {
        if (_yearsSinceEpoch == 0) ThrowHelpers.ThrowYearOverflow();
        return new CivilYear((ushort)(_yearsSinceEpoch - 1));
    }
}

#endregion
