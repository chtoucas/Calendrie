// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.GregorianSchemaTests;

using Calendrie.Core.Schemas;

using static Benchmarks.BenchmarkHelpers;

public abstract class GregorianSchemaComparisons
{
    private protected GregorianSchema schema = new();
    private protected CivilSchema civilSchema = new();

    private protected int year, month, day, daysSinceEpoch;
    private protected long yearL, daysSinceEpochL;

    private protected GregorianSchemaComparisons(GJDateType type = GJDateType.FixedFast)
    {
        (year, month, day) = CreateGregorianParts(type);
        daysSinceEpoch = DayNumber.FromGregorianParts(year, month, day).DaysSinceZero;
        (yearL, daysSinceEpochL) = (year, daysSinceEpoch);
    }
}
