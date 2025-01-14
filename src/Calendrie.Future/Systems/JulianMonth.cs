// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie;
using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Represents the Julian month.
/// <para><i>All</i> months within the range [-999_998..999_999] of years are
/// supported.
/// </para>
/// <para><see cref="JulianMonth"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct JulianMonth :
    IMonth<JulianMonth>,
    IUnsafeFactory<JulianMonth>,
    // A month viewed as a finite sequence of days
    IDaySegment<JulianDate>,
    ISetMembership<JulianDate>,
    // Arithmetic
    ISubtractionOperators<JulianMonth, JulianMonth, int>
{ }

public partial struct JulianMonth // Preamble
{
    /// <summary>Represents the minimum value of <see cref="_monthsSinceEpoch"/>.
    /// <para>This field is a constant equal to -11_999_988.</para></summary>
    private const int MinMonthsSinceEpoch = -11_999_988;

    /// <summary>Represents the maximum value of <see cref="_monthsSinceEpoch"/>.
    /// <para>This field is a constant equal to 11_999_987.</para></summary>
    private const int MaxMonthsSinceEpoch = 11_999_987;

    /// <summary>
    /// Represents the count of consecutive months since the epoch
    /// <see cref="DayZero.NewStyle"/>.
    /// <para>This field is in the range from <see cref="MinMonthsSinceEpoch"/>
    /// to <see cref="MaxMonthsSinceEpoch"/>.</para>
    /// </summary>
    private readonly int _monthsSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianMonth"/> struct
    /// to the specified month components.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid month or <paramref name="year"/> is outside the
    /// range of supported years.</exception>
    public JulianMonth(int year, int month)
    {
        // The calendar being regular, no need to use the Scope:
        // > JulianScope.ValidateYearMonthImpl(year, month);
        if (year < JulianScope.MinYear || year > JulianScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year);
        if (month < 1 || month > JulianCalendar.MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(month);

        _monthsSinceEpoch = CountMonthsSinceEpoch(year, month);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianMonth"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal JulianMonth(int monthsSinceEpoch)
    {
        _monthsSinceEpoch = monthsSinceEpoch;
    }

    /// <summary>
    /// Gets the smallest possible value of <see cref="JulianMonth"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The earliest supported month.</returns>
    public static JulianMonth MinValue { get; } = new(MinMonthsSinceEpoch);

    /// <summary>
    /// Gets the largest possible value of <see cref="JulianMonth"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The latest supported month.</returns>
    public static JulianMonth MaxValue { get; } = new(MaxMonthsSinceEpoch);

    /// <summary>
    /// Gets the companion calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static JulianCalendar Calendar => JulianCalendar.Instance;

    static Calendar IMonth.Calendar => Calendar;

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
    /// <para>Actually, this property returns the algebraic year, but since its
    /// value is greater than 0, one can ignore this subtlety.</para>
    /// </summary>
    public int Year => 1 + MathZ.Divide(_monthsSinceEpoch, JulianCalendar.MonthsInYear);

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
    bool IMonth.IsIntercalary => false;

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        var (y, m) = this;
        return y > 0
            ? FormattableString.Invariant($"{m:D2}/{y:D4} ({JulianCalendar.DisplayName})")
            : FormattableString.Invariant($"{m:D2}/{getBCEYear(y)} BCE ({JulianCalendar.DisplayName})");

        [Pure]
        static int getBCEYear(int y)
        {
            Debug.Assert(y <= 0);
            var (pos, _) = Ord.FromInt32(y);
            return pos;
        }
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month)
    {
        // See RegularSchema.GetMonthParts().
        year = 1 + MathZ.Divide(_monthsSinceEpoch, JulianCalendar.MonthsInYear, out int m0);
        month = 1 + m0;
    }
}

public partial struct JulianMonth // IDaySegment
{
    /// <inheritdoc />
    public JulianDate MinDay
    {
        get
        {
            var (y, m) = this;
            int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(y, m, 1);
            return JulianDate.UnsafeCreate(daysSinceEpoch);
        }
    }

    /// <inheritdoc />
    public JulianDate MaxDay
    {
        get
        {
            var (y, m) = this;
            int d = JulianFormulae.CountDaysInMonth(y, m);
            int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(y, m, d);
            return JulianDate.UnsafeCreate(daysSinceEpoch);
        }
    }

