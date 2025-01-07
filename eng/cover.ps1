# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [ValidateSet('Debug', 'Release')]
    [Alias('c')] [string] $Configuration = 'Debug',

                 [switch] $Future,
                 [switch] $All,
                 [switch] $Slow,
                 [switch] $NoBuild,
                 [switch] $NoTest,
                 [switch] $NoReport,

    [Alias('h')] [switch] $Help
)

. (Join-Path $PSScriptRoot 'common.ps1')

#-------------------------------------------------------------------------------

function Print-Help {
    say @"

Code coverage script.

Usage: cover.ps1 [arguments]
  -c|-Configuration  the configuration to test the solution for. Default = "Debug".
     -Future         include the project Calendrie.Future
     -All            include the project Calendrie.Future and Calendrie.Sketches
     -Slow           include slow tests (necessary for "full" test coverage)
     -NoBuild        do NOT build the test suite?
     -NoTest         do NOT execute the test suite? Implies -NoBuild
     -NoReport       do NOT run ReportGenerator?
  -h|-Help           print this help then exit

Be sure to restore the NuGet tools before running this script:
> dotnet tool restore

Examples.
> cover.ps1             # Run Coverlet then build an HTML report
> cover.ps1 -NoReport   # Run Coverlet, do NOT build an HTML report

"@
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

try {
    pushd $RootDir

    $format = 'opencover'

    $mainAssemblyName     = 'Calendrie'
    $futureAssemblyName   = 'Calendrie.Future'
    $sketchesAssemblyName = 'Calendrie.Sketches'

    if ($All) { $Future = $true }

    $outName  = 'coverage'
    if ($All) { $outName += "-all" } elseif ($Future) { $outName += "-future" }
    $outName += "-$configuration"
    $outDir   = Join-Path $ArtifactsDir $outName.ToLowerInvariant()
    $output   = Join-Path $outDir "$format.xml"
    $rgInput  = Join-Path $outDir "$format.*xml"
    $rgOutput = Join-Path $outDir 'html'

    # Filters: https://github.com/Microsoft/vstest-docs/blob/main/docs/filter.md
    $includes = @("[$mainAssemblyName]*")
    $excludes = @("[$mainAssemblyName]System.*")
    if ($Future) {
        $includes += "[$futureAssemblyName]*"
        $excludes += "[$futureAssemblyName]System.*"
    }
    if ($All) {
        $includes += "[$sketchesAssemblyName]*"
        $excludes += "[$sketchesAssemblyName]System.*"
    }
    $include  = '"' + ($includes -join '%2c') + '"'
    $exclude  = '"' + ($excludes -join '%2c') + '"'

    $args = @("-c:$Configuration")

    $testProject = Join-Path $SrcDir 'Calendrie.Tests' -Resolve

    if ($NoTest) { $NoBuild = $true }

    if (-not $NoBuild) {
        & dotnet build $testProject $args
            || die 'Failed to build the project'
    }

    if (-not $NoTest) {
        if ($Slow) {
            $filter = 'ExcludeFrom!=CodeCoverage'
        } else {
            $filter = 'ExcludeFrom!=CodeCoverage&Performance!~Slow'
        }
        $args += "--filter:$filter"

        & dotnet test $testProject $args `
            --no-build `
            /p:ExcludeByAttribute=DebuggerNonUserCode `
            /p:DoesNotReturnAttribute=DoesNotReturnAttribute `
            /p:CollectCoverage=true `
            /p:CoverletOutputFormat=$format `
            /p:CoverletOutput=$output `
            /p:Include=$include `
            /p:Exclude=$exclude
            || die 'Failed to run the test suite.'
    }

    if (-not $NoReport) {
        if (Test-Path $rgOutput) {
            Remove-Item $rgOutput -Force -Recurse
        }

        $riskhotspotassemblyfilters = "-$futureAssemblyName;-$sketchesAssemblyName"
        $reporttypes = 'Html;TextSummary'

        say 'Creating the reports...'

        & dotnet tool run reportgenerator `
            -riskhotspotassemblyfilters:$riskhotspotassemblyfilters `
            -reporttypes:$reporttypes `
            -reports:$rgInput `
            -targetdir:$rgOutput `
            -verbosity:Warning
            || die 'Failed to create the reports.'
    }

    say "`nCode coverage completed successfully" -Foreground Green
}
finally {
    popd
}
