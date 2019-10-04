<#
Description = "Kopierer alle grupper fra FromUser til ToUser."
Header = "Test"
[string]FromUser = "Brukeren hvor man kopierer grupper fra."
[string]ToUser = "Brukere hvor gruppene kopieres til."
#>
param(
     [string]$fromUser,
     [string]$toUser
     )

Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Active Directory.psm1"
Set-GlobalConfigVariables(Get-ConfigFileContents)

Copy-EikanettUserGroups $fromUser $toUser

$secondsSaved = 20
Add-TimeSaveData $secondsSaved "Kopier grupper fra bruker til bruker.ps1"