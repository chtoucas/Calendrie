// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Utilities;

using Ptolemaic12 = PtolemaicSchema.Twelve;

/// <summary>Represents the Egyptian schema.</summary>
/// <remarks>
/// <para>This class cannot be inherited.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.</para>
/// <para>A year is divided into 12 months of 30 days each, followed by 5 epagomenal days.</para>
/// <para>The epagomenal days are outside any month but, for technical reasons, we attach them to
/// the twelfth month: 31/12 to 35/12.</para>
/// </remarks>
public sealed partial class Egyptian12Schema :
    EgyptianSchema,
    IDaysInMonths,
    ISchemaActivator<Egyptian12Schema>
{
    /// <summary>Represents the number of months in a year.</summary>
    /// <remarks>This field is a constant equal to 12.</remarks>
    public const int MonthsPerYear = 12;

    /// <summary>Initializes a new instance of the <see cref="Egyptian12Schema"/> class.</summary>
    internal Egyptian12Schema() : base(minDaysInMonth: 30) { }

    /// <inheritdoc />
    public sealed override int MonthsInYear => MonthsPerYear;

    /// <summary>
    /// Gets the number of days in each month of a year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    internal static ReadOnlySpan<byte> DaysInMonth =>
        // No leap years.
        // Quarters: 90, 90, 90, 90 + 5.
        [30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 35];

    /// <inheritdoc />
    [Pure]
    static ReadOnlySpan<byte> IDaysInMonths.GetDaysInMonthsOfYear(bool leapYear) => DaysInMonth;

    /// <inheritdoc />
    [Pure]
    static Egyptian12Schema ISchemaActivator<Egyptian12Schema>.CreateInstance() => new();
}

public partial class Egyptian12Schema // Year, month or day infos
{
    /// <summary>
    /// Determines whether the specified date is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    internal static bool IsEpagomenalDayImpl(int d, out int epagomenalNumber) =>
        Ptolemaic12.IsEpagomenalDay(d, out epagomenalNumber);

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
    public sealed override bool IsSupplementaryDay(int y, int m, int d) =>
        Ptolemaic12.IsSupplementaryDay(d);
}

public partial class Egyptian12Schema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int y, int m) => m == 12 ? 35 : 30;
}

public partial class Egyptian12Schema // Conversions
{
    /// <inheritdoc />
    public sealed override void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        y = 1 + MathZ.Divide(daysSinceEpoch, DaysPerYear, out int d0y);
        m = Ptolemaic12.GetMonth(d0y, out d);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d) =>
        Ptolemaic12.GetMonth(doy - 1, out d);
}
