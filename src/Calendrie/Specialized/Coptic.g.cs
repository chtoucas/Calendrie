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
/// <see cref="CopticCalendar"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static partial class CopticScope
{
    // WARNING: the order in which the static fields are written is __important__.

    public static partial DayNumber Epoch { get; }

    public static readonly Coptic12Schema Schema = new();

    public static readonly StandardScope Instance = new(Schema, Epoch);
}

/// <summary>
/// Represents the Coptic calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class CopticCalendar : SpecialCalendar<CopticDate>
{
    internal static readonly CopticCalendar Instance = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CopticCalendar"/>
    /// class.
    /// <para>See also <seealso cref="CopticDate.Calendar"/>.</para>
    /// </summary>
    public CopticCalendar() :
        this(new StandardScope(new Coptic12Schema(), CopticScope.Epoch))
    { }

    private CopticCalendar(StandardScope scope) : base("Coptic", scope)
    {
        Adjuster = new CopticAdjuster(this);
    }

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public CopticAdjuster Adjuster { get; }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private protected sealed override CopticDate NewDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Provides common adjusters for <see cref="CopticDate"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class CopticAdjuster : SpecialAdjuster<CopticDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CopticAdjuster"/>
    /// class.
    /// </summary>
    internal CopticAdjuster(CopticCalendar calendar) : base(calendar) { }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private protected sealed override CopticDate NewDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Represents the Coptic date.
/// <para><see cref="CopticDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct CopticDate :
    IDate<CopticDate, CopticCalendar>,
    IAdjustable<CopticDate>
{ }

public partial struct CopticDate // Preamble
{
    // WARNING: the order in which the static fields are written is __important__.

    private static readonly DayNumber s_Epoch = CopticScope.Instance.Epoch;
    private static readonly Range<DayNumber> s_Domain = CopticScope.Instance.Domain;

    private static readonly int s_MinDaysSinceEpoch = CopticScope.Instance.Segment.SupportedDays.Min;
    private static readonly int s_MaxDaysSinceEpoch = CopticScope.Instance.Segment.SupportedDays.Max;

    private static readonly CopticDate s_MinValue = new(s_MinDaysSinceEpoch);
    private static readonly CopticDate s_MaxValue = new(s_MaxDaysSinceEpoch);

    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="CopticDate"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid date or <paramref name="year"/> is outside the range of supported
    /// years.</exception>
    public CopticDate(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CopticDate"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid ordinal date or <paramref name="year"/> is outside the range of
    /// supported years.</exception>
    public CopticDate(int year, int dayOfYear)
    {
        Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// This constructor does NOT validate its parameter.
    /// </summary>
    internal CopticDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static CopticDate MinValue => s_MinValue;

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static CopticDate MaxValue => s_MaxValue;

    /// <inheritdoc />
    public static CopticCalendar Calendar => CopticCalendar.Instance;

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static CopticAdjuster Adjuster => CopticCalendar.Instance.Adjuster;

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
    /// </summary>
    private static Coptic12Schema Schema => CopticScope.Schema;

    /// <summary>
    /// Gets the calendar scope.
    /// </summary>
    private static StandardScope Scope => CopticScope.Instance;

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

public partial struct CopticDate // Factories
{
    /// <inheritdoc />
    [Pure]
    public static CopticDate FromDayNumber(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        return new(dayNumber - s_Epoch);
    }
}

public partial struct CopticDate // Counting
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

public partial struct CopticDate // Adjustments
{
    /// <inheritdoc />
    /// <remarks>See also <seealso cref="Adjuster"/>.</remarks>
    [Pure]
    public CopticDate Adjust(Func<CopticDate, CopticDate> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        return adjuster.Invoke(this);
    }

    /// <inheritdoc />
    [Pure]
    public CopticDate Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public CopticDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public CopticDate Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        s_Domain.CheckOverflow(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public CopticDate NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public CopticDate Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return new(dayNumber - s_Epoch);
    }
}

public partial struct CopticDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(CopticDate left, CopticDate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(CopticDate left, CopticDate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(CopticDate other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CopticDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct CopticDate // IComparable
{
    /// <inheritdoc />
    public static bool operator <(CopticDate left, CopticDate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator <=(CopticDate left, CopticDate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >(CopticDate left, CopticDate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >=(CopticDate left, CopticDate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static CopticDate Min(CopticDate x, CopticDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static CopticDate Max(CopticDate x, CopticDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(CopticDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is CopticDate date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(CopticDate), obj);
}

public partial struct CopticDate // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    public static int operator -(CopticDate left, CopticDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static CopticDate operator +(CopticDate value, int days) => value.AddDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static CopticDate operator -(CopticDate value, int days) => value.AddDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    public static CopticDate operator ++(CopticDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    public static CopticDate operator --(CopticDate value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(CopticDate other) =>
        // No need to use a checked context here.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public CopticDate AddDays(int days)
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
    public CopticDate NextDay()
    {
        if (this == s_MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch + 1);
    }

    /// <inheritdoc />
    [Pure]
    public CopticDate PreviousDay()
    {
        if (this == s_MinValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch - 1);
    }
}

