// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Core;
using Calendrie.Core.Schemas;

/// <summary>
/// Represents the Julian calendar.
/// <para>This calendar is <i>proleptic</i>. It supports <i>all</i> dates within
/// the range [-999_998..999_999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class JulianCalendar : SpecialCalendar<JulianDate>, IRegularFeaturette
{
    // See comments in Armenian13Calendar for instance.
    internal static readonly JulianSchema SchemaT = new();
    internal static readonly JulianScope ScopeT = new(SchemaT);
    internal static readonly JulianCalendar Instance = new(ScopeT);

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianCalendar"/>
    /// class.
    /// <para>See also <seealso cref="JulianDate.Calendar"/>.</para>
    /// </summary>
    public JulianCalendar() : this(new JulianScope(new JulianSchema())) { }

    internal JulianCalendar(JulianScope scope) : base("Julian", scope)
    {
        Adjuster = new JulianAdjuster(this);
    }

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public JulianAdjuster Adjuster { get; }

    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;

    [Pure]
    private protected sealed override JulianDate NewDate(int daysSinceEpoch) => new(daysSinceEpoch);
}
