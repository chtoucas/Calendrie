// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Core;
using Calendrie.Core.Schemas;

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class ArmenianCalendar : IRegularFeaturette
{
    /// <inheritdoc />
    public int MonthsInYear => Egyptian12Schema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class Armenian13Calendar : IRegularFeaturette, IVirtualMonthFeaturette
{
    /// <inheritdoc/>
    public int MonthsInYear => Egyptian13Schema.MonthsPerYear;

    /// <inheritdoc/>
    public int VirtualMonth => SchemaT.VirtualMonth;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class CopticCalendar : IRegularFeaturette
{
    /// <inheritdoc/>
    public int MonthsInYear => Coptic12Schema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class Coptic13Calendar : IRegularFeaturette, IVirtualMonthFeaturette
{
    /// <inheritdoc/>
    public int MonthsInYear => Coptic13Schema.MonthsPerYear;

    /// <inheritdoc/>
    public int VirtualMonth => SchemaT.VirtualMonth;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class EthiopicCalendar : IRegularFeaturette
{
    /// <inheritdoc/>
    public int MonthsInYear => Coptic12Schema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class Ethiopic13Calendar : IRegularFeaturette, IVirtualMonthFeaturette
{
    /// <inheritdoc/>
    public int MonthsInYear => Coptic13Schema.MonthsPerYear;

    /// <inheritdoc/>
    public int VirtualMonth => SchemaT.VirtualMonth;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class TabularIslamicCalendar : IRegularFeaturette
{
    /// <inheritdoc/>
    public int MonthsInYear => TabularIslamicSchema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class WorldCalendar : IRegularFeaturette
{
    /// <inheritdoc/>
    public int MonthsInYear => WorldSchema.MonthsPerYear;

    /// <summary>
    /// Obtains the genuine number of days in a month (excluding the blank days
    /// that are formally outside any month).
    /// <para>See also <seealso cref="SpecialCalendar{WorldDate}.CountDaysInMonth(int, int)"/>.
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
public partial class ZoroastrianCalendar : IRegularFeaturette
{
    /// <inheritdoc/>
    public int MonthsInYear => Egyptian12Schema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class Zoroastrian13Calendar : IRegularFeaturette, IVirtualMonthFeaturette
{
    /// <inheritdoc/>
    public int MonthsInYear => Egyptian13Schema.MonthsPerYear;

    /// <inheritdoc/>
    public int VirtualMonth => SchemaT.VirtualMonth;
}
