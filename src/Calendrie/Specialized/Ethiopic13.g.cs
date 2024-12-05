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
public sealed partial class Ethiopic13Calendar : SpecialCalendar<Ethiopic13Date>
{
    internal static readonly Coptic13Schema SchemaT = new();
    internal static readonly StandardScope ScopeT = CreateScope();
    internal static readonly Ethiopic13Calendar Instance = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Ethiopic13Calendar"/>
    /// class.
    /// <para>See also <seealso cref="Ethiopic13Date.Calendar"/>.</para>
    /// </summary>
    public Ethiopic13Calendar() : this(CreateScope()) { }

    private Ethiopic13Calendar(StandardScope scope) : base("Ethiopic", scope)
    {
        Adjuster = new Ethiopic13Adjuster(this);
    }

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public Ethiopic13Adjuster Adjuster { get; }

    [Pure]
    private static partial StandardScope CreateScope();

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    // WARNING: the order in which the static fields are written is __important__.

    private static readonly CalendarScope s_Scope = Ethiopic13Calendar.ScopeT;

    private static readonly DayNumber s_Epoch = s_Scope.Epoch;
    private static readonly Range<DayNumber> s_Domain = s_Scope.Domain;
    private static readonly Range<int> s_SupportedDays = s_Scope.Segment.SupportedDays;

    private static readonly Ethiopic13Date s_MinValue = new(s_SupportedDays.Min);
    private static readonly Ethiopic13Date s_MaxValue = new(s_SupportedDays.Max);

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
        s_Scope.ValidateYearMonthDay(year, month, day);

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
        s_Scope.ValidateOrdinal(year, dayOfYear);

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
    private static Coptic13Schema Schema => Ethiopic13Calendar.SchemaT;

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

        return new(dayNumber - s_Epoch);
    }
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
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        s_Domain.CheckOverflow(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return new(dayNumber - s_Epoch);
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
        // > s_Domain.CheckOverflow(s_Epoch + daysSinceEpoch);
        s_SupportedDays.CheckOverflow(daysSinceEpoch);
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
