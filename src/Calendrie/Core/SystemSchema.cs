﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using Calendrie.Core.Intervals;

#region Developer Notes

// SystemSchema puts limits on the range of admissible values for the year
// but more importantly also for the month of the year and the day of the
// month, therefore it cannot represent schemas with __unusually long years
// or months__.
//
// This class is public but has an internal ctor since we cannot guarantee
// that a derived class follows the rules defined above.
//
// Les limites MinMinYear/MaxMaxYear ont été fixées afin de pouvoir
// utiliser Yemoda & co, mais aussi d'éviter des dépassements
// arithmétiques. Sans cela on pourrait parfois aller beaucoup plus loin
// (à condition de rester dans les limites de Int32), d'où l'interface
// ICalendricalSchema permettant de définir des schémas dépourvus des
// contraintes liées à Yemoda & co.
// Il est à noter qu'en pratique on peut très bien ignorer ces derniers.
// Ceci est plus important qu'il n'y paraît, car Yemoda impose aussi des
// limites sur les mois et les jours, ce qui pourrait être gênant si on
// décide d'écrire des schémas pour les calendriers chinois ou maya.
//
// Par défaut, on n'utilise pas Yemoda.Min/MaxYear: les valeurs sont trop
// grandes et nous obligerait à utiliser des Int64 pour effectuer un
// certain nombre de calculs.
//
// Enfin, ces limites sont tout à fait théoriques. Un schéma n'est pas
// un calendrier. Pour ces derniers, on utilisera des valeurs bien
// inférieures (voir scopes).

#endregion

/// <summary>Represents a system schema and provides a base for derived classes.
/// <para>This class can ONLY be initialized from within friend assemblies.</para>
/// <para>All results SHOULD be representable by the system; see <see cref="Yemoda"/>,
/// <see cref="Yemo"/> and <see cref="Yedoy"/>.</para></summary>
public abstract partial class SystemSchema : CalendricalSchema
{
    /// <summary>Represents the default value for the earliest supported year.
    /// <para>This field is a constant equal to -999_998.</para></summary>
    private protected const int DefaultMinYear = -999_998;

    /// <summary>Represents the default value for the latest supported year.
    /// <para>This field is a constant equal to 999_999.</para></summary>
    private protected const int DefaultMaxYear = 999_999;

    /// <summary>Called from constructors in derived classes to initialize the
    /// <see cref="SystemSchema"/> class.
    /// <para>All methods MUST work with years in <see cref="DefaultSupportedYears"/>. In particular,
    /// methods must work with negative years.</para></summary>
    /// <exception cref="AoorException"><paramref name="minDaysInYear"/> or
    /// <paramref name="minDaysInMonth"/> is a negative integer.</exception>
    private protected SystemSchema(int minDaysInYear, int minDaysInMonth)
        : this(DefaultSupportedYears, minDaysInYear, minDaysInMonth) { }

    /// <summary>Called from constructors in derived classes to initialize the
    /// <see cref="SystemSchema"/> class.</summary>
    /// <exception cref="AoorException"><paramref name="supportedYears"/> is NOT a subinterval
    /// of <see cref="Yemoda.SupportedYears"/>.</exception>
    /// <exception cref="AoorException"><paramref name="minDaysInYear"/> or
    /// <paramref name="minDaysInMonth"/> is a negative integer.</exception>
    private protected SystemSchema(Range<int> supportedYears, int minDaysInYear, int minDaysInMonth)
        : base(supportedYears, minDaysInYear, minDaysInMonth)
    {
        if (supportedYears.IsSubsetOf(Yemoda.SupportedYears) == false)
        {
            Throw.ArgumentOutOfRange(nameof(supportedYears));
        }
    }

    /// <summary>Gets the default value for <see cref="ICalendricalSchema.SupportedYears"/>, that is
    /// the interval [-999_998..999_999].
    /// <para>This static property is thread-safe.</para></summary>
    public static Range<int> DefaultSupportedYears { get; } = new(DefaultMinYear, DefaultMaxYear);

    /// <summary>Gets the maximum value for <see cref="ICalendricalSchema.SupportedYears"/>, that is
    /// the interval [<see cref="Yemoda.MinYear"/>..<see cref="Yemoda.MaxYear"/>] i.e.
    /// [-2_097_151, 2_097_152].
    /// <para>It matches the value of <see cref="Yemoda.SupportedYears"/>.</para>
    /// <para>This static property is thread-safe.</para></summary>
    public static Range<int> MaxSupportedYears => Yemoda.SupportedYears;
}

public partial class SystemSchema // Properties
{
    private Range<int> _supportedYearsCore = Range.Maximal32;
    /// <summary>Gets the core domain, the interval of years for which the <i>core</i> methods are
    /// known not to overflow.
    /// <para>The core methods are those inherited from <see cref="ICalendricalKernel"/>.</para>
    /// <para>The default value is equal to the whole range of 32-bit signed integers.</para>
    /// <para>For methods expecting a month or day parameters, we assume that they are within
    /// the ranges defined by <see cref="Yemoda"/>.</para>
    /// <para>See also <seealso cref="ICalendricalPreValidator"/>.</para></summary>
    public Range<int> SupportedYearsCore
    {
        get => _supportedYearsCore;
        protected init
        {
            if (value.IsSupersetOf(SupportedYears) == false)
            {
                Throw.Argument(nameof(value));
            }
            _supportedYearsCore = value;
        }
    }

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsRegular(out int monthsInYear)
    {
        if (this is IRegularFeaturette sch)
        {
            monthsInYear = sch.MonthsInYear;
            return true;
        }
        else
        {
            monthsInYear = 0;
            return false;
        }
    }
}

