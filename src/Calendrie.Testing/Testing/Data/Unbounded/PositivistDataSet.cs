// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data.Unbounded;

using Calendrie.Testing.Data;
using Calendrie.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) Positivist calendar.
/// </summary>
public sealed class UnboundedPositivistDataSet :
    UnboundedCalendarDataSet<PositivistDataSet>, ISingleton<UnboundedPositivistDataSet>
{
    private UnboundedPositivistDataSet() : base(PositivistDataSet.Instance, DayZero.Positivist) { }

    public static UnboundedPositivistDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedPositivistDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(PositivistDataSet.DaysSinceZeroInfos);
}
