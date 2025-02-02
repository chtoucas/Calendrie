// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core;
using Calendrie.Testing.Data;
using Calendrie.Testing.Faux;

/// <summary>
/// Provides facts about the <see cref="NonRegularSchemaPrototype"/> type.
/// </summary>
public abstract partial class NonRegularSchemaPrototypeFacts<TDataSet> :
    ICalendricalSchemaFacts<NonRegularSchemaPrototype, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected NonRegularSchemaPrototypeFacts(NonRegularSchemaPrototype schema) : base(schema) { }

    protected NonRegularSchemaPrototypeFacts(ICalendricalSchema schema, int minMonthsInYear)
        : base(FauxNonRegularSchemaPrototype.Create(schema, minMonthsInYear)) { }

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
}
