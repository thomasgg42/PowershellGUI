<#
Description = "Setter fraværsmelding hos bruker. Dette gjøres ved bruk av fraværsassistenten. I motsetning til ved bruk av en regel, sendes svaret kun 1 gang innsender."
Header = ""
[string]UserOrMailId = "BrukerID eller e-postaddresse."
[string]DateTimeStart = "mm/dd/åååå tt:mm:ss. Enkelt mellomrom vil si ingen tidsbegrensning."
[string]DateTimeEnd = "mm/dd/åååå tt:mm:ss. Enkelt mellomrom vil si ingen tidsbegrensning."
[multiline]Message = "Fraværsmelding. Bruk <br> for å indikere ny linje i meldingen."
#>

param(
     [string]$userOrMailId,
	 [string]$dateTimeStart,
	 [string]$dateTimeEnd,
	 [string]$message
     )

# Importer egendefinerte moduler
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Exchange.psm1"

# Sett globale konstanter og importer moduler avhengig av konstantene
Set-GlobalConfigVariables(Get-ConfigFileContents)
Import-ExchangeModule

$htmlMsg = "<html>" + $message + "</html>"
Set-MailboxAutoReply $userOrMailId $htmlMsg $dateTimeStart $dateTimeEnd

$secondsSaved = 20
Add-TimeSaveData $secondsSaved "Sett fraværsmelding (auto-reply).ps1"