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
public sealed class CivilCalendar : SpecialCalendar<CivilDate>, IRegularFeaturette
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CivilCalendar"/> class.
    /// <para>See also <seealso cref="CivilDate.Calendar"/>.</para>
    /// </summary>
    public CivilCalendar() : this(new CivilSchema()) { }

    internal CivilCalendar(CivilSchema schema) : base("Civil", new CivilScope(schema))
    {
        Adjuster = new CivilAdjuster(Scope);
    }

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public CivilAdjuster Adjuster { get; }

    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;

    [Pure]
    private protected sealed override CivilDate NewDate(int daysSinceZero) => new(daysSinceZero);
}
