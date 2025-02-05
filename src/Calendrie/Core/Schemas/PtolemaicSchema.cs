// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Utilities;

/// <summary>
/// Represents a Ptolemaic schema and provides a base for derived classes.
/// <para>A schema is said to be ptolemaic if each year is divided into 12
/// months of 30 days each, followed by 5 (resp. 6) epagomenal days on a
/// common (resp. leap) year.</para>
/// <para>The epagomenal days are outside any month but, for technical
/// reasons, we attach them to the twelfth month: 31/12 to 35/12 (36/12 on a
/// leap year).</para>
/// <para>We propose an alternative form using a virtual thirteenth month to
/// hold the epagomenal days: 1/13 to 5/13 (6/13 on a leap year).</para>
/// <para>This class can ONLY be initialized from within friend assemblies.</para>
/// </summary>
public abstract partial class PtolemaicSchema : RegularSchema
{
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
    /// Represents the number of days in a standard month (excluding the
    /// month holding the epagomenal days).
    /// <para>This field is constant equal to 30.</para>
    /// </summary>
    public const int DaysPerMonth = 30;

    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="PtolemaicSchema"/> class.
    /// </summary>
    private protected PtolemaicSchema(int minDaysInMonth)
        : base(
            minDaysInYear: DaysPerCommonYear,
            minDaysInMonth)
    { }

    /// <inheritdoc />
    public sealed override CalendricalFamily Family => CalendricalFamily.Solar;

    /// <inheritdoc />
    public sealed override CalendricalAdjustments PeriodicAdjustments => CalendricalAdjustments.Days;
}

public partial class PtolemaicSchema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYear(int y) =>
        IsLeapYear(y) ? DaysPerLeapYear : DaysPerCommonYear;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYearBeforeMonth(int y, int m) => DaysPerMonth * (m - 1);
}

public partial class PtolemaicSchema // Twelve & Thirteen
{
    private protected static class Twelve
    {
        [Pure]
        public static bool IsEpagomenalDay(int d, out int epagomenalNumber)
        {
            if (d > DaysPerMonth)
            {
                epagomenalNumber = d - DaysPerMonth;
                return true;
            }
            else
            {
                epagomenalNumber = 0;
                return false;
            }
        }

        [Pure]
        public static bool IsIntercalaryDay(int d) => d == 36;

        [Pure]
        public static bool IsSupplementaryDay(int d) => d > DaysPerMonth;

        [Pure]
        public static int CountDaysInMonth(bool leapYear, int m) =>
            m == 12 ? (leapYear ? 36 : 35) : DaysPerMonth;

        [Pure]
        public static int GetMonth(int d0y, out int d)
        {
            int m = MathN.AugmentedDivide(d0y, DaysPerMonth, out d);

            // Special case of the intercalary twelfth month (d0y > 329).
            if (m == 13)
            {
                m = 12;
                d += DaysPerMonth;
            }

            return m;
        }
    }

    private protected static class Thirteen
    {
        [Pure]
        public static bool IsEpagomenalDay(int m, int d, out int epagomenalNumber)
        {
            if (m == 13)
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

        [Pure]
        public static bool IsIntercalaryDay(int m, int d) => m == 13 && d == 6;

        [Pure]
        public static bool IsSupplementaryDay(int m) => m == 13;

        [Pure]
        public static int CountDaysInMonth(bool leapYear, int m) =>
            m == 13 ? (leapYear ? 6 : 5) : DaysPerMonth;

        [Pure]
        public static int GetMonth(int d0y, out int d) =>
            MathN.AugmentedDivide(d0y, DaysPerMonth, out d);
    }
}
