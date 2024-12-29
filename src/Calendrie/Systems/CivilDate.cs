// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

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
#if TEMP_BCL_CODE
        _daysSinceZero = CountDaysSinceEpoch(year, month, day);
#else
        CivilScope.ValidateYearMonthDayImpl(year, month, day);

        _daysSinceZero = CivilFormulae.CountDaysSinceEpoch(year, month, day);
#endif
    }

#if TEMP_BCL_CODE
    private static ReadOnlySpan<short> DaysToMonth365 =>
        [0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365];
    private static ReadOnlySpan<short> DaysToMonth366 =>
        [0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366];

    private static int CountDaysSinceEpoch(int y, int m, int d)
    {
        if (y < StandardScope.MinYear || y > StandardScope.MaxYear || m < 1 || m > 12 || d < 1)
            throwAoorException();

        var days = IsLeapYear(y) ? DaysToMonth366 : DaysToMonth365;

        if (d > days[m] - days[m - 1]) throwAoorException();

        int y0 = y - 1;
        int c = y0 / 100;
        return (1461 * y0 >> 2) - c + (c >> 2) + days[m - 1] + d - 1;

        [DoesNotReturn]
        static void throwAoorException() =>
            throw new ArgumentOutOfRangeException(null, "Date was out of range.");
    }

    private static bool IsLeapYear(int y) =>
        //(y & 3) == 0 && (y % 100 != 0 || y % 400 == 0);
        (y & 3) == 0 && ((y & 15) == 0 || (uint)y % 25 != 0);
#endif

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
    [Pure]
    public GregorianDate ToGregorianDate() => new(_daysSinceZero);
}
