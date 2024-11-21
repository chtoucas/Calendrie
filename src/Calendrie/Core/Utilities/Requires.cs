﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Utilities;

// See https://github.com/CommunityToolkit/dotnet/blob/main/CommunityToolkit.Diagnostics/Guard.cs
// See also https://docs.microsoft.com/en-us/dotnet/api/microsoft.assumes?view=visualstudiosdk-2022

/// <summary>
/// Provides helper methods to specify preconditions on a method.
/// </summary>
/// <remarks>
/// <para>This class cannot be inherited.</para>
/// <para>If a condition does not hold, an exception is thrown.</para>
/// </remarks>
[DebuggerStepThrough]
internal static class Requires
{
    /// <summary>
    /// Validates that the specified value is a member of the enum <see cref="DayOfWeek"/>.
    /// </summary>
    /// <exception cref="AoorException">Thrown if <paramref name="dayOfWeek"/> was not a known
    /// member of the enum <see cref="DayOfWeek"/>.</exception>
    //
    // CIL code size = 16 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Defined(
        DayOfWeek dayOfWeek,
        [CallerArgumentExpression(nameof(dayOfWeek))] string paramName = "")
    {
        if (DayOfWeek.Sunday <= dayOfWeek && dayOfWeek <= DayOfWeek.Saturday) return;

        ThrowHelpers.DayOfWeekOutOfRange(dayOfWeek, paramName);
    }

    /// <summary>
    /// Validates that the specified value is a member of the enum <see cref="IsoWeekday"/>.
    /// </summary>
    /// <exception cref="AoorException">Thrown if <paramref name="weekday"/> was not a known member
    /// of the enum <see cref="IsoWeekday"/>.</exception>
    //
    // CIL code size = XXX bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Defined(
        IsoWeekday weekday,
        [CallerArgumentExpression(nameof(weekday))] string paramName = "")
    {
        if (IsoWeekday.Monday <= weekday && weekday <= IsoWeekday.Sunday) return;

        ThrowHelpers.IsoWeekdayOutOfRange(weekday, paramName);
    }

    /// <summary>
    /// Validates that the specified value is a member of the enum
    /// <see cref="AdditionRule"/>.
    /// </summary>
    /// <exception cref="AoorException">Thrown if <paramref name="rule"/> was not a known member of
    /// the enum <see cref="AdditionRule"/>.</exception>
    //
    // CIL code size = XXX bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Defined(
        AdditionRule rule,
        [CallerArgumentExpression(nameof(rule))] string paramName = "")
    {
        if (AdditionRule.Truncate <= rule && rule <= AdditionRule.Overflow) return;

        ThrowHelpers.AdditionRuleOutOfRange(rule, paramName);
    }

    /// <summary>
    /// Validates that the specified schema has the <paramref name="expected"/> profile.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if <paramref name="schema"/> did not have the
    /// expected profile.</exception>
    public static void Profile(
        CalendricalSchema schema,
        CalendricalProfile expected,
        [CallerArgumentExpression(nameof(schema))] string paramName = "")
    {
        Debug.Assert(schema != null);

        if (schema.Profile == expected) return;

        ThrowHelpers.BadSchemaProfile(paramName, expected, schema.Profile);
    }
}
