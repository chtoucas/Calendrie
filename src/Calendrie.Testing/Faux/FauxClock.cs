// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Faux;

using Calendrie.Horology;

public sealed class FauxClock : IClock
{
    private readonly int _daysSinceZero;

    public FauxClock(int daysSinceZero)
    {
        _daysSinceZero = daysSinceZero;
    }

    [Pure]
    public DayNumber Today() => new(_daysSinceZero);
}
