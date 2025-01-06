// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Represents the Civil month.
/// <para><i>All</i> months within the range [1..9999] of years are supported.
/// </para>
/// <para><see cref="CivilMonth"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct CivilMonth :
    ICalendarMonth<CivilMonth>,
    ICalendarBound<CivilCalendar>,
    // A month viewed as a finite sequence of days
    IDaySegment<CivilDate>,
    ISetMembership<CivilDate>,
    // Arithmetic
    ISubtractionOperators<CivilMonth, CivilMonth, int>
{ }

public partial struct CivilMonth // Preamble
{
    /// <summary>Represents the maximum value of <see cref="_monthsSinceEpoch"/>.
    /// <para>This field is a constant equal to 119_987.</para></summary>
    private const int MaxMonthsSinceEpoch = 119_987;

    /// <summary>
    /// Represents the count of consecutive months since the Gregorian epoch.
    /// <para>This field is in the range from 0 to <see cref="MaxMonthsSinceEpoch"/>.
    /// </para>
    /// </summary>
    private readonly int _monthsSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilMonth"/> struct to the
    /// specified month components.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid month or <paramref name="year"/> is outside the
    /// range of years.</exception>
    public CivilMonth(int year, int month)
    {
        CivilScope.ValidateYearMonthImpl(year, month);

        _monthsSinceEpoch = CountMonthsSinceEpoch(year, month);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilMonth"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal CivilMonth(int monthsSinceEpoch)
    {
        _monthsSinceEpoch = monthsSinceEpoch;
    }

    /// <summary>
    /// Gets the earliest possible value of a <see cref="CivilMonth"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    //
    // MinValue = new(0) = new() = default(CivilMonth)
    public static CivilMonth MinValue { get; }

    /// <summary>
    /// Gets the latest possible value of a <see cref="CivilMonth"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static CivilMonth MaxValue { get; } = new(MaxMonthsSinceEpoch);

    /// <summary>
    /// Gets the calendar to which belongs the current month type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static CivilCalendar Calendar => CivilCalendar.Instance;

    /// <summary>
    /// Gets the count of consecutive months since the Gregorian epoch.
    /// </summary>
    public int MonthsSinceEpoch => _monthsSinceEpoch;

    /// <summary>
    /// Gets the century of the era.
    /// </summary>
    public Ord CenturyOfEra => Ord.FromInt32(Century);

    /// <summary>
    /// Gets the century number.
    /// </summary>
    public int Century => YearNumbering.GetCentury(Year);

    /// <summary>
    /// Gets the year of the era.
    /// </summary>
    public Ord YearOfEra => Ord.FromInt32(Year);

    /// <summary>
    /// Gets the year of the century.
    /// <para>The result is in the range from 1 to 100.</para>
    /// </summary>
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

    /// <summary>
    /// Gets the year number.
    /// <para>This property represents the algebraic year, but since it's greater
    /// than 0, there is no difference between the algebraic year and the year
    /// of the era.</para>
    /// </summary>
    public int Year =>
        // NB: both dividend and divisor are >= 0.
        1 + _monthsSinceEpoch / CivilCalendar.MonthsInYear;

    /// <inheritdoc />
    public int Month
    {
        get
        {
            var (_, m) = this;
            return m;
        }
    }

    /// <inheritdoc />
    bool ICalendarMonth.IsIntercalary => false;

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        var (y, m) = this;
        return FormattableString.Invariant($"{m:D2}/{y:D4} ({Calendar})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month)
    {
        // See RegularSchema.GetMonthParts().
        // NB: both dividend and divisor are >= 0.
        year = 1 + MathN.Divide(_monthsSinceEpoch, CivilCalendar.MonthsInYear, out int m0);
        month = 1 + m0;
    }
}

public partial struct CivilMonth // Factories
{
    /// <summary>
    /// Creates a new instance of the <see cref="CivilMonth"/> struct from the
    /// specified <see cref="CivilDate"/> value.
    /// </summary>
    [Pure]
    public static CivilMonth Create(CivilDate date)
    {
        var (y, m, _) = date;
        return UnsafeCreate(y, m);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CivilMonth"/> struct from the
    /// specified month components.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static CivilMonth UnsafeCreate(int year, int month)
    {
        int monthsSinceEpoch = CountMonthsSinceEpoch(year, month);
        return new CivilMonth(monthsSinceEpoch);
    }

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int CountMonthsSinceEpoch(int y, int m) =>
        // See RegularSchema.CountMonthsSinceEpoch().
        CivilCalendar.MonthsInYear * (y - 1) + m - 1;
}

public partial struct CivilMonth // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedMonthsInYear() => Month - 1;

    /// <inheritdoc />
    [Pure]
    public int CountRemainingMonthsInYear() => CivilCalendar.MonthsInYear - 1;

#if false
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear()
    {
        var (y, m) = this;
        return Calendar.Schema.CountDaysInYearBeforeMonth(y, m);
    }

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear()
    {
        var (y, m) = this;
        return Calendar.Schema.CountDaysInYearAfterMonth(y, m);
    }
#endif
}

public partial struct CivilMonth // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public CivilMonth WithYear(int newYear)
    {
        int m = Month;
        // Even when "newYear" is valid, we must re-check "m".
        Calendar.Scope.ValidateYearMonth(newYear, m, nameof(newYear));
        return new CivilMonth(newYear, m);
    }

    /// <inheritdoc />
    [Pure]
    public CivilMonth WithMonth(int newMonth)
    {
        int y = Year;
        // We already know that "y" is valid, we only need to check "newMonth".
        Calendar.Scope.PreValidator.ValidateMonth(y, newMonth, nameof(newMonth));
        return new CivilMonth(y, newMonth);
    }
}

public partial struct CivilMonth // IDaySegment
{
    /// <inheritdoc />
    public CivilDate MinDay
    {
        get
        {
            var (y, m) = this;
            int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(y, m, 1);
            return new CivilDate(daysSinceZero);
        }
    }

