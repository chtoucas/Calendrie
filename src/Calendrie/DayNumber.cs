﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie;

using System.Globalization;
using System.Numerics;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Core.Validation;
using Calendrie.Hemerology;

// TODO(doc): explain why DayNumber does not keep track of the underlying time scale.
// REVIEW(perf): make _daysSinceZero internal to remove an extra get method call?
// but it should not be for arithmetic ops, they may overflow.

#region Developer Notes

// Since DaysSinceZero is public, Min/MaxDaysSinceZero are public too. Another
// reason: constructing a day number requires a "daysSinceEpoch" and I like the
// fact that we can validate its value before.
//
// Min/MaxGregorianDaysSinceZero
// This time, the following two constants are private. Rationale: we construct a
// day number from a "daysSinceEpoch" and only after do we check that the result
// is within the Gregorian domain, using the prop GregorianDomain.
//
// Min/MaxSupportedYear
// We can go a bit further than JulianSchema.SupportedYears. The current
// estimation for the age of the universe is ~14 billion Julian years.
// Unfortunately, the matching day number largely exceeds the capacity of Int32.
// To go that far back in time, see DayNumber64. To simplify we use the same
// values for the min and max years in both Gregorian and Julian cases.
//
// Min/MaxValue
// The minimum value has been chosen such that its properties do not overflow,
// e.g. DayNumber.MinValue.Ordinal = Ord.MinValue. The maximum value has been
// chosen such that its properties do not overflow, e.g.
// DayNumber.MaxValue.Ordinal = Ord.MaxValue.
//
// Notice that the type definition of GregorianDomain is recursive. Even if it is
// perfectly legal, GregorianDomain cannot be part of the type initialization of
// DayNumber (e.g. it cannot be a field); otherwise the CoreCLR will throw a
// TypeLoadException. It seems to be a known limitation in some CLR
// implementations.
// NB: it would work fine if either Range<T> or DayNumber was not a struct.
// https://github.com/dotnet/runtime/issues/5479
// https://github.com/dotnet/runtime/issues/11179
// https://github.com/dotnet/roslyn/issues/10126#issuecomment-204471882
// https://stackoverflow.com/questions/36222117/maybe-a-c-sharp-compiler-bug-in-visual-studio-2015/36337761#36337761

#endregion

