﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie;

using System.Numerics;

using Calendrie.Core.Utilities;

using static Calendrie.Core.TemporalConstants;

// REVIEW(api): implement IFixedDate? Math ops? etc.

/// <summary>
/// Represents a moment with nanosecond precision.
/// <para><see cref="Moment"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct Moment :
    IEqualityOperators<Moment, Moment, bool>,
    IEquatable<Moment>,
    IComparisonOperators<Moment, Moment, bool>,
    IComparable<Moment>,
    IComparable,
    IMinMaxValue<Moment>
{
    /// <summary>
    /// Represents the day number.
    /// </summary>
    private readonly DayNumber _dayNumber;

    /// <summary>
    /// Represents the time of the day with nanosecond precision.
    /// </summary>
    private readonly InstantOfDay _timeOfDay;

    /// <summary>
    /// Initializes a new instance of the <see cref="Moment"/> struct from the
    /// specified day number and time of the day.
    /// </summary>
    public Moment(DayNumber dayNumber, InstantOfDay timeOfDay)
    {
        _dayNumber = dayNumber;
        _timeOfDay = timeOfDay;
    }

    /// <summary>
    /// Gets the origin.
    /// <para>The Monday 1st of January, 1 CE within the Gregorian calendar at
    /// midnight (0h).</para>
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Moment Zero { get; }

    /// <summary>
    /// Gets the smallest possible value of a <see cref="Moment"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Moment MinValue { get; } = new(DayNumber.MinValue, InstantOfDay.MinValue);

    /// <summary>
    /// Gets the largest possible value of a <see cref="Moment"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Moment MaxValue { get; } = new(DayNumber.MaxValue, InstantOfDay.MaxValue);

    /// <summary>
    /// Gets the day number.
    /// </summary>
    public DayNumber DayNumber => _dayNumber;

    /// <summary>
    /// Gets the time of the day with millisecond precision.
    /// </summary>
    public InstantOfDay InstantOfDay => _timeOfDay;

    /// <summary>
    /// Gets the number of elapsed seconds since <see cref="Zero"/>.
    /// </summary>
    public long SecondsSinceZero =>
        _dayNumber.DaysSinceZero * (long)SecondsPerDay
        + _timeOfDay.MillisecondOfDay / MillisecondsPerSecond;

    /// <summary>
    /// Gets the number of elapsed milliseconds since <see cref="Zero"/>.
    /// </summary>
    public long MillisecondsSinceZero =>
        _dayNumber.DaysSinceZero * (long)MillisecondsPerDay
        + _timeOfDay.MillisecondOfDay;

    /// <summary>
    /// Returns a culture-independent string representation of this instance.
    /// </summary>
    [Pure]
    public override string ToString() =>
        FormattableString.Invariant($"{DayNumber}+{InstantOfDay}");

    /// <summary>
    /// Deconstructs this instance into its components.
    /// </summary>
    public void Deconstruct(out DayNumber dayNumber, out InstantOfDay timeOfDay) =>
        (dayNumber, timeOfDay) = (DayNumber, InstantOfDay);
}

public partial struct Moment // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(Moment left, Moment right) =>
        left._dayNumber == right._dayNumber && left._timeOfDay == right._timeOfDay;

    /// <inheritdoc />
    public static bool operator !=(Moment left, Moment right) =>
        left._dayNumber != right._dayNumber || left._timeOfDay != right._timeOfDay;

    /// <inheritdoc />
    public bool Equals(Moment other) =>
        _dayNumber == other._dayNumber && _timeOfDay == other._timeOfDay;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Moment moment && Equals(moment);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => HashCode.Combine(_dayNumber, _timeOfDay);
}

public partial struct Moment // IComparable
{
    /// <inheritdoc />
    public static bool operator <(Moment left, Moment right) => left.CompareTo(right) < 0;

    /// <inheritdoc />
    public static bool operator <=(Moment left, Moment right) => left.CompareTo(right) <= 0;

    /// <inheritdoc />
    public static bool operator >(Moment left, Moment right) => left.CompareTo(right) > 0;

    /// <inheritdoc />
    public static bool operator >=(Moment left, Moment right) => left.CompareTo(right) >= 0;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(Moment other) =>
        MillisecondsSinceZero.CompareTo(other.MillisecondsSinceZero);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is Moment moment ? CompareTo(moment)
        : ThrowHelpers.ThrowNonComparable(typeof(Moment), obj);
}
