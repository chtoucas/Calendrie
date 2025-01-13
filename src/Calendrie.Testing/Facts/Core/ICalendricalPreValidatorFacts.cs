// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core;
using Calendrie.Testing.Data;

/// <summary>
/// Provides facts about the <see cref="ICalendricalPreValidator"/> type.
/// </summary>
public abstract partial class ICalendricalPreValidatorFacts<TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected ICalendricalPreValidatorFacts(ICalendricalSchema schema)
    {
        ArgumentNullException.ThrowIfNull(schema);

        PreValidatorUT = schema.PreValidator;
        (MinYear, MaxYear) = schema.SupportedYears.Endpoints;
    }

    protected ICalendricalPreValidatorFacts(
        ICalendricalPreValidator validator,
        int minYear,
        int maxYear)
    {
        ArgumentNullException.ThrowIfNull(validator);

        PreValidatorUT = validator;
        MinYear = minYear;
        MaxYear = maxYear;
    }

    /// <summary>
    /// Gets the minimal year for which the methods are known not to overflow.
    /// </summary>
    protected int MinYear { get; }
    /// <summary>
    /// Gets the maximal year for which the methods are known not to overflow.
    /// </summary>
    protected int MaxYear { get; }

    /// <summary>
    /// Gets the scope under test.
    /// </summary>
    protected ICalendricalPreValidator PreValidatorUT { get; }
}

public partial class ICalendricalPreValidatorFacts<TDataSet> // Methods
{
    #region CheckMonth()

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void CheckMonth_InvalidMonthOfYear(int y, int m) =>
        Assert.False(PreValidatorUT.CheckMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CheckMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        Assert.True(PreValidatorUT.CheckMonth(y, m));
    }

    #endregion
    #region ValidateMonth()

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void ValidateMonth_InvalidMonthOfYear(int y, int m)
    {
        AssertEx.ThrowsAoorexn("month", () => PreValidatorUT.ValidateMonth(y, m));
        AssertEx.ThrowsAoorexn("m", () => PreValidatorUT.ValidateMonth(y, m, nameof(m)));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void ValidateMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        PreValidatorUT.ValidateMonth(y, m);
    }

    #endregion

    #region CheckMonthDay()

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void CheckMonthDay_InvalidMonth(int y, int m) =>
        Assert.False(PreValidatorUT.CheckMonthDay(y, m, 1));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void CheckMonthDay_InvalidDay(int y, int m, int d) =>
        Assert.False(PreValidatorUT.CheckMonthDay(y, m, d));

