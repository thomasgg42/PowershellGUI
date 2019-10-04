<#
Description = "Hente Recoverykey til et PC objekt fra AD. Resultat lagres til utklippstavlen"
Header = "Bitlocker Recoverykey"
[String]Computer    = "PC du ønsker og ta ut bitlocker for"
#>
param(
     [string]$Computer
     )
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Set-GlobalConfigVariables(Get-ConfigFileContents)


$ADComputer = Get-ADComputer -Filter {Name -eq $Computer}
$BitLockerObjects = Get-ADObject -Filter {objectclass -eq 'msFVE-RecoveryInformation'} -SearchBase $ADComputer.DistinguishedName -Properties 'msFVE-RecoveryPassword' | select 'msFVE-RecoveryPassword',Name
Save-StringToClipboard $BitLockerObjects
Write-Output ""
Write-Output $BitLockerObjects