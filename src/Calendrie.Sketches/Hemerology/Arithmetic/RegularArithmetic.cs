// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology.Arithmetic;

using Calendrie.Core;
using Calendrie.Core.Utilities;

/// <summary>
/// Provides a generic implementation for <see cref="NakedArithmetic"/> for when
/// the underlying schema is regular.
/// <para>The length of a month must be greater than or equal to 7.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class RegularArithmetic : NakedArithmetic
{
    /// <summary>
    /// Represents the absolute minimum value admissible for the minimum total
    /// number of days there is at least in a month.
    /// <para>This field is a constant equal to 7.</para>
    /// </summary>
    public const int MinMinDaysInMonth = 7;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegularArithmetic"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="segment"/> is
    /// null.</exception>
    /// <exception cref="ArgumentException"><paramref name="segment"/> is NOT
    /// complete.</exception>
    /// <exception cref="ArgumentException">The underlying schema contains at
    /// least one month whose length is strictly less than <see cref="MinMinDaysInMonth"/>.
    /// </exception>
    /// <exception cref="ArgumentException">The underlying schema is NOT
    /// regular.</exception>
    public RegularArithmetic(CalendricalSegment segment) : base(segment)
    {
        Debug.Assert(segment != null);
        if (!segment.IsComplete) throw new ArgumentException(null, nameof(segment));

        if (Schema.MinDaysInMonth < MinMinDaysInMonth)
            throw new ArgumentException(null, nameof(segment));
        if (!Schema.IsRegular(out int monthsInYear))
            throw new ArgumentException(null, nameof(segment));

        (MinYear, MaxYear) = segment.SupportedYears.Endpoints;

        MonthsInYear = monthsInYear;
        MinDaysInYear = Schema.MinDaysInYear;
        MinDaysInMonth = Schema.MinDaysInMonth;
    }

    /// <summary>
    /// Gets the total number of months in a year.
    /// </summary>
    private int MonthsInYear { get; }

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    private int MinYear { get; }

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    private int MaxYear { get; }

    private int MinDaysInYear { get; }
    private int MinDaysInMonth { get; }

    /// <summary>
    /// The operation would overflow the range of supported dates.
    /// </summary>
    /// <exception cref="OverflowException"/>
    [DoesNotReturn, Pure]
    private static T ThrowDateOverflow<T>() =>
        throw new OverflowException("The computation would overflow the range of supported dates.");
}

