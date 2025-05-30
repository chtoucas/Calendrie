﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using Calendrie.Core.Intervals;

using static Calendrie.Core.CalendricalConstants;

// WARNING: a schema should avoid public static methods since it does not validate
// the input parameters of a method. For the same reason, prefer computational
// formulae to arrays or spans.

#region Developer Notes

// CalendricalSchema is one way to implement an arithmetical schema,
// but surely there are others.
//
// Contract
// --------
// There is a hidden contract stating that:
// - the system only passes valid parameters from a calendrical point of
//   view. In particular, there is no need for a method to validate its
//   parameters.
// - a schema MUST produce valid data from a calendrical point of view.
// - finally, all methods MUST not overflow with years within the range of
//   years defined by the schema.
//
// Derived classes
// ---------------
// In practice, only four methods may require non-trivial work:
// - IsLeapYear()
// - GetYear()
// - GetMonth()
// - GetStartOfYear()
// Of course, this is not true if we override the virtual methods.
//
// Speaking of virtual methods, we expect that most classes will override
// CountDaysSinceEpoch() and GetDateParts(), while they won't touch GetYear().
//
// If possible, derived classes should implement CountDaysInYearBeforeMonth()
// and CountDaysInMonth() without resorting to table lookups.
// Actually, for CountDaysInMonth(), it is more or less an hidden contract.
//
// Performance
// -----------
// Validation:
// - CountDaysInMonth()
// - CountDaysInYear()
// There is also CountMonthsInYear() BUT most schemas implement IRegularSchema.
//
// Other methods to pay attention to:
// - IsLeapYear()
// - CountDaysInYearBeforeMonth()
// - CountDaysSinceEpoch()
//   This one is particularly important. We use it for interconversion, math
//   operations, computation or adjustment of the day of the week.
// - GetDateParts()
//   Reverse of CountDaysSinceEpoch().
// - GetYear()
//   Mostly if we don't override GetYear(y, out doy).
// - GetStartOfYear()
//
// Access modifiers
// ----------------
// To limit the scope of schemas, all final schemas SHOULD NOT expose a
// public ctor (internal is OK) --- the schemas themselves can't be internal
// if we want to be able to define strongly-typed calendars (e.g. see
// Calendar). Of course, if you don't intend to publish your
// schema, you can ignore this recommendation.
//
// Pre-defined schemas provide a static "factory" method:
//   public static Box<MySchema> GetInstance() =>
//       Box.Of(new MySchema());
// This way, schemas are public but one can NOT use instance methods
// regardless of their access modifiers.
//
// Static methods CAN be public but only if they are guaranteed to work
// whatever input we give to them (overflows).
//
// Design
// ------
// To be able to extend this class outside this project, we ensure that
// no abstract or virtual method returns a Yemoda/Yemo/Yedoy --- no public
// ctor, just a factory method that does unnecessary param check if the
// extender respects the contract defined below: the results must be
// representable by the system.
//
// If we had only one base class for all calendars, we would have used the
// Template Method Pattern, but we don't. In other words, we would have not
// needed a separate class hierarchy from Calendar.
//
// It would have been possible to provide default implementations for:
// - CountDaysInYearBeforeMonth()
// - GetEndOfYearParts()
// but better to left them out since they would have been very inefficient.
//
// Do NOT use Yemoda as input. It has a major drawback, we could pack/unpack
// the same data multiple times without even realizing it.
//
// What's not here
// ---------------
// - GetDayOfWeek()
//   Missing requirement: an epoch.
// - Everything arithmetic
//   Missing requirement: a "scope".
// All this will be part of a calendar or a date object.
//
// Annotations
// -----------
// A : abstract
// V : virtual

#endregion

// CalendricalSchema puts limits on the range of admissible values for the year,
// therefore it should not be used to represent schemas with __unusually long
// years__. These limits are not enforced beyond the ctor, therefore they are
// purely informative.
//
// Les limites DefaultMinYear/DefaultMaxYear ont été fixées afin de pouvoir
// utiliser Yemoda & co, mais aussi afin d'éviter des dépassements
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

