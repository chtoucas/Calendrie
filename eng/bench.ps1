# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
                 [string] $Filter,

    [Parameter(Mandatory = $false)]
    [ValidateSet('Default', 'Dry', 'Short', 'Medium', 'Long')]
    [Alias('j')] [string] $Job = 'Short',

    [Alias('l')] [switch] $List,
    [Alias('q')] [switch] $Quiet,
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
  -j|-Job
  -l|-List

  -q|-Quiet
     -NoBuild        do NOT build the benchmark project?

  -h|-Help           print this help then exit

Examples.
> bench.ps1 *       # Run all tests (most certainly a bad idea)
> bench.ps1 *XXX    # Run tests whose names end with XXX
"@
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

try {
    pushd $RootDir

    $outDir = Join-Path $ArtifactsDir "benchmarks"

    $benchmarkProject = Join-Path $SrcDir 'Calendrie.Benchmarks' -Resolve

    $args = @()
    if ($Quiet)   { $args += '--disableLogFile' }
    if ($NoBuild) { $args += '--no-build' }

    if ($List) {
        & dotnet run --project $benchmarkProject $args `
            --list tree `
            -c Release `
            -p:AnalysisMode=AllDisabledByDefault

        exit
    }

    # Disable the log file written on disk.
    # Stop after the first error (by default it's not).
    & dotnet run --project $benchmarkProject $args `
        -c Release `
        --artifacts $outDir `
        --filter $Filter `
        --job $Job `
        --stopOnFirstError `
        -p:AnalysisMode=AllDisabledByDefault
}
finally {
    popd
}
