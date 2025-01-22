// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Defines the base for other date types.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IDateBase<TSelf> :
    IDate,
    IAbsoluteDateBase<TSelf>,
    IAdjustableDate<TSelf>,
    // Non-standard math ops
    IMonthFieldMath<TSelf>,
    IYearFieldMath<TSelf>
    where TSelf : IDateBase<TSelf>
{
    /// <summary>
    /// Adds a number of years to the year part of the current instance and
    /// also returns the roundoff in an output parameter, yielding a new value.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of the year field or the range of supported values.
    /// </exception>
    [Pure] TSelf PlusYears(int years, out int roundoff);

    /// <summary>
    /// Adds a number of months to the month part of the current instance and
    /// also returns the roundoff in an output parameter, yielding a new value.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of the month field or the range of supported values.
    /// </exception>
    [Pure] TSelf PlusMonths(int months, out int roundoff);
}
