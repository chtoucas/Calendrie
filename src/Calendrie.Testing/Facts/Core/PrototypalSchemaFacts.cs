// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core;
using Calendrie.Core.Prototypes;
using Calendrie.Testing.Data;

/// <summary>
/// Provides facts about <see cref="PrototypalSchema"/>.
/// </summary>
[TestPerformance(TestPerformance.SlowBundle)]
public abstract class PrototypalSchemaFacts<TDataSet> :
    ICalendricalSchemaFacts<PrototypalSchema, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected PrototypalSchemaFacts(PrototypalSchema schema) : base(schema) { }

    public ICalendricalSchema PrototypeUT => SchemaUT;

    [Fact]
    public sealed override void SupportedYears_Prop()
    {
        var sch = SchemaUT;
        if (sch.IsProleptic)
        {
            Assert.Equal(sch.SupportedYears, PrototypeHelpers.Proleptic);
        }
        else
        {
            Assert.Equal(sch.SupportedYears, PrototypeHelpers.Standard);
        }
    }

    [Fact]
    public override void KernelDoesNotOverflow()
    {
        // Min/MaxYear? We can't take Int32.Min/MaxValue, that's why we override
        // the base method... Yemoda.Min/MaxValue is something not too small that
        // should work most of the time.
        // See also LimitSchemaFacts.KernelDoesNotOverflow()

        var kernel = SchemaUT as ICalendricalCore;

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
