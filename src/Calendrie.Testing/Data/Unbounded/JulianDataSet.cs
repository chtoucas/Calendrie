﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data.Unbounded;

using Calendrie.Testing.Data;
using Calendrie.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) Julian calendar.
/// </summary>
public sealed class UnboundedJulianDataSet :
    UnboundedCalendarDataSet<JulianDataSet>, ISingleton<UnboundedJulianDataSet>
{
    private UnboundedJulianDataSet() : base(JulianDataSet.Instance, DayZero.OldStyle) { }

    public static UnboundedJulianDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedJulianDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(JulianDataSet.DaysSinceRataDieInfos);
}
