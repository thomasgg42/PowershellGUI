<#
Description = "Sett nytt tilfeldig passord p� brukerkonto."
Header = ""
#>

Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Set-GlobalConfigVariables(Get-ConfigFileContents)

$pw = Get-FriendlyPassword
Save-StringToClipboard $pw
Write-Output ""
Write-Output ("Passord: " + $pw)

$secondsSaved = 5
Add-TimeSaveData $secondsSaved "Nytt passord - utklipstavle.ps1"