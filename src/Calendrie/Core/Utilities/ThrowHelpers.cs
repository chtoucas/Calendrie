// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Utilities;

// TODO(code): do not use factories. Localized messages?

#region Developer Notes

// The main problem with ThrowHelper is that the code coverage can't see if
// the tests covered the two branches (with and without exception thrown).
//
// Even if it always throws, a method returning something should be
// decorated with the attribute Pure. This way, we get a warning if, for
// instance, we write
// > ThrowHelpers.ArgumentNull<int>("paramName");
// when we should have written
// > ThrowHelpers.ArgumentNull(paramName");
// This does not apply to the exception factory methods, as they are always
// preceded by a "throw" (one can not throw void...).
//
// About the attribute "DoesNotReturn".
//   "Do not inline methods that never return"
//   https://github.com/dotnet/coreclr/pull/6103
//
// Extracts from the BCL
// ---------------------
//
// https://source.dot.net/#System.Memory/System/ThrowHelper.cs
// This pattern of easily inlinable "void Throw" routines that stack on top of NoInlining factory methods
// is a compromise between older JITs and newer JITs (RyuJIT in .NET Core 1.1.0+ and .NET Framework in 4.6.3+).
// This package is explicitly targeted at older JITs as newer runtimes expect to implement Span intrinsically for
// best performance.
//
// The aim of this pattern is three-fold
// 1. Extracting the throw makes the method preforming the throw in a conditional branch smaller and more inlinable
// 2. Extracting the throw from generic method to non-generic method reduces the repeated codegen size for value types
// 3a. Newer JITs will not inline the methods that only throw and also recognise them, move the call to cold section
//     and not add stack prep and unwind before calling https://github.com/dotnet/coreclr/pull/6103
// 3b. Older JITs will inline the throw itself and move to cold section; but not inline the non-inlinable exception
//     factory methods - still maintaining advantages 1 & 2
//// This file defines an internal static class used to throw exceptions in BCL code.
// The main purpose is to reduce code size.
//
// https://source.dot.net/#System.Private.CoreLib/ThrowHelper.cs
// The old way to throw an exception generates quite a lot IL code and assembly code.
// Following is an example:
//     C# source
//          throw new ArgumentNullException(nameof(key), SR.ArgumentNull_Key);
//     IL code:
//          IL_0003:  ldstr      "key"
//          IL_0008:  ldstr      "ArgumentNull_Key"
//          IL_000d:  call       string System.Environment::GetResourceString(string)
//          IL_0012:  newobj     instance void System.ArgumentNullException::.ctor(string,string)
//          IL_0017:  throw
//    which is 21bytes in IL.
//
// So we want to get rid of the ldstr and call to Environment.GetResource in IL.
// In order to do that, I created two enums: ExceptionResource, ExceptionArgument to represent the
// argument name and resource name in a small integer. The source code will be changed to
//    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key, ExceptionResource.ArgumentNull_Key);
//
// The IL code will be 7 bytes.
//    IL_0008:  ldc.i4.4
//    IL_0009:  ldc.i4.4
//    IL_000a:  call       void System.ThrowHelper::ThrowArgumentNullException(valuetype System.ExceptionArgument)
//    IL_000f:  ldarg.0
//
// This will also reduce the Jitted code size a lot.
//
// Microsoft.Toolkit.Diagnostics
// -----------------------------
//
// https://docs.microsoft.com/en-us/windows/communitytoolkit/developer-tools/throwhelper
// https://github.com/CommunityToolkit/dotnet/blob/main/src/CommunityToolkit.Diagnostics/ThrowHelper.cs

#endregion

/// <summary>
/// Provides static helpers to throw exceptions.
/// </summary>
/// <remarks>This class cannot be inherited.</remarks>
[StackTraceHidden]
internal static partial class ThrowHelpers { }

internal partial class ThrowHelpers
{
    /// <summary>
    /// The control flow path reached a section of the code that should have
    /// been unreachable under any circumstances.
    /// </summary>
    public const string UnreachableMessage =
        "The control flow path reached a section of the code that should have been unreachable under any circumstances.";
}

