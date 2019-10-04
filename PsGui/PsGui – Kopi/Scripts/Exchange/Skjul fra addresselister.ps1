<#
Description = "Skjuler den angitte brukeren fra den globale e-post adresselisten."
Header = ""
[string]UserID = "Brukerens bruker-ID"
#>

param(
	 [string]$userId
	 )

# Importer egendefinerte moduler
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Exchange.psm1"

# Sett globale konstanter og importer moduler avhengig av konstantene
Set-GlobalConfigVariables(Get-ConfigFileContents)
Import-ExchangeModule

# Skjul fra adresselister
$attr = "msExchHideFromAddressLists"
$existingValue = (Get-ADUser -Identity $userId -Properties $attr).$attr
if($existingValue.Length -le 0) {
	Set-ADUser -Identity $userId `
			   -Add @{$attr = "TRUE" }
} elseif($existingValue.Length -ge 1) {
	Set-ADUser -Identity $userId `
		       -Replace @{$attr = "TRUE" }
}

$secondsSaved = 20
Add-TimeSaveData $secondsSaved "Skjul fra addresseliste.ps1"