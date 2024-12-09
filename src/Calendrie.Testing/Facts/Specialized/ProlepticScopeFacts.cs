// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Specialized;

using Calendrie.Specialized;

public static class ProlepticScopeFacts
{
    public static readonly TheoryData<int> InvalidYearData =
    [
        int.MinValue,
        ProlepticScope.MinYear - 1,
        ProlepticScope.MaxYear + 1,
        int.MaxValue,
    ];

    public static readonly TheoryData<int> ValidYearData =
    [
        ProlepticScope.MinYear,
        ProlepticScope.MinYear + 1,
        -1,
        0,
        1,
        ProlepticScope.MaxYear - 1,
        ProlepticScope.MaxYear
    ];
}
