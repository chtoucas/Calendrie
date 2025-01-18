// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;

using Calendrie.Core;

using CalendrieRange = Calendrie.Core.Intervals.Range;

public static class CivilTripleScope
{
    public const int MinYear = 1;
    public const int MaxYear = 9999;

    internal static readonly CivilPrototype Schema = new();

    private static readonly CalendricalSegment s_Segment =
        CalendricalSegment.Create(Schema, CalendrieRange.Create(MinYear, MaxYear));

    // MinYear = 1 => MinDaysSinceEpoch = 0.
    internal const int MinDaysSinceEpoch = 0;
    internal static int MaxDaysSinceEpoch => s_Segment.SupportedDays.Max;

    internal static bool CheckDaysSinceEpoch(int daysSinceEpoch) =>
        daysSinceEpoch >= MinDaysSinceEpoch && daysSinceEpoch <= MaxDaysSinceEpoch;

    internal static bool CheckYear(int year) => year >= MinYear && year <= MaxYear;

    internal static void ValidateYearMonthDay(int year, int month, int day)
    {
        if (year < MinYear || year > MaxYear) throw new ArgumentOutOfRangeException(nameof(year));
        Schema.PreValidator.ValidateMonthDay(year, month, day);
    }

    internal static void ValidateOrdinal(int year, int dayOfYear)
    {
        if (year < MinYear || year > MaxYear) throw new ArgumentOutOfRangeException(nameof(year));
        Schema.PreValidator.ValidateDayOfYear(year, dayOfYear);
    }
}
