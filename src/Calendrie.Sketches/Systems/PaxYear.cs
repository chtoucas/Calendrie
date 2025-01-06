// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Represents a Pax year.
/// <para><i>All</i> years within the range [1..9999] of years are supported.
/// </para>
/// <para><see cref="PaxYear"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct PaxYear :
    ICalendarYear<PaxYear>,
    ICalendarBound<PaxCalendar>,
    // A year viewed as a finite sequence of months
    IMonthSegment<PaxMonth>,
    ISetMembership<PaxMonth>,
    // A year viewed as a finite sequence of days
    IDaySegment<PaxDate>,
    ISetMembership<PaxDate>,
    // Arithmetic
    ISubtractionOperators<PaxYear, PaxYear, int>
{ }

public partial struct PaxYear // Preamble
{
    /// <summary>Represents the maximum value of <see cref="_yearsSinceEpoch"/>.
    /// <para>This field is a constant equal to 9998.</para></summary>
    private const int MaxYearsSinceEpoch = StandardScope.MaxYear - 1;

    /// <summary>
    /// Represents the count of consecutive years since the epoch
    /// <see cref="DayZero.SundayBeforeGregorian"/>.
    /// <para>This field is in the range from 0 to <see cref="MaxYearsSinceEpoch"/>.
    /// </para>
    /// </summary>
    private readonly uint _yearsSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaxYear"/> struct to the
    /// specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of years supported values.</exception>
    public PaxYear(int year)
    {
        if (year < StandardScope.MinYear || year > StandardScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year);

        _yearsSinceEpoch = unchecked((uint)(year - 1));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaxYear"/> struct to the
    /// specified year.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    private PaxYear(uint yearsSinceEpoch)
    {
        _yearsSinceEpoch = yearsSinceEpoch;
    }

    /// <summary>
    /// Gets the earliest possible value of a <see cref="PaxYear"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    //
    // MinValue = new(1) = new() = default(PaxYear)
    public static PaxYear MinValue { get; }

    /// <summary>
    /// Gets the latest possible value of a <see cref="PaxYear"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static PaxYear MaxValue { get; } = new(StandardScope.MaxYear);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static PaxCalendar Calendar => PaxCalendar.Instance;

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
    public int Year => YearsSinceEpoch + 1;

    /// <inheritdoc />
    public bool IsLeap => Calendar.Schema.IsLeapYear(Year);

    /// <summary>
    /// Gets the count of consecutive years since the epoch
    /// <see cref="DayZero.SundayBeforeGregorian"/>.
    /// </summary>
    private int YearsSinceEpoch => unchecked((int)_yearsSinceEpoch);

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString() => FormattableString.Invariant($"{Year:D4} ({Calendar})");
}

public partial struct PaxYear // Factories
{
    /// <summary>
    /// Creates a new instance of the <see cref="PaxYear"/> struct from the
    /// specified <see cref="PaxMonth"/> value.
    /// </summary>
    [Pure]
    public static PaxYear Create(PaxMonth month) => UnsafeCreate(month.Year);

    /// <summary>
    /// Creates a new instance of the <see cref="PaxYear"/> struct from the
    /// specified <see cref="PaxDate"/> value.
    /// </summary>
    [Pure]
    public static PaxYear Create(PaxDate date) => UnsafeCreate(date.Year);

    /// <summary>
    /// Creates a new instance of the <see cref="PaxYear"/> struct from the
    /// specified year.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static PaxYear UnsafeCreate(int year) => new(unchecked((uint)(year - 1)));
}

public partial struct PaxYear // IMonthSegment
{
    /// <inheritdoc />
    public PaxMonth MinMonth
    {
        get
        {
            int monthsSinceEpoch = Calendar.Schema.CountMonthsSinceEpoch(Year, 1);
            return new PaxMonth(monthsSinceEpoch);
        }
    }

    /// <inheritdoc />
    public PaxMonth MaxMonth
    {
        get
        {
            var sch = Calendar.Schema;
            int y = Year;
            int m = sch.CountMonthsInYear(y);
            int monthsSinceEpoch = sch.CountMonthsSinceEpoch(y, m);
            return new PaxMonth(monthsSinceEpoch);
        }
    }

    /// <inheritdoc />
    [Pure]
    public int CountMonths() => Calendar.Schema.CountMonthsInYear(Year);

    /// <inheritdoc />
    [Pure]
    public Range<PaxMonth> ToMonthRange() => Range.UnsafeCreate(MinMonth, MaxMonth);

