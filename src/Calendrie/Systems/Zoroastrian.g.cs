﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Calendrie.Systems;

using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;

using Calendrie;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Represents the Zoroastrian calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class ZoroastrianCalendar : CalendarSystem<ZoroastrianDate>
{
    /// <summary>Represents the epoch.</summary>
    private static readonly DayNumber s_Epoch = DayZero.Zoroastrian;

    /// <summary>Represents a singleton instance of the schema.</summary>
    // This schema instance is the one used by:
    // - All instances of the ZoroastrianDate type via the property Schema
    // - ZoroastrianCalendar, custom methods only (see the file _Calendar.cs)
    internal static readonly Egyptian12Schema UnderlyingSchema = new();

    /// <summary>Represents a singleton instance of the scope.</summary>
    // This scope instance is the one used by:
    // - All instances of the ZoroastrianDate type via the property Scope
    internal static readonly StandardScope UnderlyingScope = CreateScope(new Egyptian12Schema());

    /// <summary>
    /// Initializes a new instance of the <see cref="ZoroastrianCalendar"/> class.
    /// <para>See also <seealso cref="ZoroastrianDate.Calendar"/>.</para>
    /// </summary>
    private ZoroastrianCalendar() : this(CreateScope(new Egyptian12Schema())) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ZoroastrianCalendar"/> class.
    /// </summary>
    private ZoroastrianCalendar(StandardScope scope) : base("Zoroastrian", scope)
    {
        Adjuster = new DateAdjuster<ZoroastrianDate>(this);
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="ZoroastrianCalendar"/> class.
    /// </summary>
    //
    // This calendar instance is the one used by:
    // - All instances of the ZoroastrianDate type via the properties Calendar and Adjuster
    public static ZoroastrianCalendar Instance => Singleton.Instance;

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    public static int MinYear => StandardScope.MinYear;

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public static int MaxYear => StandardScope.MaxYear;

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public DateAdjuster<ZoroastrianDate> Adjuster { get; }

    /// <summary>
    /// Creates a new instance of the <see href="StandardScope"/> class.
    /// </summary>
    private static StandardScope CreateScope(Egyptian12Schema schema) => new(schema, s_Epoch);

    private static class Singleton
    {
        static Singleton() { }

        internal static readonly ZoroastrianCalendar Instance = new();
    }
}

/// <summary>
/// Represents the Zoroastrian date.
/// <para><see cref="ZoroastrianDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct ZoroastrianDate :
    IDate<ZoroastrianDate, ZoroastrianCalendar>,
    IFixedDateFactory<ZoroastrianDate>,
    ISubtractionOperators<ZoroastrianDate, ZoroastrianDate, int>
{ }

public partial struct ZoroastrianDate // Preamble
{
    // WARNING: the order in which the static fields are written is __important__.

    private static readonly int s_EpochDaysSinceZero = ZoroastrianCalendar.UnderlyingScope.Epoch.DaysSinceZero;

    /// <summary>Represents the minimum value of <see cref="_daysSinceEpoch"/>.</summary>
    private static readonly int s_MinDaysSinceEpoch = ZoroastrianCalendar.UnderlyingScope.MinDaysSinceEpoch;
    /// <summary>Represents the maximum value of <see cref="_daysSinceEpoch"/>.</summary>
    private static readonly int s_MaxDaysSinceEpoch = ZoroastrianCalendar.UnderlyingScope.MaxDaysSinceEpoch;

    /// <summary>Represents the minimum value of the current type.</summary>
    private static readonly ZoroastrianDate s_MinValue = new(s_MinDaysSinceEpoch);
    /// <summary>Represents the maximum value of the current type.</summary>
    private static readonly ZoroastrianDate s_MaxValue = new(s_MaxDaysSinceEpoch);

    /// <summary>
    /// Represents the count of consecutive days since the epoch <see cref="DayZero.Zoroastrian"/>.
    /// <para>This field is in the range from <see cref="s_MinDaysSinceEpoch"/>
    /// to <see cref="s_MaxDaysSinceEpoch"/>.</para>
    /// </summary>
    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="ZoroastrianDate"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the range
    /// of supported years.</exception>
    public ZoroastrianDate(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ZoroastrianDate"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid ordinal date or <paramref name="year"/> is outside
    /// the range of supported years.</exception>
    public ZoroastrianDate(int year, int dayOfYear)
    {
        Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// This constructor does NOT validate its parameter.
    /// </summary>
    internal ZoroastrianDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static ZoroastrianDate MinValue => s_MinValue;

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static ZoroastrianDate MaxValue => s_MaxValue;

    /// <inheritdoc />
    public static ZoroastrianCalendar Calendar => ZoroastrianCalendar.Instance;

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static DateAdjuster<ZoroastrianDate> Adjuster => ZoroastrianCalendar.Instance.Adjuster;

    /// <inheritdoc />
    //
    // We already know that the resulting day number is valid so instead of
    // > public DayNumber DayNumber => s_Epoch + _daysSinceEpoch;
    // we can use an unchecked addition
    public DayNumber DayNumber => new(s_EpochDaysSinceZero + _daysSinceEpoch);

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
    public int Year => Schema.GetYear(_daysSinceEpoch);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = Schema.GetYear(_daysSinceEpoch, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
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
            Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return Schema.IsIntercalaryDay(y, m, d);
        }
    }

    /// <inheritdoc />
    public bool IsSupplementary
    {
        get
        {
            Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return Schema.IsSupplementaryDay(y, m, d);
        }
    }

    /// <summary>
    /// Gets the underlying schema.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    //
    // Don't use Scope.Schema which is only of type ICalendricalSchema.
    private static Egyptian12Schema Schema => ZoroastrianCalendar.UnderlyingSchema;

    /// <summary>
    /// Gets the calendar scope.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    private static StandardScope Scope => ZoroastrianCalendar.UnderlyingScope;

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({Calendar})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        Schema.GetDateParts(_daysSinceEpoch, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = Schema.GetYear(_daysSinceEpoch, out dayOfYear);
}

public partial struct ZoroastrianDate // Factories
{
    /// <inheritdoc />
    [Pure]
    public static ZoroastrianDate FromDayNumber(DayNumber dayNumber)
    {
        Scope.Validate(dayNumber);

        // We know that the subtraction won't overflow
        // > return new(dayNumber - s_Epoch);
        return new(dayNumber.DaysSinceZero - s_EpochDaysSinceZero);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ZoroastrianDate"/> struct
    /// from the specified day number.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ZoroastrianDate FromDayNumberUnchecked(DayNumber dayNumber) =>
        new(dayNumber.DaysSinceZero - s_EpochDaysSinceZero);

    /// <inheritdoc />
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    static ZoroastrianDate IFixedDateFactory<ZoroastrianDate>.FromDaysSinceEpochUnchecked(int daysSinceEpoch) =>
        new(daysSinceEpoch);
}

public partial struct ZoroastrianDate // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() => Schema.CountDaysInYearBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() => Schema.CountDaysInYearAfter(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => Schema.CountDaysInMonthBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() => Schema.CountDaysInMonthAfter(_daysSinceEpoch);
}

public partial struct ZoroastrianDate // Adjustments
{
    /// <inheritdoc />
    /// <remarks>See also <seealso cref="Adjuster"/>.</remarks>
    [Pure]
    public ZoroastrianDate Adjust(Func<ZoroastrianDate, ZoroastrianDate> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        return adjuster.Invoke(this);
    }

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate WithYear(int newYear) => Adjuster.AdjustYear(this, newYear);

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate WithMonth(int newMonth) => Adjuster.AdjustMonth(this, newMonth);

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate WithDay(int newDay) => Adjuster.AdjustDay(this, newDay);

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate WithDayOfYear(int newDayOfYear) => Adjuster.AdjustDayOfYear(this, newDayOfYear);

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        Scope.CheckLowerBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        Scope.CheckLowerBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        Scope.CheckOverflow(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        Scope.CheckUpperBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        Scope.CheckUpperBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }
}

public partial struct ZoroastrianDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(ZoroastrianDate left, ZoroastrianDate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(ZoroastrianDate left, ZoroastrianDate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(ZoroastrianDate other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is ZoroastrianDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct ZoroastrianDate // IComparable
{
    /// <inheritdoc />
    public static bool operator <(ZoroastrianDate left, ZoroastrianDate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator <=(ZoroastrianDate left, ZoroastrianDate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >(ZoroastrianDate left, ZoroastrianDate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >=(ZoroastrianDate left, ZoroastrianDate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static ZoroastrianDate Min(ZoroastrianDate x, ZoroastrianDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static ZoroastrianDate Max(ZoroastrianDate x, ZoroastrianDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(ZoroastrianDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is ZoroastrianDate date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(ZoroastrianDate), obj);
}

public partial struct ZoroastrianDate // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    public static int operator -(ZoroastrianDate left, ZoroastrianDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static ZoroastrianDate operator +(ZoroastrianDate value, int days) => value.AddDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static ZoroastrianDate operator -(ZoroastrianDate value, int days) => value.AddDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    public static ZoroastrianDate operator ++(ZoroastrianDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    public static ZoroastrianDate operator --(ZoroastrianDate value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(ZoroastrianDate other) =>
        // No need to use a checked context here.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate AddDays(int days)
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
    public ZoroastrianDate NextDay()
    {
        if (this == s_MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch + 1);
    }

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate PreviousDay()
    {
        if (this == s_MinValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch - 1);
    }
}

