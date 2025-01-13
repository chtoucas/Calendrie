// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Hemerology;
using Calendrie.Testing.Data;

/// <summary>
/// Provides facts about the <see cref="Calendar"/> type.
/// </summary>
public abstract partial class CalendarFacts<TCalendar, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TCalendar : Calendar
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarFacts(TCalendar calendar)
    {
        ArgumentNullException.ThrowIfNull(calendar);

        CalendarUT = calendar;
    }

    /// <summary>
    /// Gets the calendar under test.
    /// </summary>
    protected TCalendar CalendarUT { get; }

    [Fact] public abstract void Algorithm_Prop();
    [Fact] public abstract void Family_Prop();
    [Fact] public abstract void PeriodicAdjustments_Prop();

    [Fact]
    public void Epoch_Prop() => Assert.Equal(Epoch, CalendarUT.Epoch);

    // Virtual for calendars overriding ToString().
    [Fact]
    public virtual void ToString_InvariantCulture() =>
        Assert.Equal(CalendarUT.ToString(), CalendarUT.Name);

}