    /// <inheritdoc />
    [Pure]
    public IEnumerable<PaxMonth> EnumerateMonths()
    {
        var sch = Calendar.Schema;
        int y = Year;
        int startOfYear = sch.CountMonthsSinceEpoch(y, 1);
        int monthsInYear = sch.CountMonthsInYear(y);

        return from monthsSinceEpoch
               in Enumerable.Range(startOfYear, monthsInYear)
               select new PaxMonth(monthsSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public bool Contains(PaxMonth month) => month.Year == Year;

    /// <summary>
    /// Obtains the month corresponding to the specified month of this year
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="month"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public PaxMonth GetMonthOfYear(int month)
    {
        var chr = Calendar;
        // We already know that "y" is valid, we only need to check "month".
        int y = Year;
        chr.Scope.PreValidator.ValidateMonth(y, month);
        int monthsSinceEpoch = chr.Schema.CountMonthsSinceEpoch(y, month);
        return new PaxMonth(monthsSinceEpoch);
    }
}

public partial struct PaxYear // IDaySegment
{
    /// <inheritdoc />
    public PaxDate MinDay
    {
        get
        {
            int daysSinceEpoch = Calendar.Schema.CountDaysSinceEpoch(Year, 1);
            return new PaxDate(daysSinceEpoch);
        }
    }

    /// <inheritdoc />
    public PaxDate MaxDay
    {
        get
        {
            var sch = Calendar.Schema;
            int y = Year;
            int doy = sch.CountDaysInYear(y);
            int daysSinceEpoch = sch.CountDaysSinceEpoch(y, doy);
            return new PaxDate(daysSinceEpoch);
        }
    }

    /// <inheritdoc />
    /// <remarks>See also <see cref="CalendarSystem{TDate}.CountDaysInYear(int)"/>.
    /// </remarks>
    [Pure]
    public int CountDays() => Calendar.Schema.CountDaysInYear(Year);

    /// <inheritdoc />
    /// <remarks>See also <see cref="CalendarSystem{TDate}.GetDaysInYear(int)"/>.
    /// </remarks>
    [Pure]
    public Range<PaxDate> ToDayRange() => Range.UnsafeCreate(MinDay, MaxDay);

    /// <inheritdoc />
    [Pure]
    public IEnumerable<PaxDate> EnumerateDays()
    {
        var sch = Calendar.Schema;
        int y = Year;
        int startOfYear = sch.CountDaysSinceEpoch(y, 1);
        int daysInYear = sch.CountDaysInYear(y);

        return from daysSinceEpoch
               in Enumerable.Range(startOfYear, daysInYear)
               select new PaxDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public bool Contains(PaxDate date) => date.Year == Year;

    /// <summary>
    /// Obtains the ordinal date corresponding to the specified day of this year
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfYear"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public PaxDate GetDayOfYear(int dayOfYear)
    {
        var chr = Calendar;
        int y = Year;
        // We already know that "y" is valid, we only need to check "dayOfYear".
        chr.Scope.PreValidator.ValidateDayOfYear(y, dayOfYear);
        int daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(y, dayOfYear);
        return new PaxDate(daysSinceEpoch);
    }
}

public partial struct PaxYear // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(PaxYear left, PaxYear right) =>
        left._yearsSinceEpoch == right._yearsSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(PaxYear left, PaxYear right) =>
        left._yearsSinceEpoch != right._yearsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(PaxYear other) => _yearsSinceEpoch == other._yearsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is PaxYear year && Equals(year);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => YearsSinceEpoch;
}

public partial struct PaxYear // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(PaxYear left, PaxYear right) =>
        left._yearsSinceEpoch < right._yearsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(PaxYear left, PaxYear right) =>
        left._yearsSinceEpoch <= right._yearsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(PaxYear left, PaxYear right) =>
        left._yearsSinceEpoch > right._yearsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(PaxYear left, PaxYear right) =>
        left._yearsSinceEpoch >= right._yearsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static PaxYear Min(PaxYear x, PaxYear y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static PaxYear Max(PaxYear x, PaxYear y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(PaxYear other) => _yearsSinceEpoch.CompareTo(other._yearsSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is PaxYear year ? CompareTo(year)
        : ThrowHelpers.ThrowNonComparable(typeof(PaxYear), obj);
}

public partial struct PaxYear // Math ops
{
    /// <summary>
    /// Subtracts the two specified years and returns the number of years between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountYearsSince()")]
    public static int operator -(PaxYear left, PaxYear right) => left.CountYearsSince(right);

    /// <summary>
    /// Adds a number of years to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported years.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusYears()")]
    public static PaxYear operator +(PaxYear value, int years) => value.PlusYears(years);

    /// <summary>
    /// Subtracts a number of years to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range
    /// of supported years.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusYears()")]
    public static PaxYear operator -(PaxYear value, int years) => value.PlusYears(-years);

    /// <summary>
    /// Adds one year to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported year.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextYear()")]
    public static PaxYear operator ++(PaxYear value) => value.NextYear();

    /// <summary>
    /// Subtracts one year to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported year.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousYear()")]
    public static PaxYear operator --(PaxYear value) => value.PreviousYear();

    /// <summary>
    /// Counts the number of years elapsed since the specified year.
    /// </summary>
    [Pure]
    public int CountYearsSince(PaxYear other) =>
        // No need to use a checked context here. Indeed, the absolute value of
        // the result is at most equal to (MaxYear - 1).
        YearsSinceEpoch - other.YearsSinceEpoch;

    /// <summary>
    /// Adds a number of years to the current instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported years.
    /// </exception>
    [Pure]
    public PaxYear PlusYears(int years)
    {
        uint yearsSinceEpoch = unchecked((uint)checked(YearsSinceEpoch + years));
        if (yearsSinceEpoch > MaxYearsSinceEpoch) ThrowHelpers.ThrowYearOverflow();
        return new PaxYear(yearsSinceEpoch);
    }

    /// <summary>
    /// Obtains the year after the current instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported year.</exception>
    [Pure]
    public PaxYear NextYear()
    {
        if (_yearsSinceEpoch == MaxYearsSinceEpoch) ThrowHelpers.ThrowYearOverflow();
        return new PaxYear(_yearsSinceEpoch + 1);
    }

    /// <summary>
    /// Obtains the year before the current instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported year.</exception>
    [Pure]
    public PaxYear PreviousYear()
    {
        if (_yearsSinceEpoch == 0) ThrowHelpers.ThrowYearOverflow();
        return new PaxYear(_yearsSinceEpoch - 1);
    }
}
