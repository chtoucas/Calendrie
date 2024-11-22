// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing;

using System.Numerics;

using Calendrie.Hemerology;

// Test aux limites.
public static class DayOfWeekAdjusterTester
{
    [Pure]
    public static DayOfWeekAdjusterTester<T> NearMinValue<T>(T min)
        where T : IFixedDate<T>, IAdditionOperators<T, int, T> =>
        new(min, testNext: false, (x, n) => x + n);

    [Pure]
    public static DayOfWeekAdjusterTester<T> NearMaxValue<T>(T max)
        where T : IFixedDate<T>, IAdditionOperators<T, int, T> =>
        new(max, testNext: true, (x, n) => x + n);
}

public sealed partial class DayOfWeekAdjusterTester<T> where T : IFixedDate<T>
{
    private readonly bool _testNext;

    private readonly T _day;
    private readonly T _day1;
    private readonly T _day2;
    private readonly T _day3;
    private readonly T _day4;
    private readonly T _day5;
    private readonly T _day6;
    private readonly T _day7;
    private readonly T _day8;
    private readonly T _day9;

    private readonly DayOfWeek _dow;
    private readonly DayOfWeek _dow1;
    private readonly DayOfWeek _dow2;
    private readonly DayOfWeek _dow3;
    private readonly DayOfWeek _dow4;
    private readonly DayOfWeek _dow5;
    private readonly DayOfWeek _dow6;

    public DayOfWeekAdjusterTester(T day, bool testNext, Func<T, int, T> add)
    {
        ArgumentNullException.ThrowIfNull(add);

        _day = day;
        _dow = day.DayOfWeek;
        _testNext = testNext;

        if (testNext)
        {
            _day1 = add(day, -1);
            _day2 = add(_day1, -1);
            _day3 = add(_day2, -1);
            _day4 = add(_day3, -1);
            _day5 = add(_day4, -1);
            _day6 = add(_day5, -1);
            _day7 = add(_day6, -1);
            _day8 = add(_day7, -1);
            _day9 = add(_day8, -1);

            _dow1 = PreviousDayOfWeek(_dow);
            _dow2 = PreviousDayOfWeek(_dow1);
            _dow3 = PreviousDayOfWeek(_dow2);
            _dow4 = PreviousDayOfWeek(_dow3);
            _dow5 = PreviousDayOfWeek(_dow4);
            _dow6 = PreviousDayOfWeek(_dow5);
        }
        else
        {
            _day1 = add(day, 1);
            _day2 = add(_day1, 1);
            _day3 = add(_day2, 1);
            _day4 = add(_day3, 1);
            _day5 = add(_day4, 1);
            _day6 = add(_day5, 1);
            _day7 = add(_day6, 1);
            _day8 = add(_day7, 1);
            _day9 = add(_day8, 1);

            _dow1 = NextDayOfWeek(_dow);
            _dow2 = NextDayOfWeek(_dow1);
            _dow3 = NextDayOfWeek(_dow2);
            _dow4 = NextDayOfWeek(_dow3);
            _dow5 = NextDayOfWeek(_dow4);
            _dow6 = NextDayOfWeek(_dow5);
        }
    }

    private static DayOfWeek PreviousDayOfWeek(DayOfWeek dow) =>
        dow == DayOfWeek.Sunday ? DayOfWeek.Saturday : dow - 1;

    private static DayOfWeek NextDayOfWeek(DayOfWeek dow) =>
        dow == DayOfWeek.Saturday ? DayOfWeek.Sunday : dow + 1;
}

