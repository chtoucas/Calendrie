// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Gregorian;

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

using Calendrie;
using Calendrie.Core;
using Calendrie.Specialized;

using NodaTime;

public sealed record GJSample
{
    public GJSample(int year, int month, int day)
    {
        Year = year;
        Month = month;
        Day = day;

        DayNumber = DayNumber.FromGregorianParts(Year, Month, Day);
        CivilDate = new(Year, Month, Day);
        GregorianDate = new(Year, Month, Day);
        LocalDate = new(Year, Month, Day);
        DateOnly = new(Year, Month, Day);
        DateTime = new(Year, Month, Day);
    }

    public int Year { get; init; }
    public int Month { get; init; }
    public int Day { get; init; }

    public DayNumber DayNumber { get; }
    public CivilDate CivilDate { get; }
    public GregorianDate GregorianDate { get; }
    public LocalDate LocalDate { get; }
    public DateOnly DateOnly { get; }
    public DateTime DateTime { get; }
}

public enum GJSampleKind
{
    Now = 0,
    Fixed,
    Slow,
    Random,
}

public abstract class GJComparisons
{
    protected GJComparisons() { }

    protected GJSampleKind SampleKind { get; init; }

    protected DayNumber SampleDayNumber => DayNumber.FromGregorianParts(Year, Month, Day);
    protected CivilDate SampleCivilDate => new(Year, Month, Day);
    protected GregorianDate SampleGregorianDate => new(Year, Month, Day);
    protected LocalDate SampleLocalDate => new(Year, Month, Day);
    protected DateOnly SampleDateOnly => new(Year, Month, Day);
    protected DateTime SampleDateTime => new(Year, Month, Day);

    /// <summary>Gets the Gregorian/Julian year.</summary>
    protected int Year => SampleKind switch
    {
        GJSampleKind.Now => Now.Year,
        GJSampleKind.Fixed => Fixed.Year,
        GJSampleKind.Slow => Slow.Year,
        GJSampleKind.Random => Random.Year,
        _ => Now.Year,
    };

    /// <summary>Gets the Gregorian/Julian month.</summary>
    protected int Month => SampleKind switch
    {
        GJSampleKind.Now => Now.Month,
        GJSampleKind.Fixed => Fixed.Month,
        GJSampleKind.Slow => Slow.Month,
        GJSampleKind.Random => Random.Month,
        _ => Now.Month,
    };

    /// <summary>Gets the Gregorian/Julian day.</summary>
    protected int Day => SampleKind switch
    {
        GJSampleKind.Now => Now.Day,
        GJSampleKind.Fixed => Fixed.Day,
        GJSampleKind.Slow => Slow.Day,
        GJSampleKind.Random => Random.Day,
        _ => Now.Day,
    };

    /// <summary>Gets the Gregorian day of the year.</summary>
    protected int DayOfYear => SampleKind switch
    {
        GJSampleKind.Now => Now.DayOfYear,
        GJSampleKind.Fixed => Fixed.DayOfYear,
        GJSampleKind.Slow => Slow.DayOfYear,
        GJSampleKind.Random => Random.DayOfYear,
        _ => Now.DayOfYear,
    };

    /// <summary>Gets the Gregorian value for daysSinceEpoch.</summary>
    protected int DaysSinceEpoch => SampleKind switch
    {
        GJSampleKind.Now => Now.DaysSinceEpoch,
        GJSampleKind.Fixed => Fixed.DaysSinceEpoch,
        GJSampleKind.Slow => Slow.DaysSinceEpoch,
        GJSampleKind.Random => Random.DaysSinceEpoch,
        _ => Now.DaysSinceEpoch,
    };

    /// <summary>Gets the Gregorian day number.</summary>
    protected DayNumber DayNumber => DayZero.NewStyle + DaysSinceEpoch;

    protected Yemoda Ymd => new(Year, Month, Day);
    protected Yemo Ym => new(Year, Month);

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
