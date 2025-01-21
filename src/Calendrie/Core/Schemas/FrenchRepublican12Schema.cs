// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

/// <summary>
/// Represents the French Republican schema; see also <seealso cref="FrenchRepublican13Schema"/>.
/// <para>This class cannot be inherited.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.
/// </para>
/// </summary>
public sealed partial class FrenchRepublican12Schema :
    FrenchRepublicanSchema,
    IDaysInMonths,
    ISchemaActivator<FrenchRepublican12Schema>
{
    /// <summary>
    /// Represents the number of months in a year.
    /// <para>This field is a constant equal to 12.</para>
    /// </summary>
    public const int MonthsPerYear = 12;

    /// <summary>
    /// Initializes a new instance of the <see cref="FrenchRepublican12Schema"/>
    /// class.
    /// </summary>
    internal FrenchRepublican12Schema() : base(30) { }

    /// <inheritdoc />
    public sealed override int MonthsInYear => MonthsPerYear;

    /// <summary>
    /// Gets the number of days in each month of a common year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    private static ReadOnlySpan<byte> DaysInMonthsOfCommonYear =>
        [30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 35];

    /// <summary>
    /// Gets the number of days in each month of a leap year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    private static ReadOnlySpan<byte> DaysInMonthsOfLeapYear =>
        [30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 36];

    /// <inheritdoc />
    [Pure]
    static ReadOnlySpan<byte> IDaysInMonths.GetDaysInMonthsOfYear(bool leapYear) =>
        leapYear ? DaysInMonthsOfLeapYear : DaysInMonthsOfCommonYear;

    /// <inheritdoc />
    [Pure]
    static FrenchRepublican12Schema ISchemaActivator<FrenchRepublican12Schema>.CreateInstance() => new();
}

public partial class FrenchRepublican12Schema // Year, month or day infos
{
    /// <summary>
    /// Determines whether the specified date is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    internal static bool IsEpagomenalDayImpl(int d, out int epagomenalNumber) =>
        Twelve.IsEpagomenalDay(d, out epagomenalNumber);

    /// <summary>
    /// Determines whether the specified date is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "A date has 3 components")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Static would force us to validate the parameters")]
    public bool IsEpagomenalDay(int y, int m, int d, out int epagomenalNumber) =>
        IsEpagomenalDayImpl(d, out epagomenalNumber);

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryDay(int y, int m, int d) => Twelve.IsIntercalaryDay(d);

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsSupplementaryDay(int y, int m, int d) => Twelve.IsSupplementaryDay(d);
}

public partial class FrenchRepublican12Schema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int y, int m) =>
        Twelve.CountDaysInMonth(IsLeapYear(y), m);
}

public partial class FrenchRepublican12Schema // Conversions
{
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
