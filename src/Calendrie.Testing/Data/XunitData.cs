// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data;

using System.Collections;

[SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix")]
public sealed class XunitData<T> : IReadOnlyCollection<object?[]>
{
    private readonly IEnumerable<object?[]> _values;

    public XunitData(IEnumerable<object?[]> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        _values = values;
    }

    public int Count => _values.Count();

    public IEnumerator<object?[]> GetEnumerator() => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
