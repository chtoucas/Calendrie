// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

// REVIEW(code): optimize ToRange...(). Interfaces. Idem with CivilMonth.

/// <summary>
/// Represents a Civil year.
/// <para><i>All</i> years within the range [1..9999] of years are supported.
/// </para>
/// <para><see cref="CivilYear"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct CivilYear :
    ICalendarYear<CivilYear>,
    ICalendarBound<CivilCalendar>,
    // A year viewed as a finite sequence of months
    IMonthSegment<CivilMonth>,
    ISetMembership<CivilMonth>,
    // A year viewed as a finite sequence of days
    IDaySegment<CivilDate>,
    ISetMembership<CivilDate>,
    // Arithmetic
    ISubtractionOperators<CivilYear, CivilYear, int>
{ }

public partial struct CivilYear // Preamble
{
    /// <summary>Represents the maximum value of <see cref="_year0"/>.
    /// <para>This field is a constant equal to 9998.</para></summary>
    private const int MaxYear0 = StandardScope.MaxYear - 1;

    /// <summary>
    /// Represents the count of consecutive years since the epoch
    /// <see cref="DayZero.NewStyle"/>.
    /// <para>This field is in the range from 0 to <see cref="MaxYear0"/>.
    /// </para>
    /// </summary>
    private readonly int _year0;

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilYear"/> struct to the
    /// specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of years supported values.</exception>
    public CivilYear(int year)
    {
        if (year < StandardScope.MinYear || year > StandardScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year);

        _year0 = year - 1;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilYear"/> struct to the
    /// specified year.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    internal CivilYear(int year, bool _)
    {
        _year0 = year - 1;
    }

    /// <summary>
    /// Gets the earliest possible value of a <see cref="CivilYear"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    //
    // MinValue = new(1) = new() = default(CivilYear)
    public static CivilYear MinValue { get; }

    /// <summary>
    /// Gets the latest possible value of a <see cref="CivilYear"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static CivilYear MaxValue { get; } = new(StandardScope.MaxYear);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static CivilCalendar Calendar => CivilCalendar.Instance;

    /// <summary>
    /// Gets the century of the era.
    /// </summary>
    public Ord CenturyOfEra => Ord.FromInt32(Century);

    /// <summary>
    /// Gets the century number.
    /// </summary>
    public int Century => YearNumbering.GetCentury(Year);

    /// <summary>
    /// Gets the year of the era.
    /// </summary>
    public Ord YearOfEra => Ord.FromInt32(Year);

    /// <summary>
    /// Gets the year of the century.
    /// <para>The result is in the range from 1 to 100.</para>
    /// </summary>
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

    /// <summary>
    /// Gets the year number.
    /// <para>This property represents the algebraic year, but since it's greater
    /// than 0, there is no difference between the algebraic year and the year
    /// of the era.</para>
    /// </summary>
    public int Year => _year0 + 1;

    /// <inheritdoc />
    public bool IsLeap => GregorianFormulae.IsLeapYear(Year);

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString() => FormattableString.Invariant($"{Year:D4} ({Calendar})");

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CountMonthsSinceZero(int y, int m) =>
        // See RegularSchema.CountMonthsSinceEpoch().
        CivilCalendar.MonthsInYear * (y - 1) + m - 1;
}

public partial struct CivilYear // IMonthSegment
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsCount = CivilCalendar.MonthsInYear;

    /// <inheritdoc />
    public CivilMonth MinMonth
    {
        get
        {
            int monthsSinceZero = CountMonthsSinceZero(Year, 1);
            return new CivilMonth(monthsSinceZero);
        }
    }

    /// <inheritdoc />
    public CivilMonth MaxMonth
    {
        get
        {
            int y = Year;
            int monthsSinceZero = CountMonthsSinceZero(y, MonthsCount);
            return new CivilMonth(monthsSinceZero);
        }
    }

    /// <inheritdoc />
    [Pure]
    int IMonthSegment<CivilMonth>.CountMonths() => MonthsCount;

    /// <inheritdoc />
    [Pure]
    public Range<CivilMonth> ToMonthRange() => Range.UnsafeCreate(MinMonth, MaxMonth);

    /// <inheritdoc />
    [Pure]
    public IEnumerable<CivilMonth> EnumerateMonths()
    {
        int startOfYear = CountMonthsSinceZero(Year, 1);

        return from monthsSinceZero
               in Enumerable.Range(startOfYear, MonthsCount)
               select new CivilMonth(monthsSinceZero);
    }

