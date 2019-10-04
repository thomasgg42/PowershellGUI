<#
Description = "Henter GPO-forandringer utført de siste 14 dagene. Vet ikke med sikkerhet om dette er en fullstendig liste."
Header = ""
[int]AntallDagerTilbakeITid = "Antall dager tilbake i tid man ønsker å hente GPO-endringer fra."
#>
param( 
    $antallDagerTilbakeITid
)

Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Set-GlobalConfigVariables(Get-ConfigFileContents)

$gpos = (Get-GPO -All -Domain "eikanett.eika.no" | `
                 Where-Object { ([datetime]::today - ($_.ModificationTime)).Days -le $antallDagerTilbakeITid }) | `
                 Sort-Object ModificationTime

if($gpos.Length -gt 0) {
    foreach($gpo in $gpos) {
        Write-Output $gpo.ModificationTime
        Write-Output $gpo.DisplayName
        Write-Output "`n"
    }
} else {
    Write-Output "Ingen GPO-endringer funnet for de $($antallDagerTilbakeITid) siste dagene."
}

$secondsSaved = 20
Add-TimeSaveData $secondsSaved "Hent nylige GPO-endringer.ps1"