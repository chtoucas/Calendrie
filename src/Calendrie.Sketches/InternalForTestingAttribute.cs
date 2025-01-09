// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie;

/// <summary>
/// Indicates that a method or a field has been made internal for testing
/// purposes.
/// </summary>
[AttributeUsage(
    AttributeTargets.Constructor
    | AttributeTargets.Method
    | AttributeTargets.Property
    | AttributeTargets.Field)]
internal sealed class InternalForTestingAttribute : Attribute
{
    /// <summary>
    /// Obtains the accessibiliy level that the method or field would have had
    /// otherwise.
    /// </summary>
    // On ne vérifie pas la valeur passée au "setter", mais a priori ce
    // dernier ne devrait accepter que les valeurs "Private" ou "Protected".
    public AccessibilityLevel GenuineAccessibility { get; set; } =
        AccessibilityLevel.Private;
}
