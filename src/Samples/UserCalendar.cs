// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using Calendrie.Hemerology;

public abstract class UserCalendar : Calendar
{
    protected UserCalendar(string name, MinMaxYearScope scope) : base(name, scope) { }
}
