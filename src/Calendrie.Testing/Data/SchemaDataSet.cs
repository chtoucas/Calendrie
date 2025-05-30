﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data;

using Calendrie.Core;

// TODO(fact): DaysSinceEpochInfoData should include data before the epoch, then
// we should review the test bundles to ensure that we use it to test negative years.

/// <summary>
/// Defines test data for a schema and provides a base for derived classes.
/// </summary>
public abstract partial class SchemaDataSet : ICalendricalDataSet
{
    // ICalendar, not ICalendricalSchema, to prevent us from using any
    // fancy method. We keep ICalendricalSchema in the ctor to ensure that we
    // construct an CalendricalDataSet from a schema not a calendar.
    private readonly ICalendricalCore _schema;

    protected SchemaDataSet(ICalendricalSchema schema, int commonYear, int leapYear)
    {
        ArgumentNullException.ThrowIfNull(schema);

        _schema = schema;

        SampleCommonYear = commonYear;
        SampleLeapYear = leapYear;
    }

    // Both years should be > 0. The reason is that we're going to filter the
    // data to build the calendar datasets, and most of them will filter out
    // negative years.
    // To test negative values, we will use DaysSinceEpochInfoData, so it's
    // important to include data before the epoch.
    public int SampleCommonYear { get; }
    public int SampleLeapYear { get; }

    public abstract DataGroup<MonthsSinceEpochInfo> MonthsSinceEpochInfoData { get; }
    public abstract DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData { get; }

    public abstract DataGroup<DateInfo> DateInfoData { get; }
    public abstract DataGroup<MonthInfo> MonthInfoData { get; }
    public abstract DataGroup<YearInfo> YearInfoData { get; }
    /// <inheritdoc/>
    /// <remarks>Override this property if the schema does not support all years
    /// in the default list.</remarks>
    public virtual DataGroup<CenturyInfo> CenturyInfoData => YearNumberingDataSet.CenturyInfoData;

    public virtual DataGroup<YemodaAnd<int>> DaysInYearAfterDateData => MapToDaysInYearAfterDateData(DateInfoData);
    public virtual DataGroup<YemodaAnd<int>> DaysInMonthAfterDateData => MapToDaysInMonthAfterDateData(DateInfoData);

    public virtual DataGroup<Yemoda> StartOfYearPartsData => MapToStartOfYearParts(EndOfYearPartsData);
    public abstract DataGroup<Yemoda> EndOfYearPartsData { get; }

    public abstract DataGroup<YearMonthsSinceEpoch> StartOfYearMonthsSinceEpochData { get; }
    public virtual DataGroup<YearMonthsSinceEpoch> EndOfYearMonthsSinceEpochData =>
        MapToEndOfYearMonthsSinceEpochData(StartOfYearMonthsSinceEpochData);

    public abstract DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; }
    public virtual DataGroup<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData =>
        MapToEndOfYearDaysSinceEpochData(StartOfYearDaysSinceEpochData);

    public abstract TheoryData<int, int> InvalidMonthFieldData { get; }
    public abstract TheoryData<int, int, int> InvalidDayFieldData { get; }
    public abstract TheoryData<int, int> InvalidDayOfYearFieldData { get; }

    // TODO(fact): derived classes should at least override
    // - ConsecutiveDaysData
    // - ConsecutiveDaysOrdinalData
    // - ConsecutiveMonthsData
    // The Gregorian schema should override all virtual props.
    // Other schemas, see CalendarMathTestSuite.
    // - Gregorian (Profile = Solar12)
    // - Positivist (Profile = Solar13)
    // - Lunisolar
    // Optionally
    // - Coptic13 (Profile = Other)
    // - TabularIslamic (Profile = Lunar)

#pragma warning disable IDE0306 // Simplify collection initialization
    public virtual DataGroup<YemodaPair> ConsecutiveDaysData => new(s_ConsecutiveDaysSamples);
    public virtual DataGroup<YedoyPair> ConsecutiveDaysOrdinalData => new(s_ConsecutiveDaysOrdinalSamples);
    public virtual DataGroup<YemoPair> ConsecutiveMonthsData => new(s_ConsecutivesMonthsSamples);

    public virtual DataGroup<YemodaPairAnd<int>> AddDaysData => new(s_AddDaysSamples);
    public virtual DataGroup<YemodaPairAnd<int>> AddMonthsData => new(s_AddMonthsSamples);
    public virtual DataGroup<YemodaPairAnd<int>> AddYearsData => new(s_AddYearsSamples);
    public virtual DataGroup<YedoyPairAnd<int>> AddDaysOrdinalData => new(s_AddDaysOrdinalSamples);
    public virtual DataGroup<YedoyPairAnd<int>> AddYearsOrdinalData => new(s_AddYearsOrdinalSamples);
    public virtual DataGroup<YemoPairAnd<int>> AddMonthsMonthData => new(s_AddMonthsMonthSamples);
    public virtual DataGroup<YemoPairAnd<int>> AddYearsMonthData => new(s_AddYearsMonthSamples);
