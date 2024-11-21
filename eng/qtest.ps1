# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [Alias('n')] [string] $Name = '',

    [Parameter(Mandatory = $false)]
    [Alias('ns')] [string] $Namespace = '',

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

Run selected tests.

Usage: qtest.ps1 [arguments]
    |-Name
    |-Namespace
  -c|-Configuration  configuration to test the solution for. Default = "Debug"
    |-NoBuild        do NOT build the test suite?
  -h|-Help           print this help then exit

Examples
--------
> qtest.ps1 XXX        # Run tests whose name contain XXX
> qtest.ps1 -ns YYY    # Run tests whose name contain Calendrie.Tests.YYY

"@
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

try {
    pushd $RootDir

    $args = @("-c:$configuration")
    if ($NoBuild) { $args += '--no-build' }
    if ($Name) { $filter = "FullyQualifiedName~$Name"}
    if ($Namespace) {
        if ($Name) {
            $filter += "&FullyQualifiedName~Calendrie.Tests.$Namespace"
        } else {
            $filter = "FullyQualifiedName~Calendrie.Tests.$Namespace"
        }
    }
    $args += "--filter:$filter"

    $testProject = Join-Path $TestDir 'Calendrie.Tests' -Resolve

    & dotnet test $testProject $args
        || die 'Failed to run the test suite.'
}
finally {
    popd
}
