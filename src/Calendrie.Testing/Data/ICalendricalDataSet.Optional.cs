﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data;

// TODO(fact): one interface per rule? Separate data for subtraction?
// To avoid having to create a dataset per rule, we could change the format and
// add roundoff.

// To be used as a provider for -ambiguous- data.
public interface IAdvancedMathDataSet
{
    /// <summary>Gets the rule employed to resolve ambiguities.</summary>
    AdditionRule AdditionRule { get; }

    /// <summary>Date, expected result, years to be added.</summary>
    DataGroup<YemodaPairAnd<int>> AddYearsData { get; }
    /// <summary>Date, expected result, months to be added.</summary>
    DataGroup<YemodaPairAnd<int>> AddMonthsData { get; }
    /// <summary>Date, expected result, years to be added.</summary>
    DataGroup<YedoyPairAnd<int>> AddYearsOrdinalData { get; }

    //DataGroup<YemoPairAnd<int>> AddYearsMonthData { get; }

    ///// <summary>Start date, end date, exact diff between.</summary>
    //DataGroup<DateDiff> DateDiffData { get; }
}

public interface IEpagomenalDataSet
{
    /// <summary>Date, epagomenal number.</summary>
    DataGroup<YemodaAnd<int>> EpagomenalDayInfoData { get; }
}
