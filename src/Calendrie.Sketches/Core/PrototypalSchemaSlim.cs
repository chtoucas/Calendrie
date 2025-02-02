// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using Calendrie.Core.Utilities;

using NodaTime.Calendars;

public class PrototypalSchemaSlim : PrototypalSchema
{
    /// <summary>
    /// Represents the cache for <see cref="GetStartOfYear(int)"/>.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly StartOfYearCache[] _startOfYearCache = StartOfYearCache.Create();

    /// <summary>
    /// Initializes a new instance of the <see cref="PrototypalSchemaSlim"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.
    /// </exception>
    public PrototypalSchemaSlim(ICalendricalSchema schema, int minMonthsInYear) : base(schema)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(minMonthsInYear, 0);

        MinMonthsInYear = minMonthsInYear;
        // See GetMonth() for an explanation of the formula.
        ApproxMonthsInYear = 1 + (m_MinDaysInYear - 1) / m_MinDaysInMonth;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PrototypalSchemaSlim"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="kernel"/> is null.
    /// </exception>
    public PrototypalSchemaSlim(
        ICalendricalCore kernel,
        bool proleptic,
        int minMonthsInYear,
        int minDaysInYear,
        int minDaysInMonth)
        : base(kernel, proleptic, minDaysInYear, minDaysInMonth)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(minMonthsInYear, 0);

        MinMonthsInYear = minMonthsInYear;
        // See GetMonth() for an explanation of the formula.
        ApproxMonthsInYear = 1 + (minDaysInYear - 1) / minDaysInMonth;
    }

    public int MinMonthsInYear { get; }
    protected int ApproxMonthsInYear { get; }

    /// <inheritdoc />
    public override void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
    {
        // For explanations, see NonRegularSchemaPrototype.

        y = 1 + MathZ.Divide(monthsSinceEpoch, MinMonthsInYear);
        int startOfYear = GetStartOfYearInMonths(y);

        if (monthsSinceEpoch >= 0)
        {
            while (monthsSinceEpoch < startOfYear)
            {
                startOfYear -= m_Kernel.CountMonthsInYear(--y);
            }
        }
        else
        {
            while (monthsSinceEpoch >= startOfYear)
            {
                int startOfNextYear = startOfYear + m_Kernel.CountMonthsInYear(y);
                if (monthsSinceEpoch < startOfNextYear) { break; }
                y++;
                startOfYear = startOfNextYear;
            }
            Debug.Assert(monthsSinceEpoch >= startOfYear);
        }

        m = 1 + monthsSinceEpoch - startOfYear;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch, out int doy)
    {
        // For explanations, see NonRegularSchemaPrototype.

        int y = 1 + MathZ.Divide(daysSinceEpoch, m_MinDaysInYear);
        int startOfYear = GetStartOfYear(y);

        if (daysSinceEpoch >= 0)
        {
            while (daysSinceEpoch < startOfYear)
            {
                startOfYear -= m_Kernel.CountDaysInYear(--y);
            }
        }
        else
        {
            while (daysSinceEpoch >= startOfYear)
            {
                int startOfNextYear = startOfYear + m_Kernel.CountDaysInYear(y);
                if (daysSinceEpoch < startOfNextYear) { break; }
                y++;
                startOfYear = startOfNextYear;
            }
            Debug.Assert(daysSinceEpoch >= startOfYear);
        }

        doy = 1 + daysSinceEpoch - startOfYear;
        return y;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d)
    {
        // Algorithm:
        // > int m = GetMonth(y, doy);
        // > d = doy - CountDaysInYearBeforeMonth(y, m);
        // > return m;

        int monthsInYear = m_Kernel.CountMonthsInYear(y);

        // Base method: at most (monthsInYear - 1) iteration steps.
        // Local method: at most
        //   Math.Min(ApproxMonthsInYear, monthsInYear) - 1
        // iteration steps if doy = MinDaysInYear.
        if (ApproxMonthsInYear > 2 * monthsInYear) { return base.GetMonth(y, doy, out d); }

        // To get our first approximation of the value of the month, we pretend
        // that the months have a constant length equal to MinDaysInMonth.
        // > int m = MathN.AugmentedDivide(doy - 1, MinDaysInMonth, out int d);
        // Notice that the division gives us a zero-based month.
        // We can ignore the remainder of the division which gives a theoretical
        // but wrong (except if the months do actually have a constant length)
        // value for the day of the month.
        int m = Math.Min(1 + (doy - 1) / m_MinDaysInMonth, monthsInYear);
        int daysInYearBeforeMonth = CountDaysInYearBeforeMonth(y, m);

        while (doy < 1 + daysInYearBeforeMonth)
        {
            //if (m == 1) { daysInYearBeforeMonth = 0; break; }
            daysInYearBeforeMonth -= m_Kernel.CountDaysInMonth(y, --m);
        }

        // Notice that, as expected, d >= 1.
        d = doy - daysInYearBeforeMonth;
        return m;
    }


    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYear(int y)
    {
        // TODO(code): currently, we just copy the cache class from NodaTime.
        // https://github.com/bitfaster/BitFaster.Caching

        int index = StartOfYearCache.GetIndex(y);
        var value = _startOfYearCache[index];

        if (!value.IsValidForYear(y))
        {
            int startOfYear = GetStartOfYearCore(y);
            value = new StartOfYearCache(y, startOfYear);
            _startOfYearCache[index] = value;
        }

        return value.StartOfYear;
    }

    /// <summary>
    /// Counts the number of consecutive days from the epoch to the first day
    /// of the specified year (no cache).
    /// </summary>
    [Pure]
    protected virtual int GetStartOfYearCore(int y) => base.GetStartOfYear(y);
}
