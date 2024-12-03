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
    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianCalendar"/>
    /// class.
    /// </summary>
    public GregorianCalendar() : this(new GregorianSchema()) { }

    internal GregorianCalendar(GregorianSchema schema) :
        base("Gregorian", new GregorianScope(schema))
    { }

    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;

    [Pure]
    private protected sealed override GregorianDate NewDate(int daysSinceZero) => new(daysSinceZero);
}
