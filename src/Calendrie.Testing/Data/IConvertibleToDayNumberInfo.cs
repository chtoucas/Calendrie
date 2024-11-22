// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data;

public interface IConvertibleToDayNumberInfo
{
    [Pure] DayNumberInfo ToDayNumberInfo();
}
