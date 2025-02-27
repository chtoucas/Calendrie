﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data;

using Calendrie.Core;

// For unbounded datasets, do not copy the static props from the calendrical
// dataset, it's unnecessary as one can always call them directly.
// We use a SchemaDataSet, not an ICalendricalDataSet, to construct an
// UnboundedCalendarDataSet: this way, it's clear that we build the data
// directly from the schema data, which is not the case with ICalendricalDataSet
// because we could use an ICalendarDataSet (remember that ICalendarDataSet
// derives from ICalendricalDataSet).

/// <summary>
/// Defines test data for an <i>unbounded</i> calendar and provides a base for derived classes.
/// </summary>
/// <typeparam name="TCalendricalDataSet">The type that represents the calendrical dataset.</typeparam>
public abstract class UnboundedCalendarDataSet<TCalendricalDataSet> : UnboundedCalendarDataSet
    where TCalendricalDataSet : SchemaDataSet
{
    protected UnboundedCalendarDataSet(TCalendricalDataSet dataSet, DayNumber epoch)
        : base(dataSet, epoch)
    {
        Debug.Assert(dataSet != null);

        SchemaDataSet = dataSet;
    }

    /// <summary>
    /// Gets the calendrical dataset.
    /// </summary>
    protected TCalendricalDataSet SchemaDataSet { get; }
}

/// <summary>
/// Defines test data for a <i>unbounded</i> calendar and provides a base for derived classes.
/// </summary>
public abstract class UnboundedCalendarDataSet : ICalendarDataSet
{
    /// <summary>
    /// Represents the schema dataset.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly SchemaDataSet _dataSet;

    protected UnboundedCalendarDataSet(SchemaDataSet dataSet, DayNumber epoch)
    {
        ArgumentNullException.ThrowIfNull(dataSet);

        _dataSet = dataSet;
        Epoch = epoch;
    }

    public DayNumber Epoch { get; }

    public abstract DataGroup<DayNumberInfo> DayNumberInfoData { get; }

    public DataGroup<YearDayNumber> StartOfYearDayNumberData =>
        MapToDayNumberData(_dataSet.StartOfYearDaysSinceEpochData);

    public DataGroup<YearDayNumber> EndOfYearDayNumberData =>
        MapToDayNumberData(_dataSet.EndOfYearDaysSinceEpochData);

    //
    // Affine data
    //

    public int SampleCommonYear => _dataSet.SampleCommonYear;
    public int SampleLeapYear => _dataSet.SampleLeapYear;

    public DataGroup<MonthsSinceEpochInfo> MonthsSinceEpochInfoData => _dataSet.MonthsSinceEpochInfoData;
    public DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData => _dataSet.DaysSinceEpochInfoData;

    public DataGroup<DateInfo> DateInfoData => _dataSet.DateInfoData;
    public DataGroup<MonthInfo> MonthInfoData => _dataSet.MonthInfoData;
    public DataGroup<YearInfo> YearInfoData => _dataSet.YearInfoData;
    public DataGroup<CenturyInfo> CenturyInfoData => _dataSet.CenturyInfoData;

    public DataGroup<YemodaAnd<int>> DaysInYearAfterDateData => _dataSet.DaysInYearAfterDateData;
    public DataGroup<YemodaAnd<int>> DaysInMonthAfterDateData => _dataSet.DaysInMonthAfterDateData;

    public DataGroup<Yemoda> StartOfYearPartsData => _dataSet.StartOfYearPartsData;
    public DataGroup<Yemoda> EndOfYearPartsData => _dataSet.EndOfYearPartsData;

    public DataGroup<YearMonthsSinceEpoch> StartOfYearMonthsSinceEpochData => _dataSet.StartOfYearMonthsSinceEpochData;
    public DataGroup<YearMonthsSinceEpoch> EndOfYearMonthsSinceEpochData => _dataSet.EndOfYearMonthsSinceEpochData;

    public DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData => _dataSet.StartOfYearDaysSinceEpochData;
    public DataGroup<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData => _dataSet.EndOfYearDaysSinceEpochData;

    public TheoryData<int, int> InvalidMonthFieldData => _dataSet.InvalidMonthFieldData;
    public TheoryData<int, int, int> InvalidDayFieldData => _dataSet.InvalidDayFieldData;
    public TheoryData<int, int> InvalidDayOfYearFieldData => _dataSet.InvalidDayOfYearFieldData;

    public DataGroup<YemodaPair> ConsecutiveDaysData => _dataSet.ConsecutiveDaysData;
    public DataGroup<YedoyPair> ConsecutiveDaysOrdinalData => _dataSet.ConsecutiveDaysOrdinalData;
    public DataGroup<YemoPair> ConsecutiveMonthsData => _dataSet.ConsecutiveMonthsData;

    public DataGroup<YemodaPairAnd<int>> AddDaysData => _dataSet.AddDaysData;
    public DataGroup<YedoyPairAnd<int>> AddDaysOrdinalData => _dataSet.AddDaysOrdinalData;
    public DataGroup<YemodaPairAnd<int>> AddMonthsData => _dataSet.AddMonthsData;
    public DataGroup<YemodaPairAnd<int>> AddYearsData => _dataSet.AddYearsData;
    public DataGroup<YedoyPairAnd<int>> AddYearsOrdinalData => _dataSet.AddYearsOrdinalData;
    public DataGroup<YemoPairAnd<int>> AddMonthsMonthData => _dataSet.AddMonthsMonthData;
    public DataGroup<YemoPairAnd<int>> AddYearsMonthData => _dataSet.AddYearsMonthData;

    public DataGroup<YemodaPairAnd<int>> CountMonthsBetweenData => _dataSet.CountMonthsBetweenData;
    public DataGroup<YemodaPairAnd<int>> CountYearsBetweenData => _dataSet.CountYearsBetweenData;
    public DataGroup<YedoyPairAnd<int>> CountYearsBetweenOrdinalData => _dataSet.CountYearsBetweenOrdinalData;
    public DataGroup<YemoPairAnd<int>> CountYearsBetweenMonthData => _dataSet.CountYearsBetweenMonthData;

    //
    // Helpers
    //

    [Pure]
    private DataGroup<YearDayNumber> MapToDayNumberData(DataGroup<YearDaysSinceEpoch> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var epoch = Epoch;
        return source.SelectT(selector);

        YearDayNumber selector(YearDaysSinceEpoch x) => x.ToYearDayNumber(epoch);
    }
}