public partial class RegularArithmetic // Operations on DateParts
{
    /// <inheritdoc />
    [Pure]
    public sealed override DateParts AddDays(DateParts parts, int days)
    {
        // Fast tracks.
        var (y, m, d) = parts;

        // No change of month.
        int dom = checked(d + days);
        if (1 <= dom && (dom <= MinDaysInMonth || dom <= Schema.CountDaysInMonth(y, m)))
        {
            return new DateParts(y, m, dom);
        }

        if (-MinDaysInYear <= days && days <= MinDaysInYear)
        {
            int doy = Schema.GetDayOfYear(y, m, d);
            var (newY, newDoy) = AddDaysViaDayOfYear(new OrdinalParts(y, doy), days);
            return PartsAdapter.GetDateParts(newY, newDoy);
        }

        // Slow track.
        int daysSinceEpoch = checked(Schema.CountDaysSinceEpoch(y, m, d) + days);
        DaysSinceEpochChecker.CheckOverflow(daysSinceEpoch);

        return PartsAdapter.GetDateParts(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override DateParts NextDay(DateParts parts)
    {
        var (y, m, d) = parts;

        return
            d < MinDaysInMonth || d < Schema.CountDaysInMonth(y, m) ? new DateParts(y, m, d + 1)
            : m < MonthsInYear ? DateParts.AtStartOfMonth(y, m + 1)
            : y < MaxYear ? DateParts.AtStartOfYear(y + 1)
            : ThrowDateOverflow<DateParts>();
    }

    /// <inheritdoc />
    [Pure]
    public sealed override DateParts PreviousDay(DateParts parts)
    {
        var (y, m, d) = parts;

        return
            d > 1 ? new DateParts(y, m, d - 1)
            : m > 1 ? PartsAdapter.GetDatePartsAtEndOfMonth(y, m - 1)
            : y > MinYear ? PartsAdapter.GetDatePartsAtEndOfYear(y - 1)
            : ThrowDateOverflow<DateParts>();
    }
}

public partial class RegularArithmetic // Operations on OrdinalParts
{
    /// <inheritdoc />
    [Pure]
    public sealed override OrdinalParts AddDays(OrdinalParts parts, int days)
    {
        // Fast track.
        if (-MinDaysInYear <= days && days <= MinDaysInYear)
        {
            return AddDaysViaDayOfYear(parts, days);
        }

        var (y, doy) = parts;

        // Slow track.
        int daysSinceEpoch = checked(Schema.CountDaysSinceEpoch(y, doy) + days);
        DaysSinceEpochChecker.CheckOverflow(daysSinceEpoch);

        return PartsAdapter.GetOrdinalParts(daysSinceEpoch);
    }

    [Pure]
    private OrdinalParts AddDaysViaDayOfYear(OrdinalParts parts, int days)
    {
        Debug.Assert(-MinDaysInYear <= days);
        Debug.Assert(days <= MinDaysInYear);

        var (y, doy) = parts;

        // No need to use checked arithmetic here.
        doy += days;
        if (doy < 1)
        {
            if (y == MinYear) ThrowHelpers.ThrowDateOverflow();
            y--;
            doy += Schema.CountDaysInYear(y);
        }
        else
        {
            int daysInYear = Schema.CountDaysInYear(y);
            if (doy > daysInYear)
            {
                if (y == MaxYear) ThrowHelpers.ThrowDateOverflow();
                y++;
                doy -= daysInYear;
            }
        }

        return new OrdinalParts(y, doy);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override OrdinalParts NextDay(OrdinalParts parts)
    {
        var (y, doy) = parts;

        return
            doy < MinDaysInYear || doy < Schema.CountDaysInYear(y) ? new OrdinalParts(y, doy + 1)
            : y < MaxYear ? OrdinalParts.AtStartOfYear(y + 1)
            : ThrowDateOverflow<OrdinalParts>();
    }

    /// <inheritdoc />
    [Pure]
    public sealed override OrdinalParts PreviousDay(OrdinalParts parts)
    {
        var (y, doy) = parts;

        return doy > 1 ? new OrdinalParts(y, doy - 1)
            : y > MinYear ? PartsAdapter.GetOrdinalPartsAtEndOfYear(y - 1)
            : ThrowDateOverflow<OrdinalParts>();
    }
}

public partial class RegularArithmetic // Operations on MonthParts
{
    /// <inheritdoc />
    [Pure]
    public sealed override MonthParts AddMonths(MonthParts parts, int months)
    {
        var (y, m) = parts;

        m = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsInYear, out int y0);
        y += y0;
        YearsValidator.Validate(y);

        return new MonthParts(y, m);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override MonthParts NextMonth(MonthParts parts) => AddMonths(parts, 1);

    /// <inheritdoc />
    [Pure]
    public sealed override MonthParts PreviousMonth(MonthParts parts) => AddMonths(parts, -1);
}

public partial class RegularArithmetic // Non-standard operations
{
    /// <summary>
    /// Adds a number of years to the year field of the specified date.
    /// </summary>
    /// <returns>The end of the target month (resp. year) when the naive result
    /// is not a valid day (resp. month).</returns>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported values.</exception>
    [Pure]
    public DateParts AddYears(DateParts parts, int years, out int roundoff)
    {
        var (y, m, d) = parts;

        y = checked(y + years);
        YearsValidator.CheckOverflow(y);

        int daysInMonth = Schema.CountDaysInMonth(y, m);
        roundoff = Math.Max(0, d - daysInMonth);
        return new DateParts(y, m, roundoff > 0 ? daysInMonth : d);
    }

    /// <summary>
    /// Adds a number of months to the specified date.
    /// </summary>
    /// <returns>The last day of the month when the naive result is not a valid
    /// day (roundoff > 0).</returns>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported values.</exception>
    [Pure]
    public DateParts AddMonths(DateParts parts, int months, out int roundoff)
    {
        var (y, m, d) = parts;

        m = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsInYear, out int y0);
        y += y0;
        YearsValidator.CheckOverflow(y);

        int daysInMonth = Schema.CountDaysInMonth(y, m);
        roundoff = Math.Max(0, d - daysInMonth);
        return new DateParts(y, m, roundoff > 0 ? daysInMonth : d);
    }

    /// <summary>
    /// Adds a number of years to the year field of the specified ordinal date.
    /// </summary>
    /// <returns>The last day of the year when the naive result is not a valid
    /// day (roundoff > 0).</returns>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported values.</exception>
    [Pure]
    public OrdinalParts AddYears(OrdinalParts parts, int years, out int roundoff)
    {
        var (y, doy) = parts;

        y = checked(y + years);
        YearsValidator.CheckOverflow(y);

        int daysInYear = Schema.CountDaysInYear(y);
        roundoff = Math.Max(0, doy - daysInYear);
        return new OrdinalParts(y, roundoff > 0 ? daysInYear : doy);
    }

    /// <summary>
    /// Adds a number of years to the year field of the specified month.
    /// </summary>
    /// <returns>The last month of the year when the naive result is not a valid
    /// month (roundoff > 0).</returns>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported values.</exception>
    [Pure]
    public MonthParts AddYears(MonthParts parts, int years, out int roundoff)
    {
        var (y, m) = parts;

        y = checked(y + years);
        YearsValidator.CheckOverflow(y);

        roundoff = 0;
        return new MonthParts(y, m);
    }
}
