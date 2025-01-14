// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

// This is a place to experiment additions to the API.

// REVIEW(code): maybe only ToGregorian/JulianDate(), ie when interconversion is
// always possible. Gregorian/JulianDate.From...()?
// See also Calendrie.Extensions.Interconversion.

public partial struct TropicaliaDate
{
    /// <summary>
    /// Creates a new instance of the <see cref="TropicaliaDate"/> struct from
    /// the specified absolute date.
    /// </summary>
    [Pure]
    public static TropicaliaDate FromAbsoluteDate(IAbsoluteDate date)
    {
        ArgumentNullException.ThrowIfNull(date);
        return FromDayNumber(date.DayNumber);
    }

    /// <summary>
    /// (Inter)Converts the current instance to a <typeparamref name="TDate"/>
    /// value.
    /// </summary>
    [Pure]
    public TDate ToAbsoluteDate<TDate>() where TDate : IAbsoluteDate<TDate>
    {
        return TDate.FromDayNumber(DayNumber);
    }
}

// REVIEW(code): non-standard math ops (experimental), also for month types.
// Probably, only for CivilDate, GregorianDate and JulianDate.
// For the others, use DateMath.
// See also CivilDate in main project.

public partial struct TropicaliaDate // Non-standard math ops (plain implementation)
{
    [Pure]
    public TropicaliaDate PlusYears(int years, AdditionRule rule)
    {
        var (y, m, d) = this;
        var newDate = AddYears(y, m, d, years, out int roundoff);
        return roundoff == 0 ? newDate : Adjust(newDate, roundoff, rule);
    }

    [Pure]
    public TropicaliaDate PlusYears(int years, out int roundoff)
    {
        var (y, m, d) = this;
        return AddYears(y, m, d, years, out roundoff);
    }

    [Pure]
    public TropicaliaDate PlusMonths(int months, AdditionRule rule)
    {
        var (y, m, d) = this;
        var newDate = AddMonths(y, m, d, months, out int roundoff);
        return roundoff == 0 ? newDate : Adjust(newDate, roundoff, rule);
    }

    [Pure]
    public TropicaliaDate PlusMonths(int months, out int roundoff)
    {
        var (y, m, d) = this;
        return AddMonths(y, m, d, months, out roundoff);
    }

    [Pure]
    public int CountYearsSince(TropicaliaDate other, AdditionRule rule, out TropicaliaDate newStart)
    {
        int years = TropicaliaYear.FromDate(this) - TropicaliaYear.FromDate(other);

        newStart = PlusYears(years, rule);
        if (other < this)
        {
            if (newStart > this) newStart = PlusYears(--years, rule);
            Debug.Assert(newStart <= this);
        }
        else
        {
            if (newStart < this) newStart = PlusYears(++years, rule);
            Debug.Assert(newStart >= this);
        }

        return years;
    }

    [Pure]
    public int CountMonthsSince(TropicaliaDate other, AdditionRule rule, out TropicaliaDate newStart)
    {
        int months = TropicaliaMonth.FromDate(this) - TropicaliaMonth.FromDate(other);

        newStart = PlusMonths(months, rule);
        if (other < this)
        {
            if (newStart > this) newStart = PlusMonths(--months, rule);
            Debug.Assert(newStart <= this);
        }
        else
        {
            if (newStart < this) newStart = PlusMonths(++months, rule);
            Debug.Assert(newStart >= this);
        }

        return months;
    }

    //
    // Helpers
    //

    [Pure]
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

    [Pure]
    private static TropicaliaDate AddMonths(int y, int m, int d, int months, out int roundoff)
    {
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), TropicalistaSchema.MonthsInYear, out int years);
        return AddYears(y, newM, d, years, out roundoff);
    }

    [Pure]
    private static TropicaliaDate Adjust(TropicaliaDate date, int roundoff, AdditionRule rule)
    {
        // Si on ne filtrait pas roundoff > 0, il faudrait prendre en compte
        // le cas roundoff = 0 et retourner date (résultat exact).
        Debug.Assert(roundoff > 0);

        // NB: date is the last day of a month.
        return rule switch
        {
            AdditionRule.Truncate => date,
            AdditionRule.Overspill => date.NextDay(),
            AdditionRule.Exact => date.PlusDays(roundoff),
            AdditionRule.Overflow => throw new OverflowException(),

            _ => throw new NotSupportedException(),
        };
    }
}
