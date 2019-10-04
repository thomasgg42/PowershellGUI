<#
Description = "Sett nytt tilfeldig passord på brukerkonto."
Header = ""
[username]ESSUserID = "Ditt eget H-nummer"
[password]ESSPassword = "Ditt eget passord."
[string]UserID = "Brukerens ID"
#>
param(
	 [string]$ESSUserID,
	 [string]$ESSPassword,
     [string]$userID
     )

Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Import-Module "$($PSScriptRoot)\..\..\Modules\Eika Active Directory.psm1"

Set-GlobalConfigVariables(Get-ConfigFileContents)

$pw = Get-FriendlyPassword
Save-StringToClipboard $pw
Set-NewPassword $userID $pw "terra"
$phoneNumber = Get-PhoneNumber $userID

Write-Output ""
Write-Output ("Bruker: " + $userID)
Write-Output ("Passord: " + $pw)

$smsMsg = "Nytt passord: $($pw)"
$phoneNumLength = 8
$prefix = "+47 "
$prefixLength = $prefix.Length
# If +47 xxxxxxxx, remove +47 (space included)
if(($phoneNumber.Length -eq ($phoneNumLength + $prefixLength)) -and ($phoneNumber.SubString(0, $prefixLength) -eq $prefix)) {
	$phoneNumber = $phoneNumber.SubString($prefixLength)
    $r = Invoke-WebRequest -Uri "https://intra.eika.no/wad/j_spring_security_check" `
                      -SessionVariable session `
                      -Method "POST" `
                      -Headers @{`
                          "Cache-Control"="max-age=0"; `
                          "Origin"="https://intra.eika.no"; `
                          "Upgrade-Insecure-Requests"="1"; `
                          "User-Agent"="Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.96 Safari/537.36"; `
                          "Accept"="text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8"; `
                          "Referer"="https://intra.eika.no/wad/auth/login"; `
                          "Accept-Encoding"="gzip, deflate, br"; `
                          "Accept-Language"="nb-NO,nb;q=0.9,no;q=0.8,nn;q=0.7,en-US;q=0.6,en;q=0.5"; `
                          "Cookie"="JSESSIONID=D80DE612C0CF5A60E5D3E6082C992CF6;`
                                    _ga=GA1.2.775514929.1546412298; `
                                    _gcl_au=1.1.2044668269.1547549936; `
                                    BID=2050; _gid=GA1.2.1618091196.1552041592; `
                                    InternalJointSession=27d9e9b437078d3710e45a9f01b01884a938645bb95ca7b967f55f3080379197"`
                           } `
                      -ContentType "application/x-www-form-urlencoded" `
                      -Body "j_username=$($ESSUserID)&j_password=$($ESSPassword)"
    
    
    
    $r = Invoke-WebRequest -Uri "https://intra.eika.no/wad/sendsmsRest/sendMessage"`
                 -WebSession $session `
                 -Method "POST" `
                 -Headers @{`
                     "Cookie"="JSESSIONID=E39FBBDE99748572A659C2A47D972FEE;`
                             _ga=GA1.2.775514929.1546412298;`
                             _gcl_au=1.1.2044668269.1547549936;`
                             BID=2050;`
                             _gid=GA1.2.1618091196.1552041592;`
                             InternalJointSession=eca7e9ac67715db4ad8f200c32b80d4c159c9071cc49804c475c69c9e50cbdd8";`
                             "Origin"="https://intra.eika.no";`
                             "Accept-Encoding"="gzip, deflate, br";`
                             "Accept-Language"="nb-NO,nb;q=0.9,no;q=0.8,nn;q=0.7,en-US;q=0.6,en;q=0.5";`
                             "User-Agent"="Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.96 Safari/537.36"; "Accept"="*/*";`
                             "Referer"="https://intra.eika.no/wad/sendsms/Read"; "X-Requested-With"="XMLHttpRequest"`
                  } `
                  -ContentType "application/x-www-form-urlencoded; charset=UTF-8" `
                  -Body "phoneNumber=$($phoneNumber)&messageContent=$($smsMsg)"
				 
				 
	Write-Output ("Telefon: " + $phoneNumber + ", SMS er sendt.")
} else {
	Write-Output "Telefonnummer har feil format eller er ikke funnet, SMS ikke sendt."
}

$secondsSaved = 20
Add-TimeSaveData $secondsSaved "Nytt passord - Terra.ps1"