﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

/// <summary>
/// Provides methods one can use to create new calendrical parts.
/// <para>This class assumes that input parameters are valid for the underlying
/// calendrical schema.</para>
/// </summary>
public sealed partial class PartsAdapter
{
    /// <summary>
    /// Represents the schema.
    /// </summary>
    private readonly ICalendricalSchema _schema;

    /// <summary>
    /// Initializes a new instance of the <see cref="PartsAdapter"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    internal PartsAdapter(ICalendricalSchema schema)
    {
        // The methods dot not check their parameters, therefore it's important
        // to keep the ctor internal.

        ArgumentNullException.ThrowIfNull(schema);

        _schema = schema;
    }
}

public partial class PartsAdapter // Conversions
{
    /// <summary>
    /// Obtains the date parts for the specified month count (the number of
    /// consecutive months from the epoch to a date).
    /// <para>This method does NOT check whether its parameter is valid or not.
    /// </para>
    /// </summary>
    [Pure]
    public MonthParts GetMonthParts(int monthsSinceEpoch)
    {
        _schema.GetMonthParts(monthsSinceEpoch, out int y, out int m);
        return new MonthParts(y, m);
    }

    /// <summary>
    /// Obtains the date parts for the specified day count (the number of
    /// consecutive days from the epoch to a date).
    /// <para>This method does NOT check whether its parameter is valid or not.
    /// </para>
    /// </summary>
    [Pure]
    public DateParts GetDateParts(int daysSinceEpoch)
    {
        _schema.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        return new DateParts(y, m, d);
    }

    /// <summary>
    /// Obtains the ordinal date parts for the specified day count (the number
    /// of consecutive days from the epoch to a date).
    /// <para>This method does NOT check whether its parameter is valid or not.
    /// </para>
    /// </summary>
    [Pure]
    public OrdinalParts GetOrdinalParts(int daysSinceEpoch)
    {
        int y = _schema.GetYear(daysSinceEpoch, out int doy);
        return new OrdinalParts(y, doy);
    }

    /// <summary>
    /// Obtains the ordinal date parts for the specified date.
    /// <para>This method does NOT check whether its parameters are valid or not.
    /// </para>
    /// </summary>
    [Pure]
    public OrdinalParts GetOrdinalParts(int y, int m, int d)
    {
        int doy = _schema.GetDayOfYear(y, m, d);
        return new OrdinalParts(y, doy);
    }

    /// <summary>
    /// Obtains the date parts for the specified ordinal date.
    /// <para>This method does NOT check whether its parameters are valid or not.
    /// </para>
    /// </summary>
    [Pure]
    public DateParts GetDateParts(int y, int doy)
    {
        int m = _schema.GetMonth(y, doy, out int d);
        return new DateParts(y, m, d);
    }
}

public partial class PartsAdapter // Dates in a given year or month
{
#if DEBUG
    /// <summary>
    /// Obtains the month parts for the first month of the specified year.
    /// </summary>
    [Pure]
    public static MonthParts GetMonthPartsAtStartOfYear(int y) => MonthParts.AtStartOfYear(y);

    /// <summary>
    /// Obtains the date parts for the first day of the specified year.
    /// </summary>
    [Pure]
    public static DateParts GetDatePartsAtStartOfYear(int y) => DateParts.AtStartOfYear(y);

    /// <summary>
    /// Obtains the ordinal date parts for the first day of the specified year.
    /// </summary>
    [Pure]
    public static OrdinalParts GetOrdinalPartsAtStartOfYear(int y) => OrdinalParts.AtStartOfYear(y);
#endif

    /// <summary>
    /// Obtains the date parts for the last month of the specified year.
    /// <para>This method does NOT check whether its parameter is valid or not.
    /// </para>
    /// </summary>
    [Pure]
    public MonthParts GetMonthPartsAtEndOfYear(int y)
    {
        int m = _schema.CountMonthsInYear(y);
        return new MonthParts(y, m);
    }

    /// <summary>
    /// Obtains the date parts for the last day of the specified year.
    /// <para>This method does NOT check whether its parameter is valid or not.
    /// </para>
    /// </summary>
    [Pure]
    public DateParts GetDatePartsAtEndOfYear(int y)
    {
        int m = _schema.CountMonthsInYear(y);
        int d = _schema.CountDaysInMonth(y, m);
        return new DateParts(y, m, d);
    }

    /// <summary>
    /// Obtains the ordinal date parts for the last day of the specified year.
    /// <para>This method does NOT check whether its parameter is valid or not.
    /// </para>
    /// </summary>
    [Pure]
    public OrdinalParts GetOrdinalPartsAtEndOfYear(int y)
    {
        int doy = _schema.CountDaysInYear(y);
        return new OrdinalParts(y, doy);
    }

#if DEBUG
    /// <summary>
    /// Obtains the date parts for the first day of the specified month.
    /// </summary>
    [Pure]
    public static DateParts GetDatePartsAtStartOfMonth(int y, int m) =>
        DateParts.AtStartOfMonth(y, m);
#endif

    /// <summary>
    /// Obtains the ordinal date parts for the first day of the specified month.
    /// <para>This method does NOT check whether its parameters are valid or not.
    /// </para>
    /// </summary>
    [Pure]
    public OrdinalParts GetOrdinalPartsAtStartOfMonth(int y, int m)
    {
        int doy = _schema.GetDayOfYear(y, m, 1);
        return new OrdinalParts(y, doy);
    }

    /// <summary>
    /// Obtains the date parts for the last day of the specified month.
    /// <para>This method does NOT check whether its parameters are valid or not.
    /// </para>
    /// </summary>
    [Pure]
    public DateParts GetDatePartsAtEndOfMonth(int y, int m)
    {
        int d = _schema.CountDaysInMonth(y, m);
        return new DateParts(y, m, d);
    }

    /// <summary>
    /// Obtains the ordinal date parts for the last day of the specified month.
    /// <para>This method does NOT check whether its parameters are valid or not.
    /// </para>
    /// </summary>
    [Pure]
    public OrdinalParts GetOrdinalPartsAtEndOfMonth(int y, int m)
    {
        int d = _schema.CountDaysInMonth(y, m);
        int doy = _schema.GetDayOfYear(y, m, d);
        return new OrdinalParts(y, doy);
    }
}
