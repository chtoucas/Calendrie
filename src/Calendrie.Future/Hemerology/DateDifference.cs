// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

using Calendrie.Core.Utilities;

using static Calendrie.Core.CalendricalConstants;

// FIXME(code): comparison shouldn't be lexicographic but be on the "absolute"*
// values, idem with MonthDifference. Terminer Create().
// Check that all params are >= 0 or <= 0, not mixed.

/// <summary>
/// Represents the result of <see cref="DateMath.Subtract{TDate}(TDate, TDate)"/>,
/// that is the exact difference between two dates.
/// <para><see cref="DateDifference"/> is an immutable struct.</para>
/// </summary>
public sealed partial record DateDifference :
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
    private DateDifference(int years, int months, int days, int sign)
    {
        // NB: une fois n'est pas coutume, on utilise la division euclidienne
        // à reste négatif, d'où Math.DivRem() au lieu de MathZ.Divide().
        int weeks = Math.DivRem(days, DaysInWeek, out days);

        Years = years;
        Months = months;
        Weeks = weeks;
        Days = days;
        Sign = sign;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DateDifference"/> struct.
    /// </summary>
    private DateDifference(int years, int months, int weeks, int days, int sign)
    {
        Years = years;
        Months = months;
        Weeks = weeks;
        Days = days;
        Sign = sign;
    }

    /// <summary>
    /// Gets the zero difference.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static DateDifference Zero { get; } = new(0, 0, 0, 0, sign: 0);

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
    /// Gets the common sign shared by <see cref="Years"/>, <see cref="Months"/>,
    /// <see cref="Weeks"/> and <see cref="Days"/>.
    /// <para>Returns +1 if positive, -1 if negative; otherwise returns 0.</para>
    /// </summary>
    public int Sign { get; }

    /// <summary>
    /// Deconstructs the current instance into its components.
    /// </summary>
    public void Deconstruct(out int years, out int months, out int weeks, out int days) =>
        (years, months, weeks, days) = (Years, Months, Weeks, Days);
}

public partial record DateDifference // Factories
{
    /// <summary>
    /// Creates a new instance of the <see cref="DateDifference"/> struct.
    /// </summary>
    internal static DateDifference UnsafeCreate(int years, int months, int days, int sign) =>
        new(years, months, days, sign);

    /// <summary>
    /// Creates a new instance of the <see cref="DateDifference"/> struct.
    /// </summary>
    /// <exception cref="ArgumentException">All the parameters are equal to zero.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">One of the parameters is
    /// less than zero.</exception>
    public static DateDifference CreatePositive(int years, int months, int days)
    {
        if (years == 0 && months == 0 && days == 0)
            throw new ArgumentException("All the parameters were equal to zero.");
        ArgumentOutOfRangeException.ThrowIfLessThan(years, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(months, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(days, 0);

        return new DateDifference(years, months, days, 1);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="DateDifference"/> struct.
    /// </summary>
    /// <exception cref="ArgumentException">All the parameters are equal to zero.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">One of the parameters is
    /// less than zero.</exception>
    public static DateDifference CreateNegative(int years, int months, int days)
    {
        if (years == 0 && months == 0 && days == 0)
            throw new ArgumentException("All the parameters were equal to zero.");
        ArgumentOutOfRangeException.ThrowIfLessThan(years, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(months, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(days, 0);

        return new DateDifference(-years, -months, -days, -1);
    }
}

public partial record DateDifference // IComparable
{
    /// <inheritdoc />
    public static bool operator <(DateDifference? left, DateDifference? right) =>
        Compare(left, right) < 0;

    /// <inheritdoc />
    public static bool operator <=(DateDifference? left, DateDifference? right) =>
        Compare(left, right) <= 0;

    /// <inheritdoc />
    public static bool operator >(DateDifference? left, DateDifference? right) =>
        Compare(left, right) > 0;

    /// <inheritdoc />
    public static bool operator >=(DateDifference? left, DateDifference? right) =>
        Compare(left, right) >= 0;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(DateDifference? other) => Compare(this, other);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is DateDifference other ? Compare(this, other)
        : ThrowHelpers.ThrowNonComparable(typeof(DateDifference), obj);

    [Pure]
    private static int Compare(DateDifference? left, DateDifference? right)
    {
        // "By definition, any object compares greater than null, and two null
        // references compare equal to each other."
        // https://learn.microsoft.com/en-us/dotnet/api/system.icomparable-1.compareto?view=net-9.0#remarks

        if (ReferenceEquals(left, right)) return 0;
        if (left is null) return -1;
        if (right is null) return 1;

        // We compare the "absolute" values!
        var x = left.Sign > 0 ? left : -left;
        var y = right.Sign > 0 ? right : -right;

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
}

public partial record DateDifference // Math
{
    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Meaningless here")]
    public static DateDifference operator +(DateDifference? value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.
    /// </exception>
    public static DateDifference operator -(DateDifference? value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.Negate();
    }

    /// <summary>
    /// Negates the current instance.
    /// </summary>
    public DateDifference Negate() => new(-Years, -Months, -Weeks, -Days, -Sign);
}
