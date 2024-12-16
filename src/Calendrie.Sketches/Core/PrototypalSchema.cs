// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

#pragma warning disable CA1033 // Interface methods should be callable by child types

namespace Calendrie.Core;

using Calendrie.Core.Intervals;
using Calendrie.Core.Validation;

#region Developer Notes

// The code is NOT meant to be efficient.
//
// Usefulness? ICalendar is easy to implement and test (see for instance
// GregorianKernel), which might not be the case of ICalendricalSchema.
// With PrototypalSchema, we can then
// - quickly prototype a new schema
// - validate test data independently
// At the same time, it demonstrates that an ICalendar instance is all we need
// to build a full schema.
//
// Apart from the core kernel methods, the only other methods available
// implicitely are the abstract/virtual ones.
//
// ### Abstract Methods
//
// - MinDaysInYear
// - MinDaysInMonth
// - SupportedYears
// - PreValidator
// - Arithmetic
//
// These are the properties defined by ICalendricalSchema not inherited from
// ICalendar.
//
// ### Virtual Methods
//
// - CountDaysInYearBeforeMonth()
// - GetMonthParts()
// - GetYear()
// - GetMonth()
// - GetStartOfYearInMonths()
// - GetStartOfYear()
//
// WARNING: the default impl of GetYear() and GetStartOfYear/InMonths() are
// extremely slow if the values of "y" or "daysSinceEpoch" are large; see
// SupportedYears.
//
// Notice that in Calendrie.Core.Schemas, we use purely computational
// formulae (faster, no IndexOutOfRangeException) mostly obtained by
// geometric means.
//
// - DayForm (trivial)
// - MonthForm
//   - CountDaysInYearBeforeMonth(m)
//   - CountDaysInMonth(m)
//   - GetMonth(d0y, out d0)
// - YearForm
//   - CountDaysInYear(y)
//   - GetYear(daysSinceEpoch, out d0y)
//   - GetStartOfYear(y)

#endregion

/// <summary>
/// Represents a prototypal implementation of the <see cref="ICalendricalSchemaPlus"/>
/// interface.
/// </summary>
public partial class PrototypalSchema :
    ICalendar,
    ICalendricalSchema,
    ICalendricalSchemaPlus
{
    /// <summary>
    /// Represents a partial <see cref="ICalendricalSchema"/> view of this
    /// instance.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly SchemaProxy _proxy;

#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CA1051 // Do not declare visible instance fields
    /// <summary>
    /// Represents the calendar kernel.
    /// <para>This field is read-only.</para>
    /// </summary>
    protected readonly ICalendar m_Kernel;

    protected readonly int m_MinDaysInYear;

    protected readonly int m_MinDaysInMonth;
#pragma warning restore IDE1006
#pragma warning restore CA1051

    /// <summary>
    /// Initializes a new instance of the <see cref="PrototypalSchema"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.
    /// </exception>
    public PrototypalSchema(ICalendricalSchema schema)
    {
        ArgumentNullException.ThrowIfNull(schema, nameof(schema));

        m_Kernel = schema;
        _proxy = new SchemaProxy(this);

        m_MinDaysInYear = schema.MinDaysInYear;
        m_MinDaysInMonth = schema.MinDaysInMonth;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PrototypalSchema"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="kernel"/> is null.
    /// </exception>
    public PrototypalSchema(
        ICalendar kernel,
        int minDaysInYear,
        int minDaysInMonth)
    {
        ArgumentNullException.ThrowIfNull(kernel, nameof(kernel));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(minDaysInYear, 0, nameof(minDaysInYear));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(minDaysInMonth, 0, nameof(minDaysInMonth));

        Debug.Assert(minDaysInYear > minDaysInMonth);

        m_Kernel = kernel;
        m_MinDaysInYear = minDaysInYear;
        m_MinDaysInMonth = minDaysInMonth;

        _proxy = new SchemaProxy(this);
    }

    // Another solution could have been to cast "this" to ICalendricalSchema.
    private sealed class SchemaProxy
    {
        private readonly ICalendricalSchema _this;

        public SchemaProxy(PrototypalSchema @this)
        {
            Debug.Assert(@this != null);
            _this = @this;
        }

        /// <summary>Conversion (y, m, d) -> (y, doy).</summary>
        [Pure]
        public int GetDayOfYear(int y, int m, int d) => _this.GetDayOfYear(y, m, d);

        /// <summary>Conversion daysSinceEpoch -> (y, m, d).</summary>
        public void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d) =>
            _this.GetDateParts(daysSinceEpoch, out y, out m, out d);

        [Pure]
        public int GetEndOfYear(int y) => _this.GetEndOfYear(y);

        [Pure]
        public int GetEndOfYearInMonths(int y) => _this.GetEndOfYearInMonths(y);
    }
}

