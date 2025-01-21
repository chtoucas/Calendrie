// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using Calendrie.Core.Intervals;

/// <summary>
/// Represents a limit schema and provides a base for derived classes.
/// <para>A limit schema impose limits on the range of date parts. All results
/// SHOULD be representable by the <see cref="Yemoda"/>, <see cref="Yemo"/> and
/// <see cref="Yedoy"/> types.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.
/// </para>
/// </summary>
public abstract partial class LimitSchema : CalendricalSchema
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="LimitSchema"/> class.
    /// <para>All methods MUST work with years in
    /// <see cref="CalendricalSchema.DefaultSupportedYears"/>.
    /// In particular, methods must work with negative years.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minDaysInYear"/>
    /// or <paramref name="minDaysInMonth"/> is a negative integer.</exception>
    private protected LimitSchema(int minDaysInYear, int minDaysInMonth)
        : this(DefaultSupportedYears, minDaysInYear, minDaysInMonth) { }

    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="LimitSchema"/> class.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="supportedYears"/>
    /// is not a subinterval of <see cref="Yemoda.SupportedYears"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minDaysInYear"/>
    /// or <paramref name="minDaysInMonth"/> is a negative integer.</exception>
    private protected LimitSchema(Range<int> supportedYears, int minDaysInYear, int minDaysInMonth)
        : base(supportedYears, minDaysInYear, minDaysInMonth) { }
}

public partial class LimitSchema // Conversions
{
    /// <summary>
    /// Obtains the date parts for the specified month count (the number of
    /// consecutive months from the epoch to a date).
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Yemo GetMonthParts(int monthsSinceEpoch)
    {
        GetMonthParts(monthsSinceEpoch, out int y, out int m);
        return new Yemo(y, m);
    }

    /// <summary>
    /// Obtains the date parts for the specified day count (the number of
    /// consecutive days from the epoch to a date).
    /// <para>See also
    /// <seealso cref="ICalendricalSchema.GetDateParts(int, out int, out int, out int)"/>.
    /// </para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Yemoda GetDateParts(int daysSinceEpoch)
    {
        GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        return new Yemoda(y, m, d);
    }

    /// <summary>
    /// Obtains the ordinal date parts for the specified day count (the number
    /// of consecutive days from the epoch to a date).
    /// <para>See also <seealso cref="ICalendricalSchema.GetYear(int, out int)"/>.
    /// </para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Yedoy GetOrdinalParts(int daysSinceEpoch)
    {
        int y = GetYear(daysSinceEpoch, out int doy);
        return new Yedoy(y, doy);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Yemoda"/> struct from the
    /// specified year, month and day.
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Static would force us to validate the parameters")]
    public Yemoda GetDateParts(int y, int m, int d) => new(y, m, d);

    /// <summary>
    /// Obtains the date parts for the specified ordinal date.
    /// <para>See also
    /// <seealso cref="ICalendricalSchema.GetMonth(int, int, out int)"/>.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Yemoda GetDateParts(int y, int doy)
    {
        int m = GetMonth(y, doy, out int d);
        return new Yemoda(y, m, d);
    }

    /// <summary>
    /// Obtains the ordinal date parts for the specified date.
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Yedoy GetOrdinalParts(int y, int m, int d)
    {
        int doy = GetDayOfYear(y, m, d);
        return new Yedoy(y, doy);
    }
}

public partial class LimitSchema // Dates in a given year or month
{
    //
    // Start of year
    //

    /// <summary>
    /// Obtains the month parts for the first month of the specified year.
    /// </summary>
    [Pure]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Static would force us to validate the parameters")]
    public Yemo GetMonthPartsAtStartOfYear(int y) => Yemo.AtStartOfYear(y);

    /// <summary>
    /// Obtains the date parts for the first day of the specified year.
    /// </summary>
    [Pure]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Static would force us to validate the parameters")]
    public Yemoda GetDatePartsAtStartOfYear(int y) => Yemoda.AtStartOfYear(y);

    /// <summary>
    /// Obtains the ordinal date parts for the first day of the specified year.
    /// </summary>
    [Pure]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Static would force us to validate the parameters")]
    public Yedoy GetOrdinalPartsAtStartOfYear(int y) => Yedoy.AtStartOfYear(y);

    //
    // End of year
    //

    /// <summary>
    /// Obtains the date parts for the last month of the specified year.
    /// </summary>
    [Pure]
    public Yemo GetMonthPartsAtEndOfYear(int y)
    {
        int monthsInYear = CountMonthsInYear(y);
        return new Yemo(y, monthsInYear);
    }

    /// <summary>
    /// Obtains the date parts for the last day of the specified year.
    /// </summary>
    [Pure]
    public Yemoda GetDatePartsAtEndOfYear(int y)
    {
        GetDatePartsAtEndOfYear(y, out int m, out int d);
        return new Yemoda(y, m, d);
    }

    ///// <summary>
    ///// Obtains the month and day of the month for the last day of the specified
    ///// year; the results are given in output parameters.
    ///// </summary>
    ////
    //// The default implementation
    //// > m = CountMonthsInYear(y);
    //// > d = CountDaysInMonth(y, m);
    //// is rather inefficient, indeed "m" and "d" are often constant.
    //// For instance, for regular schemas, we can write:
    //// > m = MonthsInYear;
    //// > d = CountDaysInMonth(y, MonthsInYear);
    //public abstract void GetDatePartsAtEndOfYear(int y, out int m, out int d);

    /// <summary>
    /// Obtains the ordinal date parts for the last day of the specified year.
    /// </summary>
    [Pure]
    public Yedoy GetOrdinalPartsAtEndOfYear(int y)
    {
        int doy = CountDaysInYear(y);
        return new Yedoy(y, doy);
    }

    //
    // Start of month
    //

    /// <summary>
    /// Obtains the date parts for the first day of the specified month.
    /// </summary>
    [Pure]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Static would force us to validate the parameters")]
    public Yemoda GetDatePartsAtStartOfMonth(int y, int m) => Yemoda.AtStartOfMonth(y, m);

    /// <summary>
    /// Obtains the ordinal date parts for the first day of the specified month.
    /// </summary>
    [Pure]
    public Yedoy GetOrdinalPartsAtStartOfMonth(int y, int m)
    {
        // Conversion (y, m, d) -> (y, doy)
        // > int doy = GetDayOfYear(y, m, 1);
        int doy = CountDaysInYearBeforeMonth(y, m) + 1;
        return new Yedoy(y, doy);
    }

    //
    // End of month
    //

    /// <summary>
    /// Obtains the date parts for the last day of the specified month.
    /// </summary>
    [Pure]
    public Yemoda GetDatePartsAtEndOfMonth(int y, int m)
    {
        int d = CountDaysInMonth(y, m);
        return new Yemoda(y, m, d);
    }

    /// <summary>
    /// Obtains the ordinal date parts for the last day of the specified month.
    /// </summary>
    [Pure]
    public Yedoy GetOrdinalPartsAtEndOfMonth(int y, int m)
    {
        // Conversion (y, m, d) -> (y, doy)
        // > int d = CountDaysInMonth(y, m);
        // > int doy = GetDayOfYear(y, m, d);
        int doy = CountDaysInYearBeforeMonth(y, m) + CountDaysInMonth(y, m);
        return new Yedoy(y, doy);
    }
}