    /// <inheritdoc />
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
        int y = Year;
        Calendar.Scope.PreValidator.ValidateMonth(y, month);
        int monthsSinceZero = CountMonthsSinceZero(y, month);
        return new CivilMonth(monthsSinceZero);
    }
}

public partial struct CivilYear // IDaySegment
{
    /// <inheritdoc />
    public CivilDate MinDay
    {
        get
        {
            int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(Year, 1);
            return new CivilDate(daysSinceZero);
        }
    }

    /// <inheritdoc />
    public CivilDate MaxDay
    {
        get
        {
            int y = Year;
            int doy = GregorianFormulae.CountDaysInYear(y);
            int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(y, doy);
            return new CivilDate(daysSinceZero);
        }
    }

    /// <inheritdoc />
    /// <remarks>See also <see cref="CalendarSystem{TDate}.CountDaysInYear(int)"/>.
    /// </remarks>
    [Pure]
    public int CountDays() => GregorianFormulae.CountDaysInYear(Year);

    /// <inheritdoc />
    /// <remarks>See also <see cref="CalendarSystem{TDate}.GetDaysInYear(int)"/>.
    /// </remarks>
    [Pure]
    public Range<CivilDate> ToDayRange() => Range.UnsafeCreate(MinDay, MaxDay);

    /// <inheritdoc />
    [Pure]
    public IEnumerable<CivilDate> EnumerateDays()
    {
        int y = Year;
        int startOfYear = GregorianFormulae.CountDaysSinceEpoch(y, 1);
        int daysInYear = GregorianFormulae.CountDaysInYear(y);

        return from daysSinceZero
               in Enumerable.Range(startOfYear, daysInYear)
               select new CivilDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public bool Contains(CivilDate date) => date.Year == Year;

    /// <summary>
    /// Obtains the ordinal date corresponding to the specified day of this year
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfYear"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public CivilDate GetDayOfYear(int dayOfYear)
    {
        int y = Year;
        // We already know that "y" is valid, we only need to check "dayOfYear".
        Calendar.Scope.PreValidator.ValidateDayOfYear(y, dayOfYear);
        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(y, dayOfYear);
        return new CivilDate(daysSinceZero);
    }
}

public partial struct CivilYear // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(CivilYear left, CivilYear right) => left._year0 == right._year0;

    /// <inheritdoc />
    public static bool operator !=(CivilYear left, CivilYear right) => left._year0 != right._year0;

    /// <inheritdoc />
    [Pure]
    public bool Equals(CivilYear other) => _year0 == other._year0;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CivilYear year && Equals(year);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _year0;
}

public partial struct CivilYear // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(CivilYear left, CivilYear right) => left._year0 < right._year0;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(CivilYear left, CivilYear right) => left._year0 <= right._year0;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(CivilYear left, CivilYear right) => left._year0 > right._year0;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(CivilYear left, CivilYear right) => left._year0 >= right._year0;

    /// <inheritdoc />
    [Pure]
    public static CivilYear Min(CivilYear x, CivilYear y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static CivilYear Max(CivilYear x, CivilYear y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(CivilYear other) => _year0.CompareTo(other._year0);

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
    /// Counts the number of years elapsed since the specified year.
    /// </summary>
    [Pure]
    public int CountYearsSince(CivilYear other) =>
        // No need to use a checked context here. Indeed, the absolute value of
        // the result is at most equal to (MaxYear - 1).
        _year0 - other._year0;

    /// <summary>
    /// Adds a number of years to the current instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported years.
    /// </exception>
    [Pure]
    public CivilYear PlusYears(int years)
    {
        int y0 = checked(_year0 + years);
        if (unchecked((uint)y0) > MaxYear0) ThrowHelpers.ThrowYearOverflow();
        // NB: we know that (y0 + 1) does NOT overflow.
        return new CivilYear(y0 + 1, true);
    }

    /// <summary>
    /// Obtains the year after the current instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported year.</exception>
    [Pure]
    public CivilYear NextYear()
    {
        if (_year0 == MaxYear0) ThrowHelpers.ThrowYearOverflow();
        return new(Year + 1, true);
    }

    /// <summary>
    /// Obtains the year before the current instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported year.</exception>
    [Pure]
    public CivilYear PreviousYear()
    {
        if (_year0 == 0) ThrowHelpers.ThrowYearOverflow();
        return new(Year - 1, true);
    }
}
