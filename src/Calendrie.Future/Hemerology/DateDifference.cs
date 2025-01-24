// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

using Calendrie.Core.Utilities;

using static Calendrie.Core.CalendricalConstants;

// FIXME(code): comparison shouldn't be lexicographic but be on the "length",
// idem with MonthDifference.

/// <summary>
/// Represents the result of <see cref="DateMath.Subtract{TDate}(TDate, TDate)"/>,
/// that is the exact difference between two dates.
/// <para><see cref="DateDifference"/> is an immutable struct.</para>
/// </summary>
public readonly record struct DateDifference :
    // Comparison
    IEqualityOperators<DateDifference, DateDifference, bool>,
    IEquatable<DateDifference>,
    IComparisonOperators<DateDifference, DateDifference, bool>,
    IComparable<DateDifference>,
    IComparable,
    // Arithmetic
    IUnaryPlusOperators<DateDifference, DateDifference>,
    IUnaryNegationOperators<DateDifference, DateDifference>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DateDifference"/> struct.
    /// </summary>
    internal DateDifference(int years, int months, int weeks, int days)
    {
        Years = years;
        Months = months;
        Weeks = weeks;
        Days = days;
    }

    /// <summary>
    /// Gets the zero difference.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static DateDifference Zero { get; }

    /// <summary>
    /// Gets the number of years.
    /// </summary>
    public int Years { get; }

    /// <summary>
    /// Gets the number of months.
    /// </summary>
    public int Months { get; }

    /// <summary>
    /// Gets the number of weeks.
    /// </summary>
    public int Weeks { get; }

    /// <summary>
    /// Gets the number of days.
    /// </summary>
    public int Days { get; }

    /// <summary>
    /// Deconstructs the current instance into its components.
    /// </summary>
    public void Deconstruct(out int years, out int months, out int weeks, out int days) =>
        (years, months, weeks, days) = (Years, Months, Weeks, Days);

    /// <summary>
    /// Creates a new instance of the <see cref="DateDifference"/> struct.
    /// </summary>
    internal static DateDifference Create(int years, int months, int days)
    {
        // NB: pour une fois, on utilise la division euclidienne avec reste
        // négatif, d'où Math.DivRem() au lieu de MathZ.Divide().
        int weeks = Math.DivRem(days, DaysInWeek, out days);

        return new DateDifference(years, months, weeks, days);
    }

    /// <inheritdoc />
    public static bool operator <(DateDifference left, DateDifference right) =>
        left.CompareTo(right) < 0;

    /// <inheritdoc />
    public static bool operator <=(DateDifference left, DateDifference right) =>
        left.CompareTo(right) <= 0;

    /// <inheritdoc />
    public static bool operator >(DateDifference left, DateDifference right) =>
        left.CompareTo(right) > 0;

    /// <inheritdoc />
    public static bool operator >=(DateDifference left, DateDifference right) =>
        left.CompareTo(right) >= 0;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(DateDifference other)
    {
        int c = Years.CompareTo(other.Years);
        if (c == 0)
        {
            c = Months.CompareTo(other.Months);
            if (c == 0)
            {
                c = Weeks.CompareTo(other.Weeks);
                if (c == 0)
                {
                    c = Days.CompareTo(other.Days);
                }
            }
        }
        return c;
    }

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is DateDifference diff ? CompareTo(diff)
        : ThrowHelpers.ThrowNonComparable(typeof(DateDifference), obj);

    /// <inheritdoc />
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Meaningless here")]
    public static DateDifference operator +(DateDifference value) => value;

    /// <inheritdoc />
    public static DateDifference operator -(DateDifference value) => value.Negate();

    /// <summary>
    /// Negates the current instance.
    /// </summary>
    public DateDifference Negate() => new(-Years, -Months, -Weeks, -Days);
}
