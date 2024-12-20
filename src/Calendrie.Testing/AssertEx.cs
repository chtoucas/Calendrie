﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix (Naming)

namespace Calendrie.Testing;

using Calendrie.Core.Intervals;

/// <summary>
/// Xunit Assert extended.
/// </summary>
public sealed partial class AssertEx : Assert
{
    private AssertEx() { }
}

public partial class AssertEx
{
    /// <summary>
    /// Fails with a user message.
    /// </summary>
    public static void Fails(string userMessage) => True(false, userMessage);

    public static void Equal<T>(T expected, T actual, string userMessage)
        where T : IEquatable<T>
    {
        True(actual.Equals(expected), userMessage);
    }

    public static void Overflows(Action testCode) => Throws<OverflowException>(testCode);

    public static void Overflows(Func<object?> testCode) => Throws<OverflowException>(testCode);
}

public partial class AssertEx // Arg exceptions
{
    /// <summary>
    /// Verifies that an exception is exactly the given exception type (and not
    /// a derived one), then that its message is not null.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="exn"/> is
    /// <see langword="null"/>.</exception>
    public static void CheckException(Type expectedExceptionType, Exception exn)
    {
        ArgumentNullException.ThrowIfNull(exn);

        IsType(expectedExceptionType, exn);
        NotNull(exn.Message);
    }

    /// <summary>
    /// Verifies that an exception message is not null, then that the name of the
    /// parameter that causes the exception is equal to <paramref name="expectedParamName"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="exn"/> is
    /// <see langword="null"/>.</exception>
    public static void CheckArgumentException(string expectedParamName, ArgumentException exn)
    {
        ArgumentNullException.ThrowIfNull(exn);

        NotNull(exn.Message);
        Equal(expectedParamName, exn.ParamName);
    }

    // Below, use "argName" instead of "paramName" to avoid an error CA1507 in
    // the caller code.

    /// <summary>
    /// Verifies that the specified delegate throws an exception of type
    /// <see cref="ArgumentNullException"/> (and not a derived exception type).
    /// </summary>
    public static void ThrowsAnexn(string argName, Action testCode) =>
        Throws<ArgumentNullException>(argName, testCode);

    /// <summary>
    /// Verifies that the specified delegate throws an exception of type
    /// <see cref="ArgumentNullException"/> (and not a derived exception type).
    /// </summary>
    public static void ThrowsAnexn(string argName, Func<object?> testCode) =>
        Throws<ArgumentNullException>(argName, testCode);

    /// <summary>
    /// Verifies that the specified delegate throws an exception of type
    /// <see cref="ArgumentOutOfRangeException"/> (and not a derived exception
    /// type).
    /// </summary>
    public static void ThrowsAoorexn(string argName, Action testCode) =>
        Throws<ArgumentOutOfRangeException>(argName, testCode);

    /// <summary>
    /// Verifies that the specified delegate throws an exception of type
    /// <see cref="ArgumentOutOfRangeException"/> (and not a derived exception
    /// type).
    /// </summary>
    public static void ThrowsAoorexn(string argName, Func<object?> testCode) =>
        Throws<ArgumentOutOfRangeException>(argName, testCode);
}

public partial class AssertEx // ISetEquatable
{
    public static void SetEqual<T, TOther>(TOther expected, T actual) where T : ISetEquatable<TOther>
    {
        True(actual.SetEquals(expected));
    }
}
