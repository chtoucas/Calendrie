﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data;

using Calendrie.Core.Intervals;
using Calendrie.Testing.Data.Schemas;

public static partial class CalCalDataSet { }

public partial class CalCalDataSet // Interconversion
{
    // Lazy initialization mainly to ensure that there isn't a circular
    // dependency between CalCalDataSet and Gregorian(Julian)DataSet during the
    // initialization of their static properties.
    private static DataGroup<YemodaPair>? s_GregorianToJulianData;
    public static DataGroup<YemodaPair> GregorianToJulianData =>
        s_GregorianToJulianData ??= InitGregorianToJulianData();

    [Pure]
    private static DataGroup<YemodaPair> InitGregorianToJulianData()
    {
        var lookup = GregorianDataSet.DaysSinceRataDieInfos.ToLookup(x => x.DaysSinceRataDie);

        var data = new DataGroup<YemodaPair>();
        foreach (var (rd, julian) in JulianDataSet.DaysSinceRataDieInfos)
        {
            var gs = lookup[rd].ToList();
            if (gs.Count != 1) { continue; }

            var (_, gregorian) = gs.Single();
            data.Add(new(gregorian, julian));
        }

        return data;
    }
}

public partial class CalCalDataSet // Day of the week
{
    [SuppressMessage("Naming", "CA1721:Property names should not match get methods")]
    public static TheoryData<DayNumber, DayOfWeek> DayNumberToDayOfWeekData { get; } =
        InitDayNumberToDayOfWeekData();

    [Pure]
    public static TheoryData<DayNumber, DayOfWeek> GetDayNumberToDayOfWeekData(Segment<DayNumber> domain)
    {
        var source = DaysSinceRataDieToDayOfWeek;
        var data = new TheoryData<DayNumber, DayOfWeek>();
        foreach (var (daysSinceRataDie, dayOfWeek) in source)
        {
            var dayNumber = DayZero.RataDie + daysSinceRataDie;
            if (!domain.Contains(dayNumber)) { continue; }
            data.Add(dayNumber, dayOfWeek);
        }
        return data;
    }

    [Pure]
    private static TheoryData<DayNumber, DayOfWeek> InitDayNumberToDayOfWeekData()
    {
        var source = DaysSinceRataDieToDayOfWeek;
        var data = new TheoryData<DayNumber, DayOfWeek>();
        foreach (var (daysSinceRataDie, dayOfWeek) in source)
        {
            data.Add(DayZero.RataDie + daysSinceRataDie, dayOfWeek);
        }
        return data;
    }

    private static IEnumerable<(int DaysSinceRataDie, DayOfWeek DayOfWeek)> DaysSinceRataDieToDayOfWeek
    {
        get
        {
            yield return (1, DayOfWeek.Monday);

            // D.&R. Annexe C.
            yield return (-214_193, DayOfWeek.Sunday);
            yield return (-61_387, DayOfWeek.Wednesday);
            yield return (25_469, DayOfWeek.Wednesday);
            yield return (49_217, DayOfWeek.Sunday);
            yield return (171_307, DayOfWeek.Wednesday);
            yield return (210_155, DayOfWeek.Monday);
            yield return (253_427, DayOfWeek.Saturday);
            yield return (369_740, DayOfWeek.Sunday);
            yield return (400_085, DayOfWeek.Sunday);
            yield return (434_355, DayOfWeek.Friday);
            yield return (452_605, DayOfWeek.Saturday);
            yield return (470_160, DayOfWeek.Friday);
            yield return (473_837, DayOfWeek.Sunday);
            yield return (507_850, DayOfWeek.Sunday);
            yield return (524_156, DayOfWeek.Wednesday);
            yield return (544_676, DayOfWeek.Saturday);
            yield return (567_118, DayOfWeek.Saturday);
            yield return (569_477, DayOfWeek.Saturday);
            yield return (601_716, DayOfWeek.Wednesday);
            yield return (613_424, DayOfWeek.Sunday);
            yield return (626_596, DayOfWeek.Friday);
            yield return (645_554, DayOfWeek.Sunday);
            yield return (664_224, DayOfWeek.Monday);
            yield return (671_401, DayOfWeek.Wednesday);
            yield return (694_799, DayOfWeek.Sunday);
            yield return (704_424, DayOfWeek.Sunday);
            yield return (708_842, DayOfWeek.Monday);
            yield return (709_409, DayOfWeek.Monday);
            yield return (709_580, DayOfWeek.Thursday);
            yield return (727_274, DayOfWeek.Tuesday);
            yield return (728_714, DayOfWeek.Sunday);
            yield return (744_313, DayOfWeek.Wednesday);
            yield return (764_652, DayOfWeek.Sunday);
        }
    }
}
