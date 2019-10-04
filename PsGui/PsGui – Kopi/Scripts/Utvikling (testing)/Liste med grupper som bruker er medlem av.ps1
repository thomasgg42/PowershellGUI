<#
Description = "Hent liste over Grupper som bruker er medlem av"
Header = "Grupper som bruker er medlem av"
[String]UserID = "Brukernavn"
#>
param(
     [string]$UserID
     )


Get-ADPrincipalgroupmembership -identity $UserID | select name | Export-csv -path "\\eikanett.eika.no\Konsern\KonsernFelles\Eika Servicesenter\Drift\Powershell\Output\PSGui Data\$($UserID).csv" -NoTypeInformation -Encoding "utf8"