    /// <inheritdoc />
    public CivilDate MaxDay
    {
        get
        {
            var (y, m) = this;
            int d = GregorianFormulae.CountDaysInMonth(y, m);
            int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(y, m, d);
            return new CivilDate(daysSinceZero);
        }
    }

    /// <inheritdoc />
    /// <remarks>See also <see cref="CalendarSystem{TDate}.CountDaysInMonth(int, int)"/>.
    /// </remarks>
    [Pure]
    public int CountDays()
    {
        var (y, m) = this;
        return GregorianFormulae.CountDaysInMonth(y, m);
    }

    /// <summary>
    /// Converts the current instance to a range of days.
    /// </summary>
    /// <remarks>See also <see cref="CalendarSystem{TDate}.GetDaysInMonth(int, int)"/>.
    /// </remarks>
    [Pure]
    public Range<CivilDate> ToRange() => Range.UnsafeCreate(MinDay, MaxDay);

    [Pure]
    Range<CivilDate> IDaySegment<CivilDate>.ToDayRange() => ToRange();

    /// <summary>
    /// Returns an enumerable collection of all days in this month instance.
    /// </summary>
    [Pure]
    public IEnumerable<CivilDate> ToEnumerable()
    {
        var (y, m) = this;
        int startOfMonth = GregorianFormulae.CountDaysSinceEpoch(y, m, 1);
        int daysInMonth = GregorianFormulae.CountDaysInMonth(y, m);

        return from daysSinceZero
               in Enumerable.Range(startOfMonth, daysInMonth)
               select new CivilDate(daysSinceZero);
    }

    [Pure]
    IEnumerable<CivilDate> IDaySegment<CivilDate>.EnumerateDays() => ToEnumerable();

    /// <inheritdoc />
    [Pure]
    public bool Contains(CivilDate date)
    {
        var (y, m) = this;
        GregorianFormulae.GetDateParts(date.DaysSinceZero, out int y1, out int m1, out _);
        return y1 == y && m1 == m;
    }

    /// <summary>
    /// Obtains the date corresponding to the specified day of this month
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfMonth"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public CivilDate GetDayOfMonth(int dayOfMonth)
    {
        var (y, m) = this;
        Calendar.Scope.PreValidator.ValidateDayOfMonth(y, m, dayOfMonth);
        return new CivilDate(y, m, dayOfMonth);
    }
}

public partial struct CivilMonth // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(CivilMonth left, CivilMonth right) =>
        left._monthsSinceEpoch == right._monthsSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(CivilMonth left, CivilMonth right) =>
        left._monthsSinceEpoch != right._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(CivilMonth other) => _monthsSinceEpoch == other._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CivilMonth month && Equals(month);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _monthsSinceEpoch;
}

