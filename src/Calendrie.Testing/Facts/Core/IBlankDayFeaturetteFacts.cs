// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core.Schemas;
using Calendrie.Testing.Data;

internal abstract class IBlankDayFeaturetteFacts<TSchema, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TSchema : IBlankDayFeaturette
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    private readonly TSchema _schema;

    protected IBlankDayFeaturetteFacts(TSchema schema)
    {
        ArgumentNullException.ThrowIfNull(schema);
        _schema = schema;
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void IsBlankDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        bool actual = _schema.IsBlankDay(y, m, d);
        // Assert
        Assert.Equal(info.IsSupplementary, actual);
    }
}
