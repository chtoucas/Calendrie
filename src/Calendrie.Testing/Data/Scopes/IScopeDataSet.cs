// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data.Scopes;

public interface IScopeDataSet
{
    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    static abstract int MinYear { get; }

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    static abstract int MaxYear { get; }

    static abstract TheoryData<int> InvalidYearData { get; }
    static abstract TheoryData<int> ValidYearData { get; }
}
