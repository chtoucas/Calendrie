// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using Calendrie.Core.Schemas;

// FIXME(code): temporary

public static class UnsafeSchemas
{
    public static CivilSchema CreateCivilSchema() => new();
    public static Coptic12Schema CreateCoptic12Schema() => new();
    public static Egyptian12Schema CreateEgyptian12Schema() => new();
    public static FrenchRepublican12Schema CreateFrenchRepublican12Schema() => new();
    public static GregorianSchema CreateGregorianSchema() => new();
    public static InternationalFixedSchema CreateInternationalFixedSchema() => new();
    public static JulianSchema CreateJulianSchema() => new();
    public static Persian2820Schema CreatePersian2820Schema() => new();
    public static PositivistSchema CreatePositivistSchema() => new();
    public static TropicaliaSchema CreateTropicaliaSchema() => new();
    public static WorldSchema CreateWorldSchema() => new();
}
