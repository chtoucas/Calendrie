// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Benchmarks;

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;

// dotnet run -c Release -f net9.0 --filter "*" --runtimes net9.0

[DisassemblyDiagnoser(maxDepth: 0)]
//[MemoryDiagnoser(displayGenColumns: false)]
internal static class Program
{
    public static void Main(string[] args)
    {
        _ = BenchmarkSwitcher
            .FromAssembly(typeof(Program).Assembly)
            .Run(args, DefaultConfig.Instance.WithLocalSettings());
    }

    public static IConfig WithLocalSettings(this IConfig config)
    {
        var orderer = new DefaultOrderer(
            SummaryOrderPolicy.FastestToSlowest,
            MethodOrderPolicy.Alphabetical);

        return config
            .AddValidator(ExecutionValidator.FailOnError)
            .AddColumn(RankColumn.Roman)
            .AddColumn(BaselineRatioColumn.RatioMean)
            .WithOrderer(orderer);
    }
}
