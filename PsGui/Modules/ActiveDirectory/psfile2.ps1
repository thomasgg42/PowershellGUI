<#
Description = "beskrivelse"
Header = "Funksjonsnavn"
Output = "True"
[string]navn = "Brukerens navn"
[int]alder = "Brukerens alder"
[int]hoyde = "Brukerens høyde"
#>
param(
	[string]$navn,
	[int]$alder,
    [int]$hoyde
	)
$msg = "Hei $navn, du er $alder år gammel. Høyden din er $hoyde"
Write-Output $msg