public partial class PrototypalSchema // ICalendar
{
    CalendricalAlgorithm ICalendar.Algorithm => m_Kernel.Algorithm;

    CalendricalFamily ICalendar.Family => m_Kernel.Family;

    CalendricalAdjustments ICalendar.PeriodicAdjustments => m_Kernel.PeriodicAdjustments;

    [Pure]
    bool ICalendar.IsRegular(out int monthsInYear) => m_Kernel.IsRegular(out monthsInYear);

    [Pure]
    bool ICalendar.IsLeapYear(int y) => m_Kernel.IsLeapYear(y);

    [Pure]
    bool ICalendar.IsIntercalaryMonth(int y, int m) => m_Kernel.IsIntercalaryMonth(y, m);

    [Pure]
    bool ICalendar.IsIntercalaryDay(int y, int m, int d) => m_Kernel.IsIntercalaryDay(y, m, d);

    [Pure]
    bool ICalendar.IsSupplementaryDay(int y, int m, int d) => m_Kernel.IsSupplementaryDay(y, m, d);

    [Pure]
    int ICalendar.CountMonthsInYear(int y) => m_Kernel.CountMonthsInYear(y);

    [Pure]
    int ICalendar.CountDaysInYear(int y) => m_Kernel.CountDaysInYear(y);

    [Pure]
    int ICalendar.CountDaysInMonth(int y, int m) => m_Kernel.CountDaysInMonth(y, m);
}

public partial class PrototypalSchema // ICalendricalSchema (1)
{
    // We limit the range of supported years because the default impl of
    // GetYear() and GetStartOfYear() are extremely slow if the values of
    // "y" or "daysSinceEpoch" are big.
    // Only override this property if both methods can handle big values
    // efficiently.

    private Range<int>? _supportedDays;
    /// <inheritdoc />
    public Range<int> SupportedDays =>
        _supportedDays ??= new Range<int>(
            SupportedYears.Endpoints.Select(
                GetStartOfYear,
                _proxy.GetEndOfYear));

    private Range<int>? _supportedMonths;
    /// <inheritdoc />
    public Range<int> SupportedMonths =>
        _supportedMonths ??=
        new Range<int>(
            SupportedYears.Endpoints.Select(
                GetStartOfYearInMonths,
                _proxy.GetEndOfYearInMonths));

    /// <inheritdoc />
    public Range<int> SupportedYears { get; init; } = Range.Create(-9998, 9999);

    /// <inheritdoc />
    [Pure]
    public virtual int CountDaysInYearBeforeMonth(int y, int m)
    {
        // Faster alternatives:
        // - Use a purely computational formula.
        // - Pre-compute the result and use an array lookup.
        //   A quick improvement would be to use GetDaysInMonthDistribution(leap)
        //   from IDaysInMonthDistribution which would avoid the repeated
        //   calls to CountDaysInMonth(y, m).

        int count = 0;
        for (int i = 1; i < m; i++)
        {
            count += m_Kernel.CountDaysInMonth(y, i);
        }
        return count;
    }

    /// <inheritdoc />
    public virtual void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
    {
        // Faster alternatives:
        // - Use a purely computational formula.
        // - Start with an approximation of the result.

        if (monthsSinceEpoch < 0)
        {
            y = 0;
            int startOfYear = -m_Kernel.CountMonthsInYear(0);

            while (monthsSinceEpoch < startOfYear)
            {
                startOfYear -= m_Kernel.CountMonthsInYear(--y);
            }

            // Notice that, as expected, m >= 1.
            m = 1 + monthsSinceEpoch - startOfYear;
        }
        else
        {
            y = 1;
            int startOfYear = 0;

            while (monthsSinceEpoch >= startOfYear)
            {
                int startOfNextYear = startOfYear + m_Kernel.CountMonthsInYear(y);
                if (monthsSinceEpoch < startOfNextYear) { break; }
                y++;
                startOfYear = startOfNextYear;
            }
            Debug.Assert(monthsSinceEpoch >= startOfYear);

            // Notice that, as expected, m >= 1.
            m = 1 + monthsSinceEpoch - startOfYear;
        }
    }

