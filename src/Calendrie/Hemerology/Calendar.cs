// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Core.Validation;
using Calendrie.Hemerology.Scopes;

/// <summary>
/// Represents a calendar and provides a base for derived classes.
/// <para>We do NOT assume the existence of a dedicated companion date type.
/// </para>
/// </summary>
public abstract partial class Calendar : ICalendar
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="Calendar"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException">One of the parameters is
    /// <see langword="null"/>.</exception>
    protected Calendar(string name, CalendarScope scope)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(scope);

        Name = name;
        Scope = scope;
        Schema = scope.Schema;
        YearsValidator = scope.YearsValidator;
    }

    /// <summary>
    /// Gets the culture-independent name of the calendar.
    /// </summary>
    public string Name { get; }

    /// <inheritdoc />
    public DayNumber Epoch => Scope.Epoch;

    /// <inheritdoc />
    public CalendricalAlgorithm Algorithm => Schema.Algorithm;

    /// <inheritdoc />
    public CalendricalFamily Family => Schema.Family;

    /// <inheritdoc />
    public CalendricalAdjustments PeriodicAdjustments => Schema.PeriodicAdjustments;

    /// <inheritdoc />
    public Range<DayNumber> Domain => Scope.Domain;

    /// <inheritdoc />
    public CalendarScope Scope { get; }

    //CalendarScope ICalendar.Scope => Scope;

    /// <summary>
    /// Gets a validator for the range of supported years.
    /// </summary>
    protected internal IYearsValidator YearsValidator { get; }

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    protected internal ICalendricalSchema Schema { get; }

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString() => Name;

    /// <inheritdoc />
    [Pure]
    public bool IsRegular(out int monthsInYear) => Schema.IsRegular(out monthsInYear);
}

public partial class Calendar // Year, month, day infos
{
#pragma warning disable CA1725 // Parameter names should match base declaration (Naming) ✓
    // Base parameter names (y, m, d) are not explicit enough.

    /// <inheritdoc />
    /// <exception cref="AoorException"><paramref name="year"/> is outside the
    /// range of supported years.</exception>
    [Pure]
    public bool IsLeapYear(int year)
    {
        YearsValidator.Validate(year);
        return Schema.IsLeapYear(year);
    }

    /// <inheritdoc />
    /// <exception cref="AoorException">The month is either invalid or outside
    /// the range of supported months.</exception>
    [Pure]
    public bool IsIntercalaryMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return Schema.IsIntercalaryMonth(year, month);
    }

    /// <inheritdoc />
    /// <exception cref="AoorException">The date is either invalid or outside
    /// the range of supported dates.</exception>
    [Pure]
    public bool IsIntercalaryDay(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);
        return Schema.IsIntercalaryDay(year, month, day);
    }

    /// <inheritdoc />
    /// <exception cref="AoorException">The date is either invalid or outside
    /// the range of supported dates.</exception>
    [Pure]
    public bool IsSupplementaryDay(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);
        return Schema.IsSupplementaryDay(year, month, day);
    }

    // Les méthodes suivantes sont abstraites car une année ou un mois peut être
    // incomplet.

    /// <inheritdoc />
    /// <exception cref="AoorException">The year is outside the range of
    /// supported years.</exception>
    [Pure] public abstract int CountMonthsInYear(int year);

    /// <inheritdoc />
    /// <exception cref="AoorException">The year is outside the range of
    /// supported years.</exception>
    [Pure] public abstract int CountDaysInYear(int year);

    /// <inheritdoc />
    /// <exception cref="AoorException">The month is either invalid or outside
    /// the range of supported months.</exception>
    [Pure] public abstract int CountDaysInMonth(int year, int month);

#pragma warning restore CA1725
}

public partial class Calendar // Conversions
{
    /// <inheritdoc />
    [Pure]
    public DayNumber GetDayNumber(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);
        return Epoch + Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <inheritdoc />
    [Pure]
    public DayNumber GetDayNumber(int year, int dayOfYear)
    {
        Scope.ValidateOrdinal(year, dayOfYear);
        return Epoch + Schema.CountDaysSinceEpoch(year, dayOfYear);
    }
}
