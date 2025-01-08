// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie;
using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Represents the Gregorian year.
/// <para><i>All</i> years within the range [-999_998..999_999] of years are
/// supported.
/// </para>
/// <para><see cref="GregorianYear"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct GregorianYear :
    IYear<GregorianYear>,
    ICalendarBound<GregorianCalendar>,
    // A year viewed as a finite sequence of months
    IMonthSegment<GregorianMonth>,
    ISetMembership<GregorianMonth>,
    // A year viewed as a finite sequence of days
    IDateSegment<GregorianDate>,
    ISetMembership<GregorianDate>,
    // Arithmetic
    ISubtractionOperators<GregorianYear, GregorianYear, int>
{ }

public partial struct GregorianYear // Preamble
{
    /// <summary>Represents the minimu value of <see cref="_yearsSinceEpoch"/>.
    /// <para>This field is a constant equal to -999_999.</para></summary>
    private const int MinYearsSinceEpoch = GregorianScope.MinYear - 1;

    /// <summary>Represents the maximum value of <see cref="_yearsSinceEpoch"/>.
    /// <para>This field is a constant equal to 999_998.</para></summary>
    private const int MaxYearsSinceEpoch = GregorianScope.MaxYear - 1;

    /// <summary>
    /// Represents the count of consecutive years since the epoch
    /// <see cref="DayZero.NewStyle"/>.
    /// <para>This field is in the range from <see cref="MinYearsSinceEpoch"/>
    /// to <see cref="MaxYearsSinceEpoch"/>.</para>
    /// </summary>
    private readonly int _yearsSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianYear"/> struct
    /// to the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of years supported values.</exception>
    public GregorianYear(int year)
    {
        if (year < GregorianScope.MinYear || year > GregorianScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year);

        _yearsSinceEpoch = year - 1;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianYear"/> struct.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    private GregorianYear(int yearsSinceEpoch, bool _)
    {
        _yearsSinceEpoch = yearsSinceEpoch;
    }

    /// <summary>
    /// Gets the smallest possible value of <see cref="GregorianYear"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The earliest supported year.</returns>
    public static GregorianYear MinValue { get; } = new(MinYearsSinceEpoch, default);

    /// <summary>
    /// Gets the largest possible value of <see cref="GregorianYear"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The latest supported year.</returns>
    public static GregorianYear MaxValue { get; } = new(MaxYearsSinceEpoch, default);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static GregorianCalendar Calendar => GregorianCalendar.Instance;

    /// <inheritdoc />
    public int YearsSinceEpoch => _yearsSinceEpoch;

    /// <summary>
    /// Gets the century of the era.
    /// </summary>
    public Ord CenturyOfEra => Ord.FromInt32(Century);

    /// <summary>
    /// Gets the century number.
    /// </summary>
    public int Century => YearNumbering.GetCentury(Number);

    /// <summary>
    /// Gets the year of the era.
    /// </summary>
    public Ord YearOfEra => Ord.FromInt32(Number);

    /// <summary>
    /// Gets the year of the century.
    /// <para>The result is in the range from 1 to 100.</para>
    /// </summary>
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Number);

    /// <summary>
    /// Gets the (algebraic) year number.
    /// </summary>
    public int Number => _yearsSinceEpoch + 1;

    /// <inheritdoc />
    public bool IsLeap => GregorianFormulae.IsLeapYear(Number);

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        return _yearsSinceEpoch >= 0 ? FormattableString.Invariant($"{Number:D4} ({Calendar})")
            : FormattableString.Invariant($"{getBCEYear(Number)} BCE ({Calendar})");

        [Pure]
        static int getBCEYear(int y)
        {
            Debug.Assert(y <= 0);
            var (pos, _) = Ord.FromInt32(y);
            return pos;
        }
    }
}

public partial struct GregorianYear // Factories
{
    /// <summary>
    /// Creates a new instance of the <see cref="GregorianYear"/> struct
    /// from the specified <see cref="GregorianMonth"/> value.
    /// </summary>
    [Pure]
    public static GregorianYear Create(GregorianMonth month) => UnsafeCreate(month.Year);

    /// <summary>
    /// Creates a new instance of the <see cref="GregorianYear"/> struct
    /// from the specified <see cref="GregorianDate"/> value.
    /// </summary>
    [Pure]
    public static GregorianYear Create(GregorianDate date) => UnsafeCreate(date.Year);

    /// <summary>
    /// Creates a new instance of the <see cref="GregorianYear"/> struct
    /// from the specified year.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static GregorianYear UnsafeCreate(int year) => new(year - 1, default);
}

