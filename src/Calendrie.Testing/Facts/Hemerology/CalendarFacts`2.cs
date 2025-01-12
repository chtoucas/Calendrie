// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Testing.Data;

/// <summary>
/// Provides facts about <see cref="Calendar"/>.
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

        var scope = calendar.Scope;
        var supportedYears = scope.Segment.SupportedYears;
        SupportedYearsTester = new SupportedYearsTester(supportedYears);

        var domain = scope.Domain;
        Domain = domain;
        DomainTester = new DomainTester(domain);
    }

    /// <summary>
    /// Gets the calendar under test.
    /// </summary>
    protected TCalendar CalendarUT { get; }

    protected Range<DayNumber> Domain { get; }

    protected SupportedYearsTester SupportedYearsTester { get; }
    protected DomainTester DomainTester { get; }

    [Fact] public abstract void Algorithm_Prop();
    [Fact] public abstract void Family_Prop();
    [Fact] public abstract void PeriodicAdjustments_Prop();

    [Fact]
    public void Epoch_Prop() => Assert.Equal(Epoch, CalendarUT.Epoch);
}
