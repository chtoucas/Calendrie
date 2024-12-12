// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class ArmenianCalendar
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = Egyptian12Schema.MonthsInYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class Armenian13Calendar
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 13.</para>
    /// </summary>
    public const int MonthsInYear = Egyptian13Schema.MonthsInYear;

    /// <summary>
    /// Represents the virtual month.
    /// <para>This field is constant equal to 13.</para>
    /// </summary>
    public const int VirtualMonth = Egyptian13Schema.VirtualMonth;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class CopticCalendar
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = Coptic12Schema.MonthsInYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class Coptic13Calendar
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 13.</para>
    /// </summary>
    public const int MonthsInYear = Coptic13Schema.MonthsInYear;

    /// <summary>
    /// Represents the virtual month.
    /// <para>This field is constant equal to 13.</para>
    /// </summary>
    public const int VirtualMonth = Coptic13Schema.VirtualMonth;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class EthiopicCalendar
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = Coptic12Schema.MonthsInYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class Ethiopic13Calendar
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 13.</para>
    /// </summary>
    public const int MonthsInYear = Coptic13Schema.MonthsInYear;

    /// <summary>
    /// Represents the virtual month.
    /// <para>This field is constant equal to 13.</para>
    /// </summary>
    public const int VirtualMonth = Coptic13Schema.VirtualMonth;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class TabularIslamicCalendar
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = TabularIslamicSchema.MonthsInYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class WorldCalendar
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = WorldSchema.MonthsInYear;

    /// <summary>
    /// Obtains the genuine number of days in a month (excluding the blank days
    /// that are formally outside any month).
    /// <para>See also <seealso cref="CalendarSystem{WorldDate}.CountDaysInMonth(int, int)"/>.
    /// </para>
    /// </summary>
    [Pure]
    public int CountDaysInWorldMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return WorldSchema.CountDaysInWorldMonth(month);
    }
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class ZoroastrianCalendar
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = Egyptian12Schema.MonthsInYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class Zoroastrian13Calendar
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 13.</para>
    /// </summary>
    public const int MonthsInYear = Egyptian13Schema.MonthsInYear;

    /// <summary>
    /// Represents the virtual month.
    /// <para>This field is constant equal to 13.</para>
    /// </summary>
    public const int VirtualMonth = Egyptian13Schema.VirtualMonth;
}
