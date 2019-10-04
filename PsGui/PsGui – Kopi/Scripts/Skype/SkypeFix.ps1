<#
Description = "Skype fix setter korrekt security identifier (SID) på brukerobjektet i AD."
Header = "Skype fix"
[string]BrukerID = "Brukerkonto som skal fikses."
#>
param(
	[string]$brukerId
	)

Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Active Directory.psm1"


Set-GlobalConfigVariables(Get-ConfigFileContents)
Set-SkypeFix $brukerId