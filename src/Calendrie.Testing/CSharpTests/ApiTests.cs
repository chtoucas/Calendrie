// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.CSharpTests;

using System.Linq;
using System.Reflection;

using Calendrie.Core;

public static class ApiTests
{
    private static readonly IEnumerable<TypeInfo> s_DefinedTypes =
        typeof(ICalendricalSchema).Assembly.DefinedTypes;

    [Fact]
    public static void Schema_Constructor_IsNotPublic()
    {
        // We also test the abstract classes.
        var schemaTypes = s_DefinedTypes
            .Where(t => t.IsSubclassOf(typeof(LimitSchema)));

        Assert.NotEmpty(schemaTypes);

        foreach (var type in schemaTypes)
        {
            var publicCtors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            Assert.Empty(publicCtors);
        }
    }

    [Fact]
    public static void Schema_GetInstance()
    {
        var schemaTypes = s_DefinedTypes
            .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(LimitSchema)));

        Assert.NotEmpty(schemaTypes);

        var methodName = "GetInstance";

        foreach (var type in schemaTypes)
        {
            var getInstance = type.GetMethod(
                methodName,
                BindingFlags.Public | BindingFlags.Static,
                null,
                Type.EmptyTypes,
                null);

            if (getInstance is null)
            {
                AssertEx.Fails($"Method {methodName} not found for {type}.");
                continue;
            }

            // GetInstance() does NOT return a singleton instance.
            object? inst1 = getInstance.Invoke(null, null);
            object? inst2 = getInstance.Invoke(null, null);
            Assert.NotSame(inst1, inst2);
        }
    }
}