/// <summary>
/// Represents a day number which counts the number of consecutive days since the
/// Monday 1st of January, 1 CE within the Gregorian calendar.
/// <para><see cref="DayNumber"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct DayNumber :
    IFixedDate<DayNumber>,
    // Comparison
    IComparisonOperators<DayNumber, DayNumber>,
    IMinMaxValue<DayNumber>,
    IMinMaxFunction<DayNumber>,
    // Arithmetic
    IStandardArithmetic<DayNumber>,
    IAdditionOperators<DayNumber, int, DayNumber>,
    ISubtractionOperators<DayNumber, int, DayNumber>,
    IDifferenceOperators<DayNumber, int>,
    IIncrementOperators<DayNumber>,
    IDecrementOperators<DayNumber>
{
    /// <summary>
    /// Represents the smallest possible value of the count of consecutive days
    /// since <see cref="Zero"/>.
    /// <para>This field is a constant equal to -2_147_483_647.</para>
    /// </summary>
    public const int MinDaysSinceZero = int.MinValue + 1;
    /// <summary>
    /// Represents the largest possible value of the count of consecutive days
    /// since <see cref="Zero"/>.
    /// <para>This field is a constant equal to 2_147_483_646.</para>
    /// </summary>
    public const int MaxDaysSinceZero = int.MaxValue - 1;

    /// <summary>
    /// Represents the earliest supported <i>Gregorian</i> or <i>Julian</i> year.
    /// <para>This field is a constant equal to -4_999_999.</para>
    /// </summary>
    public const int MinSupportedYear = -4_999_999;
    /// <summary>
    /// Represents the latest supported <i>Gregorian</i> or <i>Julian</i> year.
    /// <para>This field is a constant equal to 5_000_000.</para>
    /// </summary>
    public const int MaxSupportedYear = 5_000_000;

    /// <summary>
    /// Represents the count of consecutive days since <see cref="Zero"/>.
    /// <para>This field is in the range from <see cref="MinDaysSinceZero"/> to
    /// <see cref="MaxDaysSinceZero"/>.</para>
    /// </summary>
    private readonly int _daysSinceZero;

    /// <summary>
    /// Initializes a new instance of the <see cref="DayNumber"/> struct from the
    /// specified count of consecutive days since <see cref="Zero"/>.
    /// <para>This constructor does NOT validate its parameter.</para>
    /// </summary>
    internal DayNumber(int daysSinceZero)
    {
        Debug.Assert(daysSinceZero >= MinDaysSinceZero);
        Debug.Assert(daysSinceZero <= MaxDaysSinceZero);

        _daysSinceZero = daysSinceZero;
    }

    /// <summary>
    /// Gets the origin of the day numbering system, the Monday 1st of January,
    /// 1 CE within the Gregorian calendar.
    /// <para>This static property is thread-safe.</para>
    /// <para>See also <seealso cref="DayZero.NewStyle"/>.</para>
    /// </summary>
    public static DayNumber Zero { get; }

    /// <inheritdoc/>
    /// <remarks>This static property is thread-safe.</remarks>
    public static DayNumber MinValue { get; } = new(MinDaysSinceZero);

    /// <inheritdoc/>
    /// <remarks>This static property is thread-safe.</remarks>
    public static DayNumber MaxValue { get; } = new(MaxDaysSinceZero);

    DayNumber IFixedDate.DayNumber => this;

    /// <summary>
    /// Gets the count of consecutive days since <see cref="Zero"/>.
    /// <para>The result is in the range from <see cref="MinDaysSinceZero"/> to
    /// <see cref="MaxDaysSinceZero"/>.</para>
    /// </summary>
    public int DaysSinceZero => _daysSinceZero;

    /// <summary>
    /// Gets the count of consecutive days since <see cref="Zero"/>.
    /// <para>The result is in the range from <see cref="MinDaysSinceZero"/> to
    /// <see cref="MaxDaysSinceZero"/>.</para>
    /// </summary>
    int IFixedDate.DaysSinceEpoch => _daysSinceZero;

    /// <summary>
    /// Gets the ordinal numeral from this instance.
    /// </summary>
    public Ord Ordinal => Ord.First + _daysSinceZero;

    /// <summary>
    /// Gets the day of the week.
    /// </summary>
    public DayOfWeek DayOfWeek =>
        // Zero is a Monday.
        (DayOfWeek)MathZ.Modulo(
            (int)DayOfWeek.Monday + _daysSinceZero,
            CalendricalConstants.DaysInWeek);

    /// <summary>
    /// Converts the current instance to its equivalent string representation
    /// using the formatting conventions of the current culture.
    /// </summary>
    [Pure]
    public override string ToString() => _daysSinceZero.ToString(CultureInfo.CurrentCulture);
}

public partial struct DayNumber // Gregorian/Julian conversions
{
    #region Gregorian

    // To obtain this values, compute
    // > DayNumber.FromGregorianParts(DayNumber.MinSupportedYear, 1, 1);
    // > DayNumber.FromGregorianParts(DayNumber.MaxSupportedYear, 12, 31);
    private const int MinGregorianDaysSinceZero = -1_826_212_500;
    private const int MaxGregorianDaysSinceZero = 1_826_212_499;

    private static readonly DayNumber s_MinGregorianValue = new(MinGregorianDaysSinceZero);
    private static readonly DayNumber s_MaxGregorianValue = new(MaxGregorianDaysSinceZero);

    /// <summary>
    /// Gets the range of supported Gregorian values for a <see cref="DayNumber"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Range<DayNumber> GregorianDomain =>
        Range.CreateLeniently(s_MinGregorianValue, s_MaxGregorianValue);

    /// <summary>
    /// Creates a new instance of <see cref="DayNumber"/> from the specified
    /// Gregorian date parts.
    /// </summary>
    /// <exception cref="AoorException">The given parts are outside the range of
    /// supported years or do not represent a valid Gregorian date.</exception>
    [Pure]
    public static DayNumber FromGregorianParts(int year, int month, int day)
    {
        ValidateYear(year);
        GregorianPreValidator.Instance.ValidateMonthDay(year, month, day);

        int daysSinceZero = (int)GregorianFormulae.CountDaysSinceEpoch((long)year, month, day);
        return new DayNumber(daysSinceZero);
    }

