// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;

// This is a place to experiment additions to the API.

// TODO(code): add PlusYears/Months(roundoff) to all date and month types?
// For the others, use DateMath.
// Naming: newStart or ???
// Add more tests in CivilTests and GregorianTests.
// Add a warning about the data (CountYearsBetweenData & co) which only
// offer symmetrical results; see DefaultDateMathFacts and DefaultMonthMathFacts.

public partial class TropicaliaCalendar // Complements
{
    [Pure]
    internal TropicaliaDate AddYears(TropicaliaDate date, int years, out int roundoff)
    {
        Schema.GetDateParts(date.DaysSinceZero, out int y, out int m, out int d);

        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        int daysInMonth = Schema.CountDaysInMonth(newY, m);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        int newD = roundoff == 0 ? d : daysInMonth;

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(newY, m, newD);
        return TropicaliaDate.UnsafeCreate(daysSinceEpoch);
    }

    [Pure]
    internal TropicaliaDate AddMonths(TropicaliaDate date, int months, out int roundoff)
    {
        Schema.GetDateParts(date.DaysSinceZero, out int y, out int m, out int d);

        int newM = 1 + MathZ.Modulo(
            checked(m - 1 + months), TropicalistaSchema.MonthsPerYear, out int years);

        // Ce qui suit équivaut à
        // > return AddYears(new TropicaliaDate(y, newM, d), years, out roundoff);

        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        int daysInMonth = Schema.CountDaysInMonth(newY, newM);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        int newD = roundoff == 0 ? d : daysInMonth;

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(newY, newM, newD);
        return TropicaliaDate.UnsafeCreate(daysSinceEpoch);
    }
}

public partial struct TropicaliaDate // Non-standard math ops
{
    /// <summary>
    /// Adds the specified number of years to the year part of this date instance
    /// and also returns the roundoff in an output parameter, yielding a new date.
    /// </summary>
    /// <returns>The end of the target month when roundoff &gt; 0.</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public TropicaliaDate PlusYears(int years, out int roundoff) =>
        Calendar.AddYears(this, years, out roundoff);

    /// <summary>
    /// Adds the specified number of months to the month part of this date
    /// instance and also returns the roundoff in an output parameter, yielding
    /// a new date.
    /// </summary>
    /// <returns>The end of the target month when roundoff &gt; 0.</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public TropicaliaDate PlusMonths(int months, out int roundoff) =>
        Calendar.AddMonths(this, months, out roundoff);
}
