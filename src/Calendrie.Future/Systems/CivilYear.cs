// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;

public partial struct CivilYear // Preamble
{
    /// <summary>Represents the maximum value of <see cref="_yearsSinceEpoch"/>.
    /// <para>This field is a constant equal to 9998.</para></summary>
    private const int MaxYearsSinceEpoch = CivilScope.MaxYear - 1;

    /// <summary>
    /// Represents the count of consecutive years since the Gregorian epoch.
    /// <para>This field is in the range from 0 to <see cref="MaxYearsSinceEpoch"/>.
    /// </para>
    /// </summary>
    private readonly ushort _yearsSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilYear"/> struct to the
    /// specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of years supported values.</exception>
    public CivilYear(int year)
    {
        if (year < CivilScope.MinYear || year > CivilScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year);

        _yearsSinceEpoch = (ushort)(year - 1);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilYear"/> struct.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    private CivilYear(ushort yearsSinceEpoch)
    {
        _yearsSinceEpoch = yearsSinceEpoch;
    }

    /// <summary>
    /// Gets the minimum possible value of <see cref="CivilYear"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The earliest supported year.</returns>
    //
    // MinValue = new(1) = new() = default(CivilYear)
    public static CivilYear MinValue { get; }

    /// <summary>
    /// Gets the maximum possible value of <see cref="CivilYear"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The latest supported year.</returns>
    public static CivilYear MaxValue { get; } = new((ushort)MaxYearsSinceEpoch);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static CivilCalendar Calendar => CivilCalendar.Instance;

    /// <summary>
    /// Gets the count of consecutive years since the Gregorian epoch.
    /// </summary>
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
    /// Gets the year number.
    /// </summary>
    //
    // Actually, this property returns the algebraic year, but since its value
    // is greater than 0, one can ignore this subtlety.
    public int Year => _yearsSinceEpoch + 1;

    /// <inheritdoc />
    public bool IsLeap => GregorianFormulae.IsLeapYear(Year);

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString() =>
        FormattableString.Invariant($"{Year:D4} ({CivilCalendar.DisplayName})");
}

public partial struct CivilYear
{
    /// <summary>
    /// Defines an implicit conversion of a <see cref="CivilYear"/> value to a
    /// <see cref="GregorianYear"/> value.
    /// </summary>
    public static implicit operator GregorianYear(CivilYear year) =>
        new(year._yearsSinceEpoch, default);

    /// <summary>
    /// Converts the current instance to a <see cref="GregorianMonth"/> value.
    /// </summary>
    [Pure]
    public GregorianYear ToGregorianYear() => new(_yearsSinceEpoch, default);
}

public partial struct CivilYear // IDateSegment
{
    /// <inheritdoc />
    public CivilDate MinDay
    {
        get
        {
            int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(Year, 1);
            return new CivilDate(daysSinceZero);
        }
    }

    /// <inheritdoc />
    public CivilDate MaxDay
    {
        get
        {
            int doy = GregorianFormulae.CountDaysInYear(Year);
            int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(Year, doy);
            return new CivilDate(daysSinceZero);
        }
    }

    /// <inheritdoc />
    /// <remarks>See also <seealso cref="CivilCalendar.CountDaysInYear(int)"/>.
    /// </remarks>
    [Pure]
    public int CountDays() => GregorianFormulae.CountDaysInYear(Year);

    /// <inheritdoc />
    /// <remarks>See also <seealso cref="CalendarSystem{TDate}.GetDaysInYear(int)"/>.
    /// </remarks>
    [Pure]
    public Range<CivilDate> ToDayRange()
    {
        int daysInYear = GregorianFormulae.CountDaysInYear(Year);
        return Range.StartingAt(MinDay, daysInYear);
    }

    /// <inheritdoc />
    [Pure]
    public IEnumerable<CivilDate> EnumerateDays()
    {
        int startOfYear = CivilFormulae.CountDaysSinceEpoch(Year, 1);
        int daysInYear = GregorianFormulae.CountDaysInYear(Year);

        return from daysSinceZero
               in Enumerable.Range(startOfYear, daysInYear)
               select new CivilDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public bool Contains(CivilDate date) => date.Year == Year;

    /// <summary>
    /// Obtains the date corresponding to the specified day of this year instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfYear"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public CivilDate GetDayOfYear(int dayOfYear)
    {
        // We already know that "y" is valid, we only need to check "dayOfYear".
        Calendar.Scope.PreValidator.ValidateDayOfYear(Year, dayOfYear);
        int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(Year, dayOfYear);
        return new CivilDate(daysSinceZero);
    }
}
