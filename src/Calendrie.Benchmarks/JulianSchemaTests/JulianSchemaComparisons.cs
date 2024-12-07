// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.JulianSchemaTests;

using Calendrie.Core.Schemas;

using static Benchmarks.BenchmarkHelpers;

public abstract class JulianSchemaComparisons
{
    private protected JulianSchema schema = new();

    private protected int year, month, day, daysSinceEpoch;
    private protected long yearL, daysSinceEpochL;

    private protected JulianSchemaComparisons(GJDateType type = GJDateType.FixedFast)
    {
        (year, month, day) = CreateJulianParts(type);
        daysSinceEpoch = DayNumber.FromJulianParts(year, month, day) - DayZero.OldStyle;
        (yearL, daysSinceEpochL) = (year, daysSinceEpoch);
    }
}
