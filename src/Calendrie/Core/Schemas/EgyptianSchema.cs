// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Utilities;

/// <summary>
/// Represents an Egyptian schema and provides a base for derived classes.
/// <para>A schema is said to be egyptian if each year is divided into 12
/// months of 30 days each, followed by 5 epagomenal days.</para>
/// <para>This class can ONLY be inherited from within friend assemblies.</para>
/// </summary>
public abstract partial class EgyptianSchema : RegularSchema
{
    /// <summary>
    /// Represents the number of days in a year.
    /// <para>This field is a constant equal to 365.</para>
    /// </summary>
    public const int DaysPerYear = CalendricalConstants.DaysPerWanderingYear;

    /// <summary>
    /// Represents the number of days in an ordinary month, therefore excluding
    /// the intercalary month.
    /// <para>This field is a constant equal to 30.</para>
    /// </summary>
    public const int DaysPerMonth = 30;

    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="EgyptianSchema"/> class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minDaysInMonth"/>
    /// is a negative integer.</exception>
    private protected EgyptianSchema(int minDaysInMonth)
        : base(
            minDaysInYear: DaysPerYear,
            minDaysInMonth)
    { }

    /// <inheritdoc />
    public sealed override CalendricalFamily Family => CalendricalFamily.AnnusVagus;

    /// <inheritdoc />
    public sealed override CalendricalAdjustments PeriodicAdjustments => CalendricalAdjustments.None;
}

public partial class EgyptianSchema // Year, month or day infos
{
    /// <inheritdoc />
    [Pure]
    public sealed override bool IsLeapYear(int y) => false;

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryDay(int y, int m, int d) => false;
}

public partial class EgyptianSchema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYear(int y) => DaysPerYear;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYearBeforeMonth(int y, int m) => DaysPerMonth * (m - 1);
}

public partial class EgyptianSchema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysSinceEpoch(int y, int m, int d) =>
        DaysPerYear * (y - 1) + DaysPerMonth * (m - 1) + d - 1;

    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch, out int doy)
    {
        int y = 1 + MathZ.Divide(daysSinceEpoch, DaysPerYear, out int d0y);
        doy = 1 + d0y;
        return y;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch) =>
        1 + MathZ.Divide(daysSinceEpoch, DaysPerYear);
}

public partial class EgyptianSchema // Counting months and days since the epoch
{
    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYear(int y) => DaysPerYear * (y - 1);
}
