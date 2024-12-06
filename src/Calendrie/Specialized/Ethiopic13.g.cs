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
/// <see cref="Ethiopic13Calendar"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static class Ethiopic13Scope
{
    // WARNING: the order in which the static fields are written is __important__.

    public static readonly DayNumber Epoch = DayZero.Ethiopic;
    public static readonly Coptic13Schema Schema = new();

    public static readonly StandardScope Instance = new(Schema, Epoch);

    public static Range<DayNumber> Domain => Instance.Domain;

    public static int MinDaysSinceEpoch => Instance.Segment.SupportedDays.Min;
    public static int MaxDaysSinceEpoch => Instance.Segment.SupportedDays.Max;

    public static StandardScope Create() => new(new Coptic13Schema(), Epoch);
}

/// <summary>
/// Represents the Ethiopic calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class Ethiopic13Calendar : SpecialCalendar<Ethiopic13Date>
{
    internal static readonly Ethiopic13Calendar Instance = new(Ethiopic13Scope.Instance);

    /// <summary>
    /// Initializes a new instance of the <see cref="Ethiopic13Calendar"/>
    /// class.
    /// <para>See also <seealso cref="Ethiopic13Date.Calendar"/>.</para>
    /// </summary>
    public Ethiopic13Calendar() : this(Ethiopic13Scope.Create()) { }

    private Ethiopic13Calendar(StandardScope scope) : base("Ethiopic", scope)
    {
        Adjuster = new Ethiopic13Adjuster(this);
    }

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public Ethiopic13Adjuster Adjuster { get; }

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private protected sealed override Ethiopic13Date NewDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Provides common adjusters for <see cref="Ethiopic13Date"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class Ethiopic13Adjuster : SpecialAdjuster<Ethiopic13Date>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Ethiopic13Adjuster"/>
    /// class.
    /// </summary>
    internal Ethiopic13Adjuster(Ethiopic13Calendar calendar) : base(calendar) { }

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private protected sealed override Ethiopic13Date NewDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Represents the Ethiopic date.
/// <para><see cref="Ethiopic13Date"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct Ethiopic13Date :
    IDate<Ethiopic13Date, Ethiopic13Calendar>,
    IAdjustable<Ethiopic13Date>
{ }

public partial struct Ethiopic13Date // Preamble
{
    private static readonly DayNumber s_Epoch = Ethiopic13Scope.Epoch;
    private static readonly Range<DayNumber> s_Domain = Ethiopic13Scope.Domain;

    private static readonly int s_MinDaysSinceEpoch = Ethiopic13Scope.MinDaysSinceEpoch;
    private static readonly int s_MaxDaysSinceEpoch = Ethiopic13Scope.MaxDaysSinceEpoch;

    private static readonly Ethiopic13Date s_MinValue = new(Ethiopic13Scope.MinDaysSinceEpoch);
    private static readonly Ethiopic13Date s_MaxValue = new(Ethiopic13Scope.MaxDaysSinceEpoch);

    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="Ethiopic13Date"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid date or <paramref name="year"/> is outside the range of supported
    /// years.</exception>
    public Ethiopic13Date(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Ethiopic13Date"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid ordinal date or <paramref name="year"/> is outside the range of
    /// supported years.</exception>
    public Ethiopic13Date(int year, int dayOfYear)
    {
        Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = Schema.CountDaysSinceEpoch(year, dayOfYear);
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

    /// <inheritdoc />
    public static Ethiopic13Calendar Calendar => Ethiopic13Calendar.Instance;

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Ethiopic13Adjuster Adjuster => Ethiopic13Calendar.Instance.Adjuster;

    /// <inheritdoc />
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
    private static Coptic13Schema Schema => Ethiopic13Scope.Schema;

    /// <summary>
    /// Gets the calendar scope.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    private static StandardScope Scope => Ethiopic13Scope.Instance;

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

public partial struct Ethiopic13Date // Factories
{
    /// <inheritdoc />
    [Pure]
    public static Ethiopic13Date FromDayNumber(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        // We know that the subtraction won't overflow
        // > return new(dayNumber - s_Epoch);
        return new(dayNumber.DaysSinceZero - s_Epoch.DaysSinceZero);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Ethiopic13Date"/> struct
    /// from the specified day number.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Ethiopic13Date FromDayNumberUnchecked(DayNumber dayNumber) =>
        new(dayNumber.DaysSinceZero - s_Epoch.DaysSinceZero);
}

public partial struct Ethiopic13Date // Counting
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

public partial struct Ethiopic13Date // Adjustments
{
    /// <inheritdoc />
    /// <remarks>See also <seealso cref="Adjuster"/>.</remarks>
    [Pure]
    public Ethiopic13Date Adjust(Func<Ethiopic13Date, Ethiopic13Date> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        return adjuster.Invoke(this);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        s_Domain.CheckOverflow(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
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
        // > s_Domain.CheckOverflow(Epoch + daysSinceEpoch);
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

