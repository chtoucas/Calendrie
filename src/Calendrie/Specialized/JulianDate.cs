// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

public partial struct JulianDate
{
    private static readonly DayNumber s_Epoch = JulianScope.Instance.Epoch;
    private static readonly Range<DayNumber> s_Domain = JulianScope.Instance.Domain;

    private static readonly int s_MinDaysSinceEpoch = JulianScope.MinDaysSinceEpoch;
    private static readonly int s_MaxDaysSinceEpoch = JulianScope.MaxDaysSinceEpoch;

    private static readonly JulianDate s_MinValue = new(JulianScope.MinDaysSinceEpoch);
    private static readonly JulianDate s_MaxValue = new(JulianScope.MaxDaysSinceEpoch);

    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianDate"/> struct to the
    /// specified date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid date or <paramref name="year"/> is outside the range of supported
    /// years.</exception>
    public JulianDate(int year, int month, int day)
    {
        JulianScope.ValidateYearMonthDayImpl(year, month, day);

        _daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianDate"/> struct to the
    /// specified ordinal date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid ordinal date or <paramref name="year"/> is outside the range of
    /// supported years.</exception>
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
    public static JulianDate MinValue => s_MinValue;

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static JulianDate MaxValue => s_MaxValue;

    /// <inheritdoc />
    public static JulianCalendar Calendar => JulianCalendar.Instance;

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static JulianAdjuster Adjuster => JulianCalendar.Instance.Adjuster;

    /// <inheritdoc />
    public DayNumber DayNumber => new(s_Epoch.DaysSinceZero + _daysSinceEpoch);

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
    private static JulianSchema Schema => JulianScope.SchemaT;

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
