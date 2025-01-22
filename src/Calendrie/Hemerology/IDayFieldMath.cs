// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core.Utilities;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Defines the mathematical operations on the day field of a time-related type.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IDayFieldMath<TSelf>
    where TSelf : IDayFieldMath<TSelf>
{
    /// <summary>
    /// Counts the number of whole days from the specified <typeparamref name="TSelf"/>
    /// value to the current instance.
    /// </summary>
    [Pure] int CountDaysSince(TSelf other);

    /// <summary>
    /// Adds a number of days to the day field of the current instance,
    /// yielding a new value.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of the day field or the range of supported values.
    /// </exception>
    [Pure] TSelf PlusDays(int days);

    /// <summary>
    /// Returns the value obtained after adding one day to the day field of the
    /// current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported value.</exception>
    [Pure] TSelf NextDay() => PlusDays(1);

    /// <summary>
    /// Returns the value obtained after subtracting one day to the day field of
    /// the current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported value.</exception>
    [Pure] TSelf PreviousDay() => PlusDays(-1);

    //
    // Math operations based on the week unit
    //
    // Being default interface methods and types implementing this interface
    // being most certainly value types, they should override them. Even for
    // reference types, it's a good idea to implement them explicitely in order
    // to make them available to all derived classes.

    /// <summary>
    /// Counts the number of whole weeks from the specified <typeparamref name="TSelf"/>
    /// value to the current instance.
    /// </summary>
    [Pure] int CountWeeksSince(TSelf other) => MathZ.Divide(CountDaysSince(other), DaysInWeek);

    /// <summary>
    /// Adds a number of weeks to the day field of the current instance,
    /// yielding a new value.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of the day field or the range of supported values.
    /// </exception>
    [Pure] TSelf PlusWeeks(int weeks) => PlusDays(DaysInWeek * weeks);

    /// <summary>
    /// Returns the value obtained after adding seven days to the day field of
    /// the current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported value.</exception>
    [Pure] TSelf NextWeek() => PlusDays(DaysInWeek);

    /// <summary>
    /// Returns the value obtained after subtracting seven days to the day field
    /// of the current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported value.</exception>
    [Pure] TSelf PreviousWeek() => PlusDays(-DaysInWeek);
}