public partial class DayOfWeekAdjusterTester<T> // Previous() & PreviousOrSame()
{
    public void TestPrevious()
    {
        if (_testNext) { throw new InvalidOperationException(); }

        AssertEx.Overflows(() => _day.Previous(_dow));
        AssertEx.Overflows(() => _day.Previous(_dow1));
        AssertEx.Overflows(() => _day.Previous(_dow2));
        AssertEx.Overflows(() => _day.Previous(_dow3));
        AssertEx.Overflows(() => _day.Previous(_dow4));
        AssertEx.Overflows(() => _day.Previous(_dow5));
        AssertEx.Overflows(() => _day.Previous(_dow6));

        Assert.Equal(_day, _day1.Previous(_dow));
        AssertEx.Overflows(() => _day1.Previous(_dow1));
        AssertEx.Overflows(() => _day1.Previous(_dow2));
        AssertEx.Overflows(() => _day1.Previous(_dow3));
        AssertEx.Overflows(() => _day1.Previous(_dow4));
        AssertEx.Overflows(() => _day1.Previous(_dow5));
        AssertEx.Overflows(() => _day1.Previous(_dow6));

        Assert.Equal(_day, _day2.Previous(_dow));
        Assert.Equal(_day1, _day2.Previous(_dow1));
        AssertEx.Overflows(() => _day2.Previous(_dow2));
        AssertEx.Overflows(() => _day2.Previous(_dow3));
        AssertEx.Overflows(() => _day2.Previous(_dow4));
        AssertEx.Overflows(() => _day2.Previous(_dow5));
        AssertEx.Overflows(() => _day2.Previous(_dow6));

        Assert.Equal(_day, _day3.Previous(_dow));
        Assert.Equal(_day1, _day3.Previous(_dow1));
        Assert.Equal(_day2, _day3.Previous(_dow2));
        AssertEx.Overflows(() => _day3.Previous(_dow3));
        AssertEx.Overflows(() => _day3.Previous(_dow4));
        AssertEx.Overflows(() => _day3.Previous(_dow5));
        AssertEx.Overflows(() => _day3.Previous(_dow6));

        Assert.Equal(_day, _day4.Previous(_dow));
        Assert.Equal(_day1, _day4.Previous(_dow1));
        Assert.Equal(_day2, _day4.Previous(_dow2));
        Assert.Equal(_day3, _day4.Previous(_dow3));
        AssertEx.Overflows(() => _day4.Previous(_dow4));
        AssertEx.Overflows(() => _day4.Previous(_dow5));
        AssertEx.Overflows(() => _day4.Previous(_dow6));

        Assert.Equal(_day, _day5.Previous(_dow));
        Assert.Equal(_day1, _day5.Previous(_dow1));
        Assert.Equal(_day2, _day5.Previous(_dow2));
        Assert.Equal(_day3, _day5.Previous(_dow3));
        Assert.Equal(_day4, _day5.Previous(_dow4));
        AssertEx.Overflows(() => _day5.Previous(_dow5));
        AssertEx.Overflows(() => _day5.Previous(_dow6));

        Assert.Equal(_day, _day6.Previous(_dow));
        Assert.Equal(_day1, _day6.Previous(_dow1));
        Assert.Equal(_day2, _day6.Previous(_dow2));
        Assert.Equal(_day3, _day6.Previous(_dow3));
        Assert.Equal(_day4, _day6.Previous(_dow4));
        Assert.Equal(_day5, _day6.Previous(_dow5));
        AssertEx.Overflows(() => _day6.Previous(_dow6));

        Assert.Equal(_day, _day7.Previous(_dow));
        Assert.Equal(_day1, _day7.Previous(_dow1));
        Assert.Equal(_day2, _day7.Previous(_dow2));
        Assert.Equal(_day3, _day7.Previous(_dow3));
        Assert.Equal(_day4, _day7.Previous(_dow4));
        Assert.Equal(_day5, _day7.Previous(_dow5));
        Assert.Equal(_day6, _day7.Previous(_dow6));
    }

