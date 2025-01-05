// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Defines the base for other date types.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface ICalendarDateBase<TSelf> :
    ICalendarDate,
    IAbsoluteDateBase<TSelf>,
    IAdjustableDateable<TSelf>,
    // Non-standard math ops
    IMonthArithmetic<TSelf>,
    IYearArithmetic<TSelf>
    where TSelf : ICalendarDateBase<TSelf>
{ }
