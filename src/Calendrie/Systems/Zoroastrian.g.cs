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
/// Represents the Zoroastrian calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class ZoroastrianCalendar : CalendarSystem<ZoroastrianDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ZoroastrianCalendar"/> class.
    /// </summary>
    public ZoroastrianCalendar() : this(new Egyptian12Schema()) { }

    private ZoroastrianCalendar(Egyptian12Schema schema)
        : base("Zoroastrian", new StandardScope(schema, DayZero.Zoroastrian))
    {
        Debug.Assert(schema != null);
        Schema = schema;
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="ZoroastrianCalendar"/> class.
    /// <para>See <see cref="ZoroastrianDate.Calendar"/>.</para>
    /// </summary>
    internal static ZoroastrianCalendar Instance { get; } = new();

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
    internal Egyptian12Schema Schema { get; }
}

/// <summary>
/// Represents the Zoroastrian date.
/// <para><see cref="ZoroastrianDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct ZoroastrianDate :
    IDateable,
    IAbsoluteDate<ZoroastrianDate>,
    IAdjustableDate<ZoroastrianDate>,
    IDateFactory<ZoroastrianDate>,
    ISubtractionOperators<ZoroastrianDate, ZoroastrianDate, int>
{ }

public partial struct ZoroastrianDate // Preamble
{
    /// <summary>Represents the value of the property <see cref="DayNumber.DaysSinceZero"/>
    /// for the epoch <see cref="DayZero.Zoroastrian"/>.
    /// <para>This field is a constant equal to 230_637.</para></summary>
    private const int EpochDaysSinceZero = 230_637;

    /// <summary>Represents the maximum value of <see cref="_daysSinceEpoch"/>.
    /// <para>This field is a constant equal to 3_649_634.</para></summary>
    private const int MaxDaysSinceEpoch = 3_649_634;

    /// <summary>
    /// Represents the count of consecutive days since the epoch <see cref="DayZero.Zoroastrian"/>.
    /// <para>This field is in the range from 0 to <see cref="MaxDaysSinceEpoch"/>.
    /// </para>
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
        var chr = ZoroastrianCalendar.Instance;
        chr.Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(year, month, day);
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
        var chr = ZoroastrianCalendar.Instance;
        chr.Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(year, dayOfYear);
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
    public static ZoroastrianDate MinValue { get; }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static ZoroastrianDate MaxValue { get; } = new(MaxDaysSinceEpoch);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static ZoroastrianCalendar Calendar => ZoroastrianCalendar.Instance;

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

public partial struct ZoroastrianDate // Factories & conversions
{
    /// <inheritdoc />
    [Pure]
    public static ZoroastrianDate FromDayNumber(DayNumber dayNumber)
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
    static ZoroastrianDate IDateFactory<ZoroastrianDate>.UnsafeCreate(int daysSinceEpoch) =>
        new(daysSinceEpoch);
}

public partial struct ZoroastrianDate // Counting
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

public partial struct ZoroastrianDate // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate WithYear(int newYear)
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
    public ZoroastrianDate WithMonth(int newMonth)
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
    public ZoroastrianDate WithDay(int newDay)
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
    public ZoroastrianDate WithDayOfYear(int newDayOfYear)
    {
        int y = Year;

        var sch = Calendar.Schema;
        // We only need to validate "newDayOfYear".
        sch.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, newDayOfYear);
        return new(daysSinceEpoch);
    }
}

public partial struct ZoroastrianDate // Find close by day of the week
{
    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceEpoch = _daysSinceEpoch + (δ >= 0 ? δ - DaysInWeek : δ);
        if (daysSinceEpoch < 0) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate PreviousOrSame(DayOfWeek dayOfWeek)
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
    public ZoroastrianDate Nearest(DayOfWeek dayOfWeek)
    {
        var nearest = DayNumber.Nearest(dayOfWeek);
        int daysSinceEpoch = nearest.DaysSinceZero - EpochDaysSinceZero;
        if (unchecked((uint)daysSinceEpoch) > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate NextOrSame(DayOfWeek dayOfWeek)
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
    public ZoroastrianDate Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceEpoch = _daysSinceEpoch + (δ <= 0 ? δ + DaysInWeek : δ);
        if (daysSinceEpoch > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
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
    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountDaysSince()")]
    public static int operator -(ZoroastrianDate left, ZoroastrianDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static ZoroastrianDate operator +(ZoroastrianDate value, int days) => value.PlusDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static ZoroastrianDate operator -(ZoroastrianDate value, int days) => value.PlusDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextDay()")]
    public static ZoroastrianDate operator ++(ZoroastrianDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousDay()")]
    public static ZoroastrianDate operator --(ZoroastrianDate value) => value.PreviousDay();

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(ZoroastrianDate other) =>
        // No need to use a checked context here. Indeed, the result is at most
        // equal to (MaxDaysSinceEpoch - MinDaysSinceEpoch) ie MaxDaysSinceEpoch.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate PlusDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);
        if (unchecked((uint)daysSinceEpoch) > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate NextDay()
    {
        if (_daysSinceEpoch == MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch + 1);
    }

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate PreviousDay()
    {
        if (_daysSinceEpoch == 0) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch - 1);
    }
}

