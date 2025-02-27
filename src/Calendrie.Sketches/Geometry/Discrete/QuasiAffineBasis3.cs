﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Geometry.Discrete;

/// <summary>
/// Formules pour une base de numération quasi-affine d'ordre 3.
/// </summary>
public class QuasiAffineBasis3
{
    protected QuasiAffineBasis3(
        QuasiAffineForm form3,
        QuasiAffineForm form2,
        QuasiAffineForm form1,
        QuasiAffineForm form0)
    {
        ArgumentNullException.ThrowIfNull(form3);
        ArgumentNullException.ThrowIfNull(form2);
        ArgumentNullException.ThrowIfNull(form1);
        ArgumentNullException.ThrowIfNull(form0);

        Form3 = form3;
        Form2 = form2;
        Form1 = form1;
        Form0 = form0;
    }

    public QuasiAffineForm Form3 { get; }

    public QuasiAffineForm Form2 { get; }

    public QuasiAffineForm Form1 { get; }

    public QuasiAffineForm Form0 { get; }

    [Pure]
    public int Recompose(int x3, int x2, int x1, int x0) =>
        Form3.ValueAt(x3) + Form2.ValueAt(x2) + Form1.ValueAt(x1) + Form0.ValueAt(x0);

    public void Decompose(int n, out int x3, out int x2, out int x1, out int x0)
    {
        x3 = Form3.Divide(n, out int r3);
        x2 = Form2.Divide(r3, out int r2);
        x1 = Form2.Divide(r2, out int r1);
        x0 = Form2.Divide(r1, out _);
    }
}
