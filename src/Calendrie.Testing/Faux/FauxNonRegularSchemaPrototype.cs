﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Faux;

using Calendrie;
using Calendrie.Core;
using Calendrie.Core.Prototypes;

public sealed class FauxNonRegularSchemaPrototype : NonRegularSchemaPrototype
{
    private readonly ICalendricalCore _kernel;

    private FauxNonRegularSchemaPrototype(
        ICalendricalCore kernel,
        bool proleptic,
        int minDaysInYear,
        int minDaysInMonth)
        : base(proleptic, minDaysInYear, minDaysInMonth)
    {
        Debug.Assert(kernel != null);

        _kernel = kernel;
    }

    public static FauxNonRegularSchemaPrototype Create(ICalendricalSchema schema)
    {
        ArgumentNullException.ThrowIfNull(schema);

        if (schema.IsRegular(out _))
            throw new ArgumentException(null, nameof(schema));

        return new FauxNonRegularSchemaPrototype(
            schema,
            proleptic: schema.SupportedYears.Min < 0,
            schema.MinDaysInYear,
            schema.MinDaysInMonth)
        {
            PreValidator = schema.PreValidator
        };
    }

    public sealed override CalendricalFamily Family => _kernel.Family;
    public sealed override CalendricalAdjustments PeriodicAdjustments => _kernel.PeriodicAdjustments;

    public sealed override bool IsLeapYear(int y) => _kernel.IsLeapYear(y);
    public sealed override bool IsIntercalaryMonth(int y, int m) => _kernel.IsIntercalaryMonth(y, m);
    public sealed override bool IsIntercalaryDay(int y, int m, int d) => _kernel.IsIntercalaryDay(y, m, d);
    public sealed override bool IsSupplementaryDay(int y, int m, int d) => _kernel.IsSupplementaryDay(y, m, d);

    public sealed override int CountMonthsInYear(int y) => _kernel.CountMonthsInYear(y);
    public sealed override int CountDaysInYear(int y) => _kernel.CountDaysInYear(y);
    public sealed override int CountDaysInMonth(int y, int m) => _kernel.CountDaysInMonth(y, m);
}