// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie;

using System.ComponentModel;
using System.Globalization;
using System.Numerics;

using Calendrie.Core.Utilities;

// As with DayNumber, one can initialize an Ord as follows
// > ord = Ord.Zeroth + i
// where "i" is the algebraic value of the Ord, except that using "i" is
// error-prone here. Better to use FromRank().

/// <summary>
/// Represents a 32-bit "signed" ordinal numeral.
/// <para><see cref="Ord"/> is an immutable struct.</para>
/// </summary>
[DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
public readonly partial struct Ord :
    // Comparison
    IEqualityOperators<Ord, Ord, bool>,
    IEquatable<Ord>,
    IComparisonOperators<Ord, Ord, bool>,
    IComparable<Ord>,
    IComparable,
    IMinMaxValue<Ord>,
    // Arithmetic
    IAdditionOperators<Ord, int, Ord>,
    ISubtractionOperators<Ord, int, Ord>,
    ISubtractionOperators<Ord, Ord, int>,
    IIncrementOperators<Ord>,
    IDecrementOperators<Ord>,
    IUnaryNegationOperators<Ord, Ord>
{
    /// <summary>
    /// Represents the smallest possible algebraic value.
    /// <para>This field is a constant equal to -2_147_483_646.</para>
    /// </summary>
    public const int MinAlgebraicValue = int.MinValue + 2;

    /// <summary>
    /// Represents the largest possible algebraic value.
    /// <para>This field is a constant equal to 2_147_483_647.</para>
    /// </summary>
    public const int MaxAlgebraicValue = int.MaxValue;

    /// <summary>
    /// Represents the algebraic value of the current instance.
    /// <para>This field is in the range from <see cref="MinAlgebraicValue"/> to
    /// <see cref="MaxAlgebraicValue"/>.</para>
    /// </summary>
    private readonly int _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Ord"/> struct from the
    /// specified <i>algebraic</i> value.
    /// <para>This constructor does NOT validate its parameter.</para>
    /// </summary>
    private Ord(int value)
    {
        Debug.Assert(value >= MinAlgebraicValue);

        _value = value;
    }

    /// <summary>
    /// Gets the ordinal numeral zeroth.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Ord Zeroth { get; }

    /// <summary>
    /// Gets the ordinal numeral first.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Ord First { get; } = new(1);

    /// <summary>
    /// Gets the minimum value of an <see cref="Ord"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Ord MinValue { get; } = new(MinAlgebraicValue);

    /// <summary>
    /// Gets the maximum value of an <see cref="Ord"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Ord MaxValue { get; } = new(MaxAlgebraicValue);

    /// <summary>
    /// Gets the (signed) rank of the current instance.
    /// <para>The result is never equal to zero and is in the range from
    /// -<see cref="int.MaxValue"/> to <see cref="int.MaxValue"/>.</para>
    /// </summary>
    public int Rank => _value > 0 ? _value : _value - 1;

    /// <summary>
    /// Gets the string to display in the debugger watch window.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private string DebuggerDisplay => _value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts the current instance to its equivalent string representation
    /// using the formatting conventions of the current culture.
    /// </summary>
    public override string ToString() => Rank.ToString(CultureInfo.CurrentCulture);

    /// <summary>
    /// Deconstructs the current instance into its components: its position, an
    /// unsigned rank, and a boolean.
    /// </summary>
    public void Deconstruct(out int pos, out bool afterZeroth)
    {
        if (_value > 0)
        {
            pos = _value;
            afterZeroth = true;
        }
        else
        {
            pos = 1 - _value;
            afterZeroth = false;
        }
    }

    /// <summary>
    /// Converts the current instance to a 32-bit signed integer.
    /// <para>The result is in the range from <see cref="MinAlgebraicValue"/> to
    /// <see cref="MaxAlgebraicValue"/>.</para>
    /// </summary>
    public static explicit operator int(Ord ord) => ord._value;
}

public partial struct Ord // Factories, conversions
{
    /// <summary>
    /// Creates a new instance of the <see cref="Ord"/> struct from the specified
    /// signed rank.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="rank"/> is
    /// equal to zero or <see cref="int.MinValue"/>.</exception>
    public static Ord FromRank(int rank)
    {
        if (rank == 0 || rank == int.MinValue)
            ThrowHelpers.ThrowRankOutOfRange(rank);

        // The next operation never overflows. It is equivalent to:
        //   rank > 0 ? Zeroth + rank : First + rank;
        return new Ord(rank > 0 ? rank : 1 + rank);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Ord"/> struct from the specified
    /// algebraic value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/>
    /// is lower than <see cref="MinAlgebraicValue"/>.</exception>
    public static Ord FromInt32(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, MinAlgebraicValue);

        return new Ord(value);
    }

    /// <summary>
    /// Converts the current instance to its equivalent algebraic value, a 32-bit
    /// signed integer.
    /// <para>The result is in the range from <see cref="MinAlgebraicValue"/> to
    /// <see cref="MaxAlgebraicValue"/>.</para>
    /// </summary>
    public int ToInt32() => _value;
}

public partial struct Ord // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(Ord left, Ord right) => left._value == right._value;

    /// <inheritdoc />
    public static bool operator !=(Ord left, Ord right) => left._value != right._value;

    /// <inheritdoc />
    public bool Equals(Ord other) => _value == other._value;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Ord ord && Equals(ord);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _value;
}

