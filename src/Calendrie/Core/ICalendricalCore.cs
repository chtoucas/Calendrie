﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

/// <summary>
/// Defines the core calendrical methods.
/// </summary>
public interface ICalendricalCore
{
    /// <summary>
    /// Gets the calendrical algorithm: arithmetical, astronomical or
    /// observational.
    /// </summary>
    CalendricalAlgorithm Algorithm { get; }

    /// <summary>
    /// Gets the calendrical family, determined by the astronomical cycle: solar,
    /// lunar, lunisolar...
    /// </summary>
    CalendricalFamily Family { get; }

    /// <summary>
    /// Gets the method employed at regular intervals in order to synchronise
    /// the two main cycles, lunar and solar.
    /// </summary>
    CalendricalAdjustments PeriodicAdjustments { get; }

    /// <summary>
    /// Returns <see langword="true"/> if this schema is regular; otherwise
    /// returns <see langword="false"/>.
    /// <para>The number of months is given in an output parameter; if this
    /// schema is not regular <paramref name="monthsInYear"/> is set to 0.
    /// </para>
    /// <para>See also <seealso cref="CountMonthsInYear(int)"/>.</para>
    /// </summary>
    [Pure] bool IsRegular(out int monthsInYear);

    /// <summary>
    /// Determines whether the specified year is leap or not.
    /// <para>A leap year is a year with at least one intercalary day, week or
    /// month.</para>
    /// </summary>
    [Pure] bool IsLeapYear(int y);

    /// <summary>
    /// Determines whether the specified month is intercalary or not.
    /// </summary>
    [Pure] bool IsIntercalaryMonth(int y, int m);

    /// <summary>
    /// Determines whether the specified date is an intercalary day or not.
    /// </summary>
    [Pure] bool IsIntercalaryDay(int y, int m, int d);

    /// <summary>
    /// Determines whether the specified date is a supplementary day or not.
    /// <para>Supplementary days are days kept outside the intermediary cycles,
    /// those shorter than a year. For technical reasons, we usually attach them
    /// to the month before. Notice that a supplementary day may be intercalary
    /// too. An example of such days is given by the epagomenal days which are
    /// kept outside any regular month or decade.</para>
    /// </summary>
    //
    // By attaching a supplementary day to the preceding month, we differ
    // from NodaTime & others which seem to prefer the creation of a virtual
    // month for holding the supplementary days. Advantages/disadvantages:
    // - CountMonthsInYear() returns the number of months as defined by the
    //   calendar.
    // - CountDaysInMonth() does not always return the actual number of days
    //   in a month.
    // - more importantly, arithmetical ops work without any modification.
    //   For instance, with the simple Egyptian calendar, it always bothered
    //   me that with Nodatime 30/12/1970 + 13 months = 05/13/1970, it seems
    //   to me more sensical to get 30/01/1971.
    [Pure] bool IsSupplementaryDay(int y, int m, int d);

    /// <summary>
    /// Obtains the number of months in the specified year.
    /// <para>See also <seealso cref="IsRegular(out int)"/>.</para>
    /// </summary>
    [Pure] int CountMonthsInYear(int y);

    /// <summary>
    /// Obtains the number of days in the specified year.
    /// </summary>
    [Pure] int CountDaysInYear(int y);

    /// <summary>
    /// Obtains the number of days in the specified month.
    /// </summary>
    [Pure] int CountDaysInMonth(int y, int m);
}
