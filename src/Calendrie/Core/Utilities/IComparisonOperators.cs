﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Utilities;

using System.Numerics;

/// <summary>
/// Defines a mechanism for comparing two values to determine relative order.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IComparisonOperators<TSelf> :
    IComparisonOperators<TSelf, TSelf, bool>,
    IEqualityOperators<TSelf>,
    IComparable<TSelf>,
    IComparable
    where TSelf : IComparisonOperators<TSelf>?
{ }
