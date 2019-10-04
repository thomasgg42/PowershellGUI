<#
Description = "Hent liste over pc-objekter innmeldt i en gruppe."
Header = "e"
[String]GroupId = "Navn på gruppe"
#>
param(
     [string]$GroupId
     )

# Importer egendefinerte moduler
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"

# Sett globale konstanter og importer moduler avhengig av konstantene
Set-GlobalConfigVariables(Get-ConfigFileContents)

$members = (Get-AdGroupMember -Identity $GroupId).Name
$filePath = "$($file_output)\"
$fileName = "pcobjekter_i_gruppe.txt"
$path    = $filePath + $fileName

# Check file existence
if(Test-Path -Path $path) {
    Clear-Content -Path $path
} else {
    New-Item -Path $filePath -Name $fileName
}

# Append new data
$members.foreach({
    Out-File -FilePath $path -Encoding "utf8" -InputObject $_ -Append 
})

# Open file
Invoke-Item $path

$secondsSaved = 60
Add-TimeSaveData $secondsSaved "Hent alle PC-objekter fra gruppe.ps1"