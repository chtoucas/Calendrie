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

// TODO(fact): à améliorer. À refaire en F#. Idem pour BoundedBelowCalendarTests.

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
    CalendarFacts<MinMaxYearCalendar, GregorianMinMaxYearCalendarDataSet>
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
    public void SupportedYears_Prop()
    {
        // Act
        var supportedYears = CalendarUT.Scope.Segment.SupportedYears;
        // Assert
        Assert.Equal(FirstYear, supportedYears.Min);
        Assert.Equal(LastYear, supportedYears.Max);
    }

    [Fact]
    public sealed override void Algorithm_Prop() =>
        Assert.Equal(CalendricalAlgorithm.Arithmetical, CalendarUT.Algorithm);

    [Fact]
    public sealed override void Family_Prop() =>
        Assert.Equal(CalendricalFamily.Solar, CalendarUT.Family);

    [Fact]
    public sealed override void PeriodicAdjustments_Prop() =>
        Assert.Equal(CalendricalAdjustments.Days, CalendarUT.PeriodicAdjustments);
}

