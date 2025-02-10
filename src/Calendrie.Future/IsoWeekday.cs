// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie;

using Calendrie.Core.Utilities;

// Avoir une valeur _par défaut_ invalide ne me plaît guère. Ceci dit, je ne
// vois pas comment faire autrement, et ce n'est pas important car on utilise
// presque exclusivement l'énumération DayOfWeek qui n'a pas ce problème.
// IsoDayOfWeek ou IsoWeekday ? On adopte la terminologie ISO qui semble
// préférer "weekday" à "day of the week". On garde quand même le préfixe ISO
// pour être bien sûr qu'il n'y ait pas de risque de confusion entre DayOfWeek
// et IsoWeekday.

/// <summary>
/// Specifies the ISO weekday.
/// </summary>
public enum IsoWeekday
{
    /// <summary>
    /// Indicates an unknown ISO weekday.
    /// <para>This value is considered to be <i>invalid</i>. We never use it,
    /// and neither should you.</para>
    /// </summary>
    None = 0,

    /// <summary>
    /// Indicates Monday.
    /// </summary>
    Monday = 1,

    /// <summary>
    /// Indicates Tuesday.
    /// </summary>
    Tuesday = 2,

    /// <summary>
    /// Indicates Wednesday.
    /// </summary>
    Wednesday = 3,

    /// <summary>
    /// Indicates Thursday.
    /// </summary>
    Thursday = 4,

    /// <summary>
    /// Indicates Friday.
    /// </summary>
    Friday = 5,

    /// <summary>
    /// Indicates Saturday.
    /// </summary>
    Saturday = 6,

    /// <summary>
    /// Indicates Sunday.
    /// </summary>
    Sunday = 7
}

/// <summary>
/// Provides extension methods for <see cref="IsoWeekday"/> and <see cref="DayOfWeek"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class IsoWeekdayExtensions
{
    /// <summary>
    /// Converts the specified <see cref="DayOfWeek"/> value to the equivalent
    /// <see cref="IsoWeekday"/>.
    /// </summary>
    [Pure]
    public static IsoWeekday ToIsoWeekday(this DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        return dayOfWeek == DayOfWeek.Sunday ? IsoWeekday.Sunday : (IsoWeekday)dayOfWeek;
    }

    /// <summary>
    /// Converts the specified <see cref="IsoWeekday"/> value to the equivalent
    /// <see cref="DayOfWeek"/>.
    /// </summary>
    [Pure]
    public static DayOfWeek ToDayOfWeek(this IsoWeekday weekday)
    {
        // TODO(code): move validation to Requires.
        Defined(weekday);

        return (DayOfWeek)((int)weekday % 7);
    }

    /// <summary>
    /// Validates that the specified value is a member of the enum
    /// <see cref="IsoWeekday"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="weekday"/>
    /// was not a known member of the enum <see cref="IsoWeekday"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Defined(
        IsoWeekday weekday,
        [CallerArgumentExpression(nameof(weekday))] string paramName = "")
    {
        if (IsoWeekday.Monday <= weekday && weekday <= IsoWeekday.Sunday) return;

        fail(weekday, paramName);

        [DoesNotReturn]
        static void fail(IsoWeekday weekday, string paramName) =>
            throw new ArgumentOutOfRangeException(
                paramName,
                weekday,
                "The value of the day of the week must be in the range 0 through 7");
    }

}
