// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Faux;

using System.Numerics;

using Calendrie;
using Calendrie.Hemerology;

// NB: not really a "faux". This class is used to test
// - IDateable.CountElapsedDaysInYear()
// - IDateable.CountElapsedDaysInMonth()
// - IAbsoluteDate.Previous()
// - IAbsoluteDate.PreviousOrSame()
// - IAbsoluteDate.Nearest()
// - IAbsoluteDate.NextOrSame()
// - IAbsoluteDate.Next()
// - IDayFieldMath<TSelf>.NextDay()
// - IDayFieldMath<TSelf>.PreviousDay()
// - IMonthFieldMath<TSelf>.NextMonth()
// - IMonthFieldMath<TSelf>.PreviousMonth()
// - IYearFieldMath<TSelf>.NextMonth()
// - IYearFieldMath<TSelf>.PreviousMonth()

public readonly partial struct FauxGregorianDate :
    IDate<FauxGregorianDate>,
    ISubtractionOperators<FauxGregorianDate, FauxGregorianDate, int>
{
    private static readonly DayNumber s_Epoch = FauxGregorianCalendar.Instance.Epoch;

    private const int MinDaysSinceEpoch = 0;
    private static readonly int s_MaxDaysSinceEpoch = FauxGregorianCalendar.Instance.MaxDaysSinceEpoch;

    public FauxGregorianDate(int year, int month, int day)
    {
        DaysSinceEpoch = Calendar.CountDaysSinceEpoch(year, month, day);
    }

    public FauxGregorianDate(int year, int dayOfYear)
    {
        DaysSinceEpoch = Calendar.CountDaysSinceEpoch(year, dayOfYear);
    }

    internal FauxGregorianDate(int daysSinceEpoch)
    {
        DaysSinceEpoch = daysSinceEpoch;
    }

    // NB: MinValue = new(MinDaysSinceEpoch) = new(0) = default
    public static FauxGregorianDate MinValue { get; }
    public static FauxGregorianDate MaxValue { get; } = new(s_MaxDaysSinceEpoch);

    public static FauxGregorianCalendar Calendar => FauxGregorianCalendar.Instance;

    static Calendar IDate.Calendar => Calendar;

    public DayNumber DayNumber => s_Epoch + DaysSinceEpoch;
    public int DaysSinceEpoch { get; }

    public Ord CenturyOfEra => Ord.FromInt32(Century);
    public int Century => YearNumbering.GetCentury(Year);
    public Ord YearOfEra => Ord.FromInt32(Year);
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);
    public int Year => Calendar.GetYear(DaysSinceEpoch);

    public int Month
    {
        get
        {
            Calendar.GetDateParts(DaysSinceEpoch, out _, out int m, out _);
            return m;
        }
    }

    public int DayOfYear
    {
        get
        {
            _ = Calendar.GetYear(DaysSinceEpoch, out int doy);
            return doy;
        }
    }

    public int Day
    {
        get
        {
            Calendar.GetDateParts(DaysSinceEpoch, out _, out _, out int d);
            return d;
        }
    }

    public DayOfWeek DayOfWeek => DayNumber.DayOfWeek;

    public bool IsIntercalary => Calendar.IsIntercalaryDay(DaysSinceEpoch);
    bool IDateable.IsSupplementary => false;

    public override string ToString()
    {
        var (y, m, d) = this;
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({Calendar})");
    }
    public void Deconstruct(out int year, out int month, out int day) =>
        Calendar.GetDateParts(DaysSinceEpoch, out year, out month, out day);

    public void Deconstruct(out int year, out int dayOfYear) =>
        year = Calendar.GetYear(DaysSinceEpoch, out dayOfYear);
}

public partial struct FauxGregorianDate // Factories & conversions
{
    public static FauxGregorianDate Create(int year, int month, int day) => new(year, month, day);
    public static FauxGregorianDate Create(int year, int dayOfYear) => new(year, dayOfYear);

