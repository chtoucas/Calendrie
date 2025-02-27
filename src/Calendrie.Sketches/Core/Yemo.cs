﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using System.ComponentModel;
using System.Numerics;

using Calendrie.Core.Utilities;

// Yemo = YEar-MOnth
//
// We could have re-used Yemoda but I find it cleaner to have a separate
// struct (no Day prop for instance). Still, we make sure that Yemo is
// binary compatible with Yemoda.

/// <summary>
/// Provides a compact representation of a year and a month.
/// <para><see cref="Yemo"/> is an immutable struct.</para>
/// </summary>
/// <remarks>
/// <para>This type uses the lexicographic order on pairs (Year, Month).</para>
/// <para>A <see cref="Yemo"/> value is NOT a calendar month since it is not attached to any
/// calendar. As such, comparison between two instances is NOT calendrical. Still, it uses an
/// order that most of the time matches the calendrical one.</para>
/// </remarks>
[DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
public readonly partial struct Yemo :
#if ENABLE_SERIALIZATION
    ISerializable<Yemo, int>,
#endif
    IEqualityOperators<Yemo, Yemo, bool>,
    IEquatable<Yemo>,
    IComparisonOperators<Yemo, Yemo, bool>,
    IComparable<Yemo>,
    IComparable
{
    /// <summary>
    /// Represents the absolute minimum value for <see cref="Year"/>.
    /// <para>This field is a constant equal to -2_097_151.</para>
    /// </summary>
    public const int MinYear = Yemoda.MinYear;

    /// <summary>
    /// Represents the absolute maximum value for <see cref="Year"/>.
    /// <para>This field is a constant equal to 2_097_152.</para>
    /// </summary>
    public const int MaxYear = Yemoda.MaxYear;

    /// <summary>
    /// Represents the absolute minimum value for <see cref="Month"/>.
    /// <para>This field is a constant equal to 1.</para>
    /// </summary>
    public const int MinMonth = Yemoda.MinMonth;

    /// <summary>
    /// Represents the absolute maximum value for <see cref="Month"/>.
    /// <para>This field is a constant equal to 16.</para>
    /// </summary>
    public const int MaxMonth = Yemoda.MaxMonth;

    // Do NOT make MinValue & MaxValue public, it would give the impression
    // that all values in between are legal, which is not the case by far.
    // The last 6 bits of _bin are ALWAYS equal to 0.

    /// <summary>
    /// Represents the smallest possible value of a <see cref="Yemo"/>.
    /// </summary>
    internal static readonly Yemo MinValue = new(MinYear, MinMonth);

    /// <summary>
    /// Gets the largest possible value of a <see cref="Yemo"/>.
    /// </summary>
    internal static readonly Yemo MaxValue = new(MaxYear, MaxMonth);

    /// <summary>
    /// Represents the binary data stored in this instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The data is organised as follows:
    /// <code><![CDATA[
    ///   Year - 1  bbbb bbbb bbbb bbbb bbbb bb
    ///   Month - 1                            bb bb
    ///   (Day - 1)                                  00 0000
    /// ]]></code>
    /// </para>
    /// </remarks>
    private readonly int _bin;

    /// <summary>
    /// Initializes a new instance of the <see cref="Yemo"/> struct from the specified year and
    /// month.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal Yemo(int y, int m)
    {
        Debug.Assert(MinYear <= y);
        Debug.Assert(y <= MaxYear);
        Debug.Assert(MinMonth <= m);
        Debug.Assert(m <= MaxMonth);

        _bin = Pack(y, m);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Yemo"/> struct directly from the specified
    /// binary data.
    /// <para>This constructor does NOT validate its parameter.</para>
    /// </summary>
    internal Yemo(int bin)
    {
        __CheckBinaryData(bin);

        _bin = bin;
    }

    /// <summary>
    /// Gets the algebraic year from this instance.
    /// </summary>
    public int Year => unchecked(1 + (_bin >> Yemoda.YearShift));

    /// <summary>
    /// Gets the month of the year from this instance.
    /// </summary>
    public int Month => unchecked(1 + ((_bin >> Yemoda.MonthShift) & Yemoda.MonthMask));

    /// <summary>
    /// Gets the date parts for the first day of the year to which belongs this instance.
    /// </summary>
    public Yemoda StartOfYear => new(unchecked(_bin & Yemoda.UnsetMonthDayMask));

    /// <summary>
    /// Gets the date parts for the first day of the month to which belongs this instance.
    /// </summary>
    public Yemoda StartOfMonth => new(_bin);

    /// <summary>
    /// Gets the string to display in the debugger watch window.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private string DebuggerDisplay => Convert.ToString(_bin, 2);

    /// <summary>
    /// Returns a culture-independent string representation of the current instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        Unpack(out int y, out int m);
        return FormattableString.Invariant($"{m:D2}/{y:D4}");
    }

    /// <summary>
    /// Deconstructs the current instance into its components.
    /// </summary>
    public void Deconstruct(out int year, out int month) => Unpack(out year, out month);

    /// <summary>
    /// Creates a new instance of <see cref="Yemo"/> from the specified year and month.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified pair is not
    /// representable; one of the value is too large to be handled by the system.
    /// </exception>
    [Pure]
    internal static Yemo Create(int year, int month)
    {
        if (year < MinYear || year > MaxYear) ThrowHelpers.ThrowYearOutOfRange(year);
        if (month < MinMonth || month > MaxMonth) ThrowHelpers.ThrowMonthOutOfRange(month);

        return new Yemo(Pack(year, month));
    }

    /// <summary>
    /// Creates a new instance of <see cref="Yemo"/> representing the first month of the
    /// specified year.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Yemo AtStartOfYear(int y)
    {
        Debug.Assert(MinYear <= y);
        Debug.Assert(y <= MaxYear);

        return new Yemo(unchecked((y - 1) << Yemoda.YearShift));
    }

    /// <summary>
    /// Obtains the date parts for the specified day of the month to which belongs this instance.
    /// <para>See also <seealso cref="StartOfMonth"/>.</para>
    /// </summary>
    [Pure]
    public Yemoda GetDayOfMonth(int day)
    {
        if (day < Yemoda.MinDay || day > Yemoda.MaxDay) ThrowHelpers.ThrowDayOutOfRange(day);

        return new Yemoda(unchecked(_bin | (day - 1)));
    }

    /// <summary>
    /// Obtains the date parts for the specified day of the month to which belongs this instance.
    /// <para>This method does NOT validate its parameter.</para>
    /// <para>See also <seealso cref="StartOfMonth"/>.</para>
    /// </summary>
    [Pure]
    internal Yemoda GetDayOfMonthUnsafe(int d)
    {
        Debug.Assert(Yemoda.MinDay <= d);
        Debug.Assert(d <= Yemoda.MaxDay);

        return new Yemoda(unchecked(_bin | (d - 1)));
    }
}

public partial struct Yemo // Binary data helpers
{
#if ENABLE_SERIALIZATION

    /// <summary>
    /// Deserializes a 32-bit binary value and recreates an original serialized
    /// <see cref="Yemo"/> object.
    /// </summary>
    /// <exception cref="ArgumentException">The specified binary data is not well-formed.
    /// </exception>
    [Pure]
    public static Yemo FromBinary(int data)
    {
        ValidateBinaryData(data);
        return new(data);
    }

    /// <summary>
    /// Obtains the 32-bit binary value stored in the current instance.
    /// </summary>
    [Pure]
    public int ToBinary() => _bin;

#endif

    /// <summary>
    /// Packs the specified month parts into a single 32-bit word.
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Pack(int y, int m)
    {
        unchecked
        {
            return ((y - 1) << Yemoda.YearShift) | ((m - 1) << Yemoda.MonthShift);
        }
    }

    /// <summary>
    /// Unpacks the binary data.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Unpack(out int y, out int m)
    {
        int bin = _bin;

        unchecked
        {
            y = 1 + (bin >> Yemoda.YearShift);
            m = 1 + ((bin >> Yemoda.MonthShift) & Yemoda.MonthMask);
        }
    }

    /// <summary>
    /// Validates the specified binary data.
    /// </summary>
    /// <exception cref="ArgumentException">The specified binary data is not well-formed.
    /// </exception>
#if !ENABLE_SERIALIZATION
    // Only called by __CheckBinaryData().
    [Conditional("DEBUG")]
#endif
    private static void ValidateBinaryData(int data)
    {
        int d0 = data & Yemoda.DayMask;
        if (d0 != 0) ThrowHelpers.ThrowBadBinaryInput();
    }

    [Conditional("DEBUG")]
    private static void __CheckBinaryData(int bin) => ValidateBinaryData(bin);
}

public partial struct Yemo // IEquatable
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="Yemo"/> are equal.
    /// </summary>
    public static bool operator ==(Yemo left, Yemo right) => left._bin == right._bin;

    /// <summary>
    /// Determines whether two specified instances of <see cref="Yemo"/> are not equal.
    /// </summary>
    public static bool operator !=(Yemo left, Yemo right) => left._bin != right._bin;

    /// <inheritdoc />
    [Pure]
    public bool Equals(Yemo other) => _bin == other._bin;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Yemo parts && Equals(parts);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _bin;
}

