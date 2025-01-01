// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology.Arithmetic;

using Calendrie.Core;

/// <summary>
/// Provides a reference implementation for <see cref="NakedArithmetic"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class PlainArithmetic : NakedArithmetic
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlainArithmetic"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="segment"/> is
    /// null.</exception>
    public PlainArithmetic(CalendricalSegment segment) : base(segment) { }
}

public partial class PlainArithmetic // Operations on DateParts
{
    /// <inheritdoc />
    [Pure]
    public sealed override DateParts AddDays(DateParts parts, int days)
    {
        var (y, m, d) = parts;
        int daysSinceEpoch = checked(Schema.CountDaysSinceEpoch(y, m, d) + days);
        DaysSinceEpochChecker.CheckOverflow(daysSinceEpoch);

        return PartsAdapter.GetDateParts(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override DateParts NextDay(DateParts parts) => AddDays(parts, 1);

    /// <inheritdoc />
    [Pure]
    public sealed override DateParts PreviousDay(DateParts parts) => AddDays(parts, -1);
}

public partial class PlainArithmetic // Operations on OrdinalParts
{
    /// <inheritdoc />
    [Pure]
    public sealed override OrdinalParts AddDays(OrdinalParts parts, int days)
    {
        var (y, doy) = parts;
        int daysSinceEpoch = checked(Schema.CountDaysSinceEpoch(y, doy) + days);
        DaysSinceEpochChecker.CheckOverflow(daysSinceEpoch);

        return PartsAdapter.GetOrdinalParts(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override OrdinalParts NextDay(OrdinalParts parts) => AddDays(parts, 1);

    /// <inheritdoc />
    [Pure]
    public sealed override OrdinalParts PreviousDay(OrdinalParts parts) => AddDays(parts, -1);
}

public partial class PlainArithmetic // Operations on MonthParts
{
    /// <inheritdoc />
    [Pure]
    public sealed override MonthParts AddMonths(MonthParts parts, int months)
    {
        var (y, m) = parts;
        int monthsSinceEpoch = checked(Schema.CountMonthsSinceEpoch(y, m) + months);
        MonthsSinceEpochChecker.CheckOverflow(monthsSinceEpoch);

        return PartsAdapter.GetMonthParts(monthsSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override MonthParts NextMonth(MonthParts parts) => AddMonths(parts, 1);

    /// <inheritdoc />
    [Pure]
    public sealed override MonthParts PreviousMonth(MonthParts parts) => AddMonths(parts, -1);
}
