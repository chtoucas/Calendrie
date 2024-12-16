// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

public static class OffsettedSchemaPlus
{
    public static OffsettedSchemaPlus<TSchema> Create<TSchema>(int offset)
        where TSchema : ICalendricalSchemaPlus, ISchemaActivator<TSchema>
    {
        return new OffsettedSchemaPlus<TSchema>(TSchema.CreateInstance(), offset);
    }
}

public partial class OffsettedSchemaPlus<TSchema> : OffsettedSchema<TSchema>, ICalendricalSchemaPlus
    where TSchema : ICalendricalSchemaPlus
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OffsettedSchemaPlus{TSchema}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.
    /// </exception>
    public OffsettedSchemaPlus(TSchema schema, int offset) : base(schema, offset) { }

    /// <inheritdoc />
    [Pure]
    public int CountDaysInYearAfterMonth(int y, int m) =>
        Schema.CountDaysInYearAfterMonth(y - Offset, m);

    #region CountDaysInYearBefore()

    /// <inheritdoc />
    [Pure]
    public int CountDaysInYearBefore(int y, int m, int d) =>
        Schema.CountDaysInYearBefore(y - Offset, m, d);

    /// <inheritdoc />
    [Pure]
    public int CountDaysInYearBefore(int y, int doy) =>
        Schema.CountDaysInYearBefore(y - Offset, doy);

    /// <inheritdoc />
    [Pure]
    public int CountDaysInYearBefore(int daysSinceEpoch) => throw new NotImplementedException();

    #endregion
    #region CountDaysInYearAfter()

    /// <inheritdoc />
    [Pure]
    public int CountDaysInYearAfter(int y, int m, int d) =>
        Schema.CountDaysInYearAfter(y - Offset, m, d);

    /// <inheritdoc />
    [Pure]
    public int CountDaysInYearAfter(int y, int doy) => Schema.CountDaysInYearAfter(y - Offset, doy);

    /// <inheritdoc />
    [Pure]
    public int CountDaysInYearAfter(int daysSinceEpoch) => throw new NotImplementedException();

    #endregion
    #region CountDaysInMonthBefore()

    /// <inheritdoc />
    [Pure]
    public int CountDaysInMonthBefore(int y, int m, int d) =>
        Schema.CountDaysInMonthBefore(y - Offset, m, d);

    /// <inheritdoc />
    [Pure]
    public int CountDaysInMonthBefore(int y, int doy) =>
        Schema.CountDaysInMonthBefore(y - Offset, doy);

    /// <inheritdoc />
    [Pure]
    public int CountDaysInMonthBefore(int daysSinceEpoch) => throw new NotImplementedException();

    #endregion
    #region CountDaysInMonthAfter()

    /// <inheritdoc />
    [Pure]
    public int CountDaysInMonthAfter(int y, int m, int d) =>
        Schema.CountDaysInMonthAfter(y - Offset, m, d);

    /// <inheritdoc />
    [Pure]
    public int CountDaysInMonthAfter(int y, int doy) =>
        Schema.CountDaysInMonthAfter(y - Offset, doy);

    /// <inheritdoc />
    [Pure]
    public int CountDaysInMonthAfter(int daysSinceEpoch) => throw new NotImplementedException();

    #endregion
}
