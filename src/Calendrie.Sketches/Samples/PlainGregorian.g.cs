﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable

namespace Calendrie.Samples;

using System.Numerics;

using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Core.Validation;
using Calendrie.Hemerology;
using Calendrie.Systems;

/// <summary>
/// Represents the PlainGregorian calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class PlainGregorianCalendar : CalendarSystem<PlainGregorianDate>
{
    /// <summary>Represents the epoch.</summary>
    internal static readonly DayNumber Epoch = DayZero.NewStyle;

    /// <summary>Represents a singleton instance of the schema.</summary>
    // This schema instance is the one used by:
    // - All instances of the PlainGregorianDate type via the property Schema
    // - PlainGregorianCalendar, custom methods only (see the file _Calendar.cs)
    internal static readonly GregorianSchema SchemaT = new();

    /// <summary>Represents a singleton instance of the scope.</summary>
    // This scope instance is the one used by:
    // - All instances of the PlainGregorianDate type via the property Scope
    internal static readonly StandardScope ScopeT = CreateScope(new GregorianSchema());

    /// <summary>Represents a singleton instance of the calendar.</summary>
    // This calendar instance is the one used by:
    // - All instances of the PlainGregorianDate type via the properties Calendar and Adjuster
    internal static readonly PlainGregorianCalendar Instance = new(CreateScope(new GregorianSchema()));

    /// <summary>
    /// Initializes a new instance of the <see cref="PlainGregorianCalendar"/> class.
    /// <para>See also <seealso cref="PlainGregorianDate.Calendar"/>.</para>
    /// </summary>
    public PlainGregorianCalendar() : this(CreateScope(new GregorianSchema())) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlainGregorianCalendar"/> class.
    /// </summary>
    private PlainGregorianCalendar(StandardScope scope) : base("PlainGregorian", scope)
    {
        Adjuster = new DateAdjuster<PlainGregorianDate>(this);
    }

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public DateAdjuster<PlainGregorianDate> Adjuster { get; }

    /// <summary>
    /// Creates a new instance of the <see href="StandardScope"/> class.
    /// </summary>
    private static StandardScope CreateScope(GregorianSchema schema) => new(Epoch, schema);
}

/// <summary>
/// Represents the PlainGregorian date.
/// <para><see cref="PlainGregorianDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct PlainGregorianDate :
    IDate<PlainGregorianDate, PlainGregorianCalendar>,
    IDateFactory<PlainGregorianDate>,
    IAdjustable<PlainGregorianDate>,
    ISubtractionOperators<PlainGregorianDate, PlainGregorianDate, int>
{ }

public partial struct PlainGregorianDate // Preamble
{
    /// <summary>Represents the range of supported <see cref="DayNumber"/>'s by
    /// the associated calendar.</summary>
    private static readonly Range<DayNumber> s_Domain = PlainGregorianCalendar.ScopeT.Domain;

    /// <summary>Represents the minimum value of <see cref="_daysSinceZero"/>.</summary>
    private static readonly int s_MinDaysSinceZero = PlainGregorianCalendar.ScopeT.MinDaysSinceEpoch;
    /// <summary>Represents the maximum value of <see cref="_daysSinceZero"/>.</summary>
    private static readonly int s_MaxDaysSinceZero = PlainGregorianCalendar.ScopeT.MaxDaysSinceEpoch;

    /// <summary>Represents the minimum value of the current type.</summary>
    private static readonly PlainGregorianDate s_MinValue = new(s_MinDaysSinceZero);
    /// <summary>Represents the maximum value of the current type.</summary>
    private static readonly PlainGregorianDate s_MaxValue = new(s_MaxDaysSinceZero);

    /// <summary>
    /// Represents the count of consecutive days since <see cref="DayZero.NewStyle"/>.
    /// <para>This field is in the range from 0 to <see cref="s_MaxDaysSinceZero"/>.
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
        Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceZero = Schema.CountDaysSinceEpoch(year, month, day);
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
        Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceZero = Schema.CountDaysSinceEpoch(year, dayOfYear);
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
    public static PlainGregorianDate MinValue => s_MinValue;

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static PlainGregorianDate MaxValue => s_MaxValue;

    /// <inheritdoc />
    public static PlainGregorianCalendar Calendar => PlainGregorianCalendar.Instance;

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static DateAdjuster<PlainGregorianDate> Adjuster => PlainGregorianCalendar.Instance.Adjuster;

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
    public int Year => Schema.GetYear(_daysSinceZero);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            Schema.GetDateParts(_daysSinceZero, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = Schema.GetYear(_daysSinceZero, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            Schema.GetDateParts(_daysSinceZero, out _, out _, out int d);
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
            Schema.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
            return Schema.IsIntercalaryDay(y, m, d);
        }
    }

    /// <inheritdoc />
    public bool IsSupplementary
    {
        get
        {
            Schema.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
            return Schema.IsSupplementaryDay(y, m, d);
        }
    }

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    private static GregorianSchema Schema => PlainGregorianCalendar.SchemaT;

    /// <summary>
    /// Gets the calendar scope.
    /// </summary>
    private static StandardScope Scope => PlainGregorianCalendar.ScopeT;

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        Schema.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({Calendar})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        Schema.GetDateParts(_daysSinceZero, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = Schema.GetYear(_daysSinceZero, out dayOfYear);
}

public partial struct PlainGregorianDate // Factories
{
    /// <inheritdoc />
    [Pure]
    public static PlainGregorianDate FromDayNumber(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        return new(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    static PlainGregorianDate IDateFactory<PlainGregorianDate>.FromDaysSinceEpochUnchecked(int daysSinceEpoch) =>
        new(daysSinceEpoch);
}

public partial struct PlainGregorianDate // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() => Schema.CountDaysInYearBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() => Schema.CountDaysInYearAfter(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => Schema.CountDaysInMonthBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() => Schema.CountDaysInMonthAfter(_daysSinceZero);
}

public partial struct PlainGregorianDate // Adjustments
{
    /// <inheritdoc />
    /// <remarks>See also <seealso cref="Adjuster"/>.</remarks>
    [Pure]
    public PlainGregorianDate Adjust(Func<PlainGregorianDate, PlainGregorianDate> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        return adjuster.Invoke(this);
    }

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return new(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return new(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        s_Domain.CheckOverflow(dayNumber);
        return new(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return new(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return new(dayNumber.DaysSinceZero);
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
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    public static int operator -(PlainGregorianDate left, PlainGregorianDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static PlainGregorianDate operator +(PlainGregorianDate value, int days) => value.AddDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static PlainGregorianDate operator -(PlainGregorianDate value, int days) => value.AddDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    public static PlainGregorianDate operator ++(PlainGregorianDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    public static PlainGregorianDate operator --(PlainGregorianDate value) => value.PreviousDay();

#pragma warning restore CA2225

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
        // > s_Domain.CheckOverflow(Epoch + daysSinceZero);
        if (daysSinceZero < s_MinDaysSinceZero || daysSinceZero > s_MaxDaysSinceZero)
            ThrowHelpers.ThrowDateOverflow();

        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate NextDay()
    {
        if (this == s_MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceZero + 1);
    }

    /// <inheritdoc />
    [Pure]
    public PlainGregorianDate PreviousDay()
    {
        if (this == s_MinValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceZero - 1);
    }
}

