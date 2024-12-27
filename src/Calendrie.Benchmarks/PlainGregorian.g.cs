﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable

namespace Benchmarks;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Numerics;

using Calendrie;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;
using Calendrie.Systems;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Represents the PlainGregorian calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class PlainGregorianCalendar : CalendarSystem<PlainGregorianDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlainGregorianCalendar"/> class.
    /// </summary>
    public PlainGregorianCalendar() : this(new GregorianSchema()) { }

    private PlainGregorianCalendar(GregorianSchema schema)
        : base("PlainGregorian", new StandardScope(schema, DayZero.NewStyle))
    {
        Debug.Assert(schema != null);
        Schema = schema;
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="PlainGregorianCalendar"/> class.
    /// <para>See <see cref="PlainGregorianDate.Calendar"/>.</para>
    /// </summary>
    internal static PlainGregorianCalendar Instance { get; } = new();

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    public static int MinYear => StandardScope.MinYear;

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public static int MaxYear => StandardScope.MaxYear;

    /// <summary>
    /// Gets the schema.
    /// </summary>
    internal GregorianSchema Schema { get; }
}

/// <summary>
/// Represents the PlainGregorian date.
/// <para><see cref="PlainGregorianDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct PlainGregorianDate :
    IDateable,
    IAbsoluteDate<PlainGregorianDate>,
    IAdjustableDate<PlainGregorianDate>,
    IDateFactory<PlainGregorianDate>,
    ISubtractionOperators<PlainGregorianDate, PlainGregorianDate, int>
{ }

public partial struct PlainGregorianDate // Preamble
{
    /// <summary>Represents the maximum value of <see cref="_daysSinceZero"/>.
    /// <para>This field is a constant equal to 3_652_058.</para></summary>
    private const int MaxDaysSinceZero = 3_652_058;

