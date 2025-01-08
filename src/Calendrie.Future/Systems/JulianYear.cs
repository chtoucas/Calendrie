﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie;
using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Represents the Julian year.
/// <para><i>All</i> years within the range [-999_998..999_999] of years are
/// supported.
/// </para>
/// <para><see cref="JulianYear"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct JulianYear :
    IYear<JulianYear>,
    ICalendarBound<JulianCalendar>,
    // A year viewed as a finite sequence of months
    IMonthSegment<JulianMonth>,
    ISetMembership<JulianMonth>,
    // A year viewed as a finite sequence of days
    IDateSegment<JulianDate>,
    ISetMembership<JulianDate>,
    // Arithmetic
    ISubtractionOperators<JulianYear, JulianYear, int>
{ }

public partial struct JulianYear // Preamble
{
    /// <summary>Represents the minimu value of <see cref="_yearsSinceEpoch"/>.
    /// <para>This field is a constant equal to -999_999.</para></summary>
    private const int MinYearsSinceEpoch = JulianScope.MinYear - 1;

    /// <summary>Represents the maximum value of <see cref="_yearsSinceEpoch"/>.
    /// <para>This field is a constant equal to 999_998.</para></summary>
    private const int MaxYearsSinceEpoch = JulianScope.MaxYear - 1;

