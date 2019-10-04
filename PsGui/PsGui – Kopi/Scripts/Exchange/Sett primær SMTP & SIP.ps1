<#
Description = "Setter primær SMTP og SIP addresse i Exchange Admin. Gjør ingenting om disse allerede finnes."
Header = ""
[String]BrukerID = "Brukerens ID"
[String]Email = "Den primære SMTP-adressen."
#>
param(
     [string]$brukerID,
	 [string]$email
     )

Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Exchange.psm1"

# Sett globale konstanter og importer moduler avhengig av konstantene
Set-GlobalConfigVariables(Get-ConfigFileContents)
Import-ExchangeModule

Set-PrimarySmtpAddress $brukerID $email
Set-Mailbox -Identity $brukerID -EmailAddresses @{add="SIP:$($email)"}

$secondsSaved = 30
Add-TimeSaveData $secondsSaved "Sett primær SMTP & SIP.ps1"