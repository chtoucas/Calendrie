// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Represents a Pax month.
/// <para><i>All</i> months within the range [1..9999] of years are supported.
/// </para>
/// <para><see cref="PaxMonth"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct PaxMonth :
    ICalendarMonth<PaxMonth>,
    ICalendarBound<PaxCalendar>,
    // A month viewed as a finite sequence of days
    IDaySegment<PaxDate>,
    ISetMembership<PaxDate>,
    // Arithmetic
    ISubtractionOperators<PaxMonth, PaxMonth, int>
{ }

public partial struct PaxMonth // Preamble
{
    /// <summary>Represents the maximum value of <see cref="_monthsSinceEpoch"/>.
    /// <para>This field is a constant equal to 131_761.</para></summary>
    private const int MaxMonthsSinceEpoch = 131_761;

    /// <summary>
    /// Represents the count of consecutive months since the epoch
    /// <see cref="DayZero.SundayBeforeGregorian"/>.
    /// <para>This field is in the range from 0 to <see cref="MaxMonthsSinceEpoch"/>.
    /// </para>
    /// </summary>
    private readonly int _monthsSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaxMonth"/> struct to the
    /// specified month components.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid month or <paramref name="year"/> is outside the
    /// range of years.</exception>
    public PaxMonth(int year, int month)
    {
        var chr = PaxCalendar.Instance;
        chr.Scope.ValidateYearMonth(year, month);

        _monthsSinceEpoch = chr.Schema.CountMonthsSinceEpoch(year, month);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaxMonth"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal PaxMonth(int monthsSinceEpoch)
    {
        _monthsSinceEpoch = monthsSinceEpoch;
    }

    /// <summary>
    /// Gets the earliest possible value of a <see cref="PaxMonth"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    //
    // MinValue = new(0) = new() = default(PaxMonth)
    public static PaxMonth MinValue { get; }

    /// <summary>
    /// Gets the latest possible value of a <see cref="PaxMonth"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static PaxMonth MaxValue { get; } = new(MaxMonthsSinceEpoch);

    /// <summary>
    /// Gets the calendar to which belongs the current month type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static PaxCalendar Calendar => PaxCalendar.Instance;

    /// <inheritdoc />
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
    public int Year
    {
        get
        {
            Calendar.Schema.GetMonthParts(_monthsSinceEpoch, out int y, out _);
            return y;
        }
    }

    /// <inheritdoc />
    public int Month
    {
        get
        {
            Calendar.Schema.GetMonthParts(_monthsSinceEpoch, out _, out int m);
            return m;
        }
    }

    /// <inheritdoc />
    public bool IsIntercalary
    {
        get
        {
            var sch = Calendar.Schema;
            sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
            return sch.IsIntercalaryMonth(y, m);
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
        chr.Schema.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
        return FormattableString.Invariant($"{m:D2}/{y:D4} ({chr})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month) =>
        Calendar.Schema.GetMonthParts(_monthsSinceEpoch, out year, out month);
}

public partial struct PaxMonth // Factories
{
    /// <summary>
    /// Creates a new instance of the <see cref="PaxMonth"/> struct from the
    /// specified date value.
    /// </summary>
    [Pure]
    public static PaxMonth Create(CivilDate date)
    {
        var (y, m, _) = date;
        int monthsSinceEpoch = Calendar.Schema.CountMonthsSinceEpoch(y, m);
        return new(monthsSinceEpoch);
    }
}

public partial struct PaxMonth // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedMonthsInYear() => Month - 1;

    /// <inheritdoc />
    [Pure]
    public int CountRemainingMonthsInYear() => Calendar.Schema.CountMonthsInYear(Year) - 1;

#if false
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear()
    {
        var sch = Calendar.Schema;
        sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
        return sch.CountDaysInYearBeforeMonth(y, m);
    }

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear()
    {
        var sch = Calendar.Schema;
        sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
        return sch.CountDaysInYearAfterMonth(y, m);
    }
#endif
}

public partial struct PaxMonth // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public PaxMonth WithYear(int newYear)
    {
        var chr = Calendar;
        chr.Schema.GetMonthParts(_monthsSinceEpoch, out _, out int m);
        // Even when "newYear" is valid, we must re-check "m".
        chr.Scope.ValidateYearMonth(newYear, m, nameof(newYear));
        return new PaxMonth(newYear, m);
    }

    /// <inheritdoc />
    [Pure]
    public PaxMonth WithMonth(int newMonth)
    {
        var chr = Calendar;
        chr.Schema.GetMonthParts(_monthsSinceEpoch, out int y, out _);
        // We already know that "y" is valid, we only need to check "newMonth".
        chr.Scope.PreValidator.ValidateMonth(y, newMonth, nameof(newMonth));
        return new PaxMonth(y, newMonth);
    }
}

public partial struct PaxMonth // IDaySegment
{
    /// <inheritdoc />
    public PaxDate MinDay
    {
        get
        {
            var sch = Calendar.Schema;
            sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
            int daysSinceEpoch = sch.CountDaysSinceEpoch(y, m, 1);
            return new PaxDate(daysSinceEpoch);
        }
    }

    /// <inheritdoc />
    public PaxDate MaxDay
    {
        get
        {
            var sch = Calendar.Schema;
            sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
            int d = sch.CountDaysInMonth(y, m);
            int daysSinceEpoch = sch.CountDaysSinceEpoch(y, m, d);
            return new PaxDate(daysSinceEpoch);
        }
    }

    /// <inheritdoc />
    /// <remarks>See also <see cref="CalendarSystem{TDate}.CountDaysInMonth(int, int)"/>.
    /// </remarks>
    [Pure]
    public int CountDays()
    {
        var sch = Calendar.Schema;
        sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
        return sch.CountDaysInMonth(y, m);
    }

    /// <summary>
    /// Converts the current instance to a range of days.
    /// </summary>
    /// <remarks>See also <see cref="CalendarSystem{TDate}.GetDaysInMonth(int, int)"/>.
    /// </remarks>
    [Pure]
    public Range<PaxDate> ToRange() => Range.UnsafeCreate(MinDay, MaxDay);

    [Pure]
    Range<PaxDate> IDaySegment<PaxDate>.ToDayRange() => ToRange();

    /// <summary>
    /// Returns an enumerable collection of all days in this month instance.
    /// </summary>
    [Pure]
    public IEnumerable<PaxDate> ToEnumerable()
    {
        var sch = Calendar.Schema;
        sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
        int startOfMonth = sch.CountDaysSinceEpoch(y, m, 1);
        int daysInMonth = sch.CountDaysInMonth(y, m);

        return from daysSinceEpoch
               in Enumerable.Range(startOfMonth, daysInMonth)
               select new PaxDate(daysSinceEpoch);
    }

    [Pure]
    IEnumerable<PaxDate> IDaySegment<PaxDate>.EnumerateDays() => ToEnumerable();

    /// <inheritdoc />
    [Pure]
    public bool Contains(PaxDate date)
    {
        var sch = Calendar.Schema;
        sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
        sch.GetDateParts(date.DaysSinceEpoch, out int y1, out int m1, out _);
        return y1 == y && m1 == m;
    }

    /// <summary>
    /// Obtains the date corresponding to the specified day of this month
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfMonth"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public PaxDate GetDayOfMonth(int dayOfMonth)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;
        sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
        chr.Scope.PreValidator.ValidateDayOfMonth(y, m, dayOfMonth);
        return new PaxDate(y, m, dayOfMonth);
    }
}

public partial struct PaxMonth // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(PaxMonth left, PaxMonth right) =>
        left._monthsSinceEpoch == right._monthsSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(PaxMonth left, PaxMonth right) =>
        left._monthsSinceEpoch != right._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(PaxMonth other) => _monthsSinceEpoch == other._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is PaxMonth month && Equals(month);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _monthsSinceEpoch;
}

