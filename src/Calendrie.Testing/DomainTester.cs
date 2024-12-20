﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing;

using Calendrie.Core.Intervals;

public sealed class DomainTester
{
    public DomainTester(Range<DayNumber> domain)
    {
        // Un peu naïf mais on s'en contentera pour le moment.
        var (minDayNumber, maxDayNumber) = domain.Endpoints;
        ValidDayNumbers =
        [
            minDayNumber,
            minDayNumber + 1,
            maxDayNumber - 1,
            maxDayNumber,
        ];
        InvalidDayNumbers =
        [
            DayNumber.MinValue,
            minDayNumber - 1,
            maxDayNumber + 1,
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
