// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Core;
using Calendrie.Core.Schemas;

/// <summary>
/// Represents the Gregorian calendar.
/// <para>This calendar is <i>proleptic</i>. It supports <i>all</i> dates within
/// the range [-999_998..999_999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class GregorianCalendar : SpecialCalendar<GregorianDate>, IRegularFeaturette
{
    // See comments in Armenian13Calendar for instance.
    internal static readonly GregorianSchema SchemaT = new();
    internal static readonly GregorianScope ScopeT = new(new GregorianSchema());
    internal static readonly GregorianCalendar Instance = new(new GregorianScope(new GregorianSchema()));

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianCalendar"/> class.
    /// <para>See also <seealso cref="GregorianDate.Calendar"/>.</para>
    /// </summary>
    public GregorianCalendar() : this(new GregorianScope(new GregorianSchema())) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianCalendar"/> class.
    /// </summary>
    internal GregorianCalendar(GregorianScope scope) : base("Gregorian", scope)
    {
        Adjuster = new GregorianAdjuster(this);
    }

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public GregorianAdjuster Adjuster { get; }

    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;
}
