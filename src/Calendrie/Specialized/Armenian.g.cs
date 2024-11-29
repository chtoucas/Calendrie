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
using Calendrie.Hemerology.Scopes;

/// <summary>
/// Represents the Armenian calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class ArmenianCalendar : SpecialCalendar<ArmenianDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArmenianCalendar"/>
    /// class.
    /// </summary>
    public ArmenianCalendar() : this(new Egyptian12Schema()) { }

    internal ArmenianCalendar(Egyptian12Schema schema) : base("Armenian", GetScope(schema))
    {
        OnInitializing(schema);
    }

    [Pure]
    private static partial MinMaxYearScope GetScope(Egyptian12Schema schema);

    partial void OnInitializing(Egyptian12Schema schema);

    [Pure]
    private protected sealed override ArmenianDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Provides common adjusters for <see cref="ArmenianDate"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class ArmenianAdjuster : SpecialAdjuster<ArmenianDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArmenianAdjuster"/>
    /// class.
    /// </summary>
    public ArmenianAdjuster() : base(ArmenianDate.Calendar.Scope) { }

    internal ArmenianAdjuster(MinMaxYearScope scope) : base(scope) { }

    [Pure]
    private protected sealed override ArmenianDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Represents the Armenian date.
/// <para><see cref="ArmenianDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct ArmenianDate :
    IDate<ArmenianDate, ArmenianCalendar>,
    IAdjustable<ArmenianDate>
{
    // WARNING: the order in which the static fields are written is __important__.

    private static readonly Egyptian12Schema s_Schema = new();
    private static readonly ArmenianCalendar s_Calendar = new(s_Schema);
    private static readonly MinMaxYearScope s_Scope = s_Calendar.Scope;
    private static readonly DayNumber s_Epoch = s_Calendar.Epoch;
    private static readonly Range<DayNumber> s_Domain = s_Calendar.Domain;
    private static readonly ArmenianAdjuster s_Adjuster = new(s_Scope);
    private static readonly ArmenianDate s_MinValue = new(s_Domain.Min - s_Epoch);
    private static readonly ArmenianDate s_MaxValue = new(s_Domain.Max - s_Epoch);

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
        s_Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, month, day);
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
        s_Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, dayOfYear);
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

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static ArmenianAdjuster Adjuster => s_Adjuster;

    /// <inheritdoc />
    public static ArmenianCalendar Calendar => s_Calendar;

    /// <inheritdoc />
    public DayNumber DayNumber => s_Epoch + _daysSinceEpoch;

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
    public int Year => s_Schema.GetYear(_daysSinceEpoch);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = s_Schema.GetYear(_daysSinceEpoch, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
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
            s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return s_Schema.IsIntercalaryDay(y, m, d);
        }
    }

    /// <inheritdoc />
    public bool IsSupplementary
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return s_Schema.IsSupplementaryDay(y, m, d);
        }
    }

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({s_Calendar})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        s_Schema.GetDateParts(_daysSinceEpoch, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = s_Schema.GetYear(_daysSinceEpoch, out dayOfYear);
}

public partial struct ArmenianDate // Factories
{
    /// <inheritdoc />
    [Pure]
    public static ArmenianDate FromDayNumber(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        return new ArmenianDate(dayNumber - s_Epoch);
    }
}

public partial struct ArmenianDate // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() => s_Schema.CountDaysInYearBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() => s_Schema.CountDaysInYearAfter(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => s_Schema.CountDaysInMonthBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() => s_Schema.CountDaysInMonthAfter(_daysSinceEpoch);
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
        if (!s_Domain.Contains(dayNumber)) ThrowHelpers.ThrowDateOverflow();
        return new ArmenianDate(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public ArmenianDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        if (!s_Domain.Contains(dayNumber)) ThrowHelpers.ThrowDateOverflow();
        return new ArmenianDate(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public ArmenianDate Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        if (!s_Domain.Contains(dayNumber)) ThrowHelpers.ThrowDateOverflow();
        return new ArmenianDate(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public ArmenianDate NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        if (!s_Domain.Contains(dayNumber)) ThrowHelpers.ThrowDateOverflow();
        return new ArmenianDate(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public ArmenianDate Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        if (!s_Domain.Contains(dayNumber)) ThrowHelpers.ThrowDateOverflow();
        return new ArmenianDate(dayNumber - s_Epoch);
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
        // > s_Domain.CheckOverflow(s_Epoch + daysSinceEpoch);
        s_Scope.DaysValidator.CheckOverflow(daysSinceEpoch);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public ArmenianDate NextDay()
    {
        if (this == s_MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new ArmenianDate(_daysSinceEpoch + 1);
    }

    /// <inheritdoc />
    [Pure]
    public ArmenianDate PreviousDay()
    {
        if (this == s_MinValue) ThrowHelpers.ThrowDateOverflow();
        return new ArmenianDate(_daysSinceEpoch - 1);
    }
}

