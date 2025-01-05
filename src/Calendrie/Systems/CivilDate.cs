// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

public partial struct CivilDate // Preamble
{
    /// <summary>Represents the maximum value of <see cref="_daysSinceZero"/>.
    /// <para>This field is a constant equal to 3_652_058.</para></summary>
    internal const int MaxDaysSinceZero = 3_652_058;

    /// <summary>
    /// Represents the count of consecutive days since the epoch
    /// <see cref="DayZero.NewStyle"/>.
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

    /// <summary>
    /// Gets the earliest possible value of a <see cref="CivilDate"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    //
    // MinValue = new(0) = new() = default(CivilDate)
    public static CivilDate MinValue { get; }

    /// <summary>
    /// Gets the latest possible value of a <see cref="CivilDate"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
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
    [Pure]
    public GregorianDate ToGregorianDate() => new(_daysSinceZero);
}

public partial struct CivilDate // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public CivilDate WithYear(int newYear)
    {
        CivilFormulae.GetDateParts(_daysSinceZero, out _, out int m, out int d);

        // We MUST re-validate the entire date.
        CivilScope.ValidateYearMonthDayImpl(newYear, m, d, nameof(newYear));

        int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(newYear, m, d);
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate WithMonth(int newMonth)
    {
        CivilFormulae.GetDateParts(_daysSinceZero, out int y, out _, out int d);

        // We only need to validate "newMonth" and "d".
        Calendar.Scope.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(y, newMonth, d);
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate WithDay(int newDay)
    {
        CivilFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out _);

        // We only need to validate "newDay".
        Calendar.Scope.PreValidator.ValidateDayOfMonth(y, m, newDay, nameof(newDay));

        int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(y, m, newDay);
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate WithDayOfYear(int newDayOfYear)
    {
        int y = CivilFormulae.GetYear(_daysSinceZero);

        // We only need to validate "newDayOfYear".
        Calendar.Scope.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(y, newDayOfYear);
        return new(daysSinceZero);
    }
}

public partial struct CivilDate // Non-standard math ops
{
    /// <summary>
    /// Adds a number of years to the year field of this date instance, yielding
    /// a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public CivilDate PlusYears(int years)
    {
        CivilFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return AddYears(y, m, d, years);
    }

    /// <summary>
    /// Adds a number of months to the month field of this date instance,
    /// yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public CivilDate PlusMonths(int months)
    {
        CivilFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return AddMonths(y, m, d, months);
    }

    /// <summary>
    /// Counts the number of years elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountYearsSince(CivilDate other)
    {
        CivilFormulae.GetDateParts(other._daysSinceZero, out int y0, out int m0, out int d0);

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
    public int CountMonthsSince(CivilDate other)
    {
        CivilFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out _);
        CivilFormulae.GetDateParts(other._daysSinceZero, out int y0, out int m0, out int d0);

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
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private static CivilDate AddYears(int y, int m, int d, int years)
    {
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < CivilScope.MinYear || newY > CivilScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, GregorianFormulae.CountDaysInMonth(newY, m));

        int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(newY, m, newD);
        return new CivilDate(daysSinceZero);
    }

    /// <summary>
    /// Adds a number of months to the month field of the specified date,
    /// yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private static CivilDate AddMonths(int y, int m, int d, int months)
    {
        // Exact addition of months to a calendar month.
        int newM = 1 + MathZ.Modulo(
            checked(m - 1 + months), GJSchema.MonthsInYear, out int y0);
        int newY = checked(y + y0);
        if (newY < CivilScope.MinYear || newY > CivilScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, GregorianFormulae.CountDaysInMonth(newY, newM));

        int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(newY, newM, newD);
        return new CivilDate(daysSinceZero);
    }
}