    public void TestPreviousOrSame()
    {
        if (_testNext) { throw new InvalidOperationException(); }

        Assert.Equal(_day, _day.PreviousOrSame(_dow));
        AssertEx.Overflows(() => _day.PreviousOrSame(_dow1));
        AssertEx.Overflows(() => _day.PreviousOrSame(_dow2));
        AssertEx.Overflows(() => _day.PreviousOrSame(_dow3));
        AssertEx.Overflows(() => _day.PreviousOrSame(_dow4));
        AssertEx.Overflows(() => _day.PreviousOrSame(_dow5));
        AssertEx.Overflows(() => _day.PreviousOrSame(_dow6));

        Assert.Equal(_day, _day1.PreviousOrSame(_dow));
        Assert.Equal(_day1, _day1.PreviousOrSame(_dow1));
        AssertEx.Overflows(() => _day1.PreviousOrSame(_dow2));
        AssertEx.Overflows(() => _day1.PreviousOrSame(_dow3));
        AssertEx.Overflows(() => _day1.PreviousOrSame(_dow4));
        AssertEx.Overflows(() => _day1.PreviousOrSame(_dow5));
        AssertEx.Overflows(() => _day1.PreviousOrSame(_dow6));

        Assert.Equal(_day, _day2.PreviousOrSame(_dow));
        Assert.Equal(_day1, _day2.PreviousOrSame(_dow1));
        Assert.Equal(_day2, _day2.PreviousOrSame(_dow2));
        AssertEx.Overflows(() => _day2.PreviousOrSame(_dow3));
        AssertEx.Overflows(() => _day2.PreviousOrSame(_dow4));
        AssertEx.Overflows(() => _day2.PreviousOrSame(_dow5));
        AssertEx.Overflows(() => _day2.PreviousOrSame(_dow6));

        Assert.Equal(_day, _day3.PreviousOrSame(_dow));
        Assert.Equal(_day1, _day3.PreviousOrSame(_dow1));
        Assert.Equal(_day2, _day3.PreviousOrSame(_dow2));
        Assert.Equal(_day3, _day3.PreviousOrSame(_dow3));
        AssertEx.Overflows(() => _day3.PreviousOrSame(_dow4));
        AssertEx.Overflows(() => _day3.PreviousOrSame(_dow5));
        AssertEx.Overflows(() => _day3.PreviousOrSame(_dow6));

        Assert.Equal(_day, _day4.PreviousOrSame(_dow));
        Assert.Equal(_day1, _day4.PreviousOrSame(_dow1));
        Assert.Equal(_day2, _day4.PreviousOrSame(_dow2));
        Assert.Equal(_day3, _day4.PreviousOrSame(_dow3));
        Assert.Equal(_day4, _day4.PreviousOrSame(_dow4));
        AssertEx.Overflows(() => _day4.PreviousOrSame(_dow5));
        AssertEx.Overflows(() => _day4.PreviousOrSame(_dow6));

        Assert.Equal(_day, _day5.PreviousOrSame(_dow));
        Assert.Equal(_day1, _day5.PreviousOrSame(_dow1));
        Assert.Equal(_day2, _day5.PreviousOrSame(_dow2));
        Assert.Equal(_day3, _day5.PreviousOrSame(_dow3));
        Assert.Equal(_day4, _day5.PreviousOrSame(_dow4));
        Assert.Equal(_day5, _day5.PreviousOrSame(_dow5));
        AssertEx.Overflows(() => _day5.PreviousOrSame(_dow6));

        Assert.Equal(_day, _day6.PreviousOrSame(_dow));
        Assert.Equal(_day1, _day6.PreviousOrSame(_dow1));
        Assert.Equal(_day2, _day6.PreviousOrSame(_dow2));
        Assert.Equal(_day3, _day6.PreviousOrSame(_dow3));
        Assert.Equal(_day4, _day6.PreviousOrSame(_dow4));
        Assert.Equal(_day5, _day6.PreviousOrSame(_dow5));
        Assert.Equal(_day6, _day6.PreviousOrSame(_dow6));
    }
}

