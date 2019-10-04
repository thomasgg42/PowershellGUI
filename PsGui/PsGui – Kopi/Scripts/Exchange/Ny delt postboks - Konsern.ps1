<#
Description = "Oppretter en ny delt postboks for Konsern-avdelinger."
Header = ""
[string]DisplayName = "Visningsnavnet for postboksen. Mellomrom tillatt."
[string]AccountName = "SamAccountName for brukerkontoen. Ingen mellomrom tillatt."
[string]SMTP = "SMTP-adresse (e-post adresse) for postboksen."
[string]OuName = "Navnet på organisasjonsenheten under Konsern: KO-EBK/KO-EFK/KO-EKS/KO-ESS/KO-Gruppen"
[multiline]FullAccessUsers = "Brukere med full access rettigheter. Separeres med komma, ingen mellomrom. Eks: h804602,h905040,b550232"
[multiline]SendAsUsers = "Brukere med send as rettigheter. Separeres med komma, ingen mellomrom. Eks: h804602,h905040,b550232"
#>

param(
	[string]$displayName,
    [string]$accountName,
    [string]$smtp,
    [string]$ouName,
    [string]$fullAccessUsers,
    [string]$sendAsUsers
	)

# Importer egendefinerte moduler
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Active Directory.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Exchange.psm1"

# Sett globale konstanter og importer moduler avhengig av konstantene
Set-GlobalConfigVariables(Get-ConfigFileContents)
Import-ExchangeModule

# Opprett mailboks
$ou = $ad_konsern_distgruppe_oupath.Replace("AAAAA", $ouName)
Enable-SharedMailbox $displayName $accountName $smtp $ou
Set-SharedMailboxProperties $accountName $ad_konsern_companyname


# Opprett full access distribusjonsgruppe
$distinguishedName             = $ad_konsern_distgruppe_distinguishedName.Replace("AAAAA", $ouName)
$groupNameFullAccess           = $ad_konsern_distgruppe_fullaccess_name.Replace("AAAAA", $accountName)
$groupSamAccountNameFullAccess = "$($accountName)_fa"

New-ADGroup -Name $groupNameFullAccess `
            -SamAccountName $groupSamAccountNameFullAccess `
            -DisplayName $groupNameFullAccess `
            -Path $distinguishedName `
            -Description "Full Access rettigheter til $($smtp)." `
            -GroupCategory "Security" `
            -GroupScope "DomainLocal"

# Opprett send as distribusjonsgruppe
$groupNameSendAs           = $ad_konsern_distgruppe_sendas_name.Replace("AAAAA", $accountName)
$groupSamAccountNameSendAs = "$($accountName)_sa"
New-ADGroup -Name $groupNameSendAs `
            -SamAccountName $groupSamAccountNameSendAs `
            -DisplayName $groupNameSendAs `
            -Path $distinguishedName `
            -Description "Send As rettigheter til $($smtp)." `
            -GroupCategory "Security" `
            -GroupScope "DomainLocal"
           
# Legg til brukere i distribusjonsgruppe
$faUsers = $fullaccessUsers.Split(",")
Add-ADGroupMember -Identity $groupSamAccountNameFullAccess -Members $faUsers
$saUsers = $sendAsUsers.Split(",")
Add-ADGroupMember -Identity $groupSamAccountNameSendAs -Members $saUsers


# Knytte distribusjonsgrupper mot mailbokskonto. 

# Byttes ut med try function
#Start-Sleep -Seconds 600
#Add-MailboxPermission -Identity $accountName -AccessRights "FullAccess" -User $groupSamAccountNameFullAccess

$numSecPerTry     = 10
$numSecTilTimeout = 900 
Repeat-Function $numSecPerTry $numSecTilTimeout Set-MailboxPermissionFullAccess @($accountName, $groupSamAccountNameFullAccess)

Get-Mailbox -Identity $accountName | Add-ADPermission -User $groupSamAccountNameSendAs `
            -AccessRights ExtendedRight `
            -ExtendedRights "Send As"

$secondsSaved = 240
Add-TimeSaveData $secondsSaved "Ny delt postboks - Konsern.ps1"