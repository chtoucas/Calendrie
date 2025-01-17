// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Testing.Data;

// We also test the static (abstract) methods from the interface.

public partial class IMonthFacts<TMonth, TDate, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TMonth : struct, IMonth<TMonth>, IDaySegment<TDate>, ISetMembership<TDate>
    where TDate : struct, IDate<TDate>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    public IMonthFacts()
    {
        var supportedYears = TMonth.Calendar.Scope.Segment.SupportedYears;
        SupportedYears = supportedYears;
        SupportedYearsTester = new SupportedYearsTester(supportedYears);
    }

    protected TMonth MinMonth => TMonth.MinValue;
    protected TMonth MaxMonth => TMonth.MaxValue;

    protected Range<int> SupportedYears { get; }
    protected SupportedYearsTester SupportedYearsTester { get; }

    protected static TMonth GetMonth(Yemo ym)
    {
        var (y, m) = ym;
        return TMonth.Create(y, m);
    }

    /// <summary>
    /// We only use this sample year when its value matters (mathops); otherwise
    /// just use the first month of the year 1. It is initialized to ensure that
    /// the math operations we are going to perform will work.
    /// </summary>
    private static TMonth GetSampleMonth() => TMonth.Create(1234, 2);
}

public partial class IMonthFacts<TMonth, TDate, TDataSet> // Prelude
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void Deconstructor(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act
        var (yA, mA) = month;
        // Assert
        Assert.Equal(y, yA);
        Assert.Equal(m, mA);
    }

    [Fact]
    public void ToString_InvariantCulture()
    {
        var date = TMonth.Create(1, 1);
        string str = FormattableString.Invariant($"01/0001 ({TDate.Calendar})");
        // Act & Assert
        Assert.Equal(str, date.ToString());
    }

    //
    // Properties
    //

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void CenturyOfEra_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var month = TMonth.Create(y, 1);
        var centuryOfEra = Ord.Zeroth + century;
        // Act & Assert
        Assert.Equal(centuryOfEra, month.CenturyOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void Century_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var month = TMonth.Create(y, 1);
        // Act & Assert
        Assert.Equal(century, month.Century);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfEra_Prop(CenturyInfo info)
    {
        int y = info.Year;
        var month = TMonth.Create(y, 1);
        var yearOfEra = Ord.Zeroth + y;
        // Act & Assert
        Assert.Equal(yearOfEra, month.YearOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfCentury_Prop(CenturyInfo info)
    {
        var (y, _, yearOfCentury) = info;
        var month = TMonth.Create(y, 1);
        // Act & Assert
        Assert.Equal(yearOfCentury, month.YearOfCentury);
    }

    [Theory, MemberData(nameof(MonthsSinceEpochInfoData))]
    public void MonthsSinceEpoch_Prop(MonthsSinceEpochInfo info)
    {
        int monthsSinceEpoch = info.MonthsSinceEpoch;
        var month = TMonth.FromMonthsSinceEpoch(monthsSinceEpoch);
        // Act & Assert
        Assert.Equal(monthsSinceEpoch, month.MonthsSinceEpoch);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void IsIntercalary_Prop(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        var month = TMonth.Create(y, m);
        // Assert
        Assert.Equal(info.IsIntercalary, month.IsIntercalary);
    }
}

public partial class IMonthFacts<TMonth, TDate, TDataSet> // Factories
{
    #region Create()

    [Fact]
    public void Create_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => TMonth.Create(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void Create_InvalidMonth(int y, int m) =>
        AssertEx.ThrowsAoorexn("month", () => TMonth.Create(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void Create(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        var month = TMonth.Create(y, m);
        // Assert
        Assert.Equal(y, month.Year);
        Assert.Equal(m, month.Month);
    }

    #endregion
    #region TryCreate()

    [Fact]
    public void TryCreate_InvalidYear() =>
        SupportedYearsTester.TestInvalidYearTryPattern(y => TMonth.TryCreate(y, 1, out _));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void TryCreate_InvalidMonth(int y, int m) =>
        Assert.False(TMonth.TryCreate(y, m, out _));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void TryCreate(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        bool result = TMonth.TryCreate(y, m, out var month);
        // Assert
        Assert.True(result);
        Assert.Equal(y, month.Year);
        Assert.Equal(m, month.Month);
    }

    #endregion
}

public partial class IMonthFacts<TMonth, TDate, TDataSet> // Conversions
{
    #region FromMonthsSinceEpoch()

    [Fact]
    public void FromMonthsSinceEpoch_InvalidMonthsSinceEpoch()
    {
        AssertEx.ThrowsAoorexn("monthsSinceEpoch",
            () => TMonth.FromMonthsSinceEpoch(TMonth.MinValue.MonthsSinceEpoch - 1));
        AssertEx.ThrowsAoorexn("monthsSinceEpoch",
            () => TMonth.FromMonthsSinceEpoch(TMonth.MaxValue.MonthsSinceEpoch + 1));
    }

    [Theory, MemberData(nameof(MonthsSinceEpochInfoData))]
    public void FromMonthsSinceEpoch(MonthsSinceEpochInfo info)
    {
        var (monthsSinceEpoch, y, m) = info;
        // Act
        var month = TMonth.FromMonthsSinceEpoch(monthsSinceEpoch);
        // Assert
        Assert.Equal(y, month.Year);
        Assert.Equal(m, month.Month);
    }

    #endregion
}

public partial class IMonthFacts<TMonth, TDate, TDataSet> // Counting
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountElapsedMonthsInYear(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act
        int actual = month.CountElapsedMonthsInYear();
        // Assert
        Assert.Equal(m - 1, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountRemainingMonthsInYear(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        int monthsInYear = TMonth.Calendar.Scope.Schema.CountMonthsInYear(y);
        // Act
        int actual = month.CountRemainingMonthsInYear();
        // Assert
        Assert.Equal(monthsInYear - m, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountElapsedDaysInYear(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act
        int actual = month.CountElapsedDaysInYear();
        // Assert
        Assert.Equal(info.DaysInYearBeforeMonth, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountRemainingDaysInYear(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act
        int actual = month.CountRemainingDaysInYear();
        //
        Assert.Equal(info.DaysInYearAfterMonth, actual);
    }
}

public partial class IMonthFacts<TMonth, TDate, TDataSet> // Adjustments
{
    #region Year adjustment

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithYear_InvalidYears(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act & Assert
        SupportedYearsTester.TestInvalidYear(month.WithYear, "newYear");
    }

    [Fact]
    public void WithYear_ValidYears()
    {
        foreach (int y in SupportedYearsTester.ValidYears)
        {
            var month = TMonth.Create(1, 1);
            var exp = TMonth.Create(y, 1);
            // Act & Assert
            Assert.Equal(exp, month.WithYear(y));
        }
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithYear_Invariance(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(month, month.WithYear(y));
    }

    // FIXME(fact): case of intercalary months
    //[Theory, MemberData(nameof(MonthInfoData))]
    //public void WithYear(MonthInfo info)
    //{
    //    var (y, m) = info.Yemo;
    //    var month = TMonth.Create(1, m);
    //    var exp = TMonth.Create(y, m);
    //    // Act & Assert
    //    Assert.Equal(exp, month.WithYear(y));
    //}

    #endregion
    #region Month adjustment

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void WithMonth_InvalidMonth(int y, int newMonth)
    {
        var month = TMonth.Create(y, 1);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newMonth", () => month.WithMonth(newMonth));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithMonth_Invariance(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(month, month.WithMonth(m));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, 1);
        var exp = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(exp, month.WithMonth(m));
    }

    #endregion
}

public partial class IMonthFacts<TMonth, TDate, TDataSet> // IDaySegment
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void MinDay_Prop(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        var startOfMonth = TDate.Create(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, month.MinDay);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void MaxDay_Prop(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        byte daysInMonth = info.DaysInMonth;
        var month = TMonth.Create(y, m);
        var endOfMonth = TDate.Create(y, m, daysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, month.MaxDay);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDays(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act
        int actual = month.CountDays();
        // Assert
        Assert.Equal(info.DaysInMonth, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void ToDayRange(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        var min = TDate.Create(y, m, 1);
        var max = TDate.Create(y, m, info.DaysInMonth);
        // Act
        var range = month.ToDayRange();
        // Assert
        Assert.Equal(min, range.Min);
        Assert.Equal(max, range.Max);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void EnumerateDays(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        var exp = from d in Enumerable.Range(1, info.DaysInMonth)
                  select TDate.Create(y, m, d);
        // Act
        var actual = month.EnumerateDays();
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Contains(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var month = TMonth.Create(y, m);
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.True(month.Contains(date));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Contains_WithInvalidYear(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var month = TMonth.Create(y, m);
        if (TDate.TryCreate(y == 1 ? 2 : 1, m, d, out var date))
        {
            // Act & Assert
            Assert.False(month.Contains(date));
        }
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Contains_WithInvalidMonth(DateInfo info)
    {
        var (y, m, _) = info.Yemoda;
        var month = TMonth.Create(y, m);
        if (TDate.TryCreate(y, m == 1 ? 2 : 1, 1, out var date))
        {
            // Act & Assert
            Assert.False(month.Contains(date));
        }
    }
}

public partial class IMonthFacts<TMonth, TDate, TDataSet> // Math
{
    #region PlusYears()

    [Fact]
    public void PlusYears_Overflows_WithMaxYears()
    {
        var month = TMonth.Create(1, 1);
        // Act & Assert
        AssertEx.Overflows(() => month.PlusYears(int.MinValue));
        AssertEx.Overflows(() => month.PlusYears(int.MaxValue));
    }

    [Fact]
    public void PlusYears_AtMinMonth()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => MinMonth.PlusYears(-1));
        Assert.Equal(MinMonth, MinMonth.PlusYears(0));
        _ = MinMonth.PlusYears(years);
        AssertEx.Overflows(() => MinMonth.PlusYears(years + 1));
    }

    [Fact]
    public void PlusYears_AtMaxMonth()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => MaxMonth.PlusYears(-years - 1));
        _ = MaxMonth.PlusYears(-years);
        Assert.Equal(MaxMonth, MaxMonth.PlusYears(0));
        AssertEx.Overflows(() => MaxMonth.PlusYears(1));
    }

    [Fact]
    public void PlusYears_WithLimitYears()
    {
        var month = GetSampleMonth();
        int minYs = MinMonth.Year - month.Year;
        int maxYs = MaxMonth.Year - month.Year;
        // Act & Assert
        AssertEx.Overflows(() => month.PlusYears(minYs - 1));
        _ = month.PlusYears(minYs);
        _ = month.PlusYears(maxYs);
        AssertEx.Overflows(() => month.PlusYears(maxYs + 1));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void PlusYears_Zero_IsNeutral(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(month, month.PlusYears(0));
    }

    [Theory, MemberData(nameof(AddYearsMonthData))]
    public void PlusYears(YemoPairAnd<int> info)
    {
        int years = info.Value;
        var month = GetMonth(info.First);
        var other = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(other, month.PlusYears(years));
        Assert.Equal(month, other.PlusYears(-years));
    }

    #endregion
    #region CountYearsSince()

    [Fact]
    public void CountYearsSince_DoesNotOverflow()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        Assert.Equal(years, MaxMonth.CountYearsSince(MinMonth));
        Assert.Equal(-years, MinMonth.CountYearsSince(MaxMonth));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountYearsSince_WhenSame_IsZero(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(0, month.CountYearsSince(month));
    }

    [Theory, MemberData(nameof(CountYearsBetweenMonthData))]
    public void CountYearsSince(YemoPairAnd<int> info)
    {
        int years = info.Value;
        var start = GetMonth(info.First);
        var end = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(years, end.CountYearsSince(start));
        Assert.Equal(-years, start.CountYearsSince(end));
    }

    [Fact]
    public void CountYearsSince_SpecialCases()
    {
        // 3/2000 - 4/1900 = 99 years
        var date = TMonth.Create(2000, 3);
        var other = TMonth.Create(1900, 4);

        Assert.Equal(99, date.CountYearsSince(other));
        Assert.Equal(-99, other.CountYearsSince(date));
    }

    #endregion
}

public partial class IMonthFacts<TMonth, TDate, TDataSet> // IEquatable
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void Equals_WhenSame(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        var same = TMonth.Create(y, m);
        // Act & Assert
        Assert.True(month == same);
        Assert.False(month != same);

        Assert.True(month.Equals(same));
        Assert.True(month.Equals((object)same));
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 2)]
    public void Equals_WhenNotSame(int y, int m)
    {
        var month = TMonth.Create(1, 1);
        var notSame = TMonth.Create(y, m);
        // Act & Assert
        Assert.False(month == notSame);
        Assert.True(month != notSame);

        Assert.False(month.Equals(notSame));
        Assert.False(month.Equals((object)notSame));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void Equals_NullOrPlainObject(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act & Assert
        Assert.False(month.Equals(1));
        Assert.False(month.Equals(null));
        Assert.False(month.Equals(new object()));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetHashCode_Repeated(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        object obj = month;
        // Act & Assert
        Assert.Equal(month.GetHashCode(), month.GetHashCode());
        Assert.Equal(month.GetHashCode(), obj.GetHashCode());
    }
}

public partial class IMonthFacts<TMonth, TDate, TDataSet> // IComparable
{
    [Fact]
    public void CompareTo_Null()
    {
        var month = TMonth.Create(1, 1);
        var comparable = (IComparable)month;
        // Act & Assert
        Assert.Equal(1, comparable.CompareTo(null));
    }

    [Fact]
    public void CompareTo_PlainObject()
    {
        var month = TMonth.Create(1, 1);
        var comparable = (IComparable)month;
        object other = new();
        // Act & Assert
        _ = Assert.Throws<ArgumentException>("obj", () => comparable.CompareTo(other));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CompareTo_WhenEqual(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var left = TMonth.Create(y, m);
        var right = TMonth.Create(y, m);
        // Act & Assert
        Assert.False(left > right);
        Assert.True(left >= right);
        Assert.True(left <= right);
        Assert.False(left < right);

        Assert.Equal(0, left.CompareTo(right));
        Assert.Equal(0, ((IComparable)left).CompareTo(right));
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 2)]
    public void CompareTo_WhenNotEqual(int y, int m)
    {
        var left = TMonth.Create(1, 1);
        var right = TMonth.Create(y, m);
        // Act & Assert
        Assert.False(left > right);
        Assert.False(left >= right);
        Assert.True(left <= right);
        Assert.True(left < right);

        Assert.True(left.CompareTo(right) < 0);
        Assert.True(((IComparable)left).CompareTo(right) < 0);
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 2)]
    [InlineData(1, 1)]
    public void Min(int y, int m)
    {
        var min = TMonth.Create(1, 1);
        var max = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(min, TMonth.Min(min, max));
        Assert.Equal(min, TMonth.Min(max, min));
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 2)]
    [InlineData(1, 1)]
    public void Max(int y, int m)
    {
        var min = TMonth.Create(1, 1);
        var max = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(max, TMonth.Max(min, max));
        Assert.Equal(max, TMonth.Max(max, min));
    }
}

public partial class IMonthFacts<TMonth, TDate, TDataSet> // Math
{
    #region NextMonth()

    [Fact]
    public void NextMonth_Overflows_AtMaxMonth()
    {
        var copy = MaxMonth;
        // Act & Assert
        AssertEx.Overflows(() => copy++);
        AssertEx.Overflows(() => MaxMonth.NextMonth());
    }

    [Theory, MemberData(nameof(ConsecutiveMonthsData))]
    public void NextMonth(YemoPair pair)
    {
        var month = GetMonth(pair.First);
        var copy = month;
        var monthAfter = GetMonth(pair.Second);
        // Act & Assert
        Assert.Equal(monthAfter, ++copy);
        Assert.Equal(monthAfter, month.NextMonth());
    }

    #endregion
    #region PreviousMonth()

    [Fact]
    public void PreviousMonth_Overflows_AtMinMonth()
    {
        var copy = MinMonth;
        // Act & Assert
        AssertEx.Overflows(() => copy--);
        AssertEx.Overflows(() => MinMonth.PreviousMonth());
    }

    [Theory, MemberData(nameof(ConsecutiveMonthsData))]
    public void PreviousMonth(YemoPair pair)
    {
        var month = GetMonth(pair.First);
        var monthAfter = GetMonth(pair.Second);
        var copy = monthAfter;
        // Act & Assert
        Assert.Equal(month, --copy);
        Assert.Equal(month, monthAfter.PreviousMonth());
    }

    #endregion
    #region AddMonths() & CountMonthsBetween()

    [Fact]
    public void AddMonths_Overflows_WithMaxMonths()
    {
        var month = TMonth.Create(1, 1);
        // Act & Assert
        AssertEx.Overflows(() => month + int.MinValue);
        AssertEx.Overflows(() => month + int.MaxValue);
        AssertEx.Overflows(() => month.PlusMonths(int.MinValue));
        AssertEx.Overflows(() => month.PlusMonths(int.MaxValue));
    }

    [Fact]
    public void AddMonths_AtMinMonth()
    {
        int months = MaxMonth - MinMonth;
        // Act & Assert
        AssertEx.Overflows(() => MinMonth - 1);
        Assert.Equal(MinMonth, MinMonth - 0);
        Assert.Equal(MinMonth, MinMonth + 0);
        Assert.Equal(MaxMonth, MinMonth + months);
        AssertEx.Overflows(() => MinMonth + (months + 1));

        AssertEx.Overflows(() => MinMonth.PlusMonths(-1));
        Assert.Equal(MinMonth, MinMonth.PlusMonths(0));
        Assert.Equal(MaxMonth, MinMonth.PlusMonths(months));
        AssertEx.Overflows(() => MinMonth.PlusMonths(months + 1));
    }

    [Fact]
    public void AddMonths_AtMaxMonth()
    {
        int months = MaxMonth - MinMonth;
        // Act & Assert
        AssertEx.Overflows(() => MaxMonth - (months + 1));
        Assert.Equal(MinMonth, MaxMonth - months);
        Assert.Equal(MaxMonth, MaxMonth - 0);
        Assert.Equal(MaxMonth, MaxMonth + 0);
        AssertEx.Overflows(() => MaxMonth + 1);

        AssertEx.Overflows(() => MaxMonth.PlusMonths(-months - 1));
        Assert.Equal(MinMonth, MaxMonth.PlusMonths(-months));
        Assert.Equal(MaxMonth, MaxMonth.PlusMonths(0));
        AssertEx.Overflows(() => MaxMonth.PlusMonths(1));
    }

    [Fact]
    public void AddMonths_WithLimitMonths()
    {
        var month = GetSampleMonth();
        int minMonths = MinMonth - month;
        int maxMonths = MaxMonth - month;
        // Act & Assert
        AssertEx.Overflows(() => month + (minMonths - 1));
        Assert.Equal(MinMonth, month + minMonths);
        Assert.Equal(MaxMonth, month + maxMonths);
        AssertEx.Overflows(() => month + (maxMonths + 1));

        AssertEx.Overflows(() => month.PlusMonths(minMonths - 1));
        Assert.Equal(MinMonth, month.PlusMonths(minMonths));
        Assert.Equal(MaxMonth, month.PlusMonths(maxMonths));
        AssertEx.Overflows(() => month.PlusMonths(maxMonths + 1));
    }

    [Fact]
    public void CountMonthsBetween_DoesNotOverflow()
    {
        _ = MaxMonth - MinMonth;
        _ = MinMonth - MaxMonth;
        _ = MaxMonth.CountMonthsSince(MinMonth);
        _ = MinMonth.CountMonthsSince(MaxMonth);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void AddMonths_Zero_IsNeutral(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(month, month + 0);
        Assert.Equal(month, month - 0);
        Assert.Equal(month, month.PlusMonths(0));

        Assert.Equal(0, month - month);
        Assert.Equal(0, month.CountMonthsSince(month));
    }

    [Theory, MemberData(nameof(AddMonthsMonthData))]
    public void AddMonths(YemoPairAnd<int> info)
    {
        int months = info.Value;
        var month = GetMonth(info.First);
        var other = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(other, month + months);
        Assert.Equal(other, month - (-months));
        Assert.Equal(month, other - months);
        Assert.Equal(month, other + (-months));

        Assert.Equal(other, month.PlusMonths(months));
        Assert.Equal(month, other.PlusMonths(-months));

        Assert.Equal(months, other - month);
        Assert.Equal(-months, month - other);

        Assert.Equal(months, other.CountMonthsSince(month));
        Assert.Equal(-months, month.CountMonthsSince(other));
    }

    #endregion
}
