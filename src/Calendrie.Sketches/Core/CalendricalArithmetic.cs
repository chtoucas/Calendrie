// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using Calendrie.Core.Arithmetic;
using Calendrie.Core.Intervals;
using Calendrie.Core.Validation;

// TODO(doc): Explain why we require ranges of complete years.
// YearsValidator, IYearsValidator or RangeValidator? Leaning towards
// RangeValidator as the exn messages are not tied to Dates.
// Improve PlainArithmetic.
// Custom version for the Gregorian/Julian schema?

#region Developer Notes

// Types Derived from SystemArithmetic
// -----------------------------------
//
// SystemArithmetic [A]  (SystemSchema)
// ├─ GregorianSystemArithmetic     (GregorianSchema)
// ├─ LunarSystemArithmetic         (-)
// ├─ LunisolarSystemArithmetic     (-)
// ├─ PlainSystemArithmetic         (-)
// ├─ RegularSystemArithmetic       (-)
// └─ SolarSystemArithmetic [A]     (-)
//    ├─ Solar12SystemArithmetic    (-)
//    └─ Solar13SystemArithmetic    (-)
//
// Annotation: [A] = abstract
//
// Comments
// --------
// SystemArithmetic is more naturally part of SystemSchema but
// the code being the same for very different types of schemas, adding the
// members of this interface to SystemSchema would lead to a lot of
// duplications. Therefore this is just an implementation detail and one
// should really use the public property ICalendricalSchema.Arithmetic.
//
// An implementation of SystemArithmetic should follow the rules of
// ICalendricalSchema: no overflow, lenient methods, same range of years,
// etc.
//
// All methods assume that a Yemoda (Yemo, or Yedoy) input forms a valid
// object for the underlying schema.

#endregion

/// <summary>
/// Defines the core mathematical operations on dates and months, and provides
/// a base for derived classes.
/// <para>Operations are <i>lenient</i>, they assume that their parameters are
/// valid from a calendrical point of view. They MUST ensure that all returned
/// values are valid when the previous condition is met.</para>
/// </summary>
public abstract class CalendricalArithmetic
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="CalendricalArithmetic"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="supportedYears"/> is
    /// NOT a subinterval of the range of supported years by <paramref name="schema"/>.
    /// </exception>
    protected CalendricalArithmetic(LimitSchema schema, Range<int> supportedYears)
    {
        ArgumentNullException.ThrowIfNull(schema);

        Schema = schema;
        var seg = CalendricalSegment.Create(schema, supportedYears);

        YearsValidator = new RangeValidator(supportedYears);
        MonthsSinceEpochValidator = new RangeValidator(seg.SupportedMonths);
    }

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    protected LimitSchema Schema { get; }

    /// <summary>
    /// Gets the validator for the  range of supported months.
    /// </summary>
    protected RangeValidator MonthsSinceEpochValidator { get; }

    /// <summary>
    /// Gets the validator for the  range of supported years.
    /// </summary>
    protected RangeValidator YearsValidator { get; }

    /// <summary>
    /// Creates the default arithmetic object for the specified schema and range
    /// of supported years.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="supportedYears"/>
    /// is NOT a subinterval of the range of supported years by <paramref name="schema"/>.
    /// </exception>
    [Pure]
    public static CalendricalArithmetic CreateDefault(LimitSchema schema, Range<int> supportedYears)
    {
        ArgumentNullException.ThrowIfNull(schema);

        return schema.IsRegular(out _)
            ? new RegularArithmetic(schema, supportedYears)
            : new PlainArithmetic(schema, supportedYears);
    }

    //
    // Non-standard operations on Yemoda
    //

    /// <summary>
    /// Adds a number of years to the year field of the specified date.
    /// </summary>
    /// <returns>The end of the target month (resp. year) when the naive result
    /// is not a valid day (resp. month).</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported values.</exception>
    [Pure] public abstract Yemoda AddYears(Yemoda ymd, int years, out int roundoff);

    /// <summary>
    /// Adds a number of months to the specified date.
    /// </summary>
    /// <returns>The last day of the month when the naive result is not a valid
    /// day (roundoff > 0).</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported values.</exception>
    [Pure] public abstract Yemoda AddMonths(Yemoda ymd, int months, out int roundoff);

    //
    // Standard operations on Yemo
    //

    /// <summary>
    /// Adds a number of months to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported values.</exception>
    [Pure] public abstract Yemo AddMonths(Yemo ym, int months);

    /// <summary>
    /// Counts the number of months between the two specified months.
    /// </summary>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "F# & VB.NET End statement.")]
    [Pure] public abstract int CountMonthsBetween(Yemo start, Yemo end);
}
