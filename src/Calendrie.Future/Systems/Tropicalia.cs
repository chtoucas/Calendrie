// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;

// This is a place to experiment additions to the API.

// TODO(code): add PlusYears(roundoff) to all date and month types.
//
// Expand the doc for ALL non-standard math ops (experimental or
// not), idem with the other date types and the AdditionRule too.
// Truncate (end of month), CountDaysSince() but may be CountDaysTill()...
// Experimental non-standard math ops: GregorianDate and JulianDate too.
// Naming: newStart or ???
// The default behaviour of CountYearsSince() seems not entirely coherent
// (see CivilTests). Mots certainly the same happens with CountMonthsSince().
// Add more tests in CivilTests and GregorianTests.
// Add a warning about the data (CountYearsBetweenData & co) which only
// offer symmetrical results; see DefaultDateMathFacts and DefaultMonthMathFacts.
//
// Non-standard math ops (experimental), also for month types.
// Probably, only for CivilDate, GregorianDate and JulianDate.
// For the others, use DateMath.
// See also CivilDate in main project.

public partial struct TropicaliaDate // Non-standard math ops (plain implementation)
{
    /// <summary>
    /// Adds a number of years to the year field of this date instance and also
    /// returns the roundoff in an output parameter, yielding a new date.
    /// </summary>
    /// <returns>The end of the target month when roundoff &gt; 0.</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure, ExcludeFromCodeCoverage]
    public TropicaliaDate PlusYears(int years, out int roundoff)
    {
        var (y, m, d) = this;
        return AddYears(y, m, d, years, out roundoff);
    }

    /// <summary>
    /// Adds a number of months to the month field of this date instance and also
    /// returns the roundoff in an output parameter, yielding a new date.
    /// </summary>
    /// <returns>The end of the target month when roundoff &gt; 0.</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure, ExcludeFromCodeCoverage]
    public TropicaliaDate PlusMonths(int months, out int roundoff)
    {
        var (y, m, d) = this;
        return AddMonths(y, m, d, months, out roundoff);
    }

    //
    // Helpers
    //

    [Pure, ExcludeFromCodeCoverage]
    private static TropicaliaDate AddYears(int y, int m, int d, int years, out int roundoff)
    {
        var sch = Calendar.Schema;

        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        int daysInMonth = sch.CountDaysInMonth(newY, m);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        int newD = roundoff == 0 ? d : daysInMonth;

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, m, newD);
        return new TropicaliaDate(daysSinceEpoch);
    }

    [Pure, ExcludeFromCodeCoverage]
    private static TropicaliaDate AddMonths(int y, int m, int d, int months, out int roundoff)
    {
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), TropicalistaSchema.MonthsPerYear, out int years);
        return AddYears(y, newM, d, years, out roundoff);
    }
}
