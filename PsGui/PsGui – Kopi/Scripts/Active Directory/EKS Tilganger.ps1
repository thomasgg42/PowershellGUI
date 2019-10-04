<#
Description = "Setter tilganger definert i Confluence-artikkel"
Header = ""
[username]ESSUserId                   = "Din egen H-bruker ID"
[password]ESSPassword                 = "Ditt eget H-passord. Benyttes for Confluence-tilgang."
[string]NewUserId                     = "BrukerID-en som skal gis tilganger."
[string]NewUserSocialSecurityNumber   = "Brukerens fødselsnummer. Kan stå blankt (mellomrom) om dette ikke skal brukes."
[string]AlleiKonsern                  = "Distribusjonsgruppe: Alle i Konsern"
[string]AlleiEikaKundesenter          = "Distribusjonsgruppe: Alle i Eika Kundesenter"
[string]sparesmartATsparesmartDOTno   = "Felles mailboks."
[string]SalgsFront                    = "Systemer og applikasjoner"
[string]BankProd                      = ""
[string]CRM = ""
[string]EikaAdmin = ""
[string]F_SkrivetilgangBank = ""
[string]F_SkrivetilgangDigital = ""
[string]F_SkrivetilgangForsikring = ""
[string]F_SkrivetilgangAnsatte = ""
[string]AlleiEKSDigital = ""
[string]AlleiEKSBank = ""
[string]AlleiEKSForsikring = ""
[string]NetsOnline = ""
[string]Confluence = ""
[string]Jira = ""
[string]CompetellaStandardagent = ""
#>

<#
    Ved oppdatering av nye Confluence-rader må 
    accessRightInput-arrayet og PSGUI-input-felter oppdateres.
    
    Ved oppdatering av nye Confluence-kolonner må
    accessRightInput-arrayet, PSGUI-input-felter og 
    switcher i Get-ConfluenceWebRequestAccessControlData() og
    Set-AccessRights() oppdateres.

    [string]F_SkrivetilgangLedelse = "Fjernet denne da jeg nådde maks antall input felt.."
#>

param(
    $ESSUserId,
    $ESSPassword,
    $newUserId,
    $newUserSocialSecurityNumber,
    $AlleiKonsern,
    $AlleiEikaKundesenter,
    $sparesmartATsparesmartDOTno,
    $SalgsFront,
    $BankProd,
    $CRM,
    $EikaAdmin,
    $F_SkrivetilgangBank,
    $F_SkrivetilgangLedelse,
    $F_SkrivetilgangDigital,
    $F_SkrivetilgangForsikring,
    $F_SkrivetilgangAnsatte,
    $AlleiEKSDigital,
    $AlleiEKSBank,
    $AlleiEKSForsikring,
    $NetsOnline,
    $Confluence,
    $Jira,
    $CompetellaStandardagent
)

Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Active Directory.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Exchange.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Skype.psm1"

Set-GlobalConfigVariables(Get-ConfigFileContents)
Import-ExchangeModule

$accessRightInput = @{}
$accessRightInput["Alle i Konsern"]                = $AlleiKonsern
$accessRightInput["Alle i Eika Kundesenter"]       = $AlleiEikaKundesenter
$accessRightInput["Bankprod"]                      = $BankProd
$accessRightInput["sparesmart@sparesmart.no"]      = $sparesmartATsparesmartDOTno
$accessRightInput["Salgsfront"]                    = $SalgsFront
$accessRightInput["Eika Admin"]                    = $EikaAdmin
$accessRightInput["Skype for Business"]            = $SkypeForBusiness
$accessRightInput["(F:) Skrivetilgang Bank"]       = $F_SkrivetilgangBank
$accessRightInput["(F:) Skrivetilgang Ledelse"]    = $F_SkrivetilgangLedelse
$accessRightInput["(F:) Skrivetilgang Digital"]    = $F_SkrivetilgangDigital
$accessRightInput["(F:) Skrivetilgang Forsikring"] = $F_SkrivetilgangForsikring
$accessRightInput["Alle i EKS Digital"]            = $AlleiEKSDigital
$accessRightInput["Alle i EKS Bank"]               = $AlleiEKSBank
$accessRightInput["Alle i EKS Forsikring"]         = $AlleiEKSForsikring
$accessRightInput["Nets Online"]                   = $NetsOnline
$accessRightInput["Confluence"]                    = $Confluence
$accessRightInput["Competella Standardagent"]      = $CompetellaStandardagent

$confluenceTitle = "Tilgangsbestilling+-+Eika+Kundesenter"
$jsonWebRequest   = Invoke-ConfluenceWebRequestAccessControlList $ESSUserId $ESSPassword $confluenceTitle
$htmlPage         = Get-ConfluenceWebRequestHtmlElements $jsonWebRequest
$htmlPageClean    = Invoke-EncodingWorkaround $htmlPage
$accessRightsList = Get-ConfluenceWebRequestAccessControlData $htmlPageClean

# Setter alle verdier og attributter på brukerobjektet
Get-ConfluenceWebRequestKeyValues $accessRightsList $newUserId $accessRightInput

# Sett kryptert fødselsnummer i extAttr13
if($accessRightInput["Eika Admin"] -eq "ja") {
    $attributeName = "extensionAttribute13"
    $eikaAdminHtml = Invoke-EikaAdminWebRequestEncryptedSSN $ESSUserId $ESSPassword $newUserSocialSecurityNumber
    $socialSecurityNumber = Get-EikaAdminWebRequestEncryptedSSNValue $eikaAdminHtml
    Set-ADAttribute $newUserId $attributeName $socialSecurityNumber
    Write-Output "$($attributeName) = $($socialSecurityNumber)"
}


if($accessRightInput["Competella Standardagent"] -eq "ja") {
    Invoke-OpenCompetellaQueueHandlingEKS $ESSUserId $ESSPassword
}

$secondsSaved = 300
Add-TimeSaveData $secondsSaved "EKS Tilganger.ps1"