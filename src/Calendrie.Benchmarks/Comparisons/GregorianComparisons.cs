// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Comparisons;

using Calendrie.Specialized;

using NodaTime;

public abstract class GregorianComparisons
{
    private protected DayNumber dayNumber;
    private protected CivilDate civilDate;
    private protected GregorianDate gregorianDate;
    private protected DateOnly dateOnly;
    private protected DateTime dateTime;
    private protected LocalDate localDate;

    private protected GregorianComparisons(GJDateType type = GJDateType.FixedFast)
    {
        Parts = BenchmarkHelpers.CreateGregorianParts(type);
    }

    protected DateParts Parts { get; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        var (y, m, d) = Parts;

        civilDate = new(y, m, d);
        gregorianDate = new(y, m, d);
        dayNumber = civilDate.DayNumber;

        dateTime = new(y, m, d);
        dateOnly = new(y, m, d);
        localDate = new(y, m, d);
    }
}