#pragma warning restore IDE0306

    // Include AddYears(Ordinal)Data, etc.
    // NB: if a derived class overrides AddYearsData, CountYearsBetweenData uses it.
    public virtual DataGroup<YemodaPairAnd<int>> CountMonthsBetweenData =>
        DataGroup.Create(s_CountMonthsBetweenSamples)
        .ConcatT(AddMonthsData);
    public virtual DataGroup<YemodaPairAnd<int>> CountYearsBetweenData =>
        DataGroup.Create(s_CountYearsBetweenSamples)
        .ConcatT(AddYearsData);
    public virtual DataGroup<YedoyPairAnd<int>> CountYearsBetweenOrdinalData =>
        DataGroup.Create(s_CountYearsBetweenOrdinalSamples)
        .ConcatT(AddYearsOrdinalData);
    public virtual DataGroup<YemoPairAnd<int>> CountYearsBetweenMonthData =>
        DataGroup.Create(s_CountYearsBetweenMonthSamples)
        .ConcatT(AddYearsMonthData);
}

public partial class SchemaDataSet // Helpers
{
    // We could have removed the parameter "source" from MapToStartOfYearParts()
    // and use the property EndOfYearPartsData instead, but this is not such a
    // good idea. Indeed, I prefer to make it clear that, for the method to
    // work properly, "source" must not be null.
    // This remark applies to the other helpers.

    [Pure]
    protected static DataGroup<MonthsSinceEpochInfo> GenMonthsSinceEpochInfoData(int monthsInYear)
    {
        var seq = new List<MonthsSinceEpochInfo>();
        for (int y = -10; y <= 10; y++)
        {
            for (int m = 1; m <= monthsInYear; m++)
            {
                seq.Add(new MonthsSinceEpochInfo((y - 1) * monthsInYear + (m - 1), y, m));
            }
        }
        return DataGroup.Create(seq);
    }

    [Pure]
    protected static DataGroup<YearMonthsSinceEpoch> GenStartOfYearMonthsSinceEpochData(int monthsInYear)
    {
        var seq = new List<YearMonthsSinceEpoch>();
        for (int y = -10; y <= 10; y++)
        {
            seq.Add(new YearMonthsSinceEpoch(y, (y - 1) * monthsInYear));
        }
        return DataGroup.Create(seq);
    }

    [Pure]
    private static DataGroup<Yemoda> MapToStartOfYearParts(DataGroup<Yemoda> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source.SelectT(selector);

        static Yemoda selector(Yemoda x) => new(x.Year, 1, 1);
    }

    [Pure]
    private static DataGroup<YearMonthsSinceEpoch> MapToEndOfYearMonthsSinceEpochData(DataGroup<YearMonthsSinceEpoch> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source.SelectT(selector);

        static YearMonthsSinceEpoch selector(YearMonthsSinceEpoch x)
        {
            var (y, monthsSinceEpoch) = x;
            return new YearMonthsSinceEpoch(y - 1, monthsSinceEpoch - 1);
        }
    }

    [Pure]
    private static DataGroup<YearDaysSinceEpoch> MapToEndOfYearDaysSinceEpochData(DataGroup<YearDaysSinceEpoch> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source.SelectT(selector);

        static YearDaysSinceEpoch selector(YearDaysSinceEpoch x)
        {
            var (y, daysSinceEpoch) = x;
            return new YearDaysSinceEpoch(y - 1, daysSinceEpoch - 1);
        }
    }

    [Pure]
    private DataGroup<YemodaAnd<int>> MapToDaysInYearAfterDateData(DataGroup<DateInfo> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var sch = _schema;
        return source.SelectT(selector);

        YemodaAnd<int> selector(DateInfo x)
        {
            var (y, m, d, doy) = x;
            // Assumption: CountDaysInYear() is correct.
            int daysInYearAfter = sch.CountDaysInYear(y) - doy;
            return new YemodaAnd<int>(y, m, d, daysInYearAfter);
        }
    }

