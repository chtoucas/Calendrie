﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Utilities;

/// <summary>Represents an Egyptian schema and provides a base for derived classes.</summary>
/// <remarks>This class can ONLY be inherited from within friend assemblies.</remarks>
public abstract partial class EgyptianSchema : LimitSchema
{
    /// <summary>Represents the number of days in a year.</summary>
    /// <remarks>This field is constant equal to 365.</remarks>
    public const int DaysInYear = CalendricalConstants.DaysInWanderingYear;

    /// <summary>Represents the genuine number of days in a month (excluding the epagomenal days
    /// that are not formally part of the twelfth month).</summary>
    /// <remarks>This field is constant equal to 30.</remarks>
    public const int DaysInEgyptianMonth = 30;

    /// <summary>Called from constructors in derived classes to initialize the
    /// <see cref="EgyptianSchema"/> class.</summary>
    private protected EgyptianSchema(int minDaysInMonth) : base(DaysInYear, minDaysInMonth) { }

    /// <inheritdoc />
    public sealed override CalendricalFamily Family => CalendricalFamily.AnnusVagus;

    /// <inheritdoc />
    public sealed override CalendricalAdjustments PeriodicAdjustments =>
        CalendricalAdjustments.None;
}

public partial class EgyptianSchema // Year, month or day infos
{
    /// <inheritdoc />
    [Pure]
    public sealed override bool IsLeapYear(int y) => false;

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryMonth(int y, int m) => false;

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryDay(int y, int m, int d) => false;
}

public partial class EgyptianSchema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYear(int y) => DaysInYear;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYearBeforeMonth(int y, int m) => 30 * (m - 1);
}

public partial class EgyptianSchema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysSinceEpoch(int y, int m, int d) =>
        DaysInYear * (y - 1) + 30 * (m - 1) + d - 1;

    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch, out int doy)
    {
        int y = 1 + MathZ.Divide(daysSinceEpoch, DaysInYear, out int d0y);
        doy = 1 + d0y;
        return y;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch) =>
        1 + MathZ.Divide(daysSinceEpoch, DaysInYear);
}

public partial class EgyptianSchema // Counting months and days since the epoch
{
    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYear(int y) => DaysInYear * (y - 1);
}
