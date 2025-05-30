﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie;

using System.Numerics;

using Calendrie.Core;
using Calendrie.Core.Utilities;

using static Calendrie.Core.TemporalConstants;

/// <summary>
/// Represents an instant of the day with nanosecond precision.
/// <para>Nanosecond precision does not necessarily mean nanosecond resolution
/// (clock frequency).</para>
/// <para><see cref="InstantOfDay"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct InstantOfDay :
    IEqualityOperators<InstantOfDay, InstantOfDay, bool>,
    IEquatable<InstantOfDay>,
    IComparisonOperators<InstantOfDay, InstantOfDay, bool>,
    IComparable<InstantOfDay>,
    IComparable,
    IMinMaxValue<InstantOfDay>
{
    /// <summary>
    /// Represents the number of elapsed nanoseconds since midnight.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly long _nanosecondOfDay;

    /// <summary>
    /// Initializes a new instance of <see cref="InstantOfDay"/> from the
    /// specified number of elapsed nanoseconds since midnight.
    /// <para>This constructor does NOT validate its parameter.</para>
    /// </summary>
    private InstantOfDay(long nanosecondOfDay)
    {
        Debug.Assert(nanosecondOfDay >= 0);

        _nanosecondOfDay = nanosecondOfDay;
    }

    /// <summary>
    /// Represents the smallest possible value of a <see cref="InstantOfDay"/>;
    /// this property is strictly equivalent to <see cref="Midnight"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static InstantOfDay MinValue => Midnight;

    /// <summary>
    /// Represents the largest possible value of a <see cref="InstantOfDay"/>;
    /// one nanosecond before midnight.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static InstantOfDay MaxValue { get; } = new(NanosecondsPerDay - 1);

    /// <summary>
    /// Gets the <see cref="InstantOfDay"/> value at 00:00.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static InstantOfDay Midnight { get; }

    /// <summary>
    /// Gets the <see cref="InstantOfDay"/> value at 12:00.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static InstantOfDay Noon { get; } = new(NanosecondsPerDay / 2);

    /// <summary>
    /// Gets the hour of the day.
    /// <para>The result is in the range from 0 to 23.</para>
    /// </summary>
    public int Hour => TemporalArithmetic.DivideByNanosecondsPerHour(_nanosecondOfDay);

    /// <summary>
    /// Gets the hour using a 12-hour clock.
    /// <para>The result is in the range from 1 to 12.</para>
    /// </summary>
    public int HourOfHalfDay
    {
        get
        {
            int value = Hour % 12;
            return value == 0 ? 12 : value;
        }
    }

    /// <summary>
    /// Returns <see langword="true"/> if the current instance is before midday;
    /// otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsAnteMeridiem => Hour < 12;

    /// <summary>
    /// Gets the minute of the hour.
    /// <para>The result is in the range from 0 to 59.</para>
    /// </summary>
    public int Minute =>
        TemporalArithmetic.DivideByNanosecondsPerMinute(_nanosecondOfDay) % MinutesPerHour;

    /// <summary>
    /// Gets the second of the minute.
    /// <para>The result is in the range from 0 to 59.</para>
    /// </summary>
    public int Second => SecondOfDay % SecondsPerMinute;

    /// <summary>
    /// Gets the millisecond of the <i>second</i>.
    /// <para>The result is in the range from 0 to 999.</para>
    /// </summary>
    public int Millisecond => MillisecondOfDay % MillisecondsPerSecond;

    /// <summary>
    /// Gets the microsecond of the <i>second</i>.
    /// <para>The result is in the range from 0 to 999_999.</para>
    /// </summary>
    public int Microsecond => (int)(MicrosecondOfDay % MicrosecondsPerSecond);

    /// <summary>
    /// Gets the tick of the <i>second</i>.
    /// <para>The result is in the range from 0 to 9_999_999.</para>
    /// </summary>
    public int Tick => (int)(TickOfDay % TicksPerSecond);

    /// <summary>
    /// Gets the nanosecond of the <i>second</i>.
    /// <para>The result is in the range from 0 to 999_999_999.</para>
    /// </summary>
    public int Nanosecond => (int)(_nanosecondOfDay % NanosecondsPerSecond);

    /// <summary>
    /// Gets the number of elapsed seconds since midnight.
    /// <para>The result is in the range from 0 to 86_399.</para>
    /// </summary>
    public int SecondOfDay => (int)(_nanosecondOfDay / NanosecondsPerSecond);

    /// <summary>
    /// Gets the number of elapsed milliseconds since midnight.
    /// <para>The result is in the range from 0 to 86_399_999.</para>
    /// </summary>
    public int MillisecondOfDay => (int)(_nanosecondOfDay / NanosecondsPerMillisecond);

    /// <summary>
    /// Gets the number of elapsed microseconds since midnight.
    /// <para>The result is in the range from 0 to 86_399_999_999.</para>
    /// </summary>
    public long MicrosecondOfDay => _nanosecondOfDay / NanosecondsPerMicrosecond;

    /// <summary>
    /// Gets the number of elapsed ticks since midnight.
    /// <para>The result is in the range from 0 to 863_999_999_999.</para>
    /// </summary>
    public long TickOfDay => _nanosecondOfDay / NanosecondsPerTick;

    /// <summary>
    /// Gets the number of elapsed nanoseconds since midnight.
    /// <para>The result is in the range from 0 to 86_399_999_999_999.</para>
    /// </summary>
    public long NanosecondOfDay => _nanosecondOfDay;

    /// <summary>
    /// Returns a culture-independent string representation of this instance.
    /// </summary>
    public override string ToString() =>
        FormattableString.Invariant($"{Hour:D2}:{Minute:D2}:{Second:D2}.{Nanosecond:D9}");

    /// <summary>
    /// Deconstructs the current instance into its components.
    /// </summary>
    public void Deconstruct(out int hour, out int minute, out int second) =>
        (hour, minute, second) = (Hour, Minute, Second);

    /// <summary>
    /// Deconstructs the current instance into its components.
    /// </summary>
    public void Deconstruct(out int hour, out int minute, out int second, out int nanosecond) =>
        (hour, minute, second, nanosecond) = (Hour, Minute, Second, Nanosecond);
}

