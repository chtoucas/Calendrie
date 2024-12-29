﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Calendrie.Systems;

using System.Diagnostics;
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
/// Represents the Armenian calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class Armenian13Calendar : CalendarSystem<Armenian13Date>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Armenian13Calendar"/> class.
    /// </summary>
    public Armenian13Calendar() : this(new Egyptian13Schema()) { }

    private Armenian13Calendar(Egyptian13Schema schema)
        : base("Armenian", new StandardScope(schema, DayZero.Armenian))
    {
        Debug.Assert(schema != null);
        Schema = schema;
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="Armenian13Calendar"/> class.
    /// <para>See <see cref="Armenian13Date.Calendar"/>.</para>
    /// </summary>
    internal static Armenian13Calendar Instance { get; } = new();

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
    internal Egyptian13Schema Schema { get; }
}

/// <summary>
/// Represents the Armenian date.
/// <para><see cref="Armenian13Date"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct Armenian13Date :
    IDateable,
    IAbsoluteDate<Armenian13Date>,
    IAdjustableDate<Armenian13Date>,
    IDateFactory<Armenian13Date>,
    ISubtractionOperators<Armenian13Date, Armenian13Date, int>
{ }

public partial struct Armenian13Date // Preamble
{
    /// <summary>Represents the value of the property <see cref="DayNumber.DaysSinceZero"/>
    /// for the epoch <see cref="DayZero.Armenian"/>.
    /// <para>This field is a constant equal to 201_442.</para></summary>
    private const int EpochDaysSinceZero = 201_442;

    /// <summary>Represents the maximum value of <see cref="_daysSinceEpoch"/>.
    /// <para>This field is a constant equal to 3_649_634.</para></summary>
    private const int MaxDaysSinceEpoch = 3_649_634;

    /// <summary>
    /// Represents the count of consecutive days since the epoch <see cref="DayZero.Armenian"/>.
    /// <para>This field is in the range from 0 to <see cref="MaxDaysSinceEpoch"/>.
    /// </para>
    /// </summary>
    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="Armenian13Date"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the range
    /// of supported years.</exception>
    public Armenian13Date(int year, int month, int day)
    {
        var chr = Armenian13Calendar.Instance;
        chr.Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Armenian13Date"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid ordinal date or <paramref name="year"/> is outside
    /// the range of supported years.</exception>
    public Armenian13Date(int year, int dayOfYear)
    {
        var chr = Armenian13Calendar.Instance;
        chr.Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// This constructor does NOT validate its parameter.
    /// </summary>
    internal Armenian13Date(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static Armenian13Date MinValue { get; }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static Armenian13Date MaxValue { get; } = new(MaxDaysSinceEpoch);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Armenian13Calendar Calendar => Armenian13Calendar.Instance;

    /// <inheritdoc />
    //
    // We already know that the resulting day number is valid so instead of
    // > public DayNumber DayNumber => s_Epoch + _daysSinceEpoch;
    // we can use an unchecked addition
    public DayNumber DayNumber => new(EpochDaysSinceZero + _daysSinceEpoch);

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

public partial struct Armenian13Date // Factories & conversions
{
    /// <inheritdoc />
    [Pure]
    public static Armenian13Date FromDayNumber(DayNumber dayNumber)
    {
        Calendar.Scope.Validate(dayNumber);

        // We know that the subtraction won't overflow
        // > return new(dayNumber - s_Epoch);
        return new(dayNumber.DaysSinceZero - EpochDaysSinceZero);

        //int daysSinceEpoch = dayNumber.DaysSinceZero - EpochDaysSinceZero;

        //if (unchecked((uint)daysSinceEpoch) > MaxDaysSinceEpoch)
        //    throw new ArgumentOutOfRangeException(nameof(dayNumber));

        //return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    static Armenian13Date IDateFactory<Armenian13Date>.UnsafeCreate(int daysSinceEpoch) =>
        new(daysSinceEpoch);
}

public partial struct Armenian13Date // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() => Calendar.Schema.CountDaysInYearBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() => Calendar.Schema.CountDaysInYearAfter(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => Calendar.Schema.CountDaysInMonthBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() => Calendar.Schema.CountDaysInMonthAfter(_daysSinceEpoch);
}

public partial struct Armenian13Date // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public Armenian13Date WithYear(int newYear)
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
    public Armenian13Date WithMonth(int newMonth)
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
    public Armenian13Date WithDay(int newDay)
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
    public Armenian13Date WithDayOfYear(int newDayOfYear)
    {
        int y = Year;

        var sch = Calendar.Schema;
        // We only need to validate "newDayOfYear".
        sch.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, newDayOfYear);
        return new(daysSinceEpoch);
    }
}

public partial struct Armenian13Date // Find close by day of the week
{
    /// <inheritdoc />
    [Pure]
    public Armenian13Date Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceEpoch = _daysSinceEpoch + (δ >= 0 ? δ - DaysInWeek : δ);
        if (daysSinceEpoch < 0) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date PreviousOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceEpoch = _daysSinceEpoch + (δ > 0 ? δ - DaysInWeek : δ);
        if (daysSinceEpoch < 0) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date Nearest(DayOfWeek dayOfWeek)
    {
        var nearest = DayNumber.Nearest(dayOfWeek);
        int daysSinceEpoch = nearest.DaysSinceZero - EpochDaysSinceZero;
        if ((uint)daysSinceEpoch > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date NextOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceEpoch = _daysSinceEpoch + (δ < 0 ? δ + DaysInWeek : δ);
        if (daysSinceEpoch > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceEpoch = _daysSinceEpoch + (δ <= 0 ? δ + DaysInWeek : δ);
        if (daysSinceEpoch > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }
}

public partial struct Armenian13Date // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(Armenian13Date left, Armenian13Date right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(Armenian13Date left, Armenian13Date right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(Armenian13Date other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Armenian13Date date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct Armenian13Date // IComparable
{
    /// <inheritdoc />
    public static bool operator <(Armenian13Date left, Armenian13Date right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator <=(Armenian13Date left, Armenian13Date right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >(Armenian13Date left, Armenian13Date right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >=(Armenian13Date left, Armenian13Date right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static Armenian13Date Min(Armenian13Date x, Armenian13Date y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static Armenian13Date Max(Armenian13Date x, Armenian13Date y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(Armenian13Date other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is Armenian13Date date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(Armenian13Date), obj);
}

public partial struct Armenian13Date // Math
{
    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountDaysSince()")]
    public static int operator -(Armenian13Date left, Armenian13Date right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static Armenian13Date operator +(Armenian13Date value, int days) => value.PlusDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static Armenian13Date operator -(Armenian13Date value, int days) => value.PlusDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextDay()")]
    public static Armenian13Date operator ++(Armenian13Date value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousDay()")]
    public static Armenian13Date operator --(Armenian13Date value) => value.PreviousDay();

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(Armenian13Date other) =>
        // No need to use a checked context here. Indeed,
        // MaxDaysSinceEpoch - MinDaysSinceEpoch = MaxDaysSinceEpoch
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public Armenian13Date PlusDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);

        // Don't write (the addition may also overflow...):
        // > Scope.CheckOverflow(Epoch + daysSinceEpoch);
        if ((uint)daysSinceEpoch > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();

        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date NextDay()
    {
        if (_daysSinceEpoch == MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch + 1);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date PreviousDay()
    {
        // NB: MinDaysSinceEpoch = 0.
        if (_daysSinceEpoch == 0) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch - 1);
    }
}