/// <summary>
/// Represents a calendrical schema and provides a base for derived classes.
/// </summary>
public abstract partial class CalendricalSchema : ICalendricalSchema
{
    /// <summary>
    /// Represents the default value for the earliest supported year.
    /// <para>This field is a constant equal to -999_998.</para>
    /// </summary>
    private protected const int DefaultMinYear = -999_998;

    /// <summary>
    /// Represents the default value for the latest supported year.
    /// <para>This field is a constant equal to 999_999.</para>
    /// </summary>
    private protected const int DefaultMaxYear = 999_999;

    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="CalendricalSchema"/> class.
    /// <para>All methods MUST work with years in <see cref="DefaultSupportedYears"/>.
    /// In particular, methods must work with negative years.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minDaysInYear"/>
    /// or <paramref name="minDaysInMonth"/> is a negative integer.</exception>
    protected CalendricalSchema(int minDaysInYear, int minDaysInMonth)
       : this(DefaultSupportedYears, minDaysInYear, minDaysInMonth) { }

    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="CalendricalSchema"/> class.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="supportedYears"/>
    /// is not a subinterval of <see cref="MaxSupportedYears"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minDaysInYear"/>
    /// or <paramref name="minDaysInMonth"/> is a negative integer.</exception>
    protected CalendricalSchema(Segment<int> supportedYears, int minDaysInYear, int minDaysInMonth)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(minDaysInYear, 0);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(minDaysInMonth, 0);
        if (!supportedYears.IsSubsetOf(MaxSupportedYears))
        {
            throw new ArgumentException(
                "The value was not a subinterval of Yemoda.SupportedYears.",
                nameof(supportedYears));
        }

        SupportedYears = supportedYears;
        MinDaysInYear = minDaysInYear;
        MinDaysInMonth = minDaysInMonth;
    }

    /// <summary>
    /// Gets the default value for <see cref="ICalendricalSchema.SupportedYears"/>,
    /// that is the interval [-999_998..999_999].
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Segment<int> DefaultSupportedYears { get; } = new(DefaultMinYear, DefaultMaxYear);

    /// <summary>
    /// Gets the maximum value for <see cref="ICalendricalSchema.SupportedYears"/>,
    /// that is the interval [-2_097_151, 2_097_152].
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    //
    // It matches the value of <see cref="Yemoda.SupportedYears"/>.
    //public static Segment<int> MaxSupportedYears => Yemoda.SupportedYears;
    public static Segment<int> MaxSupportedYears { get; } = Segment.Create(1 - (1 << 21), 1 << 21);

    private Segment<int> _supportedYearsCore = Segment.Maximal32;
    /// <summary>
    /// Gets the core domain, the interval of years for which the <i>core</i>
    /// methods are known not to overflow.
    /// <para>The core methods are those being part of <see cref="ICalendricalCore"/>.
    /// </para>
    /// <para>The default value is equal to the whole range of 32-bit signed
    /// integers.</para>
    /// <para>For methods expecting a month parameters, we assume that
    /// they are within the range [1..16].</para>
    /// <para>For methods expecting a month or day parameters, we assume that
    /// they are within the range [1..64].</para>
    /// <para>See also <seealso cref="ICalendricalPreValidator"/>.</para>
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="value"/>
    /// is not a subinterval of <see cref="SupportedYears"/>.
    /// </exception>
    //
    // The fixed limits for the month and day parameters are related to Yemoda.
    public Segment<int> SupportedYearsCore
    {
        get => _supportedYearsCore;
        protected init
        {
            if (!value.IsSupersetOf(SupportedYears))
            {
                throw new ArgumentException(
                    "The value was not a subinterval of CalendricalSchema.SupportedYears.",
                    nameof(value));
            }

            _supportedYearsCore = value;
        }
    }

    private CalendricalProfile? _profile;
    /// <summary>
    /// Gets the schema profile.
    /// </summary>
    internal CalendricalProfile Profile => _profile ??= FindProfile();

    [Pure]
    private CalendricalProfile FindProfile()
    {
        // The schema is not regular iff monthsInYear = 0.
        _ = IsRegular(out int monthsInYear);

        // WARNING: the order is important, higher values of MinDaysInYear
        // MUST come first, then those of MinDaysInMonth.
        return this switch
        {
            {
                MinDaysInYear: >= Solar.MinDaysPerYear,      // 365
                MinDaysInMonth: >= Solar.MinDaysPerMonth,    // 28
            } => monthsInYear switch
            {
                Solar12.MonthsPerYear => CalendricalProfile.Solar12,
                Solar13.MonthsPerYear => CalendricalProfile.Solar13,
                // Notice that MinDaysInMonth >= 7.
                _ => CalendricalProfile.Other,
            },

            {
                MinDaysInYear: >= Lunar.MinDaysPerYear,      // 354
                MinDaysInMonth: >= Lunar.MinDaysPerMonth,    // 29
            } => monthsInYear == Lunar.MonthsPerYear
                ? CalendricalProfile.Lunar
                // Notice that MinDaysInMonth >= 7.
                : CalendricalProfile.Other,

            {
                MinDaysInYear: >= Lunisolar.MinDaysPerYear,  // 353
                MinDaysInMonth: >= Lunisolar.MinDaysPerMonth,// 29
            } => monthsInYear == 0
                ? CalendricalProfile.Lunisolar
                // Notice that MinDaysInMonth >= 7.
                : CalendricalProfile.Other,

            _ => CalendricalProfile.Other,
        };
    }
}