    [Theory, MemberData(nameof(DateInfoData))]
    public void CheckMonthDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        Assert.True(PreValidatorUT.CheckMonthDay(y, m, d));
    }

    #endregion
    #region ValidateMonthDay()

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void ValidateMonthDay_InvalidMonth(int y, int m)
    {
        AssertEx.ThrowsAoorexn("month", () => PreValidatorUT.ValidateMonthDay(y, m, 1));
        AssertEx.ThrowsAoorexn("m", () => PreValidatorUT.ValidateMonthDay(y, m, 1, nameof(m)));
    }

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void ValidateMonthDay_InvalidDay(int y, int m, int d)
    {
        AssertEx.ThrowsAoorexn("day", () => PreValidatorUT.ValidateMonthDay(y, m, d));
        AssertEx.ThrowsAoorexn("d", () => PreValidatorUT.ValidateMonthDay(y, m, d, nameof(d)));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void ValidateMonthDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        PreValidatorUT.ValidateMonthDay(y, m, d);
    }

    #endregion

    #region CheckDayOfYear()

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void CheckDayOfYear_InvalidDayOfYear(int y, int doy) =>
        Assert.False(PreValidatorUT.CheckDayOfYear(y, doy));

    [Theory, MemberData(nameof(DateInfoData))]
    public void CheckDayOfYear(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        Assert.True(PreValidatorUT.CheckDayOfYear(y, doy));
    }

    #endregion
    #region ValidateDayOfYear()

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void ValidateDayOfYear_InvalidDayOfYear(int y, int doy)
    {
        AssertEx.ThrowsAoorexn("dayOfYear", () => PreValidatorUT.ValidateDayOfYear(y, doy));
        AssertEx.ThrowsAoorexn("doy", () => PreValidatorUT.ValidateDayOfYear(y, doy, nameof(doy)));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void ValidateDayOfYear(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        PreValidatorUT.ValidateDayOfYear(y, doy);
    }

    #endregion

    #region ValidateDayOfMonth()

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void ValidateDayOfMonth_InvalidDay(int y, int m, int d)
    {
        AssertEx.ThrowsAoorexn("day", () => PreValidatorUT.ValidateDayOfMonth(y, m, d));
        AssertEx.ThrowsAoorexn("d", () => PreValidatorUT.ValidateDayOfMonth(y, m, d, nameof(d)));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void ValidateDayOfMonth(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        PreValidatorUT.ValidateDayOfMonth(y, m, d);
    }

    #endregion
}

public partial class ICalendricalPreValidatorFacts<TDataSet> // Overflows
{
    // Verification that the methods __ignore the year param__ and do not overflow.
    //
    // Usually, a pre-validator does not perform any arithmetical operation
    // itself, it delegates them to a schema. In practice, it all depends on the
    // behaviour of IsLeapYear() which, hopefully, we fully test elsewhere.
    //
    // Underflow:
    // To simplify, we test at the start of a year, month or day which usually
    // never overflow because we use shortcuts (MinDaysInYear/Month) to avoid
    // having to perform any calculation; this is not the case, to some extent,
    // with PlainPreValidator.
    //
    // Overflow:
    // We use 12 for the month field and 353 for the day of the year field.
    // These are values that are valid for all current schemas --- this might
    // not be the case in the future.
    //
    //
    // A consequence of these two remarks is that the following tests are not
    // that interesting.

    // Within the range [MinYear..MaxYear], we should never observe any overflow.

    [Fact] public void ValidateMonth_DoesNotUnderflow() => PreValidatorUT.ValidateMonth(MinYear, 1);
    [Fact] public void ValidateMonth_DoesNotOverflow() => PreValidatorUT.ValidateMonth(MaxYear, 12);

    [Fact] public void ValidateMonthDay_DoesNotUnderflow() => PreValidatorUT.ValidateMonthDay(MinYear, 1, 1);
    [Fact] public void ValidateMonthDay_DoesNotOverflow() => PreValidatorUT.ValidateMonthDay(MaxYear, 12, 1);

    [Fact] public void ValidateDayOfYear_DoesNotUnderflow() => PreValidatorUT.ValidateDayOfYear(MinYear, 1);
    [Fact] public void ValidateDayOfYear_DoesNotOverflow() => PreValidatorUT.ValidateDayOfYear(MaxYear, 12);

    // Outside the range of supported years, we don't know for sure, but one can
    // override the tests if necessary.
    //
    // The Civil schema is not "proleptic", but since its pre-validator is the
    // Gregorian validator, we can use int.MinValue during the tests.

    [Fact] public virtual void ValidateMonth_AtAbsoluteMinYear() => PreValidatorUT.ValidateMonth(int.MinValue, 1);
    [Fact] public virtual void ValidateMonth_AtAbsoluteMaxYear() => PreValidatorUT.ValidateMonth(int.MaxValue, 12);

    [Fact] public virtual void ValidateMonthDay_AtAbsoluteMinYear() => PreValidatorUT.ValidateMonthDay(int.MinValue, 1, 1);
    [Fact] public virtual void ValidateMonthDay_AtAbsoluteMaxYear() => PreValidatorUT.ValidateMonthDay(int.MaxValue, 12, 1);

    [Fact] public virtual void ValidateDayOfYear_AtAbsoluteMinYear() => PreValidatorUT.ValidateDayOfYear(int.MinValue, 1);
    [Fact] public virtual void ValidateDayOfYear_AtAbsoluteMaxYear() => PreValidatorUT.ValidateDayOfYear(int.MaxValue, 353);
}
