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
  -X|-Extras         enable even more tests. Only effective if -NoBuild is not
                     enabled. Notice that it does not change the test plans
                     "default" and "regular".
  -c|-Configuration  configuration to test the solution for. Default = "Debug"
     -NoBuild        do NOT build the test suite?
  -h|-Help           print this help then exit

The default behaviour is to run the regular test plan using the configuration Debug.

Test plans
----------
- "default"   = exclude tests of low importance and slow-running tests
- "regular"   = exclude tests of low importance
- "most"      = the whole test suite
- "more"      = include all tests ignored by "regular"
The difference between "default" and "regular" is really tiny.

We also have a plan named "cover". It mimics the (default) test plan used by the
code coverage tool.

Of course, one can use "dotnet test" to run the whole test suite or to apply
custom filters.

Examples
--------
> test.ps1 -NoBuild             # Default test plan (Debug), no build
> test.ps1 regular -c Release   # Comprehensive test suite (Release)

"@
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

try {
    pushd $RootDir

    $args = @("-c:$configuration")
    if ($NoBuild) { $args += '--no-build' }
    elseif ($Extras) { $args += "/p:EnableMorePrototypalTests=true" }

    switch ($Plan) {
        'default' {
            # Default test suite, excludes
            # - tests excluded from the plan Regular
            # - slow tests
            $filter = 'ExcludeFrom!=Regular&Performance!~Slow'
        }
        'regular' {
            # Regular test suite, excludes
            # - tests excluded from the plan Regular
            $filter = 'ExcludeFrom!=Regular'
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
