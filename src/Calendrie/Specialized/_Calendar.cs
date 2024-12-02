// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Core;
using Calendrie.Core.Schemas;

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class ArmenianCalendar : IRegularFeaturette
{
    private static partial StandardScope GetScope(Egyptian12Schema schema) =>
        new(schema, DayZero.Armenian);

    /// <inheritdoc />
    public int MonthsInYear => Egyptian12Schema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class Armenian13Calendar : IRegularFeaturette, IVirtualMonthFeaturette
{
    private static partial StandardScope GetScope(Egyptian13Schema schema) =>
        new(schema, DayZero.Armenian);

    partial void OnInitializing(Egyptian13Schema schema) => VirtualMonth = schema.VirtualMonth;

    /// <inheritdoc/>
    public int MonthsInYear => Egyptian13Schema.MonthsPerYear;

    /// <inheritdoc/>
    public int VirtualMonth { get; private set; }
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class CopticCalendar : IRegularFeaturette
{
    private static partial StandardScope GetScope(Coptic12Schema schema) =>
        new(schema, DayZero.Coptic);

    /// <inheritdoc/>
    public int MonthsInYear => Coptic12Schema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class Coptic13Calendar : IRegularFeaturette, IVirtualMonthFeaturette
{
    private static partial StandardScope GetScope(Coptic13Schema schema) =>
        new(schema, DayZero.Coptic);

    partial void OnInitializing(Coptic13Schema schema) => VirtualMonth = schema.VirtualMonth;

    /// <inheritdoc/>
    public int MonthsInYear => Coptic13Schema.MonthsPerYear;

    /// <inheritdoc/>
    public int VirtualMonth { get; private set; }
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class EthiopicCalendar : IRegularFeaturette
{
    private static partial StandardScope GetScope(Coptic12Schema schema) =>
        new(schema, DayZero.Ethiopic);

    /// <inheritdoc/>
    public int MonthsInYear => Coptic12Schema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class Ethiopic13Calendar : IRegularFeaturette, IVirtualMonthFeaturette
{
    private static partial StandardScope GetScope(Coptic13Schema schema) =>
        new(schema, DayZero.Ethiopic);

    partial void OnInitializing(Coptic13Schema schema) => VirtualMonth = schema.VirtualMonth;

    /// <inheritdoc/>
    public int MonthsInYear => Coptic13Schema.MonthsPerYear;

    /// <inheritdoc/>
    public int VirtualMonth { get; private set; }
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class TabularIslamicCalendar : IRegularFeaturette
{
    private static partial StandardScope GetScope(TabularIslamicSchema schema) =>
        new(schema, DayZero.TabularIslamic);

    /// <inheritdoc/>
    public int MonthsInYear => TabularIslamicSchema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class WorldCalendar : IRegularFeaturette
{
    private static partial StandardScope GetScope(WorldSchema schema) =>
        new(schema, DayZero.SundayBeforeGregorian);

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
    private static partial StandardScope GetScope(Egyptian12Schema schema) =>
        new(schema, DayZero.Zoroastrian);

    /// <inheritdoc/>
    public int MonthsInYear => Egyptian12Schema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class Zoroastrian13Calendar : IRegularFeaturette, IVirtualMonthFeaturette
{
    private static partial StandardScope GetScope(Egyptian13Schema schema) =>
        new(schema, DayZero.Zoroastrian);

    partial void OnInitializing(Egyptian13Schema schema) => VirtualMonth = schema.VirtualMonth;

    /// <inheritdoc/>
    public int MonthsInYear => Egyptian13Schema.MonthsPerYear;

    /// <inheritdoc/>
    public int VirtualMonth { get; private set; }
}
