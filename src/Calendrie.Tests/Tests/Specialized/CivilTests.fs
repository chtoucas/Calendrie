// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Specialized.CivilTests

open Calendrie
open Calendrie.Specialized
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Specialized

open Xunit

// NB: notice the use of StandardGregorianDataSet.

module Prelude =
    let private calendarDataSet = StandardGregorianDataSet.Instance

    let daysSinceEpochInfoData = calendarDataSet.DaysSinceEpochInfoData

    [<Fact>]
    let ``Value of CivilDate.MaxDaysSinceZero`` () =
        CivilDate.MaxDaysSinceZero === CivilCalendar.ScopeT.Segment.SupportedDays.Max

    [<Theory; MemberData(nameof(daysSinceEpochInfoData))>]
    let ``Property DaysSinceZero`` (info: DaysSinceEpochInfo) =
        let (daysSinceEpoch, y, m, d) = info.Deconstruct()
        let date = new CivilDate(y, m, d)

        date.DaysSinceZero === daysSinceEpoch

module Bundles =
    let private chr = new CivilCalendar()

    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<CivilDate, CivilCalendar, StandardGregorianDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new CivilDate(y, m, d);
        override __.GetDate(y, doy) = new CivilDate(y, doy);
        override __.GetDate(dayNumber) = CivilDate.FromDayNumber(dayNumber);

        [<Fact>]
        static member MonthsInYear() = CivilCalendar.MonthsInYear === 12

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<CivilDate, CivilCalendar, StandardGregorianDataSet>(chr)

        override __.MinDate = CivilDate.MinValue
        override __.MaxDate = CivilDate.MaxValue

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = CivilDate.Calendar |> isnotnull

        [<Fact>]
        static member Adjuster_Prop() = CivilDate.Adjuster |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit SpecialAdjusterFacts<CivilDate, StandardGregorianDataSet>(CivilDate.Adjuster)

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)
        override __.GetDate(y, doy) = new CivilDate(y, doy)

    [<Sealed>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<CivilDate, StandardGregorianDataSet>()

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)
