﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Calendrie.Specialized;

using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Core.Validation;
using Calendrie.Hemerology;

/// <summary>
/// Provides static methods related to the scope of application of
/// <see cref="ArmenianCalendar"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static class ArmenianScope
{
    // WARNING: the order in which the static fields are written is __important__.

    public static readonly DayNumber Epoch = DayZero.Armenian;

    // This schema instance is the one used by:
    // - ArmenianScope.Instance (ctor)
    // - ArmenianCalendar.Instance via ArmenianScope.Instance
    // - All instances of the ArmenianDate type via its property Schema
    // - ArmenianCalendar, custom methods only (see the file _Calendar.cs)
    public static readonly Egyptian12Schema Schema = new();

    // This scope instance is the one used by:
    // - ArmenianCalendar.Instance (ctor)
    // - All instances of the ArmenianDate type via its property Scope
    public static readonly StandardScope Instance = new(Schema, Epoch);

    // The next three properties are only created to ease the initialization of
    // the static fields of ArmenianDate. Notice that these properties are
    // properties (!) of value type without a backing field, therefore they only
    // exist temporarily.

    public static Range<DayNumber> Domain => Instance.Domain;

    /// <summary>
    /// Represents the minimum possible value for the number of consecutive days
    /// from the epoch.
    /// </summary>
    public static int MinDaysSinceEpoch => Instance.Segment.SupportedDays.Min;

    /// <summary>
    /// Represents the maximum possible value for the number of consecutive days
    /// from the epoch.
    /// </summary>
    public static int MaxDaysSinceEpoch => Instance.Segment.SupportedDays.Max;

    public static StandardScope Create() => new(new Egyptian12Schema(), Epoch);
}

