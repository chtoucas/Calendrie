// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data;

public static class EnumDataSet
{
    /// <summary>
    /// Gets invalid values for <see cref="DayOfWeek"/>.
    /// </summary>
    public static TheoryData<DayOfWeek> InvalidDayOfWeekData { get; } =
    [
        (DayOfWeek)(-1),
        (DayOfWeek)7,
    ];

    /// <summary>
    /// Gets all legit values of <see cref="DayOfWeek"/>.
    /// </summary>
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

    //
    // Calendrie
    //

    /// <summary>
    /// Gets invalid values for <see cref="IsoWeekday"/>.
    /// </summary>
    public static TheoryData<IsoWeekday> InvalidIsoWeekdayData { get; } =
    [
        0,
        (IsoWeekday)8
    ];

    /// <summary>
    /// Gets all legit values of <see cref="IsoWeekday"/>.
    /// </summary>
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

    /// <summary>
    /// Gets invalid values for <see cref="AdditionRule"/>.
    /// </summary>
    public static TheoryData<AdditionRule> InvalidAdditionRuleData { get; } =
    [
        (AdditionRule)(-1),
        AdditionRule.Overflow + 1,
    ];

    /// <summary>
    /// Gets all legit values of <see cref="AdditionRule"/>.
    /// </summary>
    public static TheoryData<AdditionRule> AdditionRuleData { get; } =
    [
        AdditionRule.Truncate,
        AdditionRule.Overspill,
        AdditionRule.Exact,
        AdditionRule.Overflow,
    ];

    /// <summary>
    /// Gets invalid values for <see cref="CalendricalAlgorithm"/>.
    /// </summary>
    public static TheoryData<CalendricalAlgorithm> InvalidCalendricalAlgorithmData { get; } =
    [
        (CalendricalAlgorithm)(-1),
        CalendricalAlgorithm.Observational + 1,
    ];

    /// <summary>
    /// Gets all legit values of <see cref="CalendricalAlgorithm"/>.
    /// </summary>
    public static TheoryData<CalendricalAlgorithm> CalendricalAlgorithmData { get; } =
    [
        CalendricalAlgorithm.Arithmetical,
        CalendricalAlgorithm.Astronomical,
        CalendricalAlgorithm.Observational,
        CalendricalAlgorithm.Unknown,
    ];

    /// <summary>
    /// Gets invalid values for <see cref="CalendricalFamily"/>.
    /// </summary>
    public static TheoryData<CalendricalFamily> InvalidCalendricalFamilyData { get; } =
    [
        (CalendricalFamily)(-1),
        CalendricalFamily.Lunisolar + 1,
    ];

    /// <summary>
    /// Gets all legit values of <see cref="CalendricalFamily"/>.
    /// </summary>
    public static TheoryData<CalendricalFamily> CalendricalFamilyData { get; } =
    [
        CalendricalFamily.AnnusVagus,
        CalendricalFamily.Lunar,
        CalendricalFamily.Lunisolar,
        CalendricalFamily.Other,
        CalendricalFamily.Solar,
    ];

    /// <summary>
    /// Gets the pre-defined values of <see cref="CalendricalAdjustments"/>.
    /// </summary>
    //
    // All pre-defined values. Being a flag enum, other combinations are legit.
    public static TheoryData<CalendricalAdjustments> CalendricalAdjustmentsData { get; } =
    [
        CalendricalAdjustments.Days,
        CalendricalAdjustments.DaysAndMonths,
        CalendricalAdjustments.Months,
        CalendricalAdjustments.None,
        CalendricalAdjustments.Weeks,
    ];
}
