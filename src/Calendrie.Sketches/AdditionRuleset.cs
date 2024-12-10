// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie;

/// <summary>
/// Defines the strategies employed to resolve ambiguities when adding a number
/// of months or years to a calendrical object.
/// <para><see cref="AdditionRuleset"/> is an immutable struct.</para>
/// </summary>
public readonly record struct AdditionRuleset
{
    private readonly AdditionRule _dateRule;
    /// <summary>
    /// Gets or initializes the strategy employed to resolve ambiguities when
    /// adding a number of months or years to a date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">value is outside the range
    /// of its valid values.</exception>
    public AdditionRule DateRule
    {
        get => _dateRule;
        init
        {
            RequiresEx.Defined(value);
            _dateRule = value;
        }
    }

    private readonly AdditionRule _ordinalRule;
    /// <summary>
    /// Gets or initializes the strategy employed to resolve ambiguities when
    /// adding a number of years to an ordinal date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">value is outside the range
    /// of its valid values.</exception>
    public AdditionRule OrdinalRule
    {
        get => _ordinalRule;
        init
        {
            RequiresEx.Defined(value);
            _ordinalRule = value;
        }
    }

    private readonly AdditionRule _monthRule;
    /// <summary>
    /// Gets or initializes the strategy employed to resolve ambiguities when
    /// adding a number of years to a month.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">value is outside the range
    /// of its valid values.</exception>
    public AdditionRule MonthRule
    {
        get => _monthRule;
        init
        {
            RequiresEx.Defined(value);
            _monthRule = value;
        }
    }
}