public partial struct Ord // IComparable
{
    /// <inheritdoc />
    public static bool operator <(Ord left, Ord right) => left._value < right._value;

    /// <inheritdoc />
    public static bool operator <=(Ord left, Ord right) => left._value <= right._value;

    /// <inheritdoc />
    public static bool operator >(Ord left, Ord right) => left._value > right._value;

    /// <inheritdoc />
    public static bool operator >=(Ord left, Ord right) => left._value >= right._value;

    /// <summary>
    /// Obtains the minimum of two specified values.
    /// </summary>
    [Pure]
    public static Ord Min(Ord left, Ord right) => left < right ? left : right;

    /// <summary>
    /// Obtains the maximum of two specified values.
    /// </summary>
    [Pure]
    public static Ord Max(Ord left, Ord right) => left > right ? left : right;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(Ord other) => _value.CompareTo(other._value);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is Ord ord ? CompareTo(ord)
        : ThrowHelpers.ThrowNonComparable(typeof(Ord), obj);
}

public partial struct Ord // Math ops
{
    /// <summary>
    /// Subtracts the two specified ordinal numerals.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    public static int operator -(Ord left, Ord right) => checked(left._value - right._value);

    /// <summary>
    /// Adds an integer to a specified ordinal numeral, yielding a new ordinal
    /// numeral.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    public static Ord operator +(Ord ord, int num)
    {
        int newVal = checked(ord._value + num);
        if (newVal < MinAlgebraicValue) ThrowHelpers.ThrowOrdOverflow();
        return new Ord(newVal);
    }

    /// <summary>
    /// Subtracts an integer to a specified ordinal numeral, yielding a new
    /// ordinal numeral.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    public static Ord operator -(Ord ord, int num)
    {
        int newVal = checked(ord._value - num);
        if (newVal < MinAlgebraicValue) ThrowHelpers.ThrowOrdOverflow();
        return new Ord(newVal);
    }

    /// <summary>
    /// Increments by 1 the value of the specified ordinal numeral.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    public static Ord operator ++(Ord ord) => ord.Increment();

    /// <summary>
    /// Decrements by 1 the value of the specified ordinal numeral.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    public static Ord operator --(Ord ord) => ord.Decrement();

    /// <summary>
    /// Negates the current instance.
    /// </summary>
    public static Ord operator -(Ord ord) => ord.Negate();

    /// <summary>
    /// Subtracts the specified ordinal numeral from the current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public int Subtract(Ord other) => this - other;

    /// <summary>
    /// Adds an integer to the value of the current instance, yielding a new
    /// ordinal numeral.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public Ord Add(int num) => this + num;

    /// <summary>
    /// Increments the value of the current instance by 1.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public Ord Increment()
    {
        if (this == MaxValue) ThrowHelpers.ThrowOrdOverflow();
        return new Ord(_value + 1);
    }

    /// <summary>
    /// Decrements the value of the current instance by 1.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public Ord Decrement()
    {
        if (this == MinValue) ThrowHelpers.ThrowOrdOverflow();
        return new Ord(_value - 1);
    }

    /// <summary>
    /// Negates the current instance.
    /// </summary>
    [Pure]
    // No need to use checked arithmetic, the op always succeeds.
    public Ord Negate() => new(1 - _value);
}
