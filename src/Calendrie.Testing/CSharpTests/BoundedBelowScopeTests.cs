// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.CSharpTests;

using Calendrie.Core.Schemas;
using Calendrie.Hemerology;
using Calendrie.Testing.Data;
using Calendrie.Testing.Data.Schemas;

public partial class BoundedBelowScopeTests : CalendricalDataConsumer<GregorianDataSet>
{
    private const int FirstYear = 3;
    private const int FirstMonth = 4;
    private const int FirstDay = 5;

    private static readonly GregorianSchema s_Schema = new();

    [Fact]
    public static void Create_NullSchema() =>
        AssertEx.ThrowsAnexn("schema",
            () => BoundedBelowScope.Create(null!, DayZero.NewStyle, new(3, 4, 5), 9999));

    [Fact]
    public static void Create_InvalidMinYear() =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => BoundedBelowScope.Create(
                s_Schema, DayZero.NewStyle, new(s_Schema.SupportedYears.Min - 1, 1, 1), 9999));

#if ENABLE_GENERIC_FACTORIES
    [Fact]
    public static void Create_InvalidMinYear_Generic() =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => BoundedBelowScope.Create<GregorianSchema>(
                DayZero.NewStyle, new(s_Schema.SupportedYears.Min - 1, 1, 1), 9999));
#endif

    [Fact]
    public static void Create_InvalidMaxYear() =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => BoundedBelowScope.Create(
                s_Schema, DayZero.NewStyle, new(s_Schema.SupportedYears.Max + 1, 1, 1), 9999));

#if ENABLE_GENERIC_FACTORIES
    [Fact]
    public static void Create_InvalidMaxYear_Generic() =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => BoundedBelowScope.Create<GregorianSchema>(
                DayZero.NewStyle, new(s_Schema.SupportedYears.Max + 1, 1, 1), 9999));
#endif

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public static void Create_InvalidMonth(int y, int m) =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => BoundedBelowScope.Create(s_Schema, DayZero.NewStyle, new(y, m, 1), 9999));

#if ENABLE_GENERIC_FACTORIES
    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public static void Create_InvalidMonth_Generic(int y, int m) =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => BoundedBelowScope.Create<GregorianSchema>(DayZero.NewStyle, new(y, m, 1), 9999));
#endif

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public static void Create_InvalidDay(int y, int m, int d) =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => BoundedBelowScope.Create(s_Schema, DayZero.NewStyle, new(y, m, d), 9999));

#if ENABLE_GENERIC_FACTORIES
    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public static void Create_InvalidDay_Generic(int y, int m, int d) =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => BoundedBelowScope.Create<GregorianSchema>(DayZero.NewStyle, new(y, m, d), 9999));
#endif

    [Theory(Skip = "MinDateParts cannot be the start of a year."), MemberData(nameof(DateInfoData))]
    public void Create(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        var scope = BoundedBelowScope.Create(s_Schema, DayZero.NewStyle, new(y, m, d), 9999);
        var minDate = scope.Segment.MinMaxDateParts.LowerValue;
        var minOrdinalDate = scope.Segment.MinMaxOrdinalParts.LowerValue;
        // Assert
        Assert.NotNull(scope);
        Assert.Equal(y, scope.Segment.SupportedYears.Min);
        Assert.Equal(y, minDate.Year);
        Assert.Equal(m, minDate.Month);
        Assert.Equal(d, minDate.Day);
        Assert.Equal(9999, scope.Segment.SupportedYears.Max);
        Assert.Equal(doy, minOrdinalDate.DayOfYear);
    }

    [Fact]
    public static void ValidateYearMonth()
    {
        var scope = BoundedBelowScope.Create(
            s_Schema, DayZero.NewStyle, new(FirstYear, FirstMonth, FirstDay), 9999);
        // Act
        scope.ValidateYearMonth(FirstYear, FirstMonth);
        AssertEx.ThrowsAoorexn("month", () => scope.ValidateYearMonth(FirstYear, FirstMonth - 1));
    }

    [Fact]
    public static void ValidateYearMonthDay()
    {
        var scope = BoundedBelowScope.Create(
            s_Schema, DayZero.NewStyle, new(FirstYear, FirstMonth, FirstDay), 9999);
        // Act
        scope.ValidateYearMonthDay(FirstYear, FirstMonth, FirstDay);
        scope.ValidateYearMonthDay(FirstYear, FirstMonth, FirstDay + 1);
        scope.ValidateYearMonthDay(FirstYear, FirstMonth + 1, 1);
        AssertEx.ThrowsAoorexn("month", () => scope.ValidateYearMonthDay(FirstYear, FirstMonth - 1, FirstDay));
        AssertEx.ThrowsAoorexn("day", () => scope.ValidateYearMonthDay(FirstYear, FirstMonth, FirstDay - 1));
    }

    [Fact]
    public static void ValidateOrdinal()
    {
        int firstDayOfYear = s_Schema.CountDaysInYearBeforeMonth(FirstYear, FirstMonth) + FirstDay;
        var scope = BoundedBelowScope.Create(
            s_Schema, DayZero.NewStyle, new(FirstYear, FirstMonth, FirstDay), 9999);
        // Act
        scope.ValidateOrdinal(FirstYear, firstDayOfYear);
        AssertEx.ThrowsAoorexn("dayOfYear", () => scope.ValidateOrdinal(FirstYear, firstDayOfYear - 1));
    }
}
