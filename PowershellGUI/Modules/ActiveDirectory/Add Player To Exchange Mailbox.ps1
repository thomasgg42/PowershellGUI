<#
Description = "beskrivelse"
Header = "Funksjonsnavn"
Output = "True"
[string]navn = "Brukerens navn"
[int]alder = "Brukerens alder"
#>
param(
	[string]$navn,
	[int]$alder
	)
$msg = "Hei $navn, du er $alder år gammel."
Write-Output $msg