    public static FauxGregorianDate? TryCreate(int year, int month, int day)
    {
        int? daysSinceEpoch = Calendar.TryCountDaysSinceEpoch(year, month, day);
        return daysSinceEpoch.HasValue ? new(daysSinceEpoch.Value) : null;
    }

    public static FauxGregorianDate? TryCreate(int year, int dayOfYear)
    {
        int? daysSinceEpoch = Calendar.TryCountDaysSinceEpoch(year, dayOfYear);
        return daysSinceEpoch.HasValue ? new(daysSinceEpoch.Value) : null;
    }

    // Explicit implementation: MyGregorianDate being a value type, better to use
    // the others TryCreate().

    static bool IDate<FauxGregorianDate>.TryCreate(int year, int month, int day, out FauxGregorianDate result)
    {
        var date = TryCreate(year, month, day);
        result = date ?? default;
        return date.HasValue;
    }

    static bool IDate<FauxGregorianDate>.TryCreate(int year, int dayOfYear, out FauxGregorianDate result)
    {
        var date = TryCreate(year, dayOfYear);
        result = date ?? default;
        return date.HasValue;
    }

    public static FauxGregorianDate FromDayNumber(DayNumber dayNumber) =>
        new(Calendar.CountDaysSinceEpoch(dayNumber));

    // This method eventually throws an OverflowException, not an
    // ArgumentOutOfRangeException as documented in the XML doc.
    // Why? It's to ensure that Nearest(), which uses this explicit impl via
    // IAbsoluteDate.Nearest(), throws an ArgumentOutOfRangeException only when
    // dayOfWeek is invalid and an OverflowException otherwise.
    // Here, it's the only case where we use this method.
    // Of course, one can always call this method explicitely.
    // Pourquoi on n'a pas ce problème avec les autres types date de ce projet ?
    // La réponse est simplement parce que Nearest() n'utilise pas
    // IAbsoluteDate.Nearest() mais DayNumber.Nearest().
    static FauxGregorianDate IAbsoluteDate<FauxGregorianDate>.FromDayNumber(DayNumber dayNumber) =>
        new(Calendar.CountDaysSinceEpochChecked(dayNumber));
}

public partial struct FauxGregorianDate // Counting
{
    public int CountRemainingDaysInYear() => Calendar.CountDaysInYearAfter(DaysSinceEpoch);
    public int CountRemainingDaysInMonth() => Calendar.CountDaysInMonthAfter(DaysSinceEpoch);
}

public partial struct FauxGregorianDate // Adjustments
{
    public FauxGregorianDate WithYear(int newYear) => Calendar.AdjustYear(this, newYear);
    public FauxGregorianDate WithMonth(int newMonth) => Calendar.AdjustMonth(this, newMonth);
    public FauxGregorianDate WithDay(int newDay) => Calendar.AdjustDayOfMonth(this, newDay);
    public FauxGregorianDate WithDayOfYear(int newDayOfYear) => Calendar.AdjustDayOfYear(this, newDayOfYear);

    // NB: do not change this as it's the date type we use to test IAbsoluteDate static methods.
    public FauxGregorianDate Previous(DayOfWeek dayOfWeek) => IAbsoluteDate.Previous(this, dayOfWeek);
    public FauxGregorianDate PreviousOrSame(DayOfWeek dayOfWeek) => IAbsoluteDate.PreviousOrSame(this, dayOfWeek);
    public FauxGregorianDate Nearest(DayOfWeek dayOfWeek) => IAbsoluteDate.Nearest(this, dayOfWeek);
    public FauxGregorianDate NextOrSame(DayOfWeek dayOfWeek) => IAbsoluteDate.NextOrSame(this, dayOfWeek);
    public FauxGregorianDate Next(DayOfWeek dayOfWeek) => IAbsoluteDate.Next(this, dayOfWeek);
}

public partial struct FauxGregorianDate // IEquatable
{
    public static bool operator ==(FauxGregorianDate left, FauxGregorianDate right) =>
        left.DaysSinceEpoch == right.DaysSinceEpoch;
    public static bool operator !=(FauxGregorianDate left, FauxGregorianDate right) =>
        left.DaysSinceEpoch != right.DaysSinceEpoch;

