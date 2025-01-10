﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;

public partial class Armenian13Calendar // Complements
{
    /// <summary>
    /// Represents the virtual month.
    /// <para>This field is constant equal to 13.</para>
    /// </summary>
    public const int VirtualMonth = Egyptian13Schema.VirtualMonth;
}

public partial class Coptic13Calendar // Complements
{
    /// <summary>
    /// Represents the virtual month.
    /// <para>This field is constant equal to 13.</para>
    /// </summary>
    public const int VirtualMonth = Coptic13Schema.VirtualMonth;
}

public partial class Ethiopic13Calendar // Complements
{
    /// <summary>
    /// Represents the virtual month.
    /// <para>This field is constant equal to 13.</para>
    /// </summary>
    public const int VirtualMonth = Coptic13Schema.VirtualMonth;
}

public partial class WorldCalendar // Complements
{
    /// <summary>
    /// Obtains the genuine number of days in a month (excluding the blank days
    /// that are formally outside any month).
    /// </summary>
    [Pure]
    public static int CountDaysInWorldMonth(int year, int month)
    {
        // The calendar being regular, no need to use the Scope:
        // > Scope.ValidateYearMonth(year, month);
        if (year < StandardScope.MinYear || year > StandardScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year);
        if (month < 1 || month > MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(month);

        return WorldSchema.CountDaysInWorldMonthImpl(month);
    }
}

public partial class Zoroastrian13Calendar // Complements
{
    /// <summary>
    /// Represents the virtual month.
    /// <para>This field is constant equal to 13.</para>
    /// </summary>
    public const int VirtualMonth = Egyptian13Schema.VirtualMonth;
}
