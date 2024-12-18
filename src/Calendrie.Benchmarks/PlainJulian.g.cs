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

using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;

using Calendrie;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;
using Calendrie.Systems;

/// <summary>
/// Represents the PlainJulian calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class PlainJulianCalendar : CalendarSystem<PlainJulianDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlainJulianCalendar"/> class.
    /// </summary>
    public PlainJulianCalendar() : this(new JulianSchema()) { }

    private PlainJulianCalendar(JulianSchema schema)
        : this(schema, new StandardScope(schema, DayZero.OldStyle)) { }

    private PlainJulianCalendar(JulianSchema schema, StandardScope scope)
        : base("PlainJulian", scope)
    {
        UnderlyingSchema = schema;
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="PlainJulianCalendar"/> class.
    /// <para>See also <seealso cref="PlainJulianDate.Calendar"/>.</para>
    /// </summary>
    public static PlainJulianCalendar Instance { get; } = new();

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
    internal JulianSchema UnderlyingSchema { get; }
}

/// <summary>
/// Represents the PlainJulian date.
/// <para><see cref="PlainJulianDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct PlainJulianDate :
    IDate<PlainJulianDate, PlainJulianCalendar>,
    IFixedDateFactory<PlainJulianDate>,
    ISubtractionOperators<PlainJulianDate, PlainJulianDate, int>
{ }

public partial struct PlainJulianDate // Preamble
{
    // WARNING: the order in which the static fields are written is __important__.

    /// <summary>Represents the epoch of the associated calendar.</summary>
    private static readonly DayNumber s_Epoch = PlainJulianCalendar.Instance.Epoch;

    /// <summary>Represents the minimum value of <see cref="_daysSinceEpoch"/>.</summary>
    private static readonly int s_MinDaysSinceEpoch = PlainJulianCalendar.Instance.MinDaysSinceEpoch;
    /// <summary>Represents the maximum value of <see cref="_daysSinceEpoch"/>.</summary>
    private static readonly int s_MaxDaysSinceEpoch = PlainJulianCalendar.Instance.MaxDaysSinceEpoch;

    /// <summary>Represents the minimum value of the current type.</summary>
    private static readonly PlainJulianDate s_MinValue = new(s_MinDaysSinceEpoch);
    /// <summary>Represents the maximum value of the current type.</summary>
    private static readonly PlainJulianDate s_MaxValue = new(s_MaxDaysSinceEpoch);

