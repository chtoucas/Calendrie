// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Utilities;

/// <summary>
/// Represents the Egyptian schema.
/// <para>This class cannot be inherited.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.</para>
/// <para>A year is divided into 12 months of 30 days each, followed by 5
/// epagomenal days.</para>
/// <para>The epagomenal days are outside any month but, for technical reasons,
/// we attach them to a virtual thirteenth month: 1/13 to 5/13.</para>
/// </summary>
public sealed partial class Egyptian13Schema :
    EgyptianSchema,
    IDaysInMonths,
    ISchemaActivator<Egyptian13Schema>
{
    /// <summary>
    /// Represents the number of months in a year.
    /// <para>This field is a constant equal to 13.</para>
    /// </summary>
    public const int MonthsPerYear = 13;

    /// <summary>
    /// Represents the intercalary month number.
    /// <para>This field is a constant equal to 13.</para>
    /// </summary>
    public const int IntercalaryMonth = 13;

    /// <summary>
    /// Initializes a new instance of the <see cref="Egyptian13Schema"/> class.
    /// </summary>
    internal Egyptian13Schema() : base(minDaysInMonth: 5) { }

    /// <inheritdoc />
    public sealed override int MonthsInYear => MonthsPerYear;

    /// <summary>
    /// Gets the number of days in each month of a year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    internal static ReadOnlySpan<byte> DaysInMonth =>
        // No leap years.
        // Quarters: 90, 90, 90, 90 + 5.
        [30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 5];

    /// <inheritdoc />
    [Pure]
    static ReadOnlySpan<byte> IDaysInMonths.GetDaysInMonthsOfYear(bool leapYear) => DaysInMonth;

    /// <inheritdoc />
    [Pure]
    static Egyptian13Schema ISchemaActivator<Egyptian13Schema>.CreateInstance() => new();
}

public partial class Egyptian13Schema // Year, month or day infos
{
    /// <summary>
    /// Determines whether the specified date is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    internal static bool IsEpagomenalDayImpl(int m, int d, out int epagomenalNumber)
    {
        if (m == IntercalaryMonth)
        {
            epagomenalNumber = d;
            return true;
        }
        else
        {
            epagomenalNumber = 0;
            return false;
        }
    }

    /// <summary>
    /// Determines whether the specified date is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "A date has 3 components")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Static would force us to validate the parameters")]
    public bool IsEpagomenalDay(int y, int m, int d, out int epagomenalNumber) =>
        IsEpagomenalDayImpl(m, d, out epagomenalNumber);

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsSupplementaryDay(int y, int m, int d) => m == IntercalaryMonth;
}

public partial class Egyptian13Schema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int y, int m) =>
        m == IntercalaryMonth ? 5 : DaysPerMonth;
}

public partial class Egyptian13Schema // Conversions
{
    /// <inheritdoc />
    public sealed override void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        y = 1 + MathZ.Divide(daysSinceEpoch, DaysPerYear, out int d0y);
        m = GetMonth(d0y, out d);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d) =>
        GetMonth(doy - 1, out d);

    [Pure]
    private static int GetMonth(int d0y, out int d) =>
        MathN.AugmentedDivide(d0y, DaysPerMonth, out d);
}
