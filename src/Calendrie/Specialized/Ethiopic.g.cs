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
/// Represents the Ethiopic calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class EthiopicCalendar : SpecialCalendar<EthiopicDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EthiopicCalendar"/>
    /// class.
    /// <para>See also <seealso cref="EthiopicDate.Calendar"/>.</para>
    /// </summary>
    public EthiopicCalendar() : this(CreateScope()) { }

    private EthiopicCalendar(StandardScope scope) : base("Ethiopic", scope)
    {
        Adjuster = new EthiopicAdjuster(this);
    }

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public EthiopicAdjuster Adjuster { get; }

    [Pure]
    private static partial StandardScope CreateScope();

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private protected sealed override EthiopicDate NewDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Provides common adjusters for <see cref="EthiopicDate"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class EthiopicAdjuster : SpecialAdjuster<EthiopicDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EthiopicAdjuster"/>
    /// class.
    /// </summary>
    internal EthiopicAdjuster(EthiopicCalendar calendar) : base(calendar) { }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private protected sealed override EthiopicDate NewDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Represents the Ethiopic date.
/// <para><see cref="EthiopicDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct EthiopicDate :
    IDate<EthiopicDate, EthiopicCalendar>,
    IAdjustable<EthiopicDate>
{ }

public partial struct EthiopicDate // Preamble
{
    // WARNING: the order in which the static fields are written is __important__.

    private static readonly EthiopicCalendar s_Calendar = new();
    private static readonly Coptic12Schema s_Schema = (Coptic12Schema)s_Calendar.Schema;
    private static readonly CalendarScope s_Scope = s_Calendar.Scope;

    private static readonly DayNumber s_Epoch = s_Scope.Epoch;
    private static readonly Range<DayNumber> s_Domain = s_Scope.Domain;
    private static readonly Range<int> s_SupportedDays = s_Scope.Segment.SupportedDays;

    private static readonly EthiopicDate s_MinValue = new(s_SupportedDays.Min);
    private static readonly EthiopicDate s_MaxValue = new(s_SupportedDays.Max);

    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="EthiopicDate"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid date or <paramref name="year"/> is outside the range of supported
    /// years.</exception>
    public EthiopicDate(int year, int month, int day)
    {
        s_Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EthiopicDate"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid ordinal date or <paramref name="year"/> is outside the range of
    /// supported years.</exception>
    public EthiopicDate(int year, int dayOfYear)
    {
        s_Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// This constructor does NOT validate its parameter.
    /// </summary>
    internal EthiopicDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static EthiopicDate MinValue => s_MinValue;

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static EthiopicDate MaxValue => s_MaxValue;

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static EthiopicAdjuster Adjuster => s_Calendar.Adjuster;

    /// <inheritdoc />
    public static EthiopicCalendar Calendar => s_Calendar;

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

public partial struct EthiopicDate // Factories
{
    /// <inheritdoc />
    [Pure]
    public static EthiopicDate FromDayNumber(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        return new(dayNumber - s_Epoch);
    }
}

public partial struct EthiopicDate // Counting
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

public partial struct EthiopicDate // Adjustments
{
    /// <inheritdoc />
    /// <remarks>See also <seealso cref="Adjuster"/>.</remarks>
    [Pure]
    public EthiopicDate Adjust(Func<EthiopicDate, EthiopicDate> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        return adjuster.Invoke(this);
    }

    /// <inheritdoc />
    [Pure]
    public EthiopicDate Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public EthiopicDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public EthiopicDate Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        s_Domain.CheckOverflow(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public EthiopicDate NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public EthiopicDate Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return new(dayNumber - s_Epoch);
    }
}

public partial struct EthiopicDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(EthiopicDate left, EthiopicDate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(EthiopicDate left, EthiopicDate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(EthiopicDate other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is EthiopicDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct EthiopicDate // IComparable
{
    /// <inheritdoc />
    public static bool operator <(EthiopicDate left, EthiopicDate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator <=(EthiopicDate left, EthiopicDate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >(EthiopicDate left, EthiopicDate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >=(EthiopicDate left, EthiopicDate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static EthiopicDate Min(EthiopicDate x, EthiopicDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static EthiopicDate Max(EthiopicDate x, EthiopicDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(EthiopicDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is EthiopicDate date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(EthiopicDate), obj);
}

public partial struct EthiopicDate // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    public static int operator -(EthiopicDate left, EthiopicDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static EthiopicDate operator +(EthiopicDate value, int days) => value.AddDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static EthiopicDate operator -(EthiopicDate value, int days) => value.AddDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    public static EthiopicDate operator ++(EthiopicDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    public static EthiopicDate operator --(EthiopicDate value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(EthiopicDate other) =>
        // No need to use a checked context here.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public EthiopicDate AddDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);
        // Don't write (the addition may also overflow...):
        // > s_Domain.CheckOverflow(s_Epoch + daysSinceEpoch);
        s_SupportedDays.CheckOverflow(daysSinceEpoch);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public EthiopicDate NextDay()
    {
        if (this == s_MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch + 1);
    }

    /// <inheritdoc />
    [Pure]
    public EthiopicDate PreviousDay()
    {
        if (this == s_MinValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch - 1);
    }
}