    [Pure]
    private DataGroup<YemodaAnd<int>> MapToDaysInMonthAfterDateData(DataGroup<DateInfo> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var sch = _schema;
        return source.SelectT(selector);

        YemodaAnd<int> selector(DateInfo x)
        {
            var (y, m, d) = x.Yemoda;
            // Assumption: CountDaysInMonth() is correct.
            int daysInMonthAfter = sch.CountDaysInMonth(y, m) - d;
            return new YemodaAnd<int>(y, m, d, daysInMonthAfter);
        }
    }
}

public partial class SchemaDataSet // Math helpers
{
    // NB: the data is unambiguous.
    private static readonly IEnumerable<YemodaPair> s_ConsecutiveDaysSamples = InitConsecutiveDaysSamples();
    private static readonly IEnumerable<YedoyPair> s_ConsecutiveDaysOrdinalSamples = InitConsecutiveDaysOrdinalSamples();
    private static readonly IEnumerable<YemoPair> s_ConsecutivesMonthsSamples = InitConsecutivesMonthsSamples();

    private static readonly IEnumerable<YemodaPairAnd<int>> s_AddDaysSamples = InitAddDaysSamples();
    private static readonly IEnumerable<YemodaPairAnd<int>> s_AddMonthsSamples = InitAddMonthsSamples();
    private static readonly IEnumerable<YemodaPairAnd<int>> s_AddYearsSamples = InitAddYearsSamples();
    private static readonly IEnumerable<YedoyPairAnd<int>> s_AddDaysOrdinalSamples = InitAddDaysOrdinalSamples();
    private static readonly IEnumerable<YedoyPairAnd<int>> s_AddYearsOrdinalSamples = InitAddYearsOrdinalSamples();
    private static readonly IEnumerable<YemoPairAnd<int>> s_AddMonthsMonthSamples = InitAddMonthsMonthSamples();
    private static readonly IEnumerable<YemoPairAnd<int>> s_AddYearsMonthSamples = InitAddYearsMonthSamples();

    private static readonly IEnumerable<YemodaPairAnd<int>> s_CountMonthsBetweenSamples = InitCountMonthsBetweenSamples();
    private static readonly IEnumerable<YemodaPairAnd<int>> s_CountYearsBetweenSamples = InitCountYearsBetweenSamples();
    private static readonly IEnumerable<YedoyPairAnd<int>> s_CountYearsBetweenOrdinalSamples = InitCountYearsBetweenOrdinalSamples();
    private static readonly IEnumerable<YemoPairAnd<int>> s_CountYearsBetweenMonthSamples = InitCountYearsBetweenMonthSamples();

    //
    // Data for Next() and Previous()
    //

    private static List<YemodaPair> InitConsecutiveDaysSamples()
    {
        // Hypothesis: April is at least 28-days long.
        // new(new(3, 4, 1), new(3, 4, 2))
        // new(new(3, 4, 2), new(3, 4, 3))
        // ...
        // new(new(3, 4, 27), new(3, 4, 28))
        const int
            SampleSize = 27,
            Year = 3,
            Month = 4;

        var data = new List<YemodaPair>();
        for (int d = 1; d <= SampleSize; d++)
        {
            var date = new Yemoda(Year, Month, d);
            var dateAfter = new Yemoda(Year, Month, d + 1);
            data.Add(new YemodaPair(date, dateAfter));
        }
        return data;
    }

    private static List<YedoyPair> InitConsecutiveDaysOrdinalSamples()
    {
        // Samples should covers at least the two first months.
        // new(new(3, 1), new(3, 2))
        // new(new(3, 2), new(3, 3))
        // ...
        // new(new(3, 69), new(3, 70))
        // new(new(3, 70), new(3, 71))
        const int
            SampleSize = 70,
            Year = 3;

        var data = new List<YedoyPair>();
        for (int doy = 1; doy <= SampleSize; doy++)
        {
            var date = new Yedoy(Year, doy);
            var dateAfter = new Yedoy(Year, doy + 1);
            data.Add(new YedoyPair(date, dateAfter));
        }
        return data;
    }

    private static List<YemoPair> InitConsecutivesMonthsSamples()
    {
        // Hypothesis: a year is at least 12-months long.
        // new(new(3, 1), new(3, 2))
        // new(new(3, 2), new(3, 3))
        // ...
        // new(new(3, 11), new(3, 12))
        const int
            SampleSize = 11,
            Year = 3;

        var data = new List<YemoPair>();
        for (int m = 1; m <= SampleSize; m++)
        {
            var month = new Yemo(Year, m);
            var monthAfter = new Yemo(Year, m + 1);
            data.Add(new YemoPair(month, monthAfter));
        }
        return data;
    }

