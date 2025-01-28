// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Geometry.Forms;

/// <summary>
/// Provides extension methods for <see cref="MonthForm"/>.
/// </summary>
public static class MonthFormExtensions
{
    [Pure]
    public static MonthForm WithAlgebraicNumbering(this MonthForm form)
    {
        ArgumentNullException.ThrowIfNull(form);

        return form switch
        {
            MonthForm { Numbering: MonthFormNumbering.Algebraic } => form,

            MonthForm { Numbering: MonthFormNumbering.Ordinal } =>
                AdjustNumbering(form, MonthFormNumbering.Algebraic, -1),

            TroeschMonthForm t =>
                AdjustNumbering(t, MonthFormNumbering.Algebraic, -t.ExceptionalMonth - 1),

            _ => throw new NotSupportedException()
        };
    }

    [Pure]
    public static MonthForm WithOrdinalNumbering(this MonthForm form)
    {
        ArgumentNullException.ThrowIfNull(form);

        return form switch
        {
            MonthForm { Numbering: MonthFormNumbering.Algebraic } =>
                AdjustNumbering(form, MonthFormNumbering.Ordinal, 1),

            MonthForm { Numbering: MonthFormNumbering.Ordinal } => form,

            TroeschMonthForm t =>
                AdjustNumbering(t, MonthFormNumbering.Ordinal, -t.ExceptionalMonth),

            _ => throw new NotSupportedException()
        };
    }

    [Pure]
    public static TroeschMonthForm WithTroeschNumbering(
        this MonthForm form, int exceptionalMonth)
    {
        ArgumentNullException.ThrowIfNull(form);

        return form switch
        {
            MonthForm { Numbering: MonthFormNumbering.Algebraic } =>
                withTroeschNumbering(form, exceptionalMonth + 1, exceptionalMonth),

            MonthForm { Numbering: MonthFormNumbering.Ordinal } =>
                withTroeschNumbering(form, exceptionalMonth, exceptionalMonth),

            TroeschMonthForm t => t,

            _ => throw new NotSupportedException()
        };

        static TroeschMonthForm withTroeschNumbering(
            MonthForm form, int offset, int exceptionalMonth)
        {
            Debug.Assert(form != null);

            return new(
                form.A,
                form.B,
                form.Remainder - form.A * offset,
                exceptionalMonth)
            // REVIEW: Origin.
            { Origin = form.Origin };
        }
    }

    [Pure]
    private static MonthForm AdjustNumbering(
        MonthForm form, MonthFormNumbering numbering, int offset)
    {
        Debug.Assert(form != null);

        return new(
            form.A,
            form.B,
            form.Remainder - form.A * offset,
            numbering)
        // REVIEW: Origin.
        { Origin = form.Origin };
    }
}
