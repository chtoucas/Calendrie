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
    // A year viewed as a finite sequence of months
    IMonthSegment<GregorianMonth>,
    ISetMembership<GregorianMonth>,
    // A year viewed as a finite sequence of days
    IDaySegment<GregorianDate>,
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
    internal GregorianYear(int yearsSinceEpoch, bool _)
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
    /// Gets the companion calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static GregorianCalendar Calendar => GregorianCalendar.Instance;

    static Calendar IYear.Calendar => Calendar;

    /// <inheritdoc />
    public int YearsSinceEpoch => _yearsSinceEpoch;

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
    public int Year => _yearsSinceEpoch + 1;

    /// <inheritdoc />
    public bool IsLeap => GregorianFormulae.IsLeapYear(Year);

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        return _yearsSinceEpoch >= 0
            ? FormattableString.Invariant($"{Year:D4} ({GregorianCalendar.DisplayName})")
            : FormattableString.Invariant($"{getBCEYear(Year)} BCE ({GregorianCalendar.DisplayName})");

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
    /// <inheritdoc />
    [Pure]
    public static GregorianYear Create(int year) => new(year);

    /// <summary>
    /// Attempts to create a new instance of the <see cref="GregorianYear"/>
    /// struct from the specified year.
    /// </summary>
    [Pure]
    public static GregorianYear? TryCreate(int year)
    {
        bool ok = year >= GregorianScope.MinYear && year <= GregorianScope.MaxYear;
        return ok ? UnsafeCreate(year) : null;
    }

    [Pure]
    static bool IYear<GregorianYear>.TryCreate(int year, out GregorianYear result)
    {
        bool ok = year >= GregorianScope.MinYear && year <= GregorianScope.MaxYear;
        result = ok ? UnsafeCreate(year) : default;
        return ok;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="GregorianYear"/> struct
    /// from the specified year.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static GregorianYear UnsafeCreate(int year) => new(year - 1, default);

    //
    // Conversions
    //

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

public partial struct GregorianYear // IDaySegment
{
    /// <inheritdoc />
    public GregorianDate MinDay
    {
        get
        {
            int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(Year, 1);
            return GregorianDate.UnsafeCreate(daysSinceZero);
        }
    }

    /// <inheritdoc />
    public GregorianDate MaxDay
    {
        get
        {
            int doy = GregorianFormulae.CountDaysInYear(Year);
            int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(Year, doy);
            return GregorianDate.UnsafeCreate(daysSinceZero);
        }
    }

    /// <inheritdoc />
    [Pure]
    public int CountDays() => GregorianFormulae.CountDaysInYear(Year);

    /// <inheritdoc />
    [Pure]
    public Range<GregorianDate> ToDayRange()
    {
        int startOfYear = GregorianFormulae.CountDaysSinceEpoch(Year, 1);
        int daysInYear = GregorianFormulae.CountDaysInYear(Year);
        return Range.StartingAt(GregorianDate.UnsafeCreate(startOfYear), daysInYear);
    }

    /// <inheritdoc />
    [Pure]
    public IEnumerable<GregorianDate> EnumerateDays()
    {
        int startOfYear = GregorianFormulae.CountDaysSinceEpoch(Year, 1);
        int daysInYear = GregorianFormulae.CountDaysInYear(Year);

        return from daysSinceZero
               in Enumerable.Range(startOfYear, daysInYear)
               select GregorianDate.UnsafeCreate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public bool Contains(GregorianDate date) => date.Year == Year;

    /// <summary>
    /// Obtains the date corresponding to the specified day of this year instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfYear"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public GregorianDate GetDayOfYear(int dayOfYear)
    {
        // We already know that "y" is valid, we only need to check "dayOfYear".
        Calendar.Scope.PreValidator.ValidateDayOfYear(Year, dayOfYear);
        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(Year, dayOfYear);
        return GregorianDate.UnsafeCreate(daysSinceZero);
    }
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

