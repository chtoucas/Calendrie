// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

// FIXME(code): GetAllDays(), FirstDay, LastDay, CalendarYear

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
    // TODO(code): will be a constant.
    /// <summary>Represents the maximum value of <see cref="_monthsSinceEpoch"/>.
    /// <para>This field is a constant equal to XXX.</para></summary>
    private static readonly int s_MaxMonthsSinceEpoch =
        CivilCalendar.Instance.Scope.Segment.SupportedMonths.Max;

    /// <summary>
    /// Represents the count of consecutive months since <see cref="DayZero.NewStyle"/>.
    /// <para>This field is in the range from 0 to <see cref="s_MaxMonthsSinceEpoch"/>.
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
        var chr = CivilCalendar.Instance;
        chr.Scope.ValidateYearMonth(year, month);

        _monthsSinceEpoch = chr.Schema.CountMonthsSinceEpoch(year, month);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilMonth"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal CivilMonth(int monthsSinceEpoch)
    {
        _monthsSinceEpoch = monthsSinceEpoch;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    //
    // MinValue = new(0) = new() = default(CivilMonth)
    public static CivilMonth MinValue { get; }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static CivilMonth MaxValue { get; } = new(s_MaxMonthsSinceEpoch);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static CivilCalendar Calendar => CivilCalendar.Instance;

    /// <summary>
    /// Gets the count of months since the epoch of the calendar to which belongs
    /// the current instance.
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
    /// Gets the (algebraic) year number.
    /// </summary>
    public int Year
    {
        get
        {
            Calendar.Schema.GetMonthParts(_monthsSinceEpoch, out int y, out _);
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
            Calendar.Schema.GetMonthParts(_monthsSinceEpoch, out _, out int m);
            return m;
        }
    }

    /// <summary>
    /// Returns <see langword="true"/> if the current instance is an intercalary
    /// month; otherwise returns <see langword="false"/>.
    /// </summary>
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
    /// Gets the calendar year.
    /// </summary>
    public CivilYear CalendarYear => new(Year);

    /// <summary>
    /// Gets the first day of this month instance.
    /// </summary>
    public CivilDate FirstDay => new(Year, Month, 1);

    /// <summary>
    /// Obtains the last day of this month instance.
    /// </summary>
    public CivilDate LastDay
    {
        get
        {
            var sch = Calendar.Schema;
            sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
            int d = sch.CountDaysInMonth(y, m);
            return new CivilDate(y, m, d);
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

    /// <summary>
    /// Deconstructs the current instance into its components.
    /// </summary>
    public void Deconstruct(out int year, out int month) =>
        Calendar.Schema.GetMonthParts(_monthsSinceEpoch, out year, out month);
}

public partial struct CivilMonth // Conversions
{
    /// <summary>
    /// Converts the current instance to a range of days.
    /// </summary>
    [Pure]
    public Range<CivilDate> ToRange() => Range.Create(FirstDay, LastDay);
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
        sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
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
        sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
        return sch.CountDaysInYearAfterMonth(y, m);
    }

    /// <summary>
    /// Obtains the number of days in this month instance.
    /// </summary>
    [Pure]
    public int CountDaysInMonth()
    {
        var sch = Calendar.Schema;
        sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
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
        chr.Schema.GetMonthParts(_monthsSinceEpoch, out _, out int m);
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
        chr.Schema.GetMonthParts(_monthsSinceEpoch, out int y, out _);
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
        sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
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
        sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
        int daysInMonth = sch.CountDaysInMonth(y, m);

        for (int d = 1; d <= daysInMonth; d++)
        {
            yield return new CivilDate(y, m, d);
        }
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
        sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
        var (y1, m1, _) = date;
        return y1 == y && m1 == m;
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
    /// <inheritdoc />
    public static bool operator <(CivilMonth left, CivilMonth right) =>
        left._monthsSinceEpoch < right._monthsSinceEpoch;

    /// <inheritdoc />
    public static bool operator <=(CivilMonth left, CivilMonth right) =>
        left._monthsSinceEpoch <= right._monthsSinceEpoch;

    /// <inheritdoc />
    public static bool operator >(CivilMonth left, CivilMonth right) =>
        left._monthsSinceEpoch > right._monthsSinceEpoch;

    /// <inheritdoc />
    public static bool operator >=(CivilMonth left, CivilMonth right) =>
        left._monthsSinceEpoch >= right._monthsSinceEpoch;

    /// <summary>
    /// Obtains the earlier month of two specified months.
    /// </summary>
    [Pure]
    public static CivilMonth Min(CivilMonth x, CivilMonth y) => x < y ? x : y;

    /// <summary>
    /// Obtains the later month of two specified months.
    /// </summary>
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
    /// Adds a number of months to this month instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported months.</exception>
    [Pure]
    public CivilMonth PlusMonths(int months)
    {
        int monthsSinceEpoch = checked(_monthsSinceEpoch + months);
        if (unchecked((uint)monthsSinceEpoch) > s_MaxMonthsSinceEpoch)
            ThrowHelpers.ThrowMonthOverflow();
        return new(monthsSinceEpoch);
    }

    /// <summary>
    /// Obtains the month after this month instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported month.</exception>
    [Pure]
    public CivilMonth NextMonth()
    {
        if (_monthsSinceEpoch == s_MaxMonthsSinceEpoch) ThrowHelpers.ThrowMonthOverflow();
        return new(_monthsSinceEpoch + 1);
    }

    /// <summary>
    /// Obtains the month before this month instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported month.</exception>
    [Pure]
    public CivilMonth PreviousMonth()
    {
        if (_monthsSinceEpoch == 0) ThrowHelpers.ThrowMonthOverflow();
        return new(_monthsSinceEpoch - 1);
    }
}

public partial struct CivilMonth // Non-standard math ops
{
    /// <summary>
    /// Counts the number of years elapsed since the specified month.
    /// </summary>
    [Pure]
    public int CountYearsSince(CivilMonth other)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds a number of years to the year field of this month instance, yielding
    /// a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported months.</exception>
    [Pure]
    public CivilMonth PlusYears(int years)
    {
        throw new NotImplementedException();
    }
}
