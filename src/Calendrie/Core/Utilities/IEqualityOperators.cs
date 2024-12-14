// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Utilities;

using System.Numerics;

/// <summary>
/// Defines a mechanism for comparing two values to determine equality.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IEqualityOperators<TSelf> :
    IEqualityOperators<TSelf, TSelf, bool>,
    IEquatable<TSelf>
    where TSelf : IEqualityOperators<TSelf>?
{ }
