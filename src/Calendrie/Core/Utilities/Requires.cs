// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Utilities;

// See https://github.com/CommunityToolkit/dotnet/blob/main/src/CommunityToolkit.Diagnostics/Guard.cs
// See also https://docs.microsoft.com/en-us/dotnet/api/microsoft.assumes?view=visualstudiosdk-2022

/// <summary>
/// Provides helper methods to specify preconditions on a method.
/// <para>This class cannot be inherited.</para>
/// <para>If a condition does not hold, an exception is thrown.</para>
/// </summary>
[DebuggerStepThrough]
internal static class Requires
{
    /// <summary>
    /// Validates that the specified value is a member of the enum
    /// <see cref="DayOfWeek"/>.
    /// </summary>
    /// <exception cref="AoorException">Thrown if <paramref name="dayOfWeek"/>
    /// was not a known member of the enum <see cref="DayOfWeek"/>.</exception>
    //
    // CIL code size = 16 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Defined(
        DayOfWeek dayOfWeek,
        [CallerArgumentExpression(nameof(dayOfWeek))] string paramName = "")
    {
        if (DayOfWeek.Sunday <= dayOfWeek && dayOfWeek <= DayOfWeek.Saturday) return;

        DayOfWeekOutOfRange(dayOfWeek, paramName);
    }

    /// <summary>
    /// Validates that the specified value is a member of the enum
    /// <see cref="IsoWeekday"/>.
    /// </summary>
    /// <exception cref="AoorException">Thrown if <paramref name="weekday"/> was
    /// not a known member of the enum <see cref="IsoWeekday"/>.</exception>
    //
    // CIL code size = XXX bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Defined(
        IsoWeekday weekday,
        [CallerArgumentExpression(nameof(weekday))] string paramName = "")
    {
        if (IsoWeekday.Monday <= weekday && weekday <= IsoWeekday.Sunday) return;

        IsoWeekdayOutOfRange(weekday, paramName);
    }

    /// <summary>
    /// Validates that the specified value is a member of the enum
    /// <see cref="AdditionRule"/>.
    /// </summary>
    /// <exception cref="AoorException">Thrown if <paramref name="rule"/> was
    /// not a known member of the enum <see cref="AdditionRule"/>.</exception>
    //
    // CIL code size = XXX bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Defined(
        AdditionRule rule,
        [CallerArgumentExpression(nameof(rule))] string paramName = "")
    {
        if (AdditionRule.Truncate <= rule && rule <= AdditionRule.Overflow) return;

        AdditionRuleOutOfRange(rule, paramName);
    }

    /// <summary>
    /// Validates that the specified schema has the <paramref name="expected"/>
    /// profile.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if <paramref name="schema"/>
    /// did not have the expected profile.</exception>
    public static void Profile(
        CalendricalSchema schema,
        CalendricalProfile expected,
        [CallerArgumentExpression(nameof(schema))] string paramName = "")
    {
        Debug.Assert(schema != null);

        if (schema.Profile == expected) return;

        ThrowHelpers.BadSchemaProfile(paramName, expected, schema.Profile);
    }

    /// <summary>
    /// The value of the day of the week was out of range.
    /// </summary>
    /// <exception cref="AoorException"/>
    [DoesNotReturn]
    private static void DayOfWeekOutOfRange(DayOfWeek dayOfWeek, string paramName) =>
        throw new AoorException(
            paramName,
            dayOfWeek,
            $"The value of the day of the week must be in the range 0 through 6; value = {dayOfWeek}.");

    /// <summary>
    /// The value of the ISO weekday was out of range.
    /// </summary>
    /// <exception cref="AoorException"/>
    [DoesNotReturn]
    private static void IsoWeekdayOutOfRange(IsoWeekday weekday, string paramName) =>
        throw new AoorException(
            paramName,
            weekday,
            $"The value of the ISO weekday must be in the range 0 through 6; value = {weekday}.");

    /// <summary>
    /// The value of the ISO weekday was out of range.
    /// </summary>
    /// <exception cref="AoorException"/>
    [DoesNotReturn]
    private static void AdditionRuleOutOfRange(AdditionRule rule, string paramName) =>
        throw new AoorException(
            paramName,
            rule,
            $"The value of the addition rule must be in the range 0 through 3; value = {rule}.");
}
