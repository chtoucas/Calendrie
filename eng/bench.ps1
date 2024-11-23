# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding(DefaultParameterSetName = 'Benchmark')]
param(
    [Parameter(Mandatory = $true, ParameterSetName = 'Benchmark', Position = 0)]
    [ValidateNotNullOrWhiteSpace()]
                 [string] $Filter,

    [Parameter(Mandatory = $false, ParameterSetName = 'Benchmark')]
    [ValidateSet('Default', 'Dry', 'Short', 'Medium', 'Long')]
    [Alias('j')] [string] $Job = 'Default',

    [Parameter(Mandatory = $true, ParameterSetName = 'List')]
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

    $benchmarkProject = Join-Path $SrcDir 'Calendrie.Benchmarks' -Resolve

    # Common options:
    # - Release build.
    # - Disable all analyzers here NOT within the project.
    # - Stop after the first error (by default it's not).
    $args = '-c:Release', '--stopOnFirstError', '-p:AnalysisMode=AllDisabledByDefault'
    # Disable the log file written on disk?
    if ($Quiet)   { $args += '--disableLogFile' }
    if ($NoBuild) { $args += '--no-build' }

    if ($List) {
        & dotnet run --project $benchmarkProject $args --list tree
    } else {
        $outDir = Join-Path $ArtifactsDir "benchmarks"

        & dotnet run --project $benchmarkProject $args `
            --artifacts $outDir `
            --filter $Filter `
            --job $Job
    }

}
finally {
    popd
}