    /// <inheritdoc />
    [Pure]
    public virtual int GetYear(int daysSinceEpoch, out int doy)
    {
        // Faster alternatives:
        // - Use a purely computational formula.
        // - Start with an approximation of the result, see ArchetypalSchema.

        // Find the year for which (daysSinceEpoch - startOfYear) = d0y
        // has the smallest value >= 0.
        if (daysSinceEpoch < 0)
        {
            int y = 0;
            int startOfYear = -m_Kernel.CountDaysInYear(0);

            while (daysSinceEpoch < startOfYear)
            {
                startOfYear -= m_Kernel.CountDaysInYear(--y);
            }

            // Notice that, as expected, doy >= 1.
            doy = 1 + daysSinceEpoch - startOfYear;
            return y;
        }
        else
        {
            int y = 1;
            int startOfYear = 0;

            while (daysSinceEpoch >= startOfYear)
            {
                int startOfNextYear = startOfYear + m_Kernel.CountDaysInYear(y);
                if (daysSinceEpoch < startOfNextYear) { break; }
                y++;
                startOfYear = startOfNextYear;
            }
            Debug.Assert(daysSinceEpoch >= startOfYear);

            // Notice that, as expected, doy >= 1.
            doy = 1 + daysSinceEpoch - startOfYear;
            return y;
        }
    }

    /// <inheritdoc />
    [Pure]
    public virtual int GetMonth(int y, int doy, out int d)
    {
        // Faster alternatives:
        // - Use a purely computational formula.
        // - Start with an approximation of the result, see ArchetypalSchema.

        int m = 1;
        int daysInYearBeforeMonth = 0;

        int monthsInYear = m_Kernel.CountMonthsInYear(y);
        while (m < monthsInYear)
        {
            int daysInYearBeforeNextMonth = CountDaysInYearBeforeMonth(y, m + 1);
            if (doy <= daysInYearBeforeNextMonth) { break; }

            daysInYearBeforeMonth = daysInYearBeforeNextMonth;
            m++;
        }

        // Notice that, as expected, d >= 1.
        d = doy - daysInYearBeforeMonth;
        return m;
    }

    /// <inheritdoc />
    [Pure]
    public virtual int GetStartOfYearInMonths(int y)
    {
        int monthsSinceEpoch = 0;

        if (y < 1)
        {
            for (int i = y; i < 1; i++)
            {
                monthsSinceEpoch -= m_Kernel.CountMonthsInYear(i);
            }
        }
        else
        {
            for (int i = 1; i < y; i++)
            {
                monthsSinceEpoch += m_Kernel.CountMonthsInYear(i);
            }
        }

        return monthsSinceEpoch;
    }

    /// <inheritdoc />
    [Pure]
    public virtual int GetStartOfYear(int y)
    {
        // Faster alternatives:
        // - Use a purely computational formula.
        // - Cache the result, see ArchetypalSchema.

        int daysSinceEpoch = 0;

        if (y < 1)
        {
            for (int i = y; i < 1; i++)
            {
                daysSinceEpoch -= m_Kernel.CountDaysInYear(i);
            }
        }
        else
        {
            for (int i = 1; i < y; i++)
            {
                daysSinceEpoch += m_Kernel.CountDaysInYear(i);
            }
        }

        return daysSinceEpoch;
    }
}

public partial class PrototypalSchema // ICalendricalSchema (2)
{
    int ICalendricalSchema.MinDaysInYear => m_MinDaysInYear;

    int ICalendricalSchema.MinDaysInMonth => m_MinDaysInMonth;

    public virtual ICalendricalPreValidator PreValidator => new PlainPreValidator(this);

    [Pure]
    int ICalendricalSchema.CountMonthsSinceEpoch(int y, int m) =>
        GetStartOfYearInMonths(y) + m - 1;

    [Pure]
    int ICalendricalSchema.CountDaysSinceEpoch(int y, int m, int d) =>
        GetStartOfYear(y) + CountDaysInYearBeforeMonth(y, m) + d - 1;

    [Pure]
    int ICalendricalSchema.CountDaysSinceEpoch(int y, int doy) =>
        GetStartOfYear(y) + doy - 1;

    void ICalendricalSchema.GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        y = GetYear(daysSinceEpoch, out int doy);
        m = GetMonth(y, doy, out d);
    }

    [Pure]
    int ICalendricalSchema.GetDayOfYear(int y, int m, int d) =>
        CountDaysInYearBeforeMonth(y, m) + d;

    [Pure]
    int ICalendricalSchema.GetEndOfYearInMonths(int y) =>
        GetStartOfYearInMonths(y) + m_Kernel.CountMonthsInYear(y) - 1;

    [Pure]
    int ICalendricalSchema.GetEndOfYear(int y) =>
        GetStartOfYear(y) + m_Kernel.CountDaysInYear(y) - 1;

    [Pure]
    int ICalendricalSchema.GetStartOfMonth(int y, int m) =>
        GetStartOfYear(y) + CountDaysInYearBeforeMonth(y, m);

    [Pure]
    int ICalendricalSchema.GetEndOfMonth(int y, int m) =>
        GetStartOfYear(y) + CountDaysInYearBeforeMonth(y, m) + m_Kernel.CountDaysInMonth(y, m) - 1;
}

