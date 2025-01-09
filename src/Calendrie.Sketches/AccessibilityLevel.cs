// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie;

// Documentation copied from the following reference:
// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/accessibility-levels

/// <summary>
/// Specify the accessibility level of a member or a type.
/// </summary>
internal enum AccessibilityLevel
{
    /// <summary>
    /// Access is not restricted.
    /// </summary>
    Public = 0,

    /// <summary>
    /// Access is limited to the containing class or types derived from the
    /// containing class.
    /// </summary>
    Protected,

    /// <summary>
    /// Access is limited to the current assembly.
    /// </summary>
    Internal,

    /// <summary>
    /// Access is limited to the current assembly or types derived from the
    /// containing class.
    /// </summary>
    ProtectedInternal,

    /// <summary>
    /// Access is limited to the containing type.
    /// </summary>
    Private,

    /// <summary>
    /// Access is limited to the containing class or types derived from the
    /// containing class within the current assembly. Available since C# 7.2.
    /// </summary>
    PrivateProtected,
}
