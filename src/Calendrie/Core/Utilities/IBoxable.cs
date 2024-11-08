﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design)

namespace Calendrie.Core.Utilities;

/// <summary>Defines a boxable type.</summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IBoxable<TSelf>
    where TSelf : class, IBoxable<TSelf>
{
    /// <summary>Creates a new (boxed) instance of the <typeparamref name="TSelf"/> class.</summary>
    [Pure]
    [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "<Pending>")]
    static abstract Box<TSelf> GetInstance();
}
