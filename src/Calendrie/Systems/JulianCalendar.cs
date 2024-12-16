// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

/// <summary>
/// Represents the Julian calendar.
/// <para>This calendar is <i>proleptic</i>. It supports <i>all</i> dates within
/// the range [-999_998..999_999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class JulianCalendar : CalendarSystem<JulianDate>
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = GJSchema.MonthsInYear;

    // See comments in Armenian13Calendar for instance.
    internal static readonly JulianSchema UnderlyingSchema = new();
    internal static readonly JulianScope UnderlyingScope = new(new JulianSchema());

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianCalendar"/>
    /// class.
    /// <para>See also <seealso cref="JulianDate.Calendar"/>.</para>
    /// </summary>
    private JulianCalendar() : this(new JulianScope(new JulianSchema())) { }

    private JulianCalendar(JulianScope scope) : base("Julian", scope)
    {
        Adjuster = new DateAdjuster<JulianDate>(this);
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="JulianCalendar"/> class.
    /// </summary>
    public static JulianCalendar Instance => Singleton.Instance;

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    public static int MinYear => JulianScope.MinYear;

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public static int MaxYear => JulianScope.MaxYear;

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public DateAdjuster<JulianDate> Adjuster { get; }

    private static class Singleton
    {
        static Singleton() { }

        internal static readonly JulianCalendar Instance = new();
    }
}