public partial struct CivilMonth // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(CivilMonth left, CivilMonth right) =>
        left._monthsSinceEpoch < right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(CivilMonth left, CivilMonth right) =>
        left._monthsSinceEpoch <= right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(CivilMonth left, CivilMonth right) =>
        left._monthsSinceEpoch > right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(CivilMonth left, CivilMonth right) =>
        left._monthsSinceEpoch >= right._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static CivilMonth Min(CivilMonth x, CivilMonth y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static CivilMonth Max(CivilMonth x, CivilMonth y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(CivilMonth other) => _monthsSinceEpoch.CompareTo(other._monthsSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is CivilMonth month ? CompareTo(month)
        : ThrowHelpers.ThrowNonComparable(typeof(CivilMonth), obj);
}

public partial struct CivilMonth // Standard math ops
{
    /// <summary>
    /// Subtracts the two specified months and returns the number of months
    /// between them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountMonthsSince()")]
    public static int operator -(CivilMonth left, CivilMonth right) => left.CountMonthsSince(right);

    /// <summary>
    /// Adds a number of months to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported months.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusMonths()")]
    public static CivilMonth operator +(CivilMonth value, int months) => value.PlusMonths(months);

    /// <summary>
    /// Subtracts a number of months to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported months.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusMonths()")]
    public static CivilMonth operator -(CivilMonth value, int months) => value.PlusMonths(-months);

    /// <summary>
    /// Adds one month to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported month.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextMonth()")]
    public static CivilMonth operator ++(CivilMonth value) => value.NextMonth();

    /// <summary>
    /// Subtracts one month to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported month.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousMonth()")]
    public static CivilMonth operator --(CivilMonth value) => value.PreviousMonth();

    /// <summary>
    /// Counts the number of months elapsed since the specified month.
    /// </summary>
    [Pure]
    public int CountMonthsSince(CivilMonth other) =>
        // No need to use a checked context here. Indeed, the absolute value of
        // the result is at most equal to MaxMonthsSinceEpoch.
        _monthsSinceEpoch - other._monthsSinceEpoch;

    /// <summary>
    /// Adds a number of months to the current instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported months.
    /// </exception>
    [Pure]
    public CivilMonth PlusMonths(int months)
    {
        int monthsSinceEpoch = checked(_monthsSinceEpoch + months);
        if (unchecked((uint)monthsSinceEpoch) > MaxMonthsSinceEpoch)
            ThrowHelpers.ThrowMonthOverflow();
        return new CivilMonth(monthsSinceEpoch);
    }

    /// <summary>
    /// Obtains the month after the current instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported month.</exception>
    [Pure]
    public CivilMonth NextMonth()
    {
        if (_monthsSinceEpoch == MaxMonthsSinceEpoch) ThrowHelpers.ThrowMonthOverflow();
        return new CivilMonth(_monthsSinceEpoch + 1);
    }

    /// <summary>
    /// Obtains the month before the current instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported month.</exception>
    [Pure]
    public CivilMonth PreviousMonth()
    {
        if (_monthsSinceEpoch == 0) ThrowHelpers.ThrowMonthOverflow();
        return new CivilMonth(_monthsSinceEpoch - 1);
    }
}

public partial struct CivilMonth // Non-standard math ops
{
    /// <summary>
    /// Adds a number of years to the year field of this month instance, yielding
    /// a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported months.</exception>
    [Pure]
    public CivilMonth PlusYears(int years)
    {
        var (y, m) = this;
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowMonthOverflow();

        int monthsSinceEpoch = CountMonthsSinceEpoch(newY, m);
        return new CivilMonth(monthsSinceEpoch);
    }

    /// <summary>
    /// Counts the number of years elapsed since the specified month.
    /// </summary>
    [Pure]
    public int CountYearsSince(CivilMonth other) =>
        // NB: this subtraction never overflows.
        Year - other.Year;
}
