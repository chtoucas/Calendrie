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

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Represents the Ethiopic calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class Ethiopic13Calendar : CalendarSystem<Ethiopic13Date>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Ethiopic13Calendar"/> class.
    /// </summary>
    public Ethiopic13Calendar() : this(new Coptic13Schema()) { }

    private Ethiopic13Calendar(Coptic13Schema schema)
        : this(schema, new StandardScope(schema, DayZero.Ethiopic)) { }

    private Ethiopic13Calendar(Coptic13Schema schema, StandardScope scope)
        : base("Ethiopic", scope)
    {
        UnderlyingSchema = schema;
    }

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    public static int MinYear => StandardScope.MinYear;

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public static int MaxYear => StandardScope.MaxYear;

    /// <summary>
    /// Gets a singleton instance of the <see cref="Ethiopic13Calendar"/> class.
    /// <para>See <see cref="Ethiopic13Date.Calendar"/>.</para>
    /// </summary>
    internal static Ethiopic13Calendar Instance { get; } = new();

    /// <summary>
    /// Gets the schema.
    /// </summary>
    internal Coptic13Schema UnderlyingSchema { get; }
}

/// <summary>
/// Represents the Ethiopic date.
/// <para><see cref="Ethiopic13Date"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct Ethiopic13Date :
    IDateable,
    IAbsoluteDate<Ethiopic13Date>,
    IAdjustableDate<Ethiopic13Date>,
    IAdjustableDayOfWeekField<Ethiopic13Date>,
    IDateFactory<Ethiopic13Date>,
    ISubtractionOperators<Ethiopic13Date, Ethiopic13Date, int>
{ }

public partial struct Ethiopic13Date // Preamble
{
    // WARNING: the order in which the static fields are written is __important__.

    private static readonly int s_EpochDaysSinceZero = Ethiopic13Calendar.Instance.Epoch.DaysSinceZero;

    /// <summary>Represents the minimum value of <see cref="_daysSinceEpoch"/>.</summary>
    private static readonly int s_MinDaysSinceEpoch = Ethiopic13Calendar.Instance.MinDaysSinceEpoch;
    /// <summary>Represents the maximum value of <see cref="_daysSinceEpoch"/>.</summary>
    private static readonly int s_MaxDaysSinceEpoch = Ethiopic13Calendar.Instance.MaxDaysSinceEpoch;

    /// <summary>Represents the minimum value of the current type.</summary>
    private static readonly Ethiopic13Date s_MinValue = new(s_MinDaysSinceEpoch);
    /// <summary>Represents the maximum value of the current type.</summary>
    private static readonly Ethiopic13Date s_MaxValue = new(s_MaxDaysSinceEpoch);

