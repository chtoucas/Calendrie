// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

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
    /// <summary>
    /// Initializes a new instance of the <see cref="CivilYear"/> struct to the
    /// specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of years supported values.</exception>
    public CivilYear(int year)
    {
        CivilCalendar.Instance.Scope.ValidateYear(year);

        Year = year;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    //
    // MinValue = new(0) = new() = default(CivilYear)
    public static CivilYear MinValue { get; }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
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
    /// Gets the (algebraic) year number.
    /// </summary>
    public int Year { get; }

    /// <summary>
    /// Returns <see langword="true"/> if the current instance is a leap year;
    /// otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsLeap => Calendar.Schema.IsLeapYear(Year);

    /// <summary>
    /// Gets the first month of this year instance.
    /// </summary>
    public CivilMonth FirstMonth => new(Year, 1);

    /// <summary>
    /// Gets the last month of this year instance.
    /// </summary>
    public CivilMonth LastMonth => new(Year, CountMonthsInYear());

    /// <summary>
    /// Gets the first day of this year instance.
    /// </summary>
    public CivilDate FirstDay => new(Year, 1);

    /// <summary>
    /// Gets the last day of this year instance.
    /// </summary>
    public CivilDate LastDay => new(Year, CountMonthsInYear(), 1);

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString() => FormattableString.Invariant($"{Year:D4} ({Calendar})");
}

public partial struct CivilYear // Conversions
{
    /// <summary>
    /// Converts the current instance to a range of days.
    /// </summary>
    [Pure]
    public Range<CivilDate> ToRange() => Range.Create(FirstDay, LastDay);

    /// <summary>
    /// Converts the current instance to a range of months.
    /// </summary>
    [Pure]
    public Range<CivilMonth> ToMonthRange() => Range.Create(FirstMonth, LastMonth);
}

public partial struct CivilYear // Counting
{
    /// <summary>
    /// Obtains the number of months in this year instance.
    /// </summary>
    [Pure]
    public int CountMonthsInYear() => Calendar.Schema.CountMonthsInYear(Year);

    /// <summary>
    /// Obtains the number of days in this year instance.
    /// </summary>
    [Pure]
    public int CountDaysInYear() => Calendar.Schema.CountDaysInYear(Year);
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
        // We already know that "y" is valid, we only need to check "month".
        int y = Year;
        Calendar.Scope.PreValidator.ValidateMonth(y, month);
        return new CivilMonth(y, month);
    }

    /// <summary>
    /// Obtains the sequence of all months in this year instance.
    /// </summary>
    [Pure]
    public IEnumerable<CivilMonth> GetAllMonths()
    {
        int y = Year;
        int monthsInYear = CountMonthsInYear();
        for (int m = 1; m <= monthsInYear; m++)
        {
            yield return new CivilMonth(y, m);
        }
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
        int y = Year;
        Calendar.Scope.PreValidator.ValidateDayOfYear(y, dayOfYear);
        return new CivilDate(y, dayOfYear);
    }

    /// <summary>
    /// Obtains the sequence of all days in this year instance.
    /// </summary>
    [Pure]
    public IEnumerable<CivilDate> GetAllDays()
    {
        int y = Year;
        int daysInYear = CountDaysInYear();

        for (int doy = 1; doy <= daysInYear; doy++)
        {
            yield return new CivilDate(y, doy);
        }
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
    public static bool operator ==(CivilYear left, CivilYear right) => left.Year == right.Year;

    /// <inheritdoc />
    public static bool operator !=(CivilYear left, CivilYear right) => left.Year != right.Year;

    /// <inheritdoc />
    [Pure]
    public bool Equals(CivilYear other) => Year == other.Year;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CivilYear year && Equals(year);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Year;
}

public partial struct CivilYear // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(CivilYear left, CivilYear right) => left.Year < right.Year;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(CivilYear left, CivilYear right) => left.Year <= right.Year;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(CivilYear left, CivilYear right) => left.Year > right.Year;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(CivilYear left, CivilYear right) => left.Year >= right.Year;

    /// <summary>
    /// Obtains the earlier year of two specified years.
    /// </summary>
    [Pure]
    public static CivilYear Min(CivilYear x, CivilYear y) => x < y ? x : y;

    /// <summary>
    /// Obtains the later year of two specified years.
    /// </summary>
    [Pure]
    public static CivilYear Max(CivilYear x, CivilYear y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(CivilYear other) => Year.CompareTo(other.Year);

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
        Year - other.Year;

    /// <summary>
    /// Adds a number of years to this year instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported years.</exception>
    [Pure]
    public CivilYear PlusYears(int years)
    {
        int y = checked(Year + years);
        if (y < StandardScope.MinYear || y > StandardScope.MaxYear)
            throw new OverflowException();
        return new CivilYear(y);
    }

    /// <summary>
    /// Obtains the year after this year instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported year.</exception>
    [Pure]
    public CivilYear NextYear()
    {
        if (Year == StandardScope.MaxYear) throw new OverflowException();
        return new(Year + 1);
    }

    /// <summary>
    /// Obtains the year before this year instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported year.</exception>
    [Pure]
    public CivilYear PreviousYear()
    {
        if (Year == StandardScope.MinYear) throw new OverflowException();
        return new(Year - 1);
    }
}
