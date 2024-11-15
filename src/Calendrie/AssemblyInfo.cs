// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

using System.Runtime.InteropServices;

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]

#if !__HIDE_INTERNALS__
[assembly: InternalsVisibleTo("Calendrie.Testing" + Calendrie.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Calendrie.Tests" + Calendrie.AssemblyInfo.PublicKeySuffix)]
#endif

namespace Calendrie;

/// <summary>
/// Provides constants used in Assembly's attributes.
/// </summary>
internal static partial class AssemblyInfo
{
    /// <summary>
    /// Gets the public key suffix suitable for use with <see cref="InternalsVisibleToAttribute"/>.
    /// </summary>
    public const string PublicKeySuffix =
#if __SIGN_ASSEMBLY__
        ",PublicKey="
        + "00240000048000009400000006020000002400005253413100040000010001004918847f79cb1b"
        + "ac82c378320b74004c6eccd7371d9aa45c48d5f54707cdae33f6d2490160a49ee2e73c94198001"
        + "481fa046ed05f4da57ea2dbaf4a677564dc455026bdd55a3a6458abcd9f88e0d415e616f4cf42a"
        + "ad4f214d43f932532ba5d970aceeab89ea8140db9b5c51c33e791254843dee3e3c646782f5c1ef"
        + "82d288e9";
#else
        "";
#endif
}

#if RELEASE && __ENABLE_PREVIEW_FEATURES__
#warning Built using preview features of the .NET platform
#endif
