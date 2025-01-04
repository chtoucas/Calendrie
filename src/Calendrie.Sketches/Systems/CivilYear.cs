// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

// REVIEW(code): IEnumerable<CivilDate> or IEnumerable<CivilMonth>? Idem with CivilMonth.
// Optimize ToRange...(). Idem with CivilMonth.

/// <summary>
/// Represents a Civil year.
/// <para><i>All</i> years within the range [1..9999] of years are supported.
/// </para>
/// <para><see cref="CivilYear"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct CivilYear :
    // Comparison
    IEqualityOperators<CivilYear, CivilYear, bool>,
    IEquatable<CivilYear>,
    IComparisonOperators<CivilYear, CivilYear, bool>,
    IComparable<CivilYear>,
    IComparable,
    IMinMaxValue<CivilYear>,
    // Arithmetic
    IAdditionOperators<CivilYear, int, CivilYear>,
    ISubtractionOperators<CivilYear, int, CivilYear>,
    ISubtractionOperators<CivilYear, CivilYear, int>,
    IIncrementOperators<CivilYear>,
    IDecrementOperators<CivilYear>
{ }

public partial struct CivilYear // Preamble
{
    /// <summary>Represents the maximum value of <see cref="_year0"/>.
    /// <para>This field is a constant equal to 9998.</para></summary>
    private const int MaxYear0 = StandardScope.MaxYear - 1;

    /// <summary>
    /// Represents the count of consecutive years since <see cref="DayZero.NewStyle"/>.
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

    /// <summary>
    /// Returns <see langword="true"/> if the current instance is a leap year;
    /// otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsLeap => Calendar.Schema.IsLeapYear(Year);

    /// <summary>
    /// Gets the first month of this year instance.
    /// </summary>
    public CivilMonth FirstMonth
    {
        get
        {
            int monthsSinceZero = Calendar.Schema.CountMonthsSinceEpoch(Year, 1);
            return new CivilMonth(monthsSinceZero);
        }
    }

    /// <summary>
    /// Gets the last month of this year instance.
    /// </summary>
    public CivilMonth LastMonth
    {
        get
        {
            var sch = Calendar.Schema;
            int y = Year;
            //int m = sch.CountMonthsInYear(y);
            //int monthsSinceZero = sch.CountMonthsSinceEpoch(y, m);
            int monthsSinceZero = sch.CountMonthsSinceEpoch(y, MonthsCount);
            return new CivilMonth(monthsSinceZero);
        }
    }

    /// <summary>
    /// Gets the first day of this year instance.
    /// </summary>
    public CivilDate FirstDay
    {
        get
        {
            int daysSinceZero = Calendar.Schema.CountDaysSinceEpoch(Year, 1);
            return new CivilDate(daysSinceZero);
        }
    }

    /// <summary>
    /// Gets the last day of this year instance.
    /// </summary>
    public CivilDate LastDay
    {
        get
        {
            var sch = Calendar.Schema;
            int y = Year;
            int doy = sch.CountDaysInYear(y);
            int daysSinceZero = Calendar.Schema.CountDaysSinceEpoch(y, doy);
            return new CivilDate(daysSinceZero);
        }
    }

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString() => FormattableString.Invariant($"{Year:D4} ({Calendar})");
}

public partial struct CivilYear // Factories & conversions
{
    /// <summary>
    /// Converts the current instance to a range of days.
    /// <para>See also <see cref="CalendarSystem{TDate}.GetDaysInYear(int)"/>.
    /// </para>
    /// </summary>
    [Pure]
    public Range<CivilDate> ToRangeOfDays() => Range.UnsafeCreate(FirstDay, LastDay);

    /// <summary>
    /// Converts the current instance to a range of months.
    /// </summary>
    [Pure]
    public Range<CivilMonth> ToRangeOfMonths() => Range.UnsafeCreate(FirstMonth, LastMonth);
}

public partial struct CivilYear // Counting
{
    // TODO(code): CountMonths() and MonthsInYear.

    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsCount = CivilCalendar.MonthsInYear;

