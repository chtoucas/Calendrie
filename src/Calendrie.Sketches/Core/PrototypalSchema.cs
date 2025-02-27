﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//#define ENABLE_OPTIONAL_METHODS

#pragma warning disable CA1033 // Interface methods should be callable by child types

namespace Calendrie.Core;

using Calendrie.Core.Intervals;
using Calendrie.Core.Validation;

#region Developer Notes

// The code is NOT meant to be efficient.
//
// Usefulness? ICalendricalCore is easy to implement and test (see for instance
// GregorianKernel), which might not be the case of ICalendricalSchema.
// With PrototypalSchema, we can then
// - quickly prototype a new schema
// - validate test data independently
// At the same time, it demonstrates that an ICalendricalCore instance is all we
// need to build a full schema.
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
// ICalendricalCore.
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
/// Represents a prototypal implementation of the <see cref="ICalendricalSchema"/>
/// interface.
/// </summary>
public partial class PrototypalSchema : ICalendricalCore, ICalendricalSchema
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
    protected readonly ICalendricalCore m_Kernel;

    protected readonly int m_MinDaysInYear;

    protected readonly int m_MinDaysInMonth;
#pragma warning restore IDE1006
#pragma warning restore CA1051

    private Segment<int> _supportedYears;

    /// <summary>
    /// Initializes a new instance of the <see cref="PrototypalSchema"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.
    /// </exception>
    public PrototypalSchema(ICalendricalSchema schema)
    {
        ArgumentNullException.ThrowIfNull(schema, nameof(schema));

        m_Kernel = schema;
        m_MinDaysInYear = schema.MinDaysInYear;
        m_MinDaysInMonth = schema.MinDaysInMonth;

        IsProleptic = schema.SupportedYears.Min < 1;
        _supportedYears = PrototypeHelpers.GetSupportedYears(IsProleptic);

        PreValidator = schema.PreValidator;

        _proxy = new SchemaProxy(this);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PrototypalSchema"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="kernel"/> is null.
    /// </exception>
    public PrototypalSchema(
        ICalendricalCore kernel, bool proleptic, int minDaysInYear, int minDaysInMonth)
    {
        ArgumentNullException.ThrowIfNull(kernel, nameof(kernel));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(minDaysInYear, 0, nameof(minDaysInYear));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(minDaysInMonth, 0, nameof(minDaysInMonth));

        Debug.Assert(minDaysInYear > minDaysInMonth);

        m_Kernel = kernel;
        m_MinDaysInYear = minDaysInYear;
        m_MinDaysInMonth = minDaysInMonth;

        IsProleptic = proleptic;
        _supportedYears = PrototypeHelpers.GetSupportedYears(proleptic);

        PreValidator = new PlainPreValidator(this);

        _proxy = new SchemaProxy(this);
    }

    public bool IsProleptic { get; private init; }

    // Another solution could have been to cast "this" to ICalendricalSchema.
    private sealed class SchemaProxy
    {
        private readonly ICalendricalSchema _this;

        public SchemaProxy(PrototypalSchema @this)
        {
            Debug.Assert(@this != null);
            _this = @this;
        }

#if ENABLE_OPTIONAL_METHODS
        /// <summary>Conversion (y, m, d) -> (y, doy).</summary>
        [Pure]
        public int GetDayOfYear(int y, int m, int d) => _this.GetDayOfYear(y, m, d);

        /// <summary>Conversion daysSinceEpoch -> (y, m, d).</summary>
        public void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d) =>
            _this.GetDateParts(daysSinceEpoch, out y, out m, out d);
#endif

        [Pure]
        public int GetEndOfYear(int y) => _this.GetEndOfYear(y);

        [Pure]
        public int GetEndOfYearInMonths(int y) => _this.GetEndOfYearInMonths(y);
    }
}

public partial class PrototypalSchema // ICalendricalCore
{
    CalendricalAlgorithm ICalendricalCore.Algorithm => m_Kernel.Algorithm;

    CalendricalFamily ICalendricalCore.Family => m_Kernel.Family;

    CalendricalAdjustments ICalendricalCore.PeriodicAdjustments => m_Kernel.PeriodicAdjustments;

    [Pure]
    bool ICalendricalCore.IsRegular(out int monthsInYear) => m_Kernel.IsRegular(out monthsInYear);

    [Pure]
    bool ICalendricalCore.IsLeapYear(int y) => m_Kernel.IsLeapYear(y);

