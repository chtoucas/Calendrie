// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Represents the Julian date.
/// <para><i>All</i> dates within the range [-999_998..999_999] of years are
/// supported.</para>
/// <para><see cref="JulianDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct JulianDate :
    ICalendarDate<JulianDate>,
    ICalendarBound<JulianCalendar>,
    IDateFactory<JulianDate>,
    ISubtractionOperators<JulianDate, JulianDate, int>
{ }

public partial struct JulianDate // Preamble
{
    private const int EpochDaysSinceZero = -2;

    /// <summary>Represents the minimum value of <see cref="_daysSinceEpoch"/>.
    /// <para>This field is a constant equal to -365_249_635.</para></summary>
    private const int MinDaysSinceEpoch = -365_249_635;
    /// <summary>Represents the maximum value of <see cref="_daysSinceEpoch"/>.
    /// <para>This field is a constant equal to -365_249_633.</para></summary>
    private const int MaxDaysSinceEpoch = 365_249_633;

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

    /// <summary>
    /// Gets the earliest possible value of a <see cref="JulianDate"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static JulianDate MinValue { get; } = new(MinDaysSinceEpoch);

    /// <summary>
    /// Gets the latest possible value of a <see cref="JulianDate"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static JulianDate MaxValue { get; } = new(MaxDaysSinceEpoch);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static JulianCalendar Calendar => JulianCalendar.Instance;

    /// <inheritdoc />
    public DayNumber DayNumber => new(EpochDaysSinceZero + _daysSinceEpoch);

    /// <inheritdoc />
    public int DaysSinceEpoch => _daysSinceEpoch;

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

    bool IDateable.IsSupplementary => false;

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        JulianFormulae.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return y > 0 ? FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({Calendar})")
            : FormattableString.Invariant($"{d:D2}/{m:D2}/{getBCEYear(y)} BCE ({Calendar})");

        [Pure]
        static int getBCEYear(int y)
        {
            Debug.Assert(y <= 0);
            var (pos, _) = Ord.FromInt32(y);
            return pos;
        }
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        JulianFormulae.GetDateParts(_daysSinceEpoch, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = JulianFormulae.GetYear(_daysSinceEpoch, out dayOfYear);
}

public partial struct JulianDate // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public JulianDate WithYear(int newYear)
    {
        JulianFormulae.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);

