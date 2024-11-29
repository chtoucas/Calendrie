﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Calendrie.Specialized;

using Calendrie.Core.Schemas;
using Calendrie.Hemerology;
using Calendrie.Hemerology.Scopes;

/// <summary>
/// Represents the Civil calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class CivilCalendar : SpecialCalendar<CivilDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CivilCalendar"/>
    /// class.
    /// </summary>
    public CivilCalendar() : this(new CivilSchema()) { }

    internal CivilCalendar(CivilSchema schema) : base("Civil", GetScope(schema))
    {
        OnInitializing(schema);
    }

    private static partial MinMaxYearScope GetScope(CivilSchema schema);

    partial void OnInitializing(CivilSchema schema);

    private protected sealed override CivilDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Provides common adjusters for <see cref="CivilDate"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class CivilAdjuster : SpecialAdjuster<CivilDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CivilAdjuster"/>
    /// class.
    /// </summary>
    public CivilAdjuster() : base(CivilDate.Calendar.Scope) { }

    internal CivilAdjuster(MinMaxYearScope scope) : base(scope) { }

    private protected sealed override CivilDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Represents the Civil date.
/// <para><see cref="CivilDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct CivilDate :
    IDate<CivilDate, CivilCalendar>,
    IAdjustable<CivilDate>
{ }

public partial struct CivilDate // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() => s_Schema.CountDaysInYearBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() => s_Schema.CountDaysInYearAfter(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => s_Schema.CountDaysInMonthBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() => s_Schema.CountDaysInMonthAfter(_daysSinceZero);
}


public partial struct CivilDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(CivilDate left, CivilDate right) =>
        left._daysSinceZero == right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator !=(CivilDate left, CivilDate right) =>
        left._daysSinceZero != right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public bool Equals(CivilDate other) => _daysSinceZero == other._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CivilDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceZero;
}

public partial struct CivilDate // IComparable
{
    /// <inheritdoc />
    public static bool operator <(CivilDate left, CivilDate right) =>
        left._daysSinceZero < right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator <=(CivilDate left, CivilDate right) =>
        left._daysSinceZero <= right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator >(CivilDate left, CivilDate right) =>
        left._daysSinceZero > right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator >=(CivilDate left, CivilDate right) =>
        left._daysSinceZero >= right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public static CivilDate Min(CivilDate x, CivilDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static CivilDate Max(CivilDate x, CivilDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(CivilDate other) => _daysSinceZero.CompareTo(other._daysSinceZero);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is CivilDate date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(CivilDate), obj);
}

