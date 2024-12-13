﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

// Funny interface: it's public but it can ONLY be implemented from within friend
// assemblies, having a static __internal__ method.

public interface ISchemaActivator<TSelf> where TSelf : ICalendricalSchema
{
    /// <summary>
    /// Creates a new instance of the <typeparamref name="TSelf"/> class.
    /// </summary>
    [Pure] internal static abstract TSelf CreateInstance();
}