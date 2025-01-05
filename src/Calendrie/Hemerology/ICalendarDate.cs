// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Defines a date.
/// </summary>
public interface ICalendarDate : IDateable, IAbsoluteDate { }

/// <summary>
/// Defines a date type.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface ICalendarDate<TSelf> :
    ICalendarDate,
    IAbsoluteDate<TSelf>,
    IAdjustableDateable<TSelf>,
    // Non-standard math ops
    IMonthArithmetic<TSelf>,
    IYearArithmetic<TSelf>
    where TSelf : ICalendarDate<TSelf>
{ }
