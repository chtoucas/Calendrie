﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Intervals;

/// <summary>
/// Defines an interval.
/// <para>Discrete intervals SHOULD always be represented using their normal
/// (closed) form. Roughly speaking, an interval is said to be discrete if all
/// of its members are isolated which, being a topological property, is hard to
/// define in computer words. Anyway, for our purposes, we only need to know that
/// intervals of integers or date-like types are discrete, while intervals of
/// time-like types are not.</para>
/// </summary>
/// <typeparam name="T">The type of the interval elements.</typeparam>
public interface IInterval<in T> : ISetMembership<T>
    where T : struct, IEquatable<T>, IComparable<T>
{
    /// <summary>
    /// Returns <see langword="true"/> if this interval is left-open; otherwise
    /// returns <see langword="false"/>.
    /// <para>A left-bounded interval is said to be left-open if its left
    /// endpoint is not an element of the interval. A left-unbounded interval is
    /// left-open.</para>
    /// </summary>
    bool IsLeftOpen { get; }

    /// <summary>
    /// Returns <see langword="true"/> if this interval is right-open; otherwise
    /// returns <see langword="false"/>.
    /// <para>A right-bounded interval is said to be right-open if its right
    /// endpoint is not an element of the interval. A right-unbounded interval
    /// is right-open.</para>
    /// </summary>
    bool IsRightOpen { get; }

    /// <summary>
    /// Returns <see langword="true"/> if this interval is left-bounded;
    /// otherwise returns <see langword="false"/>.
    /// <para>An interval is said to be left-bounded if it has at least one
    /// lower bound.</para>
    /// </summary>
    bool IsLeftBounded { get; }

    /// <summary>
    /// Returns <see langword="true"/> if this interval is right-bounded;
    /// otherwise returns <see langword="false"/>.
    /// <para>An interval is said to be right-bounded if it has at least one
    /// upper bound.</para>
    /// </summary>
    bool IsRightBounded { get; }
}
