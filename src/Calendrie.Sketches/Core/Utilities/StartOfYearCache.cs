﻿#pragma warning disable IDE0073 // Require file header (Style) ✓
#pragma warning disable IDE0130 // Namespace does not match folder structure (Style) ✓
// Copyright 2013 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

// Adapted from
// https://github.com/nodatime/nodatime/blob/main/src/NodaTime/Calendars/YearStartCacheEntry.cs

namespace NodaTime.Calendars;

/// <summary>
/// Type containing as much logic as possible for how the cache of "start of year" data works.
/// As of Noda Time 1.3, this is not specific to YearMonthDayCalculator - it can be used for
/// other frames of reference, so long as they comply with the restrictions listed below.
/// </summary>
/// <remarks>
/// <para>
/// Each entry in the cache is a 32-bit number. The "value" part of the entry consists of the
/// number of days since the Unix epoch (negative for a value before the epoch). As Noda Time
/// only supports a number of ticks since the Unix epoch of between long.MinValue and long.MaxValue,
/// we only need to support a number of days in the range
/// [long.MinValue / TicksPerDay, long.MaxValue / TicksPerDay] which is [-10675200, 10675200] (rounding
/// away from 0). This value can be stored in 25 bits.
/// </para>
/// <para>
/// The remaining 7 bits of the value are used for validation. For any given year, the bottom
/// 10 bits are used as the index into the cache (which is an array). The next 7 most significant
/// bits are stored in the entry. So long as we have fewer than 17 significant bits in the year value,
/// this will be a unique combination. A single validation value (the most highly positive value) is
/// reserved to indicate an invalid entry. The cache is initialized with all entries invalid.
/// This gives us a range of year numbers greater than [-60000, 60000] without any risk of collisions. By
/// contrast, the ISO calendar years are in the range [-27255, 31195] - so we'd have to be dealing with a
/// calendar with either very short years, or an epoch a long way ahead or behind the Unix epoch.
/// </para>
/// <para>
/// The fact that each cache entry is only 32 bits means that we can safely use the cache from multiple
/// threads without locking. 32-bit aligned values are guaranteed to be accessed atomically, so we know we'll
/// never get the value for one year with the validation bits for another, for example.
/// </para>
/// </remarks>
[ExcludeFromCodeCoverage]
internal readonly struct StartOfYearCache
{
    private const int IndexBits = 10;
    private const int IndexMask = CacheSize - 1;

    private const int EntryValidationBits = 7;
    private const int EntryValidationMask = (1 << EntryValidationBits) - 1;

    private const int CacheSize = 1 << IndexBits;
    // Smallest (positive) year such that the validator is as high as possible.
    // (We shift the mask down by one because the top bit of the validator is effectively the sign bit for the
    // year, and so a validation value with all bits set is already used for e.g. year -1.)
    internal const int InvalidEntryYear = (EntryValidationMask >> 1) << IndexBits;

    /// <summary>
    /// Entry which is guaranteed to be obviously invalid for any real date, by having
    /// a validation value which is larger than any valid year number.
    /// </summary>
    private static readonly StartOfYearCache s_Invalid = new(InvalidEntryYear, 0);

    /// <summary>
    /// Entry value: most significant 25 bits are the number of days (e.g. since the Unix epoch); remaining 7 bits are
    /// the validator.
    /// </summary>
    private readonly int _value;

    internal StartOfYearCache(int y, int startOfYear)
    {
        _value = (startOfYear << EntryValidationBits) | GetValidator(y);
    }

    internal static StartOfYearCache[] Create()
    {
        var cache = new StartOfYearCache[CacheSize];
        for (int i = 0; i < cache.Length; i++)
        {
            cache[i] = s_Invalid;
        }
        return cache;
    }

    /// <summary>
    /// Returns the (signed) number of days since the Unix epoch for the cache entry.
    /// </summary>
    public int StartOfYear => _value >> EntryValidationBits;

    /// <summary>
    /// Returns the cache index, in [0, CacheSize), that should be used to store the given year's cache entry.
    /// </summary>
    public static int GetIndex(int y) =>
        // Effectively keep only the bottom CacheIndexBits bits.
        y & IndexMask;

    /// <summary>
    /// Returns the validator to use for a given year, a non-negative number containing at most
    /// EntryValidationBits bits.
    /// </summary>
    private static int GetValidator(int y) =>
        // Note that we assume that the input year fits into EntryValidationBits+CacheIndexBits bits - if not,
        // this would return the same validator for more than one input year, meaning that we could potentially
        // use the wrong cache value.
        // The masking here is necessary to remove some of the sign-extended high bits for negative years.
        (y >> IndexBits) & EntryValidationMask;

    /// <summary>
    /// Returns whether this cache entry is valid for the given year, and so is safe to use.  (We assume that we
    /// have located this entry via the correct cache index.)
    /// </summary>
    public bool IsValidForYear(int y) => GetValidator(y) == (_value & EntryValidationMask);
}
