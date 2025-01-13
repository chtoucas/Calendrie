// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core;
using Calendrie.Core.Prototyping;
using Calendrie.Testing.Data;
using Calendrie.Testing.Faux;

/// <summary>
/// Provides facts about the <see cref="RegularSchemaPrototype"/> type.
/// </summary>
public abstract partial class RegularSchemaPrototypeFacts<TDataSet> :
    ICalendricalSchemaFacts<RegularSchemaPrototype, TDataSet>
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
            Assert.Equal(sch.SupportedYears, PrototypeHelpers.Proleptic);
        }
        else
        {
            Assert.Equal(sch.SupportedYears, PrototypeHelpers.Standard);
        }
    }
}
