// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Systems;

using Calendrie.Systems;
using Calendrie.Testing.Data;
using Calendrie.Testing.Data.Bounded;
using Calendrie.Testing.Data.Scopes;
using Calendrie.Testing.Facts.Hemerology;

/// <summary>
/// Provides data-driven tests for the <see cref="StandardScope"/> type.
/// <para><typeparamref name="TDataSet"/> MUST be of the
/// <see cref="StandardCalendarDataSet{TDataSet}"/> type.</para>
/// </summary>
internal class StandardScopeFacts<TDataSet> :
    CalendarScopeFacts<StandardScope, TDataSet, StandardScopeDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    public StandardScopeFacts(StandardScope scope) : base(scope) { }
}
