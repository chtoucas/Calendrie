// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

// REVIEW(code): add CountDaysInInternationalFixedMonth() and CountDaysInPositivistMonth()?

public partial struct Armenian13Month // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current month instance is virtual;
    /// otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsVirtual => Month == Egyptian13Schema.VirtualMonth;
}

public partial struct Coptic13Month // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current month instance is virtual;
    /// otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsVirtual => Month == Coptic13Schema.VirtualMonth;
}

public partial struct Egyptian13Month // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current month instance is virtual;
    /// otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsVirtual => Month == Egyptian13Schema.VirtualMonth;
}

public partial struct Ethiopic13Month // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current month instance is virtual;
    /// otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsVirtual => Month == Coptic13Schema.VirtualMonth;
}

public partial struct FrenchRepublican13Month // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current month instance is virtual;
    /// otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsVirtual => Month == FrenchRepublican13Schema.VirtualMonth;
}

public partial struct WorldMonth // Complements
{
    /// <summary>
    /// Obtains the genuine number of days in this month instance (excluding the
    /// blank days that are formally outside any month).
    /// </summary>
    [Pure]
    public int CountDaysInWorldMonth() => WorldSchema.CountDaysInWorldMonthImpl(Month);
}

public partial struct Zoroastrian13Month // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current month instance is virtual;
    /// otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsVirtual => Month == Egyptian13Schema.VirtualMonth;
}
