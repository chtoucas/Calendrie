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
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="dayOfWeek"/>
    /// was not a known member of the enum <see cref="DayOfWeek"/>.</exception>
    public static void Defined(
        DayOfWeek dayOfWeek,
        [CallerArgumentExpression(nameof(dayOfWeek))] string paramName = "")
    {
        if (DayOfWeek.Sunday <= dayOfWeek && dayOfWeek <= DayOfWeek.Saturday) return;

        fail(dayOfWeek, paramName);

        [DoesNotReturn]
        static void fail(DayOfWeek dayOfWeek, string paramName) =>
            throw new ArgumentOutOfRangeException(
                paramName,
                dayOfWeek,
                $"The value of the day of the week must be in the range 0 through 6; value = {dayOfWeek}.");
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

        fail(paramName, expected, schema.Profile);

        [DoesNotReturn]
        static void fail(string paramName, CalendricalProfile expected, CalendricalProfile actual) =>
            throw new ArgumentException(
                $"The schema profile should be equal to \"{expected}\" but it is equal to \"{actual}\".",
                paramName);
    }
}
