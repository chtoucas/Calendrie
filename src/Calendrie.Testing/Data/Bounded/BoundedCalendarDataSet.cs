﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data.Bounded;

using Calendrie.Core;

// NB: while it would have been perfectly reasonnable to use the simpler
// constraint where TDataSet is only an ICalendarDataSet, I prefer to restrict
// a bit the scope of this class.

/// <summary>
/// Defines test data for a <i>bounded</i> calendar and provides a base for
/// derived classes.
/// </summary>
/// <typeparam name="TDataSet">The type that represents the original calendar
/// dataset.</typeparam>
public class BoundedCalendarDataSet<TDataSet> : ICalendarDataSet
    where TDataSet : UnboundedCalendarDataSet
{
    public BoundedCalendarDataSet(TDataSet unbounded, IDataFilter dataFilter)
    {
        ArgumentNullException.ThrowIfNull(unbounded);
        ArgumentNullException.ThrowIfNull(dataFilter);

        Unbounded = unbounded;
        DataFilter = dataFilter;

        Epoch = unbounded.Epoch;
        SampleCommonYear = unbounded.SampleCommonYear;
        SampleLeapYear = unbounded.SampleLeapYear;
    }

    /// <summary>
    /// Gets the unbounded dataset.
    /// </summary>
    protected TDataSet Unbounded { get; }

    /// <summary>
    /// Gets the data filter.
    /// </summary>
    protected IDataFilter DataFilter { get; }

    public DayNumber Epoch { get; }

    public DataGroup<DayNumberInfo> DayNumberInfoData => Unbounded.DayNumberInfoData.WhereT(DataFilter.Filter);

    public DataGroup<YearDayNumber> StartOfYearDayNumberData => Unbounded.StartOfYearDayNumberData.WhereT(DataFilter.Filter);
    public DataGroup<YearDayNumber> EndOfYearDayNumberData => Unbounded.EndOfYearDayNumberData.WhereT(DataFilter.Filter);

    //
    // Affine data
    //

    public int SampleCommonYear { get; }
    public int SampleLeapYear { get; }

    public DataGroup<MonthsSinceEpochInfo> MonthsSinceEpochInfoData => Unbounded.MonthsSinceEpochInfoData.WhereT(DataFilter.Filter);
    public DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData => Unbounded.DaysSinceEpochInfoData.WhereT(DataFilter.Filter);

    public DataGroup<DateInfo> DateInfoData => Unbounded.DateInfoData.WhereT(DataFilter.Filter);
    public DataGroup<MonthInfo> MonthInfoData => Unbounded.MonthInfoData.WhereT(DataFilter.Filter);
    public DataGroup<YearInfo> YearInfoData => Unbounded.YearInfoData.WhereT(DataFilter.Filter);
    public DataGroup<CenturyInfo> CenturyInfoData => Unbounded.CenturyInfoData.WhereT(DataFilter.Filter);

    public DataGroup<YemodaAnd<int>> DaysInYearAfterDateData => Unbounded.DaysInYearAfterDateData.WhereT(DataFilter.Filter);
    public DataGroup<YemodaAnd<int>> DaysInMonthAfterDateData => Unbounded.DaysInMonthAfterDateData.WhereT(DataFilter.Filter);

    public DataGroup<Yemoda> StartOfYearPartsData => Unbounded.StartOfYearPartsData.WhereT(DataFilter.Filter);
    public DataGroup<Yemoda> EndOfYearPartsData => Unbounded.EndOfYearPartsData.WhereT(DataFilter.Filter);

    public DataGroup<YearMonthsSinceEpoch> StartOfYearMonthsSinceEpochData => Unbounded.StartOfYearMonthsSinceEpochData.WhereT(DataFilter.Filter);
    public DataGroup<YearMonthsSinceEpoch> EndOfYearMonthsSinceEpochData => Unbounded.EndOfYearMonthsSinceEpochData.WhereT(DataFilter.Filter);

    public DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData => Unbounded.StartOfYearDaysSinceEpochData.WhereT(DataFilter.Filter);
    public DataGroup<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData => Unbounded.EndOfYearDaysSinceEpochData.WhereT(DataFilter.Filter);

    // Normally, we don't have to filter the three following properties.
    public TheoryData<int, int> InvalidMonthFieldData => Unbounded.InvalidMonthFieldData;
    public TheoryData<int, int, int> InvalidDayFieldData => Unbounded.InvalidDayFieldData;
    public TheoryData<int, int> InvalidDayOfYearFieldData => Unbounded.InvalidDayOfYearFieldData;

    public DataGroup<YemodaPair> ConsecutiveDaysData => Unbounded.ConsecutiveDaysData.WhereT(DataFilter.Filter);
    public DataGroup<YedoyPair> ConsecutiveDaysOrdinalData => Unbounded.ConsecutiveDaysOrdinalData.WhereT(DataFilter.Filter);
    public DataGroup<YemoPair> ConsecutiveMonthsData => Unbounded.ConsecutiveMonthsData.WhereT(DataFilter.Filter);

    public DataGroup<YemodaPairAnd<int>> AddDaysData => Unbounded.AddDaysData.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<int>> AddMonthsData => Unbounded.AddMonthsData.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<int>> AddYearsData => Unbounded.AddYearsData.WhereT(DataFilter.Filter);
    public DataGroup<YedoyPairAnd<int>> AddDaysOrdinalData => Unbounded.AddDaysOrdinalData.WhereT(DataFilter.Filter);
    public DataGroup<YedoyPairAnd<int>> AddYearsOrdinalData => Unbounded.AddYearsOrdinalData.WhereT(DataFilter.Filter);
    public DataGroup<YemoPairAnd<int>> AddMonthsMonthData => Unbounded.AddMonthsMonthData.WhereT(DataFilter.Filter);
    public DataGroup<YemoPairAnd<int>> AddYearsMonthData => Unbounded.AddYearsMonthData.WhereT(DataFilter.Filter);

    public DataGroup<YemodaPairAnd<int>> CountMonthsBetweenData => Unbounded.CountMonthsBetweenData.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<int>> CountYearsBetweenData => Unbounded.CountYearsBetweenData.WhereT(DataFilter.Filter);
    public DataGroup<YedoyPairAnd<int>> CountYearsBetweenOrdinalData => Unbounded.CountYearsBetweenOrdinalData.WhereT(DataFilter.Filter);
    public DataGroup<YemoPairAnd<int>> CountYearsBetweenMonthData => Unbounded.CountYearsBetweenMonthData.WhereT(DataFilter.Filter);
}
