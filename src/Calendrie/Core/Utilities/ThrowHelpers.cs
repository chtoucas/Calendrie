// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Utilities;

#region Developer Notes

// About the attribute "DoesNotReturn".
//   "Do not inline methods that never return"
//   https://github.com/dotnet/coreclr/pull/6103
//
// See also
// https://source.dot.net/#System.Memory/System/ThrowHelper.cs
// https://source.dot.net/#System.Private.CoreLib/ThrowHelper.cs
// Microsoft.Toolkit.Diagnostics
// https://docs.microsoft.com/en-us/windows/communitytoolkit/developer-tools/throwhelper
// https://github.com/CommunityToolkit/dotnet/blob/main/src/CommunityToolkit.Diagnostics/ThrowHelper.cs

#endregion

/// <summary>
/// Provides static helpers to throw exceptions.
/// <para>This class cannot be inherited.</para>
/// </summary>
[StackTraceHidden]
internal static partial class ThrowHelpers { }

internal partial class ThrowHelpers // ArgumentOutOfRangeException
{
    /// <summary>
    /// The value of the year was out of range.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [DoesNotReturn]
    public static void ThrowYearOutOfRange(int year, string? paramName = null) =>
        throw new ArgumentOutOfRangeException(
            paramName ?? nameof(year),
            year,
            $"The value of the year was out of range; value = {year}.");

    /// <summary>
    /// The value of the month of the year was out of range.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [DoesNotReturn]
    public static void ThrowMonthOutOfRange(int month, string? paramName = null) =>
        throw new ArgumentOutOfRangeException(
            paramName ?? nameof(month),
            month,
            $"The value of the month of the year was out of range; value = {month}.");

    /// <summary>
    /// The value of the day of the month was out of range.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [DoesNotReturn]
    public static void ThrowDayOutOfRange(int day, string? paramName = null) =>
        throw new ArgumentOutOfRangeException(
            paramName ?? nameof(day),
            day,
            $"The value of the day of the month was out of range; value = {day}.");

    /// <summary>
    /// The value of the day of the year was out of range.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [DoesNotReturn]
    public static void ThrowDayOfYearOutOfRange(int dayOfYear, string? paramName = null) =>
        throw new ArgumentOutOfRangeException(
            paramName ?? nameof(dayOfYear),
            dayOfYear,
            $"The value of the day of the year was out of range; value = {dayOfYear}.");
}

internal partial class ThrowHelpers // ArgumentException
{
    /// <summary>
    /// The binary data is not well-formed.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    [DoesNotReturn]
    public static void ThrowBadBinaryInput() =>
        throw new ArgumentException("The binary data is not well-formed.", "data");

    /// <exception cref="ArgumentException"/>
    [DoesNotReturn, Pure]
    public static int ThrowNonComparable(Type expected, object obj) =>
        throw new ArgumentException(
            $"The object should be of type {expected} but it is of type {obj.GetType()}.",
            nameof(obj));
}

internal partial class ThrowHelpers // OverflowException
{
    /// <summary>
    /// The operation would overflow the range of supported dates.
    /// </summary>
    /// <exception cref="OverflowException"/>
    [DoesNotReturn]
    public static void ThrowDateOverflow() =>
        throw new OverflowException("The computation would overflow the range of supported dates.");

    /// <summary>
    /// The operation would overflow the range of supported months.
    /// </summary>
    /// <exception cref="OverflowException"/>
    [DoesNotReturn]
    public static void ThrowMonthOverflow() =>
        throw new OverflowException("The computation would overflow the range of supported months.");

    /// <summary>
    /// The operation would overflow the range of supported years.
    /// </summary>
    /// <exception cref="OverflowException"/>
    [DoesNotReturn]
    public static void ThrowYearOverflow() =>
        throw new OverflowException("The computation would overflow the range of supported years.");
}
