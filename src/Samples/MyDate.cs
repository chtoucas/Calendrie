// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

using Calendrie;
using Calendrie.Hemerology;

public readonly partial struct MyDate :
    IDate<MyDate, MyCalendar>,
    ISubtractionOperators<MyDate, MyDate, int>
{
    private static readonly DayNumber s_Epoch = MyCalendar.Instance.Epoch;

    private static readonly int s_MinDaysSinceEpoch = MyCalendar.Instance.MinDaysSinceEpoch;
    private static readonly int s_MaxDaysSinceEpoch = MyCalendar.Instance.MaxDaysSinceEpoch;

    private readonly int _daysSinceEpoch;

    public MyDate(int year, int month, int day)
    {
        _daysSinceEpoch = Calendar.CountDaysSinceEpoch(year, month, day);
    }

    public MyDate(int year, int dayOfYear)
    {
        _daysSinceEpoch = Calendar.CountDaysSinceEpoch(year, dayOfYear);
    }

    internal MyDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    public static MyDate MinValue { get; } = new(s_MinDaysSinceEpoch);
    public static MyDate MaxValue { get; } = new(s_MaxDaysSinceEpoch);

    public static MyCalendar Calendar => MyCalendar.Instance;

    public DayNumber DayNumber => s_Epoch + _daysSinceEpoch;
    public int DaysSinceEpoch => _daysSinceEpoch;

    public Ord CenturyOfEra => Ord.FromInt32(Century);
    public int Century => YearNumbering.GetCentury(Year);
    public Ord YearOfEra => Ord.FromInt32(Year);
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);
    public int Year => Calendar.GetYear(_daysSinceEpoch, out _);

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
    public bool IsSupplementary => Calendar.IsSupplementaryDay(_daysSinceEpoch);

    public void Deconstruct(out int year, out int month, out int day) =>
        Calendar.GetDateParts(_daysSinceEpoch, out year, out month, out day);

    public void Deconstruct(out int year, out int dayOfYear) =>
        year = Calendar.GetYear(_daysSinceEpoch, out dayOfYear);
}

public partial struct MyDate // Factories & conversions
{
    public static MyDate FromDayNumber(DayNumber dayNumber) => Calendar.GetDate(dayNumber);
}

public partial struct MyDate // Counting
{
    public int CountElapsedDaysInYear() => throw new NotImplementedException();
    public int CountRemainingDaysInYear() => throw new NotImplementedException();
    public int CountElapsedDaysInMonth() => throw new NotImplementedException();
    public int CountRemainingDaysInMonth() => throw new NotImplementedException();
}

public partial struct MyDate // Adjustments
{
    public MyDate Adjust(Func<MyDate, MyDate> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);
        return adjuster.Invoke(this);
    }

    public MyDate WithYear(int newYear) => Calendar.AdjustYear(this, newYear);
    public MyDate WithMonth(int newMonth) => Calendar.AdjustMonth(this, newMonth);
    public MyDate WithDay(int newDay) => Calendar.AdjustDay(this, newDay);
    public MyDate WithDayOfYear(int newDayOfYear) => Calendar.AdjustDayOfYear(this, newDayOfYear);

    public MyDate Previous(DayOfWeek dayOfWeek) => Calendar.Previous(this, dayOfWeek);
    public MyDate PreviousOrSame(DayOfWeek dayOfWeek) => Calendar.PreviousOrSame(this, dayOfWeek);
    public MyDate Nearest(DayOfWeek dayOfWeek) => Calendar.Nearest(this, dayOfWeek);
    public MyDate NextOrSame(DayOfWeek dayOfWeek) => Calendar.NextOrSame(this, dayOfWeek);
    public MyDate Next(DayOfWeek dayOfWeek) => Calendar.Next(this, dayOfWeek);
}

public partial struct MyDate // IEquatable
{
    public static bool operator ==(MyDate left, MyDate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;
    public static bool operator !=(MyDate left, MyDate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    public bool Equals(MyDate other) => _daysSinceEpoch == other._daysSinceEpoch;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is MyDate date && Equals(date);

    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct MyDate // IComparable
{
    public static bool operator <(MyDate left, MyDate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;
    public static bool operator <=(MyDate left, MyDate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;
    public static bool operator >(MyDate left, MyDate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;
    public static bool operator >=(MyDate left, MyDate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    public static MyDate Min(MyDate x, MyDate y) => x < y ? x : y;
    public static MyDate Max(MyDate x, MyDate y) => x > y ? x : y;

    public int CompareTo(MyDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is MyDate date ? CompareTo(date)
        : throw new ArgumentException(
            $"The object should be of type {typeof(MyDate)} but it is of type {obj.GetType()}.",
            nameof(obj));
}

public partial struct MyDate // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates

    public static int operator -(MyDate left, MyDate right) => left.CountDaysSince(right);
    public static MyDate operator +(MyDate value, int days) => value.AddDays(days);
    public static MyDate operator -(MyDate value, int days) => value.AddDays(-days);
    public static MyDate operator ++(MyDate value) => value.NextDay();
    public static MyDate operator --(MyDate value) => value.PreviousDay();

#pragma warning restore CA2225 // Operator overloads have named alternates

    public int CountDaysSince(MyDate other) => _daysSinceEpoch - other._daysSinceEpoch;

    public MyDate AddDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);

        if (daysSinceEpoch < s_MinDaysSinceEpoch || daysSinceEpoch > s_MaxDaysSinceEpoch)
            throw new OverflowException();

        return new(daysSinceEpoch);
    }

    public MyDate NextDay()
    {
        if (this == MaxValue) throw new OverflowException();
        return new(_daysSinceEpoch + 1);
    }

    public MyDate PreviousDay()
    {
        if (this == MinValue) throw new OverflowException();
        return new(_daysSinceEpoch - 1);
    }
}
