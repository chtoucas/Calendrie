// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

// This class works best for date types based on the count of days since the epoch.

public sealed class DateProvider<TDate, TCalendar> : IDateProvider<TDate>
    where TDate : IDate<TDate>, ICalendarBound<TCalendar>
    where TCalendar : Calendar
{
    private readonly CalendarScope _scope;
    private readonly DayNumber _epoch;

    public DateProvider()
    {
        _scope = TDate.Calendar.Scope;
        _epoch = _scope.Epoch;
    }

    /// <inheritdoc/>
    [Pure]
    public IEnumerable<TDate> GetDaysInYear(int year)
    {
        _scope.ValidateYear(year);

        var sch = _scope.Schema;
        int startOfYear = sch.GetStartOfYear(year);
        int daysInYear = sch.CountDaysInYear(year);

        return from daysSinceEpoch
               in Enumerable.Range(startOfYear, daysInYear)
               select TDate.FromDayNumber(_epoch + daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public IEnumerable<TDate> GetDaysInMonth(int year, int month)
    {
        _scope.ValidateYearMonth(year, month);

        var sch = _scope.Schema;
        int startOfMonth = sch.GetStartOfMonth(year, month);
        int daysInMonth = sch.CountDaysInMonth(year, month);

        return from daysSinceEpoch
               in Enumerable.Range(startOfMonth, daysInMonth)
               select TDate.FromDayNumber(_epoch + daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public TDate GetStartOfYear(int year)
    {
        _scope.ValidateYear(year);
        int daysSinceEpoch = _scope.Schema.GetStartOfYear(year);
        return TDate.FromDayNumber(_epoch + daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public TDate GetEndOfYear(int year)
    {
        _scope.ValidateYear(year);
        int daysSinceEpoch = _scope.Schema.GetEndOfYear(year);
        return TDate.FromDayNumber(_epoch + daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public TDate GetStartOfMonth(int year, int month)
    {
        _scope.ValidateYearMonth(year, month);
        int daysSinceEpoch = _scope.Schema.GetStartOfMonth(year, month);
        return TDate.FromDayNumber(_epoch + daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public TDate GetEndOfMonth(int year, int month)
    {
        _scope.ValidateYearMonth(year, month);
        int daysSinceEpoch = _scope.Schema.GetEndOfMonth(year, month);
        return TDate.FromDayNumber(_epoch + daysSinceEpoch);
    }
}
