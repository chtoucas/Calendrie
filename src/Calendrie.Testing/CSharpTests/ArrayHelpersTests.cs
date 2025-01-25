// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.CSharpTests;

using Calendrie.Core.Utilities;

[SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "<Pending>")]
public class ArrayHelpersTests
{
    [Fact]
    public static void ConvertToCumulativeArray()
    {
        var array = new[] { 365, 365, 365, 366 };
        var exp = new[] { 0, 365, 2 * 365, 3 * 365, 4 * 365 + 1 };
        // Act
        var actual = ArrayHelpers.ConvertToCumulativeArray(array);
        // Assert
        Assert.Equal(exp, actual);
    }
}