public partial class DayOfWeekAdjusterTester<T> // Nearest()
{
    [Fact]
    public void TestNearest()
    {
        // MinValue.
        Assert.Equal(_day, _day.Nearest(_dow));
        Assert.Equal(_day1, _day.Nearest(_dow1));
        Assert.Equal(_day2, _day.Nearest(_dow2));
        Assert.Equal(_day3, _day.Nearest(_dow3));
        AssertEx.Overflows(() => _day.Nearest(_dow4));  // day - 3
        AssertEx.Overflows(() => _day.Nearest(_dow5));  // day - 2
        AssertEx.Overflows(() => _day.Nearest(_dow6));  // day - 1

        // MinValue + 1.
        Assert.Equal(_day, _day1.Nearest(_dow));
        Assert.Equal(_day1, _day1.Nearest(_dow1));
        Assert.Equal(_day2, _day1.Nearest(_dow2));
        Assert.Equal(_day3, _day1.Nearest(_dow3));
        Assert.Equal(_day4, _day1.Nearest(_dow4));
        AssertEx.Overflows(() => _day1.Nearest(_dow5)); // day - 2
        AssertEx.Overflows(() => _day1.Nearest(_dow6)); // day - 1

        // MinValue + 2.
        Assert.Equal(_day, _day2.Nearest(_dow));
        Assert.Equal(_day1, _day2.Nearest(_dow1));
        Assert.Equal(_day2, _day2.Nearest(_dow2));
        Assert.Equal(_day3, _day2.Nearest(_dow3));
        Assert.Equal(_day4, _day2.Nearest(_dow4));
        Assert.Equal(_day5, _day2.Nearest(_dow5));
        AssertEx.Overflows(() => _day2.Nearest(_dow6)); // day - 1

        // MinValue + 3.
        Assert.Equal(_day, _day3.Nearest(_dow));
        Assert.Equal(_day1, _day3.Nearest(_dow1));
        Assert.Equal(_day2, _day3.Nearest(_dow2));
        Assert.Equal(_day3, _day3.Nearest(_dow3));
        Assert.Equal(_day4, _day3.Nearest(_dow4));
        Assert.Equal(_day5, _day3.Nearest(_dow5));
        Assert.Equal(_day6, _day3.Nearest(_dow6));

        // MinValue + 4.
        Assert.Equal(_day7, _day4.Nearest(_dow));      // day + 7
        Assert.Equal(_day1, _day4.Nearest(_dow1));
        Assert.Equal(_day2, _day4.Nearest(_dow2));
        Assert.Equal(_day3, _day4.Nearest(_dow3));
        Assert.Equal(_day4, _day4.Nearest(_dow4));
        Assert.Equal(_day5, _day4.Nearest(_dow5));
        Assert.Equal(_day6, _day4.Nearest(_dow6));

        // MinValue + 5.
        Assert.Equal(_day7, _day5.Nearest(_dow));      // day + 7
        Assert.Equal(_day8, _day5.Nearest(_dow1));     // day + 8
        Assert.Equal(_day2, _day5.Nearest(_dow2));
        Assert.Equal(_day3, _day5.Nearest(_dow3));
        Assert.Equal(_day4, _day5.Nearest(_dow4));
        Assert.Equal(_day5, _day5.Nearest(_dow5));
        Assert.Equal(_day6, _day5.Nearest(_dow6));

        // MinValue + 6.
        Assert.Equal(_day7, _day6.Nearest(_dow));      // day + 7
        Assert.Equal(_day8, _day6.Nearest(_dow1));     // day + 8
        Assert.Equal(_day9, _day6.Nearest(_dow2));     // day + 9
        Assert.Equal(_day3, _day6.Nearest(_dow3));
        Assert.Equal(_day4, _day6.Nearest(_dow4));
        Assert.Equal(_day5, _day6.Nearest(_dow5));
        Assert.Equal(_day6, _day6.Nearest(_dow6));
    }
}

