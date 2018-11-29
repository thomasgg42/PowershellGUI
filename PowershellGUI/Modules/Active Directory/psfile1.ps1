<#
Description = "beskrivelse"
Header = "Funksjonsnavn"
Output = "True"
[string]Navn = "Brukerens navn"
[int]Alder = "Brukerens alder"
#>
param(
	[string]$navn = "TempNavn",
	[string]$alder
	)
$msg = "Hei $navn, du er $alder år gammel"
Write-Output $msg