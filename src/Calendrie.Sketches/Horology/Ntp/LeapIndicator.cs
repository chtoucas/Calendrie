﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Horology.Ntp;

// LI is a 2-bit unsigned integer. The values are in the range from 0 to 3,
// they are fixed in the sense that there is no room left for new values.
// Ignoring O, all values in LeapIndicator are fixed manually to ensure that
// (int)LeapIndicator - 1 matches the binary value.

/// <summary>
/// Specifies the warning of an impending leap second to be inserted/deleted in
/// the last minute of the current day.
/// </summary>
[SuppressMessage("Design", "CA1028:Enum Storage should be Int32", Justification = "<Pending>")]
public enum LeapIndicator : byte
{
    /// <summary>
    /// The leap indicator is not known.
    /// <para>This value is considered to be <i>invalid</i>. We never use it,
    /// and neither should you.</para>
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// No warning.
    /// </summary>
    NoWarning = 1,

    /// <summary>
    /// Last minute has 61 seconds.
    /// </summary>
    PositiveLeapSecond = 2,

    /// <summary>
    /// Last minute has 59 seconds.
    /// </summary>
    NegativeLeapSecond = 3,

    /// <summary>
    /// Alarm condition (clock not synchronized).
    /// </summary>
    Unsynchronized = 4
}