    public bool Equals(FauxGregorianDate other) => DaysSinceEpoch == other.DaysSinceEpoch;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is FauxGregorianDate date && Equals(date);

    public override int GetHashCode() => DaysSinceEpoch;
}

public partial struct FauxGregorianDate // IComparable
{
    public static bool operator <(FauxGregorianDate left, FauxGregorianDate right) =>
        left.DaysSinceEpoch < right.DaysSinceEpoch;
    public static bool operator <=(FauxGregorianDate left, FauxGregorianDate right) =>
        left.DaysSinceEpoch <= right.DaysSinceEpoch;
    public static bool operator >(FauxGregorianDate left, FauxGregorianDate right) =>
        left.DaysSinceEpoch > right.DaysSinceEpoch;
    public static bool operator >=(FauxGregorianDate left, FauxGregorianDate right) =>
        left.DaysSinceEpoch >= right.DaysSinceEpoch;

    public static FauxGregorianDate Min(FauxGregorianDate x, FauxGregorianDate y) => x < y ? x : y;
    public static FauxGregorianDate Max(FauxGregorianDate x, FauxGregorianDate y) => x > y ? x : y;

    public int CompareTo(FauxGregorianDate other) => DaysSinceEpoch.CompareTo(other.DaysSinceEpoch);

    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is FauxGregorianDate date ? CompareTo(date)
        : throw new ArgumentException(
            $"The object should be of type {typeof(FauxGregorianDate)} but it is of type {obj.GetType()}.",
            nameof(obj));
}

public partial struct FauxGregorianDate // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates
    public static int operator -(FauxGregorianDate left, FauxGregorianDate right) => left.CountDaysSince(right);
    public static FauxGregorianDate operator +(FauxGregorianDate value, int days) => value.PlusDays(days);
    public static FauxGregorianDate operator -(FauxGregorianDate value, int days) => value.PlusDays(-days);
    public static FauxGregorianDate operator ++(FauxGregorianDate value) => value.PlusDays(1);
    public static FauxGregorianDate operator --(FauxGregorianDate value) => value.PlusDays(-1);
#pragma warning restore CA2225 // Operator overloads have named alternates

    public int CountDaysSince(FauxGregorianDate other) => DaysSinceEpoch - other.DaysSinceEpoch;

    public FauxGregorianDate PlusDays(int days)
    {
        int daysSinceEpoch = checked(DaysSinceEpoch + days);

        return daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > s_MaxDaysSinceEpoch
            ? throw new OverflowException()
            : new(daysSinceEpoch);
    }

    public FauxGregorianDate PlusYears(int years)
    {
        var (y, m, d) = this;
        return Calendar.AddYears(y, m, d, years);
    }

    public FauxGregorianDate PlusMonths(int months)
    {
        var (y, m, d) = this;
        return Calendar.AddMonths(y, m, d, months);
    }

    public FauxGregorianDate PlusYears(int years, out int roundoff)
    {
        var (y, m, d) = this;
        return Calendar.AddYears(y, m, d, years, out roundoff);
    }

    public FauxGregorianDate PlusMonths(int months, out int roundoff)
    {
        var (y, m, d) = this;
        return Calendar.AddMonths(y, m, d, months, out roundoff);
    }

    public int CountYearsSince(FauxGregorianDate other)
    {
        // Exact difference between two calendar years.
        int years = Year - other.Year;

        var newStart = other.PlusYears(years);
        if (other < this)
        {
            if (newStart > this) years--;
        }
        else
        {
            if (newStart < this) years++;
        }

        return years;
    }

    public int CountMonthsSince(FauxGregorianDate other)
    {
        var (y0, m0, _) = other;
        var (y, m, _) = this;

        // Exact difference between two calendar months.
        int months = checked(FauxGregorianCalendar.MonthsInYear * (y - y0) + m - m0);

        var newStart = other.PlusMonths(months);

        if (other < this)
        {
            if (newStart > this) months--;
        }
        else
        {
            if (newStart < this) months++;
        }

        return months;
    }
}
