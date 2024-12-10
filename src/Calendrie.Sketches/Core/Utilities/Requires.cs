// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Utilities;

/// <summary>
/// Provides helper methods to specify preconditions on a method.
/// <para>This class cannot be inherited.</para>
/// <para>If a condition does not hold, an exception is thrown.</para>
/// </summary>
[DebuggerStepThrough]
internal static class RequiresEx
{
    /// <summary>
    /// Validates that the specified value is a member of the enum
    /// <see cref="IsoWeekday"/>.
    /// </summary>
    /// <exception cref="AoorException">Thrown if <paramref name="weekday"/> was
    /// not a known member of the enum <see cref="IsoWeekday"/>.</exception>
    public static void Defined(
        IsoWeekday weekday,
        [CallerArgumentExpression(nameof(weekday))] string paramName = "")
    {
        if (IsoWeekday.Monday <= weekday && weekday <= IsoWeekday.Sunday) return;

        fail(weekday, paramName);

        static void fail(IsoWeekday weekday, string paramName) =>
            throw new AoorException(
                paramName,
                weekday,
                $"The value of the ISO weekday must be in the range 0 through 6; value = {weekday}.");

    }

    /// <summary>
    /// Validates that the specified value is a member of the enum
    /// <see cref="AdditionRule"/>.
    /// </summary>
    /// <exception cref="AoorException">Thrown if <paramref name="rule"/> was
    /// not a known member of the enum <see cref="AdditionRule"/>.</exception>
    public static void Defined(
        AdditionRule rule,
        [CallerArgumentExpression(nameof(rule))] string paramName = "")
    {
        if (AdditionRule.Truncate <= rule && rule <= AdditionRule.Overflow) return;

        fail(rule, paramName);

        static void fail(AdditionRule rule, string paramName) =>
            throw new AoorException(
                paramName,
                rule,
                $"The value of the addition rule must be in the range 0 through 3; value = {rule}.");
    }
}