    /// <summary>
    /// Creates a new instance of <see cref="DayNumber"/> from the specified
    /// Gregorian ordinal date parts.
    /// </summary>
    /// <exception cref="AoorException">The given parts are outside the range of
    /// supported years or do not represent a valid Gregorian ordinal date.
    /// </exception>
    [Pure]
    public static DayNumber FromGregorianOrdinalParts(int year, int dayOfYear)
    {
        ValidateYear(year);
        GregorianPreValidator.Instance.ValidateDayOfYear(year, dayOfYear);

        // NB: Here we can use GregorianFormulae instead of GregorianFormulae64.
        int daysSinceZero = GregorianFormulae.GetStartOfYear(year) + dayOfYear - 1;
        return new DayNumber(daysSinceZero);
    }

    /// <summary>
    /// Obtains the Gregorian date parts of the current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public DateParts GetGregorianParts()
    {
        CheckGregorianOverflow();

        GregorianFormulae.GetDateParts(_daysSinceZero, out long y, out int m, out int d);
        return new DateParts((int)y, m, d);
    }

    /// <summary>
    /// Obtains the Gregorian ordinal date parts of the current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported ordinal dates.</exception>
    [Pure]
    public OrdinalParts GetGregorianOrdinalParts()
    {
        CheckGregorianOverflow();

        int y = (int)GregorianFormulae.GetYear((long)_daysSinceZero);
        // We could have used
        // > GregorianFormulae64.GetDateParts(_daysSinceZero, out year, out month, out day);
        // > doy = GregorianFormulae64.CountDaysInYearBeforeMonth(year, month) + day;
        // but, even without GetDateParts(), it seems to be a little bit slower.
        // NB: here we can use GregorianFormulae instead of GregorianFormulae64.
        int doy = 1 + _daysSinceZero - GregorianFormulae.GetStartOfYear(y);
        return new OrdinalParts(y, doy);
    }

    /// <summary>
    /// Obtains the Gregorian year of the current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported years.</exception>
    [Pure]
    public int GetGregorianYear()
    {
        CheckGregorianOverflow();

        return (int)GregorianFormulae.GetYear((long)_daysSinceZero);
    }

    #endregion
    #region Julian

    // This is DayZero.OldStyle - DayNumber.NewStyle.
    private const int DaysFromJulianEpochToZero = 2;

    private const int MinJulianDaysSinceZero = -1_826_250_002;
    private const int MaxJulianDaysSinceZero = 1_826_249_997;

    private static readonly DayNumber s_MinJulianValue = new(MinJulianDaysSinceZero);
    private static readonly DayNumber s_MaxJulianValue = new(MaxJulianDaysSinceZero);

    /// <summary>
    /// Gets the range of supported Julian values for a <see cref="DayNumber"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Range<DayNumber> JulianDomain =>
        Range.CreateLeniently(s_MinJulianValue, s_MaxJulianValue);

    /// <summary>
    /// Creates a new instance of <see cref="DayNumber"/> from the specified
    /// Julian date parts.
    /// </summary>
    /// <exception cref="AoorException">The given parts are outside the range of
    /// supported years or do not represent a valid Julian date.</exception>
    [Pure]
    public static DayNumber FromJulianParts(int year, int month, int day)
    {
        ValidateYear(year);
        JulianPreValidator.Instance.ValidateMonthDay(year, month, day);

        int daysSinceEpoch = (int)JulianFormulae.CountDaysSinceEpoch((long)year, month, day);
        int daysSinceZero = daysSinceEpoch - DaysFromJulianEpochToZero;
        return new DayNumber(daysSinceZero);
    }

    /// <summary>
    /// Creates a new instance of <see cref="DayNumber"/> from the specified
    /// Julian ordinal date parts.
    /// </summary>
    /// <exception cref="AoorException">The given parts are outside the range of
    /// supported years or do not represent a valid Julian ordinal date.
    /// </exception>
    [Pure]
    public static DayNumber FromJulianOrdinalParts(int year, int dayOfYear)
    {
        ValidateYear(year);
        JulianPreValidator.Instance.ValidateDayOfYear(year, dayOfYear);

        int daysSinceEpoch = JulianFormulae.GetStartOfYear(year) + dayOfYear - 1;
        int daysSinceZero = daysSinceEpoch - DaysFromJulianEpochToZero;
        return new DayNumber(daysSinceZero);
    }