    /// <summary>
    /// Represents the count of consecutive years since the epoch
    /// <see cref="DayZero.NewStyle"/>.
    /// <para>This field is in the range from <see cref="MinYearsSinceEpoch"/>
    /// to <see cref="MaxYearsSinceEpoch"/>.</para>
    /// </summary>
    private readonly int _yearsSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianYear"/> struct
    /// to the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of years supported values.</exception>
    public JulianYear(int year)
    {
        if (year < JulianScope.MinYear || year > JulianScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year);

        _yearsSinceEpoch = year - 1;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianYear"/> struct.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    private JulianYear(int yearsSinceEpoch, bool _)
    {
        _yearsSinceEpoch = yearsSinceEpoch;
    }

    /// <summary>
    /// Gets the smallest possible value of <see cref="JulianYear"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The earliest supported year.</returns>
    public static JulianYear MinValue { get; } = new(MinYearsSinceEpoch, default);

    /// <summary>
    /// Gets the largest possible value of <see cref="JulianYear"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The latest supported year.</returns>
    public static JulianYear MaxValue { get; } = new(MaxYearsSinceEpoch, default);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static JulianCalendar Calendar => JulianCalendar.Instance;

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
    public bool IsLeap => JulianFormulae.IsLeapYear(Number);

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

public partial struct JulianYear // Factories
{
    /// <inheritdoc />
    [Pure]
    public static JulianYear Create(int year) => new(year);

    /// <summary>
    /// Creates a new instance of the <see cref="JulianYear"/> struct
    /// from the specified <see cref="JulianMonth"/> value.
    /// </summary>
    [Pure]
    public static JulianYear Create(JulianMonth month) => UnsafeCreate(month.Year);

    /// <summary>
    /// Creates a new instance of the <see cref="JulianYear"/> struct
    /// from the specified <see cref="JulianDate"/> value.
    /// </summary>
    [Pure]
    public static JulianYear Create(JulianDate date) => UnsafeCreate(date.Year);

    /// <summary>
    /// Creates a new instance of the <see cref="JulianYear"/> struct
    /// from the specified year.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static JulianYear UnsafeCreate(int year) => new(year - 1, default);
}

public partial struct JulianYear // IDateSegment
{
    /// <inheritdoc />
    public JulianDate MinDay
    {
        get
        {
            int daysSinceZero = JulianFormulae.CountDaysSinceEpoch(Number, 1);
            return new JulianDate(daysSinceZero);
        }
    }

    /// <inheritdoc />
    public JulianDate MaxDay
    {
        get
        {
            int doy = JulianFormulae.CountDaysInYear(Number);
            int daysSinceZero = JulianFormulae.CountDaysSinceEpoch(Number, doy);
            return new JulianDate(daysSinceZero);
        }
    }

    /// <inheritdoc />
    /// <remarks>See also <see cref="CalendarSystem{TDate}.CountDaysInYear(int)"/>.
    /// </remarks>
    [Pure]
    public int CountDays() => JulianFormulae.CountDaysInYear(Number);

    /// <inheritdoc />
    /// <remarks>See also <see cref="CalendarSystem{TDate}.GetDaysInYear(int)"/>.
    /// </remarks>
    [Pure]
    public Range<JulianDate> ToDayRange()
    {
        int startOfYear = JulianFormulae.CountDaysSinceEpoch(Number, 1);
        int daysInYear = JulianFormulae.CountDaysInYear(Number);
        return Range.StartingAt(new JulianDate(startOfYear), daysInYear);
    }

    /// <inheritdoc />
    [Pure]
    public IEnumerable<JulianDate> EnumerateDays()
    {
        int startOfYear = JulianFormulae.CountDaysSinceEpoch(Number, 1);
        int daysInYear = JulianFormulae.CountDaysInYear(Number);

        return from daysSinceZero
               in Enumerable.Range(startOfYear, daysInYear)
               select new JulianDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public bool Contains(JulianDate date) => date.Year == Number;

    /// <summary>
    /// Obtains the date corresponding to the specified day of this year instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfYear"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public JulianDate GetDayOfYear(int dayOfYear)
    {
        // We already know that "y" is valid, we only need to check "dayOfYear".
        Calendar.Scope.PreValidator.ValidateDayOfYear(Number, dayOfYear);
        int daysSinceZero = JulianFormulae.CountDaysSinceEpoch(Number, dayOfYear);
        return new JulianDate(daysSinceZero);
    }
}

public partial struct JulianYear // Math ops
{
    /// <summary>
    /// Subtracts the two specified years and returns the number of years between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountYearsSince()")]
    public static int operator -(JulianYear left, JulianYear right) => left.CountYearsSince(right);

    /// <summary>
    /// Adds a number of years to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported years.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusYears()")]
    public static JulianYear operator +(JulianYear value, int years) => value.PlusYears(years);

    /// <summary>
    /// Subtracts a number of years to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range
    /// of supported years.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusYears()")]
    public static JulianYear operator -(JulianYear value, int years) => value.PlusYears(-years);

    /// <summary>
    /// Adds one year to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported year.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextYear()")]
    public static JulianYear operator ++(JulianYear value) => value.NextYear();

    /// <summary>
    /// Subtracts one year to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported year.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousYear()")]
    public static JulianYear operator --(JulianYear value) => value.PreviousYear();

    /// <summary>
    /// Counts the number of years elapsed since the specified year.
    /// </summary>
    [Pure]
    public int CountYearsSince(JulianYear other) =>
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
    public JulianYear PlusYears(int years)
    {
        int yearsSinceEpoch = checked(_yearsSinceEpoch + years);
        if (years < MinYearsSinceEpoch || yearsSinceEpoch > MaxYearsSinceEpoch)
            ThrowHelpers.ThrowYearOverflow();
        return new JulianYear(yearsSinceEpoch, default);
    }

    /// <summary>
    /// Obtains the year after the current instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported year.</exception>
    [Pure]
    public JulianYear NextYear()
    {
        if (_yearsSinceEpoch == MaxYearsSinceEpoch) ThrowHelpers.ThrowYearOverflow();
        return new JulianYear(_yearsSinceEpoch + 1, default);
    }

    /// <summary>
    /// Obtains the year before the current instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported year.</exception>
    [Pure]
    public JulianYear PreviousYear()
    {
        if (_yearsSinceEpoch == MinYearsSinceEpoch) ThrowHelpers.ThrowYearOverflow();
        return new JulianYear(_yearsSinceEpoch - 1, default);
    }
}

