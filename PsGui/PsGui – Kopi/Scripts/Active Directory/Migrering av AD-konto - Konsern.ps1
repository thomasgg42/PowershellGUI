<#
Description = "Kopierer bruker fra Terra til Eikanett. Oppretter Skype og Exchange-konto og kopierer AD grupper."
Header = ""
[string]UserId = "Terra-brukeren som skal kopieres til Eikanett."
#>

param(
	 [string]$userId
	 )

# Importer egendefinerte moduler
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Active Directory.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Exchange.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Skype.psm1"

# Sett globale konstanter og importer moduler avhengig av konstantene
Set-GlobalConfigVariables(Get-ConfigFileContents)
#Import-SkypeModule $terraAdminUser $terraAdminPassword
Import-ExchangeModule

# Sjekk om input verdier eksisterer fra før

# Opprett bruker
$terraObj = (Get-ADUser -Server $ad_terra_fqdn -Identity $userId -Properties *)
$user = Get-NewKonsernUserDataFromTerra $terraObj
New-EikanettUserObject $user
Copy-EikanettUserGroups $terraObj.samAccountName $user.id $ad_terra_fqdn
$numSecPerTry = 10
$numSecTilTimeout = 900
Repeat-Function $numSecPerTry $numSecTilTimeout Enable-UserMailbox @($user.id, $user.email)
#Repeat-Function $numSecPerTry $numSecTilTimeout Enable-Skype @($user.id)

$secondsSaved = 240
Add-TimeSaveData $secondsSaved "Migrering av AD-konto - Konsern.ps1"