public partial struct InstantOfDay // Factories, conversions...
{
    #region Factories using (hh, mm, ss, subunit-of-second)

    /// <summary>
    /// Creates a new instance of <see cref="InstantOfDay"/> from the specified
    /// hour-of-day.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="hour"/> is
    /// out of range.</exception>
    [Pure]
    public static InstantOfDay FromHour(int hour)
    {
        if ((uint)hour >= HoursPerDay) throw new ArgumentOutOfRangeException(nameof(hour));

        return new InstantOfDay(TemporalArithmetic.MultiplyByNanosecondsPerHour(hour));
    }

    /// <summary>
    /// Creates a new instance of <see cref="InstantOfDay"/> from the specified
    /// hour-of-day and minute-of-hour.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">One of the parameters is
    /// out of range.</exception>
    [Pure]
    public static InstantOfDay FromHourMinute(int hour, int minute)
    {
        if ((uint)hour >= HoursPerDay) throw new ArgumentOutOfRangeException(nameof(hour));
        if ((uint)minute >= MinutesPerHour) throw new ArgumentOutOfRangeException(nameof(minute));

        long nanosecondOfDay =
            TemporalArithmetic.MultiplyByNanosecondsPerHour(hour)
            + NanosecondsPerMinute * minute;

        return new InstantOfDay(nanosecondOfDay);
    }

    /// <summary>
    /// Creates a new instance of <see cref="InstantOfDay"/> from the specified
    /// hour-of-day, minute-of-hour and second-of-minute.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">One of the parameters is
    /// out of range.</exception>
    [Pure]
    public static InstantOfDay FromHourMinuteSecond(int hour, int minute, int second)
    {
        if ((uint)hour >= HoursPerDay) throw new ArgumentOutOfRangeException(nameof(hour));
        if ((uint)minute >= MinutesPerHour) throw new ArgumentOutOfRangeException(nameof(minute));
        if ((uint)second >= SecondsPerMinute) throw new ArgumentOutOfRangeException(nameof(second));

        long nanosecondOfDay =
            TemporalArithmetic.MultiplyByNanosecondsPerHour(hour)
            + NanosecondsPerMinute * minute
            + (long)NanosecondsPerSecond * second;

        return new InstantOfDay(nanosecondOfDay);
    }

