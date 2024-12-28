// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core;
using Calendrie.Core.Schemas;
using Calendrie.Testing.Data;

// REVIEW(fact): if IDaysInMonths becomes public, this should be part of
// LimitSchemaFacts.

internal static class IDaysInMonthsFacts
{
    public static void Test<TSchema>(TSchema schema, int commonYear, int leapYear)
        where TSchema : IDaysInMonths, ICalendricalCore
    {
        if (leapYear != commonYear)
        {
            TestCore(schema, leapYear, leapYear: true);
            TestCore(schema, commonYear, leapYear: false);
        }
        else
        {
            TestCore(schema, commonYear, leapYear: false);
        }
    }

    public static void TestCore<TSchema>(TSchema schema, int y, bool leapYear)
        where TSchema : IDaysInMonths, ICalendricalCore
    {
        ArgumentNullException.ThrowIfNull(schema);

        // Sanity check.
        Assert.Equal(leapYear, schema.IsLeapYear(y));

        var daysInMonths = TSchema.GetDaysInMonthsOfYear(leapYear);

        for (int m = 1; m <= daysInMonths.Length; m++)
        {
            Assert.Equal(
                (m, schema.CountDaysInMonth(y, m)),
                (m, daysInMonths[m - 1]));
        }
    }
}

[Obsolete("Unused")]
internal abstract class IDaysInMonthsFacts<TSchema, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TSchema : IDaysInMonths, ICalendricalCore
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected IDaysInMonthsFacts(TSchema schema)
    {
        ArgumentNullException.ThrowIfNull(schema);

        SchemaUT = schema;
    }

    protected TSchema SchemaUT { get; }

    [Fact]
    public void GetDaysInMonthsOfYear()
    {
        if (SampleLeapYear != SampleCommonYear)
        {
            IDaysInMonthsFacts.TestCore(SchemaUT, SampleCommonYear, leapYear: false);
            IDaysInMonthsFacts.TestCore(SchemaUT, SampleLeapYear, leapYear: true);
        }
        else
        {
            IDaysInMonthsFacts.TestCore(SchemaUT, SampleCommonYear, leapYear: false);
        }
    }
}