    /// <summary>
    /// Obtains the Julian date parts of the current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public DateParts GetJulianParts()
    {
        CheckJulianOverflow();

        int daysSinceEpoch = DaysFromJulianEpochToZero + _daysSinceZero;
        JulianFormulae.GetDateParts(daysSinceEpoch, out long y, out int m, out int d);
        return new DateParts((int)y, m, d);
    }

    /// <summary>
    /// Obtains the Julian ordinal date parts of the current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported ordinal dates.</exception>
    [Pure]
    public OrdinalParts GetJulianOrdinalParts()
    {
        CheckJulianOverflow();

        int daysSinceEpoch = DaysFromJulianEpochToZero + _daysSinceZero;
        int y = (int)JulianFormulae.GetYear((long)daysSinceEpoch);
        int doy = 1 + daysSinceEpoch - JulianFormulae.GetStartOfYear(y);
        return new OrdinalParts(y, doy);
    }

    /// <summary>
    /// Obtains the Julian year of the current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported years.</exception>
    [Pure]
    public int GetJulianYear()
    {
        CheckJulianOverflow();

        int daysSinceEpoch = DaysFromJulianEpochToZero + _daysSinceZero;
        return (int)JulianFormulae.GetYear((long)daysSinceEpoch);
    }

    #endregion
    #region Validation helpers

    /// <summary>
    /// Validates the specified Gregorian or Julian year.
    /// </summary>
    /// <exception cref="AoorException" />
    private static void ValidateYear(int year)
    {
        if (year < MinSupportedYear || year > MaxSupportedYear) { Throw.YearOutOfRange(year); }
    }

    /// <summary>
    /// Checks that the operation does not overflow the range of supported
    /// Gregorian values.
    /// </summary>
    /// <exception cref="OverflowException" />
    private void CheckGregorianOverflow()
    {
        if (_daysSinceZero < MinGregorianDaysSinceZero || _daysSinceZero > MaxGregorianDaysSinceZero)
        {
            Throw.DateOverflow();
        }
    }

    /// <summary>
    /// Checks that the operation does not overflow the range of supported Julian
    /// values.
    /// </summary>
    /// <exception cref="OverflowException" />
    private void CheckJulianOverflow()
    {
        if (_daysSinceZero < MinJulianDaysSinceZero || _daysSinceZero > MaxJulianDaysSinceZero)
        {
            Throw.DateOverflow();
        }
    }

    #endregion
}

public partial struct DayNumber // Adjust the day of the week
{
    private static readonly DayNumber s_ThreeDaysBeforeMaxValue = MaxValue - 3;

    /// <inheritdoc/>
    [Pure]
    public DayNumber Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        return this + (δ >= 0 ? δ - CalendricalConstants.DaysInWeek : δ);
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber PreviousOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        return δ == 0 ? this : this + (δ > 0 ? δ - CalendricalConstants.DaysInWeek : δ);
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber Nearest(DayOfWeek dayOfWeek) =>
        // WARNING:
        // - PreviousOrSameCore() fails near MaxValue.
        // - NextOrSameCore() fails near MinValue.
        this > s_ThreeDaysBeforeMaxValue
        ? NextOrSameCore(this, dayOfWeek, -3, 0)
        : PreviousOrSameCore(this, dayOfWeek, 3, 0);

    /// <inheritdoc/>
    [Pure]
    public DayNumber NextOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        return δ == 0 ? this : this + (δ < 0 ? δ + CalendricalConstants.DaysInWeek : δ);
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        return this + (δ <= 0 ? δ + CalendricalConstants.DaysInWeek : δ);
    }

    //
    // Helpers
    //

    [Pure]
    internal static DayNumber PreviousOrSameCore(
        DayNumber dayNumber,
        DayOfWeek dayOfWeek,
        int dayShift,
        int weeks)
    {
        Debug.Assert(dayShift >= 3);
        Requires.Defined(dayOfWeek);

        int daysSinceZero;
        checked
        {
            daysSinceZero = dayNumber.DaysSinceZero + dayShift;
            // DayNumber.Zero is a Monday.
            daysSinceZero -= MathZ.Modulo(
                daysSinceZero + (DayOfWeek.Monday - dayOfWeek),
                CalendricalConstants.DaysInWeek);
            daysSinceZero -= CalendricalConstants.DaysInWeek * weeks;
        }

        return Zero + daysSinceZero;
    }

    [Pure]
    internal static DayNumber NextOrSameCore(
        DayNumber dayNumber,
        DayOfWeek dayOfWeek,
        int dayShift,
        int weeks)
    {
        Debug.Assert(dayShift <= -3);
        Requires.Defined(dayOfWeek);

        int daysSinceZero;
        checked
        {
            daysSinceZero = dayNumber.DaysSinceZero + dayShift;
            // DayNumber.Zero is a Monday.
            daysSinceZero += MathZ.Modulo(
                -daysSinceZero - (DayOfWeek.Monday - dayOfWeek),
                CalendricalConstants.DaysInWeek);
            daysSinceZero += CalendricalConstants.DaysInWeek * weeks;
        }

        return Zero + daysSinceZero;
    }
}

