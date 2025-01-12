// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Systems;

using Calendrie.Systems;
using Calendrie.Testing.Data;
using Calendrie.Testing.Data.Scopes;
using Calendrie.Testing.Facts.Hemerology;

// REVIEW(fact): how to enforce the use of a StandardCalendarDataSet?

/// <summary>
/// Provides data-driven tests for <see cref="StandardScope"/>.
/// </summary>
internal class StandardScopeFacts<TDataSet> :
    CalendarScopeFacts<StandardScope, TDataSet, StandardScopeData>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    public StandardScopeFacts(StandardScope scope) : base(scope) { }
}
