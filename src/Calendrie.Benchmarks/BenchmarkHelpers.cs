// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks;

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

using NodaTime;

internal enum GJSampleKind
{
    Fixed = 0,
    Slow,
    Now,
    Random,
}

/// <summary>
/// Provides static helpers to prevent from dead code elimination.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static partial class BenchmarkHelpers
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in bool _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in uint _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in int _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in long _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in DayOfWeek _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in IsoWeekday _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in IsoDayOfWeek _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in Yemoda _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in Yemo _) { }
}

internal partial class BenchmarkHelpers
{
    public static DateParts CreateGregorianParts(GJSampleKind kind)
    {
        return kind switch
        {
            GJSampleKind.Now => new(Now.Year, Now.Month, Now.Day),
            GJSampleKind.Slow => new(Slow.Year, Slow.Month, Slow.Day),
            GJSampleKind.Random => new(Random.Year, Random.Month, Random.Day),
            GJSampleKind.Fixed => new(Now.Year, Now.Month, Now.Day),
            _ => throw new ArgumentException(
                $"The value is not valid; value = {kind}.",
                nameof(kind))
        };
    }

    public static DateParts CreateJulianParts(GJSampleKind kind)
    {
        return kind switch
        {
            GJSampleKind.Slow => new(Slow.Year, Slow.Month, Slow.Day),
            GJSampleKind.Random => new(Random.Year, Random.Month, Random.Day),
            GJSampleKind.Fixed => new(Now.Year, Now.Month, Now.Day),
            GJSampleKind.Now or _ => throw new ArgumentException(
                $"The value is not valid or not supported; value = {kind}.",
                nameof(kind))
        };
    }

    private static class Now
    {
        internal static readonly int Year;
        internal static readonly int Month;
        internal static readonly int Day;

        [SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline")]
        static Now()
        {
            var now = DateTime.Now;
            Year = now.Year;
            Month = now.Month;
            Day = now.Day;
        }
    }

    /// <summary>Provides fixed Gregorian/Julian data (fast track).</summary>
    private static class Fixed
    {
        // NB : NodaTime utilise un cache pour les dates de 1900 à 2100 dans
        // le calendrier grégorien.

        internal const int Year = 2017;
        internal const int Month = 11;
        internal const int Day = 19;
    }

    /// <summary>Provides fixed Gregorian/Julian data (slow track).</summary>
    private static class Slow
    {
        internal const int Year = 2020;
        internal const int Month = 12;
        internal const int Day = 31;
    }

    private static class Random
    {
        internal static readonly int Year;
        internal static readonly int Month;
        internal static readonly int Day;

        [SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline")]
        static Random()
        {
            Year = RandomNumberGenerator.GetInt32(2000, 2100);
            Month = RandomNumberGenerator.GetInt32(1, 12);
            Day = RandomNumberGenerator.GetInt32(1, 28);
        }
    }
}
