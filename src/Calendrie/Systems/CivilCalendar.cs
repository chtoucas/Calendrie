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
public sealed class CivilCalendar : CalendarSystem<CivilDate>
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = GJSchema.MonthsInYear;

    // See comments in Armenian13Calendar for instance.
    internal static readonly CivilSchema UnderlyingSchema = new();
    internal static readonly CivilScope UnderlyingScope = new(new CivilSchema());

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilCalendar"/> class.
    /// <para>See also <seealso cref="CivilDate.Calendar"/>.</para>
    /// </summary>
    private CivilCalendar() : this(new CivilScope(new CivilSchema())) { }

    private CivilCalendar(CivilScope scope) : base("Civil", scope)
    {
        Adjuster = new DateAdjuster<CivilDate>(this);
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="CivilCalendar"/> class.
    /// </summary>
    public static CivilCalendar Instance => Singleton.Instance;

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    public static int MinYear => CivilScope.MinYear;

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public static int MaxYear => CivilScope.MaxYear;

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public DateAdjuster<CivilDate> Adjuster { get; }

    private static class Singleton
    {
        static Singleton() { }

        internal static readonly CivilCalendar Instance = new();
    }
}
