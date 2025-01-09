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

#region JulianMonth

public partial struct JulianMonth // Factories & conversions
{
    /// <inheritdoc />
    [Pure]
    public static JulianMonth Create(int year, int month) => new(year, month);

    /// <summary>
    /// Attempts to create a new instance of the <see cref="JulianMonth"/>
    /// struct from the specified month components.
    /// </summary>
    [Pure]
    public static JulianMonth? TryCreate(int year, int month)
    {
        // The calendar being regular, no need to use the PreValidator.
        if (year < JulianScope.MinYear || year > JulianScope.MaxYear
            || month < 1 || month > JulianCalendar.MonthsInYear)
        {
            return null;
        }

        return UnsafeCreate(year, month);
    }

    // Explicit implementation: JulianMonth being a value type, better
    // to use the other TryCreate().
    [Pure]
    static bool IMonth<JulianMonth>.TryCreate(int year, int month, out JulianMonth result)
    {
        var monthValue = TryCreate(year, month);
        result = monthValue ?? default;
        return monthValue.HasValue;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="JulianMonth"/> struct
    /// from the specified month components.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static JulianMonth UnsafeCreate(int year, int month)
    {
        int monthsSinceEpoch = CountMonthsSinceEpoch(year, month);
        return new JulianMonth(monthsSinceEpoch);
    }

    [Pure]
    static JulianMonth IUnsafeFactory<JulianMonth>.UnsafeCreate(int monthsSinceEpoch) =>
        new(monthsSinceEpoch);

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CountMonthsSinceEpoch(int y, int m) =>
        // See RegularSchema.CountMonthsSinceEpoch().
        JulianCalendar.MonthsInYear * (y - 1) + m - 1;

    //
    // Conversions
    //

    /// <summary>
    /// Creates a new instance of the <see cref="JulianMonth"/> struct
    /// from the specified number of consecutive months since the epoch.
    /// </summary>
    [Pure]
    public static JulianMonth FromMonthsSinceEpoch(int monthsSinceEpoch)
    {
        if (unchecked((uint)monthsSinceEpoch) > MaxMonthsSinceEpoch)
            ThrowHelpers.ThrowMonthsSinceEpochOutOfRange(monthsSinceEpoch);
        return new JulianMonth(monthsSinceEpoch);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="JulianMonth"/> struct
    /// from the specified <see cref="JulianDate"/> value.
    /// </summary>
    [Pure]
    public static JulianMonth FromDate(JulianDate date)
    {
        var (y, m, _) = date;
        return UnsafeCreate(y, m);
    }
}

public partial struct JulianMonth // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedMonthsInYear() => Month - 1;

    /// <inheritdoc />
    [Pure]
    public int CountRemainingMonthsInYear() => JulianCalendar.MonthsInYear - 1;

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

public partial struct JulianMonth // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public JulianMonth WithYear(int newYear)
    {
        int m = Month;

        // Even when "newYear" is valid, we must re-check "m".
        // The calendar being regular, no need to use the Scope:
        // > Calendar.Scope.ValidateYearMonth(newYear, m, nameof(newYear));
        if (newYear < JulianScope.MinYear || newYear > JulianScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(newYear);
        if (m < 1 || m > JulianCalendar.MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(m, nameof(newYear));

        return UnsafeCreate(newYear, m);
    }

    /// <inheritdoc />
    [Pure]
    public JulianMonth WithMonth(int newMonth)
    {
        int y = Year;

        // We already know that "y" is valid, we only need to check "newMonth".
        // The calendar being regular, no need to use the Scope:
        // > Calendar.Scope.PreValidator.ValidateMonth(y, newMonth, nameof(newMonth));
        if (newMonth < 1 || newMonth > JulianCalendar.MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(newMonth, nameof(newMonth));

        return UnsafeCreate(y, newMonth);
    }
}

public partial struct JulianMonth // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(JulianMonth left, JulianMonth right) =>
        left._monthsSinceEpoch == right._monthsSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(JulianMonth left, JulianMonth right) =>
        left._monthsSinceEpoch != right._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(JulianMonth other) => _monthsSinceEpoch == other._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is JulianMonth month && Equals(month);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _monthsSinceEpoch;
}

public partial struct JulianMonth // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(JulianMonth left, JulianMonth right) =>
        left._monthsSinceEpoch < right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(JulianMonth left, JulianMonth right) =>
        left._monthsSinceEpoch <= right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(JulianMonth left, JulianMonth right) =>
        left._monthsSinceEpoch > right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(JulianMonth left, JulianMonth right) =>
        left._monthsSinceEpoch >= right._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static JulianMonth Min(JulianMonth x, JulianMonth y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static JulianMonth Max(JulianMonth x, JulianMonth y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(JulianMonth other) => _monthsSinceEpoch.CompareTo(other._monthsSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is JulianMonth month ? CompareTo(month)
        : ThrowHelpers.ThrowNonComparable(typeof(JulianMonth), obj);
}

public partial struct JulianMonth // Non-standard math ops
{
    /// <summary>
    /// Adds a number of years to the year field of this month instance, yielding
    /// a new month.
    /// <para>In the particular case of the Julian calendar, this
    /// operation is exact.</para>
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported months.</exception>
    [Pure]
    public JulianMonth PlusYears(int years)
    {
        var (y, m) = this;
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < JulianScope.MinYear || newY > JulianScope.MaxYear)
            ThrowHelpers.ThrowMonthOverflow();

        return UnsafeCreate(newY, m);
    }

    /// <summary>
    /// Counts the number of years elapsed since the specified month.
    /// <para>In the particular case of the Julian calendar, this
    /// operation is exact.</para>
    /// </summary>
    [Pure]
    public int CountYearsSince(JulianMonth other) =>
        // NB: this subtraction never overflows.
        Year - other.Year;
}

#endregion

#region JulianYear

public partial struct JulianYear // IMonthSegment
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthCount = JulianCalendar.MonthsInYear;

    /// <inheritdoc />
    public JulianMonth MinMonth => JulianMonth.UnsafeCreate(Year, 1);

    /// <inheritdoc />
    public JulianMonth MaxMonth => JulianMonth.UnsafeCreate(Year, MonthCount);

    /// <inheritdoc />
    [Pure]
    int IMonthSegment<JulianMonth>.CountMonths() => MonthCount;

    /// <inheritdoc />
    [Pure]
    public Range<JulianMonth> ToMonthRange() => Range.StartingAt(MinMonth, MonthCount);

    /// <inheritdoc />
    [Pure]
    public IEnumerable<JulianMonth> EnumerateMonths()
    {
        int startOfYear = JulianMonth.UnsafeCreate(Year, 1).MonthsSinceEpoch;

        return from monthsSinceEpoch
               in Enumerable.Range(startOfYear, MonthCount)
               select new JulianMonth(monthsSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public bool Contains(JulianMonth month) => month.Year == Year;

    /// <summary>
    /// Obtains the month corresponding to the specified month of this year
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="month"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public JulianMonth GetMonthOfYear(int month)
    {
        // We already know that "y" is valid, we only need to check "month".
        // The calendar being regular, no need to use the Scope:
        // > Calendar.Scope.PreValidator.ValidateMonth(Year, month);
        if (month < 1 || month > GregorianCalendar.MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(month);

        return JulianMonth.UnsafeCreate(Year, month);
    }
}

public partial struct JulianYear // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(JulianYear left, JulianYear right) =>
        left._yearsSinceEpoch == right._yearsSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(JulianYear left, JulianYear right) =>
        left._yearsSinceEpoch != right._yearsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(JulianYear other) => _yearsSinceEpoch == other._yearsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is JulianYear year && Equals(year);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _yearsSinceEpoch;
}

public partial struct JulianYear // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(JulianYear left, JulianYear right) =>
        left._yearsSinceEpoch < right._yearsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(JulianYear left, JulianYear right) =>
        left._yearsSinceEpoch <= right._yearsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(JulianYear left, JulianYear right) =>
        left._yearsSinceEpoch > right._yearsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(JulianYear left, JulianYear right) =>
        left._yearsSinceEpoch >= right._yearsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static JulianYear Min(JulianYear x, JulianYear y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static JulianYear Max(JulianYear x, JulianYear y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(JulianYear other) =>
        _yearsSinceEpoch.CompareTo(other._yearsSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is JulianYear year ? CompareTo(year)
        : ThrowHelpers.ThrowNonComparable(typeof(JulianYear), obj);
}

#endregion