public partial struct GregorianYear // IMonthSegment
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthCount = GregorianCalendar.MonthsInYear;

    /// <inheritdoc />
    public GregorianMonth MinMonth => GregorianMonth.UnsafeCreate(Number, 1);

    /// <inheritdoc />
    public GregorianMonth MaxMonth => GregorianMonth.UnsafeCreate(Number, MonthCount);

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
        int startOfYear = GregorianMonth.UnsafeCreate(Number, 1).MonthsSinceEpoch;

        return from monthsSinceEpoch
               in Enumerable.Range(startOfYear, MonthCount)
               select new GregorianMonth(monthsSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public bool Contains(GregorianMonth month) => month.Year == Number;

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
        Calendar.Scope.PreValidator.ValidateMonth(Number, month);
        return GregorianMonth.UnsafeCreate(Number, month);
    }
}

public partial struct GregorianYear // IDateSegment
{
    /// <inheritdoc />
    public GregorianDate MinDay
    {
        get
        {
            int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(Number, 1);
            return new GregorianDate(daysSinceZero);
        }
    }

    /// <inheritdoc />
    public GregorianDate MaxDay
    {
        get
        {
            int doy = GregorianFormulae.CountDaysInYear(Number);
            int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(Number, doy);
            return new GregorianDate(daysSinceZero);
        }
    }

    /// <inheritdoc />
    /// <remarks>See also <see cref="CalendarSystem{TDate}.CountDaysInYear(int)"/>.
    /// </remarks>
    [Pure]
    public int CountDays() => GregorianFormulae.CountDaysInYear(Number);

    /// <inheritdoc />
    /// <remarks>See also <see cref="CalendarSystem{TDate}.GetDaysInYear(int)"/>.
    /// </remarks>
    [Pure]
    public Range<GregorianDate> ToDayRange()
    {
        int startOfYear = GregorianFormulae.CountDaysSinceEpoch(Number, 1);
        int daysInYear = GregorianFormulae.CountDaysInYear(Number);
        return Range.StartingAt(new GregorianDate(startOfYear), daysInYear);
    }

    /// <inheritdoc />
    [Pure]
    public IEnumerable<GregorianDate> EnumerateDays()
    {
        int startOfYear = GregorianFormulae.CountDaysSinceEpoch(Number, 1);
        int daysInYear = GregorianFormulae.CountDaysInYear(Number);

        return from daysSinceZero
               in Enumerable.Range(startOfYear, daysInYear)
               select new GregorianDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public bool Contains(GregorianDate date) => date.Year == Number;

    /// <summary>
    /// Obtains the date corresponding to the specified day of this year instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfYear"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public GregorianDate GetDayOfYear(int dayOfYear)
    {
        // We already know that "y" is valid, we only need to check "dayOfYear".
        Calendar.Scope.PreValidator.ValidateDayOfYear(Number, dayOfYear);
        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(Number, dayOfYear);
        return new GregorianDate(daysSinceZero);
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

public partial struct GregorianYear // Math ops
{
    /// <summary>
    /// Subtracts the two specified years and returns the number of years between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountYearsSince()")]
    public static int operator -(GregorianYear left, GregorianYear right) => left.CountYearsSince(right);

    /// <summary>
    /// Adds a number of years to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported years.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusYears()")]
    public static GregorianYear operator +(GregorianYear value, int years) => value.PlusYears(years);

    /// <summary>
    /// Subtracts a number of years to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range
    /// of supported years.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusYears()")]
    public static GregorianYear operator -(GregorianYear value, int years) => value.PlusYears(-years);

    /// <summary>
    /// Adds one year to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported year.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextYear()")]
    public static GregorianYear operator ++(GregorianYear value) => value.NextYear();

    /// <summary>
    /// Subtracts one year to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported year.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousYear()")]
    public static GregorianYear operator --(GregorianYear value) => value.PreviousYear();

    /// <summary>
    /// Counts the number of years elapsed since the specified year.
    /// </summary>
    [Pure]
    public int CountYearsSince(GregorianYear other) =>
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
    public GregorianYear PlusYears(int years)
    {
        int yearsSinceEpoch = checked(_yearsSinceEpoch + years);
        if (years < MinYearsSinceEpoch || yearsSinceEpoch > MaxYearsSinceEpoch)
            ThrowHelpers.ThrowYearOverflow();
        return new GregorianYear(yearsSinceEpoch, default);
    }

    /// <summary>
    /// Obtains the year after the current instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported year.</exception>
    [Pure]
    public GregorianYear NextYear()
    {
        if (_yearsSinceEpoch == MaxYearsSinceEpoch) ThrowHelpers.ThrowYearOverflow();
        return new GregorianYear(_yearsSinceEpoch + 1, default);
    }

    /// <summary>
    /// Obtains the year before the current instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported year.</exception>
    [Pure]
    public GregorianYear PreviousYear()
    {
        if (_yearsSinceEpoch == MinYearsSinceEpoch) ThrowHelpers.ThrowYearOverflow();
        return new GregorianYear(_yearsSinceEpoch - 1, default);
    }
}

