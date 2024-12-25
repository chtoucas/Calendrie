// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Prototyping;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

// TODO(code): optimizations, idem with NonRegular... and PrototypalSchema

// WARNING: only meant to be used for rapid prototyping.
//
// For explanations, see PrototypalSchemaSlim.

/// <summary>
/// Represents a prototype for a regular schema and provides a base for derived
/// classes.
/// </summary>
public abstract partial class RegularSchemaPrototype : RegularSchema
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegularSchemaPrototype"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minDaysInYear"/>
    /// or <paramref name="minDaysInMonth"/> is a negative integer.</exception>
    protected RegularSchemaPrototype(bool proleptic, int minDaysInYear, int minDaysInMonth)
        : this(PrototypeHelpers.GetSupportedYears(proleptic), minDaysInYear, minDaysInMonth) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="RegularSchemaPrototype"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minDaysInYear"/>
    /// or <paramref name="minDaysInMonth"/> is a negative integer.</exception>
    protected RegularSchemaPrototype(
        Range<int> supportedYears, int minDaysInYear, int minDaysInMonth)
        : base(supportedYears, minDaysInYear, minDaysInMonth)
    {
        IsProleptic = supportedYears.Min < 1;
    }

    /// <summary>
    /// Returns <see langword="true"/> if the current instance is proleptic;
    /// otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsProleptic { get; }
}

public partial class RegularSchemaPrototype // Prototypal methods
{
    /// <inheritdoc />
    /// <remarks>For performance reasons, a derived class SHOULD override this
    /// method.</remarks>
    [Pure]
    public override int CountDaysInYearBeforeMonth(int y, int m)
    {
        int count = 0;
        for (int i = 1; i < m; i++)
        {
            count += CountDaysInMonth(y, i);
        }
        return count;
    }

    /// <inheritdoc />
    /// <remarks>For performance reasons, a derived class SHOULD override this
    /// method.</remarks>
    [Pure]
    public override int GetYear(int daysSinceEpoch, out int doy)
    {
        int y = 1 + MathZ.Divide(daysSinceEpoch, MinDaysInYear);
        int startOfYear = GetStartOfYear(y);

        if (daysSinceEpoch >= 0)
        {
            while (daysSinceEpoch < startOfYear)
            {
                startOfYear -= CountDaysInYear(--y);
            }
        }
        else
        {
            while (daysSinceEpoch >= startOfYear)
            {
                int startOfNextYear = startOfYear + CountDaysInYear(y);
                if (daysSinceEpoch < startOfNextYear) { break; }
                y++;
                startOfYear = startOfNextYear;
            }
            Debug.Assert(daysSinceEpoch >= startOfYear);
        }

        doy = 1 + daysSinceEpoch - startOfYear;
        return y;
    }

    /// <inheritdoc />
    /// <remarks>For performance reasons, a derived class SHOULD override this
    /// method.</remarks>
    [Pure]
    public override int GetYear(int daysSinceEpoch) => GetYear(daysSinceEpoch, out _);

    /// <inheritdoc />
    /// <remarks>For performance reasons, a derived class SHOULD override this
    /// method.</remarks>
    [Pure]
    public override int GetMonth(int y, int doy, out int d)
    {
        int m = 1;
        int daysInYearBeforeMonth = 0;

        while (m < MonthsInYear)
        {
            int daysInYearBeforeNextMonth = CountDaysInYearBeforeMonth(y, m + 1);
            if (doy <= daysInYearBeforeNextMonth) { break; }

            daysInYearBeforeMonth = daysInYearBeforeNextMonth;
            m++;
        }

        d = doy - daysInYearBeforeMonth;
        return m;
    }

    /// <inheritdoc />
    /// <remarks>For performance reasons, a derived class <b>OUGHT TO</b>
    /// override this method.</remarks>
    [Pure]
    public override int GetStartOfYear(int y)
    {
        int daysSinceEpoch = 0;

        if (y < 1)
        {
            for (int i = y; i < 1; i++)
            {
                daysSinceEpoch -= CountDaysInYear(i);
            }
        }
        else
        {
            for (int i = 1; i < y; i++)
            {
                daysSinceEpoch += CountDaysInYear(i);
            }
        }

        return daysSinceEpoch;
    }
}
