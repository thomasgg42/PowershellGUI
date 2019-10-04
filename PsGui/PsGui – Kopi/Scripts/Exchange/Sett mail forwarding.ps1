<#
Description = "Setter videresending av mail på mailkonto. Mail vil lagres i begge postkasser."
Header = ""
[String]RecipientID = "Bruker-IDen eller e-postadressen til mottaker av mail."
[String]ForwardTo = "E-postadressen det skal videresendes til."
#>
param(
     [string]$recipientId,
	[string]$forwardTo
     )

# Importer egendefinerte moduler
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Set-GlobalConfigVariables(Get-ConfigFileContents)

Set-Mailbox -Identity $recipientId -DeliverToMailboxAndForward $true -ForwardingSMTPAddress $forwardTo

$secondsSaved = 20
Add-TimeSaveData $secondsSaved "Sett mail forwarding.ps1"