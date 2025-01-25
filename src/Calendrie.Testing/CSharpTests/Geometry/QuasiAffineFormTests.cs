// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.CSharpTests.Geometry;

using Calendrie.Geometry.Discrete;
using Calendrie.Testing;

[SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "<Pending>")]
public class QuasiAffineFormTests
{
    [Fact]
    public static void Constructor_InvalidA() =>
        AssertEx.ThrowsAoorexn("A", () => new QuasiAffineForm(0, 1, 1));

    [Fact]
    public static void Constructor_InvalidB() =>
        AssertEx.ThrowsAoorexn("B", () => new QuasiAffineForm(1, 0, 1));

    [Fact]
    public static void Deconstructor()
    {
        var form = new QuasiAffineForm(3, 4, 5);
        // Act
        var (a, b, r) = form;
        // Assert
        Assert.Equal(3, a);
        Assert.Equal(4, b);
        Assert.Equal(5, r);
    }

    [Fact]
    public static void Init_InvalidA() =>
        AssertEx.ThrowsAoorexn("value", () => new QuasiAffineForm(1, 1, 1) with { A = 0 });

    [Fact]
    public static void Init_InvalidB() =>
        AssertEx.ThrowsAoorexn("value", () => new QuasiAffineForm(1, 1, 1) with { B = 0 });
}
