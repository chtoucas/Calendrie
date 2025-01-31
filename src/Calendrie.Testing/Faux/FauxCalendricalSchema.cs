// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Faux;

using Calendrie.Core;
using Calendrie.Core.Intervals;

// GetStartOfYear() must be implemented for calendars, arithmetic & scope to work.
// We must also implement IsRegular(), otherwise CalendricalSchema.Profile will fail.
// CountMonthsInYear() (must be > 0) and CountMonthsSinceEpoch() must be implemented
// too for arithmetic.

using static Calendrie.Core.CalendricalConstants;

public partial class FauxCalendricalSchema : CalendricalSchema
{
    private const int DefaultMinDaysInYear = 365;
    private const int DefaultMinDaysInMonth = 28;

    public FauxCalendricalSchema() : base(DefaultMinDaysInYear, DefaultMinDaysInMonth) { }

    // Base constructor.
    protected FauxCalendricalSchema(Segment<int> supportedYears, int minDaysInYear, int minDaysInMonth)
        : base(supportedYears, minDaysInYear, minDaysInMonth) { }

    // Constructors to be able to test the base constructors; see also WithMinDaysInXXX().
    public FauxCalendricalSchema(Segment<int> supportedYears)
        : base(supportedYears, DefaultMinDaysInYear, DefaultMinDaysInMonth) { }
    private FauxCalendricalSchema(int minDaysInYear, int minDaysInMonth)
        : base(Yemoda.SupportedYears, minDaysInYear, minDaysInMonth) { }

    // Constructors to test the properties.
    public FauxCalendricalSchema(Segment<int> supportedYears, Segment<int> supportedYearsCore)
        : this(supportedYears) { SupportedYearsCore = supportedYearsCore; }

    // Pre-defined instances.
    public static FauxCalendricalSchema Regular12 => new FauxRegularSchema(12);
    public static FauxCalendricalSchema Regular13 => new FauxRegularSchema(13);
    public static FauxCalendricalSchema Regular14 => new FauxRegularSchema(14);

    // Constructor to be able to test the setter for PreValidator.
    [Pure]
    public static FauxCalendricalSchema
        WithPreValidator(Func<CalendricalSchema, ICalendricalPreValidator> preValidator) =>
        new FauxRegularSchema(preValidator);

    [Pure]
    public static FauxCalendricalSchema WithMinDaysInYear(int minDaysInYear) =>
        new(minDaysInYear, DefaultMinDaysInMonth);

    [Pure]
    public static FauxCalendricalSchema WithMinDaysInMonth(int minDaysInMonth) =>
        new(DefaultMinDaysInYear, minDaysInMonth);

    private sealed class FauxRegularSchema : FauxCalendricalSchema
    {
        public FauxRegularSchema(Func<CalendricalSchema, ICalendricalPreValidator> preValidator)
            : this(12)
        {
            ArgumentNullException.ThrowIfNull(preValidator);

            // NB: it will only works with Solar12PreValidator...
            PreValidator = preValidator.Invoke(this);
        }

        public FauxRegularSchema(int monthsInYear)
            : this(monthsInYear, DefaultMinDaysInYear, DefaultMinDaysInMonth) { }

        public FauxRegularSchema(int monthsInYear, int minDaysInYear, int minDaysInMonth)
            : base(minDaysInYear, minDaysInMonth)
        { MonthsInYear = monthsInYear; }

        public int MonthsInYear { get; }

        [Pure]
        public sealed override bool IsRegular(out int monthsInYear)
        {
            monthsInYear = MonthsInYear;
            return true;
        }

        [Pure] public override int CountMonthsInYear(int y) => MonthsInYear;
    }
}

public partial class FauxCalendricalSchema // Props & methods
{
    public sealed override CalendricalFamily Family => throw new NotSupportedException();
    public sealed override CalendricalAdjustments PeriodicAdjustments => throw new NotSupportedException();

    [Pure]
    public override bool IsRegular(out int monthsInYear)
    {
        monthsInYear = 0;
        return true;
    }

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
}

public partial class FauxCalendricalSchema // Profiles
{
    public static readonly TheoryData<FauxCalendricalSchema> NotLunar =
    [
        new FauxRegularSchema(Lunar.MonthsPerYear + 1, Lunar.MinDaysPerYear, Lunar.MinDaysPerMonth),
        new FauxRegularSchema(Lunar.MonthsPerYear, Lunar.MinDaysPerYear - 1, Lunar.MinDaysPerMonth),
        new FauxRegularSchema(Lunar.MonthsPerYear, Lunar.MinDaysPerYear, Lunar.MinDaysPerMonth - 1),
    ];

    public static readonly TheoryData<FauxCalendricalSchema> NotLunisolar =
    [
        new FauxCalendricalSchema(Lunisolar.MinDaysPerYear - 1, Lunisolar.MinDaysPerMonth),
        new FauxCalendricalSchema(Lunisolar.MinDaysPerYear, Lunisolar.MinDaysPerMonth - 1),
    ];

    public static readonly TheoryData<FauxCalendricalSchema> NotSolar12 =
    [
        new FauxRegularSchema(Solar12.MonthsPerYear + 1, Solar.MinDaysPerYear, Solar.MinDaysPerMonth),
        new FauxRegularSchema(Solar12.MonthsPerYear, Solar.MinDaysPerYear - 1, Solar.MinDaysPerMonth),
        new FauxRegularSchema(Solar12.MonthsPerYear, Solar.MinDaysPerYear, Solar.MinDaysPerMonth - 1),
    ];

    public static readonly TheoryData<FauxCalendricalSchema> NotSolar13 =
    [
        new FauxRegularSchema(Solar13.MonthsPerYear + 1, Solar.MinDaysPerYear, Solar.MinDaysPerMonth),
        new FauxRegularSchema(Solar13.MonthsPerYear, Solar.MinDaysPerYear - 1, Solar.MinDaysPerMonth),
        new FauxRegularSchema(Solar13.MonthsPerYear, Solar.MinDaysPerYear, Solar.MinDaysPerMonth - 1),
    ];

    public static CalendricalSchema WithBadProfile => new FauxSchemaWithBadProfile();

    private sealed class FauxSchemaWithBadProfile : FauxCalendricalSchema
    {
#pragma warning disable CA1822 // Mark members as static
        internal new CalendricalProfile Profile => (CalendricalProfile)5;
#pragma warning restore CA1822
    }
}
