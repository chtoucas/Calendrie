// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

using Calendrie;
using Calendrie.Core;
using Calendrie.Hemerology;

using CalendrieRange = Calendrie.Core.Intervals.Range;

// Demonstration that almost all operations only depend on the schema.
//
// No epoch means no interconversion and no day of the week.
// Actually we can do everything since we focus on the Gregorian case, but
// let's keep the code as general as possible (only s_Schema is specific
// here).

internal static class CivilTripleScope
{
    public const int MinYear = 1;
    public const int MaxYear = 9999;

    public static readonly ICalendricalSchema Schema = new CivilPrototype();

    // Cache the computed property pre-validator.
    public static readonly ICalendricalPreValidator PreValidator = Schema.PreValidator;

    public static readonly CalendricalSegment Segment =
        CalendricalSegment.Create(Schema, CalendrieRange.Create(MinYear, MaxYear));

    public static void ValidateYearMonthDay(int year, int month, int day)
    {
        if (year < MinYear || year > MaxYear) throw new ArgumentOutOfRangeException(nameof(year));
        PreValidator.ValidateMonthDay(year, month, day);
    }

    public static void ValidateOrdinal(int year, int dayOfYear)
    {
        if (year < MinYear || year > MaxYear) throw new ArgumentOutOfRangeException(nameof(year));
        PreValidator.ValidateDayOfYear(year, dayOfYear);
    }
}

public readonly partial struct CivilTriple :
    IDateable,
    IAffineDate<CivilTriple>,
    ISubtractionOperators<CivilTriple, CivilTriple, int>
{
    private static readonly int s_MinDaysSinceEpoch = Segment.SupportedDays.Min;
    private static readonly int s_MaxDaysSinceEpoch = Segment.SupportedDays.Max;

    private readonly int _daysSinceEpoch;

    public CivilTriple(int year, int month, int day)
    {
        CivilTripleScope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = Schema.CountDaysSinceEpoch(year, month, day);
    }

    public CivilTriple(int year, int dayOfYear)
    {
        CivilTripleScope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    private CivilTriple(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    public static CivilTriple MinValue { get; } = new(s_MinDaysSinceEpoch);
    public static CivilTriple MaxValue { get; } = new(s_MaxDaysSinceEpoch);

    public int DaysSinceEpoch => _daysSinceEpoch;

    public Ord CenturyOfEra => Ord.FromInt32(Century);
    public int Century => YearNumbering.GetCentury(Year);
    public Ord YearOfEra => Ord.FromInt32(Year);
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);
    public int Year => Schema.GetYear(_daysSinceEpoch);

    public int Month
    {
        get
        {
            Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
            return m;
        }
    }

    public int DayOfYear
    {
        get
        {
            _ = Schema.GetYear(_daysSinceEpoch, out int doy);
            return doy;
        }
    }

    public int Day
    {
        get
        {
            Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
            return d;
        }
    }

    public bool IsIntercalary
    {
        get
        {
            var sch = Schema;
            sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return sch.IsIntercalaryDay(y, m, d);
        }
    }

    bool IDateable.IsSupplementary => false;

    private static ICalendricalSchema Schema => CivilTripleScope.Schema;
    private static CalendricalSegment Segment => CivilTripleScope.Segment;

    public override string ToString()
    {
        Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} (Civil)");
    }

    public void Deconstruct(out int year, out int month, out int day) =>
        Schema.GetDateParts(_daysSinceEpoch, out year, out month, out day);

    public void Deconstruct(out int year, out int dayOfYear) =>
        year = Schema.GetYear(_daysSinceEpoch, out dayOfYear);
}

public partial struct CivilTriple // Factories & conversions
{
    public static CivilTriple FromDaysSinceEpoch(int daysSinceEpoch)
    {
        if (daysSinceEpoch < s_MinDaysSinceEpoch || daysSinceEpoch > s_MaxDaysSinceEpoch)
            throw new ArgumentOutOfRangeException(nameof(daysSinceEpoch));

        return new(daysSinceEpoch);
    }
}

public partial struct CivilTriple // Counting
{
    public int CountElapsedDaysInYear() => DayOfYear - 1;
    public int CountRemainingDaysInYear() => throw new NotImplementedException();
    public int CountElapsedDaysInMonth() => Day - 1;
    public int CountRemainingDaysInMonth() => throw new NotImplementedException();
}

public partial struct CivilTriple // IEquatable
{
    public static bool operator ==(CivilTriple left, CivilTriple right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;
    public static bool operator !=(CivilTriple left, CivilTriple right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    public bool Equals(CivilTriple other) => _daysSinceEpoch == other._daysSinceEpoch;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CivilTriple date && Equals(date);

    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct CivilTriple // IComparable
{
    public static bool operator <(CivilTriple left, CivilTriple right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;
    public static bool operator <=(CivilTriple left, CivilTriple right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;
    public static bool operator >(CivilTriple left, CivilTriple right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;
    public static bool operator >=(CivilTriple left, CivilTriple right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    public static CivilTriple Min(CivilTriple x, CivilTriple y) => x < y ? x : y;
    public static CivilTriple Max(CivilTriple x, CivilTriple y) => x > y ? x : y;

    public int CompareTo(CivilTriple other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

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

    public int CountDaysSince(CivilTriple other) => _daysSinceEpoch - other._daysSinceEpoch;

    public CivilTriple PlusDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);

        if (daysSinceEpoch < s_MinDaysSinceEpoch || daysSinceEpoch > s_MaxDaysSinceEpoch)
            throw new OverflowException();

        return new(daysSinceEpoch);
    }

    public CivilTriple NextDay() =>
        this == MaxValue ? throw new OverflowException() : new(_daysSinceEpoch + 1);

    public CivilTriple PreviousDay() =>
        this == MinValue ? throw new OverflowException() : new(_daysSinceEpoch - 1);

    public CivilTriple PlusYears(int years) => throw new NotImplementedException();
    public CivilTriple PlusMonths(int months) => throw new NotImplementedException();
    public int CountYearsSince(CivilTriple other) => throw new NotImplementedException();
    public int CountMonthsSince(CivilTriple other) => throw new NotImplementedException();
}
