﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Calendrie.Systems;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

#region GregorianMonth

public partial struct GregorianMonth // Factories & conversions
{
    /// <inheritdoc />
    [Pure]
    public static GregorianMonth Create(int year, int month) => new(year, month);

    /// <summary>
    /// Attempts to create a new instance of the <see cref="GregorianMonth"/>
    /// struct from the specified month components.
    /// </summary>
    [Pure]
    public static GregorianMonth? TryCreate(int year, int month)
    {
        bool ok = year >= GregorianScope.MinYear && year <= GregorianScope.MaxYear
            && month >= 1 && month <= GregorianCalendar.MonthsInYear;

        return ok ? UnsafeCreate(year, month) : null;
    }

    [Pure]
    static bool IMonth<GregorianMonth>.TryCreate(int year, int month, out GregorianMonth result)
    {
        bool ok = year >= GregorianScope.MinYear && year <= GregorianScope.MaxYear
            && month >= 1 && month <= GregorianCalendar.MonthsInYear;

        result = ok ? UnsafeCreate(year, month) : default;
        return ok;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="GregorianMonth"/> struct
    /// from the specified month components.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static GregorianMonth UnsafeCreate(int year, int month)
    {
        int monthsSinceEpoch = CountMonthsSinceEpoch(year, month);
        return new GregorianMonth(monthsSinceEpoch);
    }

    [Pure]
    static GregorianMonth IUnsafeFactory<GregorianMonth>.UnsafeCreate(int monthsSinceEpoch) =>
        new(monthsSinceEpoch);

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CountMonthsSinceEpoch(int y, int m) =>
        // See RegularSchema.CountMonthsSinceEpoch().
        GregorianCalendar.MonthsInYear * (y - 1) + m - 1;

    //
    // Conversions
    //

    /// <summary>
    /// Creates a new instance of the <see cref="GregorianMonth"/> struct
    /// from the specified <see cref="GregorianDate"/> value.
    /// </summary>
    [Pure]
    public static GregorianMonth FromDate(GregorianDate date)
    {
        var (y, m, _) = date;
        return UnsafeCreate(y, m);
    }
}

public partial struct GregorianMonth // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedMonthsInYear() => Month - 1;

    /// <inheritdoc />
    [Pure]
    public int CountRemainingMonthsInYear() => GregorianCalendar.MonthsInYear - 1;

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

public partial struct GregorianMonth // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(GregorianMonth left, GregorianMonth right) =>
        left._monthsSinceEpoch == right._monthsSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(GregorianMonth left, GregorianMonth right) =>
        left._monthsSinceEpoch != right._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(GregorianMonth other) => _monthsSinceEpoch == other._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is GregorianMonth month && Equals(month);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _monthsSinceEpoch;
}

public partial struct GregorianMonth // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(GregorianMonth left, GregorianMonth right) =>
        left._monthsSinceEpoch < right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(GregorianMonth left, GregorianMonth right) =>
        left._monthsSinceEpoch <= right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(GregorianMonth left, GregorianMonth right) =>
        left._monthsSinceEpoch > right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(GregorianMonth left, GregorianMonth right) =>
        left._monthsSinceEpoch >= right._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static GregorianMonth Min(GregorianMonth x, GregorianMonth y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static GregorianMonth Max(GregorianMonth x, GregorianMonth y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(GregorianMonth other) => _monthsSinceEpoch.CompareTo(other._monthsSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is GregorianMonth month ? CompareTo(month)
        : ThrowHelpers.ThrowNonComparable(typeof(GregorianMonth), obj);
}

public partial struct GregorianMonth // Non-standard math ops
{
    /// <summary>
    /// Adds a number of years to the year field of this month instance, yielding
    /// a new month.
    /// <para>In the particular case of the Gregorian calendar, this
    /// operation is exact.</para>
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported months.</exception>
    [Pure]
    public GregorianMonth PlusYears(int years)
    {
        var (y, m) = this;
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < GregorianScope.MinYear || newY > GregorianScope.MaxYear)
            ThrowHelpers.ThrowMonthOverflow();

        return UnsafeCreate(newY, m);
    }

    /// <summary>
    /// Counts the number of years elapsed since the specified month.
    /// <para>In the particular case of the Gregorian calendar, this
    /// operation is exact.</para>
    /// </summary>
    [Pure]
    public int CountYearsSince(GregorianMonth other) =>
        // NB: this subtraction never overflows.
        Year - other.Year;
}

#endregion

#region GregorianYear

public partial struct GregorianYear // IMonthSegment
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthCount = GregorianCalendar.MonthsInYear;

    /// <inheritdoc />
    public GregorianMonth MinMonth => GregorianMonth.UnsafeCreate(Year, 1);

    /// <inheritdoc />
    public GregorianMonth MaxMonth => GregorianMonth.UnsafeCreate(Year, MonthCount);

    /// <inheritdoc />
    [Pure]
    int IMonthSegment<GregorianMonth>.CountMonths() => MonthCount;

    /// <inheritdoc />
    [Pure]
    public Range<GregorianMonth> ToMonthRange() => Range.StartingAt(MinMonth, MonthCount);

    /// <inheritdoc />
    [Pure]
    public IEnumerable<GregorianMonth> EnumerateMonths()
    {
        int startOfYear = GregorianMonth.UnsafeCreate(Year, 1).MonthsSinceEpoch;

        return from monthsSinceEpoch
               in Enumerable.Range(startOfYear, MonthCount)
               select new GregorianMonth(monthsSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public bool Contains(GregorianMonth month) => month.Year == Year;

    /// <summary>
    /// Obtains the month corresponding to the specified month of this year
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="month"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public GregorianMonth GetMonthOfYear(int month)
    {
        // We already know that "y" is valid, we only need to check "month".
        Calendar.Scope.PreValidator.ValidateMonth(Year, month);
        return GregorianMonth.UnsafeCreate(Year, month);
    }
}

public partial struct GregorianYear // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(GregorianYear left, GregorianYear right) =>
        left._yearsSinceEpoch == right._yearsSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(GregorianYear left, GregorianYear right) =>
        left._yearsSinceEpoch != right._yearsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(GregorianYear other) => _yearsSinceEpoch == other._yearsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is GregorianYear year && Equals(year);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _yearsSinceEpoch;
}

public partial struct GregorianYear // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(GregorianYear left, GregorianYear right) =>
        left._yearsSinceEpoch < right._yearsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(GregorianYear left, GregorianYear right) =>
        left._yearsSinceEpoch <= right._yearsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(GregorianYear left, GregorianYear right) =>
        left._yearsSinceEpoch > right._yearsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(GregorianYear left, GregorianYear right) =>
        left._yearsSinceEpoch >= right._yearsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static GregorianYear Min(GregorianYear x, GregorianYear y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static GregorianYear Max(GregorianYear x, GregorianYear y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(GregorianYear other) =>
        _yearsSinceEpoch.CompareTo(other._yearsSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is GregorianYear year ? CompareTo(year)
        : ThrowHelpers.ThrowNonComparable(typeof(GregorianYear), obj);
}

#endregion
