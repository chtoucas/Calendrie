﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data.Unbounded;

using Calendrie.Testing.Data;
using Calendrie.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) Zoroastrian calendar (alternative form).
/// </summary>
public sealed class UnboundedZoroastrian13DataSet :
    UnboundedCalendarDataSet<Egyptian13DataSet>, IEpagomenalDataSet, ISingleton<UnboundedZoroastrian13DataSet>
{
    private static readonly DayNumber s_Epoch = DayZero.Zoroastrian;

    private UnboundedZoroastrian13DataSet() : base(Egyptian13DataSet.Instance, s_Epoch) { }

    public static UnboundedZoroastrian13DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedZoroastrian13DataSet Instance = new();
        static Singleton() { }
    }

    public sealed override DataGroup<DayNumberInfo> DayNumberInfoData =>
        DataGroup.CreateDayNumberInfoData(Egyptian13DataSet.DaysSinceRataDieInfos, DayZero.Egyptian, s_Epoch);

    // IEpagomenalDataSet
    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => SchemaDataSet.EpagomenalDayInfoData;
}

/// <summary>
/// Provides test data for the (unbounded) Zoroastrian calendar.
/// </summary>
public sealed class UnboundedZoroastrian12DataSet :
    UnboundedCalendarDataSet<Egyptian12DataSet>, IEpagomenalDataSet, ISingleton<UnboundedZoroastrian12DataSet>
{
    private static readonly DayNumber s_Epoch = DayZero.Zoroastrian;

    private UnboundedZoroastrian12DataSet() : base(Egyptian12DataSet.Instance, s_Epoch) { }

    public static UnboundedZoroastrian12DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedZoroastrian12DataSet Instance = new();
        static Singleton() { }
    }

    public sealed override DataGroup<DayNumberInfo> DayNumberInfoData =>
        DataGroup.CreateDayNumberInfoData(Egyptian12DataSet.DaysSinceRataDieInfos, DayZero.Egyptian, s_Epoch);

    // IEpagomenalDataSet
    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => SchemaDataSet.EpagomenalDayInfoData;
}
