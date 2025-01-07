// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

public partial class PaxCalendar // Complements
{
    /// <summary>
    /// Obtains the number of weeks in the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is outside the
    /// range of supported years.</exception>
    [Pure]
    public int CountWeeksInYear(int year)
    {
        Scope.ValidateYear(year);
        return Schema.CountWeeksInYear(year);
    }
}

/// <summary>
/// Defines the non-standard mathematical operations suitable for use with the
/// <see cref="PaxDate"/> type.
/// <para>This class allows to customize the <see cref="AdditionRule"/> used
/// to resolve ambiguities.</para>
/// </summary>
public sealed class PaxDateMath : DateMathPlain<PaxDate, PaxCalendar>
{
    public PaxDateMath(AdditionRule rule) : base(rule) { }
}

/// <summary>
/// Defines the non-standard mathematical operations suitable for use with the
/// <see cref="PaxMonth"/> type.
/// <para>This class allows to customize the <see cref="AdditionRule"/> used
/// to resolve ambiguities.</para>
/// </summary>
public sealed class PaxMonthMath : MonthMathPlain<PaxMonth, PaxCalendar>
{
    public PaxMonthMath(AdditionRule rule) : base(rule) { }
}