    //
    // Data for the additions
    //

    private static List<YemodaPairAnd<int>> InitAddDaysSamples()
    {
        // Hypothesis: April is at least 28-days long.
        // new(new(3, 4, 14), new(3, 4, 1), -13)
        // new(new(3, 4, 14), new(3, 4, 2), -12)
        // ...
        // new(new(3, 4, 14), new(3, 4, 27), 13)
        // new(new(3, 4, 14), new(3, 4, 28), 14)
        const int
            SampleSize = 28,
            Year = 3,
            Month = 4,
            Day = SampleSize / 2;

        var start = new Yemoda(Year, Month, Day);

        var data = new List<YemodaPairAnd<int>>();
        for (int days = -Day + 1; days <= Day; days++)
        {
            var end = new Yemoda(Year, Month, Day + days);
            data.Add(new YemodaPairAnd<int>(start, end, days));
        }
        return data;
    }

    private static List<YemodaPairAnd<int>> InitAddMonthsSamples()
    {
        // Hypothesis: a year is at least 12-months long.
        // new(new(3, 6, 5), new(3, 1, 5), -5)
        // new(new(3, 6, 5), new(3, 2, 5), -4)
        // ...
        // new(new(3, 6, 5), new(3, 11, 5), 5)
        // new(new(3, 6, 5), new(3, 12, 5), 6)
        const int
            SampleSize = 12,
            Year = 3,
            Month = SampleSize / 2,
            Day = 5;

        var start = new Yemoda(Year, Month, Day);

        var data = new List<YemodaPairAnd<int>>();
        for (int months = -Month + 1; months <= Month; months++)
        {
            var end = new Yemoda(Year, Month + months, Day);
            data.Add(new YemodaPairAnd<int>(start, end, months));
        }
        return data;
    }

    private static List<YemodaPairAnd<int>> InitAddYearsSamples()
    {
        // new(new(5, 4, 5), new(1, 4, 5), -4)
        // new(new(5, 4, 5), new(2, 4, 5), -3)
        // ...
        // new(new(5, 4, 5), new(9, 4, 5), 4)
        // new(new(5, 4, 5), new(10, 4, 5), 5)
        const int
            SampleSize = 10,
            Year = SampleSize / 2,
            Month = 4,
            Day = 5;

        var start = new Yemoda(Year, Month, Day);

        var data = new List<YemodaPairAnd<int>>();
        for (int years = -Year + 1; years <= Year; years++)
        {
            var end = new Yemoda(Year + years, Month, Day);
            data.Add(new YemodaPairAnd<int>(start, end, years));
        }
        return data;
    }

    private static List<YedoyPairAnd<int>> InitAddDaysOrdinalSamples()
    {
        // Samples should cover the two first months.
        // new(new(3, 35), new(3, 1), -34)
        // new(new(3, 35), new(3, 2), -33)
        // ...
        // new(new(3, 35), new(3, 69), 34)
        // new(new(3, 35), new(3, 70), 35)
        const int
            SampleSize = 70,
            Year = 3,
            DayOfYear = SampleSize / 2;

        var start = new Yedoy(Year, DayOfYear);

        var data = new List<YedoyPairAnd<int>>();
        for (int days = -DayOfYear + 1; days <= DayOfYear; days++)
        {
            var end = new Yedoy(Year, DayOfYear + days);
            data.Add(new YedoyPairAnd<int>(start, end, days));
        }
        return data;
    }

    private static List<YedoyPairAnd<int>> InitAddYearsOrdinalSamples()
    {
        // new(new(5, 35), new(1, 35), -4)
        // new(new(5, 35), new(2, 35), -3)
        // ...
        // new(new(5, 35), new(9, 35), 4)
        // new(new(5, 35), new(10, 35), 5)
        const int
            SampleSize = 10,
            Year = SampleSize / 2,
            DayOfYear = 35;

        var start = new Yedoy(Year, DayOfYear);

        var data = new List<YedoyPairAnd<int>>();
        for (int years = -Year + 1; years <= Year; years++)
        {
            var end = new Yedoy(Year + years, DayOfYear);
            data.Add(new YedoyPairAnd<int>(start, end, years));
        }
        return data;
    }

