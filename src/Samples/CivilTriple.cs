﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

using Calendrie;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

// Demonstration that almost all operations only depend on the schema.
//
// No epoch means no interconversion and no day of the week.
// Actually we can do everything since we focus on the Gregorian case, but
// let's keep the code as general as possible (only the Schema is specific here).

public readonly partial struct CivilTriple :
    IDateable,
    IAffineDate<CivilTriple>,
    ISubtractionOperators<CivilTriple, CivilTriple, int>
{
    public CivilTriple(int year, int month, int day)
    {
        CivilTripleScope.ValidateYearMonthDay(year, month, day);
        DaysSinceEpoch = Schema.CountDaysSinceEpoch(year, month, day);
    }

    public CivilTriple(int year, int dayOfYear)
    {
        CivilTripleScope.ValidateOrdinal(year, dayOfYear);
        DaysSinceEpoch = Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    private CivilTriple(int daysSinceEpoch)
    {
        DaysSinceEpoch = daysSinceEpoch;
    }

    // MinValue = new(MinDaysSinceEpoch) = new(0) = default
    public static CivilTriple MinValue { get; }
    public static CivilTriple MaxValue { get; } = new(CivilTripleScope.MaxDaysSinceEpoch);

    public int DaysSinceEpoch { get; }

    public Ord CenturyOfEra => Ord.FromInt32(Century);
    public int Century => YearNumbering.GetCentury(Year);
    public Ord YearOfEra => Ord.FromInt32(Year);
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);
    public int Year => Schema.GetYear(DaysSinceEpoch);

    public int Month
    {
        get
        {
            Schema.GetDateParts(DaysSinceEpoch, out _, out int m, out _);
            return m;
        }
    }

    public int DayOfYear
    {
        get
        {
            _ = Schema.GetYear(DaysSinceEpoch, out int doy);
            return doy;
        }
    }

    public int Day
    {
        get
        {
            Schema.GetDateParts(DaysSinceEpoch, out _, out _, out int d);
            return d;
        }
    }

    public bool IsIntercalary
    {
        get
        {
            Schema.GetDateParts(DaysSinceEpoch, out int y, out int m, out int d);
            return Schema.IsIntercalaryDay(y, m, d);
        }
    }

    bool IDateable.IsSupplementary => false;

    private static CivilPrototype Schema => CivilTripleScope.Schema;

    public override string ToString()
    {
        Schema.GetDateParts(DaysSinceEpoch, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} (Civil)");
    }

    public void Deconstruct(out int year, out int month, out int day) =>
        Schema.GetDateParts(DaysSinceEpoch, out year, out month, out day);

    public void Deconstruct(out int year, out int dayOfYear) =>
        year = Schema.GetYear(DaysSinceEpoch, out dayOfYear);
}

public partial struct CivilTriple // Factories
{
    public static CivilTriple FromDaysSinceEpoch(int daysSinceEpoch)
    {
        return CivilTripleScope.CheckDaysSinceEpoch(daysSinceEpoch)
            ? new(daysSinceEpoch)
            : throw new ArgumentOutOfRangeException(nameof(daysSinceEpoch));
    }
}

public partial struct CivilTriple // Counting
{
    public int CountElapsedDaysInYear() => DayOfYear - 1;

    public int CountRemainingDaysInYear()
    {
        var (y, doy) = this;
        return Schema.CountDaysInYear(y) - doy;
    }

    public int CountElapsedDaysInMonth() => Day - 1;

    public int CountRemainingDaysInMonth()
    {
        var (y, m, d) = this;
        return Schema.CountDaysInMonth(y, m) - d;
    }
}

public partial struct CivilTriple // IEquatable
{
    public static bool operator ==(CivilTriple left, CivilTriple right) =>
        left.DaysSinceEpoch == right.DaysSinceEpoch;
    public static bool operator !=(CivilTriple left, CivilTriple right) =>
        left.DaysSinceEpoch != right.DaysSinceEpoch;

    public bool Equals(CivilTriple other) => DaysSinceEpoch == other.DaysSinceEpoch;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CivilTriple date && Equals(date);

    public override int GetHashCode() => DaysSinceEpoch;
}

