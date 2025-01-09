// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using Calendrie.Core.Validation;

#region Developer Notes

// Types Implementing ICalendricalPreValidator
// -------------------------------------------
//
// ICalendricalPreValidator                                 PUBLIC
// ├─ GregorianPreValidator         (Gregorian-only)
// ├─ JulianPreValidator            (Julian-only)
// ├─ LunarPreValidator             (CalendricalSchema)
// ├─ LunisolarPreValidator         (CalendricalSchema)
// ├─ PaxPreValidator               (Pax-only)
// ├─ PlainPreValidator             (ICalendricalSchema)
// ├─ Solar12PreValidator           (CalendricalSchema)
// └─ Solar13PreValidator           (CalendricalSchema)
//
// Comments
// --------
// ICalendricalPreValidator is more naturally part of ICalendricalSchema but
// the code being the same for very different types of schemas, adding the
// members of this interface to ICalendricalSchema would lead to a lot of
// duplications. Therefore this is just an implementation detail and one
// should really use the public property ICalendricalSchema.PreValidator.

#endregion

/// <summary>
/// Provides methods to check the well-formedness of calendrical data.
/// </summary>
public interface ICalendricalPreValidator
{
    //
    // Soft validation
    //

    /// <summary>
    /// Checks whether the of the specified month of the year is well-formed or
    /// not.
    /// <para>For regular calendars, it's advisable to write the validation
    /// <i>in situ</i>.</para>
    /// <para>This method does NOT check <paramref name="y"/>.</para>
    /// </summary>
    bool CheckMonth(int y, int month);

    /// <summary>
    /// Validates whether the specified month of the year and day of the month
    /// are well-formed or not.
    /// <para>This method does NOT check <paramref name="y"/>.</para>
    /// </summary>
    bool CheckMonthDay(int y, int month, int day);

    /// <summary>
    /// Validates whether the specified day of the year is well-formed or not.
    /// <para>This method does NOT check <paramref name="y"/>.</para>
    /// </summary>
    bool CheckDayOfYear(int y, int dayOfYear);

    //
    // Hard validation
    //

    /// <summary>
    /// Validates the well-formedness of the specified month of the year.
    /// <para>For regular calendars, it's advisable to write the validation
    /// <i>in situ</i>.</para>
    /// <para>This method does NOT validate <paramref name="y"/>.</para>
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    void ValidateMonth(int y, int month, string? paramName = null);

    /// <summary>
    /// Validates the well-formedness of the specified month of the year and day
    /// of the month.
    /// <para>This method does NOT validate <paramref name="y"/>.</para>
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    void ValidateMonthDay(int y, int month, int day, string? paramName = null);

    /// <summary>
    /// Validates the well-formedness of the specified day of the year.
    /// <para>This method does NOT validate <paramref name="y"/>.</para>
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    void ValidateDayOfYear(int y, int dayOfYear, string? paramName = null);

    /// <summary>
    /// Validates the well-formedness of the specified day of the month.
    /// <para>This method does NOT validate <paramref name="y"/> and
    /// <paramref name="m"/>.</para>
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    void ValidateDayOfMonth(int y, int m, int day, string? paramName = null);

    //
    // Static factory method
    //

    /// <summary>
    /// Creates the default <see cref="ICalendricalPreValidator"/> for the
    /// specified schema.
    /// <para>This method does not necessarily return the same pre-validator as
    /// the property <see cref="ICalendricalSchema.PreValidator"/>. Each schema
    /// may decide to use a more specialized pre-validator.</para>
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    [Pure]
    public static ICalendricalPreValidator CreateDefault(CalendricalSchema schema)
    {
        ArgumentNullException.ThrowIfNull(schema);

        return schema.Profile switch
        {
            CalendricalProfile.Solar12 => new Solar12PreValidator(schema),
            CalendricalProfile.Solar13 => new Solar13PreValidator(schema),
            CalendricalProfile.Lunar => new LunarPreValidator(schema),
            CalendricalProfile.Lunisolar => new LunisolarPreValidator(schema),
            // The fall-back case should be unreachable. Anyway, even if it is
            // not a valid profile, the plain validator will still work.
            CalendricalProfile.Other or _ => new PlainPreValidator(schema)
        };
    }
}
