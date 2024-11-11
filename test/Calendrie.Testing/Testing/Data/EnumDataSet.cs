// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data;

//using Calendrie.Simple;

public static class EnumDataSet
{
    public static TheoryData<DayOfWeek> InvalidDayOfWeekData { get; } =
    [
        (DayOfWeek)(-1),
        (DayOfWeek)7,
    ];

    public static TheoryData<DayOfWeek> DayOfWeekData { get; } =
    [
        DayOfWeek.Monday,
        DayOfWeek.Tuesday,
        DayOfWeek.Wednesday,
        DayOfWeek.Thursday,
        DayOfWeek.Friday,
        DayOfWeek.Saturday,
        DayOfWeek.Sunday,
    ];

    public static TheoryData<IsoWeekday> InvalidIsoWeekdayData { get; } =
    [
        0,
        (IsoWeekday)8
    ];

    public static TheoryData<IsoWeekday> IsoWeekdayData { get; } =
    [
        IsoWeekday.Monday,
        IsoWeekday.Tuesday,
        IsoWeekday.Wednesday,
        IsoWeekday.Thursday,
        IsoWeekday.Friday,
        IsoWeekday.Saturday,
        IsoWeekday.Sunday,
    ];

    //
    // Calendrie
    //

    public static TheoryData<AdditionRule> InvalidAdditionRuleData { get; } =
    [
        (AdditionRule)(-1),
        AdditionRule.Overflow + 1,
    ];

    public static TheoryData<AdditionRule> AdditionRuleData { get; } =
    [
        AdditionRule.Truncate,
        AdditionRule.Overspill,
        AdditionRule.Exact,
        AdditionRule.Overflow,
    ];

    public static TheoryData<CalendricalAlgorithm> InvalidCalendricalAlgorithmData { get; } =
    [
        (CalendricalAlgorithm)(-1),
        CalendricalAlgorithm.Observational + 1,
    ];

    public static TheoryData<CalendricalAlgorithm> CalendricalAlgorithmData { get; } =
    [
        CalendricalAlgorithm.Arithmetical,
        CalendricalAlgorithm.Astronomical,
        CalendricalAlgorithm.Observational,
        CalendricalAlgorithm.Unknown,
    ];

    public static TheoryData<CalendricalFamily> InvalidCalendricalFamilyData { get; } =
    [
        (CalendricalFamily)(-1),
        CalendricalFamily.Lunisolar + 1,
    ];

    public static TheoryData<CalendricalFamily> CalendricalFamilyData { get; } =
    [
        CalendricalFamily.AnnusVagus,
        CalendricalFamily.Lunar,
        CalendricalFamily.Lunisolar,
        CalendricalFamily.Other,
        CalendricalFamily.Solar,
    ];

    // All pre-defined values. Being a flag enum, other combinations are legit.
    public static TheoryData<CalendricalAdjustments> CalendricalAdjustmentsData { get; } =
    [
        CalendricalAdjustments.Days,
        CalendricalAdjustments.DaysAndMonths,
        CalendricalAdjustments.Months,
        CalendricalAdjustments.None,
        CalendricalAdjustments.Weeks,
    ];

    //public static TheoryData<CalendarId> InvalidCalendarIdData { get; } = new()
    //{
    //    (CalendarId)(-1),
    //    (CalendarId)(int)(1 + Cuid.MaxSystem)
    //};

    //public static TheoryData<CalendarId> CalendarIdData { get; } = new()
    //{
    //    CalendarId.Armenian,
    //    CalendarId.Civil,
    //    CalendarId.Coptic,
    //    CalendarId.Ethiopic,
    //    CalendarId.Gregorian,
    //    CalendarId.Julian,
    //    CalendarId.TabularIslamic,
    //    CalendarId.Zoroastrian,
    //};

    ////
    //// Calendrie.Simple
    ////

    //internal static TheoryData<Cuid> UnfixedCuidData { get; } = new()
    //{
    //    Cuid.MaxSystem + 1,
    //    Cuid.MinUser,
    //    Cuid.Max,
    //    Cuid.Invalid,
    //};

    //internal static TheoryData<Cuid> FixedCuidData { get; } = new()
    //{
    //    Cuid.Armenian,
    //    Cuid.Civil,
    //    Cuid.Coptic,
    //    Cuid.Ethiopic,
    //    Cuid.Gregorian,
    //    Cuid.Julian,
    //    Cuid.TabularIslamic,
    //    Cuid.Zoroastrian,
    //};
}