    /// <summary>
    /// Creates a new instance of <see cref="InstantOfDay"/> from the specified
    /// hour-of-day, minute-of-hour, second-of-minute and millisecond-of-second.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">One of the parameters is
    /// out of range.</exception>
    [Pure]
    public static InstantOfDay FromHourMinuteSecondMillisecond(
        int hour, int minute, int second, int millisecond)
    {
        if ((uint)hour >= HoursPerDay) throw new ArgumentOutOfRangeException(nameof(hour));
        if ((uint)minute >= MinutesPerHour) throw new ArgumentOutOfRangeException(nameof(minute));
        if ((uint)second >= SecondsPerMinute) throw new ArgumentOutOfRangeException(nameof(second));
        if ((uint)millisecond >= MillisecondsPerSecond)
            throw new ArgumentOutOfRangeException(nameof(millisecond));

        long nanosecondOfDay =
            TemporalArithmetic.MultiplyByNanosecondsPerHour(hour)
            + NanosecondsPerMinute * minute
            + (long)NanosecondsPerSecond * second
            + (long)NanosecondsPerMillisecond * millisecond;

        return new InstantOfDay(nanosecondOfDay);
    }

    /// <summary>
    /// Creates a new instance of <see cref="InstantOfDay"/> from the specified
    /// hour-of-day, minute-of-hour, second-of-minute and microsecond-of-second.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">One of the parameters is
    /// out of range.</exception>
    [Pure]
    public static InstantOfDay FromHourMinuteSecondMicrosecond(
        int hour, int minute, int second, int microsecond)
    {
        if ((uint)hour >= HoursPerDay) throw new ArgumentOutOfRangeException(nameof(hour));
        if ((uint)minute >= MinutesPerHour) throw new ArgumentOutOfRangeException(nameof(minute));
        if ((uint)second >= SecondsPerMinute) throw new ArgumentOutOfRangeException(nameof(second));
        if ((uint)microsecond >= MicrosecondsPerSecond)
            throw new ArgumentOutOfRangeException(nameof(microsecond));

        long nanosecondOfDay =
            TemporalArithmetic.MultiplyByNanosecondsPerHour(hour)
            + NanosecondsPerMinute * minute
            + (long)NanosecondsPerSecond * second
            + (long)NanosecondsPerMicrosecond * microsecond;

        return new InstantOfDay(nanosecondOfDay);
    }

    /// <summary>
    /// Creates a new instance of <see cref="InstantOfDay"/> from the specified
    /// hour-of-day, minute-of-hour, second-of-minute and tick-of-second.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">One of the parameters is
    /// out of range.</exception>
    [Pure]
    public static InstantOfDay FromHourMinuteSecondTick(
        int hour, int minute, int second, int tick)
    {
        if ((uint)hour >= HoursPerDay) throw new ArgumentOutOfRangeException(nameof(hour));
        if ((uint)minute >= MinutesPerHour) throw new ArgumentOutOfRangeException(nameof(minute));
        if ((uint)second >= SecondsPerMinute) throw new ArgumentOutOfRangeException(nameof(second));
        if ((uint)tick >= TicksPerSecond) throw new ArgumentOutOfRangeException(nameof(tick));

        long nanosecondOfDay =
            TemporalArithmetic.MultiplyByNanosecondsPerHour(hour)
            + NanosecondsPerMinute * minute
            + (long)NanosecondsPerSecond * second
            + (long)NanosecondsPerTick * tick;

        return new InstantOfDay(nanosecondOfDay);
    }

    /// <summary>
    /// Creates a new instance of <see cref="InstantOfDay"/> from the specified
    /// hour-of-day, minute-of-hour, second-of-minute and nanosecond-of-second.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">One of the parameters is
    /// out of range.</exception>
    [Pure]
    public static InstantOfDay FromHourMinuteSecondNanosecond(
        int hour, int minute, int second, int nanosecond)
    {
        if ((uint)hour >= HoursPerDay) throw new ArgumentOutOfRangeException(nameof(hour));
        if ((uint)minute >= MinutesPerHour) throw new ArgumentOutOfRangeException(nameof(minute));
        if ((uint)second >= SecondsPerMinute) throw new ArgumentOutOfRangeException(nameof(second));
        if ((uint)nanosecond >= NanosecondsPerSecond)
            throw new ArgumentOutOfRangeException(nameof(nanosecond));

        long nanosecondOfDay =
            TemporalArithmetic.MultiplyByNanosecondsPerHour(hour)
            + NanosecondsPerMinute * minute
            + (long)NanosecondsPerSecond * second
            + nanosecond;

        return new InstantOfDay(nanosecondOfDay);
    }

