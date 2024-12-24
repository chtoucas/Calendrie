// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core;
using Calendrie.Core.Prototypes;
using Calendrie.Testing.Data;
using Calendrie.Testing.Faux;

/// <summary>
/// Provides facts about <see cref="RegularSchema"/>.
/// </summary>
[TestPerformance(TestPerformance.SlowBundle)]
public abstract partial class RegularSchemaPrototypeFacts<TDataSet> :
    CalendricalSchemaFacts<RegularSchemaPrototype, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected RegularSchemaPrototypeFacts(RegularSchemaPrototype schema) : base(schema) { }

    protected RegularSchemaPrototypeFacts(ICalendricalSchema schema)
        : base(FauxRegularSchemaPrototype.Create(schema)) { }

    [Fact]
    public sealed override void SupportedYears_Prop()
    {
        var sch = SchemaUT;
        if (sch.IsProleptic)
        {
            Assert.Equal(sch.SupportedYears, RegularSchemaPrototype.ProlepticSupportedYears);
        }
        else
        {
            Assert.Equal(sch.SupportedYears, RegularSchemaPrototype.StandardSupportedYears);
        }
    }
}
