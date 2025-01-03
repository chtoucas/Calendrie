// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

// Keep this interface internal, it only exists to simplify testing.
// This interface is expected to be implemented explicitely.
// Cannot be part of ICalendricalSchema as it would require C# to support
// "static abstract" methods in abstract classes.
//
// Do NOT change the parameter from "leapYear" to "year". The method is abstract
// and we don't want to have to validate the input parameter.

/// <summary>
/// Defines support for a function returning the distribution of days in month.
/// </summary>
internal interface IDaysInMonths
{
    /// <summary>
    /// Obtains the number of days in each month of a common or leap year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    [Pure] static abstract ReadOnlySpan<byte> GetDaysInMonthsOfYear(bool leapYear);
}
