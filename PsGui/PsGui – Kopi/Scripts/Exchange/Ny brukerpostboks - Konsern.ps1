<#
Description = "Oppretter en ny brukerpostboks med primær SIP og SMTP-addresse."
Header = ""
[string]UserID = "Brukerens bruker-ID"
[string]UserEmail = "Brukerens ønskede SIP/SMTP-addresse."
#>

param(
	[string]$UserID,
	[string]$userEmail
	)

# Importer egendefinerte moduler
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Exchange.psm1"

# Sett globale konstanter og importer moduler avhengig av konstantene
Set-GlobalConfigVariables(Get-ConfigFileContents)
Import-ExchangeModule

# Opprett mailboks
Enable-UserMailbox $UserID $userEmail

$secondsSaved = 20
Add-TimeSaveData $secondsSaved "Ny brukerpostboks - Konsern.ps1"