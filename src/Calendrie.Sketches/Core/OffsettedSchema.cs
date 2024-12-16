// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using Calendrie.Core.Intervals;
using Calendrie.Core.Validation;

public static class OffsettedSchema
{
    public static OffsettedSchema<TSchema> Create<TSchema>(int offset)
        where TSchema : ICalendricalSchema, ISchemaActivator<TSchema>
    {
        return new OffsettedSchema<TSchema>(TSchema.CreateInstance(), offset);
    }
}

public partial class OffsettedSchema<TSchema> : ICalendricalSchema
    where TSchema : ICalendricalSchema
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OffsettedSchema{TSchema}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.
    /// </exception>
    public OffsettedSchema(TSchema schema, int offset)
    {
        ArgumentNullException.ThrowIfNull(schema);

        Schema = schema;
        Offset = offset;
    }

    public int Offset { get; }

    /// <summary>
    /// Gets the plain schema.
    /// <para>This field is read-ony.</para>
    /// </summary>
    protected TSchema Schema { get; }
}

public partial class OffsettedSchema<TSchema> // ICalendar
{
    /// <inheritdoc />
    public CalendricalAlgorithm Algorithm => Schema.Algorithm;

    /// <inheritdoc />
    public CalendricalFamily Family => Schema.Family;

    /// <inheritdoc />
    public CalendricalAdjustments PeriodicAdjustments => Schema.PeriodicAdjustments;

    /// <inheritdoc />
    [Pure]
    public bool IsRegular(out int monthsInYear) => Schema.IsRegular(out monthsInYear);

    /// <inheritdoc />
    [Pure]
    public bool IsLeapYear(int y) => Schema.IsLeapYear(y - Offset);

    /// <inheritdoc />
    [Pure]
    public bool IsIntercalaryMonth(int y, int m) => Schema.IsIntercalaryMonth(y - Offset, m);

    /// <inheritdoc />
    [Pure]
    public bool IsIntercalaryDay(int y, int m, int d) => Schema.IsIntercalaryDay(y - Offset, m, d);

    /// <inheritdoc />
    [Pure]
    public bool IsSupplementaryDay(int y, int m, int d) =>
        Schema.IsSupplementaryDay(y - Offset, m, d);

    /// <inheritdoc />
    [Pure]
    public int CountMonthsInYear(int y) => Schema.CountMonthsInYear(y - Offset);

    /// <inheritdoc />
    [Pure]
    public int CountDaysInYear(int y) => Schema.CountDaysInYear(y - Offset);

    /// <inheritdoc />
    [Pure]
    public int CountDaysInMonth(int y, int m) => Schema.CountDaysInMonth(y - Offset, m);
}

public partial class OffsettedSchema<TSchema> // ICalendricalSchema
{
    /// <inheritdoc />
    public int MinDaysInYear => Schema.MinDaysInYear;

    /// <inheritdoc />
    public int MinDaysInMonth => Schema.MinDaysInMonth;

    private Range<int>? _supportedDays;
    /// <inheritdoc />
    public Range<int> SupportedDays =>
        _supportedDays ??= new Range<int>(
            SupportedYears.Endpoints.Select(GetStartOfYear, GetEndOfYear));

    private Range<int>? _supportedMonths;
    /// <inheritdoc />
    public Range<int> SupportedMonths =>
        _supportedMonths ??= new Range<int>(
            SupportedYears.Endpoints.Select(GetStartOfYearInMonths, GetEndOfYearInMonths));

    /// <inheritdoc />
    public Range<int> SupportedYears => Schema.SupportedYears;

    /// <inheritdoc />
    public ICalendricalPreValidator PreValidator => Schema.PreValidator;

    //
    // Counting months and days within a year or a month
    //

    /// <inheritdoc />
    [Pure]
    public int CountDaysInYearBeforeMonth(int y, int m) =>
        Schema.CountDaysInYearBeforeMonth(y - Offset, m);

    //
    // Conversions
    //

    /// <inheritdoc />
    [Pure]
    public int CountMonthsSinceEpoch(int y, int m) => Schema.CountMonthsSinceEpoch(y - Offset, m);

    /// <inheritdoc />
    [Pure]
    public int CountDaysSinceEpoch(int y, int m, int d) =>
        Schema.CountDaysSinceEpoch(y - Offset, m, d);

    /// <inheritdoc />
    [Pure]
    public int CountDaysSinceEpoch(int y, int doy) => Schema.CountDaysSinceEpoch(y - Offset, doy);

    /// <inheritdoc />
    public void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
    {
        Schema.GetMonthParts(monthsSinceEpoch, out y, out m);
        y += Offset;
    }

    /// <inheritdoc />
    public void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        Schema.GetDateParts(daysSinceEpoch, out y, out m, out d);
        y += Offset;
    }

    /// <inheritdoc />
    [Pure]
    public int GetYear(int daysSinceEpoch, out int doy) =>
        Offset + Schema.GetYear(daysSinceEpoch, out doy);

    /// <inheritdoc />
    [Pure]
    public int GetMonth(int y, int doy, out int d) => Schema.GetMonth(y - Offset, doy, out d);

    /// <inheritdoc />
    [Pure]
    public int GetDayOfYear(int y, int m, int d) => Schema.GetDayOfYear(y - Offset, m, d);

    //
    // Counting months and days since the epoch
    //

    /// <inheritdoc />
    [Pure]
    public int GetStartOfYearInMonths(int y) => Schema.GetStartOfYearInMonths(y - Offset);

    /// <inheritdoc />
    [Pure]
    public int GetEndOfYearInMonths(int y) => Schema.GetEndOfYearInMonths(y - Offset);

    /// <inheritdoc />
    [Pure]
    public int GetStartOfYear(int y) => Schema.GetStartOfYear(y - Offset);

    /// <inheritdoc />
    [Pure]
    public int GetEndOfYear(int y) => Schema.GetEndOfYear(y - Offset);

    /// <inheritdoc />
    [Pure]
    public int GetStartOfMonth(int y, int m) => Schema.GetStartOfMonth(y - Offset, m);

    /// <inheritdoc />
    [Pure]
    public int GetEndOfMonth(int y, int m) => Schema.GetEndOfMonth(y - Offset, m);
}
