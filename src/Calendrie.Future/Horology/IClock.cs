// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Horology;

/// <summary>
/// Represents a clock.
/// </summary>
public interface IClock
{
    /// <summary>
    /// Obtains a <see cref="DayNumber"/> value representing the current day.
    /// </summary>
    [Pure] DayNumber Today();
}
