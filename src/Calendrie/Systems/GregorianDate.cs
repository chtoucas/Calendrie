﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

/// <remarks><i>All</i> dates within the range [-999_998..999_999] of years are
/// supported.</remarks>
public partial struct GregorianDate // Preamble
{
    /// <summary>Represents the minimum value of <see cref="_daysSinceZero"/>.
    /// <para>This field is a constant equal to -365_242_135.</para></summary>
    private const int MinDaysSinceZero = -365_242_135;
    /// <summary>Represents the maximum value of <see cref="_daysSinceZero"/>.
    /// <para>This field is a constant equal to 365_242_133.</para></summary>
    private const int MaxDaysSinceZero = 365_242_133;

    /// <summary>
    /// Represents the count of consecutive days since <see cref="DayZero.NewStyle"/>.
    /// <para>This field is in the range from <see cref="MinDaysSinceZero"/>
    /// to <see cref="MaxDaysSinceZero"/>.</para>
    /// </summary>
    private readonly int _daysSinceZero;

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianDate"/> struct to
    /// the specified date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the range
    /// of supported years.</exception>
    public GregorianDate(int year, int month, int day)
    {
        GregorianScope.ValidateYearMonthDayImpl(year, month, day);

        _daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianDate"/> struct to
    /// the specified ordinal date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid ordinal date or <paramref name="year"/> is outside
    /// the range of supported years.</exception>
    public GregorianDate(int year, int dayOfYear)
    {
        GregorianScope.ValidateOrdinalImpl(year, dayOfYear);

        _daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianDate"/> struct.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    internal GregorianDate(int daysSinceZero)
    {
        _daysSinceZero = daysSinceZero;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static GregorianDate MinValue { get; } = new(MinDaysSinceZero);

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static GregorianDate MaxValue { get; } = new(MaxDaysSinceZero);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static GregorianCalendar Calendar => GregorianCalendar.Instance;

    /// <inheritdoc />
    public DayNumber DayNumber => new(_daysSinceZero);

    /// <summary>Gets the count of days since the Gregorian epoch.</summary>
    public int DaysSinceZero => _daysSinceZero;

    int IAbsoluteDate.DaysSinceEpoch => _daysSinceZero;

    /// <inheritdoc />
    public Ord CenturyOfEra => Ord.FromInt32(Century);

    /// <inheritdoc />
    public int Century => YearNumbering.GetCentury(Year);

    /// <inheritdoc />
    public Ord YearOfEra => Ord.FromInt32(Year);

    /// <inheritdoc />
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

    /// <inheritdoc />
    public int Year => GregorianFormulae.GetYear(_daysSinceZero);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            GregorianFormulae.GetDateParts(_daysSinceZero, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = GregorianFormulae.GetYear(_daysSinceZero, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            GregorianFormulae.GetDateParts(_daysSinceZero, out _, out _, out int d);
            return d;
        }
    }

    /// <inheritdoc />
    public DayOfWeek DayOfWeek => DayNumber.DayOfWeek;

    /// <inheritdoc />
    public bool IsIntercalary
    {
        get
        {
            GregorianFormulae.GetDateParts(_daysSinceZero, out _, out int m, out int d);
            return GregorianFormulae.IsIntercalaryDay(m, d);
        }
    }

    bool IDateable.IsSupplementary => false;

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        GregorianFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({Calendar})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        GregorianFormulae.GetDateParts(_daysSinceZero, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = GregorianFormulae.GetYear(_daysSinceZero, out dayOfYear);
}

public partial struct GregorianDate // Factories & conversions
{
    /// <summary>
    /// Creates a new instance of the <see cref="GregorianDate"/> struct from
    /// the specified <see cref="CivilDate"/> value.
    /// <para>See also <see cref="CivilDate.ToGregorianDate()"/></para>
    /// </summary>
    public static GregorianDate FromCivilDate(CivilDate date) => new(date.DaysSinceZero);
}

public partial struct GregorianDate // Find close by day of the week
{
    /// <inheritdoc />
    [Pure]
    public GregorianDate Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceZero = _daysSinceZero + (δ >= 0 ? δ - DaysInWeek : δ);
        if (daysSinceZero < MinDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceZero = _daysSinceZero + (δ > 0 ? δ - DaysInWeek : δ); ;
        if (daysSinceZero < MinDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate Nearest(DayOfWeek dayOfWeek)
    {
        int daysSinceZero = DayNumber.Nearest(dayOfWeek).DaysSinceZero;
        if (daysSinceZero < MinDaysSinceZero || daysSinceZero > MaxDaysSinceZero)
            ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate NextOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceZero = _daysSinceZero + (δ < 0 ? δ + DaysInWeek : δ);
        if (daysSinceZero > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceZero = _daysSinceZero + (δ <= 0 ? δ + DaysInWeek : δ);
        if (daysSinceZero > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }
}

public partial struct GregorianDate // Math
{
    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountDaysSince()")]
    public static int operator -(GregorianDate left, GregorianDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static GregorianDate operator +(GregorianDate value, int days) => value.PlusDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static GregorianDate operator -(GregorianDate value, int days) => value.PlusDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextDay()")]
    public static GregorianDate operator ++(GregorianDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousDay()")]
    public static GregorianDate operator --(GregorianDate value) => value.PreviousDay();

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(GregorianDate other) =>
        // No need to use a checked context here.
        _daysSinceZero - other._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public GregorianDate PlusDays(int days)
    {
        int daysSinceZero = checked(_daysSinceZero + days);

        if (daysSinceZero < MinDaysSinceZero || daysSinceZero > MaxDaysSinceZero)
            ThrowHelpers.ThrowDateOverflow();

        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate NextDay()
    {
        if (this == MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceZero + 1);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate PreviousDay()
    {
        if (this == MinValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceZero - 1);
    }
}
