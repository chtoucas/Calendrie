﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Utilities;

/// <summary>Defines an adjustable type.</summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IAdjustable<TSelf>
    where TSelf : IAdjustable<TSelf>
{
    /// <summary>Adjusts the current instance using the specified adjuster.
    /// <para>If the adjuster throws, this method will propagate the exception.</para>
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="adjuster"/> is null.</exception>
    [Pure] TSelf Adjust(Func<TSelf, TSelf> adjuster);
}
