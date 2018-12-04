<#
Description = "beskrivelse"
Header = "Funksjonsnavn"
Output = "True"
[string]tekstfilnavn = "Mappens navn"
[int]mappenavn = "Antall tekstfiler i mappe"
[int]tekstfilinnhold = "Tekstfilens innhold"
#>
param(
	[string]$mappenavn,
	[string]$tekstfilnavn,
	[string]$tekstfilinnhold
	)
cd "C:\Users\h804602.EIKANETT\Desktop"
New-Item -Name $mappenavn -ItemType "directory"
cd $mappenavn
New-Item -Name $tekstfilnavn -ItemType "file"
Write-Output $tekstfilinnhold >> $tekstfilnavn