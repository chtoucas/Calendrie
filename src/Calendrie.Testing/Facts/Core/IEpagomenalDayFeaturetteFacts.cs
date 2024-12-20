﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core.Schemas;
using Calendrie.Testing.Data;

internal abstract class IEpagomenalDayFeaturetteFacts<TSchema, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TSchema : IEpagomenalDayFeaturette
    where TDataSet : ICalendricalDataSet, IEpagomenalDataSet, ISingleton<TDataSet>
{
    private readonly TSchema _schema;

    protected IEpagomenalDayFeaturetteFacts(TSchema schema)
    {
        ArgumentNullException.ThrowIfNull(schema);
        _schema = schema;
    }

    public static DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => DataSet.EpagomenalDayInfoData;

    [Theory, MemberData(nameof(DateInfoData))]
    public void IsEpagomenalDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        bool isEpagomenal = _schema.IsEpagomenalDay(y, m, d, out int epanum);
        // Assert
        Assert.Equal(info.IsSupplementary, isEpagomenal);
        if (isEpagomenal)
        {
            Assert.True(epanum > 0);
        }
        else
        {
            Assert.Equal(0, epanum);
        }
    }

    [Theory, MemberData(nameof(EpagomenalDayInfoData))]
    public void IsEpagomenalDay_EpagomenalNumber(YemodaAnd<int> info)
    {
        var (y, m, d, epanum) = info;
        // Act
        bool isEpagomenal = _schema.IsEpagomenalDay(y, m, d, out int epanumA);
        // Assert
        Assert.True(isEpagomenal);
        Assert.Equal(epanum, epanumA);
    }
}
