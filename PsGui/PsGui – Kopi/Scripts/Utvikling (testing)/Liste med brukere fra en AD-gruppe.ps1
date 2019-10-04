<#
Description = "Hent liste over brukere som er medlem av en gruppe"
Header = "Medlemmer av en gruppe"
[String]GruppeID = "Navn på gruppe"
#>
param(
     [string]$GruppeID
     )


Get-ADGroupMember -identity $GruppeID | select name | Export-csv -path "\\eikanett.eika.no\Konsern\KonsernFelles\Eika Servicesenter\Drift\Powershell\Output\PSGui Data\$($GruppeID).csv" -NoTypeInformation -Encoding "utf8"