    /// <inheritdoc />
    [Pure]
    public int CountDays()
    {
        var (y, m) = this;
        return JulianFormulae.CountDaysInMonth(y, m);
    }

    /// <summary>
    /// Converts the current instance to a range of days.
    /// </summary>
    [Pure]
    public Range<JulianDate> ToRange()
    {
        var (y, m) = this;
        int startOfMonth = JulianFormulae.CountDaysSinceEpoch(y, m, 1);
        int daysInMonth = JulianFormulae.CountDaysInMonth(y, m);
        return Range.StartingAt(JulianDate.UnsafeCreate(startOfMonth), daysInMonth);
    }

    [Pure]
    Range<JulianDate> IDaySegment<JulianDate>.ToDayRange() => ToRange();

    /// <summary>
    /// Returns an enumerable collection of all days in this month instance.
    /// </summary>
    [Pure]
    public IEnumerable<JulianDate> ToEnumerable()
    {
        var (y, m) = this;
        int startOfMonth = JulianFormulae.CountDaysSinceEpoch(y, m, 1);
        int daysInMonth = JulianFormulae.CountDaysInMonth(y, m);

        return from daysSinceEpoch
               in Enumerable.Range(startOfMonth, daysInMonth)
               select JulianDate.UnsafeCreate(daysSinceEpoch);
    }

    [Pure]
    IEnumerable<JulianDate> IDaySegment<JulianDate>.EnumerateDays() => ToEnumerable();

    /// <inheritdoc />
    [Pure]
    public bool Contains(JulianDate date)
    {
        var (y, m) = this;
        JulianFormulae.GetDateParts(date.DaysSinceEpoch, out int y1, out int m1, out _);
        return y1 == y && m1 == m;
    }

    /// <summary>
    /// Obtains the date corresponding to the specified day of this month
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfMonth"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public JulianDate GetDayOfMonth(int dayOfMonth)
    {
        var (y, m) = this; ;
        Calendar.Scope.PreValidator.ValidateDayOfMonth(y, m, dayOfMonth);
        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(y, m, dayOfMonth);
        return JulianDate.UnsafeCreate(daysSinceEpoch);
    }
}

public partial struct JulianMonth // Standard math ops
{
    /// <summary>
    /// Subtracts the two specified months and returns the number of months
    /// between them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountMonthsSince()")]
    public static int operator -(JulianMonth left, JulianMonth right) => left.CountMonthsSince(right);

    /// <summary>
    /// Adds a number of months to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported months.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusMonths()")]
    public static JulianMonth operator +(JulianMonth value, int months) => value.PlusMonths(months);

    /// <summary>
    /// Subtracts a number of months to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported months.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusMonths()")]
    public static JulianMonth operator -(JulianMonth value, int months) => value.PlusMonths(-months);

    /// <summary>
    /// Adds one month to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported month.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextMonth()")]
    public static JulianMonth operator ++(JulianMonth value) => value.NextMonth();

    /// <summary>
    /// Subtracts one month to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported month.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousMonth()")]
    public static JulianMonth operator --(JulianMonth value) => value.PreviousMonth();

    /// <summary>
    /// Counts the number of months elapsed since the specified month.
    /// </summary>
    [Pure]
    public int CountMonthsSince(JulianMonth other) =>
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
    public JulianMonth PlusMonths(int months)
    {
        int monthsSinceEpoch = checked(_monthsSinceEpoch + months);
        if (monthsSinceEpoch < MinMonthsSinceEpoch || monthsSinceEpoch > MaxMonthsSinceEpoch)
            ThrowHelpers.ThrowMonthOverflow();
        return new JulianMonth(monthsSinceEpoch);
    }

    /// <summary>
    /// Obtains the month after the current instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported month.</exception>
    [Pure]
    public JulianMonth NextMonth()
    {
        if (_monthsSinceEpoch == MaxMonthsSinceEpoch) ThrowHelpers.ThrowMonthOverflow();
        return new JulianMonth(_monthsSinceEpoch + 1);
    }

    /// <summary>
    /// Obtains the month before the current instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported month.</exception>
    [Pure]
    public JulianMonth PreviousMonth()
    {
        if (_monthsSinceEpoch == MinMonthsSinceEpoch) ThrowHelpers.ThrowMonthOverflow();
        return new JulianMonth(_monthsSinceEpoch - 1);
    }
}
