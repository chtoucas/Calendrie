// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Core.Validation;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Represents the proleptic short scope of the Gregorian schema.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static class GregorianProlepticScope
{
    /// <summary>
    /// Represents the earliest supported year.
    /// <para>This field is a constant equal to -9998.</para>
    /// </summary>
    public const int MinYear = ProlepticScope.MinYear;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 9999.</para>
    /// </summary>
    public const int MaxYear = ProlepticScope.MaxYear;

    /// <summary>
    /// Represents the minimum possible value for the number of consecutive days
    /// from the epoch.
    /// </summary>
    private static readonly int s_MinDaysSinceEpoch = GregorianFormulae.GetStartOfYear(MinYear);

    /// <summary>
    /// Represents the maximum possible value for the number of consecutive days
    /// from the epoch.
    /// </summary>
    private static readonly int s_MaxDaysSinceEpoch = GregorianFormulae.GetEndOfYear(MaxYear);

    /// <summary>
    /// Gets the range of supported <see cref="DayNumber"/> values by the
    /// <i>Gregorian</i> calendar, the one using the default epoch i.e.
    /// <see cref="DayZero.NewStyle"/> .
    /// <para>This static propery is thread-safe.</para>
    /// </summary>
    //
    //[Obsolete("To be removed")]
    public static Range<DayNumber> DefaultDomain { get; } =
        Range.Create(
            DayZero.NewStyle + s_MinDaysSinceEpoch,
            DayZero.NewStyle + s_MaxDaysSinceEpoch);

    /// <summary>
    /// Gets the validator for the range of supported days.
    /// <para>This static propery is thread-safe.</para>
    /// </summary>
    //
    //[Obsolete("To be removed")]
    public static DaysValidator DaysValidator { get; } =
        new(Range.Create(s_MinDaysSinceEpoch, s_MaxDaysSinceEpoch));

    /// <summary>
    /// Gets the validator for the range of supported years.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    //
    //[Obsolete("To be removed")]
    public static IYearsValidator YearsValidator => ProlepticScope.YearsValidatorImpl;

    /// <summary>
    /// Validates the specified month.
    /// </summary>
    /// <exception cref="AoorException">The validation failed.</exception>
    public static void ValidateYearMonth(int year, int month, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year, paramName);
        if (month < 1 || month > Solar12.MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
    }

    /// <summary>
    /// Validates the specified date.
    /// </summary>
    /// <exception cref="AoorException">The validation failed.</exception>
    public static void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year, paramName);
        if (month < 1 || month > Solar12.MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
        if (day < 1
            || day > Solar.MinDaysInMonth
                && day > GregorianFormulae.CountDaysInMonth(year, month))
        {
            ThrowHelpers.ThrowDayOutOfRange(day, paramName);
        }
    }

    /// <summary>
    /// Validates the specified ordinal date.
    /// </summary>
    /// <exception cref="AoorException">The validation failed.</exception>
    public static void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year, paramName);
        if (dayOfYear < 1
            || dayOfYear > Solar.MinDaysInYear
                && dayOfYear > GregorianFormulae.CountDaysInYear(year))
        {
            ThrowHelpers.ThrowDayOfYearOutOfRange(dayOfYear, paramName);
        }
    }
}
