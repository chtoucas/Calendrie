﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Testing.Data;

/// <summary>
/// Provides data-driven tests for the <see cref="IDate{TSelf}"/> type.
/// </summary>
public abstract partial class IDateFacts<TDate, TCalendar, TDataSet> :
    IAbsoluteDateFacts<TDate, TDataSet>
    where TCalendar : Calendar
    where TDate : struct, IDate<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected IDateFacts(TCalendar calendar) : base(GetDomain(calendar))
    {
        Debug.Assert(calendar != null);

        Calendar = calendar;
    }

    private static Range<DayNumber> GetDomain(Calendar calendar)
    {
        ArgumentNullException.ThrowIfNull(calendar);

        return calendar.Scope.Domain;
    }

    public TCalendar Calendar { get; }

    protected sealed override TDate GetDate(int y, int m, int d) => TDate.Create(y, m, d);

    [Fact]
    public void ToString_InvariantCulture()
    {
        var date = GetDate(1, 1, 1);
        string str = FormattableString.Invariant($"01/01/0001 ({Calendar})");
        // Act & Assert
        Assert.Equal(str, date.ToString());
    }

    // Althought, we do not usually test static methods/props in a fact class,
    // the situation is a bit different here since this is a static method on a
    // __type__.

    //[Fact]
    //public void Today()
    //{
    //    // This test may fail if there is a change of day between the two calls
    //    // to Today().
    //    var today = DayNumber.Today();
    //    // Act & Assert
    //    Assert.Equal(today, TDate.Today().DayNumber);
    //}

    //[Theory, MemberData(nameof(DayNumberInfoData))]
    //public void FromDayNumber(DayNumberInfo info)
    //{
    //    var (dayNumber, y, m, d) = info;
    //    var date = GetDate(y, m, d);
    //    // Act & Assert
    //    Assert.Equal(date, TDate.FromDayNumber(dayNumber));
    //}
}
