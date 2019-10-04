<#
Description = "Oppretter en ny Konsern-bruker i Eikanett & Terra. Oppretter Skype og Exchange-konto."
Header = ""
[string]UserId = "BrukerID-en opprettet av Zalaris"
[string]SimilarUserId = "BrukerID tilhørende bruker hvor AD-grupper kopieres fra."
[string]NewGivenName = "Ny brukers fornavn."
[string]NewSurName = "Ny brukers etternavn."
[string]NewPhoneNum = "Ny brukers telefonnummer. <+47 xxxxxxxx>"
[string]NewMail = "Ny brukers e-postadresse"
[string]ESSNumber = "ESS saksnummer"
[string]MailTo = "Hvem mailen rundt opprettelse av bruker skal sendes til."
#>

param(
    [string]$userId,
	[string]$similarUserId,
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
# Import-SkypeModule $terraAdminUser $terraAdminPassword
Import-ExchangeModule

# Opprett bruker
$user = Get-NewADUserData $similarUserId $NewGivenName $NewSurName $NewPhoneNum $NewMail "konsern" $userId
New-EikanettUserObject $user
New-TerraUserObject $user

Copy-EikanettUserGroups $similarUserId $user.id

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
Add-TimeSaveData $secondsSaved "Ny bruker - Konsern.ps1"