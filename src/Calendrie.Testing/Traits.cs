// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing;

using Xunit.Abstractions;
using Xunit.Sdk;

// Used by eng\test.ps1, eng\cover.ps1 and the github action.
// See https://github.com/xunit/samples.xunit/blob/main/TraitExtensibility/
//
// WARNING: trait discoverers do not inspect the base class attributes.

internal static class XunitTraitAssembly
{
    public const string Name = "Calendrie.Testing";
    public const string TypePrefix = Name + ".";
}

// Be careful if you change the values, the scripts rely on them.
internal static class XunitTraits
{
    // All traits have a single property: we use the same string for both name
    // and value of a trait.
    public const string ExcludeFrom = "ExcludeFrom";
    public const string Performance = "Performance";
}

// Be careful if you change the values, the scripts rely on the fact that
// the name contains the string "Slow" to filter out the slow tests.
public enum TestPerformance
{
    /// <summary>
    /// A slow test unit.
    /// </summary>
    SlowUnit,

    /// <summary>
    /// A slow test bundle, typically a test class.
    /// </summary>
    SlowBundle
}

public enum TestExcludeFrom
{
    /// <summary>
    /// Exclude from code coverage.
    /// <para>We use this value to exclude tests not needed to achieve full code
    /// coverage. For instance, we exclude redundant tests.</para>
    /// <para>We also exclude deeply recursive functions.</para>
    /// </summary>
    CodeCoverage,

    /// <summary>
    /// Exclude from the "regular" test plan.
    /// <para>We use this value to exclude tests of low importance.</para>
    /// <para>This value only exists to reduce the time needed to complete the
    /// test plan "Regular".</para>
    /// </summary>
    Regular
}

public static class TestExcludeFromValues
{
    public static readonly string CodeCoverage = TestExcludeFrom.CodeCoverage.ToString();
    public static readonly string Regular = TestExcludeFrom.Regular.ToString();
}

// One SHOULD NOT use this attribute in a test bundle ie facts class: one should
// use RedundantTest or something else that does not depend on the plan.
// One exception: CodeCoverage is OK.
[TraitDiscoverer(XunitTraitAssembly.TypePrefix + nameof(ExcludeFromTraitDiscoverer), XunitTraitAssembly.Name)]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class TestExcludeFromAttribute : Attribute, ITraitAttribute
{
    public TestExcludeFromAttribute(TestExcludeFrom excludeFrom) { ExcludeFrom = excludeFrom; }

    public TestExcludeFrom ExcludeFrom { get; }
}

[TraitDiscoverer(XunitTraitAssembly.TypePrefix + nameof(PerformanceTraitDiscoverer), XunitTraitAssembly.Name)]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class TestPerformanceAttribute : Attribute, ITraitAttribute
{
    public TestPerformanceAttribute(TestPerformance performance) { Performance = performance; }

    public TestPerformance Performance { get; }
}

#region Discoverers

public sealed class ExcludeFromTraitDiscoverer : ITraitDiscoverer
{
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
        ArgumentNullException.ThrowIfNull(traitAttribute);

        var value = traitAttribute.GetNamedArgument<TestExcludeFrom>(XunitTraits.ExcludeFrom);

        switch (value)
        {
            case TestExcludeFrom.CodeCoverage:
                yield return new KeyValuePair<string, string>(XunitTraits.ExcludeFrom, TestExcludeFromValues.CodeCoverage);
                break;
            case TestExcludeFrom.Regular:
                yield return new KeyValuePair<string, string>(XunitTraits.ExcludeFrom, TestExcludeFromValues.Regular);
                break;
            default:
                throw new InvalidOperationException();
        }
    }
}

public sealed class PerformanceTraitDiscoverer : ITraitDiscoverer
{
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
        ArgumentNullException.ThrowIfNull(traitAttribute);

        var value = traitAttribute.GetNamedArgument<TestPerformance>(XunitTraits.Performance);
        yield return new KeyValuePair<string, string>(XunitTraits.Performance, value.ToString());
    }
}

#endregion
