// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

public partial struct Armenian13Month // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is the intercalary
    /// month; otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsIntercalary => Month == Egyptian13Schema.IntercalaryMonth;
}

public partial struct Coptic13Month // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is the intercalary
    /// month; otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsIntercalary => Month == Coptic13Schema.IntercalaryMonth;
}

public partial struct Egyptian13Month // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is the intercalary
    /// month; otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsIntercalary => Month == Egyptian13Schema.IntercalaryMonth;
}

public partial struct Ethiopic13Month // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is the intercalary
    /// month; otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsIntercalary => Month == Coptic13Schema.IntercalaryMonth;
}

public partial struct FrenchRepublican13Month // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is the intercalary
    /// month; otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsIntercalary => Month == FrenchRepublican13Schema.IntercalaryMonth;
}

public partial struct Zoroastrian13Month // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is the intercalary
    /// month; otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsIntercalary => Month == Egyptian13Schema.IntercalaryMonth;
}
