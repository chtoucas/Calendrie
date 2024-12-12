// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

using System.Runtime.InteropServices;

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]

[assembly: InternalsVisibleTo("Calendrie.Testing" + Calendrie.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Calendrie.Tests" + Calendrie.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Calendrie.Benchmarks" + Calendrie.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Samples" + Calendrie.AssemblyInfo.PublicKeySuffix)]
