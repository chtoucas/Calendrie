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

using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

#region GregorianDate

public partial struct GregorianDate // Counting
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
    /// <summary>
    /// Compares the two specified dates to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero < right._daysSinceZero;

    /// <summary>
    /// Compares the two specified dates to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero <= right._daysSinceZero;

    /// <summary>
    /// Compares the two specified dates to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero > right._daysSinceZero;

    /// <summary>
    /// Compares the two specified dates to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
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

#endregion

#region GregorianMonth

public partial struct GregorianMonth // Factories
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
        // The calendar being regular, no need to use the PreValidator.
        if (year < GregorianScope.MinYear || year > GregorianScope.MaxYear
            || month < 1 || month > GregorianSchema.MonthsInYear)
        {
            return null;
        }

        return UnsafeCreate(year, month);
    }

    // Explicit implementation: GregorianMonth being a value type, better
    // to use the other TryCreate().
    [Pure]
    static bool IMonth<GregorianMonth>.TryCreate(int year, int month, out GregorianMonth result)
    {
        var monthValue = TryCreate(year, month);
        result = monthValue ?? default;
        return monthValue.HasValue;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="GregorianMonth"/> struct
    /// from the specified month components.
    /// <para>This method does NOT validate its parameters.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static GregorianMonth UnsafeCreate(int year, int month)
    {
        int monthsSinceEpoch = CountMonthsSinceEpoch(year, month);
        return new GregorianMonth(monthsSinceEpoch);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="GregorianMonth"/> struct
    /// from the specified count of consecutive months since the epoch.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static GregorianMonth UnsafeCreate(int monthsSinceEpoch) => new(monthsSinceEpoch);

    [Pure]
    static GregorianMonth IUnsafeFactory<GregorianMonth>.UnsafeCreate(int monthsSinceEpoch) =>
        UnsafeCreate(monthsSinceEpoch);

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CountMonthsSinceEpoch(int y, int m) =>
        // See RegularSchema.CountMonthsSinceEpoch().
        GregorianSchema.MonthsInYear * (y - 1) + m - 1;
}

public partial struct GregorianMonth // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedMonthsInYear() => Month - 1;

    /// <inheritdoc />
    [Pure]
    public int CountRemainingMonthsInYear() => GregorianSchema.MonthsInYear - Month;

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

public partial struct GregorianMonth // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public GregorianMonth WithYear(int newYear)
    {
        int m = Month;

        // Even when "newYear" is valid, we should re-check "m", but the calendar
        // being regular this is not needed here.
        // The calendar being regular, no need to use the Scope:
        // > Calendar.Scope.ValidateYearMonth(newYear, m, nameof(newYear));
        if (newYear < GregorianScope.MinYear || newYear > GregorianScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(newYear, nameof(newYear));

        return UnsafeCreate(newYear, m);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianMonth WithMonth(int newMonth)
    {
        int y = Year;

        // We already know that "y" is valid, we only need to check "newMonth".
        // The calendar being regular, no need to use the Scope:
        // > Calendar.Scope.PreValidator.ValidateMonth(y, newMonth, nameof(newMonth));
        if (newMonth < 1 || newMonth > GregorianSchema.MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(newMonth, nameof(newMonth));

        return UnsafeCreate(y, newMonth);
    }
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
    /// Counts the number of whole years elapsed since the specified month.
    /// </summary>
    [Pure]
    public int CountYearsSince(GregorianMonth other)
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

#region GregorianYear

public partial struct GregorianYear // Conversions
{
    /// <summary>
    /// Creates a new instance of the <see cref="GregorianYear"/> struct
    /// from the specified <see cref="GregorianMonth"/> value.
    /// </summary>
    [Pure]
    public static GregorianYear FromMonth(GregorianMonth month) => UnsafeCreate(month.Year);

    /// <summary>
    /// Creates a new instance of the <see cref="GregorianYear"/> struct
    /// from the specified <see cref="GregorianDate"/> value.
    /// </summary>
    [Pure]
    public static GregorianYear FromDate(GregorianDate date) => UnsafeCreate(date.Year);
}

public partial struct GregorianYear // IMonthSegment
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthCount = GregorianSchema.MonthsInYear;

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
               select GregorianMonth.UnsafeCreate(monthsSinceEpoch);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the current year instance contains
    /// the specified month; otherwise returns <see langword="false"/>.
    /// </summary>
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
        // The calendar being regular, no need to use the Scope:
        // > Calendar.Scope.PreValidator.ValidateMonth(Year, month);
        if (month < 1 || month > GregorianSchema.MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(month);

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