        // We MUST re-validate the entire date.
        JulianScope.ValidateYearMonthDayImpl(newYear, m, d, nameof(newYear));

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(newYear, m, d);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate WithMonth(int newMonth)
    {
        JulianFormulae.GetDateParts(_daysSinceEpoch, out int y, out _, out int d);

        // We only need to validate "newMonth" and "d".
        Calendar.Scope.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(y, newMonth, d);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate WithDay(int newDay)
    {
        JulianFormulae.GetDateParts(_daysSinceEpoch, out int y, out int m, out _);

        // We only need to validate "newDay".
        Calendar.Scope.PreValidator.ValidateDayOfMonth(y, m, newDay, nameof(newDay));

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(y, m, newDay);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate WithDayOfYear(int newDayOfYear)
    {
        int y = JulianFormulae.GetYear(_daysSinceEpoch);

        // We only need to validate "newDayOfYear".
        Calendar.Scope.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(y, newDayOfYear);
        return new(daysSinceEpoch);
    }
}

public partial struct JulianDate // Find close by day of the week
{
    /// <inheritdoc />
    [Pure]
    public JulianDate Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceEpoch = _daysSinceEpoch + (δ >= 0 ? δ - DaysInWeek : δ);
        if (daysSinceEpoch < MinDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceEpoch = _daysSinceEpoch + (δ > 0 ? δ - DaysInWeek : δ); ;
        if (daysSinceEpoch < MinDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate Nearest(DayOfWeek dayOfWeek)
    {
        var nearest = DayNumber.Nearest(dayOfWeek);
        int daysSinceEpoch = nearest.DaysSinceZero - EpochDaysSinceZero;
        if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate NextOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceEpoch = _daysSinceEpoch + (δ < 0 ? δ + DaysInWeek : δ);
        if (daysSinceEpoch > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceEpoch = _daysSinceEpoch + (δ <= 0 ? δ + DaysInWeek : δ);
        if (daysSinceEpoch > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }
}

public partial struct JulianDate // Standard math ops
{
    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountDaysSince()")]
    public static int operator -(JulianDate left, JulianDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static JulianDate operator +(JulianDate value, int days) => value.PlusDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static JulianDate operator -(JulianDate value, int days) => value.PlusDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextDay()")]
    public static JulianDate operator ++(JulianDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousDay()")]
    public static JulianDate operator --(JulianDate value) => value.PreviousDay();

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(JulianDate other) =>
        // No need to use a checked context here. Indeed, the result is at most
        // equal to:
        //   MaxDaysSinceEpoch - MinDaysSinceEpoch
        //     = 365_249_633 - (-365_249_635)
        //     = 730_499_268 <= int.MaxValue
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public JulianDate PlusDays(int days)
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
        if (_daysSinceEpoch == MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch + 1);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate PreviousDay()
    {
        if (_daysSinceEpoch == MinDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch - 1);
    }
}

public partial struct JulianDate // Non-standard math ops
{
    /// <summary>
    /// Adds a number of years to the year field of this date instance, yielding
    /// a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public JulianDate PlusYears(int years)
    {
        JulianFormulae.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return AddYears(y, m, d, years);
    }

    /// <summary>
    /// Adds a number of months to the month field of this date instance,
    /// yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public JulianDate PlusMonths(int months)
    {
        JulianFormulae.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return AddMonths(y, m, d, months);
    }

    /// <summary>
    /// Counts the number of years elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountYearsSince(JulianDate other)
    {
        JulianFormulae.GetDateParts(other._daysSinceEpoch, out int y0, out int m0, out int d0);

        // Exact difference between two calendar years.
        int years = Year - y0;

        // To avoid extracting y0 twice, we inline:
        // > var newStart = other.PlusYears(years);
        var newStart = AddYears(y0, m0, d0, years);
        if (other < this)
        {
            if (newStart > this) years--;
        }
        else
        {
            if (newStart < this) years++;
        }

        return years;
    }

    /// <summary>
    /// Counts the number of months elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountMonthsSince(JulianDate other)
    {
        JulianFormulae.GetDateParts(_daysSinceEpoch, out int y, out int m, out _);
        JulianFormulae.GetDateParts(other._daysSinceEpoch, out int y0, out int m0, out int d0);

        // Exact difference between two calendar months.
        int months = checked(GJSchema.MonthsInYear * (y - y0) + m - m0);

        // To avoid extracting (y0, m0, d0) twice, we inline:
        // > var newStart = other.PlusMonths(months);
        var newStart = AddMonths(y0, m0, d0, months);

        if (other < this)
        {
            if (newStart > this) months--;
        }
        else
        {
            if (newStart < this) months++;
        }

        return months;
    }

    /// <summary>
    /// Adds a number of years to the year field of the specified date, yielding
    /// a new date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private static JulianDate AddYears(int y, int m, int d, int years)
    {
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < JulianScope.MinYear || newY > JulianScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, JulianFormulae.CountDaysInMonth(newY, m));

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(newY, m, newD);
        return new JulianDate(daysSinceEpoch);
    }

    /// <summary>
    /// Adds a number of months to the month field of the specified date,
    /// yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private static JulianDate AddMonths(int y, int m, int d, int months)
    {
        // Exact addition of months to a calendar month.
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), GJSchema.MonthsInYear, out int y0);
        int newY = checked(y + y0);
        if (newY < JulianScope.MinYear || newY > JulianScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, JulianFormulae.CountDaysInMonth(newY, newM));

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(newY, newM, newD);
        return new JulianDate(daysSinceEpoch);
    }
}
