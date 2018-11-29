<#
Description = "beskrivelse"
Header = "Funksjonsnavn"
Output = "True"
[string]MappeNavn = "Mappens navn"
[int]TekstfilNavn = "Antall tekstfiler i mappe"
[int]TekstfilInnhold = "Tekstfilens innhold"
#>
param(
	[string]$mappenavn,
	[string]$tekstfilnavn,
	[string]$tekstfilinnhold
	)
cd "C:\Users\h804602.EIKANETT\Desktop"
New-Item -Name $navn -ItemType "directory"
cd $navn
New-Item -Name $tekstfilnavn -ItemType "file"
Write-Output $tekstfilinnhold >> $tekstfilnavn