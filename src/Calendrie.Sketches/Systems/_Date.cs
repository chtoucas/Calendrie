// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;

public partial struct Armenian12Date // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber) =>
        Egyptian12Schema.IsEpagomenalDayImpl(Day, out epagomenalNumber);
}

public partial struct Coptic12Date // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber) =>
        Coptic12Schema.IsEpagomenalDayImpl(Day, out epagomenalNumber);
}

public partial struct Egyptian12Date // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber) =>
        Egyptian12Schema.IsEpagomenalDayImpl(Day, out epagomenalNumber);

    public DayOfDecan DayOfDecan
    {
        get
        {
            int day = Day;
            return day > EgyptianSchema.DaysPerMonth ? DayOfDecan.Blank
                : (DayOfDecan)MathN.AdjustedModulo(day, 10);
        }
    }

    public int DecanOfMonth
    {
        get
        {
            int day = Day;
            return day > EgyptianSchema.DaysPerMonth ? 0 : MathN.AdjustedDivide(day, 10);
        }
    }

    public int DecanOfYear
    {
        get
        {
            int doy = DayOfYear;
            return doy > 360 ? 0 : MathN.AdjustedDivide(doy, 10);
        }
    }
}

public partial struct Ethiopic12Date // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber) =>
        Coptic12Schema.IsEpagomenalDayImpl(Day, out epagomenalNumber);
}

public partial struct FrenchRepublican12Date // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber) =>
        FrenchRepublican12Schema.IsEpagomenalDayImpl(Day, out epagomenalNumber);
}

public partial struct Zoroastrian12Date // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber) =>
        Egyptian12Schema.IsEpagomenalDayImpl(Day, out epagomenalNumber);
}
