# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding(DefaultParameterSetName = 'Benchmark')]
param(
    [Parameter(Mandatory = $false, ParameterSetName = 'List')]
    [ValidateSet('flat', 'tree')]
    [Alias('l')] [string] $List = 'flat',

    [Parameter(Mandatory = $true, ParameterSetName = 'Benchmark', Position = 0)]
    [ValidateNotNullOrWhiteSpace()]
    [Alias('f')] [string] $Filter,

    [Parameter(Mandatory = $false, ParameterSetName = 'Benchmark')]
    [ValidateSet('Default', 'Dry', 'Short', 'Medium', 'Long')]
    [Alias('j')] [string] $Job = 'Default',

    [Parameter(Mandatory = $false, ParameterSetName = 'Benchmark')]
    [ValidateSet('0', '1')]
    [Alias('d')] [string] $Disassemble = '',

    [Alias('q')] [switch] $Quiet,
                 [switch] $NoBuild,

    [Parameter(Mandatory = $false, ParameterSetName = 'Help')]
    [Alias('h')] [switch] $Help
)

. (Join-Path $PSScriptRoot 'common.ps1')

#-------------------------------------------------------------------------------

function Print-Help {
    say @"

Benchmark script.

Usage: bench.ps1 [arguments]
  -l|-List

  -f|-Filter
  -j|-Job
  -d|-Disassamble

  -q|-Quiet
     -NoBuild        do NOT build the benchmark project?

  -h|-Help           print this help then exit

Examples.
> bench.ps1 -l
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
    if ($NoBuild) { $args += '--no-build' }
    # Disable the log file written on disk?
    if ($Quiet) { $args += '--disableLogFile' }

    switch ($PSCmdlet.ParameterSetName) {
        'List' {
            & dotnet run --project $benchmarkProject $args --list $List

            break
        }

        'Benchmark' {
            $outDir = Join-Path $ArtifactsDir "benchmarks"

            if ($Disassemble) {
                $args += '--disasm'
                $args += "--disasmDepth=$Disassemble"
            }

            & dotnet run --project $benchmarkProject $args `
                --artifacts $outDir `
                --filter $Filter `
                --job $Job

            break
        }
    }
}
finally {
    popd
}