public partial struct PaxMonth // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(PaxMonth left, PaxMonth right) =>
        left._monthsSinceEpoch < right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(PaxMonth left, PaxMonth right) =>
        left._monthsSinceEpoch <= right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(PaxMonth left, PaxMonth right) =>
        left._monthsSinceEpoch > right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(PaxMonth left, PaxMonth right) =>
        left._monthsSinceEpoch >= right._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static PaxMonth Min(PaxMonth x, PaxMonth y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static PaxMonth Max(PaxMonth x, PaxMonth y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(PaxMonth other) => _monthsSinceEpoch.CompareTo(other._monthsSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is PaxMonth month ? CompareTo(month)
        : ThrowHelpers.ThrowNonComparable(typeof(PaxMonth), obj);
}

public partial struct PaxMonth // Standard math ops
{
    /// <summary>
    /// Subtracts the two specified months and returns the number of months
    /// between them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountMonthsSince()")]
    public static int operator -(PaxMonth left, PaxMonth right) => left.CountMonthsSince(right);

    /// <summary>
    /// Adds a number of months to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported months.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusMonths()")]
    public static PaxMonth operator +(PaxMonth value, int months) => value.PlusMonths(months);

    /// <summary>
    /// Subtracts a number of months to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported months.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusMonths()")]
    public static PaxMonth operator -(PaxMonth value, int months) => value.PlusMonths(-months);

    /// <summary>
    /// Adds one month to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported month.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextMonth()")]
    public static PaxMonth operator ++(PaxMonth value) => value.NextMonth();

    /// <summary>
    /// Subtracts one month to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported month.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousMonth()")]
    public static PaxMonth operator --(PaxMonth value) => value.PreviousMonth();

    /// <summary>
    /// Counts the number of months elapsed since the specified month.
    /// </summary>
    [Pure]
    public int CountMonthsSince(PaxMonth other) =>
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
    public PaxMonth PlusMonths(int months)
    {
        int monthsSinceEpoch = checked(_monthsSinceEpoch + months);
        if (unchecked((uint)monthsSinceEpoch) > MaxMonthsSinceEpoch)
            ThrowHelpers.ThrowMonthOverflow();
        return new(monthsSinceEpoch);
    }

    /// <summary>
    /// Obtains the month after the current instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported month.</exception>
    [Pure]
    public PaxMonth NextMonth()
    {
        if (_monthsSinceEpoch == MaxMonthsSinceEpoch) ThrowHelpers.ThrowMonthOverflow();
        return new(_monthsSinceEpoch + 1);
    }

    /// <summary>
    /// Obtains the month before the current instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported month.</exception>
    [Pure]
    public PaxMonth PreviousMonth()
    {
        if (_monthsSinceEpoch == 0) ThrowHelpers.ThrowMonthOverflow();
        return new(_monthsSinceEpoch - 1);
    }
}

public partial struct PaxMonth // Non-standard math ops
{
    /// <summary>
    /// Adds a number of years to the year field of this month instance, yielding
    /// a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported months.</exception>
    [Pure]
    public PaxMonth PlusYears(int years)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Counts the number of years elapsed since the specified month.
    /// </summary>
    [Pure]
    public int CountYearsSince(PaxMonth other)
    {
        throw new NotImplementedException();
    }
}
