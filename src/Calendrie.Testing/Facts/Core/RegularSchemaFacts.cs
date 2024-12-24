// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core;
using Calendrie.Testing.Data;
using Calendrie.Testing.Faux;

/// <summary>
/// Provides facts about <see cref="RegularSchema"/>.
/// </summary>
public abstract partial class RegularSchemaFacts<TDataSet> :
    CalendricalSchemaFacts<RegularSchema, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected RegularSchemaFacts(ICalendricalSchema schema) : base(FauxRegularSchema.Create(schema)) { }

    [Fact]
    public sealed override void SupportedYears_Prop()
    {
        var sch = SchemaUT;
        if (sch.IsProleptic)
        {
            Assert.Equal(sch.SupportedYears, RegularSchema.ProlepticSupportedYears);
        }
        else
        {
            Assert.Equal(sch.SupportedYears, RegularSchema.StandardSupportedYears);
        }
    }
}
