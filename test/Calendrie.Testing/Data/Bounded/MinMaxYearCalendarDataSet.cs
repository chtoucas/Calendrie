// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data.Bounded;

using Calendrie.Core.Intervals;
using Calendrie.Testing.Data;

public class MinMaxYearCalendarDataSet<TDataSet> : BoundedCalendarDataSet<TDataSet>
    where TDataSet : UnboundedCalendarDataSet
{
    public MinMaxYearCalendarDataSet(TDataSet inner, int minYear, int maxYear)
        : this(inner, Range.Create(minYear, maxYear)) { }

    public MinMaxYearCalendarDataSet(TDataSet inner, Range<int> supportedYears)
        : base(inner, new MinMaxYearDataFilter(supportedYears))
    {
        SupportedYears = supportedYears;

        if (supportedYears.Contains(inner.SampleCommonYear) == false)
        {
            throw new ArgumentException("inner.SampleCommonYear is out of range", nameof(inner));
        }
        if (supportedYears.Contains(inner.SampleLeapYear) == false)
        {
            throw new ArgumentException("inner.SampleLeapYear is out of range", nameof(inner));
        }
    }

    public Range<int> SupportedYears { get; }
}
