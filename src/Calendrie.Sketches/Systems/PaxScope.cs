// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Validation;

/// <summary>
/// Represents the standard scope of the Pax calendar.
/// <para>Supported dates are within the range [1..9999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class PaxScope : StandardScope
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaxScope"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    public PaxScope(PaxSchema schema, DayNumber epoch) : base(schema, epoch)
    {
        PreValidator = new PaxPrevalidator(schema);
    }
}
