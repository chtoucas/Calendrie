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
    where TSelf : struct, IDateBase<TSelf>
{
    /// <summary>
    /// Creates a new instance of the <typeparamref name="TSelf"/> struct
    /// from the specified date components.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the
    /// range of supported years.</exception>
    [Pure] static abstract TSelf Create(int year, int month, int day);
}
