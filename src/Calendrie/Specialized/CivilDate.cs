﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

public partial struct CivilDate
{
    private static readonly CivilDate s_MinValue = new(CivilScope.MinDaysSinceZero);
    private static readonly CivilDate s_MaxValue = new(CivilScope.MaxDaysSinceZero);

    private readonly int _daysSinceZero;

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilDate"/> struct to the
    /// specified date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid date or <paramref name="year"/> is outside the range of supported
    /// years.</exception>
    public CivilDate(int year, int month, int day)
    {
        CivilScope.ValidateYearMonthDayImpl(year, month, day);

        _daysSinceZero = CivilFormulae.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilDate"/> struct to the
    /// specified ordinal date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid ordinal date or <paramref name="year"/> is outside the range of
    /// supported years.</exception>
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
    public static CivilDate MinValue => s_MinValue;

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static CivilDate MaxValue => s_MaxValue;

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static CivilAdjuster Adjuster => CivilCalendar.Instance.Adjuster;

    /// <inheritdoc />
    public static CivilCalendar Calendar => CivilCalendar.Instance;

    /// <inheritdoc />
    public DayNumber DayNumber => new(_daysSinceZero);

    /// <summary>
    /// Gets the count of days since the Gregorian epoch.
    /// </summary>
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

    /// <inheritdoc />
    public bool IsSupplementary => false;

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    private static CivilSchema Schema => CivilCalendar.SchemaT;

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

public partial struct CivilDate // Factories
{
    /// <inheritdoc />
    [Pure]
    public static CivilDate FromDayNumber(DayNumber dayNumber)
    {
        int daysSinceZero = dayNumber.DaysSinceZero;

        if (unchecked((uint)daysSinceZero) > (uint)CivilScope.MaxDaysSinceZero)
            throw new AoorException(nameof(dayNumber));

        return new(daysSinceZero);
    }
}

public partial struct CivilDate // Adjustments
{
    /// <inheritdoc />
    /// <remarks>See also <seealso cref="CivilCalendar.Adjuster"/>.</remarks>
    [Pure]
    public CivilDate Adjust(Func<CivilDate, CivilDate> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        return adjuster.Invoke(this);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate Previous(DayOfWeek dayOfWeek)
    {
        int daysSinceZero = DayNumber.Previous(dayOfWeek).DaysSinceZero;
        if (daysSinceZero < 0) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        int daysSinceZero = DayNumber.PreviousOrSame(dayOfWeek).DaysSinceZero;
        if (daysSinceZero < 0) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate Nearest(DayOfWeek dayOfWeek)
    {
        int daysSinceZero = DayNumber.Nearest(dayOfWeek).DaysSinceZero;

        if ((uint)daysSinceZero > (uint)CivilScope.MaxDaysSinceZero)
            ThrowHelpers.ThrowDateOverflow();

        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate NextOrSame(DayOfWeek dayOfWeek)
    {
        int daysSinceZero = DayNumber.NextOrSame(dayOfWeek).DaysSinceZero;
        if (daysSinceZero > CivilScope.MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate Next(DayOfWeek dayOfWeek)
    {
        int daysSinceZero = DayNumber.Next(dayOfWeek).DaysSinceZero;
        if (daysSinceZero > CivilScope.MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }
}

public partial struct CivilDate // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    public static int operator -(CivilDate left, CivilDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static CivilDate operator +(CivilDate value, int days) => value.AddDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static CivilDate operator -(CivilDate value, int days) => value.AddDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    public static CivilDate operator ++(CivilDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    public static CivilDate operator --(CivilDate value) => value.PreviousDay();

#pragma warning restore CA2225

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

        // Don't write (the addition may also overflow...):
        // > s_Domain.CheckOverflow(s_Epoch + daysSinceEpoch);
        if ((uint)daysSinceZero > (uint)CivilScope.MaxDaysSinceZero)
            ThrowHelpers.ThrowDateOverflow();

        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate NextDay()
    {
        if (this == s_MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceZero + 1);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate PreviousDay()
    {
        if (this == s_MinValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceZero - 1);
    }
}
