// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Faux;

using Calendrie;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;
using Calendrie.Systems;

// The sole purpose of this type is to test IMonth.CountElapsedMonthsInYear().

public readonly struct FauxTropicaliaMonth : IMonth, IEquatable<FauxTropicaliaMonth>
{
    public FauxTropicaliaMonth(int year, int month)
    {
        if (year < StandardScope.MinYear || year > StandardScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year);
        if (month < 1 || month > TropicalistaSchema.MonthsPerYear)
            ThrowHelpers.ThrowMonthOutOfRange(month);

        MonthsSinceEpoch = TropicalistaSchema.MonthsPerYear * (year - 1) + month - 1;
    }

    public static TropicaliaCalendar Calendar => TropicaliaCalendar.Instance;
    static Calendar IMonth.Calendar => Calendar;

    public int MonthsSinceEpoch { get; }

    public Ord CenturyOfEra => Ord.FromInt32(Century);
    public int Century => YearNumbering.GetCentury(Year);
    public Ord YearOfEra => Ord.FromInt32(Year);
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);
    public int Year => 1 + MonthsSinceEpoch / TropicalistaSchema.MonthsPerYear;

    public int Month { get { var (_, m) = this; return m; } }

    bool IMonth.IsIntercalary => false;

    public void Deconstruct(out int year, out int month)
    {
        year = 1 + MathN.Divide(MonthsSinceEpoch, TropicalistaSchema.MonthsPerYear, out int m0);
        month = 1 + m0;
    }

    public int CountRemainingMonthsInYear() => TropicalistaSchema.MonthsPerYear - Month;

    public int CountElapsedDaysInYear()
    {
        var (y, m) = this;
        return Calendar.Schema.CountDaysInYearBeforeMonth(y, m);
    }

    public int CountRemainingDaysInYear()
    {
        var (y, m) = this;
        return Calendar.Schema.CountDaysInYearAfterMonth(y, m);
    }

    //
    // IEquatable
    //

    public static bool operator ==(FauxTropicaliaMonth left, FauxTropicaliaMonth right) =>
        left.MonthsSinceEpoch == right.MonthsSinceEpoch;

    public static bool operator !=(FauxTropicaliaMonth left, FauxTropicaliaMonth right) =>
        left.MonthsSinceEpoch != right.MonthsSinceEpoch;

    public bool Equals(FauxTropicaliaMonth other) => MonthsSinceEpoch == other.MonthsSinceEpoch;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is FauxTropicaliaMonth month && Equals(month);

    public override int GetHashCode() => MonthsSinceEpoch;
}