    #endregion
    #region Factories using a subunit-of-day

    // FromHourOfDay() is named FromHour().

    /// <summary>
    /// Creates a new instance of <see cref="InstantOfDay"/> from the specified
    /// elapsed minutes since midnight.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minuteOfDay"/>
    /// is out of range.</exception>
    [Pure]
    public static InstantOfDay FromMinuteOfDay(int minuteOfDay)
    {
        if ((uint)minuteOfDay >= MinutesPerDay)
            throw new ArgumentOutOfRangeException(nameof(minuteOfDay));

        return new InstantOfDay(NanosecondsPerMinute * minuteOfDay);
    }

    /// <summary>
    /// Creates a new instance of <see cref="InstantOfDay"/> from the specified
    /// elapsed seconds since midnight.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="secondOfDay"/>
    /// is out of range.</exception>
    [Pure]
    public static InstantOfDay FromSecondOfDay(int secondOfDay)
    {
        if ((uint)secondOfDay >= SecondsPerDay)
            throw new ArgumentOutOfRangeException(nameof(secondOfDay));

        return new InstantOfDay((long)NanosecondsPerSecond * secondOfDay);
    }

    /// <summary>
    /// Creates a new instance of <see cref="InstantOfDay"/> from the specified
    /// elapsed milliseconds since midnight.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondOfDay"/>
    /// is out of range. </exception>
    [Pure]
    public static InstantOfDay FromMillisecondOfDay(int millisecondOfDay)
    {
        if ((uint)millisecondOfDay >= MillisecondsPerDay)
            throw new ArgumentOutOfRangeException(nameof(millisecondOfDay));

        return new InstantOfDay((long)NanosecondsPerMillisecond * millisecondOfDay);
    }

    /// <summary>
    /// Creates a new instance of <see cref="InstantOfDay"/> from the specified
    /// elapsed milliseconds since midnight.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="microsecondOfDay"/>
    /// is out of range.</exception>
    [Pure]
    public static InstantOfDay FromMicrosecondOfDay(long microsecondOfDay)
    {
        if ((ulong)microsecondOfDay >= MicrosecondsPerDay)
            throw new ArgumentOutOfRangeException(nameof(microsecondOfDay));

        return new InstantOfDay(NanosecondsPerMicrosecond * microsecondOfDay);
    }

    /// <summary>
    /// Creates a new instance of <see cref="InstantOfDay"/> from the specified
    /// elapsed ticks since midnight.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="tickOfDay"/>
    /// is out of range.</exception>
    [Pure]
    public static InstantOfDay FromTickOfDay(long tickOfDay)
    {
        if ((ulong)tickOfDay >= TicksPerDay)
            throw new ArgumentOutOfRangeException(nameof(tickOfDay));

        return new InstantOfDay(NanosecondsPerTick * tickOfDay);
    }

    /// <summary>
    /// Creates a new instance of <see cref="InstantOfDay"/> from the specified
    /// elapsed nanoseconds since midnight.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="nanosecondOfDay"/>
    /// is out of range.</exception>
    [Pure]
    public static InstantOfDay FromNanosecondOfDay(long nanosecondOfDay)
    {
        if ((ulong)nanosecondOfDay >= NanosecondsPerDay)
            throw new ArgumentOutOfRangeException(nameof(nanosecondOfDay));

        return new InstantOfDay(nanosecondOfDay);
    }

    /// <summary>
    /// Creates a new instance of <see cref="InstantOfDay"/> from the specified
    /// fraction of the day.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="fractionOfDay"/>
    /// is out of range.</exception>
    [Pure]
    public static InstantOfDay FromFractionOfDay(double fractionOfDay)
    {
        if (fractionOfDay < 0d || fractionOfDay >= 1d)
            throw new ArgumentOutOfRangeException(nameof(fractionOfDay));

        long nanosecondOfDay = (long)(fractionOfDay * NanosecondsPerDay);

        return new InstantOfDay(nanosecondOfDay);
    }

