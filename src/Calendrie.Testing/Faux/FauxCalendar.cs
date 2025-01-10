﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Faux;

using System.Collections.Generic;

using Calendrie.Hemerology;

[Obsolete("Use Calendar instead")]
public sealed class FauxCalendar : CalendarSystem
{
    public FauxCalendar(string name, CalendarScope scope) : base(name, scope) { }

    //public override int CountDaysInMonth(int year, int month) => throw new NotSupportedException();
    //public override int CountDaysInYear(int year) => throw new NotSupportedException();
}

public sealed class FauxCalendar<TDate> : CalendarSystem, IDateProvider<TDate>
{
    public FauxCalendar(string name, CalendarScope scope) : base(name, scope) { }

    //public override int CountDaysInMonth(int year, int month) => throw new NotSupportedException();
    //public override int CountDaysInYear(int year) => throw new NotSupportedException();

    public IEnumerable<TDate> GetDaysInMonth(int year, int month) => throw new NotSupportedException();
    public IEnumerable<TDate> GetDaysInYear(int year) => throw new NotSupportedException();

    public TDate GetStartOfYear(int year) => throw new NotSupportedException();
    public TDate GetEndOfYear(int year) => throw new NotSupportedException();

    public TDate GetStartOfMonth(int year, int month) => throw new NotSupportedException();
    public TDate GetEndOfMonth(int year, int month) => throw new NotSupportedException();
}
