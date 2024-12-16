// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;

/// <remarks><i>All</i> dates within the range [-999_998..999_999] of years are
/// supported.</remarks>
public partial struct JulianDate
{
    private const int EpochDaysSinceZero = -2;

    // Min/MaxDaysSinceZero = JulianScope.Instance.Segment.SupportedDays.Min/Max

    /// <summary>Represents the minimum value of <see cref="_daysSinceEpoch"/>.
    /// <para>This field is a constant equal to -365_249_635.</para></summary>
    internal const int MinDaysSinceEpoch = -365_249_635;
    /// <summary>Represents the maximum value of <see cref="_daysSinceEpoch"/>.
    /// <para>This field is a constant equal to -365_249_633.</para></summary>
    internal const int MaxDaysSinceEpoch = 365_249_633;

    /// <summary>
    /// Represents the count of consecutive days since <see cref="DayZero.OldStyle"/>.
    /// <para>This field is in the range from <see cref="MinDaysSinceEpoch"/>
    /// to <see cref="MaxDaysSinceEpoch"/>.</para>
    /// </summary>
    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianDate"/> struct to the
    /// specified date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the range
    /// of supported years.</exception>
    public JulianDate(int year, int month, int day)
    {
        JulianScope.ValidateYearMonthDayImpl(year, month, day);

        _daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianDate"/> struct to the
    /// specified ordinal date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid ordinal date or <paramref name="year"/> is outside
    /// the range of supported years.</exception>
    public JulianDate(int year, int dayOfYear)
    {
        JulianScope.ValidateOrdinalImpl(year, dayOfYear);

        _daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianDate"/> struct.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    internal JulianDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static JulianDate MinValue { get; } = new(MinDaysSinceEpoch);

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static JulianDate MaxValue { get; } = new(MaxDaysSinceEpoch);

    /// <inheritdoc />
    public static JulianCalendar Calendar => JulianCalendar.Instance;

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static DateAdjuster<JulianDate> Adjuster => JulianCalendar.Instance.Adjuster;

    /// <inheritdoc />
    public DayNumber DayNumber => new(EpochDaysSinceZero + _daysSinceEpoch);

    /// <inheritdoc />
    public int DaysSinceEpoch => _daysSinceEpoch;

    /// <inheritdoc />
    public Ord CenturyOfEra => Ord.FromInt32(Century);

    /// <inheritdoc />
    public int Century => YearNumbering.GetCentury(Year);

    /// <inheritdoc />
    public Ord YearOfEra => Ord.FromInt32(Year);

    /// <inheritdoc />
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

    /// <inheritdoc />
    public int Year => JulianFormulae.GetYear(_daysSinceEpoch);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            JulianFormulae.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = JulianFormulae.GetYear(_daysSinceEpoch, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            JulianFormulae.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
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
            JulianFormulae.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);
            return JulianFormulae.IsIntercalaryDay(m, d);
        }
    }

    /// <inheritdoc />
    public bool IsSupplementary => false;

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    private static JulianSchema Schema => JulianCalendar.UnderlyingSchema;

    /// <summary>
    /// Gets the calendar scope.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    private static JulianScope Scope => JulianCalendar.UnderlyingScope;

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        JulianFormulae.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({Calendar})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        JulianFormulae.GetDateParts(_daysSinceEpoch, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = JulianFormulae.GetYear(_daysSinceEpoch, out dayOfYear);
}

public partial struct JulianDate // Factories
{
    /// <inheritdoc />
    [Pure]
    public static JulianDate FromDayNumber(DayNumber dayNumber)
    {
        Scope.Validate(dayNumber);

        // We know that the subtraction won't overflow
        // > return new(dayNumber - s_Epoch);
        return new(dayNumber.DaysSinceZero - EpochDaysSinceZero);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="JulianDate"/> struct
    /// from the specified day number.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static JulianDate FromDayNumberUnchecked(DayNumber dayNumber) =>
        new(dayNumber.DaysSinceZero - EpochDaysSinceZero);

    /// <inheritdoc />
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    static JulianDate IFixedDateFactory<JulianDate>.FromDaysSinceEpochUnchecked(int daysSinceEpoch) =>
        new(daysSinceEpoch);
}

public partial struct JulianDate // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    public static int operator -(JulianDate left, JulianDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static JulianDate operator +(JulianDate value, int days) => value.AddDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static JulianDate operator -(JulianDate value, int days) => value.AddDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    public static JulianDate operator ++(JulianDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    public static JulianDate operator --(JulianDate value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(JulianDate other) =>
        // No need to use a checked context here.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public JulianDate AddDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);

        if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            ThrowHelpers.ThrowDateOverflow();

        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate NextDay()
    {
        if (this == MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch + 1);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate PreviousDay()
    {
        if (this == MinValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch - 1);
    }
}
