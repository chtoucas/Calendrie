// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Arithmetic;

/// <summary>
/// Defines the core mathematical operations on dates for schemas with profile
/// <see cref="CalendricalProfile.Solar12"/> or <see cref="CalendricalProfile.Solar13"/>,
/// and provides a base for derived classes.
/// </summary>
internal abstract class SolarArithmetic : CalendricalArithmetic
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="SolarArithmetic"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="segment"/> is
    /// <see langword="null"/>.</exception>
    protected SolarArithmetic(CalendricalSegment segment) : base(segment)
    {
        // Disabled, otherwise we cannot test the derived constructors.
        // Not that important since this class is internal.
        //Debug.Assert(schema.Profile == CalendricalProfile.Solar12
        //    || schema.Profile == CalendricalProfile.Solar13);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override Yemoda AddYears(Yemoda ymd, int years, out int roundoff)
    {
        ymd.Unpack(out int y, out int m, out int d);

        y = checked(y + years);
        YearsValidator.CheckOverflow(y);

        int daysInMonth = Schema.CountDaysInMonth(y, m);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
    }
}
