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

F# linter.

Usage: fsharplint.ps1 [arguments]
  -h|-Help           print this help then exit

"@
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

Write-Error "Disabled. See https://github.com/fsprojects/FSharpLint/issues/687"
exit 1

try {
    pushd $RootDir

    $testProject = Join-Path $TestDir 'Calendrie.Tests\Calendrie.Tests.fsproj' -Resolve
    $conf = Join-Path $EngDir 'fsharplint.json' -Resolve

    & dotnet fsharplint lint -l $conf $testProject
}
finally {
    popd
}
