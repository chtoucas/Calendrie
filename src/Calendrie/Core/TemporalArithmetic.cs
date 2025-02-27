﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using static TemporalConstants;

// REVIEW(code): do we need to use checked ops? Use ulong args

// I only observe very tiny performance gains with:
// - DivideByNanosecondsPerHour
// - MultiplyByNanosecondsPerHour
// - DivideByNanosecondsPerMinute
// For the other methods, whether we use the plain ops or the "optimized"
// ones, it does no seem to make any difference.
//
// NanosecondsPerHour:          3_600_000_000_000 = 2^13 x 5^11 x 3^2
// TicksPerDay:                   864_000_000_000 = 2^14 x 5^9  x 3^3
// NanosecondsPerMinute:           60_000_000_000 = 2^11 x 5^10 x 3
// NanosecondsPerSecond:            1_000_000_000 = 2^9  x 5^9
// NanosecondsPerMillisecond:           1_000_000 = 2^6  x 5^6
//
// For the multiplication, we have the choice of shift then multiply or
// multiply then shift, but perf results are similar. Nevertheless, I opt
// for multiply then shift to avoid a cast.
//
// We don't bother with NanosecondsPerDay which is only multiplied or
// divided with a double or decimal.

/// <summary>
/// Provides fast arithmetical operations related to <see cref="TemporalConstants"/>.
/// </summary>
internal static partial class TemporalArithmetic { }

internal partial class TemporalArithmetic // TicksPerDay
{
    /// <summary>
    /// Represents the 2-adic valuation of <see cref="TicksPerDay"/>.
    /// <para>This field is a constant equal to 14.</para>
    /// </summary>
    private const int TicksPerDayTwoAdicOrder = 14;

    /// <summary>
    /// Represents the product of odd prime factors of <see cref="TicksPerDay"/>.
    /// <para>This field is a constant equal to 52_734_375.</para>
    /// </summary>
    private const long TicksPerDayOddPart = TicksPerDay >> TicksPerDayTwoAdicOrder;

    /// <summary>
    /// <para><c>daysSinceZero = ticksSinceZero / TicksPerDay</c></para>
    /// <para><paramref name="ticksSinceZero"/> MUST be &gt;= 0.</para>
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong DivideByTicksPerDay(ulong ticksSinceZero)
    {
        //Debug.Assert(ticksSinceZero >= 0);

        return (ticksSinceZero >> TicksPerDayTwoAdicOrder) / TicksPerDayOddPart;
    }

    /// <summary>
    /// <para><c>tickOfDay     = ticksSinceZero % TicksPerDay</c></para>
    /// <para><c>daysSinceZero = ticksSinceZero / TicksPerDay</c></para>
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong DivideByTicksPerDay(ulong ticksSinceZero, out ulong tickOfDay)
    {
        // daysSinceZero = ticksSinceZero / TicksPerDay
        ulong daysSinceZero = (ticksSinceZero >> TicksPerDayTwoAdicOrder) / TicksPerDayOddPart;
        // tickOfDay = ticksSinceZero - TicksPerDay * daysSinceZero
        tickOfDay = ticksSinceZero - ((daysSinceZero * TicksPerDayOddPart) << TicksPerDayTwoAdicOrder);
        return daysSinceZero;
    }

    /// <summary>
    /// <para><c>ticksSinceZero = TicksPerDay * daysSinceZero</c></para>
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long MultiplyByTicksPerDay(long daysSinceZero) =>
        //(daysSinceZero << TicksPerDayTwoAdicOrder) * TicksPerDayOddPart;
        (daysSinceZero * TicksPerDayOddPart) << TicksPerDayTwoAdicOrder;
}

internal partial class TemporalArithmetic // NanosecondsPerHour
{
    /// <summary>
    /// Represents the 2-adic valuation of <see cref="NanosecondsPerHour"/>.
    /// <para>This field is a constant equal to 13.</para>
    /// </summary>
    private const int NanosecondsPerHourTwoAdicOrder = 13;

    /// <summary>
    /// Represents the product of odd prime factors of <see cref="NanosecondsPerHour"/>.
    /// <para>This field is a constant equal to 439_453_125.</para>
    /// </summary>
    private const long NanosecondsPerHourOddPart =
        NanosecondsPerHour >> NanosecondsPerHourTwoAdicOrder;

    /// <summary>
    /// <para><c>hourOfDay = nanosecondOfDay / NanosecondsPerHour</c></para>
    /// <para><paramref name="nanosecondOfDay"/> MUST be &gt;= 0.</para>
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int DivideByNanosecondsPerHour(long nanosecondOfDay)
    {
        Debug.Assert(nanosecondOfDay >= 0);
        // If the following condition is not met, the cast to int may fail.
        Debug.Assert(nanosecondOfDay < NanosecondsPerDay);

        return (int)((ulong)(nanosecondOfDay >> NanosecondsPerHourTwoAdicOrder) / NanosecondsPerHourOddPart);
    }

    /// <summary>
    /// <para><c>nanosecondOfDay = NanosecondsPerHour * hourOfDay</c></para>
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long MultiplyByNanosecondsPerHour(int hourOfDay)
    {
        Debug.Assert(hourOfDay >= 0);
        Debug.Assert(hourOfDay < HoursPerDay);

        //return ((long)hourOfDay << NanosecondsPerHourTwoAdicOrder) * NanosecondsPerHourOddPart;
        return (hourOfDay * NanosecondsPerHourOddPart) << NanosecondsPerHourTwoAdicOrder;
    }
}

