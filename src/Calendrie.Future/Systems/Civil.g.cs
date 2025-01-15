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
            || month < 1 || month > CivilSchema.MonthsInYear)
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
        CivilSchema.MonthsInYear * (y - 1) + m - 1;
}

public partial struct CivilMonth // Conversions
{
    /// <summary>
    /// Creates a new instance of the <see cref="CivilMonth"/> struct
    /// from the specified number of consecutive months since the epoch.
    /// </summary>
    [Pure]
    public static CivilMonth FromMonthsSinceEpoch(int monthsSinceEpoch)
    {
        if (unchecked((uint)monthsSinceEpoch) > MaxMonthsSinceEpoch)
            ThrowHelpers.ThrowMonthsSinceEpochOutOfRange(monthsSinceEpoch);
        return new CivilMonth(monthsSinceEpoch);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CivilMonth"/> struct
    /// from the specified <see cref="CivilDate"/> value.
    /// </summary>
    [Pure]
    public static CivilMonth FromDate(CivilDate date)
    {
        var (y, m, _) = date;
        return UnsafeCreate(y, m);
    }
}

public partial struct CivilMonth // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedMonthsInYear() => Month - 1;

    /// <inheritdoc />
    [Pure]
    public int CountRemainingMonthsInYear() => CivilSchema.MonthsInYear - 1;

#if false
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
#endif
}

public partial struct CivilMonth // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public CivilMonth WithYear(int newYear)
    {
        int m = Month;

        // Even when "newYear" is valid, we must re-check "m".
        // The calendar being regular, no need to use the Scope:
        // > Calendar.Scope.ValidateYearMonth(newYear, m, nameof(newYear));
        if (newYear < CivilScope.MinYear || newYear > CivilScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(newYear);
        if (m < 1 || m > CivilSchema.MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(m, nameof(newYear));

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
        if (newMonth < 1 || newMonth > CivilSchema.MonthsInYear)
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
    /// Adds a number of years to the year field of this month instance, yielding
    /// a new month.
    /// <para>In the particular case of the Civil calendar, this
    /// operation is exact.</para>
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

    /// <summary>
    /// Counts the number of whole years elapsed since the specified month.
    /// <para>In the particular case of the Civil calendar, this
    /// operation is exact.</para>
    /// </summary>
    [Pure]
    public int CountYearsSince(CivilMonth other) =>
        // NB: this subtraction never overflows.
        Year - other.Year;
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

public partial struct CivilYear // Conversions
{
    /// <summary>
    /// Creates a new instance of the <see cref="CivilYear"/> struct
    /// from the specified <see cref="CivilMonth"/> value.
    /// </summary>
    [Pure]
    public static CivilYear FromMonth(CivilMonth month) => UnsafeCreate(month.Year);

    /// <summary>
    /// Creates a new instance of the <see cref="CivilYear"/> struct
    /// from the specified <see cref="CivilDate"/> value.
    /// </summary>
    [Pure]
    public static CivilYear FromDate(CivilDate date) => UnsafeCreate(date.Year);
}

public partial struct CivilYear // IMonthSegment
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthCount = CivilSchema.MonthsInYear;

    /// <inheritdoc />
    public CivilMonth MinMonth => CivilMonth.UnsafeCreate(Year, 1);

    /// <inheritdoc />
    public CivilMonth MaxMonth => CivilMonth.UnsafeCreate(Year, MonthCount);

    /// <inheritdoc />
    [Pure]
    int IMonthSegment<CivilMonth>.CountMonths() => MonthCount;

    /// <inheritdoc />
    [Pure]
    public Range<CivilMonth> ToMonthRange() => Range.StartingAt(MinMonth, MonthCount);

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
        if (month < 1 || month > CivilSchema.MonthsInYear)
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
