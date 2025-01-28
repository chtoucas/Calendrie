// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Geometry.Forms;

// Permet de passer d'une représentation (y, m) ordinaire vers celle où on
// positionne le mois contenant les jours bissextils en fin d'année, et
// vice-versa.
public abstract class MonthRegularizer : IMonthRegularizer
{
    // exceptionalMonth = mois contenant les jours bissextils.
    protected MonthRegularizer(int monthsInYear, int exceptionalMonth)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(monthsInYear, 0);
        // NB: exceptionalMonth ne peut pas être égal à monthsInYear.
        if (exceptionalMonth < 0 || exceptionalMonth >= monthsInYear)
        {
            throw new ArgumentOutOfRangeException(nameof(monthsInYear));
        }

        MonthsInYear = monthsInYear;
        ExceptionalMonth = exceptionalMonth;
        MonthsAfterExceptionalMonth = monthsInYear - exceptionalMonth;
    }

    public int MonthsInYear { get; }

    public int ExceptionalMonth { get; }

    public int MonthsAfterExceptionalMonth { get; }

    // On décale les mois de sorte que le mois contenant les jours
    // intercalaires soit le dernier de l'année précédente.
    public abstract void Regularize(ref int y, ref int m);

    public abstract void Deregularize(ref int y0, ref int m0);
}
