// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Benchmarks.Micro;

using System.Runtime.CompilerServices;
using System.Security.Cryptography;

using Calendrie.Core.Utilities;

// Tout n'est pas comparable.
// - "Div Mul" et "Div Mod"
// - "/ 31" et "(uint) / 31"

[DisassemblyDiagnoser]
public class DivideBenchmark
{
    private const int Dividor = 31;

    private static readonly int s_Dividend = RandomNumberGenerator.GetInt32(1000, 1_000_000_000);

    #region Division by a constant

    [Benchmark(Description = "/ 31")]
    public int Div()
    {
        int q = s_Dividend / Dividor;
        Consume(in q);
        return q;
    }

    [Benchmark(Description = "(uint) / 31")]
    public int Div_Unsigned()
    {
        int q = (int)(uint)s_Dividend / Dividor;
        Consume(in q);
        return q;
    }

    #endregion
    #region Muliply or Modulo

    [Benchmark(Description = "Div Mul")]
    public int Div_Mul()
    {
        int q = DivMulImpl(s_Dividend, Dividor, out int r);
        Consume(in q);
        return r;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int DivMulImpl(int m, int n, out int r)
    {
        int q = m / n;
        r = m - q * n;
        return q;
    }

    [Benchmark(Description = "Div Mod")]
    public int Div_Mod()
    {
        int q = DivModImpl(s_Dividend, Dividor, out int r);
        Consume(in q);
        return r;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int DivModImpl(int m, int n, out int r)
    {
        r = m % n;
        return m / n;
    }

    #endregion

    [Benchmark(Description = "Math.DivRem", Baseline = true)]
    public int Math_DivRem()
    {
        int q = Math.DivRem(s_Dividend, Dividor, out int r);
        Consume(in q);
        return r;
    }

    [Benchmark(Description = "MathN.Divide")]
    public int MathN_Divide()
    {
        int q = MathN.Divide(s_Dividend, Dividor, out int r);
        Consume(in q);
        return r;
    }

    [Benchmark(Description = "MathU.Divide")]
    public uint MathU_Divide()
    {
        uint q = MathU.Divide((uint)s_Dividend, Dividor, out uint r);
        Consume(in q);
        return r;
    }

    [Benchmark(Description = "MathZ.Divide")]
    public int MathZ_Divide()
    {
        int q = MathZ.Divide(s_Dividend, Dividor, out int r);
        Consume(in q);
        return r;
    }
}
