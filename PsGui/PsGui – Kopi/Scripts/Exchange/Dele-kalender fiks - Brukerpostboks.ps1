<#
Description = "NB: KUN BRUKERpostbokser. Fikser feilen med feilmelding: Kan ikke legge til en eller flere brukere i tilgangslisten for mappen. Kan ikke gi rettigheter på serveren til brukere som ikke er lokale."
Header = ""
[string]UserID = "Brukerens bruker-ID"
#>

param(
	[string]$UserID
	)

Set-CalendarSharingFix $UserID
