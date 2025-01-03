// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

// For non-regular calendars, add the following method:
#if false
/// <summary>
/// Obtains the number of months in the specified year.
/// </summary>
/// <exception cref="ArgumentOutOfRangeException">The year is outside the
/// range of supported years.</exception>
[Pure]
public int CountMonthsInYear(int year)
{
    YearsValidator.Validate(year);
    return Schema.CountMonthsInYear(year);
}
#endif

public partial class ArmenianCalendar // Complements
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = Egyptian12Schema.MonthsInYear;
}

public partial class Armenian13Calendar // Complements
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

public partial class CopticCalendar // Complements
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = Coptic12Schema.MonthsInYear;
}

public partial class Coptic13Calendar // Complements
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

public partial class EthiopicCalendar // Complements
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = Coptic12Schema.MonthsInYear;
}

public partial class Ethiopic13Calendar // Complements
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

public partial class TabularIslamicCalendar // Complements
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = TabularIslamicSchema.MonthsInYear;
}

public partial class WorldCalendar // Complements
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

public partial class ZoroastrianCalendar // Complements
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = Egyptian12Schema.MonthsInYear;
}

public partial class Zoroastrian13Calendar // Complements
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