internal partial class ThrowHelpers // ArgumentOutOfRangeException
{
    /// <summary>
    /// The value of the year was out of range.
    /// </summary>
    /// <exception cref="AoorException"/>
    [DoesNotReturn]
    public static void YearOutOfRange(long year) => throw GetYearOutOfRangeExn(null, year);

    /// <summary>
    /// The value of the year was out of range.
    /// </summary>
    /// <exception cref="AoorException"/>
    [DoesNotReturn]
    public static void YearOutOfRange(int year) => throw GetYearOutOfRangeExn(null, year);

    /// <summary>
    /// The value of the year was out of range.
    /// </summary>
    /// <exception cref="AoorException"/>
    [DoesNotReturn]
    public static void YearOutOfRange(int year, string? paramName) => throw GetYearOutOfRangeExn(paramName, year);

    /// <summary>
    /// The value of the month of the year was out of range.
    /// </summary>
    /// <exception cref="AoorException"/>
    [DoesNotReturn]
    public static void MonthOutOfRange(int month) => throw GetMonthOutOfRangeExn(null, month);

    /// <summary>
    /// The value of the month of the year was out of range.
    /// </summary>
    /// <exception cref="AoorException"/>
    [DoesNotReturn]
    public static void MonthOutOfRange(int month, string? paramName) =>
        throw GetMonthOutOfRangeExn(paramName, month);

    /// <summary>
    /// The value of the day of the month was out of range.
    /// </summary>
    /// <exception cref="AoorException"/>
    [DoesNotReturn]
    public static void DayOutOfRange(int day) => throw GetDayOutOfRangeExn(null, day);

    /// <summary>
    /// The value of the day of the month was out of range.
    /// </summary>
    /// <exception cref="AoorException"/>
    [DoesNotReturn]
    public static void DayOutOfRange(int day, string? paramName) => throw GetDayOutOfRangeExn(paramName, day);

    /// <summary>
    /// The value of the day of the year was out of range.
    /// </summary>
    /// <exception cref="AoorException"/>
    [DoesNotReturn]
    public static void DayOfYearOutOfRange(int dayOfYear) => throw GetDayOfYearOutOfRangeExn(null, dayOfYear);

    /// <summary>
    /// The value of the day of the year was out of range.
    /// </summary>
    /// <exception cref="AoorException"/>
    [DoesNotReturn]
    public static void DayOfYearOutOfRange(int dayOfYear, string? paramName) =>
        throw GetDayOfYearOutOfRangeExn(paramName, dayOfYear);

    /// <summary>
    /// The value of the day of the week was out of range.
    /// </summary>
    /// <exception cref="AoorException"/>
    [DoesNotReturn]
    public static void DayOfWeekOutOfRange(DayOfWeek dayOfWeek, string? paramName) =>
        throw GetDayOfWeekOutOfRangeExn(paramName, dayOfWeek);

    /// <summary>
    /// The value of the ISO weekday was out of range.
    /// </summary>
    /// <exception cref="AoorException"/>
    [DoesNotReturn]
    public static void IsoWeekdayOutOfRange(IsoWeekday weekday, string? paramName) =>
        throw GetIsoWeekdayOutOfRangeExn(paramName, weekday);

    /// <summary>
    /// The value of the ISO weekday was out of range.
    /// </summary>
    /// <exception cref="AoorException"/>
    [DoesNotReturn]
    public static void AdditionRuleOutOfRange(AdditionRule rule, string? paramName) =>
        throw GetAdditionRuleOutOfRangeExn(paramName, rule);