internal partial class TemporalArithmetic // NanosecondsPerMinute
{
    /// <summary>
    /// Represents the 2-adic valuation of <see cref="NanosecondsPerMinute"/>.
    /// <para>This field is a constant equal to 11.</para>
    /// </summary>
    private const int NanosecondsPerMinuteTwoAdicOrder = 11;

    /// <summary>
    /// Represents the product of odd prime factors of <see cref="NanosecondsPerMinute"/>.
    /// <para>This field is a constant equal to 29_296_875.</para>
    /// </summary>
    private const long NanosecondsPerMinuteOddPart =
        NanosecondsPerMinute >> NanosecondsPerMinuteTwoAdicOrder;

    /// <summary>
    /// <para><c>minuteOfDay = nanosecondOfDay / NanosecondsPerMinute</c></para>
    /// <para><paramref name="nanosecondOfDay"/> MUST be &gt;= 0.</para>
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int DivideByNanosecondsPerMinute(long nanosecondOfDay)
    {
        Debug.Assert(nanosecondOfDay >= 0);
        // If the following condition is not met, the cast to int may fail.
        Debug.Assert(nanosecondOfDay < NanosecondsPerDay);

        return (int)((ulong)(nanosecondOfDay >> NanosecondsPerMinuteTwoAdicOrder) / NanosecondsPerMinuteOddPart);
    }

    /// <summary>
    /// <para><c>nanosecondOfDay = NanosecondsPerMinute * minuteOfDay</c></para>
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long MultiplyByNanosecondsPerMinute(int minuteOfDay)
    {
        Debug.Assert(minuteOfDay >= 0);
        Debug.Assert(minuteOfDay < MinutesPerDay);

        //return ((long)minuteOfDay << NanosecondsPerMinuteTwoAdicOrder) * NanosecondsPerMinuteOddPart;
        return (minuteOfDay * NanosecondsPerMinuteOddPart) << NanosecondsPerMinuteTwoAdicOrder;
    }
}

internal partial class TemporalArithmetic // NanosecondsPerSecond (disabled)
{
#if false // NanosecondsPerSecond
    /// <summary>
    /// Represents the 2-adic valuation of <see cref="NanosecondsPerSecond"/>.
    /// <para>This field is a constant equal to 9.</para>
    /// </summary>
    private const int NanosecondsPerSecondTwoAdicOrder = 9;

    /// <summary>
    /// Represents the product of odd prime factors of <see cref="NanosecondsPerSecond"/>.
    /// <para>This field is a constant equal to 1_953_125.</para>
    /// </summary>
    private const long NanosecondsPerSecondOddPart =
        NanosecondsPerSecond >> NanosecondsPerSecondTwoAdicOrder;

    /// <summary>
    /// <para><c>secondOfDay = nanosecondOfDay / NanosecondsPerSecond</c></para>
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long DivideByNanosecondsPerSecond(long nanosecondOfDay)
    {
        Debug.Assert(nanosecondOfDay >= 0);
        Debug.Assert(nanosecondOfDay < NanosecondsPerDay);

        return (nanosecondOfDay >> NanosecondsPerSecondTwoAdicOrder) / NanosecondsPerSecondOddPart;
    }

    /// <summary>
    /// <para><c>nanosecondOfDay = NanosecondsPerSecond * secondOfDay</c></para>
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long MultiplyByNanosecondsPerSecond(int secondOfDay)
    {
        Debug.Assert(secondOfDay < SecondsPerDay);

        return (secondOfDay << NanosecondsPerSecondTwoAdicOrder) * NanosecondsPerSecondOddPart;
    }
#endif
}

internal partial class TemporalArithmetic // NanosecondsPerMillisecond (disabled)
{
#if false // NanosecondsPerMillisecond
    /// <summary>
    /// Represents the 2-adic valuation of <see cref="NanosecondsPerMillisecond"/>.
    /// <para>This field is a constant equal to 6.</para>
    /// </summary>
    private const int NanosecondsPerMillisecondTwoAdicOrder = 6;

    /// <summary>
    /// Represents the product of odd prime factors of <see cref="NanosecondsPerMillisecond"/>.
    /// <para>This field is a constant equal to 15_625.</para>
    /// </summary>
    private const long NanosecondsPerMillisecondOddPart =
        NanosecondsPerMillisecond >> NanosecondsPerMillisecondTwoAdicOrder;

    /// <summary>
    /// <para><c>millisecondOfDay = nanosecondOfDay / NanosecondsPerMillisecond</c></para>
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long DivideByNanosecondsPerMillisecond(long nanosecondOfDay)
    {
        Debug.Assert(nanosecondOfDay >= 0);
        Debug.Assert(nanosecondOfDay < NanosecondsPerDay);

        return (nanosecondOfDay >> NanosecondsPerMillisecondTwoAdicOrder) / NanosecondsPerMillisecondOddPart;
    }

    /// <summary>
    /// <para><c>nanosecondOfDay = NanosecondsPerMillisecond * millisecondOfDay</c></para>
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long MultiplyByNanosecondsPerMillisecond(int millisecondOfDay)
    {
        Debug.Assert(millisecondOfDay < MillisecondsPerDay);

        return (millisecondOfDay << NanosecondsPerMillisecondTwoAdicOrder) * NanosecondsPerMillisecondOddPart;
    }
#endif
}
