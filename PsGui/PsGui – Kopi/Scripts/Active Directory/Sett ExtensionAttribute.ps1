<#
Description = "Setter extensionAttribute for en gitt bruker."
Header = ""
[string]UserId = "Brukers ID"
[int]ExtensionAttributeNumber = "Attributtnummeret"
[string]ExtensionAttributeValue = "Attributtnummeret sin verdi. Et enkelt mellomrom fjerner attributten."
#>

param(
	  $userId,
	  $extensionAttributeNumber,
	  $extensionAttributeValue
	 )


Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Set-GlobalConfigVariables(Get-ConfigFileContents)
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Active Directory.psm1"

$extAttr = ("extensionAttribute" + $extensionAttributeNumber)
Set-ADAttribute $userId $extAttr $extensionAttributeValue

$secondsSaved = 15
Add-TimeSaveData $secondsSaved "Sett ExtensionAttribute.ps1"