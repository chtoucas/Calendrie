﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

// REVIEW(api): which type needs to implement ISchemaBound? segments,
// validators and scopes.

// This interface is only meant to be implemented __explicitely__.

public interface ISchemaBound
{
    /// <summary>Gets the underlying schema.</summary>
    ICalendricalSchema Schema { get; }
}