public partial class PrototypalSchema // ICalendricalSchemaPlus
{
    [Pure]
    int ICalendricalSchemaPlus.CountDaysInYearAfterMonth(int y, int m) =>
        m_Kernel.CountDaysInYear(y) - m_Kernel.CountDaysInMonth(y, m) - CountDaysInYearBeforeMonth(y, m);

    #region CountDaysInYearBefore()

    [Pure]
    int ICalendricalSchemaPlus.CountDaysInYearBefore(int y, int m, int d)
    {
        // Conversion (y, m, d) -> (y, doy)
        int doy = _proxy.GetDayOfYear(y, m, d);
        return CountDaysInYearBeforeImpl(doy);
    }

    [Pure]
    int ICalendricalSchemaPlus.CountDaysInYearBefore(int y, int doy) =>
        CountDaysInYearBeforeImpl(doy);

    [Pure]
    int ICalendricalSchemaPlus.CountDaysInYearBefore(int daysSinceEpoch)
    {
        // Conversion daysSinceEpoch -> (y, ydoy)
        _ = GetYear(daysSinceEpoch, out int doy);
        return CountDaysInYearBeforeImpl(doy);
    }

    // "Natural" version, based on (y, ydoy).
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CountDaysInYearBeforeImpl(int doy) => doy - 1;

    #endregion
    #region CountDaysInYearAfter()

    [Pure]
    int ICalendricalSchemaPlus.CountDaysInYearAfter(int y, int m, int d)
    {
        // Conversion (y, m, d) -> (y, doy)
        int doy = _proxy.GetDayOfYear(y, m, d);
        return CountDaysInYearAfterImpl(y, doy);
    }

    [Pure]
    int ICalendricalSchemaPlus.CountDaysInYearAfter(int y, int doy) =>
        CountDaysInYearAfterImpl(y, doy);

    [Pure]
    int ICalendricalSchemaPlus.CountDaysInYearAfter(int daysSinceEpoch)
    {
        // Conversion daysSinceEpoch -> (y, doy)
        int y = GetYear(daysSinceEpoch, out int doy);
        return CountDaysInYearAfterImpl(y, doy);
    }

    // "Natural" version, based on (y, ydoy).
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int CountDaysInYearAfterImpl(int y, int doy) => m_Kernel.CountDaysInYear(y) - doy;

    #endregion
    #region CountDaysInMonthBefore()

    [Pure]
    int ICalendricalSchemaPlus.CountDaysInMonthBefore(int y, int m, int d) =>
        CountDaysInMonthBeforeImpl(d);

    /// <inheritdoc />
    [Pure]
    int ICalendricalSchemaPlus.CountDaysInMonthBefore(int y, int doy)
    {
        // Conversion (y, doy) -> (y, m, d)
        _ = GetMonth(y, doy, out int d);
        return CountDaysInMonthBeforeImpl(d);
    }

    [Pure]
    int ICalendricalSchemaPlus.CountDaysInMonthBefore(int daysSinceEpoch)
    {
        // Conversion daysSinceEpoch -> (y, m, d)
        _proxy.GetDateParts(daysSinceEpoch, out _, out _, out int d);
        return CountDaysInMonthBeforeImpl(d);
    }

    // "Natural" version, based on (y, m, d).
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CountDaysInMonthBeforeImpl(int d) => d - 1;

    #endregion
    #region CountDaysInMonthAfter()

    [Pure]
    int ICalendricalSchemaPlus.CountDaysInMonthAfter(int y, int m, int d) =>
        CountDaysInMonthAfterImpl(y, m, d);

    [Pure]
    int ICalendricalSchemaPlus.CountDaysInMonthAfter(int y, int doy)
    {
        // Conversion (y, doy) -> (y, m, d)
        int m = GetMonth(y, doy, out int d);
        return CountDaysInMonthAfterImpl(y, m, d);
    }

    [Pure]
    int ICalendricalSchemaPlus.CountDaysInMonthAfter(int daysSinceEpoch)
    {
        // Conversion daysSinceEpoch -> (y, m, d)
        _proxy.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        return CountDaysInMonthAfterImpl(y, m, d);
    }

    // "Natural" version, based on (y, m, d).
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int CountDaysInMonthAfterImpl(int y, int m, int d) => m_Kernel.CountDaysInMonth(y, m) - d;

    #endregion
}
