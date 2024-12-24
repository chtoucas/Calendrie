// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core;
using Calendrie.Core.Prototypes;
using Calendrie.Testing.Data;
using Calendrie.Testing.Faux;

/// <summary>
/// Provides facts about <see cref="NonRegularSchemaPrototype"/>.
/// </summary>
[TestPerformance(TestPerformance.SlowBundle)]
public abstract partial class NonRegularSchemaPrototypeFacts<TDataSet> :
    CalendricalSchemaFacts<NonRegularSchemaPrototype, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected NonRegularSchemaPrototypeFacts(NonRegularSchemaPrototype schema) : base(schema) { }

    protected NonRegularSchemaPrototypeFacts(ICalendricalSchema schema)
        : base(FauxNonRegularSchemaPrototype.Create(schema)) { }

    [Fact]
    public sealed override void SupportedYears_Prop()
    {
        var sch = SchemaUT;
        if (sch.IsProleptic)
        {
            Assert.Equal(sch.SupportedYears, NonRegularSchemaPrototype.ProlepticSupportedYears);
        }
        else
        {
            Assert.Equal(sch.SupportedYears, NonRegularSchemaPrototype.StandardSupportedYears);
        }
    }
}
