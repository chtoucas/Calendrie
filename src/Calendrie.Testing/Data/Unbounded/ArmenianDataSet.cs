﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data.Unbounded;

using Calendrie.Testing.Data;
using Calendrie.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) Amenian calendar.
/// </summary>
public sealed class UnboundedArmenian12DataSet :
    UnboundedCalendarDataSet<Egyptian12DataSet>, IEpagomenalDataSet, ISingleton<UnboundedArmenian12DataSet>
{
    private static readonly DayNumber s_Epoch = DayZero.Armenian;

    private UnboundedArmenian12DataSet() : base(Egyptian12DataSet.Instance, s_Epoch) { }

    public static UnboundedArmenian12DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedArmenian12DataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Egyptian12DataSet.DaysSinceRataDieInfos, DayZero.Egyptian, s_Epoch);

    // IEpagomenalDataSet
    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => SchemaDataSet.EpagomenalDayInfoData;
}

/// <summary>
/// Provides test data for the (unbounded) Amenian calendar (alternative form).
/// </summary>
public sealed class UnboundedArmenian13DataSet :
    UnboundedCalendarDataSet<Egyptian13DataSet>, IEpagomenalDataSet, ISingleton<UnboundedArmenian13DataSet>
{
    private static readonly DayNumber s_Epoch = DayZero.Armenian;

    private UnboundedArmenian13DataSet() : base(Egyptian13DataSet.Instance, s_Epoch) { }

    public static UnboundedArmenian13DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedArmenian13DataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Egyptian13DataSet.DaysSinceRataDieInfos, DayZero.Egyptian, s_Epoch);

    // IEpagomenalDataSet
    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => SchemaDataSet.EpagomenalDayInfoData;
}
