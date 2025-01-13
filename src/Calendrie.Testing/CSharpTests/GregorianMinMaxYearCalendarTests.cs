// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.CSharpTests;

using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;
using Calendrie.Testing.Data;
using Calendrie.Testing.Data.Bounded;
using Calendrie.Testing.Data.Unbounded;
using Calendrie.Testing.Facts.Hemerology;

public class GregorianMinMaxYearCalendarDataSet :
    MinMaxYearCalendarDataSet<UnboundedGregorianDataSet>,
    ISingleton<GregorianMinMaxYearCalendarDataSet>
{
    public GregorianMinMaxYearCalendarDataSet()
        : base(
            UnboundedGregorianDataSet.Instance,
            GregorianMinMaxYearCalendarTests.FirstYear,
            GregorianMinMaxYearCalendarTests.LastYear)
    { }

    public static GregorianMinMaxYearCalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly GregorianMinMaxYearCalendarDataSet Instance = new();
        static Singleton() { }
    }
}

public class GregorianMinMaxYearCalendarTests :
    CalendarSansFacts<MinMaxYearCalendar, GregorianMinMaxYearCalendarDataSet>
{
    // On triche un peu, les années de début et de fin ont été choisies de
    // telle sorte que les tests marchent... (cf. GregorianData).
    public const int FirstYear = 1;
    public const int LastYear = 123_456;

    public GregorianMinMaxYearCalendarTests() : base(MakeCalendar()) { }

    private static MinMaxYearCalendar MakeCalendar() =>
        new(
            "Gregorian",
            MinMaxYearScope.Create(
                new GregorianSchema(),
                DayZero.NewStyle,
                Range.Create(FirstYear, LastYear)));

    [Fact]
    public void MinYear_Prop() => Assert.Equal(FirstYear, CalendarUT.MinYear);

    [Fact]
    public void MaxYear_Prop() => Assert.Equal(LastYear, CalendarUT.MaxYear);

    [Fact]
    public override void IsRegular()
    {
        Assert.True(CalendarUT.IsRegular(out int monthsIsYear));
        Assert.Equal(monthsIsYear, 12);
    }
}

