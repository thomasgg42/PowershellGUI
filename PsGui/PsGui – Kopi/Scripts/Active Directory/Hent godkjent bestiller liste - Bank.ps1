<#
Description = "Henter ut alle godkjente bestillere fra gitt bank og kopierer disse inn på utklippstavlen."
Header = ""
[int]BankRegNummer = "Reg-nummeret til bank på 4 siffer"
#>

param (
       $bankRegNummer
      )

Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Active Directory.psm1"
Set-GlobalConfigVariables(Get-ConfigFileContents)

$bankRegNummer = "PI$($bankRegNummer)"
$ou = $ad_eikanett_skala_oupath.Replace("CCCCC", "Bank").
                       Replace("BBBBB", $bankRegNummer).
                       Replace("AAAAA", "Users")

$gb = (Get-ADUser -Properties * -Filter {extensionAttribute5 -like "*GB*"}`
                  -SearchBase $ou).displayname
$output = ""
foreach($g in $gb) {
    $output += "$($g)`n"
}

Write-Output "Godkjente bestillere kopiert til utklippstavle:`n`n$($output)"
Save-StringToClipboard $output

$secondsSaved = 20
Add-TimeSaveData $secondsSaved "Hent godkjent bestiller liste - Bank.ps1"