    [Pure]
    bool ICalendricalCore.IsIntercalaryMonth(int y, int m) => m_Kernel.IsIntercalaryMonth(y, m);

    [Pure]
    bool ICalendricalCore.IsIntercalaryDay(int y, int m, int d) => m_Kernel.IsIntercalaryDay(y, m, d);

    [Pure]
    bool ICalendricalCore.IsSupplementaryDay(int y, int m, int d) => m_Kernel.IsSupplementaryDay(y, m, d);

    [Pure]
    int ICalendricalCore.CountMonthsInYear(int y) => m_Kernel.CountMonthsInYear(y);

    [Pure]
    int ICalendricalCore.CountDaysInYear(int y) => m_Kernel.CountDaysInYear(y);

    [Pure]
    int ICalendricalCore.CountDaysInMonth(int y, int m) => m_Kernel.CountDaysInMonth(y, m);
}

public partial class PrototypalSchema // ICalendricalSchema (1)
{
    private Segment<int>? _supportedDays;
    /// <inheritdoc />
    public Segment<int> SupportedDays =>
        _supportedDays ??= new Segment<int>(
            SupportedYears.Endpoints.Select(
                GetStartOfYear,
                _proxy.GetEndOfYear));

    private Segment<int>? _supportedMonths;
    /// <inheritdoc />
    public Segment<int> SupportedMonths =>
        _supportedMonths ??=
        new Segment<int>(
            SupportedYears.Endpoints.Select(
                GetStartOfYearInMonths,
                _proxy.GetEndOfYearInMonths));

    /// <inheritdoc />
    //
    // We limit the range of supported years because the default impl of
    // GetYear() and GetStartOfYear() are extremely slow if the values of
    // "y" or "daysSinceEpoch" are big.
    // Only override this property if both methods can handle big values
    // efficiently.
    public Segment<int> SupportedYears
    {
        get => _supportedYears;
        init
        {
            IsProleptic = value.Min < 1;
            _supportedYears = value;
        }
    }

