// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Core;
using Calendrie.Core.Schemas;

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class PlainGregorianCalendar : IRegularFeaturette
{
    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class PlainJulianCalendar : IRegularFeaturette
{
    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;
}
