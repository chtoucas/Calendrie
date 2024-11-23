// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks;

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;

[DisassemblyDiagnoser(maxDepth: 0)]
internal static class Program
{
    public static void Main(string[] args)
    {
        var config = GetConfig();

        _ = BenchmarkSwitcher
            .FromAssembly(typeof(Program).Assembly)
            .Run(args, config.WithLocalSettings());
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

    public static IConfig GetConfig()
    {
        var defaultConfig = DefaultConfig.Instance;

        var config = new ManualConfig()
            .AddAnalyser(defaultConfig.GetAnalysers().ToArray())
            .AddColumnProvider(defaultConfig.GetColumnProviders().ToArray())
            .AddDiagnoser(defaultConfig.GetDiagnosers().ToArray())
            .AddExporter(MarkdownExporter.Default)
            .AddFilter(defaultConfig.GetFilters().ToArray())
            .AddHardwareCounters(defaultConfig.GetHardwareCounters().ToArray())
            //.AddJob(defaultConfig.GetJobs().ToArray())
            .AddLogicalGroupRules(defaultConfig.GetLogicalGroupRules().ToArray())
            .AddLogger(defaultConfig.GetLoggers().ToArray())
            .AddValidator(defaultConfig.GetValidators().ToArray());

        config.UnionRule = ConfigUnionRule.AlwaysUseGlobal;

        _ = config.AddLogger(defaultConfig.GetLoggers().ToArray());

        return config;
    }
}
