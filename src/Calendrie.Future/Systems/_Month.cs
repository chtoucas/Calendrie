// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

public partial struct InternationalFixedMonth // Complements
{
    /// <summary>
    /// Represents the genuine number of days in a month (excluding the blank
    /// days that are formally outside any month).
    /// <para>This field is a constant equal to 28.</para>
    /// <para>See also <seealso cref="CountDays()"/>.</para>
    /// </summary>
    public const int DaysPerMonth = InternationalFixedSchema.DaysPerMonth;
}

public partial struct PaxMonth // Complements
{
    /// <summary>
    /// Determines whether the current instance is the Pax month of a year or not.
    /// </summary>
    public bool IsPaxMonthOfYear
    {
        get
        {
            var sch = Calendar.Schema;
            sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
            return sch.IsPaxMonth(y, m);
        }
    }

    /// <summary>
    /// Determines whether the current instance is the last month of the year or
    /// not.
    /// <para>Whether the year is leap or not, the last month of the year is
    /// called December.</para>
    /// </summary>
    public bool IsLastMonthOfYear
    {
        get
        {
            var sch = Calendar.Schema;
            sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
            return sch.IsLastMonthOfYear(y, m);
        }
    }
}

public partial struct PositivistMonth // Complements
{
    /// <summary>
    /// Represents the genuine number of days in a month (excluding the blank
    /// days that are formally outside any month).
    /// <para>This field is a constant equal to 28.</para>
    /// <para>See also <seealso cref="CountDays()"/>.</para>
    /// </summary>
    public const int DaysPerMonth = PositivistSchema.DaysPerMonth;
}

public partial struct WorldMonth // Complements
{
    /// <summary>
    /// Obtains the genuine number of days in this month instance (excluding the
    /// blank days that are formally outside any month).
    /// </summary>
    [Pure]
    public int CountDaysInWorldMonth() => WorldSchema.CountDaysInWorldMonthImpl(Month);
}
