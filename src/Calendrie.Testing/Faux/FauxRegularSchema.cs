// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Faux;

using Calendrie;
using Calendrie.Core;
using Calendrie.Core.Intervals;

public sealed class FauxRegularSchema : RegularSchema
{
    private readonly ICalendricalCore _kernel;

    public FauxRegularSchema(ICalendricalCore kernel, Range<int> supportedYears, int monthsInYear)
        : base(supportedYears, minDaysInYear: 1, minDaysInMonth: 1)
    {
        Debug.Assert(kernel != null);

        _kernel = kernel;
        MonthsInYear = monthsInYear;
    }

    public static RegularSchema Create(ICalendricalCore kernel, bool proleptic = true)
    {
        ArgumentNullException.ThrowIfNull(kernel);

        var supportedYears = proleptic ? ProlepticSupportedYears : StandardSupportedYears;

        return kernel.IsRegular(out int monthsInYear)
            ? new FauxRegularSchema(kernel, supportedYears, monthsInYear)
            : throw new ArgumentException(null, nameof(kernel));
    }

    public static Range<int> StandardSupportedYears { get; } = Range.Create(1, 9999);
    public static Range<int> ProlepticSupportedYears { get; } = Range.Create(-9998, 9999);

    public sealed override int MonthsInYear { get; }

    public sealed override CalendricalFamily Family => _kernel.Family;

    public sealed override CalendricalAdjustments PeriodicAdjustments =>
        _kernel.PeriodicAdjustments;

    public sealed override bool IsLeapYear(int y) => _kernel.IsLeapYear(y);

    public sealed override bool IsIntercalaryDay(int y, int m, int d) =>
        _kernel.IsIntercalaryDay(y, m, d);

    public sealed override bool IsSupplementaryDay(int y, int m, int d) =>
        _kernel.IsSupplementaryDay(y, m, d);

    public sealed override int CountDaysInYear(int y) => _kernel.CountDaysInYear(y);

    public sealed override int CountDaysInMonth(int y, int m) => _kernel.CountDaysInMonth(y, m);
}
