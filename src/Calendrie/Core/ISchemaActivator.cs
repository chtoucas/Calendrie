﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

// Funny interface: it's public but it can ONLY be implemented from within friend
// assemblies, having a static __internal__ method.

/// <summary>
/// Defines an internal method to create new instances of a given schema type.
/// </summary>
/// <typeparam name="TSchema">The type of schema to be created.</typeparam>
public interface ISchemaActivator<TSchema> where TSchema : ICalendricalSchema
{
    /// <summary>
    /// Creates a new instance of the <typeparamref name="TSchema"/> class.
    /// <para>This method can ONLY be called from within friend assemblies.</para>
    /// </summary>
    [Pure] internal static abstract TSchema CreateInstance();
}
