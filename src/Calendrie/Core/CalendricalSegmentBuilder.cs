// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;
using Calendrie.Core.Validation;

using Endpoint = CalendricalSegment.Endpoint;

// We validate before and after calling a method from the method:
// - before, to respect the schema layout (_supportedYears)
// - after, to stay within the limits of Yemoda/Yedoy (_partsFactory)

/// <summary>
/// Represents a builder for <see cref="CalendricalSegment"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class CalendricalSegmentBuilder
{
    /// <summary>
    /// Represents the schema.
    /// </summary>
    private readonly ICalendricalSchema _schema;

    /// <summary>
    /// Represents the adapter for calendrical parts.
    /// </summary>
    private readonly PartsAdapter _partsAdapter;

    /// <summary>
    /// Initializes a new instance of the <see cref="CalendricalSegmentBuilder"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    public CalendricalSegmentBuilder(ICalendricalSchema schema)
    {
        ArgumentNullException.ThrowIfNull(schema);

        _schema = schema;
        _partsAdapter = new PartsAdapter(schema);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the minimum has been set; otherwise
    /// returns <see langword="false"/>.
    /// </summary>
    public bool HasMin => _min != null;

    /// <summary>
    /// Returns <see langword="true"/> if the maximum has been set; otherwise
    /// returns <see langword="false"/>.
    /// </summary>
    public bool HasMax => _max != null;

    /// <summary>
    /// Returns <see langword="true"/> if both minimum and maximum have been set;
    /// otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsBuildable => HasMin && HasMax;

    private Endpoint? _min;
    /// <summary>
    /// Gets the minimum of the segment.
    /// </summary>
    /// <exception cref="InvalidOperationException">The minimum was not set.
    /// </exception>
    private Endpoint Min
    {
        get => _min ?? throw new InvalidOperationException();
        set
        {
            if (Endpoint.IsGreaterThan(value, _max))
                throw new ArgumentOutOfRangeException(nameof(value));
            _min = value;
        }
    }

    private Endpoint? _max;
    /// <summary>
    /// Gets the maximum of the segment.
    /// </summary>
    /// <exception cref="InvalidOperationException">The maximum was not set.
    /// </exception>
    private Endpoint Max
    {
        get => _max ?? throw new InvalidOperationException();
        set
        {
            if (Endpoint.IsGreaterThan(_min, value))
                throw new ArgumentOutOfRangeException(nameof(value));
            _max = value;
        }
    }

    /// <summary>
    /// Gets the pre-validator for this schema.
    /// </summary>
    private ICalendricalPreValidator PreValidator => _schema.PreValidator;

    /// <summary>
    /// Builds the segment.
    /// </summary>
    /// <exception cref="InvalidOperationException">The segment was not buildable.
    /// </exception>
    [Pure]
    public CalendricalSegment BuildSegment()
    {
        var min = fixEndpoint(Min);
        var max = fixEndpoint(Max);

        return new CalendricalSegment(_schema, min, max)
        {
            MinIsStartOfYear = isStartOfYear(min),
            MaxIsEndOfYear = isEndOfYear(max)
        };

        [Pure]
        bool isStartOfYear(Endpoint ep) =>
            ep.OrdinalParts == OrdinalParts.AtStartOfYear(ep.Year);

        [Pure]
        bool isEndOfYear(Endpoint ep) =>
            ep.OrdinalParts == _partsAdapter.GetOrdinalPartsAtEndOfYear(ep.Year);

        [Pure]
        Endpoint fixEndpoint(Endpoint ep)
        {
            var (y, m) = ep.MonthParts;
            ep.MonthsSinceEpoch = _schema.CountMonthsSinceEpoch(y, m);
            return ep;
        }
    }
}

public partial class CalendricalSegmentBuilder // Builder methods
{
    /// <summary>
    /// Gets or sets the minimum.
    /// <para>The setter automatically update <see cref="MinDateParts"/> and
    /// <see cref="MinOrdinalParts"/>.</para>
    /// </summary>
    /// <value>The minimal number of consecutive days from the epoch.</value>
    /// <exception cref="InvalidOperationException">(Getter) The minimum is not
    /// set.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(Setter) The specified
    /// number of consecutive days from the epoch is outside the range of
    /// supported values by the schema.</exception>
    public int MinDaysSinceEpoch
    {
        get => Min.DaysSinceEpoch;
        set => Min = GetEndpointFromDaysSinceEpoch(value);
    }

    /// <summary>
    /// Gets or sets the maximum.
    /// <para>The setter automatically update <see cref="MaxDateParts"/> and
    /// <see cref="MaxOrdinalParts"/>.</para>
    /// </summary>
    /// <value>The maximal number of consecutive days from the epoch.</value>
    /// <exception cref="InvalidOperationException">(Getter) The maximum is not
    /// set.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(Setter) The specified
    /// number of consecutive days from the epoch is outside the range of
    /// supported values by the schema.</exception>
    public int MaxDaysSinceEpoch
    {
        get => Max.DaysSinceEpoch;
        set => Max = GetEndpointFromDaysSinceEpoch(value);
    }

    /// <summary>
    /// Gets or sets the minimum.
    /// <para>The setter automatically update <see cref="MinDaysSinceEpoch"/>
    /// and <see cref="MinOrdinalParts"/>.</para>
    /// </summary>
    /// <value>The minimal value of a <see cref="DateParts"/>.</value>
    /// <exception cref="InvalidOperationException">(Getter) The minimum is not
    /// set.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(Setter) The specified date
    /// parts are invalid or outside the range of supported values by the schema.
    /// </exception>
    public DateParts MinDateParts
    {
        get => Min.DateParts;
        set => Min = GetEndpoint(value);
    }

    /// <summary>
    /// Gets or sets the maximum.
    /// <para>The setter automatically update <see cref="MaxDaysSinceEpoch"/>
    /// and <see cref="MaxOrdinalParts"/>.</para>
    /// </summary>
    /// <value>The maximal value of a <see cref="DateParts"/>.</value>
    /// <exception cref="InvalidOperationException">(Getter) The maximum is not
    /// set.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(Setter) The specified date
    /// parts are invalid or outside the range of supported values by the schema.
    /// </exception>
    public DateParts MaxDateParts
    {
        get => Max.DateParts;
        set => Max = GetEndpoint(value);
    }

    /// <summary>
    /// Gets or sets the minimum.
    /// <para>The setter automatically update <see cref="MinDaysSinceEpoch"/>
    /// and <see cref="MinDateParts"/>.</para>
    /// </summary>
    /// <value>The minimal value of an <see cref="OrdinalParts"/>.</value>
    /// <exception cref="InvalidOperationException">(Getter) The minimum is not
    /// set.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(Setter) The specified
    /// ordinal date parts are invalid or outside the range of supported values
    /// by the schema.</exception>
    public OrdinalParts MinOrdinalParts
    {
        get => Min.OrdinalParts;
        set => Min = GetEndpoint(value);
    }

    /// <summary>
    /// Gets or sets the maximum.
    /// <para>The setter automatically update <see cref="MaxDaysSinceEpoch"/>
    /// and <see cref="MaxDateParts"/>.</para></summary>
    /// <value>The maximal value of an <see cref="OrdinalParts"/>.</value>
    /// <exception cref="InvalidOperationException">(Getter) The maximum is not
    /// set.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(Setter) The specified
    /// ordinal date parts are invalid or outside the range of supported values
    /// by the schema.</exception>
    public OrdinalParts MaxOrdinalParts
    {
        get => Max.OrdinalParts;
        set => Max = GetEndpoint(value);
    }

    /// <summary>
    /// Sets the minimum to the start of the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of supported values by the schema.</exception>
    public void SetMinToStartOfYear(int year)
    {
        ValidateYear(year, nameof(year));

        Min = GetEndpointAtStartOfYear(year);
    }

    /// <summary>
    /// Sets the minimum to the start of the earliest supported year.
    /// </summary>
    public void SetMinToStartOfMinSupportedYear() =>
        Min = GetEndpointAtStartOfYear(_schema.SupportedYears.Min);

    /// <summary>
    /// Attempts to set the minimum to the start of the earliest supported year
    /// &gt;= 1.
    /// </summary>
    [Pure]
    public bool TrySetMinToStartOfMinSupportedYearOnOrAfterYear1()
    {
        var set = Interval.Intersect(_schema.SupportedYears, Range.StartingAt(1));
        if (set.IsEmpty) return false;

        Min = GetEndpointAtStartOfYear(set.Range.Min);
        return true;
    }

    /// <summary>
    /// Sets the maximum to the end of the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of supported values by the schema.</exception>
    public void SetMaxToEndOfYear(int year)
    {
        ValidateYear(year, nameof(year));

        Max = GetEndpointAtEndOfYear(year);
    }

    /// <summary>
    /// Sets the maximum to the end of the latest supported year.
    /// </summary>
    public void SetMaxToEndOfMaxSupportedYear() =>
        Max = GetEndpointAtEndOfYear(_schema.SupportedYears.Max);

    // This method throw an ArgumentException not an ArgumentOutOfRangeException,
    // therefore it's not equivalent to set Min and Max separately.
    internal void SetSupportedYears(Range<int> supportedYears)
    {
        if (!supportedYears.IsSubsetOf(_schema.SupportedYears))
            throw new ArgumentException(null, nameof(supportedYears));

        Min = GetEndpointAtStartOfYear(supportedYears.Min);
        Max = GetEndpointAtEndOfYear(supportedYears.Max);
    }

    //
    // Private helpers
    //

    private void ValidateDaysSinceEpoch(int daysSinceEpoch, string paramName)
    {
        var range = _schema.SupportedDays;
        if (daysSinceEpoch < range.Min || daysSinceEpoch > range.Max)
        {
            throw new ArgumentOutOfRangeException(
                paramName,
                daysSinceEpoch,
                $"The value was out of range; value = {daysSinceEpoch}.");
        }
    }

    private void ValidateYear(int year, string? paramName = null)
    {
        var range = _schema.SupportedYears;
        if (year < range.Min || year > range.Max)
            ThrowHelpers.ThrowYearOutOfRange(year, paramName);
    }

    [Conditional("DEBUG"), ExcludeFromCodeCoverage]
    private void __ValidateYear(int year) => ValidateYear(year);

    [Pure]
    private Endpoint GetEndpointAtStartOfYear(int year)
    {
        __ValidateYear(year);

        return new Endpoint
        {
            DaysSinceEpoch = _schema.GetStartOfYear(year),
            DateParts = DateParts.AtStartOfYear(year),
            OrdinalParts = OrdinalParts.AtStartOfYear(year),
        };
    }

    [Pure]
    private Endpoint GetEndpointAtEndOfYear(int year)
    {
        __ValidateYear(year);

        return new Endpoint
        {
            DaysSinceEpoch = _schema.GetEndOfYear(year),
            DateParts = _partsAdapter.GetDatePartsAtEndOfYear(year),
            OrdinalParts = _partsAdapter.GetOrdinalPartsAtEndOfYear(year),
        };
    }

    [Pure]
    private Endpoint GetEndpointFromDaysSinceEpoch(int value)
    {
        ValidateDaysSinceEpoch(value, nameof(value));

        return new Endpoint
        {
            DaysSinceEpoch = value,
            DateParts = _partsAdapter.GetDateParts(value),
            OrdinalParts = _partsAdapter.GetOrdinalParts(value),
        };
    }

    [Pure]
    private Endpoint GetEndpoint(DateParts value)
    {
        var (y, m, d) = value;
        ValidateYear(y, nameof(value));
        PreValidator.ValidateMonthDay(y, m, d, nameof(value));

        return new Endpoint
        {
            DaysSinceEpoch = _schema.CountDaysSinceEpoch(y, m, d),
            DateParts = value,
            OrdinalParts = _partsAdapter.GetOrdinalParts(y, m, d),
        };
    }

    [Pure]
    private Endpoint GetEndpoint(OrdinalParts value)
    {
        var (y, doy) = value;
        ValidateYear(y, nameof(value));
        PreValidator.ValidateDayOfYear(y, doy, nameof(value));

        return new Endpoint
        {
            DaysSinceEpoch = _schema.CountDaysSinceEpoch(y, doy),
            DateParts = _partsAdapter.GetDateParts(y, doy),
            OrdinalParts = value,
        };
    }
}
