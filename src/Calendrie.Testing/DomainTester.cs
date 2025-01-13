// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing;

using Calendrie.Core.Intervals;

public sealed class DomainTester
{
    public DomainTester(Range<DayNumber> domain)
    {
        // Un peu naïf mais pour le moment on s'en contentera pour le moment.
        var (min, max) = domain.Endpoints;
        ValidDayNumbers =
        [
            min,
            min + 1,
            max - 1,
            max,
        ];
        InvalidDayNumbers =
        [
            DayNumber.MinValue,
            min - 1,
            max + 1,
            DayNumber.MaxValue,
        ];
    }

    public IEnumerable<DayNumber> ValidDayNumbers { get; }
    public IEnumerable<DayNumber> InvalidDayNumbers { get; }

    public void TestInvalidDayNumber(Action<DayNumber> fun, string argName = "dayNumber")
    {
        foreach (var dayNumber in InvalidDayNumbers)
        {
            AssertEx.ThrowsAoorexn(argName, () => fun.Invoke(dayNumber));
        }
    }

    public void TestInvalidDayNumber<T>(Func<DayNumber, T> fun, string argName = "dayNumber")
    {
        foreach (var dayNumber in InvalidDayNumbers)
        {
            AssertEx.ThrowsAoorexn(argName, () => fun.Invoke(dayNumber));
        }
    }
}
