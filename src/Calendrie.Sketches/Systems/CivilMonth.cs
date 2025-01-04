// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

/// <summary>
/// Represents a Civil month.
/// <para><i>All</i> months within the range [1..9999] of years are supported.
/// </para>
/// <para><see cref="CivilMonth"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct CivilMonth :
    // Comparison
    IEqualityOperators<CivilMonth, CivilMonth, bool>,
    IEquatable<CivilMonth>,
    IComparisonOperators<CivilMonth, CivilMonth, bool>,
    IComparable<CivilMonth>,
    IComparable,
    IMinMaxValue<CivilMonth>,
    // Arithmetic
    IAdditionOperators<CivilMonth, int, CivilMonth>,
    ISubtractionOperators<CivilMonth, int, CivilMonth>,
    ISubtractionOperators<CivilMonth, CivilMonth, int>,
    IIncrementOperators<CivilMonth>,
    IDecrementOperators<CivilMonth>
{ }

public partial struct CivilMonth // Preamble
{
    /// <summary>Represents the maximum value of <see cref="_monthsSinceZero"/>.
    /// <para>This field is a constant equal to 119_987.</para></summary>
    private const int MaxMonthsSinceZero = 119_987;

    /// <summary>
    /// Represents the count of consecutive months since <see cref="DayZero.NewStyle"/>.
    /// <para>This field is in the range from 0 to <see cref="MaxMonthsSinceZero"/>.
    /// </para>
    /// </summary>
    private readonly int _monthsSinceZero;

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilMonth"/> struct to the
    /// specified month components.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid month or <paramref name="year"/> is outside the
    /// range of years.</exception>
    public CivilMonth(int year, int month)
    {
        var chr = CivilCalendar.Instance;
        chr.Scope.ValidateYearMonth(year, month);

        _monthsSinceZero = chr.Schema.CountMonthsSinceEpoch(year, month);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilMonth"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal CivilMonth(int monthsSinceZero)
    {
        _monthsSinceZero = monthsSinceZero;
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
    public static CivilMonth MaxValue { get; } = new(MaxMonthsSinceZero);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static CivilCalendar Calendar => CivilCalendar.Instance;

    /// <summary>
    /// Gets the count of months since the epoch of the calendar to which belongs
    /// the current instance.
    /// </summary>
    public int MonthsSinceZero => _monthsSinceZero;

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
    public int Year
    {
        get
        {
            Calendar.Schema.GetMonthParts(_monthsSinceZero, out int y, out _);
            return y;
        }
    }

    /// <summary>
    /// Gets the month of the year.
    /// </summary>
    public int Month
    {
        get
        {
            Calendar.Schema.GetMonthParts(_monthsSinceZero, out _, out int m);
            return m;
        }
    }

    /// <summary>
    /// Returns <see langword="true"/> if the current instance is an intercalary
    /// month; otherwise returns <see langword="false"/>.
    /// </summary>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public bool IsIntercalary => false;
    //{
    //    get
    //    {
    //        var sch = Calendar.Schema;
    //        sch.GetMonthParts(_monthsSinceZero, out int y, out int m);
    //        return sch.IsIntercalaryMonth(y, m);
    //    }
    //}

    /// <summary>
    /// Gets the calendar year.
    /// </summary>
    public CivilYear CalendarYear => new(Year, true);

    /// <summary>
    /// Gets the first day of this month instance.
    /// </summary>
    public CivilDate FirstDay
    {
        get
        {
            var sch = Calendar.Schema;
            sch.GetMonthParts(_monthsSinceZero, out int y, out int m);
            int daysSinceZero = sch.CountDaysSinceEpoch(y, m, 1);
            return new CivilDate(daysSinceZero);
        }
    }

    /// <summary>
    /// Obtains the last day of this month instance.
    /// </summary>
    public CivilDate LastDay
    {
        get
        {
            var sch = Calendar.Schema;
            sch.GetMonthParts(_monthsSinceZero, out int y, out int m);
            int d = sch.CountDaysInMonth(y, m);
            int daysSinceZero = sch.CountDaysSinceEpoch(y, m, d);
            return new CivilDate(daysSinceZero);
        }
    }

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        var chr = Calendar;
        chr.Schema.GetMonthParts(_monthsSinceZero, out int y, out int m);
        return FormattableString.Invariant($"{m:D2}/{y:D4} ({chr})");
    }

    /// <summary>
    /// Deconstructs the current instance into its components.
    /// </summary>
    public void Deconstruct(out int year, out int month) =>
        Calendar.Schema.GetMonthParts(_monthsSinceZero, out year, out month);
}

public partial struct CivilMonth // Factories & conversions
{
    /// <summary>
    /// Converts the current instance to a range of days.
    /// <para>See also <see cref="CalendarSystem{TDate}.GetDaysInMonth(int, int)"/>.
    /// </para>
    /// </summary>
    [Pure]
    public Range<CivilDate> ToRange() => Range.UnsafeCreate(FirstDay, LastDay);
}

public partial struct CivilMonth // Counting
{
    /// <summary>
    /// Obtains the number of whole days in the year elapsed since the start of
    /// the year and before this month instance.
    /// </summary>
    [Pure]
    public int CountElapsedDaysInYear()
    {
        var sch = Calendar.Schema;
        sch.GetMonthParts(_monthsSinceZero, out int y, out int m);
        return sch.CountDaysInYearBeforeMonth(y, m);
    }

