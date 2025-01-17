// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Systems;

using Calendrie.Hemerology;
using Calendrie.Systems;
using Calendrie.Testing.Data;

// Useful to test the static (abstract) methods from the interface.

/// <summary>
/// Provides facts about the <see cref="IUnsafeFactory{T}"/> type.
/// </summary>
public class IUnsafeMonthFactoryFacts<TMonth, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TMonth : struct, IMonth<TMonth>, IUnsafeFactory<TMonth>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    [Theory, MemberData(nameof(MonthsSinceEpochInfoData))]
    public void UnsafeCreate(MonthsSinceEpochInfo info)
    {
        var (monthsSinceEpoch, y, m) = info;
        // Act
        var date = TMonth.UnsafeCreate(monthsSinceEpoch);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
    }
}
