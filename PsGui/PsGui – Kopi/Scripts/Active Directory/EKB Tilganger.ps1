<#
Description = "Setter tilganger definert i Confluence-artikkel"
Header = ""
[username]ESSUserId                   = "Din egen H-bruker ID"
[password]ESSPassword                 = "Ditt eget H-passord"
[string]NewUserId                     = "BrukerID-en som skal gis tilganger."
[string]NewUserSocialSecurityNumber   = "Brukerens fødselsnummer. Kan stå blankt (mellomrom) om dette ikke skal brukes."
[string]AlleIKonsern                  = "Distribusjonsgruppe: Alle i Konsern"
[string]AlleIEikaGjovik               = "Distribusjonsgruppe: Alle i Eika Gjøvik"
[string]AlleIEikaKredittbank          = "Distribusjonsgruppe: Alle i Eika Kredittbank"
[string]AlleIEikaKredittbankGjovik    = "Distribusjonsgruppe: Alle i Eika Kredittbank Gjøvik"
[string]backofficeATeikaDOTno         = "SMTP: backoffice@eika.no"
[string]ekbATeikaDOTno                = "SMTP: ekb@eika.no"
[string]ekbDOTbmDOTsoknadATeikaDOTno  = "SMTP: ekb.bm.soknad@eika.no"
[string]esbokforingATeikaDOTno        = "SMTP: esbokforing@eika.no"
[string]eikaKredittbankAsNy           = "AD-Gruppe: Eika Kredittbank AS NY (ansatt)"
[string]EikaAdminForEKB               = "Eika Admin for EKB"
[string]BankProd                      = "BankProd"
[string]hjemmeKontor                  = "Hjemmekontor"
[string]eArkivKredittKort             = "eArkiv Kredittkort (lese)"
[string]eArkivSparesmart              = "eArkiv Sparesmart"
[string]Teams                         = "Teams"
[string]EikaAdmin                     = "Eika Admin"
[string]NetsOnline                    = "Nets Online"
[string]Confluence                    = "Confluence"
#>

<#
    Ved oppdatering av nye Confluence-rader må 
    accessRightInput-arrayet og PSGUI-input-felter oppdateres.
    
    Ved oppdatering av nye Confluence-kolonner må
    accessRightInput-arrayet, PSGUI-input-felter og 
    switcher i Get-ConfluenceWebRequestAccessControlData() og
    Set-AccessRights() oppdateres.
#>

param(
    $ESSUserId,
    $ESSPassword,
    $newUserId,
    $newUserSocialSecurityNumber,
    $AlleIKonsern,
    $AlleIEikaGjovik,
    $AlleIEikaKredittbank,
    $AlleIEikaKredittbankGjovik,
    $backofficeATeikaDOTno,
    $ekbATeikaDOTno,
    $ekbDOTbmDOTsoknadATeikaDOTno,
    $esbokforingATeikaDOTno,
    $eikaKredittbankAsNy,
    $EikaAdminForEKB,
    $BankProd,
    $hjemmeKontor,
    $eArkivKredittKort,
    $eArkivSparesmart,
    $EikaAdmin,
    $NetsOnline,
    $Confluence
)

Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Active Directory.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Exchange.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Skype.psm1"

Set-GlobalConfigVariables(Get-ConfigFileContents)
Import-ExchangeModule

$accessRightInput = @{}
$accessRightInput["Alle i Konsern"]                  = $AlleIKonsern
$accessRightInput["Alle i Eika Gjøvik"]              = $AlleIEikaGjovik
$accessRightInput["Alle i Eika Kredittbank"]         = $AlleIEikaKredittbank
$accessRightInput["Alle i Eika Kredittbank Gjøvik"]  = $AlleIEikaKredittbankGjovik
$accessRightInput["backoffice@eika.no"]              = $backofficeATeikaDOTno
$accessRightInput["ekb@eika.no"]                     = $ekbATeikaDOTno
$accessRightInput["ekb.bm.soknad@eika.no"]           = $ekbDOTbmDOTsoknadATeikaDOTno
$accessRightInput["esbokforing@eika.no"]             = $esbokforingATeikaDOTno
$accessRightInput["Eika Kredittbank AS NY (ansatt)"] = $eikaKredittbankAsNy
$accessRightInput["BankProd"]                        = $BankProd
$accessRightInput["Hjemmekontor"]                    = $hjemmeKontor
$accessRightInput["eArkiv Kredittkort"]              = $eArkivKredittKort
$accessRightInput["eArkiv Sparesmart"]               = $eArkivSparesmart
$accessRightInput["Skype for Business"]              = $SkypeForBusiness
$accessRightInput["Eika Admin"]                      = $EikaAdmin
$accessRightInput["Eika Admin For EKB"]              = $EikaAdminForEKB
$accessRightInput["Nets Online"]                     = $NetsOnline
$accessRightInput["Confluence"]                      = $Confluence

$confluenceTitle = "Tilgangsbestilling+-+Eika+Kredittbank"
$jsonWebRequest   = Invoke-ConfluenceWebRequestAccessControlList $ESSUserId $ESSPassword $confluenceTitle
$htmlPage         = Get-ConfluenceWebRequestHtmlElements $jsonWebRequest
$htmlPageClean    = Invoke-EncodingWorkaround $htmlPage
$accessRightsList = Get-ConfluenceWebRequestAccessControlData $htmlPageClean
Get-ConfluenceWebRequestKeyValues $accessRightsList $newUserId $accessRightInput

# Sett kryptert fødselsnummer i extAttr13
if($accessRightInput["Eika Admin"] -eq "ja") {
    $attributeName = "extensionAttribute13"
    $eikaAdminHtml = Invoke-EikaAdminWebRequestEncryptedSSN $ESSUserId $ESSPassword $newUserSocialSecurityNumber
    $socialSecurityNumber = Get-EikaAdminWebRequestEncryptedSSNValue $eikaAdminHtml
    Set-ADAttribute $newUserId $attributeName $socialSecurityNumber
    Write-Output "$($attributeName) = $($socialSecurityNumber)"
}

<#
if($accessRightInput["Skype for Business"] -eq "ja") {
    $xUser = "$($ESSUserId)X"
    Import-SkypeModule $xUser $ESSXPassword
    Enable-Skype $newUserId
}
#>

$secondsSaved = 300
Add-TimeSaveData $secondsSaved "EKB Tilganger.ps1"