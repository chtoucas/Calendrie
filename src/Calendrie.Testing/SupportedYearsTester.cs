// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing;

using Calendrie.Core.Intervals;

public sealed class SupportedYearsTester
{
    public SupportedYearsTester(Range<int> supportedYears)
    {
        // Un peu naïf mais pour le moment on s'en contentera pour le moment.
        var (minYear, maxYear) = supportedYears.Endpoints;
        ValidYears =
        [
            minYear,
            minYear + 1,
            maxYear - 1,
            maxYear,
        ];
        InvalidYears =
        [
            int.MinValue,
            minYear - 1,
            maxYear + 1,
            int.MaxValue,
        ];
    }

    public IEnumerable<int> ValidYears { get; }
    public IEnumerable<int> InvalidYears { get; }

    public void TestInvalidYear(Action<int> fun, string argName = "year")
    {
        foreach (int y in InvalidYears)
        {
            AssertEx.ThrowsAoorexn(argName, () => fun.Invoke(y));
        }
    }

    public void TestInvalidYear<T>(Func<int, T> fun, string argName = "year")
    {
        foreach (int y in InvalidYears)
        {
            AssertEx.ThrowsAoorexn(argName, () => fun.Invoke(y));
        }
    }

    public void TestInvalidYearTryPattern(Func<int, bool> fun)
    {
        ArgumentNullException.ThrowIfNull(fun);

        foreach (int y in InvalidYears)
        {
            Assert.False(fun.Invoke(y));
        }
    }
}
