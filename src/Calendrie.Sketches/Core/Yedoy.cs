﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using System.ComponentModel;
using System.Numerics;

using Calendrie.Core.Utilities;

// Yedoy = YEar-DayOfYear

/// <summary>
/// Provides a compact representation of a year and a day of the year.
/// <para><see cref="Yedoy"/> is an immutable struct.</para>
/// </summary>
/// <remarks>
/// <para>This type uses the lexicographic order on pairs (Year, DayOfYear).</para>
/// <para>A <see cref="Yedoy"/> value is NOT an ordinal date since it is not attached to any
/// calendar. As such, comparison between two instances is NOT calendrical. Still, it uses an
/// order that most of the time matches the calendrical one.</para>
/// </remarks>
[DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
public readonly partial struct Yedoy :
#if ENABLE_SERIALIZATION
    ISerializable<Yedoy, int>,
#endif
    IEqualityOperators<Yedoy, Yedoy, bool>,
    IEquatable<Yedoy>,
    IComparisonOperators<Yedoy, Yedoy, bool>,
    IComparable<Yedoy>,
    IComparable,
    IMinMaxValue<Yedoy>
{
    #region Bit settings

    /// <summary>
    /// <see cref="DayOfYear"/> (minus 1) is an 10-bit unsigned integer.
    /// <para>This field is a constant equal to 10.</para>
    /// </summary>
    internal const int DayOfYearBits = 32 - Yemoda.YearBits;

    /// <summary>This field is a constant equal to 10.</summary>
    private const int YearShift = DayOfYearBits;

    /// <summary>This field is a constant equal to 1023.</summary>
    internal const int DayOfYearMask = (1 << DayOfYearBits) - 1;

    /// <summary>This field is a constant equal to -1024.</summary>
    private const int UnsetDayOfYearMask = ~((1 << YearShift) - 1);

    #endregion

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
    /// Represents the absolute minimum value for <see cref="DayOfYear"/>.
    /// <para>This field is a constant equal to 1.</para>
    /// </summary>
    public const int MinDayOfYear = 1;

    // Notice that MaxMonth * MaxDay (cf. Yemoda) = MaxDayOfYear.

    /// <summary>
    /// Represents the absolute maximum value for <see cref="DayOfYear"/>.
    /// <para>This field is a constant equal to 1024 (= 2^10).</para>
    /// </summary>
    public const int MaxDayOfYear = 1 << DayOfYearBits;

    /// <summary>
    /// Represents the binary data stored in this instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The data is organised as follows:
    /// <code><![CDATA[
    ///   Year - 1      bbbb bbbb bbbb bbbb bbbb bb
    ///   DayOfYear - 1                            bb bbbb bbbb
    /// ]]></code>
    /// </para>
    /// </remarks>
    private readonly int _bin;

    /// <summary>
    /// Initializes a new instance of the <see cref="Yedoy"/> struct from the specified year and
    /// day of the year.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal Yedoy(int y, int doy)
    {
        Debug.Assert(MinYear <= y);
        Debug.Assert(y <= MaxYear);
        Debug.Assert(MinDayOfYear <= doy);
        Debug.Assert(doy <= MaxDayOfYear);

        _bin = Pack(y, doy);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Yedoy"/> struct directly from the specified
    /// binary data.
    /// <para>This constructor does NOT validate its parameter.</para>
    /// </summary>
    internal Yedoy(int bin)
    {
        _bin = bin;
    }

    /// <summary>
    /// Gets the smallest possible value of a <see cref="Yedoy"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Yedoy MinValue { get; } = new(MinYear, MinDayOfYear);

    /// <summary>
    /// Gets the largest possible value of a <see cref="Yedoy"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Yedoy MaxValue { get; } = new(MaxYear, MaxDayOfYear);

    /// <summary>
    /// Gets the year.
    /// </summary>
    public int Year => unchecked(1 + (_bin >> YearShift));

    /// <summary>
    /// Gets the day of the year.
    /// </summary>
    public int DayOfYear => unchecked(1 + (_bin & DayOfYearMask));

    /// <summary>
    /// Gets the ordinal parts for the first day of the year to which belongs this instance.
    /// </summary>
    public Yedoy StartOfYear => new(unchecked(_bin & UnsetDayOfYearMask));

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
        Unpack(out int y, out int doy);
        return FormattableString.Invariant($"{doy:D3}/{y:D4}");
    }

    /// <summary>
    /// Deconstructs the current instance into its components.
    /// </summary>
    public void Deconstruct(out int year, out int dayOfYear) => Unpack(out year, out dayOfYear);

    /// <summary>
    /// Creates a new instance of <see cref="Yedoy"/> from the specified year and day of the
    /// year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified pair is not
    /// representable; one of the value is too large to be handled by the system.
    /// </exception>
    [Pure]
    internal static Yedoy Create(int year, int dayOfYear)
    {
        if (year < MinYear || year > MaxYear)
        {
            ThrowHelpers.ThrowYearOutOfRange(year);
        }
        if (dayOfYear < MinDayOfYear || dayOfYear > MaxDayOfYear)
        {
            ThrowHelpers.ThrowDayOfYearOutOfRange(dayOfYear);
        }

        return new Yedoy(Pack(year, dayOfYear));
    }

    /// <summary>
    /// Creates a new instance of <see cref="Yedoy"/> representing the first day of the
    /// specified year.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Yedoy AtStartOfYear(int y)
    {
        Debug.Assert(MinYear <= y);
        Debug.Assert(y <= MaxYear);

        return new Yedoy(unchecked((y - 1) << YearShift));
    }
}

public partial struct Yedoy // Binary data helpers
{
#if ENABLE_SERIALIZATION

    /// <summary>
    /// Deserializes a 32-bit binary value and recreates an original serialized
    /// <see cref="Yedoy"/> object.
    /// </summary>
    [Pure]
    public static Yedoy FromBinary(int data) => new(data);

    /// <summary>
    /// Obtains the 32-bit binary value stored in the current instance.
    /// </summary>
    [Pure]
    public int ToBinary() => _bin;

#else

    /// <summary>
    /// Creates a new instance of the <see cref="Yedoy"/> struct from the
    /// specified 32-bit integer.
    /// </summary>
    [Pure]
    internal static Yedoy FromInt32(int data) => new(data);

#endif

    /// <summary>
    /// Packs the specified ordinal date parts into a single 32-bit word.
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Pack(int y, int doy)
    {
        unchecked
        {
            return ((y - 1) << YearShift) | (doy - 1);
        }
    }

    /// <summary>
    /// Unpacks the binary data.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Unpack(out int y, out int doy)
    {
        int bin = _bin;

        unchecked
        {
            y = 1 + (bin >> YearShift);
            doy = 1 + (bin & DayOfYearMask);
        }
    }
}

public partial struct Yedoy // IEquatable
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="Yedoy"/> are equal.
    /// </summary>
    public static bool operator ==(Yedoy left, Yedoy right) => left._bin == right._bin;

    /// <summary>
    /// Determines whether two specified instances of <see cref="Yedoy"/> are not equal.
    /// </summary>
    public static bool operator !=(Yedoy left, Yedoy right) => left._bin != right._bin;

    /// <inheritdoc />
    [Pure]
    public bool Equals(Yedoy other) => _bin == other._bin;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Yedoy parts && Equals(parts);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _bin;
}

