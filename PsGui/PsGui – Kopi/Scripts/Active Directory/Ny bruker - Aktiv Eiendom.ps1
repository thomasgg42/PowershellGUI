<#
Description = "Oppretter en ny Aktiv Eiendom bruker i Eikanett & Terra. Oppretter Skype og Exchange-konto."
Header = ""
[string]SimilarUser = "Bruker hvor AD-grupper kopieres fra."
[string]NewGivenName = "Ny brukers fornavn."
[string]NewSurName = "Ny brukers etternavn."
[string]NewPhoneNum = "Ny brukers telefonnummer. <+47 xxxxxxxx>"
[string]NewMail = "Ny brukers e-postadresse"
[string]ESSNumber = "ESS saksnummer"
[string]MailTo = "Hvem mailen rundt opprettelse av bruker skal sendes til."
#>

param(
	[string]$similarUser,
	[string]$newGivenName,
	[string]$newSurName,
	[string]$newPhoneNum,
	[string]$newMail,
	[string]$essNumber,
	[string]$mailTo
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
$user = Get-NewADUserData $similarUser $NewGivenName $NewSurName $NewPhoneNum $NewMail "eiendom"
New-EikanettUserObject $user

# ExtensionAttribute11 kreves for aktivering av kalendersync i Webmegler
Set-ADAttribute $userId "extensionAttribute11" $user.extAttr11

Copy-EikanettUserGroups $similarUser $user.id
New-TerraUserObject $user

# Repeter funksjoner 
$numSecPerTry = 10
$numSecTilTimeout = 900
Repeat-Function $numSecPerTry $numSecTilTimeout Enable-UserMailbox @($user.id, $user.email)
#Repeat-Function $numSecPerTry $numSecTilTimeout Enable-Skype @($user.id)

# Send mail
$from = "Driftsupport@eika.no"
$subject = "Ny bruker"
$message = "<html>
				Hei, under f&#248;lger kontoinformasjon for ny bruker.
				<br><br>Navn: $($user.DisplayName)
				<br>Brukernavn: $($user.id)
				<br>Passord: $($user.pwClearText)
				<br>E-post: $($user.email)
				<br>Saksnummer: $($essNumber)
				<br><br>Mvh. ESS Drift
			</html>"
            
Send-Email $from $mailTo $subject $message $essNumber

$secondsSaved = 360
Add-TimeSaveData $secondsSaved "Ny bruker - Aktiv Eiendom.ps1"