    /// <summary>
    /// Represents the count of consecutive days since the epoch <see cref="DayZero.Ethiopic"/>.
    /// <para>This field is in the range from <see cref="s_MinDaysSinceEpoch"/>
    /// to <see cref="s_MaxDaysSinceEpoch"/>.</para>
    /// </summary>
    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="Ethiopic13Date"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the range
    /// of supported years.</exception>
    public Ethiopic13Date(int year, int month, int day)
    {
        var chr = Ethiopic13Calendar.Instance;
        chr.Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Ethiopic13Date"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid ordinal date or <paramref name="year"/> is outside
    /// the range of supported years.</exception>
    public Ethiopic13Date(int year, int dayOfYear)
    {
        var chr = Ethiopic13Calendar.Instance;
        chr.Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// This constructor does NOT validate its parameter.
    /// </summary>
    internal Ethiopic13Date(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static Ethiopic13Date MinValue => s_MinValue;

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static Ethiopic13Date MaxValue => s_MaxValue;

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Ethiopic13Calendar Calendar => Ethiopic13Calendar.Instance;

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
    public int Year => Calendar.Schema.GetYear(_daysSinceEpoch);

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

    /// <inheritdoc />
    public bool IsSupplementary
    {
        get
        {
            var sch = Calendar.Schema;
            sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return sch.IsSupplementaryDay(y, m, d);
        }
    }

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

public partial struct Ethiopic13Date // Factories & conversions
{
    /// <inheritdoc />
    [Pure]
    public static Ethiopic13Date FromDayNumber(DayNumber dayNumber)
    {
        Calendar.Scope.Validate(dayNumber);

        // We know that the subtraction won't overflow
        // > return new(dayNumber - s_Epoch);
        return new(dayNumber.DaysSinceZero - s_EpochDaysSinceZero);
    }

    /// <inheritdoc />
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    static Ethiopic13Date IDateFactory<Ethiopic13Date>.FromDaysSinceEpochUnchecked(int daysSinceEpoch) =>
        new(daysSinceEpoch);
}

public partial struct Ethiopic13Date // Counting
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

public partial struct Ethiopic13Date // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date Adjust(Func<Ethiopic13Date, Ethiopic13Date> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        return adjuster.Invoke(this);
    }

    //
    // Adjustments for the core parts
    //

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date WithYear(int newYear)
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
    public Ethiopic13Date WithMonth(int newMonth)
    {
        var (y, _, d) = this;

        var sch = Calendar.Schema;
        // We only need to validate "newMonth" and "d".
        sch.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, newMonth, d);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date WithDay(int newDay)
    {
        var (y, m, _) = this;

        var sch = Calendar.Schema;
        // We only need to validate "newDay".
        sch.PreValidator.ValidateDayOfMonth(y, m, newDay, nameof(newDay));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, m, newDay);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date WithDayOfYear(int newDayOfYear)
    {
        int y = Year;

        var sch = Calendar.Schema;
        // We only need to validate "newDayOfYear".
        sch.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, newDayOfYear);
        return new(daysSinceEpoch);
    }

    //
    // Adjust the day of the week
    //

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceEpoch = _daysSinceEpoch + (δ >= 0 ? δ - DaysInWeek : δ);
        if (daysSinceEpoch < s_MinDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date PreviousOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceEpoch = _daysSinceEpoch + (δ > 0 ? δ - DaysInWeek : δ); ;
        if (daysSinceEpoch < s_MinDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date Nearest(DayOfWeek dayOfWeek)
    {
        var nearest = DayNumber.Nearest(dayOfWeek);
        int daysSinceEpoch = nearest.DaysSinceZero - s_EpochDaysSinceZero;
        if (daysSinceEpoch < s_MinDaysSinceEpoch || daysSinceEpoch > s_MaxDaysSinceEpoch)
            ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date NextOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceEpoch = _daysSinceEpoch + (δ < 0 ? δ + DaysInWeek : δ);
        if (daysSinceEpoch > s_MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceEpoch = _daysSinceEpoch + (δ <= 0 ? δ + DaysInWeek : δ);
        if (daysSinceEpoch > s_MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }
}

public partial struct Ethiopic13Date // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(Ethiopic13Date left, Ethiopic13Date right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(Ethiopic13Date left, Ethiopic13Date right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(Ethiopic13Date other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Ethiopic13Date date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct Ethiopic13Date // IComparable
{
    /// <inheritdoc />
    public static bool operator <(Ethiopic13Date left, Ethiopic13Date right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator <=(Ethiopic13Date left, Ethiopic13Date right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >(Ethiopic13Date left, Ethiopic13Date right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >=(Ethiopic13Date left, Ethiopic13Date right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static Ethiopic13Date Min(Ethiopic13Date x, Ethiopic13Date y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static Ethiopic13Date Max(Ethiopic13Date x, Ethiopic13Date y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(Ethiopic13Date other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is Ethiopic13Date date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(Ethiopic13Date), obj);
}

public partial struct Ethiopic13Date // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    public static int operator -(Ethiopic13Date left, Ethiopic13Date right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static Ethiopic13Date operator +(Ethiopic13Date value, int days) => value.AddDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static Ethiopic13Date operator -(Ethiopic13Date value, int days) => value.AddDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    public static Ethiopic13Date operator ++(Ethiopic13Date value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    public static Ethiopic13Date operator --(Ethiopic13Date value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(Ethiopic13Date other) =>
        // No need to use a checked context here.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date AddDays(int days)
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
    public Ethiopic13Date NextDay()
    {
        if (this == s_MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch + 1);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date PreviousDay()
    {
        if (this == s_MinValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch - 1);
    }
}

