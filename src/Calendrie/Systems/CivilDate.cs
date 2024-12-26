// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
public partial struct CivilDate // Preamble
{
    /// <summary>Represents the maximum value of <see cref="_daysSinceZero"/>.
    /// <para>This field is a constant equal to 3_652_058.</para></summary>
    internal const int MaxDaysSinceZero = 3_652_058;

    /// <summary>
    /// Represents the count of consecutive days since <see cref="DayZero.NewStyle"/>.
    /// <para>This field is in the range from 0 to <see cref="MaxDaysSinceZero"/>.
    /// </para>
    /// </summary>
    private readonly int _daysSinceZero;

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilDate"/> struct to the
    /// specified date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the range
    /// of supported years.</exception>
    public CivilDate(int year, int month, int day)
    {
        CivilScope.ValidateYearMonthDayImpl(year, month, day);

        _daysSinceZero = CivilFormulae.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilDate"/> struct to the
    /// specified ordinal date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid ordinal date or <paramref name="year"/> is outside
    /// the range of supported years.</exception>
    public CivilDate(int year, int dayOfYear)
    {
        CivilScope.ValidateOrdinalImpl(year, dayOfYear);

        _daysSinceZero = CivilFormulae.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilDate"/> struct.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    internal CivilDate(int daysSinceZero)
    {
        _daysSinceZero = daysSinceZero;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    //
    // MinValue = new(0) = new() = default(CivilDate)
    public static CivilDate MinValue { get; }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static CivilDate MaxValue { get; } = new(MaxDaysSinceZero);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static CivilCalendar Calendar => CivilCalendar.Instance;

    /// <inheritdoc />
    public DayNumber DayNumber => new(_daysSinceZero);

    /// <summary>
    /// Gets the count of days since the Gregorian epoch.
    /// </summary>
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
    public int Year => CivilFormulae.GetYear(_daysSinceZero);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            CivilFormulae.GetDateParts(_daysSinceZero, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = CivilFormulae.GetYear(_daysSinceZero, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            CivilFormulae.GetDateParts(_daysSinceZero, out _, out _, out int d);
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
            CivilFormulae.GetDateParts(_daysSinceZero, out _, out int m, out int d);
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
        CivilFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({Calendar})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        CivilFormulae.GetDateParts(_daysSinceZero, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = CivilFormulae.GetYear(_daysSinceZero, out dayOfYear);
}

public partial struct CivilDate // Factories & conversions
{
    /// <summary>
    /// Defines an implicit conversion of a <see cref="CivilDate"/> value to a
    /// <see cref="GregorianDate"/> value.
    /// </summary>
    public static implicit operator GregorianDate(CivilDate date) => new(date._daysSinceZero);

    /// <summary>
    /// Converts the current instance to a <see cref="GregorianDate"/> value.
    /// <para>See also <see cref="GregorianDate.FromCivilDate(CivilDate)"/></para>
    /// </summary>
    public GregorianDate ToGregorianDate() => new(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public static CivilDate FromDayNumber(DayNumber dayNumber)
    {
        int daysSinceZero = dayNumber.DaysSinceZero;

        if (unchecked((uint)daysSinceZero) > MaxDaysSinceZero)
            throw new ArgumentOutOfRangeException(nameof(dayNumber));

        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    static CivilDate IDateFactory<CivilDate>.CreateUnchecked(int daysSinceEpoch) =>
        new(daysSinceEpoch);
}

public partial struct CivilDate // Find close by day of the week
{
    /// <inheritdoc />
    [Pure]
    public CivilDate Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceZero = _daysSinceZero + (δ >= 0 ? δ - DaysInWeek : δ); ;
        if (daysSinceZero < 0) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceZero = _daysSinceZero + (δ > 0 ? δ - DaysInWeek : δ); ;
        if (daysSinceZero < 0) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate Nearest(DayOfWeek dayOfWeek)
    {
        int daysSinceZero = DayNumber.Nearest(dayOfWeek).DaysSinceZero;
        if ((uint)daysSinceZero > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate NextOrSame(DayOfWeek dayOfWeek)
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
    public CivilDate Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceZero = _daysSinceZero + (δ <= 0 ? δ + DaysInWeek : δ);
        if (daysSinceZero > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }
}

public partial struct CivilDate // Math
{
    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountDaysSince()")]
    public static int operator -(CivilDate left, CivilDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See AddDays()")]
    public static CivilDate operator +(CivilDate value, int days) => value.AddDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See AddDays()")]
    public static CivilDate operator -(CivilDate value, int days) => value.AddDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextDay()")]
    public static CivilDate operator ++(CivilDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousDay()")]
    public static CivilDate operator --(CivilDate value) => value.PreviousDay();

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(CivilDate other) =>
        // No need to use a checked context here.
        _daysSinceZero - other._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public CivilDate AddDays(int days)
    {
        int daysSinceZero = checked(_daysSinceZero + days);

        if ((uint)daysSinceZero > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();

        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate NextDay()
    {
        if (this == MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceZero + 1);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate PreviousDay()
    {
        if (this == MinValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceZero - 1);
    }
}
