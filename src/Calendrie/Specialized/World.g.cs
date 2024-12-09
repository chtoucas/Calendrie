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
/// Represents the World calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class WorldCalendar : SpecialCalendar<WorldDate>
{
    /// <summary>Represents the epoch.</summary>
    internal static readonly DayNumber Epoch = DayZero.SundayBeforeGregorian;

    /// <summary>Represents a singleton instance of the schema.</summary>
    // This schema instance is the one used by:
    // - All instances of the WorldDate type via the property Schema
    // - WorldCalendar, custom methods only (see the file _Calendar.cs)
    internal static readonly WorldSchema SchemaT = new();

    /// <summary>Represents a singleton instance of the scope.</summary>
    // This scope instance is the one used by:
    // - All instances of the WorldDate type via the property Scope
    internal static readonly StandardScope ScopeT = CreateScope(new WorldSchema());

    /// <summary>Represents a singleton instance of the calendar.</summary>
    // This calendar instance is the one used by:
    // - All instances of the WorldDate type via the properties Calendar and Adjuster
    internal static readonly WorldCalendar Instance = new(CreateScope(new WorldSchema()));

    /// <summary>
    /// Initializes a new instance of the <see cref="WorldCalendar"/> class.
    /// <para>See also <seealso cref="WorldDate.Calendar"/>.</para>
    /// </summary>
    public WorldCalendar() : this(CreateScope(new WorldSchema())) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorldCalendar"/> class.
    /// </summary>
    private WorldCalendar(StandardScope scope) : base("World", scope)
    {
        Adjuster = new WorldAdjuster(this);
    }

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public WorldAdjuster Adjuster { get; }

    /// <summary>
    /// Creates a new instance of the <see href="StandardScope"/> class.
    /// </summary>
    private static StandardScope CreateScope(WorldSchema schema) => new(Epoch, schema);

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private protected sealed override WorldDate NewDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Provides common adjusters for <see cref="WorldDate"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class WorldAdjuster : SpecialAdjuster<WorldDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorldAdjuster"/> class.
    /// </summary>
    internal WorldAdjuster(WorldCalendar calendar) : base(calendar) { }

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private protected sealed override WorldDate NewDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Represents the World date.
/// <para><see cref="WorldDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct WorldDate :
    IDate<WorldDate, WorldCalendar>,
    IAdjustable<WorldDate>
{ }

public partial struct WorldDate // Preamble
{
    // WARNING: the order in which the static fields are written is __important__.

    private static readonly int s_EpochDaysSinceZero = WorldCalendar.Epoch.DaysSinceZero;

    /// <summary>Represents the range of supported <see cref="DayNumber"/>'s by
    /// the associated calendar.</summary>
    private static readonly Range<DayNumber> s_Domain = WorldCalendar.ScopeT.Domain;

    /// <summary>Represents the minimum value of <see cref="_daysSinceEpoch"/>.</summary>
    private static readonly int s_MinDaysSinceEpoch = WorldCalendar.ScopeT.MinDaysSinceEpoch;
    /// <summary>Represents the maximum value of <see cref="_daysSinceEpoch"/>.</summary>
    private static readonly int s_MaxDaysSinceEpoch = WorldCalendar.ScopeT.MaxDaysSinceEpoch;

    /// <summary>Represents the minimum value of the current type.</summary>
    private static readonly WorldDate s_MinValue = new(s_MinDaysSinceEpoch);
    /// <summary>Represents the maximum value of the current type.</summary>
    private static readonly WorldDate s_MaxValue = new(s_MaxDaysSinceEpoch);

    /// <summary>
    /// Represents the count of consecutive days since the epoch <see cref="DayZero.SundayBeforeGregorian"/>.
    /// <para>This field is in the range from <see cref="s_MinDaysSinceEpoch"/>
    /// to <see cref="s_MaxDaysSinceEpoch"/>.</para>
    /// </summary>
    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorldDate"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid date or <paramref name="year"/> is outside the range of supported
    /// years.</exception>
    public WorldDate(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorldDate"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid ordinal date or <paramref name="year"/> is outside the range of
    /// supported years.</exception>
    public WorldDate(int year, int dayOfYear)
    {
        Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// This constructor does NOT validate its parameter.
    /// </summary>
    internal WorldDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static WorldDate MinValue => s_MinValue;

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static WorldDate MaxValue => s_MaxValue;

    /// <inheritdoc />
    public static WorldCalendar Calendar => WorldCalendar.Instance;

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static WorldAdjuster Adjuster => WorldCalendar.Instance.Adjuster;

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
    private static WorldSchema Schema => WorldCalendar.SchemaT;

    /// <summary>
    /// Gets the calendar scope.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    private static StandardScope Scope => WorldCalendar.ScopeT;

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

public partial struct WorldDate // Factories
{
    /// <inheritdoc />
    [Pure]
    public static WorldDate FromDayNumber(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        // We know that the subtraction won't overflow
        // > return new(dayNumber - s_Epoch);
        return new(dayNumber.DaysSinceZero - s_EpochDaysSinceZero);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="WorldDate"/> struct
    /// from the specified day number.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static WorldDate FromDayNumberUnchecked(DayNumber dayNumber) =>
        new(dayNumber.DaysSinceZero - s_EpochDaysSinceZero);
}

public partial struct WorldDate // Counting
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

public partial struct WorldDate // Adjustments
{
    /// <inheritdoc />
    /// <remarks>See also <seealso cref="Adjuster"/>.</remarks>
    [Pure]
    public WorldDate Adjust(Func<WorldDate, WorldDate> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        return adjuster.Invoke(this);
    }

    /// <inheritdoc />
    [Pure]
    public WorldDate Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public WorldDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public WorldDate Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        s_Domain.CheckOverflow(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public WorldDate NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public WorldDate Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }
}

public partial struct WorldDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(WorldDate left, WorldDate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(WorldDate left, WorldDate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(WorldDate other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is WorldDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct WorldDate // IComparable
{
    /// <inheritdoc />
    public static bool operator <(WorldDate left, WorldDate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator <=(WorldDate left, WorldDate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >(WorldDate left, WorldDate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >=(WorldDate left, WorldDate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static WorldDate Min(WorldDate x, WorldDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static WorldDate Max(WorldDate x, WorldDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(WorldDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is WorldDate date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(WorldDate), obj);
}

public partial struct WorldDate // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    public static int operator -(WorldDate left, WorldDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static WorldDate operator +(WorldDate value, int days) => value.AddDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static WorldDate operator -(WorldDate value, int days) => value.AddDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    public static WorldDate operator ++(WorldDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    public static WorldDate operator --(WorldDate value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(WorldDate other) =>
        // No need to use a checked context here.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public WorldDate AddDays(int days)
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
    public WorldDate NextDay()
    {
        if (this == s_MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch + 1);
    }

    /// <inheritdoc />
    [Pure]
    public WorldDate PreviousDay()
    {
        if (this == s_MinValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch - 1);
    }
}

