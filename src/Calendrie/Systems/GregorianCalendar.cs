// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

/// <summary>
/// Represents the Gregorian calendar.
/// <para>This calendar is <i>proleptic</i>. It supports <i>all</i> dates within
/// the range [-999_998..999_999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class GregorianCalendar : CalendarSystem<GregorianDate>
{
    /// <summary>
    /// Represents the earliest supported year.
    /// <para>This field is a constant equal to -999_998.</para>
    /// </summary>
    public const int MinYear = ProlepticScope.MinYear;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 9999.</para>
    /// </summary>
    public const int MaxYear = ProlepticScope.MaxYear;

    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = GJSchema.MonthsInYear;

    // See comments in Armenian13Calendar for instance.
    internal static readonly GregorianSchema UnderlyingSchema = new();
    internal static readonly GregorianScope UnderlyingScope = new(new GregorianSchema());
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
        Adjuster = new DateAdjuster<GregorianDate>(this);
    }

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public DateAdjuster<GregorianDate> Adjuster { get; }
}