    #region Factories

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static AoorException GetYearOutOfRangeExn(string? paramName, long year) =>
         new(paramName ?? nameof(year),
             year,
             $"The value of the year was out of range; value = {year}.");

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static AoorException GetYearOutOfRangeExn(string? paramName, int year) =>
         new(paramName ?? nameof(year),
             year,
             $"The value of the year was out of range; value = {year}.");

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static AoorException GetMonthOutOfRangeExn(string? paramName, int month) =>
         new(paramName ?? nameof(month),
             month,
             $"The value of the month of the year was out of range; value = {month}.");

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static AoorException GetDayOutOfRangeExn(string? paramName, int day) =>
         new(paramName ?? nameof(day),
             day,
             $"The value of the day of the month was out of range; value = {day}.");

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static AoorException GetDayOfYearOutOfRangeExn(string? paramName, int dayOfYear) =>
         new(paramName ?? nameof(dayOfYear),
             dayOfYear,
             $"The value of the day of the year was out of range; value = {dayOfYear}.");

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static AoorException GetDayOfWeekOutOfRangeExn(string? paramName, DayOfWeek dayOfWeek) =>
         new(paramName ?? nameof(dayOfWeek),
             dayOfWeek,
             $"The value of the day of the week must be in the range 0 through 6; value = {dayOfWeek}.");

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static AoorException GetIsoWeekdayOutOfRangeExn(string? paramName, IsoWeekday weekday) =>
         new(paramName ?? nameof(weekday),
             weekday,
             $"The value of the ISO weekday must be in the range 0 through 6; value = {weekday}.");

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static AoorException GetAdditionRuleOutOfRangeExn(string? paramName, AdditionRule rule) =>
         new(paramName ?? nameof(rule),
             rule,
             $"The value of the addition rule must be in the range 0 through 3; value = {rule}.");

    #endregion
}

internal partial class ThrowHelpers // ArgumentException
{
    /// <summary>
    /// The binary data is not well-formed.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    [DoesNotReturn]
    public static void BadBinaryInput() =>
        throw new ArgumentException("The binary data is not well-formed.", "data");

    /// <exception cref="ArgumentException"/>
    [DoesNotReturn]
    public static int NonComparable(Type expected, object obj) =>
        throw new ArgumentException(
            $"The object should be of type {expected} but it is of type {obj.GetType()}.",
            nameof(obj));

    /// <exception cref="ArgumentException"/>
    [DoesNotReturn]
    public static void BadSchemaProfile(string paramName, CalendricalProfile expected, CalendricalProfile actual) =>
        throw new ArgumentException(
            $"The schema profile should be equal to \"{expected}\" but it is equal to \"{actual}\".",
            paramName);
}

internal partial class ThrowHelpers // OverflowException
{
    /// <summary>
    /// The operation would overflow the range of supported months.
    /// </summary>
    /// <exception cref="OverflowException"/>
    [DoesNotReturn]
    public static void MonthOverflow() =>
        throw new OverflowException("The computation would overflow the range of supported months.");

    /// <summary>
    /// The operation would overflow the range of supported dates.
    /// </summary>
    /// <exception cref="OverflowException"/>
    [DoesNotReturn]
    public static void DateOverflow() =>
        throw new OverflowException("The computation would overflow the range of supported dates.");

    /// <summary>
    /// The operation would overflow the range of supported dates.
    /// </summary>
    /// <exception cref="OverflowException"/>
    [DoesNotReturn, Pure]
    public static T DateOverflow<T>() =>
        throw new OverflowException("The computation would overflow the range of supported dates.");

    /// <summary>
    /// The operation would overflow the range of supported day numbers.
    /// </summary>
    /// <exception cref="OverflowException"/>
    [DoesNotReturn]
    public static void DayNumberOverflow() =>
        throw new OverflowException("The computation would overflow the range of supported day numbers.");

    /// <summary>
    /// The operation would overflow the range of supported day numbers.
    /// </summary>
    /// <exception cref="OverflowException"/>
    [DoesNotReturn, Pure]
    public static T DayNumberOverflow<T>() =>
        throw new OverflowException("The computation would overflow the range of supported day numbers.");

    /// <summary>
    /// The operation would overflow the range of supported ordinal numerals.
    /// </summary>
    /// <exception cref="OverflowException"/>
    [DoesNotReturn, Pure]
    public static T OrdOverflow<T>() =>
        throw new OverflowException("The computation would overflow the range of supported ordinal numerals.");
}
