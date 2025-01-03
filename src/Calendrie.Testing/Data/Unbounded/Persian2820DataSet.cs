﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data.Unbounded;

using Calendrie.Testing.Data;
using Calendrie.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) Persian calendar (proposed arithmetical form).
/// </summary>
public sealed class UnboundedPersian2820DataSet :
    UnboundedCalendarDataSet<Persian2820DataSet>, ISingleton<UnboundedPersian2820DataSet>
{
    private UnboundedPersian2820DataSet() : base(Persian2820DataSet.Instance, DayZero.Persian) { }

    public static UnboundedPersian2820DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedPersian2820DataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Persian2820DataSet.DaysSinceRataDieInfos);
}

