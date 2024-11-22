# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
                 [string] $Filter,

                 [switch] $NoBuild,

    [Alias('h')] [switch] $Help
)

. (Join-Path $PSScriptRoot 'common.ps1')

#-------------------------------------------------------------------------------

function Print-Help {
    say @"

Benchmark script.

Usage: bench.ps1 [arguments]
     -Filter
     -NoBuild        do NOT build the benchmark project?

  -h|-Help           print this help then exit

Examples.
> bench.ps1 *       # Run all tests
> bench.ps1 *XXX*   # Run tests whose name contain XXX
"@
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

try {
    pushd $RootDir

    $benchmarkProject = Join-Path $SrcDir 'Calendrie.Benchmarks' -Resolve

    $args = @("-c:Release")
    if ($NoBuild) { $args += '--no-build' }

    & dotnet run --project $benchmarkProject $args `
        --filter $Filter
}
finally {
    popd
}