    /// <summary>
    /// Obtains the number of whole days remaining after this month instance and
    /// until the end of the year.
    /// </summary>
    [Pure]
    public int CountRemainingDaysInYear()
    {
        var sch = Calendar.Schema;
        sch.GetMonthParts(_monthsSinceZero, out int y, out int m);
        return sch.CountDaysInYearAfterMonth(y, m);
    }

    /// <summary>
    /// Obtains the number of days in this month instance.
    /// <para>See also <see cref="CalendarSystem{TDate}.CountDaysInMonth(int, int)"/>.
    /// </para>
    /// </summary>
    [Pure]
    public int CountDays()
    {
        var sch = Calendar.Schema;
        sch.GetMonthParts(_monthsSinceZero, out int y, out int m);
        return sch.CountDaysInMonth(y, m);
    }
}

public partial struct CivilMonth // Adjustments
{
    /// <summary>
    /// Adjusts the year field to the specified value, yielding a new month.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified month cannot
    /// be converted into the new calendar, the resulting year would be outside
    /// its range of years.</exception>
    [Pure]
    public CivilMonth WithYear(int newYear)
    {
        var chr = Calendar;
        chr.Schema.GetMonthParts(_monthsSinceZero, out _, out int m);
        // Even when "newYear" is valid, we must re-check "m".
        chr.Scope.ValidateYearMonth(newYear, m, nameof(newYear));
        return new CivilMonth(newYear, m);
    }

    /// <summary>
    /// Adjusts the month field to the specified value, yielding a new month.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The resulting month would
    /// be invalid.</exception>
    [Pure]
    public CivilMonth WithMonth(int newMonth)
    {
        var chr = Calendar;
        chr.Schema.GetMonthParts(_monthsSinceZero, out int y, out _);
        // We already know that "y" is valid, we only need to check "newMonth".
        chr.Scope.PreValidator.ValidateMonth(y, newMonth, nameof(newMonth));
        return new CivilMonth(y, newMonth);
    }
}

public partial struct CivilMonth // Days within the month & "membership"
{
    /// <summary>
    /// Obtains the date corresponding to the specified day of this month
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfMonth"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public CivilDate GetDayOfMonth(int dayOfMonth)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;
        sch.GetMonthParts(_monthsSinceZero, out int y, out int m);
        chr.Scope.PreValidator.ValidateDayOfMonth(y, m, dayOfMonth);
        return new CivilDate(y, m, dayOfMonth);
    }

    /// <summary>
    /// Obtains the sequence of all days in this month instance.
    /// </summary>
    [Pure]
    public IEnumerable<CivilDate> GetAllDays()
    {
        var sch = Calendar.Schema;
        sch.GetMonthParts(_monthsSinceZero, out int y, out int m);
        int startOfMonth = sch.CountDaysSinceEpoch(y, m, 1);
        int daysInMonth = sch.CountDaysInMonth(y, m);

        return from daysSinceZero
               in Enumerable.Range(startOfMonth, daysInMonth)
               select new CivilDate(daysSinceZero);
    }

