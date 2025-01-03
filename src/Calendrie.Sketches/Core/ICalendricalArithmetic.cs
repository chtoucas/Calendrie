// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

/// <summary>
/// Defines the core mathematical operations on dates and months, and provides
/// a base for derived classes.
/// <para>Operations are <i>lenient</i>, they assume that their parameters are
/// valid from a calendrical point of view. They MUST ensure that all returned
/// values are valid when the previous condition is met.</para>
/// </summary>
public interface ICalendricalArithmetic
{
    //
    // Operations on "Yemoda"
    //
    // Non-standard ops, those using the year or month units:
    // - AddYears(Yemoda, years)
    // - AddYears(Yemoda, years, out roundoff)
    // - AddMonths(Yemoda, months)
    // - AddMonths(Yemoda, months, out roundoff)
    // Of course, one can also implement the standard ops:
    // - AddDays(Yemoda, days)
    // - CountDaysBetween(Yemoda, Yemoda)
    // but we don't do it here because all our date types are based on the count
    // of days since the epoch (daysSinceEpoch) for which these ops are trivial.

    /// <summary>
    /// Adds a number of years to the year field of the specified date, yielding
    /// a new date.
    /// </summary>
    /// <returns>The end of the target month (resp. year) when the naive result
    /// is not a valid day (resp. month).</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure] Yemoda AddYears(int y, int m, int d, int years);

    /// <summary>
    /// Adds a number of years to the year field of the specified date, yielding
    /// a new date.
    /// </summary>
    /// <returns>The end of the target month (resp. year) when the naive result
    /// is not a valid day (resp. month).</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure] Yemoda AddYears(int y, int m, int d, int years, out int roundoff);

    /// <summary>
    /// Adds a number of months to the specified date, yielding a new date.
    /// </summary>
    /// <returns>The last day of the month when the naive result is not a valid
    /// day (roundoff > 0).</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure] Yemoda AddMonths(int y, int m, int d, int months);

    /// <summary>
    /// Adds a number of months to the specified date, yielding a new date.
    /// </summary>
    /// <returns>The last day of the month when the naive result is not a valid
    /// day (roundoff > 0).</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure] Yemoda AddMonths(int y, int m, int d, int months, out int roundoff);

    //
    // Operations on "Yemo"
    //
    // The standard ops, those based on the month unit:
    // - AddMonths(Yemo, months)
    // - CountMonthsBetween(Yemo, Yemo)
    // The non-standard ops:
    // - AddYears(Yemo, years)
    // - AddYears(Yemo, years, out roundoff)

    /// <summary>
    /// Adds a number of months to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported calendar months.</exception>
    [Pure] Yemo AddMonths(int y, int m, int months);

    /// <summary>
    /// Counts the number of months between the two specified months.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported calendar months.</exception>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "F# & VB.NET End statement.")]
    [Pure] int CountMonthsBetween(Yemo start, Yemo end);
}
