// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Core;
using Calendrie.Core.Schemas;

/// <summary>
/// Represents the Civil calendar.
/// <para>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class CivilCalendar : SpecialCalendar<CivilDate>, IRegularFeaturette
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CivilCalendar"/> class.
    /// </summary>
    public CivilCalendar() : this(new CivilSchema()) { }

    internal CivilCalendar(CivilSchema schema) : base("Civil", new CivilScope(schema)) { }

    [Pure]
    private protected sealed override CivilDate GetDate(int daysSinceZero) => new(daysSinceZero);

    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;
}