    private static List<YemoPairAnd<int>> InitAddMonthsMonthSamples()
    {
        // Hypothesis: a year is at least 12-months long.
        // new(new(3, 6), new(3, 1), -5)
        // new(new(3, 6), new(3, 2), -4)
        // ...
        // new(new(3, 6), new(3, 11), 5)
        // new(new(3, 6), new(3, 12), 6)
        const int
            SampleSize = 12,
            Year = 3,
            Month = SampleSize / 2;

        var start = new Yemo(Year, Month);

        var data = new List<YemoPairAnd<int>>();
        for (int months = -Month + 1; months <= Month; months++)
        {
            var end = new Yemo(Year, Month + months);
            data.Add(new YemoPairAnd<int>(start, end, months));
        }
        return data;
    }

    private static List<YemoPairAnd<int>> InitAddYearsMonthSamples()
    {
        // new(new(5, 4), new(1, 4), -4)
        // new(new(5, 4), new(2, 4), -3)
        // ...
        // new(new(5, 4), new(9, 4), 4)
        // new(new(5, 4), new(10, 4), 5)
        const int
            SampleSize = 10,
            Year = SampleSize / 2,
            Month = 4;

        var start = new Yemo(Year, Month);

        var data = new List<YemoPairAnd<int>>();
        for (int months = -Year + 1; months <= Year; months++)
        {
            var end = new Yemo(Year + months, Month);
            data.Add(new YemoPairAnd<int>(start, end, months));
        }
        return data;
    }

    //
    // Data for the subtractions
    //

    private static List<YemodaPairAnd<int>> InitCountMonthsBetweenSamples()
    {
        // Hypothesis: a year is at least 12-months long.
        // new(new(3, 6, 5), new(3, 1, 5), -5)
        // new(new(3, 6, 5), new(3, 2, 5), -4)
        // ...
        // new(new(3, 6, 5), new(3, 11, 5), 5)
        // new(new(3, 6, 5), new(3, 12, 5), 6)
        const int
            SampleSize = 12,
            Year = 3,
            Month = SampleSize / 2,
            Day = 5;

        var start = new Yemoda(Year, Month, Day);

        var data = new List<YemodaPairAnd<int>>();
        for (int months = -Month + 1; months <= Month; months++)
        {
            var end = new Yemoda(Year, Month + months, Day);
            data.Add(new YemodaPairAnd<int>(start, end, months));
        }
        return data;
    }

    private static List<YemodaPairAnd<int>> InitCountYearsBetweenSamples()
    {
        // Hypothesis: a year is at least 12-months long.
        // new(new(3, 6, 5), new(3, 1, 5), 0)
        // new(new(3, 6, 5), new(3, 2, 5), 0)
        // ...
        // new(new(3, 6, 5), new(3, 11, 5), 0)
        // new(new(3, 6, 5), new(3, 12, 5), 0)
        const int
            SampleSize = 12,
            Year = 3,
            Month = SampleSize / 2,
            Day = 5;

        var start = new Yemoda(Year, Month, Day);

        var data = new List<YemodaPairAnd<int>>();
        for (int months = -Month + 1; months <= Month; months++)
        {
            var end = new Yemoda(Year, Month + months, Day);
            data.Add(new YemodaPairAnd<int>(start, end, 0));
        }
        return data;
    }

    private static List<YedoyPairAnd<int>> InitCountYearsBetweenOrdinalSamples()
    {
        // Samples should covers at least the two first months.
        // new(new(3, 35), new(3, 1), 0)
        // new(new(3, 35), new(3, 2), 0)
        // ...
        // new(new(3, 35), new(3, 70), 0)
        // new(new(3, 35), new(3, 71), 0)
        const int
            SampleSize = 70,
            Year = 3,
            DayOfYear = SampleSize / 2;

        var start = new Yedoy(Year, DayOfYear);

        var data = new List<YedoyPairAnd<int>>();
        for (int days = -DayOfYear + 1; days <= DayOfYear; days++)
        {
            var end = new Yedoy(Year, DayOfYear + days);
            data.Add(new YedoyPairAnd<int>(start, end, 0));
        }
        return data;
    }

    private static List<YemoPairAnd<int>> InitCountYearsBetweenMonthSamples()
    {
        // Hypothesis: a year is at least 12-months long.
        // new(new(3, 6), new(3, 1), 0)
        // new(new(3, 6), new(3, 2), 0)
        // ...
        // new(new(3, 6), new(3, 11), 0)
        // new(new(3, 6), new(3, 12), 0)
        const int
            SampleSize = 12,
            Year = 3,
            Month = SampleSize / 2;

        var start = new Yemo(Year, Month);

        var data = new List<YemoPairAnd<int>>();
        for (int months = -Month + 1; months <= Month; months++)
        {
            var end = new Yemo(Year, Month + months);
            data.Add(new YemoPairAnd<int>(start, end, 0));
        }
        return data;
    }
}
