// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

using Calendrie;
using Calendrie.Hemerology;

public readonly partial struct MyGregorianDate :
    IDateable,
    IAbsoluteDate<MyGregorianDate>,
    ISubtractionOperators<MyGregorianDate, MyGregorianDate, int>
{
    private static readonly DayNumber s_Epoch = MyGregorianCalendar.Instance.Epoch;

    private static readonly int s_MinDaysSinceEpoch = MyGregorianCalendar.Instance.MinDaysSinceEpoch;
    private static readonly int s_MaxDaysSinceEpoch = MyGregorianCalendar.Instance.MaxDaysSinceEpoch;

    private readonly int _daysSinceEpoch;

    public MyGregorianDate(int year, int month, int day)
    {
        _daysSinceEpoch = Calendar.CountDaysSinceEpoch(year, month, day);
    }

    public MyGregorianDate(int year, int dayOfYear)
    {
        _daysSinceEpoch = Calendar.CountDaysSinceEpoch(year, dayOfYear);
    }

    internal MyGregorianDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    public static MyGregorianDate MinValue { get; } = new(s_MinDaysSinceEpoch);
    public static MyGregorianDate MaxValue { get; } = new(s_MaxDaysSinceEpoch);

    public static MyGregorianCalendar Calendar => MyGregorianCalendar.Instance;

    public DayNumber DayNumber => s_Epoch + _daysSinceEpoch;
    public int DaysSinceEpoch => _daysSinceEpoch;

    public Ord CenturyOfEra => Ord.FromInt32(Century);
    public int Century => YearNumbering.GetCentury(Year);
    public Ord YearOfEra => Ord.FromInt32(Year);
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);
    public int Year => Calendar.GetYear(_daysSinceEpoch);

    public int Month
    {
        get
        {
            Calendar.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
            return m;
        }
    }

    public int DayOfYear
    {
        get
        {
            _ = Calendar.GetYear(_daysSinceEpoch, out int doy);
            return doy;
        }
    }

    public int Day
    {
        get
        {
            Calendar.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
            return d;
        }
    }

    public DayOfWeek DayOfWeek => DayNumber.DayOfWeek;

    public bool IsIntercalary => Calendar.IsIntercalaryDay(_daysSinceEpoch);
    bool IDateable.IsSupplementary => false;

    public override string ToString()
    {
        Calendar.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({MyGregorianCalendar.DisplayName})");
    }

    public void Deconstruct(out int year, out int month, out int day) =>
        Calendar.GetDateParts(_daysSinceEpoch, out year, out month, out day);

    public void Deconstruct(out int year, out int dayOfYear) =>
        year = Calendar.GetYear(_daysSinceEpoch, out dayOfYear);
}

public partial struct MyGregorianDate // Factories & conversions
{
    // This factory method eventually throws an OverflowException, not an
    // ArgumentOutOfRangeException as documented in the XML doc. Only used by
    // Nearest() via IAbsoluteDate.Nearest().
    static MyGregorianDate IAbsoluteDate<MyGregorianDate>.FromDayNumber(DayNumber dayNumber) =>
        new(Calendar.CountDaysSinceEpochChecked(dayNumber));

    public static MyGregorianDate FromDayNumber(DayNumber dayNumber) =>
        new(Calendar.CountDaysSinceEpoch(dayNumber));
}

public partial struct MyGregorianDate // Counting
{
    public int CountElapsedDaysInYear() => DayOfYear - 1;
    public int CountRemainingDaysInYear() => Calendar.CountDaysInYearAfter(_daysSinceEpoch);
    public int CountElapsedDaysInMonth() => Day - 1;
    public int CountRemainingDaysInMonth() => Calendar.CountDaysInMonthAfter(_daysSinceEpoch);
}

public partial struct MyGregorianDate // Adjustments
{
    public MyGregorianDate WithYear(int newYear) => Calendar.AdjustYear(this, newYear);
    public MyGregorianDate WithMonth(int newMonth) => Calendar.AdjustMonth(this, newMonth);
    public MyGregorianDate WithDay(int newDay) => Calendar.AdjustDayOfMonth(this, newDay);
    public MyGregorianDate WithDayOfYear(int newDayOfYear) => Calendar.AdjustDayOfYear(this, newDayOfYear);

    // NB: do not change this as it's the date type we use to test IAbsoluteDate static methods.
    public MyGregorianDate Previous(DayOfWeek dayOfWeek) => IAbsoluteDate.Previous(this, dayOfWeek);
    public MyGregorianDate PreviousOrSame(DayOfWeek dayOfWeek) => IAbsoluteDate.PreviousOrSame(this, dayOfWeek);
    public MyGregorianDate Nearest(DayOfWeek dayOfWeek) => IAbsoluteDate.Nearest(this, dayOfWeek);
    public MyGregorianDate NextOrSame(DayOfWeek dayOfWeek) => IAbsoluteDate.NextOrSame(this, dayOfWeek);
    public MyGregorianDate Next(DayOfWeek dayOfWeek) => IAbsoluteDate.Next(this, dayOfWeek);
}

public partial struct MyGregorianDate // IEquatable
{
    public static bool operator ==(MyGregorianDate left, MyGregorianDate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;
    public static bool operator !=(MyGregorianDate left, MyGregorianDate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    public bool Equals(MyGregorianDate other) => _daysSinceEpoch == other._daysSinceEpoch;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is MyGregorianDate date && Equals(date);

    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct MyGregorianDate // IComparable
{
    public static bool operator <(MyGregorianDate left, MyGregorianDate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;
    public static bool operator <=(MyGregorianDate left, MyGregorianDate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;
    public static bool operator >(MyGregorianDate left, MyGregorianDate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;
    public static bool operator >=(MyGregorianDate left, MyGregorianDate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    public static MyGregorianDate Min(MyGregorianDate x, MyGregorianDate y) => x < y ? x : y;
    public static MyGregorianDate Max(MyGregorianDate x, MyGregorianDate y) => x > y ? x : y;

    public int CompareTo(MyGregorianDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is MyGregorianDate date ? CompareTo(date)
        : throw new ArgumentException(
            $"The object should be of type {typeof(MyGregorianDate)} but it is of type {obj.GetType()}.",
            nameof(obj));
}

public partial struct MyGregorianDate // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates

    public static int operator -(MyGregorianDate left, MyGregorianDate right) => left.CountDaysSince(right);
    public static MyGregorianDate operator +(MyGregorianDate value, int days) => value.PlusDays(days);
    public static MyGregorianDate operator -(MyGregorianDate value, int days) => value.PlusDays(-days);
    public static MyGregorianDate operator ++(MyGregorianDate value) => value.NextDay();
    public static MyGregorianDate operator --(MyGregorianDate value) => value.PreviousDay();

#pragma warning restore CA2225 // Operator overloads have named alternates

    public int CountDaysSince(MyGregorianDate other) => _daysSinceEpoch - other._daysSinceEpoch;

    public MyGregorianDate PlusDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);

        if (daysSinceEpoch < s_MinDaysSinceEpoch || daysSinceEpoch > s_MaxDaysSinceEpoch)
            throw new OverflowException();

        return new(daysSinceEpoch);
    }

    public MyGregorianDate NextDay() =>
        this == MaxValue ? throw new OverflowException() : new(_daysSinceEpoch + 1);

    public MyGregorianDate PreviousDay() =>
        this == MinValue ? throw new OverflowException() : new(_daysSinceEpoch - 1);
}
