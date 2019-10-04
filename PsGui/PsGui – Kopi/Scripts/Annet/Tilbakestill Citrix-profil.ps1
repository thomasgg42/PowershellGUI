<#
Description = "Resette brukerprofil fra GUI"
Header = "Resette brukerprofil"
[String]UserId  = "Brukerens ID"
[string]ExecuteNow = "JA dersom tilbakestilling skal utføres nå. NEI dersom tilbakestilling skal utføres automatisk i natt."
#>
param(
      [string]$userId,
      [String]$executeNow
     )

Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Set-GlobalConfigVariables(Get-ConfigFileContents)
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Active Directory.psm1"

$folderSuffix      = "_w2012"
$profileFolderName = "$($userId)$($folderSuffix)"
$profilePath       = ""
$type              = Get-UserType $userId

if($type.ToLower() -eq "bank") {
     $pi = Get-BankPINumber $userId
     $profilePath = $file_eikanett_profiles.Replace("AAAAA", $profileFolderName).
                                            Replace("BBBBB", "PI$($pi)UPM").
                                            Replace("CCCCC", $type)
} else {
     $profilePath = $file_eikanett_profiles.Replace("AAAAA", $profileFolderName).
                                            Replace("BBBBB", "UPM").
                                            Replace("CCCCC", $type)
}

$pathExists = Test-Path $profilePath
if($pathExists) {
     if($executeNow.ToLower() -eq "ja") {
          $numSecPerTry     = 5
          $numSecTilTimeout = 20
          Repeat-Function $numSecPerTry $numSecTilTimeout Add-NameSuffix @($profilePath, ".old")
          Write-Output "Profil: $($profilePath) tilbakestilt."
     } else {
          Add-Content -Path "$($file_output)\Profil\Resette profil.txt" `
                      -Value $profilePath
     }
} else {
     Write-Output "Fant ikke mappen. Ingen endringer utført."
}

$secondsSaved = 30
Add-TimeSaveData $secondsSaved "Tilbakestill Citrix-profil.ps1"