    //
    // "Membership"
    //

    /// <summary>
    /// Determines whether the current instance contains the specified date or
    /// not.
    /// </summary>
    [Pure]
    public bool Contains(CivilDate date)
    {
        var sch = Calendar.Schema;
        sch.GetMonthParts(_monthsSinceZero, out int y, out int m);
        sch.GetDateParts(date.DaysSinceZero, out int y1, out int m1, out _);
        return y1 == y && m1 == m;
    }
}

public partial struct CivilMonth // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(CivilMonth left, CivilMonth right) =>
        left._monthsSinceZero == right._monthsSinceZero;

    /// <inheritdoc />
    public static bool operator !=(CivilMonth left, CivilMonth right) =>
        left._monthsSinceZero != right._monthsSinceZero;

    /// <inheritdoc />
    [Pure]
    public bool Equals(CivilMonth other) => _monthsSinceZero == other._monthsSinceZero;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CivilMonth month && Equals(month);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _monthsSinceZero;
}

public partial struct CivilMonth // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(CivilMonth left, CivilMonth right) =>
        left._monthsSinceZero < right._monthsSinceZero;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(CivilMonth left, CivilMonth right) =>
        left._monthsSinceZero <= right._monthsSinceZero;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(CivilMonth left, CivilMonth right) =>
        left._monthsSinceZero > right._monthsSinceZero;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(CivilMonth left, CivilMonth right) =>
        left._monthsSinceZero >= right._monthsSinceZero;

    /// <summary>
    /// Obtains the earliest month of two specified months.
    /// </summary>
    [Pure]
    public static CivilMonth Min(CivilMonth x, CivilMonth y) => x < y ? x : y;

    /// <summary>
    /// Obtains the latest month of two specified months.
    /// </summary>
    [Pure]
    public static CivilMonth Max(CivilMonth x, CivilMonth y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(CivilMonth other) => _monthsSinceZero.CompareTo(other._monthsSinceZero);

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
        // the result is at most equal to MaxMonthsSinceZero.
        _monthsSinceZero - other._monthsSinceZero;

    /// <summary>
    /// Adds a number of months to this month instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported months.</exception>
    [Pure]
    public CivilMonth PlusMonths(int months)
    {
        int monthsSinceZero = checked(_monthsSinceZero + months);
        if (unchecked((uint)monthsSinceZero) > MaxMonthsSinceZero)
            ThrowHelpers.ThrowMonthOverflow();
        return new(monthsSinceZero);
    }

    /// <summary>
    /// Obtains the month after this month instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported month.</exception>
    [Pure]
    public CivilMonth NextMonth()
    {
        if (_monthsSinceZero == MaxMonthsSinceZero) ThrowHelpers.ThrowMonthOverflow();
        return new(_monthsSinceZero + 1);
    }

    /// <summary>
    /// Obtains the month before this month instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported month.</exception>
    [Pure]
    public CivilMonth PreviousMonth()
    {
        if (_monthsSinceZero == 0) ThrowHelpers.ThrowMonthOverflow();
        return new(_monthsSinceZero - 1);
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
        var sch = Calendar.Schema;
        sch.GetMonthParts(_monthsSinceZero, out int y, out int m);
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowMonthOverflow();

        int daysSinceZero = sch.CountMonthsSinceEpoch(newY, m);
        return new CivilMonth(daysSinceZero);
    }

    /// <summary>
    /// Counts the number of years elapsed since the specified month.
    /// </summary>
    [Pure]
    public int CountYearsSince(CivilMonth other)
    {
        var sch = Calendar.Schema;
        sch.GetMonthParts(_monthsSinceZero, out int y, out _);
        sch.GetMonthParts(other._monthsSinceZero, out int y0, out _);
        // NB: the calendar is regular and the subtraction never overflows.
        return y - y0;
    }
}
