// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

public partial struct CivilMonth // Preamble
{
    /// <summary>Represents the maximum value of <see cref="_monthsSinceEpoch"/>.
    /// <para>This field is a constant equal to 119_987.</para></summary>
    internal const int MaxMonthsSinceEpoch = 119_987;

    /// <summary>
    /// Represents the count of consecutive months since the Gregorian epoch.
    /// <para>This field is in the range from 0 to <see cref="MaxMonthsSinceEpoch"/>.
    /// </para>
    /// </summary>
    private readonly int _monthsSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilMonth"/> struct to the
    /// specified month components.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid month or <paramref name="year"/> is outside the
    /// range of years.</exception>
    public CivilMonth(int year, int month)
    {
        // The calendar being regular, no need to use the Scope:
        // > CivilScope.ValidateYearMonthImpl(year, month);
        if (year < CivilScope.MinYear || year > CivilScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year);
        if (month < 1 || month > GJSchema.MonthsPerYear)
            ThrowHelpers.ThrowMonthOutOfRange(month);

        _monthsSinceEpoch = CountMonthsSinceEpoch(year, month);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilMonth"/> struct
    /// from the specified <see cref="CivilDate"/> value.
    /// </summary>
    public CivilMonth(CivilDate date)
    {
        var (y, m, _) = date;
        _monthsSinceEpoch = CountMonthsSinceEpoch(y, m);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilMonth"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal CivilMonth(int monthsSinceEpoch)
    {
        _monthsSinceEpoch = monthsSinceEpoch;
    }

    /// <summary>
    /// Gets the earliest possible value of <see cref="CivilMonth"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The earliest supported month.</returns>
    //
    // MinValue = new(0) = new() = default(CivilMonth)
    public static CivilMonth MinValue { get; }

    /// <summary>
    /// Gets the latest possible value of <see cref="CivilMonth"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The latest supported month.</returns>
    public static CivilMonth MaxValue { get; } = new(MaxMonthsSinceEpoch);

    /// <summary>
    /// Gets the companion calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static CivilCalendar Calendar => CivilCalendar.Instance;

    static Calendar IMonth.Calendar => Calendar;

    /// <summary>
    /// Gets the count of consecutive months since the Gregorian epoch.
    /// </summary>
    public int MonthsSinceEpoch => _monthsSinceEpoch;

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
    public int Year =>
        // NB: both dividend and divisor are >= 0.
        1 + _monthsSinceEpoch / GJSchema.MonthsPerYear;

    /// <inheritdoc />
    public int Month
    {
        get
        {
            var (_, m) = this;
            return m;
        }
    }

    /// <inheritdoc />
    bool IMonth.IsIntercalary => false;

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        var (y, m) = this;
        return FormattableString.Invariant($"{m:D2}/{y:D4} ({CivilCalendar.DisplayName})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month)
    {
        // See RegularSchema.GetMonthParts().
        // NB: both dividend and divisor are >= 0.
        year = 1 + MathN.Divide(_monthsSinceEpoch, GJSchema.MonthsPerYear, out int m0);
        month = 1 + m0;
    }
}

public partial struct CivilMonth // Conversions
{
    /// <summary>
    /// Defines an implicit conversion of a <see cref="CivilMonth"/> value to a
    /// <see cref="GregorianMonth"/> value.
    /// </summary>
    public static implicit operator GregorianMonth(CivilMonth month) => new(month._monthsSinceEpoch);

    /// <summary>
    /// Converts the current instance to a <see cref="GregorianMonth"/> value.
    /// </summary>
    [Pure]
    public GregorianMonth ToGregorianMonth() => new(_monthsSinceEpoch);
}

public partial struct CivilMonth // IDaySegment
{
    /// <inheritdoc />
    public CivilDate MinDay
    {
        get
        {
            var (y, m) = this;
            int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(y, m, 1);
            return CivilDate.UnsafeCreate(daysSinceZero);
        }
    }

    /// <inheritdoc />
    public CivilDate MaxDay
    {
        get
        {
            var (y, m) = this;
            int d = GregorianFormulae.CountDaysInMonth(y, m);
            int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(y, m, d);
            return CivilDate.UnsafeCreate(daysSinceZero);
        }
    }

    /// <inheritdoc />
    [Pure]
    public int CountDays()
    {
        var (y, m) = this;
        return GregorianFormulae.CountDaysInMonth(y, m);
    }

    /// <summary>
    /// Converts the current instance to a range of days.
    /// </summary>
    [Pure]
    public Segment<CivilDate> ToRange()
    {
        var (y, m) = this;
        int daysInMonth = GregorianFormulae.CountDaysInMonth(y, m);
        return Range.StartingAt(MinDay, daysInMonth);
    }

    [Pure]
    Segment<CivilDate> IDaySegment<CivilDate>.ToDayRange() => ToRange();

    /// <summary>
    /// Returns an enumerable collection of all days in this month instance.
    /// </summary>
    [Pure]
    public IEnumerable<CivilDate> ToEnumerable()
    {
        var (y, m) = this;
        int startOfMonth = CivilFormulae.CountDaysSinceEpoch(y, m, 1);
        int daysInMonth = GregorianFormulae.CountDaysInMonth(y, m);

        return from daysSinceZero
               in Enumerable.Range(startOfMonth, daysInMonth)
               select CivilDate.UnsafeCreate(daysSinceZero);
    }

    [Pure]
    IEnumerable<CivilDate> IDaySegment<CivilDate>.EnumerateDays() => ToEnumerable();

    /// <inheritdoc />
    [Pure]
    public bool Contains(CivilDate date)
    {
        var (y, m) = this;
        CivilFormulae.GetDateParts(date.DaysSinceZero, out int y1, out int m1, out _);
        return y1 == y && m1 == m;
    }

    /// <summary>
    /// Obtains the date corresponding to the specified day of this month
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfMonth"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public CivilDate GetDayOfMonth(int dayOfMonth)
    {
        var (y, m) = this;
        Calendar.Scope.PreValidator.ValidateDayOfMonth(y, m, dayOfMonth);
        int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(y, m, dayOfMonth);
        return CivilDate.UnsafeCreate(daysSinceZero);
    }
}
