﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

// We don't need to use MathZ, therefore the computations here are strictly
// identical whether the year is >= 0 or < 0.

/// <summary>
/// Provides a base for the Gregorian and Julian schemas.
/// <para>This class can ONLY be inherited from within friend assemblies.</para>
/// </summary>
public abstract partial class GJSchema : RegularSchema, IDaysInMonths
{
    /// <summary>
    /// Represents the number of months in a year.
    /// <para>This field is a constant equal to 12.</para>
    /// </summary>
    public const int MonthsPerYear = 12;

    /// <summary>
    /// Represents the number of days in a common year.
    /// <para>This field is a constant equal to 365.</para>
    /// </summary>
    public const int DaysPerCommonYear = CalendricalConstants.DaysPerWanderingYear;

    /// <summary>
    /// Represents the number of days in a leap year.
    /// <para>This field is a constant equal to 366.</para>
    /// </summary>
    public const int DaysPerLeapYear = DaysPerCommonYear + 1;

    /// <summary>
    /// Represents the number of days from march to december, both included.
    /// <para>This field is a constant equal to 306.</para>
    /// </summary>
    public const int DaysPerYearAfterFebruary = 306;

    /// <summary>
    /// Called from constructors in derived classes to initialize the <see cref="GJSchema"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="supportedYears"/>
    /// is not a subinterval of <see cref="CalendricalSchema.MaxSupportedYears"/>.
    /// </exception>
    private protected GJSchema(Segment<int> supportedYears)
        : base(
            supportedYears,
            minDaysInYear: DaysPerCommonYear,
            minDaysInMonth: 28)
    {
        Debug.Assert(supportedYears.IsSubsetOf(DefaultSupportedYears));
    }

    /// <inheritdoc />
    public sealed override CalendricalFamily Family => CalendricalFamily.Solar;

    /// <inheritdoc />
    public sealed override CalendricalAdjustments PeriodicAdjustments =>
        CalendricalAdjustments.Days;

    /// <summary>
    /// Gets the number of days in each year of the first 4-year cycle,
    /// the one starting at year 0.
    /// <para>The span index matches the year number (0 to 3).</para>
    /// </summary>
    internal static ReadOnlySpan<ushort> DaysIn4YearCycle => [366, 365, 365, 365];

    /// <summary>
    /// Gets the number of days in each month of a common year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    private static ReadOnlySpan<byte> DaysInMonthsOfCommonYear =>
        // Quarters: 90, 91, 92, 92.
        [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    /// <summary>
    /// Gets the number of days in each month of a leap year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    private static ReadOnlySpan<byte> DaysInMonthsOfLeapYear =>
        // Quarters: 91, 91, 92, 92.
        [31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    /// <inheritdoc />
    [Pure]
    static ReadOnlySpan<byte> IDaysInMonths.GetDaysInMonthsOfYear(bool leapYear) =>
        leapYear ? DaysInMonthsOfLeapYear : DaysInMonthsOfCommonYear;
}

public partial class GJSchema // Year, month or day infos
{
    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryDay(int y, int m, int d) => m == 2 && d == 29;

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsSupplementaryDay(int y, int m, int d) => false;
}

public partial class GJSchema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYear(int y) =>
        IsLeapYear(y) ? DaysPerLeapYear : DaysPerCommonYear;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYearBeforeMonth(int y, int m) =>
        // The "plain" formula is given by:
        //   days = (153 * m + 2) / 5
        // corrected for m > 2:
        //   days + (IsLeapYear(y) ? 60 : 59);
        m < 3 ? 31 * (m - 1)
        : IsLeapYear(y) ? (int)((uint)(153 * m - 157) / 5)
        : (int)((uint)(153 * m - 162) / 5);

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int y, int m) =>
        // Trick:
        // - month < 8,  30 if even, 31 if odd
        // - month >= 8, 31 if even, 30 if odd
        // divide by 8 (m >> 3) = 0 (m < 8) or 1 (m >= 8), add m, and
        // finally check the parity (p & 1) = 0 if even, 1 if odd.
        m != 2 ? 30 + ((m + (m >> 3)) & 1)
        : IsLeapYear(y) ? 29
        : 28;
}

public partial class GJSchema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d)
    {
        int m;

        // Ordered from the most common case to least common one.
        // NB: if we change the code, update TropicaliaSchema.
        if (doy > 60)
        {
            doy -= IsLeapYear(y) ? 61 : 60;
            uint n = (uint)(5 * doy + 2) / 153;

            m = (int)(n + 3);
            d = doy - (int)((153 * n + 2) / 5) + 1;
        }
        else if (doy < 60)
        {
            m = MathN.AugmentedDivide(doy - 1, 31, out d);

        }
        else // doy == 60
        {
            if (IsLeapYear(y))
            {
                m = 2; d = 29;
            }
            else
            {
                m = 3; d = 1;
            }
        }

        return m;
    }
}
