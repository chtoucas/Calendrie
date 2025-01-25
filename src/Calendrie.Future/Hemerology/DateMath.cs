// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core.Utilities;

// TODO(code): XML doc. Explain comparison with DateDifference
// Add a warning about the data (CountYearsBetweenData & co) which only
// offer symmetrical results; see DefaultDateMathFacts and DefaultMonthMathFacts.

// AddYears() et CountYearsBetween() ne sont pas indépendantes car ce dernier
// utilise le premier pour fonctionner. Les deux méthodes doivent donc utiliser
// la même règle AdditionRule. Idem avec l'"unité" mois.

/// <summary>
/// Provides non-standard mathematical operations for the
/// <see cref="IDateBase{TSelf}"/> type.
/// <para>This class allows to customize the <see cref="Calendrie.AdditionRule"/>
/// strategy.</para>
/// </summary>
public partial class DateMath
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DateMath"/> class using the
    /// default strategy.
    /// </summary>
    public DateMath() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DateMath"/> class using the
    /// specified strategy.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="rule"/>
    /// is not a known member of the <see cref="AdditionRule"/> enum.</exception>
    public DateMath(AdditionRule rule)
    {
        Requires.Defined(rule);

        AdditionRule = rule;
    }

    /// <summary>
    /// Gets the strategy employed to resolve ambiguities.
    /// </summary>
    public AdditionRule AdditionRule { get; }

    /// <summary>
    /// Computes the exact difference (expressed in years, months, weeks and
    /// days) between the two specified dates.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public DateDifference Subtract<TDate>(TDate start, TDate end)
        where TDate : struct, IDateBase<TDate>
    {
        // Fast track.
        if (start == end) return DateDifference.Zero;

        // Le résultat est exact car on effectue les calculs de proche en proche.
        // > end = start.PlusYears(years).PlusMonths(months).PlusWeeks(weeks).PlusDays(days)
        // À chaque étape, la valeur utilisée est la valeur maximale telle que
        // le résultat soit <= end (si start <= end). Attention, l'opération
        // n'est pas réversible :
        // > end.PlusDays(-days).PlusWeeks(-weeks).PlusMonths(-months).PlusYears(-years);
        // ne redonnera pas toujours "start". De même,
        // > Subtract(start, end) != - Subtract(end, start)
        int years = CountYearsBetween(start, end, out var newStart);
        int months = CountMonthsBetween(newStart, end, out newStart);
        int days = end.CountDaysSince(newStart);
        return DateDifference.UnsafeCreate(years, months, days);
    }

    /// <summary>
    /// Adds a number of years to the year field of the specified date, yielding
    /// a new date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public TDate AddYears<TDate>(TDate date, int years)
        where TDate : struct, IDateBase<TDate>
    {
        var newDate = date.PlusYears(years, out int roundoff);
        return roundoff == 0 ? newDate : Adjust(newDate, roundoff);
    }

    /// <summary>
    /// Adds a number of months to the month field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public TDate AddMonths<TDate>(TDate date, int months)
        where TDate : struct, IDateBase<TDate>
    {
        var newDate = date.PlusMonths(months, out int roundoff);
        return roundoff == 0 ? newDate : Adjust(newDate, roundoff);
    }

    /// <summary>
    /// Counts the number of years between the two specified dates.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public int CountYearsBetween<TDate>(TDate start, TDate end)
        where TDate : struct, IDateBase<TDate>
    {
        // Exact difference between two calendar years.
        int years = end.Year - start.Year;

        var newStart = AddYears(start, years);
        if (start < end)
        {
            if (newStart > end) years--;
        }
        else
        {
            if (newStart < end) years++;
        }

        return years;
    }

    /// <summary>
    /// Counts the number of years between the two specified dates.
    /// <para><paramref name="newStart"/> is the result of adding the found
    /// number (of years) to <paramref name="start"/>.</para>
    /// <para>If <paramref name="start"/> &lt;= <paramref name="end"/>, then
    /// <paramref name="start"/> &lt;= <paramref name="newStart"/> &lt;= <paramref name="end"/></para>
    /// <para>If <paramref name="start"/> &gt;= <paramref name="end"/>, then
    /// <paramref name="start"/> &gt;= <paramref name="newStart"/> &gt;= <paramref name="end"/></para>
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public int CountYearsBetween<TDate>(TDate start, TDate end, out TDate newStart)
        where TDate : struct, IDateBase<TDate>
    {
        // Exact difference between two calendar years.
        int years = end.Year - start.Year;

        newStart = AddYears(start, years);
        if (start < end)
        {
            if (newStart > end) newStart = AddYears(start, --years);
            Debug.Assert(newStart <= end);
        }
        else
        {
            if (newStart < end) newStart = AddYears(start, ++years);
            Debug.Assert(newStart >= end);
        }

        return years;
    }

    /// <summary>
    /// Counts the number of months between the two specified dates.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public int CountMonthsBetween<TDate>(TDate start, TDate end)
        where TDate : struct, IDateBase<TDate>
    {
        var (y0, m0, _) = start;
        var (y1, m1, _) = end;

        // Exact difference between two calendar months.
        int months = CountMonthsBetween<TDate>(y0, m0, y1, m1);

        var newStart = AddMonths(start, months);
        if (start < end)
        {
            if (newStart > end) months--;
        }
        else
        {
            if (newStart < end) months++;
        }

        return months;
    }

    /// <summary>
    /// Counts the number of months between the two specified dates.
    /// <para><paramref name="newStart"/> is the result of adding the found
    /// number (of months) to <paramref name="start"/>.</para>
    /// <para>If <paramref name="start"/> &lt;= <paramref name="end"/>, then
    /// <paramref name="start"/> &lt;= <paramref name="newStart"/> &lt;= <paramref name="end"/></para>
    /// <para>If <paramref name="start"/> &gt;= <paramref name="end"/>, then
    /// <paramref name="start"/> &gt;= <paramref name="newStart"/> &gt;= <paramref name="end"/></para>
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public int CountMonthsBetween<TDate>(TDate start, TDate end, out TDate newStart)
        where TDate : struct, IDateBase<TDate>
    {
        var (y0, m0, _) = start;
        var (y1, m1, _) = end;

        // Exact difference between two calendar months.
        int months = CountMonthsBetween<TDate>(y0, m0, y1, m1);

        newStart = AddMonths(start, months);
        if (start < end)
        {
            if (newStart > end) newStart = AddMonths(start, --months);
            Debug.Assert(newStart <= end);
        }
        else
        {
            if (newStart < end) newStart = AddMonths(start, ++months);
            Debug.Assert(newStart >= end);
        }

        return months;
    }

    //
    // Helpers
    //

    /// <summary>
    /// Counts the (exact) number of months between the two specified months.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    private static int CountMonthsBetween<TDate>(int y0, int m0, int y1, int m1)
        where TDate : struct, IDateBase<TDate>
    {
        var sch = TDate.Calendar.Scope.Schema;
        return checked(sch.CountMonthsSinceEpoch(y1, m1) - sch.CountMonthsSinceEpoch(y0, m0));
    }

    /// <summary>
    /// Adjusts the result using the specified rule.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    private TDate Adjust<TDate>(TDate date, int roundoff)
        where TDate : struct, IDateBase<TDate>
    {
        // Si on ne filtrait pas roundoff > 0, il faudrait prendre en compte
        // le cas roundoff = 0 et retourner date (résultat exact).
        Debug.Assert(roundoff > 0);

        // NB: date is the last day of a month.
        return AdditionRule switch
        {
            AdditionRule.Truncate => date,
            AdditionRule.Overspill => date.NextDay(),
            AdditionRule.Exact => date.PlusDays(roundoff),

            _ => throw new NotSupportedException(),
        };
    }
}
