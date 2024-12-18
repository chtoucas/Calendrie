// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System.Diagnostics.Contracts;

using Calendrie.Systems;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Provides static helpers and extension methods for <see cref="CivilDate"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static partial class CivilDateExtensions { }

// Arithmetic
//
public partial class CivilDateExtensions
{
    [Pure]
    public static CivilDate AddWeeks(this CivilDate date, int weeks) => date.AddDays(DaysInWeek * weeks);

    [Pure]
    public static CivilDate NextWeek(this CivilDate date) => date.AddDays(DaysInWeek);

    [Pure]
    public static CivilDate PreviousWeek(this CivilDate date) => date.AddDays(-DaysInWeek);
}

// Interconversion
//
public partial class CivilDateExtensions
{
    //
    // CivilDate -> other date types
    //

    // May throw an ArgumentOutOfRangeException.
    [Pure]
    public static GregorianDate ToGregorianDate(this CivilDate date) =>
        GregorianDate.FromDayNumber(date.DayNumber);

    // Simpler, faster, no exceptions: there is an implicit conversion from
    // CivilDate to GregorianDate, or if you prefer you can use the more explicit
    // version: GregorianDate.FromCivilDate(date).
    [Pure]
    public static GregorianDate AsGregorianDate(this CivilDate date) => date;

    // May throw an ArgumentOutOfRangeException.
    [Pure]
    public static JulianDate ToJulianDate(this CivilDate date) =>
        JulianDate.FromDayNumber(date.DayNumber);

    // May throw an ArgumentOutOfRangeException.
    [Pure]
    public static WorldDate ToWorldDate(this CivilDate date) =>
        WorldDate.FromDayNumber(date.DayNumber);

    //
    // Other date types -> CivilDate
    //

    // May throw an ArgumentOutOfRangeException.
    [Pure]
    public static CivilDate FromGregorianDate(GregorianDate date) =>
        CivilDate.FromDayNumber(date.DayNumber);

    // May throw an ArgumentOutOfRangeException.
    [Pure]
    public static CivilDate FromJulianDate(JulianDate date) =>
        CivilDate.FromDayNumber(date.DayNumber);

    // May throw an ArgumentOutOfRangeException.
    [Pure]
    public static CivilDate FromWorldDate(WorldDate date) =>
        CivilDate.FromDayNumber(date.DayNumber);
}