public partial class DayOfWeekAdjusterTester<T> // Next() & NextOrSame()
{
    public void TestNextOrSame()
    {
        if (!_testNext) { throw new InvalidOperationException(); }

        Assert.Equal(_day, _day.NextOrSame(_dow));
        AssertEx.Overflows(() => _day.NextOrSame(_dow1));
        AssertEx.Overflows(() => _day.NextOrSame(_dow2));
        AssertEx.Overflows(() => _day.NextOrSame(_dow3));
        AssertEx.Overflows(() => _day.NextOrSame(_dow4));
        AssertEx.Overflows(() => _day.NextOrSame(_dow5));
        AssertEx.Overflows(() => _day.NextOrSame(_dow6));

        Assert.Equal(_day, _day1.NextOrSame(_dow));
        Assert.Equal(_day1, _day1.NextOrSame(_dow1));
        AssertEx.Overflows(() => _day1.NextOrSame(_dow2));
        AssertEx.Overflows(() => _day1.NextOrSame(_dow3));
        AssertEx.Overflows(() => _day1.NextOrSame(_dow4));
        AssertEx.Overflows(() => _day1.NextOrSame(_dow5));
        AssertEx.Overflows(() => _day1.NextOrSame(_dow6));

        Assert.Equal(_day, _day2.NextOrSame(_dow));
        Assert.Equal(_day1, _day2.NextOrSame(_dow1));
        Assert.Equal(_day2, _day2.NextOrSame(_dow2));
        AssertEx.Overflows(() => _day2.NextOrSame(_dow3));
        AssertEx.Overflows(() => _day2.NextOrSame(_dow4));
        AssertEx.Overflows(() => _day2.NextOrSame(_dow5));
        AssertEx.Overflows(() => _day2.NextOrSame(_dow6));

        Assert.Equal(_day, _day3.NextOrSame(_dow));
        Assert.Equal(_day1, _day3.NextOrSame(_dow1));
        Assert.Equal(_day2, _day3.NextOrSame(_dow2));
        Assert.Equal(_day3, _day3.NextOrSame(_dow3));
        AssertEx.Overflows(() => _day3.NextOrSame(_dow4));
        AssertEx.Overflows(() => _day3.NextOrSame(_dow5));
        AssertEx.Overflows(() => _day3.NextOrSame(_dow6));

        Assert.Equal(_day, _day4.NextOrSame(_dow));
        Assert.Equal(_day1, _day4.NextOrSame(_dow1));
        Assert.Equal(_day2, _day4.NextOrSame(_dow2));
        Assert.Equal(_day3, _day4.NextOrSame(_dow3));
        Assert.Equal(_day4, _day4.NextOrSame(_dow4));
        AssertEx.Overflows(() => _day4.NextOrSame(_dow5));
        AssertEx.Overflows(() => _day4.NextOrSame(_dow6));

        Assert.Equal(_day, _day5.NextOrSame(_dow));
        Assert.Equal(_day1, _day5.NextOrSame(_dow1));
        Assert.Equal(_day2, _day5.NextOrSame(_dow2));
        Assert.Equal(_day3, _day5.NextOrSame(_dow3));
        Assert.Equal(_day4, _day5.NextOrSame(_dow4));
        Assert.Equal(_day5, _day5.NextOrSame(_dow5));
        AssertEx.Overflows(() => _day5.NextOrSame(_dow6));

        Assert.Equal(_day, _day6.NextOrSame(_dow));
        Assert.Equal(_day1, _day6.NextOrSame(_dow1));
        Assert.Equal(_day2, _day6.NextOrSame(_dow2));
        Assert.Equal(_day3, _day6.NextOrSame(_dow3));
        Assert.Equal(_day4, _day6.NextOrSame(_dow4));
        Assert.Equal(_day5, _day6.NextOrSame(_dow5));
        Assert.Equal(_day6, _day6.NextOrSame(_dow6));
    }

