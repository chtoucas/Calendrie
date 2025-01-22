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
    /// <summary>
    /// Adds the specified number of years to the year part of the specified date,
    /// yielding a new date.
    /// <para>This method may truncate the result to the end of the target month
    /// to ensure that it returns a valid date; see <see cref="AdditionRule.Truncate"/>.
    /// </para>
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    internal TropicaliaDate AddYears(int y, int m, int d, int years)
    {
        var sch = Schema;

        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, sch.CountDaysInMonth(newY, m));

        int daysSinceZero = sch.CountDaysSinceEpoch(newY, m, newD);
        return TropicaliaDate.UnsafeCreate(daysSinceZero);
    }

    /// <summary>
    /// Adds the specified number of years to the year part of the specified date
    /// and also returns the roundoff in an output parameter, yielding a new date.
    /// <para><paramref name="roundoff"/> corresponds to the number of days that
    /// were cut off, which is greater than or equal to 0, the latter only
    /// happening when the operation is exact.</para>
    /// </summary>
    /// <returns>The end of the target month when roundoff &gt; 0.</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    internal TropicaliaDate AddYears(int y, int m, int d, int years, out int roundoff)
    {
        var sch = Schema;

        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        int daysInMonth = sch.CountDaysInMonth(newY, m);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        int newD = roundoff == 0 ? d : daysInMonth;

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, m, newD);
        return TropicaliaDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <summary>
    /// Adds the specified number of months to the month part of the specified
    /// date, yielding a new date.
    /// <para>This method may truncate the result to the end of the target month
    /// to ensure that it returns a valid date; see <see cref="AdditionRule.Truncate"/>.
    /// </para>
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    internal TropicaliaDate AddMonths(int y, int m, int d, int months)
    {
        // Exact addition of months to a calendar month.
        int newM = 1 + MathZ.Modulo(
            checked(m - 1 + months), TropicalistaSchema.MonthsPerYear, out int years);

        return AddYears(y, newM, d, years);
    }

    /// <summary>
    /// Adds the specified number of months to the month part of the specified
    /// date and also returns the roundoff in an output parameter, yielding a
    /// new date.
    /// <para><paramref name="roundoff"/> corresponds to the number of days that
    /// were cut off, which is greater than or equal to 0, the latter only
    /// happening when the operation is exact.</para>
    /// </summary>
    /// <returns>The end of the target month when roundoff &gt; 0.</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    internal TropicaliaDate AddMonths(int y, int m, int d, int months, out int roundoff)
    {
        // Exact addition of months to a calendar month.
        int newM = 1 + MathZ.Modulo(
            checked(m - 1 + months), TropicalistaSchema.MonthsPerYear, out int years);

        return AddYears(y, newM, d, years, out roundoff);
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
    public TropicaliaDate PlusYears(int years, out int roundoff)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return Calendar.AddYears(y, m, d, years, out roundoff);
    }

    /// <summary>
    /// Adds the specified number of months to the month part of this date
    /// instance and also returns the roundoff in an output parameter, yielding
    /// a new date.
    /// </summary>
    /// <returns>The end of the target month when roundoff &gt; 0.</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public TropicaliaDate PlusMonths(int months, out int roundoff)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return Calendar.AddMonths(y, m, d, months, out roundoff);
    }
}
