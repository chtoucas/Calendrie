// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

/// <summary>
/// Represents the Coptic schema.
/// <para>This class cannot be inherited.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.</para>
/// </summary>
public sealed partial class Coptic12Schema :
    CopticSchema,
    IEpagomenalDayFeaturette,
    IDaysInMonthDistribution,
    ISchemaActivator<Coptic12Schema>
{
    /// <summary>
    /// Represents the number of months in a year.
    /// <para>This field is a constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = 12;

    /// <summary>
    /// Initializes a new instance of the <see cref="Coptic12Schema"/> class.
    /// </summary>
    internal Coptic12Schema() : base(30) { }

    /// <summary>
    /// Gets the number of days in each month of a common year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    internal static ReadOnlySpan<byte> DaysInMonth =>
        [30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 35];

    /// <summary>
    /// Gets the number of days in each month of a leap year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    internal static ReadOnlySpan<byte> DaysInMonthLeapYear =>
        [30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 36];

    /// <inheritdoc />
    [Pure]
    static ReadOnlySpan<byte> IDaysInMonthDistribution.GetDaysInMonthDistribution(bool leap) =>
        leap ? DaysInMonthLeapYear : DaysInMonth;

    /// <inheritdoc />
    [Pure]
    static Coptic12Schema ISchemaActivator<Coptic12Schema>.CreateInstance() => new();

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsRegular(out int monthsInYear)
    {
        monthsInYear = MonthsInYear;
        return true;
    }
}

public partial class Coptic12Schema // Year, month or day infos
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenalDay(int y, int m, int d, out int epagomenalNumber) =>
        Twelve.IsEpagomenalDay(d, out epagomenalNumber);

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryDay(int y, int m, int d) => Twelve.IsIntercalaryDay(d);

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsSupplementaryDay(int y, int m, int d) => Twelve.IsSupplementaryDay(d);
}

public partial class Coptic12Schema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsInYear(int y) => MonthsInYear;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int y, int m) =>
        Twelve.CountDaysInMonth(IsLeapYear(y), m);
}

public partial class Coptic12Schema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsSinceEpoch(int y, int m) =>
        MonthsCalculator.Regular12.CountMonthsSinceEpoch(y, m);

    /// <inheritdoc />
    public sealed override void GetMonthParts(int monthsSinceEpoch, out int y, out int m) =>
        MonthsCalculator.Regular12.GetMonthParts(monthsSinceEpoch, out y, out m);

    /// <inheritdoc />
    public sealed override void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        y = GetYear(daysSinceEpoch, out int doy);
        m = Twelve.GetMonth(doy - 1, out d);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d) =>
        Twelve.GetMonth(doy - 1, out d);
}

public partial class Coptic12Schema // Counting months and days since the epoch
{
    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYearInMonths(int y) =>
        MonthsCalculator.Regular12.GetStartOfYear(y);

    /// <inheritdoc />
    [Pure]
    public sealed override int GetEndOfYearInMonths(int y) =>
        MonthsCalculator.Regular12.GetEndOfYear(y);
}

public partial class Coptic12Schema // Dates in a given year or month
{
    /// <inheritdoc />
    public sealed override void GetDatePartsAtEndOfYear(int y, out int m, out int d) =>
        Twelve.GetEndOfYearParts(IsLeapYear(y), out m, out d);
}
