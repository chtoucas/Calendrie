// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Defines methods specific to calendars featuring blank days.
/// <para>A blank day does not belong to any month and is kept outside the
/// weekday cycle.</para>
/// </summary>
public interface IBlankDay : IDateable
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is a blank day;
    /// otherwise returns <see langword="false"/>.
    /// </summary>
    bool IsBlank { get; }
}