public partial class CalendricalSchema // Properties
{
    /// <inheritdoc />
    public CalendricalAlgorithm Algorithm => CalendricalAlgorithm.Arithmetical;

    /// <inheritdoc />
    public abstract CalendricalFamily Family { get; }

    /// <inheritdoc />
    public abstract CalendricalAdjustments PeriodicAdjustments { get; }

    /// <inheritdoc />
    public int MinDaysInYear { get; }

    /// <inheritdoc />
    public int MinDaysInMonth { get; }

    /// <inheritdoc />
    public Segment<int> SupportedYears { get; }

    private Segment<int>? _domain;
    /// <inheritdoc />
    public Segment<int> SupportedDays =>
        _domain ??= new Segment<int>(SupportedYears.Endpoints.Select(GetStartOfYear, GetEndOfYear));

    private Segment<int>? _supportedMonths;
    /// <inheritdoc />
    public Segment<int> SupportedMonths =>
        _supportedMonths ??=
        new Segment<int>(SupportedYears.Endpoints.Select(GetStartOfYearInMonths, GetEndOfYearInMonths));

    /// <inheritdoc />
    [Pure]
    public abstract bool IsRegular(out int monthsInYear);

    private ICalendricalPreValidator? _validator;
    /// <inheritdoc />
    public ICalendricalPreValidator PreValidator
    {
        get => _validator ??= ICalendricalPreValidator.CreateDefault(this);
        protected init
        {
            ArgumentNullException.ThrowIfNull(value);
            _validator = value;
        }
    }
}

public partial class CalendricalSchema // Year, month or day infos
{
    /// <inheritdoc />
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure] public abstract bool IsLeapYear(int y);

    /// <inheritdoc />
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure] public abstract bool IsIntercalaryMonth(int y, int m);

    /// <inheritdoc />
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure] public abstract bool IsIntercalaryDay(int y, int m, int d);

    /// <inheritdoc />
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure] public abstract bool IsSupplementaryDay(int y, int m, int d);
}

