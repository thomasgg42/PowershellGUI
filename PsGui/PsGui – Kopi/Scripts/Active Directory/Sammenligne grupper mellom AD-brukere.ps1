<#
Description = "Sammenlikner to AD-brukere og lister forskjeller hos hver enkelt bruker."
Header = ""
[String]UserId1 = "Bruker-id 1"
[string]UserId2 = "Bruker-id 2"
#>
param( 
    $userId1,
    $userId2
)
# O(n) space and time complexity

Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Set-GlobalConfigVariables(Get-ConfigFileContents)

$givenName1      = (Get-ADUser -Identity $userId1).givenName
$givenName2      = (Get-ADUser -Identity $userId2).givenName
$user1GroupList  = (Get-ADPrincipalGroupMembership -Identity $userId1).Name 
$user2GroupList  = (Get-ADPrincipalGroupMembership -Identity $userId2).Name
$hashUser1Groups = @{}
$hashUser2Groups = @{}

[System.Collections.ArrayList]$groupsUser2ButNotUser1 = @()
[System.Collections.ArrayList]$groupsUser1ButNotUser2 = @()
[System.Collections.ArrayList]$groupsShared           = @()

# Populate hash tables with all groupmemberships

foreach($user1Group in $user1GroupList) {
    $hashUser1Groups.add($user1Group, $user1Group) # Only need the key
}

foreach($user2Group in $user2GroupList) {
    $hashUser2Groups.add($user2Group, $user2Group) # Only need the key
}

# Get differences in group memberships

foreach($group in $hashUser1Groups.Keys) {
    if(!$hashUser2Groups.ContainsKey($group)) {
        $groupsUser1ButNotUser2.add($group) | Out-Null
    }
}

foreach($group in $hashUser2Groups.Keys) {
    if(!$hashUser1Groups.ContainsKey($group)) {
        $groupsUser2ButNotUser1.add($group) | Out-Null
    } else {
        $groupsShared.Add($group) | Out-Null
    }
}

# Send stores values to output file and open the file

$file = "$($file_output)\sammenlikn.txt"
"###### Bruker: $($userId1)($($givenName1)) har følgende grupper som $($userId2)($($givenName2)) ikke har: ######" | Out-File -FilePath $file 
foreach($group in $groupsUser1ButNotUser2) {
    $group | Out-File -FilePath $file -Append
}

"`r`n###### Bruker: $($userId2)($($givenName2)) har følgende grupper som $($userId1)($($givenName1)) ikke har: ######" | Out-File -FilePath $file  -Append
foreach($group in $groupsUser2ButNotUser1) {
    $group | Out-File -FilePath $file -Append
}

"`r`n###### Følgnede grupper har begge brukere: ######" | Out-File -FilePath $file  -Append
foreach($group in $groupsShared) {
    $group | Out-File -FilePath $file  -Append
}

Invoke-Item $file

$secondsSaved = 40
Add-TimeSaveData $secondsSaved "Sammenligne grupper mellom AD-brukere.ps1"