    public void TestNext()
    {
        if (!_testNext) { throw new InvalidOperationException(); }

        AssertEx.Overflows(() => _day.Next(_dow));
        AssertEx.Overflows(() => _day.Next(_dow1));
        AssertEx.Overflows(() => _day.Next(_dow2));
        AssertEx.Overflows(() => _day.Next(_dow3));
        AssertEx.Overflows(() => _day.Next(_dow4));
        AssertEx.Overflows(() => _day.Next(_dow5));
        AssertEx.Overflows(() => _day.Next(_dow6));

        Assert.Equal(_day, _day1.Next(_dow));
        AssertEx.Overflows(() => _day1.Next(_dow1));
        AssertEx.Overflows(() => _day1.Next(_dow2));
        AssertEx.Overflows(() => _day1.Next(_dow3));
        AssertEx.Overflows(() => _day1.Next(_dow4));
        AssertEx.Overflows(() => _day1.Next(_dow5));
        AssertEx.Overflows(() => _day1.Next(_dow6));

        Assert.Equal(_day, _day2.Next(_dow));
        Assert.Equal(_day1, _day2.Next(_dow1));
        AssertEx.Overflows(() => _day2.Next(_dow2));
        AssertEx.Overflows(() => _day2.Next(_dow3));
        AssertEx.Overflows(() => _day2.Next(_dow4));
        AssertEx.Overflows(() => _day2.Next(_dow5));
        AssertEx.Overflows(() => _day2.Next(_dow6));

        Assert.Equal(_day, _day3.Next(_dow));
        Assert.Equal(_day1, _day3.Next(_dow1));
        Assert.Equal(_day2, _day3.Next(_dow2));
        AssertEx.Overflows(() => _day3.Next(_dow3));
        AssertEx.Overflows(() => _day3.Next(_dow4));
        AssertEx.Overflows(() => _day3.Next(_dow5));
        AssertEx.Overflows(() => _day3.Next(_dow6));

        Assert.Equal(_day, _day4.Next(_dow));
        Assert.Equal(_day1, _day4.Next(_dow1));
        Assert.Equal(_day2, _day4.Next(_dow2));
        Assert.Equal(_day3, _day4.Next(_dow3));
        AssertEx.Overflows(() => _day4.Next(_dow4));
        AssertEx.Overflows(() => _day4.Next(_dow5));
        AssertEx.Overflows(() => _day4.Next(_dow6));

        Assert.Equal(_day, _day5.Next(_dow));
        Assert.Equal(_day1, _day5.Next(_dow1));
        Assert.Equal(_day2, _day5.Next(_dow2));
        Assert.Equal(_day3, _day5.Next(_dow3));
        Assert.Equal(_day4, _day5.Next(_dow4));
        AssertEx.Overflows(() => _day5.Next(_dow5));
        AssertEx.Overflows(() => _day5.Next(_dow6));

        Assert.Equal(_day, _day6.Next(_dow));
        Assert.Equal(_day1, _day6.Next(_dow1));
        Assert.Equal(_day2, _day6.Next(_dow2));
        Assert.Equal(_day3, _day6.Next(_dow3));
        Assert.Equal(_day4, _day6.Next(_dow4));
        Assert.Equal(_day5, _day6.Next(_dow5));
        AssertEx.Overflows(() => _day6.Next(_dow6));

        Assert.Equal(_day, _day7.Next(_dow));
        Assert.Equal(_day1, _day7.Next(_dow1));
        Assert.Equal(_day2, _day7.Next(_dow2));
        Assert.Equal(_day3, _day7.Next(_dow3));
        Assert.Equal(_day4, _day7.Next(_dow4));
        Assert.Equal(_day5, _day7.Next(_dow5));
        Assert.Equal(_day6, _day7.Next(_dow6));
    }
}