public partial struct Yemo // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly earlier than the
    /// right one.
    /// </summary>
    /// <remarks>The comparison is done using the lexicographic order on pairs (Year, Month).
    /// </remarks>
    public static bool operator <(Yemo left, Yemo right) => left._bin < right._bin;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier than or equal to
    /// the right one.
    /// </summary>
    /// <remarks>The comparison is done using the lexicographic order on pairs (Year, Month).
    /// </remarks>
    public static bool operator <=(Yemo left, Yemo right) => left._bin <= right._bin;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly later than the
    /// right one.
    /// </summary>
    /// <remarks>The comparison is done using the lexicographic order on pairs (Year, Month).
    /// </remarks>
    public static bool operator >(Yemo left, Yemo right) => left._bin > right._bin;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than or equal to
    /// the right one.
    /// </summary>
    /// <remarks>The comparison is done using the lexicographic order on pairs (Year, Month).
    /// </remarks>
    public static bool operator >=(Yemo left, Yemo right) => left._bin >= right._bin;

    /// <inheritdoc />
    /// <remarks>The comparison is done using the lexicographic order on pairs (Year, Month).
    /// </remarks>
    [Pure]
    public int CompareTo(Yemo other) => _bin.CompareTo(other._bin);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is Yemo ym ? CompareTo(ym)
        : ThrowHelpers.ThrowNonComparable(typeof(Yemo), obj);
}
