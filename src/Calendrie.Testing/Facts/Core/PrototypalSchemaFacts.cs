// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Testing.Data;

/// <summary>
/// Provides facts about <see cref="PrototypalSchema"/>.
/// </summary>
[TestPerformance(TestPerformance.SlowBundle)]
public abstract class PrototypalSchemaFacts<TDataSet> :
    ICalendricalSchemaPlusFacts<PrototypalSchema, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected PrototypalSchemaFacts(PrototypalSchema schema) : base(schema) { }

    public ICalendricalSchemaPlus PrototypeUT => SchemaUT;

    [Fact]
    public override void SupportedYears_Prop() =>
        Assert.Equal(Range.Create(-9998, 9999), SchemaUT.SupportedYears);

    [Fact]
    public override void KernelDoesNotOverflow()
    {
        // Min/MaxYear? We can't take Int32.Min/MaxValue, that's why we override
        // the base method... Yemoda.Min/MaxValue is something not too small that
        // should work most of the time.
        // See also SystemSchemaFacts.KernelDoesNotOverflow()

        _ = SchemaUT.IsLeapYear(Yemoda.MinYear);
        _ = SchemaUT.IsLeapYear(Yemoda.MaxYear);

        _ = SchemaUT.CountMonthsInYear(int.MinValue);
        _ = SchemaUT.CountMonthsInYear(int.MaxValue);

        _ = SchemaUT.CountDaysInYear(Yemoda.MinYear);
        _ = SchemaUT.CountDaysInYear(Yemoda.MaxYear);

        for (int m = 1; m <= MaxMonth; m++)
        {
            _ = SchemaUT.IsIntercalaryMonth(int.MinValue, m);
            _ = SchemaUT.IsIntercalaryMonth(int.MaxValue, m);

            _ = SchemaUT.CountDaysInMonth(Yemoda.MinYear, m);
            _ = SchemaUT.CountDaysInMonth(Yemoda.MaxYear, m);

            for (int d = 1; d <= MaxDay; d++)
            {
                _ = SchemaUT.IsIntercalaryDay(int.MinValue, m, d);
                _ = SchemaUT.IsIntercalaryDay(int.MaxValue, m, d);

                _ = SchemaUT.IsSupplementaryDay(int.MinValue, m, d);
                _ = SchemaUT.IsSupplementaryDay(int.MaxValue, m, d);
            }
        }
    }
}
