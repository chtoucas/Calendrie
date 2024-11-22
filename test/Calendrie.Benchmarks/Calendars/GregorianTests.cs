// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Benchmarks.Calendars;

using NodaTime;

using Calendrie;
using Calendrie.Specialized;

public class GregorianTests : GJBenchmarkTests
{
    private const int D7 = 7;        // No change of month.
    private const int D30 = 30;      // Change of month.
    private const int D401 = 401;    // "Slow-track".

    public GregorianTests() { Option = BenchmarkOption.Fixed; }

    #region Calendrie date types

    [Benchmark(Description = "DayNumber")]
    public void WithDayNumber()
    {
        DayNumber start = DayNumber.FromGregorianParts(Year, Month, Day);
        DayNumber end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);
        var parts = end.GetGregorianParts();
        var oparts = end.GetGregorianOrdinalParts();

        var (y, m, d) = parts;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = oparts.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "CivilDate", Baseline = true)]
    public void WithCivilDate()
    {
        CivilDate start = new(Year, Month, Day);
        CivilDate end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "GregorianDate")]
    public void WithGregorianDate()
    {
        GregorianDate start = new(Year, Month, Day);
        GregorianDate end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    #endregion
    #region External date types

    [Benchmark(Description = "LocalDate")]
    public void WithLocalDate()
    {
        LocalDate start = new(Year, Month, Day);
        LocalDate end = start.PlusDays(1).PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        IsoDayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "DateOnly")]
    public void WithDateOnly()
    {
        DateOnly start = new(Year, Month, Day);
        DateOnly end = start.AddDays(1).AddDays(D7).AddDays(D30).AddDays(D401);

        int y = end.Year;
        int m = end.Month;
        int d = end.Day;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "DateTime")]
    public void WithDateTime()
    {
        DateTime start = new(Year, Month, Day);
        DateTime end = start.AddDays(1).AddDays(D7).AddDays(D30).AddDays(D401);

        int y = end.Year;
        int m = end.Month;
        int d = end.Day;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    #endregion
}
