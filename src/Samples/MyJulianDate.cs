﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

#if FORCE_ENABLE_GENERIC_FACTORIES || ENABLE_GENERIC_FACTORIES

using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

using Calendrie;
using Calendrie.Core;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

public readonly partial struct MyJulianDate :
    IDate<MyJulianDate>,
    ISubtractionOperators<MyJulianDate, MyJulianDate, int>
{
    private static readonly DayNumber s_Epoch = MyJulianCalendar.Instance.Epoch;

    private readonly Yemoda _bin;

    public MyJulianDate(int year, int month, int day)
    {
        _bin = Calendar.CreateDateParts(year, month, day);
    }

    public MyJulianDate(int year, int dayOfYear)
    {
        _bin = Calendar.CreateDateParts(year, dayOfYear);
    }

    internal MyJulianDate(Yemoda bin) { _bin = bin; }

    public static MyJulianDate MinValue { get; } = new(MyJulianCalendar.Instance.MinDateParts);
    public static MyJulianDate MaxValue { get; } = new(MyJulianCalendar.Instance.MaxDateParts);

    public static MyJulianCalendar Calendar => MyJulianCalendar.Instance;

    static Calendar IDate.Calendar => Calendar;

    public DayNumber DayNumber => s_Epoch + DaysSinceEpoch;
    public int DaysSinceEpoch => Calendar.CountDaysSinceEpoch(_bin);

    public Ord CenturyOfEra => Ord.FromInt32(Century);
    public int Century => YearNumbering.GetCentury(_bin.Year);
    public Ord YearOfEra => Ord.FromInt32(_bin.Year);
    public int YearOfCentury => YearNumbering.GetYearOfCentury(_bin.Year);
    public int Year => _bin.Year;
    public int Month => _bin.Month;
    public int DayOfYear => Calendar.GetDayOfYear(_bin);
    public int Day => _bin.Day;

    public DayOfWeek DayOfWeek
    {
        get
        {
            return (DayOfWeek)mod((int)(s_Epoch.DayOfWeek + DaysSinceEpoch), DaysInWeek);

            static int mod(int m, int n)
            {
                int r = m % n;
                return r >= 0 ? r : (r + n);
            }
        }
    }

    public bool IsIntercalary => Calendar.IsIntercalaryDay(_bin);
    bool IDateable.IsSupplementary => false;

    public override string ToString()
    {
        var (y, m, d) = _bin;
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({MyJulianCalendar.DisplayName})");
    }

    public void Deconstruct(out int year, out int month, out int day) =>
        (year, month, day) = _bin;

    public void Deconstruct(out int year, out int dayOfYear) =>
        (year, dayOfYear) = Calendar.GetOrdinalParts(_bin);
}

public partial struct MyJulianDate // Factories & conversions
{
    public static MyJulianDate Create(int year, int month, int day) => new(year, month, day);
    public static MyJulianDate Create(int year, int dayOfYear) => new(year, dayOfYear);

    public static MyJulianDate? TryCreate(int year, int month, int day) => throw new NotImplementedException();
    public static MyJulianDate? TryCreate(int year, int dayOfYear) => throw new NotImplementedException();

    // Explicit implementation: MyJulianDate being a value type, better to use
    // the others TryCreate().

    static bool IDate<MyJulianDate>.TryCreate(int year, int month, int day, out MyJulianDate result)
    {
        var date = TryCreate(year, month, day);
        result = date ?? default;
        return date.HasValue;
    }

    static bool IDate<MyJulianDate>.TryCreate(int year, int dayOfYear, out MyJulianDate result)
    {
        var date = TryCreate(year, dayOfYear);
        result = date ?? default;
        return date.HasValue;
    }

    public static MyJulianDate FromDayNumber(DayNumber dayNumber) =>
        new(Calendar.CreateDateParts(dayNumber));
}

public partial struct MyJulianDate // Counting
{
    public int CountElapsedDaysInYear() => DayOfYear - 1;
    public int CountRemainingDaysInYear() => Calendar.CountDaysInYearAfter(_bin);
    public int CountElapsedDaysInMonth() => _bin.Day - 1;
    public int CountRemainingDaysInMonth() => Calendar.CountDaysInMonthAfter(_bin);
}

