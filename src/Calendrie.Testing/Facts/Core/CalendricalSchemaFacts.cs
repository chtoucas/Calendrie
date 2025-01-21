// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Testing.Data;

/// <summary>
/// Provides facts about the <see cref="CalendricalSchema"/> type.
/// </summary>
public abstract partial class CalendricalSchemaFacts<TSchema, TDataSet> :
    ICalendricalSchemaFacts<TSchema, TDataSet>
    where TSchema : CalendricalSchema
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected CalendricalSchemaFacts(TSchema schema) : base(schema) { }

    // This property is actually part of CalendricalSchema but being internal
    // it's not publicly testable.
    [Fact] public abstract void Profile_Prop();
}

public partial class CalendricalSchemaFacts<TSchema, TDataSet> // Properties
{
    [Fact]
    public sealed override void Algorithm_Prop() =>
        Assert.Equal(CalendricalAlgorithm.Arithmetical, SchemaUT.Algorithm);

    [Fact]
    public override void SupportedYears_Prop() =>
        Assert.Equal(CalendricalSchema.DefaultSupportedYears, SchemaUT.SupportedYears);

    [Fact]
    public virtual void SupportedYearsCore_Prop() =>
        Assert.Equal(Range.Maximal32, SchemaUT.SupportedYearsCore);
}

public partial class CalendricalSchemaFacts<TSchema, TDataSet> // Methods
{
    [Theory, MemberData(nameof(EndOfYearPartsData))]
    public void GetDatePartsAtEndOfYear(Yemoda ymd)
    {
        int y = ymd.Year;
        // Act
        SchemaUT.GetDatePartsAtEndOfYear(y, out int m, out int d);
        var actual = new Yemoda(y, m, d);
        // Assert
        Assert.Equal(ymd, actual);
    }
}

public partial class CalendricalSchemaFacts<TSchema, TDataSet> // Overflows
{
    [Fact]
    public sealed override void KernelDoesNotOverflow()
    {
        var (minYearCore, maxYearCore) = SchemaUT.SupportedYearsCore.Endpoints;

        _ = SchemaUT.IsLeapYear(minYearCore);
        _ = SchemaUT.IsLeapYear(maxYearCore);

        // NB: right now, it works w/ Int32.Min(Max)Year but it might change
        // in the future with new lunisolar schemas.
        _ = SchemaUT.CountMonthsInYear(int.MinValue);
        _ = SchemaUT.CountMonthsInYear(int.MaxValue);

        _ = SchemaUT.CountDaysInYear(minYearCore);
        _ = SchemaUT.CountDaysInYear(maxYearCore);

        for (int m = 1; m <= MaxMonth; m++)
        {
            _ = SchemaUT.IsIntercalaryMonth(int.MinValue, m);
            _ = SchemaUT.IsIntercalaryMonth(int.MaxValue, m);

            _ = SchemaUT.CountDaysInMonth(minYearCore, m);
            _ = SchemaUT.CountDaysInMonth(maxYearCore, m);

            for (int d = 1; d <= MaxDay; d++)
            {
                _ = SchemaUT.IsIntercalaryDay(int.MinValue, m, d);
                _ = SchemaUT.IsIntercalaryDay(int.MaxValue, m, d);

                _ = SchemaUT.IsSupplementaryDay(int.MinValue, m, d);
                _ = SchemaUT.IsSupplementaryDay(int.MaxValue, m, d);
            }
        }

        if (minYearCore != int.MinValue)
        {
            AssertEx.Overflows(() => SchemaUT.IsLeapYear(int.MinValue));
            AssertEx.Overflows(() => SchemaUT.CountDaysInYear(int.MinValue));
        }
        if (maxYearCore != int.MaxValue)
        {
            AssertEx.Overflows(() => SchemaUT.IsLeapYear(int.MaxValue));
            AssertEx.Overflows(() => SchemaUT.CountDaysInYear(int.MaxValue));
        }
    }

    [Fact] public void GetYear﹍Plain_DoesNotUnderflow() => _ = SchemaUT.GetYear(MinDaysSinceEpoch);
    [Fact] public void GetYear﹍Plain_DoesNotOverflow() => _ = SchemaUT.GetYear(MaxDaysSinceEpoch);
}
