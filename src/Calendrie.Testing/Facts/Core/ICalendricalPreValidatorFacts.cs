﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core;
using Calendrie.Testing.Data;

/// <summary>
/// Provides facts about <see cref="ICalendricalPreValidator"/>.
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
    /// Gets a minimum value of the year for which the methods are known not to overflow.
    /// </summary>
    protected int MinYear { get; }
    /// <summary>
    /// Gets a maximum value of the year for which the methods are known not to overflow.
    /// </summary>
    protected int MaxYear { get; }

    /// <summary>
    /// Gets the scope under test.
    /// </summary>
    protected ICalendricalPreValidator PreValidatorUT { get; }
}

public partial class ICalendricalPreValidatorFacts<TDataSet> // Methods
{
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
    // Verification that the methods ignore the year param and do not overflow.
    //
    // Usually, a pre-validator does not perform any arithmetical operation
    // itself, it delegates them to a schema. In practice, it all depends on the
    // behaviour of IsLeapYear() which, hopefully, we fully test elsewhere.
    //
    // To simplify, we only test the start of a month or a day which usually
    // never overflow because we use shortcuts (MinDaysInYear/Month) to avoid
    // having to perform any calculation; this is not the case with
    // CalendricalPreValidator and, to some extent, PlainPreValidator.
    //
    // A consequence of these two remarks is that the following tests are not
    // that interesting.

    // Within the range [MinYear..MaxYear], we should never observe any overflow.

    [Fact] public void ValidateMonth_DoesNotUnderflow() => PreValidatorUT.ValidateMonth(MinYear, 1);
    [Fact] public void ValidateMonth_DoesNotOverflow() => PreValidatorUT.ValidateMonth(MaxYear, 1);

    [Fact] public void ValidateMonthDay_DoesNotUnderflow() => PreValidatorUT.ValidateMonthDay(MinYear, 1, 1);
    [Fact] public void ValidateMonthDay_DoesNotOverflow() => PreValidatorUT.ValidateMonthDay(MaxYear, 1, 1);

    [Fact] public void ValidateDayOfYear_DoesNotUnderflow() => PreValidatorUT.ValidateDayOfYear(MinYear, 1);
    [Fact] public void ValidateDayOfYear_DoesNotOverflow() => PreValidatorUT.ValidateDayOfYear(MaxYear, 1);

    // Outside the range of supported years, we don't know for sure, but one can
    // override the tests if necessary.

    [Fact] public virtual void ValidateMonth_AtAbsoluteMinYear() => PreValidatorUT.ValidateMonth(int.MinValue, 1);
    [Fact] public virtual void ValidateMonth_AtAbsoluteMaxYear() => PreValidatorUT.ValidateMonth(int.MaxValue, 1);

    [Fact] public virtual void ValidateMonthDay_AtAbsoluteMinYear() => PreValidatorUT.ValidateMonthDay(int.MinValue, 1, 1);
    [Fact] public virtual void ValidateMonthDay_AtAbsoluteMaxYear() => PreValidatorUT.ValidateMonthDay(int.MaxValue, 1, 1);

    [Fact] public virtual void ValidateDayOfYear_AtAbsoluteMinYear() => PreValidatorUT.ValidateDayOfYear(int.MinValue, 1);
    [Fact] public virtual void ValidateDayOfYear_AtAbsoluteMaxYear() => PreValidatorUT.ValidateDayOfYear(int.MaxValue, 1);
}