public partial struct MyJulianDate // Adjustments
{
    public MyJulianDate WithYear(int newYear) => Calendar.AdjustYear(this, newYear);
    public MyJulianDate WithMonth(int newMonth) => Calendar.AdjustMonth(this, newMonth);
    public MyJulianDate WithDay(int newDay) => Calendar.AdjustDayOfMonth(this, newDay);
    public MyJulianDate WithDayOfYear(int newDayOfYear) => Calendar.AdjustDayOfYear(this, newDayOfYear);

    public MyJulianDate Previous(DayOfWeek dayOfWeek) => IAbsoluteDate.Previous(this, dayOfWeek);
    public MyJulianDate PreviousOrSame(DayOfWeek dayOfWeek) => IAbsoluteDate.PreviousOrSame(this, dayOfWeek);
    public MyJulianDate Nearest(DayOfWeek dayOfWeek) => Calendar.Nearest(this, dayOfWeek);
    public MyJulianDate NextOrSame(DayOfWeek dayOfWeek) => IAbsoluteDate.NextOrSame(this, dayOfWeek);
    public MyJulianDate Next(DayOfWeek dayOfWeek) => IAbsoluteDate.Next(this, dayOfWeek);
}

public partial struct MyJulianDate // IEquatable
{
    public static bool operator ==(MyJulianDate left, MyJulianDate right) => left._bin == right._bin;
    public static bool operator !=(MyJulianDate left, MyJulianDate right) => left._bin != right._bin;

    public bool Equals(MyJulianDate other) => _bin == other._bin;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is MyJulianDate date && Equals(date);

    public override int GetHashCode() => _bin.GetHashCode();
}

public partial struct MyJulianDate // IComparable
{
    public static bool operator <(MyJulianDate left, MyJulianDate right) => left._bin < right._bin;
    public static bool operator <=(MyJulianDate left, MyJulianDate right) => left._bin <= right._bin;
    public static bool operator >(MyJulianDate left, MyJulianDate right) => left._bin > right._bin;
    public static bool operator >=(MyJulianDate left, MyJulianDate right) => left._bin >= right._bin;

    public static MyJulianDate Min(MyJulianDate x, MyJulianDate y) => x < y ? x : y;
    public static MyJulianDate Max(MyJulianDate x, MyJulianDate y) => x > y ? x : y;

    public int CompareTo(MyJulianDate other) => _bin.CompareTo(other._bin);

    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is MyJulianDate date ? CompareTo(date)
        : throw new ArgumentException(
            $"The object should be of type {typeof(MyJulianDate)} but it is of type {obj.GetType()}.",
            nameof(obj));
}

public partial struct MyJulianDate // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates

    public static int operator -(MyJulianDate left, MyJulianDate right) => left.CountDaysSince(right);
    public static MyJulianDate operator +(MyJulianDate value, int days) => value.PlusDays(days);
    public static MyJulianDate operator -(MyJulianDate value, int days) => value.PlusDays(-days);
    public static MyJulianDate operator ++(MyJulianDate value) => value.NextDay();
    public static MyJulianDate operator --(MyJulianDate value) => value.PreviousDay();

#pragma warning restore CA2225 // Operator overloads have named alternates

    public int CountDaysSince(MyJulianDate other) => Calendar.CountDaysBetween(other._bin, _bin);
    public MyJulianDate PlusDays(int days) => new(Calendar.AddDays(_bin, days));
    public MyJulianDate NextDay() => new(Calendar.NextDay(_bin));
    public MyJulianDate PreviousDay() => new(Calendar.PreviousDay(_bin));

    public MyJulianDate PlusYears(int years) => throw new NotImplementedException();
    public int CountYearsSince(MyJulianDate other) => throw new NotImplementedException();

    public MyJulianDate PlusMonths(int months) => throw new NotImplementedException();
    public int CountMonthsSince(MyJulianDate other) => throw new NotImplementedException();
}

#endif
