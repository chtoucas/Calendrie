// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks;

using System.Runtime.CompilerServices;

using NodaTime;

/// <summary>
/// Provides static helpers to prevent from dead code elimination.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static class BenchmarkHelpers
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in bool _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in uint _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in int _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in long _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in DayOfWeek _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in IsoWeekday _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in IsoDayOfWeek _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in Yemoda _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Consume(in Yemo _) { }
}
