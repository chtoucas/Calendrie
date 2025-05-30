# See LICENSE in the project root for license information.

#Requires -Version 7

New-Variable RootDir (Get-Item $PSScriptRoot).Parent.FullName -Scope Script -Option Constant
New-Variable EngDir       (Join-Path $RootDir 'eng')  -Scope Script -Option Constant
New-Variable SrcDir       (Join-Path $RootDir 'src')  -Scope Script -Option Constant
New-Variable ArtifactsDir (Join-Path $RootDir '__')   -Scope Script -Option Constant
New-Variable PackagesDir  (Join-Path $ArtifactsDir 'packages')   -Scope Script -Option Constant

New-Alias "say" Write-Host

function die([string] $message) { Write-Error "`n$message" ; exit 1 }
