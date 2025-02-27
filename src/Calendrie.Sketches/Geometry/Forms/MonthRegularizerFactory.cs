﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Geometry.Forms;

public static class MonthRegularizerFactory
{
    [Pure]
    public static MonthRegularizer CreateRegularizer(
        MonthForm monthForm,
        int monthsInYear,
        int exceptionalMonth)
    {
        ArgumentNullException.ThrowIfNull(monthForm);

        return monthForm switch
        {
            MonthForm { Numbering: MonthFormNumbering.Algebraic } =>
                new AlgebraicMonthRegularizer(monthsInYear, exceptionalMonth),

            MonthForm { Numbering: MonthFormNumbering.Ordinal } =>
                new OrdinalMonthRegularizer(monthsInYear, exceptionalMonth),

            TroeschMonthForm =>
                new TroeschMonthRegularizer(monthsInYear, exceptionalMonth),

            _ => throw new NotSupportedException()
        };
    }
}
