<#
Description = "Legger brukere fra tekst-fil inn i gitt gruppe"
Header = ""
[string]GroupName = "AD-gruppens navn"
[string]RelativeFilePath = "Relativ filsti fra applikasjonen, inkludert filnavn og filutvidelse. Punktum (.) vil si samme mappe som applikasjon. Eks: .\brukere.txt"
#>

param(
     [string]$groupName,
	[string]$relativeFilePath
     )

$users = Get-Content $relativeFilePath
foreach ($user in $users)
    {
    Add-ADGroupMember -Identity $groupName -Members $user
    }