public partial struct Yedoy // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly earlier than the
    /// right one.
    /// </summary>
    /// <remarks>The comparison is done using the lexicographic order on pairs (Year, DayOfYear).
    /// </remarks>
    public static bool operator <(Yedoy left, Yedoy right) => left._bin < right._bin;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier than or equal to
    /// the right one.
    /// </summary>
    /// <remarks>The comparison is done using the lexicographic order on pairs (Year, DayOfYear).
    /// </remarks>
    public static bool operator <=(Yedoy left, Yedoy right) => left._bin <= right._bin;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly later than the
    /// right one.
    /// </summary>
    /// <remarks>The comparison is done using the lexicographic order on pairs (Year, DayOfYear).
    /// </remarks>
    public static bool operator >(Yedoy left, Yedoy right) => left._bin > right._bin;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than or equal to
    /// the right one.
    /// </summary>
    /// <remarks>The comparison is done using the lexicographic order on pairs (Year, DayOfYear).
    /// </remarks>
    public static bool operator >=(Yedoy left, Yedoy right) => left._bin >= right._bin;

    /// <inheritdoc />
    /// <remarks>The comparison is done using the lexicographic order on pairs (Year, DayOfYear).
    /// </remarks>
    [Pure]
    public int CompareTo(Yedoy other) => _bin.CompareTo(other._bin);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is Yedoy ydoy ? CompareTo(ydoy)
        : ThrowHelpers.ThrowNonComparable(typeof(Yedoy), obj);
}
