// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

/// <summary>
/// Represents the Civil calendar.
/// <para>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class CivilCalendar : SpecialCalendar<CivilDate>
{
    // See comments in Armenian13Calendar for instance.
    internal static readonly CivilSchema SchemaT = new();
    internal static readonly CivilScope ScopeT = new(new CivilSchema());
    internal static readonly CivilCalendar Instance = new(new CivilScope(new CivilSchema()));

    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = GJSchema.MonthsInYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilCalendar"/> class.
    /// <para>See also <seealso cref="CivilDate.Calendar"/>.</para>
    /// </summary>
    public CivilCalendar() : this(new CivilScope(new CivilSchema())) { }

    private CivilCalendar(CivilScope scope) : base("Civil", scope)
    {
        Adjuster = new SpecialAdjuster<CivilDate>(this);
    }

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public SpecialAdjuster<CivilDate> Adjuster { get; }
}
