// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.JulianDateTests;

public class DayOfYearTests : JulianDateComparisons
{
    [Benchmark(Description = "DayNumber")]
    public int WithDayNumber() => dayNumber.GetJulianOrdinalParts().DayOfYear;

    [Benchmark(Description = "JulianDate")]
    public int WithJulianDate() => julianDate.DayOfYear;

    [Benchmark(Description = "JulianDate_Plain")]
    public int WithPlainJulianDate() => plainJulianDate.DayOfYear;

    [Benchmark(Description = "LocalDate_NodaTime")]
    public int WithLocalDate() => localDate.DayOfYear;

    // TODO(code): date parts of a DateOnly in the Julian calendar.
    // See the other benchmarks too.
    //[Benchmark(Description = "DateOnly_BCL", Baseline = true)]
    //public int WithDateOnly() => BclJulianCalendar.GetDayOfYear(dateOnly.ToDateTime(TimeOnly.MinValue));

    [Benchmark(Description = "DateTime_BCL")]
    public int WithDateTime() => BclJulianCalendar.GetDayOfYear(dateTime);
}
