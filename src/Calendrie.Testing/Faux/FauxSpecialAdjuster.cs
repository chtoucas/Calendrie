﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Faux;

using Calendrie.Hemerology;
using Calendrie.Specialized;

public sealed class FauxSpecialAdjuster<TDate> : SpecialAdjuster<TDate>
    where TDate : IDateable
{
    public FauxSpecialAdjuster(MinMaxYearScope scope) : base(scope) { }

    private protected override TDate NewDate(int daysSinceEpoch) =>
        throw new NotSupportedException();
}
