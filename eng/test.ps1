# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [ValidateSet(
        'regular', 'cover', 'more', 'most', 'redundant')]
                 [string] $Plan = 'regular',

    [Parameter(Mandatory = $false)]
    [ValidateSet('Debug', 'Release')]
    [Alias('c')] [string] $Configuration = 'Debug',

                 [switch] $NoBuild,

    [Alias('h')] [switch] $Help
)

. (Join-Path $PSScriptRoot 'common.ps1')

#-------------------------------------------------------------------------------

function Print-Help {
    say @"

Run the test suite.

Usage: test.ps1 [arguments]
    |-Plan           specify the test plan. Default = "regular"
  -c|-Configuration  configuration to test the solution for. Default = "Debug"
    |-NoBuild        do NOT build the test suite?
  -h|-Help           print this help then exit

The default behaviour is to run the regular test plan using the configuration Debug.

Test plans
----------
- "regular"   = exclude redundant tests, slow-running tests
- "more"      = exclude redundant tests
- "most"      = the whole test suite

The extra test plans are
- "redundant" = complement of "more" in "most", ie "redundant" = "most" - "more".

We have also a plan named "cover". It mimics the default test plan used by the
code coverage tool. The difference between "cover" and "regular" is really tiny.
For a test to be in "regular" but not in "cover", it must be known to be slow
and not being explicitely excluded from code coverage, right now there is none.

Of course, one can use "dotnet test" to run the whole test suite or to apply
custom filters.

Examples
--------
> test.ps1 -NoBuild             # Regular test plan (Debug)
> test.ps1 regular -c Release   # Regular test plan (Release)
> test.ps1 more                 # Comprehensive test suite (Debug)

"@
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

try {
    pushd $RootDir

    $args = @("-c:$configuration")
    if ($NoBuild) { $args += '--no-build' }

    switch ($Plan) {
        'regular' {
            # Regular test suite, excludes
            # - tests explicitely excluded from this plan
            # - slow tests
            # - redundant tests (implicit via RedundantTraitDiscoverer)
            $filter = 'ExcludeFrom!=Regular&Performance!~Slow'
        }
        'more' {
            # Only exclude redundant tests.
            $filter = 'Redundant!=true'
        }
        'redundant' {
            # Only include redundant tests.
            $filter = 'Redundant=true'
        }
        'most' {
            $filter = ''
        }
        'cover' {
            # Mimic the default test plan used by cover.ps1, excludes
            # - tests explicitely excluded from this plan
            # - slow tests
            # - redundant tests (implicit via RedundantTraitDiscoverer)
            # - tests excluded from the "regular" plan (implicit via ExcludeFromTraitDiscoverer)
            $filter = 'ExcludeFrom!=CodeCoverage&Performance!~Slow'
        }
    }

    if ($filter) { $args += "--filter:$filter" }

    $testProject = Join-Path $TestDir 'Calendrie.Tests' -Resolve

    & dotnet test $testProject $args
        || die 'Failed to run the test suite.'
}
finally {
    popd
}