/// <summary>
/// Represents the Armenian calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class ArmenianCalendar : SpecialCalendar<ArmenianDate>
{
    // This class is not a singleton but we ensure that all date instances are
    // using the same calendar instance. While not mandatory at all, I like the
    // idea.
    internal static readonly ArmenianCalendar Instance = new(ArmenianScope.Instance);

    /// <summary>
    /// Initializes a new instance of the <see cref="ArmenianCalendar"/> class.
    /// <para>See also <seealso cref="ArmenianDate.Calendar"/>.</para>
    /// </summary>
    public ArmenianCalendar() : this(ArmenianScope.Create()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ArmenianCalendar"/> class.
    /// </summary>
    private ArmenianCalendar(StandardScope scope) : base("Armenian", scope)
    {
        Adjuster = new ArmenianAdjuster(this);
    }

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public ArmenianAdjuster Adjuster { get; }

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private protected sealed override ArmenianDate NewDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Provides common adjusters for <see cref="ArmenianDate"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class ArmenianAdjuster : SpecialAdjuster<ArmenianDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArmenianAdjuster"/> class.
    /// </summary>
    internal ArmenianAdjuster(ArmenianCalendar calendar) : base(calendar) { }

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private protected sealed override ArmenianDate NewDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Represents the Armenian date.
/// <para><see cref="ArmenianDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct ArmenianDate :
    IDate<ArmenianDate, ArmenianCalendar>,
    IAdjustable<ArmenianDate>
{ }

public partial struct ArmenianDate // Preamble
{
    /// <summary>Represents the epoch of the associated calendar.</summary>
    private static readonly DayNumber s_Epoch = ArmenianScope.Epoch;

    /// <summary>Represents the range of supported <see cref="DayNumber"/>'s by
    /// the associated calendar.</summary>
    private static readonly Range<DayNumber> s_Domain = ArmenianScope.Domain;

    /// <summary>Represents the minimum value of <see cref="_daysSinceEpoch"/>.</summary>
    private static readonly int s_MinDaysSinceEpoch = ArmenianScope.MinDaysSinceEpoch;
    /// <summary>Represents the maximum value of <see cref="_daysSinceEpoch"/>.</summary>
    private static readonly int s_MaxDaysSinceEpoch = ArmenianScope.MaxDaysSinceEpoch;

    /// <summary>Represents the minimum value of the current type.</summary>
    private static readonly ArmenianDate s_MinValue = new(ArmenianScope.MinDaysSinceEpoch);
    /// <summary>Represents the maximum value of the current type.</summary>
    private static readonly ArmenianDate s_MaxValue = new(ArmenianScope.MaxDaysSinceEpoch);

    /// <summary>
    /// Represents the count of consecutive days since <see cref="s_Epoch"/>.
    /// <para>This field is in the range from <see cref="s_MinDaysSinceEpoch"/>
    /// to <see cref="s_MaxDaysSinceEpoch"/>.</para>
    /// </summary>
    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArmenianDate"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid date or <paramref name="year"/> is outside the range of supported
    /// years.</exception>
    public ArmenianDate(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ArmenianDate"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid ordinal date or <paramref name="year"/> is outside the range of
    /// supported years.</exception>
    public ArmenianDate(int year, int dayOfYear)
    {
        Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// This constructor does NOT validate its parameter.
    /// </summary>
    internal ArmenianDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static ArmenianDate MinValue => s_MinValue;

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static ArmenianDate MaxValue => s_MaxValue;

    /// <inheritdoc />
    public static ArmenianCalendar Calendar => ArmenianCalendar.Instance;

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static ArmenianAdjuster Adjuster => ArmenianCalendar.Instance.Adjuster;

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
    // Don't use Scope.Schema or ArmenianScope.Instance.Schema. Both are of
    // type ICalendricalSchema, not Egyptian12Schema.
    private static Egyptian12Schema Schema => ArmenianScope.Schema;

    /// <summary>
    /// Gets the calendar scope.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    private static StandardScope Scope => ArmenianScope.Instance;

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

public partial struct ArmenianDate // Factories
{
    /// <inheritdoc />
    [Pure]
    public static ArmenianDate FromDayNumber(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        // We know that the subtraction won't overflow
        // > return new(dayNumber - s_Epoch);
        return new(dayNumber.DaysSinceZero - s_Epoch.DaysSinceZero);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ArmenianDate"/> struct
    /// from the specified day number.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ArmenianDate FromDayNumberUnchecked(DayNumber dayNumber) =>
        new(dayNumber.DaysSinceZero - s_Epoch.DaysSinceZero);
}

public partial struct ArmenianDate // Counting
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

public partial struct ArmenianDate // Adjustments
{
    /// <inheritdoc />
    /// <remarks>See also <seealso cref="Adjuster"/>.</remarks>
    [Pure]
    public ArmenianDate Adjust(Func<ArmenianDate, ArmenianDate> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        return adjuster.Invoke(this);
    }

    /// <inheritdoc />
    [Pure]
    public ArmenianDate Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public ArmenianDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public ArmenianDate Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        s_Domain.CheckOverflow(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public ArmenianDate NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public ArmenianDate Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }
}

public partial struct ArmenianDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(ArmenianDate left, ArmenianDate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(ArmenianDate left, ArmenianDate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(ArmenianDate other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is ArmenianDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct ArmenianDate // IComparable
{
    /// <inheritdoc />
    public static bool operator <(ArmenianDate left, ArmenianDate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator <=(ArmenianDate left, ArmenianDate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >(ArmenianDate left, ArmenianDate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >=(ArmenianDate left, ArmenianDate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static ArmenianDate Min(ArmenianDate x, ArmenianDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static ArmenianDate Max(ArmenianDate x, ArmenianDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(ArmenianDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is ArmenianDate date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(ArmenianDate), obj);
}

public partial struct ArmenianDate // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    public static int operator -(ArmenianDate left, ArmenianDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static ArmenianDate operator +(ArmenianDate value, int days) => value.AddDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static ArmenianDate operator -(ArmenianDate value, int days) => value.AddDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    public static ArmenianDate operator ++(ArmenianDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    public static ArmenianDate operator --(ArmenianDate value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(ArmenianDate other) =>
        // No need to use a checked context here.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public ArmenianDate AddDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);

        // Don't write (the addition may also overflow...):
        // > s_Domain.CheckOverflow(Epoch + daysSinceEpoch);
        if (daysSinceEpoch < s_MinDaysSinceEpoch || daysSinceEpoch > s_MaxDaysSinceEpoch)
            ThrowHelpers.ThrowDateOverflow();

        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public ArmenianDate NextDay()
    {
        if (this == s_MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch + 1);
    }

    /// <inheritdoc />
    [Pure]
    public ArmenianDate PreviousDay()
    {
        if (this == s_MinValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch - 1);
    }
}