public partial class CalendricalSchema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure] public abstract int CountMonthsInYear(int y);

    /// <inheritdoc />
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure] public abstract int CountDaysInYear(int y);

    /// <inheritdoc />
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure] public abstract int CountDaysInMonth(int y, int m);

    /// <inheritdoc />
    //
    // Notice that CountDaysInYearBeforeMonth() is entirely determined
    // by CountDaysInMonth(): sum of CountDaysInMonth(y, i) for 0 < i < m.
    // Nevertheless, this method being used by the default implementation of
    // CountDaysSinceEpoch() and other methods, we need a more efficient
    // solution.
    [Pure] public abstract int CountDaysInYearBeforeMonth(int y, int m);

    //
    // Default interface methods
    //
    // Implemented so that there are available to all derived classes.

    /// <inheritdoc />
    [Pure]
    public int CountDaysInYearAfterMonth(int y, int m) =>
        m >= CountMonthsInYear(y) ? 0
        : CountDaysInYear(y) - CountDaysInYearBeforeMonth(y, m + 1);

    #region CountDaysInYearBefore()

    /// <inheritdoc />
    [Pure]
    public int CountDaysInYearBefore(int y, int m, int d) =>
        CountDaysInYearBeforeMonth(y, m) + d - 1;

    // Intentionally not overriden.
    //[Pure] int CountDaysInYearBefore(int y, int doy);

    /// <inheritdoc />
    [Pure]
    public int CountDaysInYearBefore(int daysSinceEpoch)
    {
        _ = GetYear(daysSinceEpoch, out int doy);
        return doy - 1;
    }

    #endregion
    #region CountDaysInYearAfter()

    /// <inheritdoc />
    [Pure]
    public int CountDaysInYearAfter(int y, int m, int d) =>
        CountDaysInYear(y) - CountDaysInYearBeforeMonth(y, m) - d;

    /// <inheritdoc />
    [Pure]
    public int CountDaysInYearAfter(int y, int doy) => CountDaysInYear(y) - doy;

    /// <inheritdoc />
    [Pure]
    public int CountDaysInYearAfter(int daysSinceEpoch)
    {
        int y = GetYear(daysSinceEpoch, out int doy);
        return CountDaysInYear(y) - doy;
    }

    #endregion
    #region CountDaysInMonthBefore()

    // Intentionally not overriden.
    //[Pure] int CountDaysInMonthBefore(int y, int m, int d);

    /// <inheritdoc />
    [Pure]
    public int CountDaysInMonthBefore(int y, int doy)
    {
        _ = GetMonth(y, doy, out int d);
        return d - 1;
    }

    /// <inheritdoc />
    [Pure]
    public int CountDaysInMonthBefore(int daysSinceEpoch)
    {
        GetDateParts(daysSinceEpoch, out _, out _, out int d);
        return d - 1;
    }

    #endregion
    #region CountDaysInMonthAfter()

    /// <inheritdoc />
    [Pure]
    public int CountDaysInMonthAfter(int y, int m, int d) => CountDaysInMonth(y, m) - d;

    /// <inheritdoc />
    [Pure]
    public int CountDaysInMonthAfter(int y, int doy)
    {
        int m = GetMonth(y, doy, out int d);
        return CountDaysInMonth(y, m) - d;
    }

    /// <inheritdoc />
    [Pure]
    public int CountDaysInMonthAfter(int daysSinceEpoch)
    {
        GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        return CountDaysInMonth(y, m) - d;
    }

    #endregion
}

