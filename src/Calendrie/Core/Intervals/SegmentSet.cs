// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Intervals;

using System.Numerics;

// REVIEW(code): add factory Create(Segment<T>), Singleton()?

// SegmentSet<T> is the return type for Intersect() and Gap().
//
// Beware, if the runtime size of SegmentSet<T> and IntervalBoundary<T> is
// equal to 12 bytes w/ Int32, it starts to be too large w/ Int64 or Double
// (24 bytes); see Calendrie.ValueTypeFacts.

/// <summary>
/// Provides static helpers for <see cref="SegmentSet{T}"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class SegmentSet
{
    #region Factories

    /// <summary>
    /// Obtains the empty range.
    /// <para>The empty range is both an intersection absorber and a span
    /// identity.</para>
    /// </summary>
    [Pure]
    public static SegmentSet<T> Empty<T>()
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return SegmentSet<T>.Empty;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SegmentSet{T}"/> struct
    /// representing the range [<paramref name="min"/>..<paramref name="max"/>].
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="max"/> is
    /// less than <paramref name="min"/>.</exception>
    [Pure]
    public static SegmentSet<T> Create<T>(T min, T max)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(min, max);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SegmentSet{T}"/> struct
    /// representing the range [<paramref name="min"/>..<paramref name="max"/>].
    /// <para>This factory method does NOT validate its parameters.</para>
    /// </summary>
    [Pure]
    internal static SegmentSet<T> UnsafeCreate<T>(T min, T max)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(OrderedPair.UnsafeCreate(min, max));
    }

    #endregion
    #region Conversions

    /// <summary>
    /// Creates a new instance of the <see cref="SegmentSet{T}"/> struct from the
    /// specified endpoints.
    /// </summary>
    [Pure]
    public static SegmentSet<T> FromEndpoints<T>(OrderedPair<T> endpoints)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(endpoints);
    }

    #endregion
}

/// <summary>
/// Represents a (possibly empty) closed bounded interval.
/// <para>An instance may be empty or reduced to a single value.</para>
/// <para><i>Unless you need to take the intersection of two ranges, you most
/// certainly should use <see cref="Segment{T}"/> instead.</i></para>
/// <para><typeparamref name="T"/> SHOULD be an <i>immutable</i> value type.
/// </para>
/// <para><see cref="SegmentSet{T}"/> is an immutable struct.</para>
/// </summary>
/// <typeparam name="T">The type of the interval's elements.</typeparam>
public readonly partial struct SegmentSet<T> :
    IEqualityOperators<SegmentSet<T>, SegmentSet<T>, bool>,
    IEquatable<SegmentSet<T>>
    where T : struct, IEquatable<T>, IComparable<T>
{
    // Default value = empty set, _isInhabited = false and _endpoints = (default(T), default(T)).

    /// <summary>
    /// Represents the pair of left and right endpoints if this range is
    /// inhabited.
    /// </summary>
    private readonly OrderedPair<T> _endpoints;

    private readonly bool _isInhabited;

    /// <summary>
    /// Initializes a new instance of the <see cref="SegmentSet{T}"/> struct
    /// representing the range [<paramref name="min"/>..<paramref name="max"/>].
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="max"/> is
    /// less than <paramref name="min"/>.</exception>
    public SegmentSet(T min, T max)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(max, min);

        _endpoints = OrderedPair.UnsafeCreate(min, max);
        _isInhabited = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SegmentSet{T}"/> struct from
    /// the specified endpoints.
    /// </summary>
    // Public version: see SegmentSet.FromEndpoints().
    internal SegmentSet(OrderedPair<T> endpoints)
    {
        _endpoints = endpoints;
        _isInhabited = true;
    }

    /// <summary>
    /// Represents the empty range.
    /// <para>The empty range is both an intersection absorber and a span
    /// identity.</para>
    /// </summary>
    internal static SegmentSet<T> Empty { get; }

    /// <summary>
    /// Returns <see langword="true"/> if this range is empty; otherwise returns
    /// <see langword="false"/>.
    /// </summary>
    public bool IsEmpty => !_isInhabited;

    /// <summary>
    /// Returns a <see cref="Segment{T}"/> view of this range.
    /// </summary>
    /// <exception cref="InvalidOperationException">The set is empty.</exception>
    public Segment<T> Range => _isInhabited ? new Segment<T>(_endpoints)
        : throw new InvalidOperationException("The set was empty.");

    /// <summary>
    /// Returns a culture-independent string representation of this range.
    /// </summary>
    [Pure]
    public override string ToString() => _isInhabited ? Range.ToString() : "[]";
}

public partial struct SegmentSet<T> // IEquatable
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="SegmentSet{T}"/>
    /// are equal.
    /// </summary>
    public static bool operator ==(SegmentSet<T> left, SegmentSet<T> right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of <see cref="SegmentSet{T}"/>
    /// are not equal.
    /// </summary>
    public static bool operator !=(SegmentSet<T> left, SegmentSet<T> right) => !left.Equals(right);

    /// <inheritdoc />
    [Pure]
    public bool Equals(SegmentSet<T> other) =>
        _isInhabited == other._isInhabited
        && _endpoints.Equals(other._endpoints);

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is SegmentSet<T> range && Equals(range);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => HashCode.Combine(_isInhabited, _endpoints);
}
