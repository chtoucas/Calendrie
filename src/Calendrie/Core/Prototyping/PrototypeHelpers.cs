// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Prototyping;

using Calendrie.Core.Intervals;

// We limit the range of supported years because the default impl of
// GetYear() and GetStartOfYear() are extremely slow if the values of
// "y" or "daysSinceEpoch" are big.
// Only override this property if both methods can handle big values
// efficiently.

internal static class PrototypeHelpers
{
    public static Segment<int> Standard => Segment.Create(1, 9999);
    public static Segment<int> Proleptic => Segment.Create(-9998, 9999);

    public static Segment<int> GetSupportedYears(bool proleptic) =>
        proleptic ? Proleptic : Standard;
}
