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
    /// Gets the year number.
    /// <para>This property represents the algebraic year, but since it's greater
    /// than 0, there is no difference between the algebraic year and the year
    /// of the era.</para>
    /// </summary>
    public int Number => _yearsSinceEpoch + 1;

    /// <inheritdoc />
    public bool IsLeap => GregorianFormulae.IsLeapYear(Number);

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString() => FormattableString.Invariant($"{Number:D4} ({Calendar})");
}

public partial struct CivilYear // IDateSegment
{
    /// <inheritdoc />
    public CivilDate MinDay
    {
        get
        {
            int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(Number, 1);
            return new CivilDate(daysSinceZero);
        }
    }

    /// <inheritdoc />
    public CivilDate MaxDay
    {
        get
        {
            int doy = GregorianFormulae.CountDaysInYear(Number);
            int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(Number, doy);
            return new CivilDate(daysSinceZero);
        }
    }

    /// <inheritdoc />
    /// <remarks>See also <see cref="CalendarSystem{TDate}.CountDaysInYear(int)"/>.
    /// </remarks>
    [Pure]
    public int CountDays() => GregorianFormulae.CountDaysInYear(Number);

    /// <inheritdoc />
    /// <remarks>See also <see cref="CalendarSystem{TDate}.GetDaysInYear(int)"/>.
    /// </remarks>
    [Pure]
    public Range<CivilDate> ToDayRange()
    {
        int daysInYear = GregorianFormulae.CountDaysInYear(Number);
        return Range.StartingAt(MinDay, daysInYear);
    }

    /// <inheritdoc />
    [Pure]
    public IEnumerable<CivilDate> EnumerateDays()
    {
        int startOfYear = GregorianFormulae.CountDaysSinceEpoch(Number, 1);
        int daysInYear = GregorianFormulae.CountDaysInYear(Number);

        return from daysSinceZero
               in Enumerable.Range(startOfYear, daysInYear)
               select new CivilDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public bool Contains(CivilDate date) => date.Year == Number;

    /// <summary>
    /// Obtains the date corresponding to the specified day of this year instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfYear"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public CivilDate GetDayOfYear(int dayOfYear)
    {
        // We already know that "y" is valid, we only need to check "dayOfYear".
        Calendar.Scope.PreValidator.ValidateDayOfYear(Number, dayOfYear);
        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(Number, dayOfYear);
        return new CivilDate(daysSinceZero);
    }
}
