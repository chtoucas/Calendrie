﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable

namespace Calendrie.Specialized;

using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Core.Validation;
using Calendrie.Hemerology;
using Calendrie.Hemerology.Scopes;

/// <summary>
/// Represents the Zoroastrian calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class ZoroastrianCalendar : SpecialCalendar<ZoroastrianDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ZoroastrianCalendar"/>
    /// class.
    /// </summary>
    public ZoroastrianCalendar() : this(new Egyptian12Schema()) { }

    internal ZoroastrianCalendar(Egyptian12Schema schema) : base("Zoroastrian", GetScope(schema))
    {
        OnInitializing(schema);
    }

    private static partial MinMaxYearScope GetScope(Egyptian12Schema schema);

    partial void OnInitializing(Egyptian12Schema schema);

    private protected sealed override ZoroastrianDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Provides common adjusters for <see cref="ZoroastrianDate"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class ZoroastrianAdjuster : SpecialAdjuster<ZoroastrianDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ZoroastrianAdjuster"/>
    /// class.
    /// </summary>
    public ZoroastrianAdjuster() : base(ZoroastrianDate.Calendar.Scope) { }

    internal ZoroastrianAdjuster(MinMaxYearScope scope) : base(scope) { }

    private protected sealed override ZoroastrianDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Represents the Zoroastrian date.
/// <para><see cref="ZoroastrianDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct ZoroastrianDate :
    IDate<ZoroastrianDate, ZoroastrianCalendar>,
    IAdjustable<ZoroastrianDate>
{
    // WARNING: the order in which the static fields are written is __important__.

    private static readonly Egyptian12Schema s_Schema = new();
    private static readonly ZoroastrianCalendar s_Calendar = new(s_Schema);
    private static readonly MinMaxYearScope s_Scope = s_Calendar.Scope;
    private static readonly DayNumber s_Epoch = s_Calendar.Epoch;
    private static readonly Range<DayNumber> s_Domain = s_Calendar.Domain;
    private static readonly ZoroastrianAdjuster s_Adjuster = new(s_Scope);
    private static readonly ZoroastrianDate s_MinValue = new(s_Domain.Min - s_Epoch);
    private static readonly ZoroastrianDate s_MaxValue = new(s_Domain.Max - s_Epoch);

    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="ZoroastrianDate"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid date or <paramref name="year"/> is outside the range of supported
    /// years.</exception>
    public ZoroastrianDate(int year, int month, int day)
    {
        s_Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ZoroastrianDate"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid ordinal date or <paramref name="year"/> is outside the range of
    /// supported years.</exception>
    public ZoroastrianDate(int year, int dayOfYear)
    {
        s_Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, dayOfYear);
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

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static ZoroastrianAdjuster Adjuster => s_Adjuster;

    /// <inheritdoc />
    public static ZoroastrianCalendar Calendar => s_Calendar;

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

public partial struct ZoroastrianDate // Factories
{
    /// <summary>
    /// Creates a new instance of the <see cref="ZoroastrianDate"/> struct from
    /// the specified day number.
    /// </summary>
    /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside
    /// the range of supported values.</exception>
    public static ZoroastrianDate FromDayNumber(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        return new ZoroastrianDate(dayNumber - s_Epoch);
    }
}

public partial struct ZoroastrianDate // Counting
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
    public ZoroastrianDate Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new ZoroastrianDate(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new ZoroastrianDate(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new ZoroastrianDate(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new ZoroastrianDate(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new ZoroastrianDate(dayNumber - s_Epoch);
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
        : Throw.NonComparable(typeof(ZoroastrianDate), obj);
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
    /// the capacity of <see cref="Int32"/> or the range of supported dates.
    /// </exception>
    public static ZoroastrianDate operator +(ZoroastrianDate value, int days) => value.PlusDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="Int32"/> or the range of supported dates.
    /// </exception>
    public static ZoroastrianDate operator -(ZoroastrianDate value, int days) => value.PlusDays(-days);

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
    public ZoroastrianDate PlusDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);
        // Don't write (the addition may also overflow...):
        // > s_Domain.CheckOverflow(s_Epoch + daysSinceEpoch);
        s_Scope.DaysValidator.CheckOverflow(daysSinceEpoch);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate NextDay() =>
        this == s_MaxValue ? Throw.DateOverflow<ZoroastrianDate>()
        : new ZoroastrianDate(_daysSinceEpoch + 1);

    /// <inheritdoc />
    [Pure]
    public ZoroastrianDate PreviousDay() =>
        this == s_MinValue ? Throw.DateOverflow<ZoroastrianDate>()
        : new ZoroastrianDate(_daysSinceEpoch - 1);
}