    /// <summary>
    /// Creates a new instance of <see cref="InstantOfDay"/> from the specified
    /// fraction of the day.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="fractionOfDay"/>
    /// is out of range.</exception>
    [Pure]
    internal static InstantOfDay FromFractionOfDay(decimal fractionOfDay)
    {
        if (fractionOfDay < 0m || fractionOfDay >= 1m)
            throw new ArgumentOutOfRangeException(nameof(fractionOfDay));

        long nanosecondOfDay = (long)(fractionOfDay * NanosecondsPerDay);

        return new InstantOfDay(nanosecondOfDay);
    }

    #endregion
    #region Conversions

#if false // REVIEW(api): conversion to InstantOfDay.
    /// <summary>
    /// Converts the current instance to a <see cref="TimeOfDay"/>.
    /// </summary>
    [Pure]
    public TimeOfDay ToTimeOfDay() => TimeOfDay.FromMillisecondOfDayCore(MillisecondOfDay);
#endif

    /// <summary>
    /// Converts this instance to a fraction of the day.
    /// </summary>
    [Pure]
    public double ToFractionOfDay() => (double)_nanosecondOfDay / NanosecondsPerDay;

    /// <summary>
    /// Converts this instance to a fraction of the day.
    /// </summary>
    [Pure]
    internal decimal ToDecimal() => (decimal)_nanosecondOfDay / NanosecondsPerDay;

    #endregion
}

public partial struct InstantOfDay // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(InstantOfDay left, InstantOfDay right) =>
        left._nanosecondOfDay == right._nanosecondOfDay;

    /// <inheritdoc />
    public static bool operator !=(InstantOfDay left, InstantOfDay right) =>
        left._nanosecondOfDay != right._nanosecondOfDay;

    /// <inheritdoc />
    [Pure]
    public bool Equals(InstantOfDay other) => _nanosecondOfDay == other._nanosecondOfDay;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is InstantOfDay timeOfDay && Equals(timeOfDay);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _nanosecondOfDay.GetHashCode();
}

public partial struct InstantOfDay // IComparable
{
    /// <summary>
    /// Compares the two specified times of the day to see if the left one is
    /// strictly earlier than the right one.
    /// </summary>
    public static bool operator <(InstantOfDay left, InstantOfDay right) =>
        left._nanosecondOfDay < right._nanosecondOfDay;

    /// <summary>
    /// Compares the two specified times of the day to see if the left one is
    /// earlier than or equal to the right one.
    /// </summary>
    public static bool operator <=(InstantOfDay left, InstantOfDay right) =>
        left._nanosecondOfDay <= right._nanosecondOfDay;

    /// <summary>
    /// Compares the two specified times of the day to see if the left one is
    /// strictly later than the right one.
    /// </summary>
    public static bool operator >(InstantOfDay left, InstantOfDay right) =>
        left._nanosecondOfDay > right._nanosecondOfDay;

    /// <summary>
    /// Compares the two specified times of the day to see if the left one is
    /// later than or equal to the right one.
    /// </summary>
    public static bool operator >=(InstantOfDay left, InstantOfDay right) =>
        left._nanosecondOfDay >= right._nanosecondOfDay;

    /// <summary>
    /// Obtains the earlier time of two specified times.
    /// </summary>
    [Pure]
    public static InstantOfDay Min(InstantOfDay x, InstantOfDay y) => x < y ? x : y;

    /// <summary>
    /// Obtains the later time of two specified times.
    /// </summary>
    [Pure]
    public static InstantOfDay Max(InstantOfDay x, InstantOfDay y) => x > y ? x : y;

    /// <summary>
    /// Indicates whether this instance is earlier, later or the same as the
    /// specified one.
    /// </summary>
    [Pure]
    public int CompareTo(InstantOfDay other) => _nanosecondOfDay.CompareTo(other._nanosecondOfDay);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is InstantOfDay timeOfDay ? CompareTo(timeOfDay)
        : ThrowHelpers.ThrowNonComparable(typeof(InstantOfDay), obj);
}
