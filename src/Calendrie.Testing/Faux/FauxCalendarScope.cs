// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Faux;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Hemerology;

public sealed class FauxCalendarScope : CalendarScope
{
    public FauxCalendarScope(ICalendricalSchema schema, int minYear, int maxYear)
        : this(schema, default, minYear, maxYear) { }

    public FauxCalendarScope(ICalendricalSchema schema, DayNumber epoch, int minYear, int maxYear)
        : base(CalendricalSegment.Create(schema, Range.Create(minYear, maxYear)), epoch) { }

    public FauxCalendarScope(DayNumber epoch, CalendricalSegment segment)
        : base(segment, epoch) { }

    // Constructor to be able to test the base constructors; see also WithMinDaysInXXX().
    public FauxCalendarScope(CalendricalSegment segment)
        : base(segment, default) { }

    public override bool CheckYear(int year) => throw new NotSupportedException();
    public override bool CheckYearMonth(int year, int month) => throw new NotSupportedException();
    public override bool CheckYearMonthDay(int year, int month, int day) => throw new NotSupportedException();
    public override bool CheckOrdinal(int year, int dayOfYear) => throw new NotSupportedException();
    public override void ValidateYear(int year, string? paramName = null) => throw new NotSupportedException();
    public override void ValidateYearMonth(int year, int month, string? paramName = null) => throw new NotSupportedException();
    public override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null) => throw new NotSupportedException();
    public override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null) => throw new NotSupportedException();
}
