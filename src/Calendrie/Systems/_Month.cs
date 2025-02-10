// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

public partial struct ArmenianMonth // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is the intercalary
    /// month; otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsIntercalary => Month == Egyptian13Schema.IntercalaryMonth;
}

public partial struct CopticMonth // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is the intercalary
    /// month; otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsIntercalary => Month == Coptic13Schema.IntercalaryMonth;
}

public partial struct EgyptianMonth // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is the intercalary
    /// month; otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsIntercalary => Month == Egyptian13Schema.IntercalaryMonth;
}

public partial struct EthiopicMonth // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is the intercalary
    /// month; otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsIntercalary => Month == Coptic13Schema.IntercalaryMonth;
}

public partial struct FrenchRepublicanMonth // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is the intercalary
    /// month; otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsIntercalary => Month == FrenchRepublican13Schema.IntercalaryMonth;
}

public partial struct ZoroastrianMonth // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is the intercalary
    /// month; otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsIntercalary => Month == Egyptian13Schema.IntercalaryMonth;
}
