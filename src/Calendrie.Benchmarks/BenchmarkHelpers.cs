// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

using Calendrie;
using Calendrie.Core;

using NodaTime;

internal enum GJDateType
{
    FixedFast = 0,
    FixedSlow,
    Random,
}

/// <summary>
/// Provides static helpers to prevent from dead code elimination.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static partial class BenchmarkHelpers { }

internal partial class BenchmarkHelpers // Consume
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
    public static void Consume(in IsoDayOfWeek _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in Yemoda _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in Yemo _) { }
}

internal partial class BenchmarkHelpers // Gregorian/Julian
{
    public static DateParts CreateGregorianParts() => CreateGregorianParts(GJDateType.FixedFast);

    public static DateParts CreateGregorianParts(GJDateType type)
    {
        return type switch
        {
            GJDateType.FixedFast => new(FixedFast.Year, FixedFast.Month, FixedFast.Day),
            GJDateType.FixedSlow => new(FixedSlow.Year, FixedSlow.Month, FixedSlow.Day),
            GJDateType.Random => new(Random.Year, Random.Month, Random.Day),
            _ => throw new ArgumentException($"The value is not valid; value = {type}.", nameof(type))
        };
    }

    public static DateParts CreateJulianParts() => CreateJulianParts(GJDateType.FixedFast);

    public static DateParts CreateJulianParts(GJDateType type = GJDateType.FixedFast)
    {
        return type switch
        {
            GJDateType.FixedFast => new(FixedFast.Year, FixedFast.Month, FixedFast.Day),
            GJDateType.FixedSlow => new(FixedSlow.Year, FixedSlow.Month, FixedSlow.Day),
            GJDateType.Random => new(Random.Year, Random.Month, Random.Day),
            _ => throw new ArgumentException($"The value is not valid; value = {type}.", nameof(type))
        };
    }

    /// <summary>Provider of a fixed Gregorian/Julian date (fast track).</summary>
    private static class FixedFast
    {
        // Le jour est valide quelque soit le mois.
        // NodaTime utilise un cache pour les dates allant de 1900 à 2100 dans
        // le calendrier grégorien.

        internal const int Year = 2017;
        internal const int Month = 11;
        internal const int Day = 19;
    }

    /// <summary>Provider of a fixed Gregorian/Julian date (slow track).</summary>
    private static class FixedSlow
    {
        // La validité du jour dépend du mois.
        // Bien entendu, la date proposée est bien valide mais le code testé ne
        // pourra pas utiliser de raccourci.

        internal const int Year = 1801;
        internal const int Month = 12;
        internal const int Day = 31;
    }

    /// <summary>Provider of a random Gregorian/Julian date.</summary>
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
