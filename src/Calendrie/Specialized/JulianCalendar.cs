// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Core;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

/// <summary>
/// Represents the Julian calendar.
/// <para>This calendar is <i>proleptic</i>. It supports <i>all</i> dates within
/// the range [-999_998..999_999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class JulianCalendar : SpecialCalendar<JulianDate>, IRegularFeaturette
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JulianCalendar"/>
    /// class.
    /// </summary>
    public JulianCalendar() : this(new JulianSchema()) { }

    // TODO(code): use a custom proleptic scope.
    internal JulianCalendar(JulianSchema schema) :
        base("Julian", MinMaxYearScope.CreateMaximal(schema, DayZero.OldStyle))
    { }

    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;

    [Pure]
    private protected sealed override JulianDate NewDate(int daysSinceEpoch) => new(daysSinceEpoch);
}
