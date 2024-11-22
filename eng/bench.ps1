# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [Alias('h')] [switch] $Help
)

. (Join-Path $PSScriptRoot 'common.ps1')

#-------------------------------------------------------------------------------

function Print-Help {
    say @"

Benchmark script.

Usage: benchmark.ps1 [arguments]
  -h|-Help           print this help then exit

"@
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

try {
    pushd $RootDir

    $benchmarkProject = Join-Path $TestDir 'Calendrie.Benchmarks' -Resolve

    & dotnet run -c Release --project $benchmarkProject `
        -f net9.0 --runtimes net9.0 `
        --filter "*" `
        -p:AnalysisMode=AllDisabledByDefault
}
finally {
    popd
}
