// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

public partial struct GregorianDate
{
    // WARNING: the order in which the static fields are written is __important__.

    private static readonly GregorianSchema s_Schema = new();
    private static readonly GregorianScope s_Scope = new(s_Schema);
    private static readonly GregorianCalendar s_Calendar = new(s_Scope);

    private static readonly Range<DayNumber> s_Domain = s_Scope.Domain;
    private static readonly Range<int> s_SupportedDays = s_Scope.Segment.SupportedDays;

    private static readonly GregorianDate s_MinValue = new(s_Domain.Min.DaysSinceZero);
    private static readonly GregorianDate s_MaxValue = new(s_Domain.Max.DaysSinceZero);

    private readonly int _daysSinceZero;

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianDate"/> struct to
    /// the specified date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid date or <paramref name="year"/> is outside the range of supported
    /// years.</exception>
    public GregorianDate(int year, int month, int day)
    {
        GregorianScope.ValidateYearMonthDayImpl(year, month, day);

        _daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianDate"/> struct to
    /// the specified ordinal date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid ordinal date or <paramref name="year"/> is outside the range of
    /// supported years.</exception>
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
    public static GregorianDate MinValue => s_MinValue;

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static GregorianDate MaxValue => s_MaxValue;

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static GregorianAdjuster Adjuster => s_Calendar.Adjuster;

    /// <inheritdoc />
    public static GregorianCalendar Calendar => s_Calendar;

    /// <inheritdoc />
    public DayNumber DayNumber => new(_daysSinceZero);

    /// <summary>Gets the count of days since the Gregorian epoch.</summary>
    public int DaysSinceZero => _daysSinceZero;

    int IFixedDate.DaysSinceEpoch => _daysSinceZero;

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

    /// <inheritdoc />
    public bool IsSupplementary => false;

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        GregorianFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({s_Calendar})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        GregorianFormulae.GetDateParts(_daysSinceZero, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = GregorianFormulae.GetYear(_daysSinceZero, out dayOfYear);
}
