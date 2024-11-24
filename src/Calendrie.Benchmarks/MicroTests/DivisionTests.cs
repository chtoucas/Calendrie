// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.MicroTests;

using System.Runtime.CompilerServices;

using Calendrie.Core.Utilities;

public class DivisionTests
{
    private const int Dividor = 31;
    private const int Dividend = 5_484_955;

    [Benchmark(Description = "MathN.Divide")]
    public int MathN_Divide()
    {
        int q = MathN.Divide(Dividend, Dividor, out int r);
        Consume(in q);
        return r;
    }

    [Benchmark(Description = "MathU.Divide")]
    public uint MathU_Divide()
    {
        uint q = MathU.Divide(Dividend, Dividor, out uint r);
        Consume(in q);
        return r;
    }

    [Benchmark(Description = "MathZ.Divide")]
    public int MathZ_Divide()
    {
        int q = MathZ.Divide(Dividend, Dividor, out int r);
        Consume(in q);
        return r;
    }

    //
    // .NET way of achieving the same thing
    //

    [Benchmark(Description = "Math.DivRem", Baseline = true)]
    public int Math_DivRem()
    {
        int q = Math.DivRem(Dividend, Dividor, out int r);
        Consume(in q);
        return r;
    }

    [Benchmark(Description = "Div Mul")]
    public int Div_Mul()
    {
        int q = impl(Dividend, Dividor, out int r);
        Consume(in q);
        return r;

        [MethodImpl(MethodImplOptions.NoInlining)]
        static int impl(int m, int n, out int r)
        {
            int q = m / n;
            r = m - q * n;
            return q;
        }
    }

    [Benchmark(Description = "Div Mod")]
    public int Div_Mod()
    {
        int q = impl(Dividend, Dividor, out int r);
        Consume(in q);
        return r;

        [MethodImpl(MethodImplOptions.NoInlining)]
        static int impl(int m, int n, out int r)
        {
            r = m % n;
            return m / n;
        }
    }
}