    /// <summary>
    /// Obtains the number of months in this year instance.
    /// </summary>
    [Pure]
    public int CountMonths() => Calendar.Schema.CountMonthsInYear(Year);

    /// <summary>
    /// Obtains the number of days in this year instance.
    /// <para>See also <see cref="CalendarSystem{TDate}.CountDaysInYear(int)"/>.
    /// </para>
    /// </summary>
    [Pure]
    public int CountDays() => Calendar.Schema.CountDaysInYear(Year);
}

public partial struct CivilYear // Days within the year & "membership"
{
    /// <summary>
    /// Obtains the month corresponding to the specified month of this year
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="month"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public CivilMonth GetMonthOfYear(int month)
    {
        var chr = Calendar;
        // We already know that "y" is valid, we only need to check "month".
        int y = Year;
        chr.Scope.PreValidator.ValidateMonth(y, month);
        int monthsSinceZero = chr.Schema.CountMonthsSinceEpoch(y, month);
        return new CivilMonth(monthsSinceZero);
    }

    /// <summary>
    /// Obtains the sequence of all months in this year instance.
    /// </summary>
    [Pure]
    public IEnumerable<CivilMonth> GetAllMonths()
    {
        var sch = Calendar.Schema;
        int y = Year;
        int startOfYear = sch.CountMonthsSinceEpoch(y, 1);
        //int monthsInYear = sch.CountMonthsInYear(y);

        return from monthsSinceZero
               //in Enumerable.Range(startOfYear, monthsInYear)
               in Enumerable.Range(startOfYear, MonthsCount)
               select new CivilMonth(monthsSinceZero);
    }

    /// <summary>
    /// Obtains the ordinal date corresponding to the specified day of this year
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfYear"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public CivilDate GetDayOfYear(int dayOfYear)
    {
        var chr = Calendar;
        int y = Year;
        // We already know that "y" is valid, we only need to check "dayOfYear".
        chr.Scope.PreValidator.ValidateDayOfYear(y, dayOfYear);
        int daysSinceZero = Calendar.Schema.CountDaysSinceEpoch(y, dayOfYear);
        return new CivilDate(daysSinceZero);
    }

    /// <summary>
    /// Obtains the sequence of all days in this year instance.
    /// </summary>
    [Pure]
    public IEnumerable<CivilDate> GetAllDays()
    {
        var sch = Calendar.Schema;
        int y = Year;
        int startOfYear = sch.CountDaysSinceEpoch(y, 1);
        int daysInYear = sch.CountDaysInYear(y);

        return from daysSinceZero
               in Enumerable.Range(startOfYear, daysInYear)
               select new CivilDate(daysSinceZero);
    }

    //
    // "Membership"
    //

    /// <summary>
    /// Determines whether the current instance contains the specified month or
    /// not.
    /// </summary>
    [Pure]
    public bool Contains(CivilMonth month) => month.Year == Year;

    /// <summary>
    /// Determines whether the current instance contains the specified date or
    /// not.
    /// </summary>
    [Pure]
    public bool Contains(CivilDate date) => date.Year == Year;
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

    /// <summary>
    /// Obtains the earliest year between the two specified years.
    /// </summary>
    [Pure]
    public static CivilYear Min(CivilYear x, CivilYear y) => x < y ? x : y;

    /// <summary>
    /// Obtains the latest year between the two specified years.
    /// </summary>
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

public partial struct CivilYear // Standard math ops
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
    /// Adds a number of years to this year instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported years.</exception>
    [Pure]
    public CivilYear PlusYears(int years)
    {
        int y0 = checked(_year0 + years);
        if (unchecked((uint)y0) > MaxYear0) ThrowHelpers.ThrowYearOverflow();
        // NB: we know that (y0 + 1) does NOT overflow.
        return new CivilYear(y0 + 1, true);
    }

    /// <summary>
    /// Obtains the year after this year instance, yielding a new year.
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
    /// Obtains the year before this year instance, yielding a new year.
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