public partial class CalendricalSchema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public virtual int CountMonthsSinceEpoch(int y, int m) => GetStartOfYearInMonths(y) + m - 1;

    /// <inheritdoc />
    [Pure]
    public virtual int CountDaysSinceEpoch(int y, int m, int d) =>
        GetStartOfYear(y) + CountDaysInYearBeforeMonth(y, m) + d - 1;

    /// <inheritdoc />
    [Pure]
    public int CountDaysSinceEpoch(int y, int doy) => GetStartOfYear(y) + doy - 1;

    /// <inheritdoc />
    public abstract void GetMonthParts(int monthsSinceEpoch, out int y, out int m);

    /// <inheritdoc />
    public virtual void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        // This is just daysSinceEpoch -> (y, doy) -> (y, m, d), that is
        // 1. Find the year; "doy" is then a given.
        // 2. Find the month; "d" is then a given.
        // I doubt that one can do better than that. Even in the Gregorian
        // and Julian schemas where the impl seems different, the principle
        // is actually identical; the computation is just done using a
        // different month numbering.

        // Conversion daysSinceEpoch -> (y, doy).
        y = GetYear(daysSinceEpoch, out int doy);
        // Conversion (y, doy) -> (y, m, d).
        m = GetMonth(y, doy, out d);
    }

    /// <inheritdoc />
    [Pure]
    public virtual int GetYear(int daysSinceEpoch, out int doy)
    {
        int y = GetYear(daysSinceEpoch);
        doy = 1 + daysSinceEpoch - GetStartOfYear(y);
        return y;
    }

    /// <inheritdoc />
    //
    // Partial form of the other GetYear(). One can also say that it's a
    // partial form of GetDateParts(), but this method usually delegates
    // part of its work to GetOrdinalParts().
    [Pure] public abstract int GetYear(int daysSinceEpoch);

    /// <inheritdoc />
    //
    // GetMonth() without an out param could be useful for OrdinalDate but,
    // in practice, it seems simpler and better to keep the computations of
    // "d" & "m" together.
    // REVIEW(code): we encountered a similar situation with GetYear(),
    // except that we often compute its result without having to compute
    // GetStartOfYear().
    // Once we know "m", it is easy to compute "d".
    //   d = doy - CountDaysInYearBeforeMonth(y, m);
    [Pure] public abstract int GetMonth(int y, int doy, out int d);

    /// <inheritdoc />
    [Pure]
    public int GetDayOfYear(int y, int m, int d) =>
        // NB: when one already has also "daysSinceEpoch" at its disposal,
        // it is faster to compute:
        // > 1 + daysSinceEpoch - GetStartOfYear(y);
        // This is just CountDaysInYearBefore(y, m, d) + 1
        CountDaysInYearBeforeMonth(y, m) + d;
}

public partial class CalendricalSchema // Counting months and days since the epoch
{
    /// <inheritdoc />
    //
    // Even if it is just CountMonthsSinceEpoch(y, 1), this method MUST be
    // implemented independently. Indeed, we use it to provide a default
    // impl for CountMonthsSinceEpoch().
    [Pure] public abstract int GetStartOfYearInMonths(int y);

    /// <inheritdoc />
    [Pure]
    public virtual int GetEndOfYearInMonths(int y) =>
        GetStartOfYearInMonths(y) + CountMonthsInYear(y) - 1;

    /// <inheritdoc />
    //
    // Even if it is just CountDaysSinceEpoch(y, 1, 1), this method MUST
    // be implemented independently. Indeed, we use it to provide a default
    // impl for CountDaysSinceEpoch(); see also GetYear().
    // Idem w/ GetEndOfYear(y - 1) + 1, moreover it could overflow at MinYear.
    [Pure] public abstract int GetStartOfYear(int y);

    /// <inheritdoc />
    // We could use GetStartOfYear(y + 1) - 1 but it might overflow at MaxYear.
    [Pure]
    public int GetEndOfYear(int y) => GetStartOfYear(y) + CountDaysInYear(y) - 1;

    /// <inheritdoc />
    [Pure]
    public int GetStartOfMonth(int y, int m) => GetStartOfYear(y) + CountDaysInYearBeforeMonth(y, m);

    /// <inheritdoc />
    [Pure]
    public int GetEndOfMonth(int y, int m) => GetStartOfMonth(y, m) + CountDaysInMonth(y, m) - 1;
}
