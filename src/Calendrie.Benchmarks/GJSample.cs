// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks;

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

using Calendrie;
using Calendrie.Core;
using Calendrie.Specialized;

using NodaTime;

public enum GJSampleKind
{
    Fixed = 0,
    Slow,
    Now,
    Random,
}

public sealed class GJSample
{
    public GJSampleKind SampleKind { get; init; }

    /// <summary>Gets the Gregorian date.</summary>
    public CivilDate CivilDate => new(Year, Month, Day);
    /// <summary>Gets the Gregorian date.</summary>
    public GregorianDate GregorianDate => new(Year, Month, Day);
    /// <summary>Gets the Gregorian date.</summary>
    public LocalDate LocalDate => new(Year, Month, Day);
    /// <summary>Gets the Gregorian date.</summary>
    public DateOnly DateOnly => new(Year, Month, Day);
    /// <summary>Gets the Gregorian date-time.</summary>
    public DateTime DateTime => new(Year, Month, Day);

    /// <summary>Gets the Gregorian/Julian year.</summary>
    public int Year => SampleKind switch
    {
        GJSampleKind.Now => Now.Year,
        GJSampleKind.Fixed => Fixed.Year,
        GJSampleKind.Slow => Slow.Year,
        GJSampleKind.Random => Random.Year,
        _ => Now.Year,
    };

    /// <summary>Gets the Gregorian/Julian month.</summary>
    public int Month => SampleKind switch
    {
        GJSampleKind.Now => Now.Month,
        GJSampleKind.Fixed => Fixed.Month,
        GJSampleKind.Slow => Slow.Month,
        GJSampleKind.Random => Random.Month,
        _ => Now.Month,
    };

    /// <summary>Gets the Gregorian/Julian day.</summary>
    public int Day => SampleKind switch
    {
        GJSampleKind.Now => Now.Day,
        GJSampleKind.Fixed => Fixed.Day,
        GJSampleKind.Slow => Slow.Day,
        GJSampleKind.Random => Random.Day,
        _ => Now.Day,
    };

    /// <summary>Gets the Gregorian day of the year.</summary>
    public int DayOfYear => SampleKind switch
    {
        GJSampleKind.Now => Now.DayOfYear,
        GJSampleKind.Fixed => Fixed.DayOfYear,
        GJSampleKind.Slow => Slow.DayOfYear,
        GJSampleKind.Random => Random.DayOfYear,
        _ => Now.DayOfYear,
    };

    /// <summary>Gets the Gregorian value for daysSinceEpoch.</summary>
    public int DaysSinceEpoch => SampleKind switch
    {
        GJSampleKind.Now => Now.DaysSinceEpoch,
        GJSampleKind.Fixed => Fixed.DaysSinceEpoch,
        GJSampleKind.Slow => Slow.DaysSinceEpoch,
        GJSampleKind.Random => Random.DaysSinceEpoch,
        _ => Now.DaysSinceEpoch,
    };

    /// <summary>Gets the Gregorian day number.</summary>
    public DayNumber DayNumber => DayZero.NewStyle + DaysSinceEpoch;

    public Yemoda Ymd => new(Year, Month, Day);
    public Yemo Ym => new(Year, Month);

    public void Deconstruct(out int year, out int month, out int day) =>
        (year, month, day) = (Year, Month, Day);

    private static class Now
    {
        internal static readonly int Year;
        internal static readonly int Month;
        internal static readonly int Day;
        internal static readonly int DayOfYear;
        internal static readonly int DaysSinceEpoch;

        [SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline", Justification = "<Pending>")]
        static Now()
        {
            var now = DateTime.Now;
            Year = now.Year;
            Month = now.Month;
            Day = now.Day;
            DayOfYear = now.DayOfYear;
            DaysSinceEpoch = (int)TemporalArithmetic.DivideByTicksPerDay(now.Ticks);
        }
    }

    // WARNING: en utilisant des constantes, on triche un peu car
    // .NET va certainement effectuer des optimisations qu'il ne
    // ferait pas en temps normal. Ceci dit, le comportement est le
    // même pour tout le monde.

    /// <summary>Provides fixed Gregorian data (fast track).</summary>
    private static class Fixed
    {
        // NB : NodaTime utilise un cache pour les dates de 1900 à 2100 dans
        // le calendrier grégorien.

        internal const int Year = 2017;
        internal const int Month = 11;
        internal const int Day = 19;
        internal const int DayOfYear = 323;
        internal const int DaysSinceEpoch = 736_651;
    }

    /// <summary>Provides fixed Gregorian data (slow track).</summary>
    private static class Slow
    {
        internal const int Year = 2020;
        internal const int Month = 12;
        internal const int Day = 31;
        internal const int DayOfYear = 366;
        internal const int DaysSinceEpoch = 737_789;
    }

    private static class Random
    {
        internal static readonly int Year;
        internal static readonly int Month;
        internal static readonly int Day;
        internal static readonly int DayOfYear;
        internal static readonly int DaysSinceEpoch;

        [SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline", Justification = "<Pending>")]
        static Random()
        {
            Year = RandomNumberGenerator.GetInt32(2000, 2100);
            Month = RandomNumberGenerator.GetInt32(1, 12);
            Day = RandomNumberGenerator.GetInt32(1, 28);

            // WARNING: les champs suivants ne correspondent aux autres.
            DayOfYear = RandomNumberGenerator.GetInt32(1, 365);
            DaysSinceEpoch = RandomNumberGenerator.GetInt32(600_000, 800_000);
        }
    }
}
