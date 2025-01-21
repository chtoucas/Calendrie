// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Faux;

using Calendrie.Core;
using Calendrie.Core.Intervals;

public abstract class FauxLimitSchemaBase : LimitSchema
{
    protected FauxLimitSchemaBase(int minDaysInYear, int minDaysInMonth)
        : base(minDaysInYear, minDaysInMonth) { }
    protected FauxLimitSchemaBase(Range<int> supportedYears, int minDaysInYear, int minDaysInMonth)
        : base(supportedYears, minDaysInYear, minDaysInMonth) { }

    [Pure]
    public override bool IsRegular(out int monthsInYear)
    {
        monthsInYear = 0;
        return true;
    }
}

public partial class FauxLimitSchema : LimitSchema
{
    private const int DefaultMinDaysInYear = 365;
    private const int DefaultMinDaysInMonth = 28;

    public FauxLimitSchema()
        : base(DefaultMinDaysInYear, DefaultMinDaysInMonth) { }

    // Base constructors.
    public FauxLimitSchema(int minDaysInYear, int minDaysInMonth)
        : base(minDaysInYear, minDaysInMonth) { }
    public FauxLimitSchema(Range<int> supportedYears, int minDaysInYear, int minDaysInMonth)
        : base(supportedYears, minDaysInYear, minDaysInMonth) { }

    // Constructor in order to test the base constructors.
    public FauxLimitSchema(Range<int> supportedYears)
        : base(supportedYears, DefaultMinDaysInYear, DefaultMinDaysInMonth) { }

    // Constructors to test the properties.
    public FauxLimitSchema(CalendricalFamily family)
        : this() { Family = family; }
    public FauxLimitSchema(CalendricalAdjustments adjustments)
        : this() { PeriodicAdjustments = adjustments; }
    public FauxLimitSchema(Range<int> supportedYears, Range<int> supportedYearsCore)
        : this(supportedYears) { SupportedYearsCore = supportedYearsCore; }

    // Pre-defined instances.
    public static FauxLimitSchema Regular12 => new FauxRegularSchema(12);
    public static FauxLimitSchema Regular13 => new FauxRegularSchema(13);
    public static FauxLimitSchema Regular14 => new FauxRegularSchema(14);

    [Pure]
    public static FauxLimitSchema WithMinDaysInYear(int minDaysInYear) =>
        new(minDaysInYear, DefaultMinDaysInMonth);

    [Pure]
    public static FauxLimitSchema WithMinDaysInMonth(int minDaysInMonth) =>
        new(DefaultMinDaysInYear, minDaysInMonth);

    [Pure]
    public override bool IsRegular(out int monthsInYear)
    {
        monthsInYear = 0;
        return true;
    }

    private sealed class FauxRegularSchema : FauxLimitSchema
    {
        public FauxRegularSchema(int monthsInYear)
            : this(monthsInYear, DefaultMinDaysInYear, DefaultMinDaysInMonth) { }

        public FauxRegularSchema(int monthsInYear, int minDaysInYear, int minDaysInMonth)
            : base(minDaysInYear, minDaysInMonth)
        { MonthsInYear = monthsInYear; }

        public int MonthsInYear { get; }

        [Pure] public override int CountMonthsInYear(int y) => MonthsInYear;

        [Pure]
        public sealed override bool IsRegular(out int monthsInYear)
        {
            monthsInYear = MonthsInYear;
            return true;
        }
    }
}

public partial class FauxLimitSchema // Props & methods
{
    public sealed override CalendricalFamily Family { get; } = CalendricalFamily.Other;
    public sealed override CalendricalAdjustments PeriodicAdjustments { get; } = CalendricalAdjustments.None;

    [Pure] public sealed override bool IsLeapYear(int y) => throw new NotSupportedException();
    [Pure] public sealed override bool IsIntercalaryMonth(int y, int m) => throw new NotSupportedException();
    [Pure] public sealed override bool IsIntercalaryDay(int y, int m, int d) => throw new NotSupportedException();
    [Pure] public sealed override bool IsSupplementaryDay(int y, int m, int d) => throw new NotSupportedException();

    [Pure] public override int CountMonthsInYear(int y) => 1;
    [Pure] public sealed override int CountDaysInYear(int y) => MinDaysInYear;
    [Pure] public sealed override int CountDaysInYearBeforeMonth(int y, int m) => throw new NotSupportedException();
    [Pure] public sealed override int CountDaysInMonth(int y, int m) => MinDaysInMonth;

    public sealed override void GetMonthParts(int monthsSinceEpoch, out int y, out int m) => throw new NotSupportedException();
    [Pure] public sealed override int GetMonth(int y, int doy, out int d) => throw new NotSupportedException();
    [Pure] public sealed override int GetYear(int daysSinceEpoch) => throw new NotSupportedException();

    [Pure] public sealed override int GetStartOfYearInMonths(int y) => 0;
    [Pure] public sealed override int GetStartOfYear(int y) => 0;
    public sealed override void GetDatePartsAtEndOfYear(int y, out int m, out int d)
    {
        m = CountMonthsInYear(y);
        d = CountDaysInMonth(y, m);
    }
}
