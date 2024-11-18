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

Packaging script.

Usage: pack.ps1 [arguments]
  -h|-Help           print this help then exit

"@
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

try {
    pushd $RootDir

    $args = "-c:Release",
        "/p:ContinuousIntegrationBuild=true",
        "/p:HideInternals=true",
        "/p:PrintSettings=true"

    $Calendrie = Join-Path $SrcDir 'Calendrie' -Resolve

    say 'Cleaning solution...' -Foreground Magenta
    & dotnet clean -c Release -v minimal

    say "`nBuilding project Calendrie..." -Foreground Magenta
    # Safety measure: delete project.assets.json (--force)
    & dotnet build $Calendrie $args --force

    say "`nPackaging Calendrie..." -Foreground Magenta
    & dotnet pack $Calendrie $args --no-build --output $PackagesDir `
        || die "Failed to pack '$Calendrie'."

    say "`nPackaging completed successfully" -Foreground Green
}
finally {
    popd
}
