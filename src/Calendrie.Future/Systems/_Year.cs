// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

public partial struct PaxYear // Complements
{
    /// <summary>
    /// Obtains the number of weeks in the current instance.
    /// </summary>
    [Pure]
    public int CountWeeks() => Calendar.Schema.CountWeeksInYear(Year);
}

[ExcludeFromCodeCoverage]
public partial struct RevisedWorldYear { }
