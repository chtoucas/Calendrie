// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Systems;

using Calendrie.Hemerology;
using Calendrie.Systems;
using Calendrie.Testing.Data;

// Useful to test the interface when the implementation is __explicit__.

/// <summary>
/// Provides facts about the <see cref="IUnsafeFactory{T}"/> type.
/// </summary>
public class IUnsafeDateFactoryFacts<TDate, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDate : struct, IDate<TDate>, IUnsafeFactory<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void UnsafeCreate(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;
        // Act
        var date = TDate.UnsafeCreate(daysSinceEpoch);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }
}