    /// <inheritdoc />
    /// <remarks>For performance reasons, a derived class SHOULD override this
    /// method.</remarks>
    [Pure]
    public virtual int CountDaysInYearBeforeMonth(int y, int m)
    {
        // Faster alternatives:
        // - Use a purely computational formula.
        // - Pre-compute the result and use an array lookup.
        //   A quick improvement would be to use GetDaysInMonths(leapYear)
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
    /// <remarks>For performance reasons, a derived class SHOULD override this
    /// method.</remarks>
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
    /// <remarks>For performance reasons, a derived class <b>OUGHT TO</b>
    /// override this method.</remarks>
    [Pure]
    public virtual int GetYear(int daysSinceEpoch, out int doy)
    {
        // Faster alternatives:
        // - Use a purely computational formula.
        // - Start with an approximation of the result, see PrototypalSchemaSlim.

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
    /// <remarks>For performance reasons, a derived class SHOULD override this
    /// method.</remarks>
    [Pure]
    public virtual int GetMonth(int y, int doy, out int d)
    {
        // Faster alternatives:
        // - Use a purely computational formula.
        // - Start with an approximation of the result, see PrototypalSchemaSlim.

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
    /// <remarks>For performance reasons, a derived class <b>OUGHT TO</b>
    /// override this method.</remarks>
    [Pure]
    public virtual int GetStartOfYearInMonths(int y)
    {
        // Faster alternatives:
        // - Use a purely computational formula.
        // - Cache the result.

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
    /// <remarks>For performance reasons, a derived class <b>OUGHT TO</b>
    /// override this method.</remarks>
    [Pure]
    public virtual int GetStartOfYear(int y)
    {
        // Faster alternatives:
        // - Use a purely computational formula.
        // - Cache the result.

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

    public virtual ICalendricalPreValidator PreValidator { get; init; }

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
    int ICalendricalSchema.GetYear(int daysSinceEpoch) => GetYear(daysSinceEpoch, out _);

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

#if ENABLE_OPTIONAL_METHODS

public partial class PrototypalSchema //
{
    /// <summary>
    /// Obtains the number of whole days remaining after the specified month and
    /// until the end of the year.
    /// </summary>
    [Pure]
    public int CountDaysInYearAfterMonth(int y, int m) =>
        m_Kernel.CountDaysInYear(y)
        - m_Kernel.CountDaysInMonth(y, m)
        - CountDaysInYearBeforeMonth(y, m);

    #region CountDaysInYearBefore()

    /// <summary>
    /// Obtains the number of whole days elapsed since the start of the year and
    /// before the specified day.
    /// <para>The result should match the value of <c>(DayOfYear - 1)</c>.</para>
    /// </summary>
    [Pure]
    public int CountDaysInYearBefore(int y, int m, int d)
    {
        // Conversion (y, m, d) -> (y, doy)
        int doy = _proxy.GetDayOfYear(y, m, d);
        return doy - 1;
    }

    /// <summary>
    /// Obtains the number of whole days elapsed since the start of the year and
    /// before the specified day.
    /// <para>Trivial (<c>= <paramref name="doy"/> - 1</c>), only added for
    /// completeness.</para>
    /// </summary>
    [Pure]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Static would force us to validate the parameters")]
    public int CountDaysInYearBefore(int y, int doy) => doy - 1;

    /// <summary>
    /// Obtains the number of whole days elapsed since the start of the year and
    /// before the specified day.
    /// <para>The result should match the value of <c>(DayOfYear - 1)</c>.</para>
    /// </summary>
    [Pure]
    public int CountDaysInYearBefore(int daysSinceEpoch)
    {
        // Conversion daysSinceEpoch -> (y, ydoy)
        _ = GetYear(daysSinceEpoch, out int doy);
        return doy - 1;
    }

    #endregion
    #region CountDaysInYearAfter()

    /// <summary>
    /// Obtains the number of whole days remaining after the specified date and
    /// until the end of the year.
    /// </summary>
    [Pure]
    public int CountDaysInYearAfter(int y, int m, int d)
    {
        // Conversion (y, m, d) -> (y, doy)
        int doy = _proxy.GetDayOfYear(y, m, d);
        return CountDaysInYearAfterImpl(y, doy);
    }

    /// <summary>
    /// Obtains the number of whole days remaining after the specified date and
    /// until the end of the year.
    /// </summary>
    [Pure]
    public int CountDaysInYearAfter(int y, int doy) => CountDaysInYearAfterImpl(y, doy);

    /// <summary>
    /// Obtains the number of whole days remaining after the specified date and
    /// until the end of the year.
    /// </summary>
    [Pure]
    public int CountDaysInYearAfter(int daysSinceEpoch)
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

    /// <summary>
    /// Obtains the number of whole days elapsed since the start of the month
    /// and before the specified day.
    /// <para>Trivial (<c>= <paramref name="d"/> - 1</c>), only added for
    /// completeness.</para>
    /// </summary>
    [Pure]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Static would force us to validate the parameters")]
    public int CountDaysInMonthBefore(int y, int m, int d) => d - 1;

    /// <summary>
    /// Obtains the number of whole days elapsed since the start of the month
    /// and before the specified day.
    /// <para>The result should match the value of <c>(Day - 1)</c>.</para>
    /// </summary>
    [Pure]
    public int CountDaysInMonthBefore(int y, int doy)
    {
        // Conversion (y, doy) -> (y, m, d)
        _ = GetMonth(y, doy, out int d);
        return d - 1;
    }

    /// <summary>
    /// Obtains the number of whole days elapsed since the start of the month
    /// and before the specified day.
    /// <para>The result should match the value of <c>(Day - 1)</c>.</para>
    /// </summary>
    [Pure]
    public int CountDaysInMonthBefore(int daysSinceEpoch)
    {
        // Conversion daysSinceEpoch -> (y, m, d)
        _proxy.GetDateParts(daysSinceEpoch, out _, out _, out int d);
        return d - 1;
    }

    #endregion
    #region CountDaysInMonthAfter()

    /// <summary>
    /// Obtains the number of whole days remaining after the specified date and
    /// until the end of the month.
    /// </summary>
    [Pure]
    public int CountDaysInMonthAfter(int y, int m, int d) => CountDaysInMonthAfterImpl(y, m, d);

    /// <summary>
    /// Obtains the number of whole days remaining after the specified date and
    /// until the end of the month.
    /// </summary>
    [Pure]
    public int CountDaysInMonthAfter(int y, int doy)
    {
        // Conversion (y, doy) -> (y, m, d)
        int m = GetMonth(y, doy, out int d);
        return CountDaysInMonthAfterImpl(y, m, d);
    }

    /// <summary>
    /// Obtains the number of whole days remaining after the specified date and
    /// until the end of the month.
    /// </summary>
    [Pure]
    public int CountDaysInMonthAfter(int daysSinceEpoch)
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

#endif
