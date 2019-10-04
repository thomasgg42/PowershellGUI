<#
Description = "Hent bruker locked-status"
Header = "Test"
[String]BrukerID = "Brukerens ID"
#>
param(
     [string]$brukerID
     )

# Importer egendefinerte moduler
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"

# Sett globale konstanter og importer moduler avhengig av konstantene
Set-GlobalConfigVariables(Get-ConfigFileContents)


$eikdc1 = (Get-ADUser -Server $ad_domain_controller_1 -Identity $brukerID -Properties *)
$eikdc2 = (Get-ADUser -Server $ad_domain_controller_2 -Identity $brukerID -Properties *)
$eikdc3 = (Get-ADUser -Server $ad_domain_controller_3 -Identity $brukerID -Properties *)
$eikdc4 = (Get-ADUser -Server $ad_domain_controller_4 -Identity $brukerID -Properties *)
$terraProperties = (Get-ADUser -Server $ad_terra_fqdn -Identity $brukerID -Properties *)

Write-Output ("ID: " + $eikdc1.samAccountName)
Write-Output ("Name: " + $eikdc1.GivenName + " " + $eikdc1.SurName)
Write-Output ("PrincipalName: " + $eikdc1.UserPrincipalName)
Write-Output ("Title: " + $eikdc1.Title)
Write-Output ("Email: " + $eikdc1.Mail)
Write-Output ("Phone: " + $eikdc1.MobilePhone)
Write-Output ("Canoncial: " + $eikdc1.CanonicalName)
Write-Output ("Home dir: " + $eikdc1.HomeDirectory)
Write-Output ("Dep Code: " + $eikdc1.Department)
Write-Output ("ExtAtt8: " + $eikdc1.extensionAttribute8)
Write-Output ("ExtAtt10: " + $eikdc1.extensionAttribute10)
Write-Output ("ExtAtt12: " + $eikdc1.extensionAttribute12)
Write-Output ("ExtAtt13: " + $eikdc1.extensionAttribute13)

Write-Output ("Locked eikdc200: " + $eikdc1.lockedout)
Write-Output ("Locked eikdc201: " + $eikdc2.lockedout)
Write-Output ("Locked eikdc202: " + $eikdc3.lockedout)
Write-Output ("Locked eikdc203: " + $eikdc4.lockedout)
Write-Output ("LastBadPw eikdc200: " + $eikdc1.LastBadPasswordAttempt)
Write-Output ("LastBadPw eikdc201: " + $eikdc2.LastBadPasswordAttempt)
Write-Output ("LastBadPw eikdc202: " + $eikdc3.LastBadPasswordAttempt)
Write-Output ("LastBadPw eikdc203: " + $eikdc4.LastBadPasswordAttempt)
Write-Output ("BadPwdCount eikdc200: " + $eikdc1.badpwdcount)
Write-Output ("BadPwdCount eikdc201: " + $eikdc2.badpwdcount)
Write-Output ("BadPwdCount eikdc202: " + $eikdc3.badpwdcount)
Write-Output ("BadPwdCount eikdc203: " + $eikdc4.badpwdcount)
Write-Output ("PwLastSet eikdc200: " + $eikdc1.PasswordLastSet)
Write-Output ("PwLastSet eikddc201: " + $eikdc2.PasswordLastSet)
Write-Output ("PwLastSet eikdc202: " + $eikdc3.PasswordLastSet)
Write-Output ("PwLastSet eikddc203: " + $eikdc4.PasswordLastSet)
Write-Output ("Pw last set Terra: " + $terraProperties.PasswordLastSet)

$secondsSaved = 20
Add-TimeSaveData $secondsSaved "AD-bruker status.ps1"