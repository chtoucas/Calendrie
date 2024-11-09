// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Hemerology;
using Calendrie.Hemerology.Scopes;

public sealed class FauxSpecialAdjuster<TDate> : SpecialAdjuster<TDate>
    where TDate : IDateable
{
    public FauxSpecialAdjuster(MinMaxYearScope scope) : base(scope) { }

    private protected override TDate GetDate(int daysSinceEpoch) =>
        throw new NotSupportedException();
}
