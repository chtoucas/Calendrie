// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Intervals;

/// <summary>
/// Provides static methods related to the (proleptic) scope of a calendar
/// supporting <i>all</i> dates within the range [-999_998..999_999] of years.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static class ProlepticScope
{
    // Even if this class becomes public, these constants MUST stay internal
    // in case we change their values in the future.

    /// <summary>
    /// Represents the earliest supported year.
    /// <para>This field is a constant equal to -999_998.</para>
    /// </summary>
    internal const int MinYear = -999_998;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 999_999.</para>
    /// </summary>
    internal const int MaxYear = 999_999;

    /// <summary>
    /// Represents the range of supported years.
    /// </summary>
    public static readonly Range<int> SupportedYears = Range.Create(MinYear, MaxYear);
}
