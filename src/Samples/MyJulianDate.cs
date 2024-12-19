// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

using Calendrie;
using Calendrie.Core;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

public readonly partial struct MyJulianDate :
    IDate<MyJulianDate, MyJulianCalendar>,
    ISubtractionOperators<MyJulianDate, MyJulianDate, int>
{
    private static readonly DayNumber s_Epoch = MyJulianCalendar.Instance.Epoch;

    private readonly Yemoda _bin;

    public MyJulianDate(int year, int month, int day)
    {
        _bin = Calendar.GetDate(year, month, day);
    }

    public MyJulianDate(int year, int dayOfYear)
    {
        _bin = Calendar.GetDate(year, dayOfYear);
    }

    private MyJulianDate(Yemoda bin) { _bin = bin; }

    public static MyJulianDate MinValue => throw new NotImplementedException();
    public static MyJulianDate MaxValue => throw new NotImplementedException();

    public static MyJulianCalendar Calendar => MyJulianCalendar.Instance;

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
    public DayOfWeek DayOfWeek =>
        (DayOfWeek)Modulo((int)checked(s_Epoch.DayOfWeek + DaysSinceEpoch), DaysInWeek);

    public bool IsIntercalary => Calendar.IsIntercalaryDay(_bin);
    public bool IsSupplementary => Calendar.IsSupplementaryDay(_bin);

    public override string ToString()
    {
        var (y, m, d) = _bin;
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({MyJulianCalendar.DisplayName})");
    }

    public void Deconstruct(out int year, out int month, out int day) =>
        (year, month, day) = _bin;

    public void Deconstruct(out int year, out int dayOfYear) =>
        year = Calendar.GetOrdinalParts(_bin, out dayOfYear);

    //
    // Helpers
    //

    private static int Modulo(int m, int n)
    {
        Debug.Assert(n > 0);

        int r = m % n;
        return r >= 0 ? r : (r + n);
    }
}

public partial struct MyJulianDate // Factories & conversions
{
    public static MyJulianDate FromDayNumber(DayNumber dayNumber) =>
        new(Calendar.GetDate(dayNumber));
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
    public MyJulianDate Adjust(Func<MyJulianDate, MyJulianDate> adjuster) => throw new NotImplementedException();

    public MyJulianDate WithYear(int newYear) => throw new NotImplementedException();
    public MyJulianDate WithMonth(int newMonth) => throw new NotImplementedException();
    public MyJulianDate WithDay(int newDay) => throw new NotImplementedException();
    public MyJulianDate WithDayOfYear(int newDayOfYear) => throw new NotImplementedException();

    public MyJulianDate Previous(DayOfWeek dayOfWeek) => throw new NotImplementedException();
    public MyJulianDate PreviousOrSame(DayOfWeek dayOfWeek) => throw new NotImplementedException();
    public MyJulianDate Nearest(DayOfWeek dayOfWeek) => throw new NotImplementedException();
    public MyJulianDate NextOrSame(DayOfWeek dayOfWeek) => throw new NotImplementedException();
    public MyJulianDate Next(DayOfWeek dayOfWeek) => throw new NotImplementedException();
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
    public static MyJulianDate operator +(MyJulianDate value, int days) => value.AddDays(days);
    public static MyJulianDate operator -(MyJulianDate value, int days) => value.AddDays(-days);
    public static MyJulianDate operator ++(MyJulianDate value) => value.NextDay();
    public static MyJulianDate operator --(MyJulianDate value) => value.PreviousDay();

#pragma warning restore CA2225 // Operator overloads have named alternates

    public int CountDaysSince(MyJulianDate other) => Calendar.CountDaysBetween(other._bin, _bin);
    public MyJulianDate AddDays(int days) => new(Calendar.AddDays(_bin, days));
    public MyJulianDate NextDay() => new(Calendar.NextDay(_bin));
    public MyJulianDate PreviousDay() => new(Calendar.PreviousDay(_bin));
}