public partial struct DayNumber // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(DayNumber left, DayNumber right) =>
        left._daysSinceZero == right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator !=(DayNumber left, DayNumber right) =>
        left._daysSinceZero != right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public bool Equals(DayNumber other) => _daysSinceZero == other._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is DayNumber dayNumber && Equals(dayNumber);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceZero;
}

public partial struct DayNumber // IComparable
{
    /// <inheritdoc />
    public static bool operator <(DayNumber left, DayNumber right) =>
        left._daysSinceZero < right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator <=(DayNumber left, DayNumber right) =>
        left._daysSinceZero <= right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator >(DayNumber left, DayNumber right) =>
        left._daysSinceZero > right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator >=(DayNumber left, DayNumber right) =>
        left._daysSinceZero >= right._daysSinceZero;

    /// <summary>
    /// Obtains the earlier day of two specified day numbers.
    /// </summary>
    [Pure]
    public static DayNumber Min(DayNumber x, DayNumber y) => x < y ? x : y;

    /// <summary>
    /// Obtains the later day of two specified day numbers.
    /// </summary>
    [Pure]
    public static DayNumber Max(DayNumber x, DayNumber y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(DayNumber other) => _daysSinceZero.CompareTo(other._daysSinceZero);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is DayNumber dayNumber ? CompareTo(dayNumber)
        : Throw.NonComparable(typeof(DayNumber), obj);
}

public partial struct DayNumber // Math ops
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified day numbers and returns the number of days
    /// between them.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    public static int operator -(DayNumber left, DayNumber right) =>
        checked(left._daysSinceZero - right._daysSinceZero);

    /// <summary>
    /// Adds a number of days to a specified day number, yielding a new day
    /// number.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest or the latest supported day numbers.</exception>
    public static DayNumber operator +(DayNumber value, int days)
    {
        int newDays = checked(value._daysSinceZero + days);
        if (newDays == MinDaysSinceZero - 1 || newDays == MaxDaysSinceZero + 1)
        {
            Throw.DayNumberOverflow();
        }
        return new DayNumber(newDays);
    }

    /// <summary>
    /// Subtracts a number of days to a specified day number, yielding a new day
    /// number.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest or the latest supported day numbers.</exception>
    public static DayNumber operator -(DayNumber value, int days)
    {
        int newDays = checked(value._daysSinceZero - days);
        if (newDays == MinDaysSinceZero - 1 || newDays == MaxDaysSinceZero + 1)
        {
            Throw.DayNumberOverflow();
        }
        return new DayNumber(newDays);
    }

    /// <summary>
    /// Adds one day to a specified day number, yielding a new day number.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported day number.</exception>
    public static DayNumber operator ++(DayNumber value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to a specified day number, yielding a new day number.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported day number.</exception>
    public static DayNumber operator --(DayNumber value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <summary>
    /// Subtracts the specified day number from this instance and returns the
    /// number of days between them.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public int CountDaysSince(DayNumber other) => this - other;

    /// <summary>
    /// Adds a number of days to this instance, yielding a new day number.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest or the latest supported day numbers.</exception>
    [Pure]
    public DayNumber PlusDays(int days) => this + days;

    /// <summary>
    /// Obtains the day number following this instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported day number.</exception>
    [Pure]
    public DayNumber NextDay() =>
        this == MaxValue ? Throw.DayNumberOverflow<DayNumber>() : new DayNumber(_daysSinceZero + 1);

    /// <summary>
    /// Obtains the day number preceding this instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported day number.</exception>
    [Pure]
    public DayNumber PreviousDay() =>
        this == MinValue ? Throw.DayNumberOverflow<DayNumber>() : new DayNumber(_daysSinceZero - 1);
}
