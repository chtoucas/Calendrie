﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data.Unbounded;

using Calendrie.Testing.Data;
using Calendrie.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) Egyptian calendar.
/// </summary>
public sealed class UnboundedEgyptian12DataSet :
    UnboundedCalendarDataSet<Egyptian12DataSet>, IEpagomenalDataSet, ISingleton<UnboundedEgyptian12DataSet>
{
    private UnboundedEgyptian12DataSet() : base(Egyptian12DataSet.Instance, DayZero.Egyptian) { }

    public static UnboundedEgyptian12DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedEgyptian12DataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Egyptian12DataSet.DaysSinceRataDieInfos);

    // IEpagomenalDataSet
    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => SchemaDataSet.EpagomenalDayInfoData;
}

/// <summary>
/// Provides test data for the (unbounded) Egyptian calendar (alternative form).
/// </summary>
public sealed class UnboundedEgyptian13DataSet :
    UnboundedCalendarDataSet<Egyptian13DataSet>, IEpagomenalDataSet, ISingleton<UnboundedEgyptian13DataSet>
{
    private UnboundedEgyptian13DataSet() : base(Egyptian13DataSet.Instance, DayZero.Egyptian) { }

    public static UnboundedEgyptian13DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedEgyptian13DataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Egyptian13DataSet.DaysSinceRataDieInfos);

    // IEpagomenalDataSet
    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => SchemaDataSet.EpagomenalDayInfoData;
}