    /// <summary>
    /// Represents the count of consecutive days since the epoch <see cref="DayZero.OldStyle"/>.
    /// <para>This field is in the range from <see cref="s_MinDaysSinceEpoch"/>
    /// to <see cref="s_MaxDaysSinceEpoch"/>.</para>
    /// </summary>
    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlainJulianDate"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the range
    /// of supported years.</exception>
    public PlainJulianDate(int year, int month, int day)
    {
        var chr = PlainJulianCalendar.Instance;

        chr.Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlainJulianDate"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid ordinal date or <paramref name="year"/> is outside
    /// the range of supported years.</exception>
    public PlainJulianDate(int year, int dayOfYear)
    {
        var chr = PlainJulianCalendar.Instance;

        chr.Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// This constructor does NOT validate its parameter.
    /// </summary>
    internal PlainJulianDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static PlainJulianDate MinValue => s_MinValue;

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static PlainJulianDate MaxValue => s_MaxValue;

    /// <inheritdoc />
    public static PlainJulianCalendar Calendar => PlainJulianCalendar.Instance;

    /// <summary>
    /// Gets the adjuster of the current date type.
    /// <remarks>This static property is thread-safe.</remarks>
    /// </summary>
    public static DateAdjuster<PlainJulianDate> Adjuster => PlainJulianCalendar.Instance.Adjuster;

    /// <inheritdoc />
    //
    // We already know that the resulting day number is valid so instead of
    // > public DayNumber DayNumber => s_Epoch + _daysSinceEpoch;
    // we can use an unchecked addition
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
    public int Year => Calendar.UnderlyingSchema.GetYear(_daysSinceEpoch);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = Calendar.Schema.GetYear(_daysSinceEpoch, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
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
            sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
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
        chr.Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({chr})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        Calendar.Schema.GetDateParts(_daysSinceEpoch, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = Calendar.Schema.GetYear(_daysSinceEpoch, out dayOfYear);
}

public partial struct PlainJulianDate // Factories
{
    /// <inheritdoc />
    [Pure]
    public static PlainJulianDate FromDayNumber(DayNumber dayNumber)
    {
        Calendar.Scope.Validate(dayNumber);

        // We know that the subtraction won't overflow
        // > return new(dayNumber - s_Epoch);
        return new(dayNumber.DaysSinceZero - s_Epoch.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    static PlainJulianDate IFixedDateFactory<PlainJulianDate>.FromDayNumberUnchecked(DayNumber dayNumber) =>
        new(dayNumber.DaysSinceZero - s_Epoch.DaysSinceZero);

    /// <inheritdoc />
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    static PlainJulianDate IFixedDateFactory<PlainJulianDate>.FromDaysSinceEpochUnchecked(int daysSinceEpoch) =>
        new(daysSinceEpoch);
}

public partial struct PlainJulianDate // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() =>
        Calendar.UnderlyingSchema.CountDaysInYearBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() =>
        Calendar.UnderlyingSchema.CountDaysInYearAfter(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() =>
        Calendar.UnderlyingSchema.CountDaysInMonthBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() =>
        Calendar.UnderlyingSchema.CountDaysInMonthAfter(_daysSinceEpoch);
}

public partial struct PlainJulianDate // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public PlainJulianDate Adjust(Func<PlainJulianDate, PlainJulianDate> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        return adjuster.Invoke(this);
    }

    //
    // "Nondestructive mutation"
    //

    /// <inheritdoc />
    [Pure]
    public PlainJulianDate WithYear(int newYear) =>
        Calendar.Adjuster.AdjustYear(this, newYear);

    /// <inheritdoc />
    [Pure]
    public PlainJulianDate WithMonth(int newMonth) =>
        Calendar.Adjuster.AdjustMonth(this, newMonth);

    /// <inheritdoc />
    [Pure]
    public PlainJulianDate WithDay(int newDay) =>
        Calendar.Adjuster.AdjustDay(this, newDay);

    /// <inheritdoc />
    [Pure]
    public PlainJulianDate WithDayOfYear(int newDayOfYear) =>
        Calendar.Adjuster.AdjustDayOfYear(this, newDayOfYear);

    //
    // Adjust the day of the week
    //

    /// <inheritdoc />
    [Pure]
    public PlainJulianDate Previous(DayOfWeek dayOfWeek) =>
        Calendar.Adjuster.Previous(this, dayOfWeek);

    /// <inheritdoc />
    [Pure]
    public PlainJulianDate PreviousOrSame(DayOfWeek dayOfWeek) =>
        Calendar.Adjuster.PreviousOrSame(this, dayOfWeek);

    /// <inheritdoc />
    [Pure]
    public PlainJulianDate Nearest(DayOfWeek dayOfWeek) =>
        Calendar.Adjuster.Nearest(this, dayOfWeek);

    /// <inheritdoc />
    [Pure]
    public PlainJulianDate NextOrSame(DayOfWeek dayOfWeek) =>
        Calendar.Adjuster.NextOrSame(this, dayOfWeek);

    /// <inheritdoc />
    [Pure]
    public PlainJulianDate Next(DayOfWeek dayOfWeek) =>
        Calendar.Adjuster.Next(this, dayOfWeek);
}

public partial struct PlainJulianDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(PlainJulianDate left, PlainJulianDate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(PlainJulianDate left, PlainJulianDate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(PlainJulianDate other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is PlainJulianDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct PlainJulianDate // IComparable
{
    /// <inheritdoc />
    public static bool operator <(PlainJulianDate left, PlainJulianDate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator <=(PlainJulianDate left, PlainJulianDate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >(PlainJulianDate left, PlainJulianDate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >=(PlainJulianDate left, PlainJulianDate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static PlainJulianDate Min(PlainJulianDate x, PlainJulianDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static PlainJulianDate Max(PlainJulianDate x, PlainJulianDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(PlainJulianDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is PlainJulianDate date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(PlainJulianDate), obj);
}

public partial struct PlainJulianDate // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    public static int operator -(PlainJulianDate left, PlainJulianDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static PlainJulianDate operator +(PlainJulianDate value, int days) => value.AddDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static PlainJulianDate operator -(PlainJulianDate value, int days) => value.AddDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    public static PlainJulianDate operator ++(PlainJulianDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    public static PlainJulianDate operator --(PlainJulianDate value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(PlainJulianDate other) =>
        // No need to use a checked context here.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public PlainJulianDate AddDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);

        // Don't write (the addition may also overflow...):
        // > Scope.CheckOverflow(Epoch + daysSinceEpoch);
        if (daysSinceEpoch < s_MinDaysSinceEpoch || daysSinceEpoch > s_MaxDaysSinceEpoch)
            ThrowHelpers.ThrowDateOverflow();

        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public PlainJulianDate NextDay()
    {
        if (this == s_MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch + 1);
    }

    /// <inheritdoc />
    [Pure]
    public PlainJulianDate PreviousDay()
    {
        if (this == s_MinValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch - 1);
    }
}

