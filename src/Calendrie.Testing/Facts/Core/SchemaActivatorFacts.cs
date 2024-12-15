// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core;

internal static class SchemaActivatorFacts
{
    public static void Test<TSchema>()
        where TSchema : ISchemaActivator<TSchema>, ICalendricalSchema
    {
        var sch1 = TSchema.CreateInstance();
        var sch2 = TSchema.CreateInstance();

        Assert.NotSame(sch1, sch2);
    }
}