public partial struct CivilTriple // IComparable
{
    public static bool operator <(CivilTriple left, CivilTriple right) =>
        left.DaysSinceEpoch < right.DaysSinceEpoch;
    public static bool operator <=(CivilTriple left, CivilTriple right) =>
        left.DaysSinceEpoch <= right.DaysSinceEpoch;
    public static bool operator >(CivilTriple left, CivilTriple right) =>
        left.DaysSinceEpoch > right.DaysSinceEpoch;
    public static bool operator >=(CivilTriple left, CivilTriple right) =>
        left.DaysSinceEpoch >= right.DaysSinceEpoch;

    public static CivilTriple Min(CivilTriple x, CivilTriple y) => x < y ? x : y;
    public static CivilTriple Max(CivilTriple x, CivilTriple y) => x > y ? x : y;

    public int CompareTo(CivilTriple other) => DaysSinceEpoch.CompareTo(other.DaysSinceEpoch);

    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is CivilTriple date ? CompareTo(date)
        : throw new ArgumentException(
            $"The object should be of type {typeof(CivilTriple)} but it is of type {obj.GetType()}.",
            nameof(obj));
}

public partial struct CivilTriple // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates

    public static int operator -(CivilTriple left, CivilTriple right) => left.CountDaysSince(right);
    public static CivilTriple operator +(CivilTriple value, int days) => value.PlusDays(days);
    public static CivilTriple operator -(CivilTriple value, int days) => value.PlusDays(-days);
    public static CivilTriple operator ++(CivilTriple value) => value.NextDay();
    public static CivilTriple operator --(CivilTriple value) => value.PreviousDay();

#pragma warning restore CA2225 // Operator overloads have named alternates

    public int CountDaysSince(CivilTriple other) => DaysSinceEpoch - other.DaysSinceEpoch;

    public CivilTriple PlusDays(int days)
    {
        int daysSinceEpoch = checked(DaysSinceEpoch + days);
        return CivilTripleScope.CheckDaysSinceEpoch(daysSinceEpoch)
            ? new(daysSinceEpoch)
            : throw new OverflowException();
    }

    public CivilTriple NextDay() =>
        this == MaxValue ? throw new OverflowException() : new(DaysSinceEpoch + 1);

    public CivilTriple PreviousDay() =>
        this == MinValue ? throw new OverflowException() : new(DaysSinceEpoch - 1);

    public int CountWeeksSince(CivilTriple other)
    {
        return divide(CountDaysSince(other), DaysInWeek);

        static int divide(int m, int n) => m >= 0 || m % n == 0 ? m / n : (m / n - 1);
    }

    public CivilTriple PlusWeeks(int weeks) => PlusDays(DaysInWeek * weeks);
    public CivilTriple NextWeek() => PlusDays(DaysInWeek);
    public CivilTriple PreviousWeek() => PlusDays(-DaysInWeek);

    public CivilTriple PlusYears(int years)
    {
        var (y, m, d) = this;
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (!CivilTripleScope.CheckYear(newY)) throw new OverflowException();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, Schema.CountDaysInMonth(newY, m));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(newY, m, newD);
        return new CivilTriple(daysSinceEpoch);
    }

    public CivilTriple PlusMonths(int months)
    {
        var (y, m, d) = this;
        // Exact addition of months to a calendar month.
        int newM = 1 + modulo(checked(m - 1 + months), Schema.MonthsInYear, out int y0);
        int newY = checked(y + y0);
        if (!CivilTripleScope.CheckYear(newY)) throw new OverflowException();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, Schema.CountDaysInMonth(newY, newM));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(newY, newM, newD);
        return new CivilTriple(daysSinceEpoch);

        static int modulo(int i, int n, out int q)
        {
            q = i / n;
            int r = i % n;
            if (i < 0 && r != 0)
            {
                q--;
                r += n;
            }
            return r;
        }
    }

    public int CountYearsSince(CivilTriple other)
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

    public int CountMonthsSince(CivilTriple other)
    {
        var (y0, m0, _) = other;
        var (y, m, _) = this;

        // Exact difference between two calendar months.
        int months = checked(Schema.MonthsInYear * (y - y0) + m - m0);

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