    /// <summary>
    /// Represents the count of consecutive days since <see cref="DayZero.NewStyle"/>.
    /// <para>This field is in the range from 0 to <see cref="MaxDaysSinceZero"/>.
    /// </para>
    /// </summary>
    private readonly int _daysSinceZero;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlainGregorianDate"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the range
    /// of supported years.</exception>
    public PlainGregorianDate(int year, int month, int day)
    {
        var chr = PlainGregorianCalendar.Instance;
        chr.Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceZero = chr.Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlainGregorianDate"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid ordinal date or <paramref name="year"/> is outside
    /// the range of supported years.</exception>
    public PlainGregorianDate(int year, int dayOfYear)
    {
        var chr = PlainGregorianCalendar.Instance;
        chr.Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceZero = chr.Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// This constructor does NOT validate its parameter.
    /// </summary>
    internal PlainGregorianDate(int daysSinceZero)
    {
        _daysSinceZero = daysSinceZero;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static PlainGregorianDate MinValue { get; }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static PlainGregorianDate MaxValue { get; } = new(MaxDaysSinceZero);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static PlainGregorianCalendar Calendar => PlainGregorianCalendar.Instance;

    /// <inheritdoc />
    public DayNumber DayNumber => new(_daysSinceZero);

    /// <summary>Gets the count of days since the Gregorian epoch.</summary>
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
    public int Year => Calendar.Schema.GetYear(_daysSinceZero);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            Calendar.Schema.GetDateParts(_daysSinceZero, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = Calendar.Schema.GetYear(_daysSinceZero, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            Calendar.Schema.GetDateParts(_daysSinceZero, out _, out _, out int d);
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
            var sch = Calendar.Schema;
            sch.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
            return sch.IsIntercalaryDay(y, m, d);
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
        var chr = Calendar;
        chr.Schema.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({chr})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        Calendar.Schema.GetDateParts(_daysSinceZero, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = Calendar.Schema.GetYear(_daysSinceZero, out dayOfYear);
}

public partial struct PlainGregorianDate // Factories & conversions
{
    /// <inheritdoc />
    [Pure]
    public static PlainGregorianDate FromDayNumber(DayNumber dayNumber)
    {
        Calendar.Scope.Validate(dayNumber);

        return new(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    static PlainGregorianDate IDateFactory<PlainGregorianDate>.UnsafeCreate(int daysSinceEpoch) =>
        new(daysSinceEpoch);
}

public partial struct PlainGregorianDate // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() => Calendar.Schema.CountDaysInYearBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() => Calendar.Schema.CountDaysInYearAfter(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => Calendar.Schema.CountDaysInMonthBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() => Calendar.Schema.CountDaysInMonthAfter(_daysSinceZero);
}

public partial struct PlainGregorianDate // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate WithYear(int newYear)
    {
        var (_, m, d) = this;

        var chr = Calendar;
        // We MUST re-validate the entire date.
        chr.Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));

        int daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(newYear, m, d);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate WithMonth(int newMonth)
    {
        var (y, _, d) = this;

        var sch = Calendar.Schema;
        // We only need to validate "newMonth" and "d".
        sch.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceZero = sch.CountDaysSinceEpoch(y, newMonth, d);
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate WithDay(int newDay)
    {
        var (y, m, _) = this;

        var sch = Calendar.Schema;
        // We only need to validate "newDay".
        sch.PreValidator.ValidateDayOfMonth(y, m, newDay, nameof(newDay));

        int daysSinceZero = sch.CountDaysSinceEpoch(y, m, newDay);
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate WithDayOfYear(int newDayOfYear)
    {
        int y = Year;

        var sch = Calendar.Schema;
        // We only need to validate "newDayOfYear".
        sch.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceZero = sch.CountDaysSinceEpoch(y, newDayOfYear);
        return new(daysSinceZero);
    }
}

public partial struct PlainGregorianDate // Find close by day of the week
{
    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceZero = _daysSinceZero + (δ >= 0 ? δ - DaysInWeek : δ);
        if (daysSinceZero < 0) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceZero = _daysSinceZero + (δ > 0 ? δ - DaysInWeek : δ);
        if (daysSinceZero < 0) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate Nearest(DayOfWeek dayOfWeek)
    {
        int daysSinceZero = DayNumber.Nearest(dayOfWeek).DaysSinceZero;
        if ((uint)daysSinceZero > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate NextOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceZero = _daysSinceZero + (δ < 0 ? δ + DaysInWeek : δ);
        if (daysSinceZero > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceZero = _daysSinceZero + (δ <= 0 ? δ + DaysInWeek : δ);
        if (daysSinceZero > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }
}

public partial struct PlainGregorianDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(PlainGregorianDate left, PlainGregorianDate right) =>
        left._daysSinceZero == right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator !=(PlainGregorianDate left, PlainGregorianDate right) =>
        left._daysSinceZero != right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public bool Equals(PlainGregorianDate other) => _daysSinceZero == other._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is PlainGregorianDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceZero;
}

public partial struct PlainGregorianDate // IComparable
{
    /// <inheritdoc />
    public static bool operator <(PlainGregorianDate left, PlainGregorianDate right) =>
        left._daysSinceZero < right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator <=(PlainGregorianDate left, PlainGregorianDate right) =>
        left._daysSinceZero <= right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator >(PlainGregorianDate left, PlainGregorianDate right) =>
        left._daysSinceZero > right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator >=(PlainGregorianDate left, PlainGregorianDate right) =>
        left._daysSinceZero >= right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public static PlainGregorianDate Min(PlainGregorianDate x, PlainGregorianDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static PlainGregorianDate Max(PlainGregorianDate x, PlainGregorianDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(PlainGregorianDate other) => _daysSinceZero.CompareTo(other._daysSinceZero);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is PlainGregorianDate date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(PlainGregorianDate), obj);
}

public partial struct PlainGregorianDate // Math
{
    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountDaysSince()")]
    public static int operator -(PlainGregorianDate left, PlainGregorianDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See AddDays()")]
    public static PlainGregorianDate operator +(PlainGregorianDate value, int days) => value.AddDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See AddDays()")]
    public static PlainGregorianDate operator -(PlainGregorianDate value, int days) => value.AddDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextDay()")]
    public static PlainGregorianDate operator ++(PlainGregorianDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousDay()")]
    public static PlainGregorianDate operator --(PlainGregorianDate value) => value.PreviousDay();

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(PlainGregorianDate other) =>
        // No need to use a checked context here.
        _daysSinceZero - other._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate AddDays(int days)
    {
        int daysSinceZero = checked(_daysSinceZero + days);

        // Don't write (the addition may also overflow...):
        // > Scope.CheckOverflow(Epoch + daysSinceZero);
        if ((uint)daysSinceZero > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();

        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate NextDay()
    {
        if (this == MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceZero + 1);
    }

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate PreviousDay()
    {
        if (this == MinValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceZero - 1);
    }
}

