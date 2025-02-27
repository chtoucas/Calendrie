﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Horology.Ntp;

// The binary value for a stratum is a 3-bit unsigned integer.
// Contrary to LeapIndicator and NtpMode, there is no one-to-one mapping
// between NtpStratum and the binary values (StratumLevel).

/// <summary>
/// Specifies the NTP stratum family.
/// </summary>
[SuppressMessage("Naming", "CA1700:Do not name enum values 'Reserved'", Justification = "NTP wording")]
public enum StratumFamily
{
    /// <summary>
    /// The NTP stratum is not known.
    /// <para>This value is considered to be <i>invalid</i>. We never use it,
    /// and neither should you.</para>
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Unspecified or unavailable (Kiss-o'-Death message).
    /// </summary>
    Unspecified,

    /// <summary>
    /// Primary reference (e.g., synchronized by radio clock).
    /// </summary>
    PrimaryReference,

    /// <summary>
    /// Secondary reference (synchronized by NTP or SNTP).
    /// </summary>
    SecondaryReference,

    /// <summary>
    /// Unsynchronized.
    /// </summary>
    Unsynchronized,

    /// <summary>
    /// Reserved.
    /// </summary>
    Reserved
}
