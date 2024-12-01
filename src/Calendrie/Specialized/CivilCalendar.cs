// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Core;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class CivilCalendar : IRegularFeaturette
{
    private static partial CalendarScope GetScope(CivilSchema schema) => new CivilScope(schema);

    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;
}
