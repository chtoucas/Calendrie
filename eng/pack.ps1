# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
                 # Do NOT pack Calendrie.Extras?
                 #[switch] $NoExtras,

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
        # FIXME(code): CS0649 -> RangeSet<T>.Empty
        #"/p:HideInternals=true",
        "/p:PrintSettings=true"

    $Calendrie       = Join-Path $SrcDir 'Calendrie' -Resolve
    #$CalendrieExtras = Join-Path $SrcDir 'Calendrie.Extras' -Resolve

    say 'Cleaning solution...' -Foreground Magenta
    & dotnet clean -c Release -v minimal

    say "`nBuilding project Calendrie..." -Foreground Magenta
    # Safety measures:
    # - Always build Calendrie.Extras, not just Calendrie
    #   This is to ensure that HideInternals=true does not break Calendrie.Extras
    # - Delete project.assets.json (--force)
    & dotnet build $Calendrie $args --force
    #& dotnet build $CalendrieExtras $args --force

    # Pack Calendrie
    say "`nPackaging Calendrie..." -Foreground Magenta
    & dotnet pack $Calendrie $args --no-build --output $PackagesDir `
        || die "Failed to pack '$Calendrie'."

    # Pack Calendrie.Extras
    #if (-not $NoExtras) {
    #    say "`nPackaging Calendrie.Extras..." -Foreground Magenta
    #
    #    & dotnet pack $CalendrieExtras $args --no-build --output $PackagesDir `
    #        || die "Failed to pack '$CalendrieExtras'."
    #}

    say "`nPackaging completed successfully" -Foreground Green
}
finally {
    popd
}
