// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

using Calendrie.Core.Utilities;

using static Calendrie.Core.CalendricalConstants;

// The result is ALWAYS in its "minimal" form which makes comparison possible.
// For instance, with the Gregorian calendar, it means that
// - Months < 12
// - Weeks < 5
// - Days < 31
// Notice however that comparison between two values only makes sense when both
// are produced by the same calendar and rule.

/// <summary>
/// Represents the result of <see cref="DateMath.Subtract{TDate}(TDate, TDate)"/>,
/// that is the exact difference between two dates.
/// <para><see cref="DateDifference"/> is an immutable struct.</para>
/// </summary>
public readonly partial record struct DateDifference :
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
    private DateDifference(int years, int months, int days)
    {
        // NB: une fois n'est pas coutume, on utilise la division euclidienne
        // à reste négatif, d'où Math.DivRem() au lieu de MathZ.Divide().

        Years = years;
        Months = months;
        Weeks = Math.DivRem(days, DaysInWeek, out days);
        Days = days;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DateDifference"/> struct.
    /// </summary>
    private DateDifference(int years, int months, int weeks, int days)
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
    public static DateDifference Zero { get; } = new(0, 0, 0, 0);

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
}

public partial record struct DateDifference // Factories
{
    /// <summary>
    /// Creates a new instance of the <see cref="DateDifference"/> struct.
    /// </summary>
    internal static DateDifference UnsafeCreate(int years, int months, int days) =>
        new(years, months, days);

    /// <summary>
    /// Creates a new instance of the <see cref="DateDifference"/> struct.
    /// </summary>
    /// <exception cref="ArgumentException">All the parameters are equal to zero.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">One of the parameters is
    /// less than zero.</exception>
    internal static DateDifference CreatePositive(int years, int months, int days)
    {
        if (years == 0 && months == 0 && days == 0)
            throw new ArgumentException("All the parameters were equal to zero.");
        ArgumentOutOfRangeException.ThrowIfLessThan(years, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(months, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(days, 0);

        return new DateDifference(years, months, days);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="DateDifference"/> struct.
    /// </summary>
    /// <exception cref="ArgumentException">All the parameters are equal to zero.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">One of the parameters is
    /// less than zero.</exception>
    internal static DateDifference CreateNegative(int years, int months, int days)
    {
        if (years == 0 && months == 0 && days == 0)
            throw new ArgumentException("All the parameters were equal to zero.");
        ArgumentOutOfRangeException.ThrowIfLessThan(years, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(months, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(days, 0);

        return new DateDifference(-years, -months, -days);
    }
}

public partial record struct DateDifference // IComparable
{
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
        // We compare the "absolute" values!
        var x = Abs(this);
        var y = Abs(other);

        int c = x.Years.CompareTo(y.Years);
        if (c == 0)
        {
            c = x.Months.CompareTo(y.Months);
            if (c == 0)
            {
                c = x.Weeks.CompareTo(y.Weeks);
                if (c == 0)
                {
                    c = x.Days.CompareTo(y.Days);
                }
            }
        }
        return c;
    }

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is DateDifference other ? CompareTo(other)
        : ThrowHelpers.ThrowNonComparable(typeof(DateDifference), obj);
}

public partial record struct DateDifference // Math
{
    /// <summary>
    /// Computes the absolute value of the specified <see cref="DateDifference"/>
    /// value.
    /// </summary>
    public static DateDifference Abs(DateDifference value) => IsPositive(value) ? value : -value;

    /// <summary>
    /// Determines whether the specified value is equal to <see cref="Zero"/> or
    /// not.
    /// </summary>
    public static bool IsZero(DateDifference value) => value == Zero;

    // NB: Years, Months, Weeks and Days have the same sign.

    /// <summary>
    /// Determines whether the specified <see cref="DateDifference"/> value
    /// is greater than or equal to <see cref="Zero"/>.
    /// </summary>
    public static bool IsPositive(DateDifference value) => value.Years >= 0;

    /// <summary>
    /// Determines whether the specified <see cref="DateDifference"/> value
    /// is less than or equal to <see cref="Zero"/>.
    /// </summary>
    public static bool IsNegative(DateDifference value) => value.Years <= 0;

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Meaningless here")]
    public static DateDifference operator +(DateDifference value) => value;

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.
    /// </exception>
    public static DateDifference operator -(DateDifference value) => value.Negate();

    /// <summary>
    /// Negates the current instance.
    /// </summary>
    public DateDifference Negate() => new(-Years, -Months, -Weeks, -Days);
}
