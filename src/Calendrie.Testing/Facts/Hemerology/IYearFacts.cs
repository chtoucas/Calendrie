// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie;
using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Testing.Data;

// We also test the static (abstract) methods from the interface.

public partial class IYearFacts<TYear, TMonth, TDate, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TYear : struct, IYear<TYear>,
        IMonthSegment<TMonth>, ISetMembership<TMonth>,
        IDaySegment<TDate>, ISetMembership<TDate>
    where TMonth : struct, IMonth<TMonth>
    where TDate : struct, IDate<TDate>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    public IYearFacts()
    {
        var supportedYears = TYear.Calendar.Scope.Segment.SupportedYears;
        SupportedYears = supportedYears;
        SupportedYearsTester = new SupportedYearsTester(supportedYears);
    }

    protected TYear MinYear => TYear.MinValue;
    protected TYear MaxYear => TYear.MaxValue;

    protected Range<int> SupportedYears { get; }
    protected SupportedYearsTester SupportedYearsTester { get; }

    /// <summary>
    /// We only use this sample year when its value matters (mathops); otherwise
    /// just use the year 1.
    /// </summary>
    private static TYear GetSampleYear() => TYear.Create(1234);
}

public partial class IYearFacts<TYear, TMonth, TDate, TDataSet> // Prelude
{
    [Fact]
    public void ToString_InvariantCulture()
    {
        var date = TYear.Create(1);
        string str = FormattableString.Invariant($"0001 ({TDate.Calendar})");
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
        var year = TYear.Create(y);
        var centuryOfEra = Ord.Zeroth + century;
        // Act & Assert
        Assert.Equal(centuryOfEra, year.CenturyOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void Century_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var year = TYear.Create(y);
        // Act & Assert
        Assert.Equal(century, year.Century);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfEra_Prop(CenturyInfo info)
    {
        int y = info.Year;
        var year = TYear.Create(y);
        var yearOfEra = Ord.Zeroth + y;
        // Act & Assert
        Assert.Equal(yearOfEra, year.YearOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfCentury_Prop(CenturyInfo info)
    {
        var (y, _, yearOfCentury) = info;
        var year = TYear.Create(y);
        // Act & Assert
        Assert.Equal(yearOfCentury, year.YearOfCentury);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void Year_Prop(CenturyInfo info)
    {
        int y = info.Year;
        var year = TYear.Create(y);
        // Act & Assert
        Assert.Equal(y, year.Year);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void IsLeap_Prop(YearInfo info)
    {
        // Act
        var year = TYear.Create(info.Year);
        // Assert
        Assert.Equal(info.IsLeap, year.IsLeap);
    }
}

public partial class IYearFacts<TYear, TMonth, TDate, TDataSet> // Factories
{
    #region Create()

    [Fact]
    public void Create_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(TYear.Create);

    [Theory, MemberData(nameof(YearInfoData))]
    public void Create(YearInfo info)
    {
        int y = info.Year;
        var year = TYear.Create(y);
        // Act & Assert
        Assert.Equal(y, year.Year);
    }

    #endregion
    #region TryCreate()

    [Fact]
    public void TryCreate_InvalidYear() =>
        SupportedYearsTester.TestInvalidYearTryPattern(y => TYear.TryCreate(y, out _));

    [Theory, MemberData(nameof(YearInfoData))]
    public void TryCreate(YearInfo info)
    {
        int y = info.Year;
        // Act
        bool result = TYear.TryCreate(y, out var year);
        // Assert
        Assert.True(result);
        Assert.Equal(y, year.Year);
    }

    #endregion
}

public partial class IYearFacts<TYear, TMonth, TDate, TDataSet> // IMonthSegment
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void MinMonth_Prop(YearInfo info)
    {
        int y = info.Year;
        var year = TYear.Create(y);
        var month = TMonth.Create(y, 1);
        // Act & Assert
        Assert.Equal(month, year.MinMonth);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void MaxMonth_Prop(YearInfo info)
    {
        int y = info.Year;
        var year = TYear.Create(y);
        var month = TMonth.Create(y, info.MonthsInYear);
        // Act & Assert
        Assert.Equal(month, year.MaxMonth);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDays(YearInfo info)
    {
        var year = TYear.Create(info.Year);
        // Act & Assert
        Assert.Equal(info.DaysInYear, year.CountDays());
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void ToDayRange(YearInfo info)
    {
        int y = info.Year;
        var year = TYear.Create(y);
        var min = TDate.Create(y, 1);
        var max = TDate.Create(y, info.DaysInYear);
        // Act
        var range = year.ToDayRange();
        // Assert
        Assert.Equal(min, range.Min);
        Assert.Equal(max, range.Max);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void EnumerateDays(YearInfo info)
    {
        int y = info.Year;
        var year = TYear.Create(y);
        var exp = from doy in Enumerable.Range(1, info.DaysInYear)
                  select TDate.Create(y, doy);
        // Act
        var actual = year.EnumerateDays();
        // Assert
        Assert.Equal(exp, actual);
    }

    // TODO(fact): add tests when Contains() returns false.
    // Idem w/ IYear.Contains(date) and IMonth.Contains(date).
    // GetDayOfYear() and GetMonthOfYear() when invalid. Idem in IMonthFacts.

    [Theory, MemberData(nameof(DateInfoData))]
    public void Contains_Month(DateInfo info)
    {
        var (y, m, _) = info.Yemoda;
        var year = TYear.Create(y);
        var month = TMonth.Create(y, m);
        // Act & Assert
        Assert.True(year.Contains(month));
    }

    //[Theory, MemberData(nameof(DateInfoData))]
    //public void GetDayOfYear(DateInfo info)
    //{
    //    var (y, doy) = info.Yedoy;
    //    var year = TYear.Create(y);
    //    var date = TDate.Create(y, doy);
    //    // Act & Assert
    //    Assert.Equal(date, year.GetDayOfYear(doy));
    //}
}

public partial class IYearFacts<TYear, TMonth, TDate, TDataSet> // IDaySegment
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void MinDay_Prop(YearInfo info)
    {
        int y = info.Year;
        var year = TYear.Create(y);
        var startOfYear = TDate.Create(y, 1);
        // Act & Assert
        Assert.Equal(startOfYear, year.MinDay);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void MaxDay_Prop(YearInfo info)
    {
        int y = info.Year;
        var year = TYear.Create(y);
        var endOfYear = TDate.Create(y, info.DaysInYear);
        // Act & Assert
        Assert.Equal(endOfYear, year.MaxDay);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountMonths(YearInfo info)
    {
        var year = TYear.Create(info.Year);
        // Act & Assert
        Assert.Equal(info.MonthsInYear, year.CountMonths());
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void ToMonthRange(YearInfo info)
    {
        int y = info.Year;
        var year = TYear.Create(y);
        var min = TMonth.Create(y, 1);
        var max = TMonth.Create(y, info.MonthsInYear);
        // Act
        var range = year.ToMonthRange();
        // Assert
        Assert.Equal(min, range.Min);
        Assert.Equal(max, range.Max);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void EnumerateMonths(YearInfo info)
    {
        int y = info.Year;
        var year = TYear.Create(y);
        var exp = from m in Enumerable.Range(1, info.MonthsInYear)
                  select TMonth.Create(y, m);
        // Act
        var actual = year.EnumerateMonths();
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Contains_Day(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var year = TYear.Create(y);
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.True(year.Contains(date));
    }

    //[Theory, MemberData(nameof(MonthInfoData))]
    //public void GetMonthOfYear(MonthInfo info)
    //{
    //    var (y, m) = info.Yemo;
    //    var year = TYear.Create(y);
    //    var month = TMonth.Create(y, m);
    //    // Act & Assert
    //    Assert.Equal(month, year.GetMonthOfYear(m));
    //}
}

public partial class IYearFacts<TYear, TMonth, TDate, TDataSet> // IEquatable
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void Equals_WhenSame(YearInfo info)
    {
        int y = info.Year;
        var year = TYear.Create(y);
        var same = TYear.Create(y);
        // Act & Assert
        Assert.True(year == same);
        Assert.False(year != same);

        Assert.True(year.Equals(same));
        Assert.True(year.Equals((object)same));
    }

    [Fact]
    public void Equals_WhenNotSame()
    {
        var year = TYear.Create(1);
        var notSame = TYear.Create(2);
        // Act & Assert
        Assert.False(year == notSame);
        Assert.True(year != notSame);

        Assert.False(year.Equals(notSame));
        Assert.False(year.Equals((object)notSame));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void Equals_NullOrPlainObject(YearInfo info)
    {
        var year = TYear.Create(info.Year);
        // Act & Assert
        Assert.False(year.Equals(1));
        Assert.False(year.Equals(null));
        Assert.False(year.Equals(new object()));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetHashCode_Repeated(YearInfo info)
    {
        var year = TYear.Create(info.Year);
        object obj = year;
        // Act & Assert
        Assert.Equal(year.GetHashCode(), year.GetHashCode());
        Assert.Equal(year.GetHashCode(), obj.GetHashCode());
    }
}

public partial class IYearFacts<TYear, TMonth, TDate, TDataSet> // IComparable
{
    [Fact]
    public void CompareTo_Null()
    {
        var year = TYear.Create(1);
        var comparable = (IComparable)year;
        // Act & Assert
        Assert.Equal(1, comparable.CompareTo(null));
    }

    [Fact]
    public void CompareTo_PlainObject()
    {
        var year = TYear.Create(1);
        var comparable = (IComparable)year;
        object other = new();
        // Act & Assert
        _ = Assert.Throws<ArgumentException>("obj", () => comparable.CompareTo(other));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CompareTo_WhenEqual(YearInfo info)
    {
        int y = info.Year;
        var left = TYear.Create(y);
        var right = TYear.Create(y);
        // Act & Assert
        Assert.False(left > right);
        Assert.True(left >= right);
        Assert.True(left <= right);
        Assert.False(left < right);

        Assert.Equal(0, left.CompareTo(right));
        Assert.Equal(0, ((IComparable)left).CompareTo(right));
    }

    [Fact]
    public void CompareTo_WhenNotEqual()
    {
        var left = TYear.Create(1);
        var right = TYear.Create(2);
        // Act & Assert
        Assert.False(left > right);
        Assert.False(left >= right);
        Assert.True(left <= right);
        Assert.True(left < right);

        Assert.True(left.CompareTo(right) < 0);
        Assert.True(((IComparable)left).CompareTo(right) < 0);
    }

    [Fact]
    public void Min()
    {
        var min = TYear.Create(1);
        var max = TYear.Create(2);
        // Act & Assert
        Assert.Equal(min, TYear.Min(min, max));
        Assert.Equal(min, TYear.Min(max, min));
    }

    [Fact]
    public void Max()
    {
        var min = TYear.Create(1);
        var max = TYear.Create(2);
        // Act & Assert
        Assert.Equal(max, TYear.Max(min, max));
        Assert.Equal(max, TYear.Max(max, min));
    }
}

public partial class IYearFacts<TYear, TMonth, TDate, TDataSet> // Math
{
    #region NextYear()

    [Fact]
    public void NextYear_Overflows_AtMaxValue()
    {
        var copy = MaxYear;
        // Act & Assert
        AssertEx.Overflows(() => copy++);
        AssertEx.Overflows(() => MaxYear.NextYear());
    }

    [Fact]
    public void NextYear()
    {
        var year = GetSampleYear();
        var copy = year;
        var yearAfter = TYear.Create(year.Year + 1);
        // Act & Assert
        Assert.Equal(yearAfter, ++copy);
        Assert.Equal(yearAfter, year.NextYear());
    }

    #endregion
    #region PreviousYear()

    [Fact]
    public void PreviousYear_Overflows_AtMinValue()
    {
        var copy = MinYear;
        // Act & Assert
        AssertEx.Overflows(() => copy--);
        AssertEx.Overflows(() => MinYear.PreviousYear());
    }

    [Fact]
    public void PreviousYear()
    {
        var year = GetSampleYear();
        var copy = year;
        var yearBefore = TYear.Create(year.Year - 1);
        // Act & Assert
        Assert.Equal(yearBefore, --copy);
        Assert.Equal(yearBefore, year.PreviousYear());
    }

    #endregion
    #region PlusYears() & CountYearsSince()

    [Fact]
    public void PlusYears_Overflows_WithMaxYears()
    {
        var year = TYear.Create(1);
        // Act & Assert
        AssertEx.Overflows(() => year + int.MinValue);
        AssertEx.Overflows(() => year + int.MaxValue);

        AssertEx.Overflows(() => year.PlusYears(int.MinValue));
        AssertEx.Overflows(() => year.PlusYears(int.MaxValue));
    }

    [Fact]
    public void PlusYears_WithLimitYears()
    {
        var year = GetSampleYear();
        int minYears = MinYear - year;
        int maxYears = MaxYear - year;
        // Act & Assert
        AssertEx.Overflows(() => year + (minYears - 1));
        Assert.Equal(MinYear, year + minYears);
        Assert.Equal(MaxYear, year + maxYears);
        AssertEx.Overflows(() => year + (maxYears + 1));

        AssertEx.Overflows(() => year.PlusYears(minYears - 1));
        Assert.Equal(MinYear, year.PlusYears(minYears));
        Assert.Equal(MaxYear, year.PlusYears(maxYears));
        AssertEx.Overflows(() => year.PlusYears(maxYears + 1));
    }

    [Fact]
    public void CountYearsSince_DoesNotOverflow()
    {
        int years = MaxYear.Year - MinYear.Year;
        // Act & Assert
        Assert.Equal(years, MaxYear - MinYear);
        Assert.Equal(-years, MinYear - MaxYear);

        Assert.Equal(years, MaxYear.CountYearsSince(MinYear));
        Assert.Equal(-years, MinYear.CountYearsSince(MaxYear));
    }

    [Fact]
    public void PlusYears_AtMinYear()
    {
        // We could have written:
        // > int years = MaxYear - MinYear;
        // but this is CountYearsSince() in disguise and I prefer to stick to
        // basic maths.
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => MinYear - 1);
        Assert.Equal(MinYear, MinYear - 0);
        Assert.Equal(MinYear, MinYear + 0);
        Assert.Equal(MaxYear, MinYear + years);
        AssertEx.Overflows(() => MinYear + (years + 1));

        AssertEx.Overflows(() => MinYear.PlusYears(-1));
        Assert.Equal(MinYear, MinYear.PlusYears(0));
        Assert.Equal(MaxYear, MinYear.PlusYears(years));
        AssertEx.Overflows(() => MinYear.PlusYears(years + 1));
    }

    [Fact]
    public void PlusYears_AtMaxYear()
    {
        // We could have written:
        // > int years = MaxYear - MinYear;
        // but this is CountYearsSince() in disguise and I prefer to stick to
        // basic maths.
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => MaxYear - (years + 1));
        Assert.Equal(MinYear, MaxYear - years);
        Assert.Equal(MaxYear, MaxYear - 0);
        Assert.Equal(MaxYear, MaxYear + 0);
        AssertEx.Overflows(() => MaxYear + 1);

        AssertEx.Overflows(() => MaxYear.PlusYears(-years - 1));
        Assert.Equal(MinYear, MaxYear.PlusYears(-years));
        Assert.Equal(MaxYear, MaxYear.PlusYears(0));
        AssertEx.Overflows(() => MaxYear.PlusYears(1));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void PlusYears_Zero_IsNeutral(YearInfo info)
    {
        var year = TYear.Create(info.Year);
        // Act & Assert
        Assert.Equal(year, year + 0);
        Assert.Equal(year, year - 0);
        Assert.Equal(year, year.PlusYears(0));

        Assert.Equal(0, year - year);
        Assert.Equal(0, year.CountYearsSince(year));
    }

    [Fact]
    public void PlusYears()
    {
        // NB: ys is such that "other" is a valid year for both standard and
        // proleptic calendars.
        int years = 876;
        var year = GetSampleYear();
        var other = TYear.Create(year.Year + years);
        // Act & Assert
        Assert.Equal(other, year + years);
        Assert.Equal(other, year - (-years));
        Assert.Equal(year, other - years);
        Assert.Equal(year, other + (-years));

        Assert.Equal(other, year.PlusYears(years));
        Assert.Equal(year, other.PlusYears(-years));

        Assert.Equal(years, other - year);
        Assert.Equal(-years, year - other);

        Assert.Equal(years, other.CountYearsSince(year));
        Assert.Equal(-years, year.CountYearsSince(other));
    }

    #endregion
}
