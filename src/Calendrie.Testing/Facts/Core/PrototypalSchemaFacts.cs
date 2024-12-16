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

        var kernel = SchemaUT as ICalendar;

        _ = kernel.IsLeapYear(Yemoda.MinYear);
        _ = kernel.IsLeapYear(Yemoda.MaxYear);

        _ = kernel.CountMonthsInYear(int.MinValue);
        _ = kernel.CountMonthsInYear(int.MaxValue);

        _ = kernel.CountDaysInYear(Yemoda.MinYear);
        _ = kernel.CountDaysInYear(Yemoda.MaxYear);

        for (int m = 1; m <= MaxMonth; m++)
        {
            _ = kernel.IsIntercalaryMonth(int.MinValue, m);
            _ = kernel.IsIntercalaryMonth(int.MaxValue, m);

            _ = kernel.CountDaysInMonth(Yemoda.MinYear, m);
            _ = kernel.CountDaysInMonth(Yemoda.MaxYear, m);

            for (int d = 1; d <= MaxDay; d++)
            {
                _ = kernel.IsIntercalaryDay(int.MinValue, m, d);
                _ = kernel.IsIntercalaryDay(int.MaxValue, m, d);

                _ = kernel.IsSupplementaryDay(int.MinValue, m, d);
                _ = kernel.IsSupplementaryDay(int.MaxValue, m, d);
            }
        }
    }
}
