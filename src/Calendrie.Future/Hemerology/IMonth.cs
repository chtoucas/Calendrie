// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

// No IAbsoluteMonth or IAffineMonth. Interconversion can only be achieved after
// converting a month to a range of days.

/// <summary>
/// Defines a calendar month.
/// </summary>
public interface IMonth
{
    /// <summary>
    /// Gets the count of consecutive months since the epoch of the calendar to
    /// which belongs the current instance.
    /// </summary>
    int MonthsSinceEpoch { get; }

    /// <summary>
    /// Gets the century of the era.
    /// </summary>
    /// <remarks>
    /// A default implementation should look like this:
    /// <code><![CDATA[
    ///   Ord CenturyOfEra => Ord.FromInt32(Century);
    /// ]]></code>
    /// </remarks>
    Ord CenturyOfEra { get; }

    /// <summary>
    /// Gets the century number.
    /// </summary>
    /// <remarks>
    /// A default implementation should look like this:
    /// <code><![CDATA[
    ///   int Century => YearNumbering.GetCentury(Year);
    /// ]]></code>
    /// </remarks>
    int Century { get; }

    /// <summary>
    /// Gets the year of the era.
    /// </summary>
    /// <remarks>
    /// A default implementation should look like this:
    /// <code><![CDATA[
    ///   Ord YearOfEra => Ord.FromInt32(Year);
    /// ]]></code>
    /// </remarks>
    Ord YearOfEra { get; }

    /// <summary>
    /// Gets the year of the century.
    /// <para>The result is in the range from 1 to 100.</para>
    /// </summary>
    /// <remarks>
    /// A default implementation should look like this:
    /// <code><![CDATA[
    ///   int YearOfCentury => YearNumbering.GetYearOfCentury(Year);
    /// ]]></code>
    /// </remarks>
    int YearOfCentury { get; }

    /// <summary>
    /// Gets the (algebraic) year number.
    /// </summary>
    int Year { get; }

    /// <summary>
    /// Gets the month of the year.
    /// </summary>
    int Month { get; }

    /// <summary>
    /// Returns <see langword="true"/> if the current instance is an intercalary
    /// month; otherwise returns <see langword="false"/>.
    /// </summary>
    bool IsIntercalary { get; }

    /// <summary>
    /// Deconstructs the current instance into its components.
    /// </summary>
    void Deconstruct(out int year, out int month);

    /// <summary>
    /// Obtains the number of whole months in the year elapsed since the start
    /// of the year and before this month instance.
    /// <para>Trivial (= Month - 1), only added for completeness.</para>
    /// </summary>
    [Pure] int CountElapsedMonthsInYear() => Month - 1;

    /// <summary>
    /// Obtains the number of whole months remaining after this month instance
    /// and until the end of the year.
    /// </summary>
    [Pure] int CountRemainingMonthsInYear();

    // REVIEW(code): CountElapsedDaysInYear() and CountRemainingDaysInYear()?
#if false
    /// <summary>
    /// Obtains the number of whole days in the year elapsed since the start of
    /// the year and before this month instance.
    /// </summary>
    [Pure] int CountElapsedDaysInYear();

    /// <summary>
    /// Obtains the number of whole days remaining after this month instance and
    /// until the end of the year.
    /// </summary>
    [Pure] int CountRemainingDaysInYear();
#endif
}

/// <summary>
/// Defines a calendar month type.
/// <para>This interface SHOULD NOT be implemented by types participating in a
/// poly-calendar system; see <see cref="IMonthBase{TSelf}"/> for a more
/// suitable interface.</para>
/// </summary>
/// <typeparam name="TSelf">The month type that implements this interface.
/// </typeparam>
public interface IMonth<TSelf> :
    IMonthBase<TSelf>,
    IMinMaxValue<TSelf>
    where TSelf : IMonth<TSelf>
{
    /// <summary>
    /// Creates a new instance of the <typeparamref name="TSelf"/> type from
    /// the specified month components.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid month or <paramref name="year"/> is outside the
    /// range of supported years.</exception>
    [Pure] static abstract TSelf Create(int year, int month);

    /// <summary>
    /// Attempts to create a new instance of the <typeparamref name="TSelf"/>
    /// type from the specified month components.
    /// </summary>
    /// <returns><see langword="true"/> if the method succeeded; otherwise,
    /// <see langword="false"/>.</returns>
    [Pure] static abstract bool TryCreate(int year, int month, [NotNullWhen(true)] out TSelf? result);

    /// <summary>
    /// Creates a new instance of the <typeparamref name="TSelf"/> type from the
    /// specified number of consecutive months since the epoch.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="monthsSinceEpoch"/>
    /// is outside the range of supported values.</exception>
    [Pure] static abstract TSelf FromMonthsSinceEpoch(int monthsSinceEpoch);
}
