// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie.Core.Utilities;

//[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly partial struct CivilAnnual :
    // Comparison
    IEqualityOperators<CivilAnnual, CivilAnnual, bool>,
    IEquatable<CivilAnnual>,
    IComparisonOperators<CivilAnnual, CivilAnnual, bool>,
    IComparable<CivilAnnual>,
    IComparable
{
    private readonly CivilDate _reference;

    public CivilAnnual(int dayOfYear) : this(new CivilDate(1, dayOfYear)) { }

    public CivilAnnual(int month, int day) : this(new CivilDate(1, month, day)) { }

    public CivilAnnual(CivilDate reference)
    {
        _reference = reference;
    }

    public static CivilCalendar Calendar => CivilCalendar.Instance;

    public int Month => _reference.Month;

    public int Day => _reference.Day;

    public void Deconstruct(out int month, out int day) => (_, month, day) = _reference;

    public CivilAnnual GetAnniversaryAfter(int years) //, AdditionRule rule = AdditionRule.Truncate)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(years, 0);

        return years == 0 ? this : new CivilAnnual(_reference.PlusYears(years));
    }
}

public partial struct CivilAnnual // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(CivilAnnual left, CivilAnnual right) =>
        left._reference == right._reference;

    /// <inheritdoc />
    public static bool operator !=(CivilAnnual left, CivilAnnual right) =>
        left._reference != right._reference;

    /// <inheritdoc />
    [Pure]
    public bool Equals(CivilAnnual other) => _reference == other._reference;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CivilAnnual date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _reference.GetHashCode();
}

public partial struct CivilAnnual // IComparable
{
    /// <summary>
    /// Compares the two specified dates to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(CivilAnnual left, CivilAnnual right) =>
        left._reference < right._reference;

    /// <summary>
    /// Compares the two specified dates to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(CivilAnnual left, CivilAnnual right) =>
        left._reference <= right._reference;

    /// <summary>
    /// Compares the two specified dates to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(CivilAnnual left, CivilAnnual right) =>
        left._reference > right._reference;

    /// <summary>
    /// Compares the two specified dates to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(CivilAnnual left, CivilAnnual right) =>
        left._reference >= right._reference;

    /// <inheritdoc />
    [Pure]
    public static CivilAnnual Min(CivilAnnual x, CivilAnnual y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static CivilAnnual Max(CivilAnnual x, CivilAnnual y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(CivilAnnual other) => _reference.CompareTo(other._reference);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is CivilAnnual date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(CivilAnnual), obj);
}