public partial class SystemSchema // Conversions
{
    /// <summary>Obtains the date parts for the specified month count (the number of consecutive
    /// months from the epoch to a date).</summary>
    [Pure]
    // CIL code size = 19 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Yemo GetMonthParts(int monthsSinceEpoch)
    {
        GetMonthParts(monthsSinceEpoch, out int y, out int m);
        return new Yemo(y, m);
    }

    /// <summary>Obtains the date parts for the specified day count (the number of consecutive days
    /// from the epoch to a date).
    /// <para>See also <seealso cref="ICalendricalSchema.GetDateParts(int, out int, out int, out int)"/>.
    /// </para></summary>
    [Pure]
    // CIL code size = 22 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Yemoda GetDateParts(int daysSinceEpoch)
    {
        GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        return new Yemoda(y, m, d);
    }

    /// <summary>Obtains the ordinal date parts for the specified day count (the number of
    /// consecutive days from the epoch to a date).
    /// <para>See also <seealso cref="ICalendricalSchema.GetYear(int, out int)"/>.</para></summary>
    [Pure]
    // CIL code size = 16 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Yedoy GetOrdinalParts(int daysSinceEpoch)
    {
        int y = GetYear(daysSinceEpoch, out int doy);
        return new Yedoy(y, doy);
    }

    /// <summary>Obtains the date parts for the specified ordinal date.
    /// <para>See also <seealso cref="ICalendricalSchema.GetMonth(int, int, out int)"/>.</para>
    /// </summary>
    [Pure]
    // CIL code size = 20 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Yemoda GetDateParts(int y, int doy)
    {
        int m = GetMonth(y, doy, out int d);
        return new Yemoda(y, m, d);
    }

    /// <summary>Obtains the ordinal date parts for the specified date.</summary>
    // CIL code size = 18 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Yedoy GetOrdinalParts(int y, int m, int d)
    {
        int doy = GetDayOfYear(y, m, d);
        return new Yedoy(y, doy);
    }
}

public partial class SystemSchema // Dates in a given year or month
{
    //
    // Start of year
    //

    /// <summary>Obtains the month parts for the first month of the specified year.</summary>
    [Pure]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Box principle")]
    public Yemo GetMonthPartsAtStartOfYear(int y) => Yemo.AtStartOfYear(y);

    /// <summary>Obtains the date parts for the first day of the specified year.</summary>
    [Pure]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Box principle")]
    public Yemoda GetDatePartsAtStartOfYear(int y) => Yemoda.AtStartOfYear(y);

    /// <summary>Obtains the ordinal date parts for the first day of the specified year.</summary>
    [Pure]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Box principle")]
    public Yedoy GetOrdinalPartsAtStartOfYear(int y) => Yedoy.AtStartOfYear(y);

    //
    // End of year
    //

    /// <summary>Obtains the date parts for the last month of the specified year.</summary>
    [Pure]
    public Yemo GetMonthPartsAtEndOfYear(int y)
    {
        int monthsInYear = CountMonthsInYear(y);
        return new Yemo(y, monthsInYear);
    }

    /// <summary>Obtains the date parts for the last day of the specified year.</summary>
    [Pure]
    public Yemoda GetDatePartsAtEndOfYear(int y)
    {
        GetDatePartsAtEndOfYear(y, out int m, out int d);
        return new Yemoda(y, m, d);
    }

    /// <summary>Obtains the month and day of the month for the last day of the specified year; the
    /// results are given in output parameters.</summary>
    // The default implementation
    // > m = CountMonthsInYear(y);
    // > d = CountDaysInMonth(y, m);
    // is rather inefficient, indeed "m" and "d" are often constant.
    // For instance, for regular schemas, we can write:
    // > m = MonthsInYear;
    // > d = CountDaysInMonth(y, MonthsInYear);
    public abstract void GetDatePartsAtEndOfYear(int y, out int m, out int d);

    /// <summary>Obtains the ordinal date parts for the last day of the specified year.</summary>
    [Pure]
    public Yedoy GetOrdinalPartsAtEndOfYear(int y)
    {
        int doy = CountDaysInYear(y);
        return new Yedoy(y, doy);
    }

    //
    // Start of month
    //

    /// <summary>Obtains the date parts for the first day of the specified month.</summary>
    [Pure]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Box principle")]
    public Yemoda GetDatePartsAtStartOfMonth(int y, int m) => Yemoda.AtStartOfMonth(y, m);

    /// <summary>Obtains the ordinal date parts for the first day of the specified month.</summary>
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

    /// <summary>Obtains the date parts for the last day of the specified month.</summary>
    [Pure]
    public Yemoda GetDatePartsAtEndOfMonth(int y, int m)
    {
        int d = CountDaysInMonth(y, m);
        return new Yemoda(y, m, d);
    }

    /// <summary>Obtains the ordinal date parts for the last day of the specified month.</summary>
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
