// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Prototypes;

using Calendrie.Core.Intervals;

// We limit the range of supported years because the default impl of
// GetYear() and GetStartOfYear() are extremely slow if the values of
// "y" or "daysSinceEpoch" are big.
// Only override this property if both methods can handle big values
// efficiently.

internal static class YearsRanges
{
    public static Range<int> Standard => Range.Create(1, 9999);
    public static Range<int> Proleptic => Range.Create(-9998, 9999);

    public static Range<int> GetRange(bool proleptic) => proleptic ? Proleptic : Standard;
}
