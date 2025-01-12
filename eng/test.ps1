# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [ValidateSet(
        'default', 'regular', 'more', 'most', 'cover')]
                 [string] $Plan = 'default',

    [Parameter(Mandatory = $false)]
    [ValidateSet('Debug', 'Release')]
    [Alias('c')] [string] $Configuration = 'Debug',

    [Alias('X')] [switch] $Extras,
                 [switch] $NoBuild,

    [Alias('h')] [switch] $Help
)

. (Join-Path $PSScriptRoot 'common.ps1')

#-------------------------------------------------------------------------------

function Print-Help {
    say @"

Run the test suite.

Usage: test.ps1 [arguments]
     -Plan           specify the test plan. Default = "default"
  -X|-Extras         enable even more tests related to the prototypal schemas.
                     Only effective if -NoBuild is not enabled. Notice that it
                     does not change the test plans "default" and "regular".
  -c|-Configuration  configuration to test the solution for. Default = "Debug"
     -NoBuild        do NOT build the test suite?
  -h|-Help           print this help then exit

Test plans.
- "default"   = exclude tests of low importance and slow-running tests
- "regular"   = exclude tests of low importance
- "more"      = tests ignored by "regular", but no extras unless -X is selected
- "most"      = the whole test suite, but no extras unless -X is selected
- "cover"     = mimic the (default) test plan used by the code coverage tool
The difference between "default" and "regular" is really tiny.

Of course, one can use "dotnet test" to run the whole test suite or to apply
custom filters.

Examples.
> test.ps1 -NoBuild             # Default test plan (Debug), no build
> test.ps1 regular -c Release   # Regular test plan (Release)
> test.ps1 most -X              # Whole test suite (Debug)

"@
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

try {
    pushd $RootDir

    $args = @("-c:$configuration")
    if ($Extras) {
        $args += "/p:EnableMorePrototypalTests=true"

        if ($NoBuild) {
            Write-Host -ForegroundColor Red `
                'The option -NoBuild is NOT compatible with the option -Extras (-X)'
            exit
        }
    }
    if ($NoBuild) { $args += '--no-build' }

    switch ($Plan) {
        'default' {
            # Default test suite, excludes
            # - tests excluded from the plan Regular
            # - slow tests
            $filter = 'ExcludeFrom!=Regular&Performance!~Slow'
            if ($Extras) {
                Write-Host -ForegroundColor Yellow `
                    'The option -Extras (-X) has no effect on the "default" plan'
            }
        }
        'regular' {
            # Regular test suite, excludes
            # - tests excluded from the plan Regular
            $filter = 'ExcludeFrom!=Regular'
            if ($Extras) {
                Write-Host -ForegroundColor Yellow `
                    'The option -Extras (-X) has no effect on the "regular" plan'
            }
        }
        'more' {
            # Only include tests excluded from the plan Regular.
            $filter = 'ExcludeFrom=Regular'
        }
        'most' {
            $filter = ''
        }
        'cover' {
            # Mimic the (default) test plan used by cover.ps1. It excludes tests
            # excluded from the plan CodeCoverage and slow tests.
            $filter = 'ExcludeFrom!=CodeCoverage&Performance!~Slow'
        }
    }

    if ($filter) { $args += "--filter:$filter" }

    $testProject = Join-Path $SrcDir 'Calendrie.Tests' -Resolve

    & dotnet test $testProject $args
        || die 'Failed to run the test suite.'
}
finally {
    popd
}
