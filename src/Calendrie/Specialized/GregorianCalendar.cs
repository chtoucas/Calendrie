// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Core;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology.Scopes;

/// <remarks>This calendar is <i>proleptic</i>. It supports <i>all</i> dates
/// within the range [-999_998..999_999] of years.</remarks>
public partial class GregorianCalendar : IRegularFeaturette
{
    private static partial CalendarScope GetScope(GregorianSchema schema) =>
        MinMaxYearScope.CreateMaximal(schema, DayZero.NewStyle);

    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;
}
