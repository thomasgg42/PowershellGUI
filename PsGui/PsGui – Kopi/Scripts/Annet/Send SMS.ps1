<#
Description = "Sends an SMS through the use of Eika Admin to the specified user."
Header = ""
[username]UserName = "Username to log into Eika Admin"
[password]Password = "Password to log into Eika Admin"
[multiline]SmsMsg = "Contents of the message to be sent"
[int]PhoneNumber = "Phone number to send to"
#>
param( 
      $userName,
      $password,
      $smsMsg,
      $phoneNumber
      )

Import-Module "$($PSScriptRoot)\..\..\Modules\Eika General.psm1"
Set-GlobalConfigVariables(Get-ConfigFileContents)

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
                  -Body "j_username=$($userName)&j_password=$($password)"

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

$secondsSaved = 5
Add-TimeSaveData $secondsSaved "Send SMS.ps1"