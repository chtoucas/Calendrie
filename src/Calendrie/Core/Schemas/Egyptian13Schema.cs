﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Utilities;

using Ptolemaic13 = PtolemaicSchema.Thirteen;

/// <summary>
/// Represents the Egyptian schema; alternative form using a virtual month to hold the
/// epagomenal days, see also <seealso cref="Egyptian12Schema"/>.
/// <para>This class cannot be inherited.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.</para>
/// </summary>
/// <remarks>
/// <para>A year is divided into 12 months of 30 days each, followed by 5 epagomenal days.</para>
/// <para>The epagomenal days are outside any month but, for technical reasons, we attach them
/// to a virtual thirteenth month: 1/13 to 5/13.</para>
/// </remarks>
public sealed partial class Egyptian13Schema :
    EgyptianSchema,
    IEpagomenalDayFeaturette,
    IDaysInMonths,
    ISchemaActivator<Egyptian13Schema>
{
    /// <summary>
    /// Represents the number of months in a year.
    /// <para>This field is a constant equal to 13.</para>
    /// </summary>
    public const int MonthsInYear = 13;

    /// <summary>
    /// Represents the virtual month.
    /// <para>This field is a constant equal to 13.</para>
    /// </summary>
    public const int VirtualMonth = 13;

    /// <summary>
    /// Initializes a new instance of the <see cref="Egyptian13Schema"/> class.
    /// </summary>
    internal Egyptian13Schema() : base(5) { }

    /// <summary>
    /// Gets the number of days in each month of a year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    internal static ReadOnlySpan<byte> DaysInMonth =>
        // No leap years.
        [30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 5];

    /// <inheritdoc />
    [Pure]
    static ReadOnlySpan<byte> IDaysInMonths.GetDaysInMonthsOfYear(bool leapYear) =>
        DaysInMonth;

    /// <inheritdoc />
    [Pure]
    static Egyptian13Schema ISchemaActivator<Egyptian13Schema>.CreateInstance() => new();

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsRegular(out int monthsInYear)
    {
        monthsInYear = MonthsInYear;
        return true;
    }
}

public partial class Egyptian13Schema // Year, month or day infos
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenalDay(int y, int m, int d, out int epagomenalNumber) =>
        Ptolemaic13.IsEpagomenalDay(m, d, out epagomenalNumber);

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsSupplementaryDay(int y, int m, int d) =>
        Ptolemaic13.IsSupplementaryDay(m);
}

public partial class Egyptian13Schema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsInYear(int y) => MonthsInYear;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int y, int m) => m == 13 ? 5 : 30;
}

public partial class Egyptian13Schema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsSinceEpoch(int y, int m) =>
        MonthsCalculator.Regular13.CountMonthsSinceEpoch(y, m);

    /// <inheritdoc />
    public sealed override void GetMonthParts(int monthsSinceEpoch, out int y, out int m) =>
        MonthsCalculator.Regular13.GetMonthParts(monthsSinceEpoch, out y, out m);

    /// <inheritdoc />
    public sealed override void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        y = 1 + MathZ.Divide(daysSinceEpoch, DaysInYear, out int d0y);
        m = Ptolemaic13.GetMonth(d0y, out d);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d) =>
        Ptolemaic13.GetMonth(doy - 1, out d);
}

public partial class Egyptian13Schema // Counting months and days since the epoch
{
    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYearInMonths(int y) =>
        MonthsCalculator.Regular13.GetStartOfYear(y);

    /// <inheritdoc />
    [Pure]
    public sealed override int GetEndOfYearInMonths(int y) =>
        MonthsCalculator.Regular13.GetEndOfYear(y);
}

public partial class Egyptian13Schema // Dates in a given year or month
{
    /// <inheritdoc />
    public sealed override void GetDatePartsAtEndOfYear(int y, out int m, out int d)
    {
        m = MonthsInYear; d = 5;
    }
}
