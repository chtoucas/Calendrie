// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Specialized;

using Calendrie.Specialized;

public static class StandardScopeFacts
{
    public static readonly TheoryData<int> InvalidYearData =
    [
        int.MinValue,
        StandardScope.MinYear - 1,
        StandardScope.MaxYear + 1,
        int.MaxValue,
    ];

    public static readonly TheoryData<int> ValidYearData =
    [
        StandardScope.MinYear,
        StandardScope.MinYear + 1,
        StandardScope.MaxYear - 1,
        StandardScope.MaxYear
    ];
}
