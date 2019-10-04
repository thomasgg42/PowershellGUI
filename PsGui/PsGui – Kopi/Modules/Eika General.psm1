<#
    A general powershell module for generic functions
    PSScriptRoot ensures the relative file path to calculated
    from this file and not from where you call this file.
    This means the relative path between the module folder 
    and the script folder must not be changed.
#>

# Global, read-only variable accessible in all functions
Set-Variable -Name "configFile" -Value "$($PSScriptRoot)\..\config.ini" -Scope Global -Option constant

# Gets the config file variables
Function Get-ConfigFileContents {
	$config = @{}
	foreach($line in Get-Content $configFile) {
		# If not comment or empty line
		if(!(($line.StartsWith("#") -or [string]::IsNullOrEmpty($line)))) {
			$line  = Get-CleanedInput $line
			$key   = Get-KeyName $line '='
			$value = Get-QuotationContents $line
			$config.Add($key, $value)
		}
	}
	return $config
}

# Parses a string, extracts and returns the contents
# between quotation marks.
# @Returns string inside quotation marks.
# @Returns empty string if none found.
Function Get-QuotationContents([string]$line) {
	$quotationContents = ($line |%{$_.split('"')[1]})
	return $quotationContents
}

# Extracts and returns  the key in a string
# "<key> <separator> <value>".
# Ex: myName = "Bob" returns myName
# @Returns key as a string
Function Get-KeyName([string]$line, [string]$separator) {
	return $line.Split($separator)[0].Trim()
}


# Sanitizes input. Removing leading and 
# trailing white-space.
# @Returns sanitized input string.
Function Get-CleanedInput([string]$line) {
	return $line.Trim()
}

# Sets global constants from the provided hash table.
# This function is mainly used to load the config values
# from the config file into the script as global constants.
# The constant names will be equal those listed in the config file
# and are available at file scope.
Function Set-GlobalConfigVariables($configContents) {
	foreach($entry in $configContents.GetEnumerator()) {
		Set-Variable -Name $entry.Name -Value $entry.Value -Scope Global -Option Constant
	}
}

# Returns a semi-random password consisting of six
# lower case letters (lower case L excluded) 
# and two digits between 0-9
# @Returns a semi-random string
Function Get-FriendlyPassword {
	$letters = -join ((97..107) + (109..122)| Get-Random -Count 6 | % {[char]$_})
	$numbers = -join ((0..9) | Get-Random -Count 2)
	return ($letters + $numbers)
}

# Saves the provided input to Windows clipboard, allowing
# you to paste the values in the Windows environment.
Function Save-StringToClipboard([string]$line) {
	$line | clip.exe
}


# Executes the provided function with the provided function arguments.
# If the provided function generates an exception, the function-call
# will be repeated every provided amount of seconds, until reaching a 
# maximum amount of seconds secondsUntilTimeout.
# The provided function's arguments must be "splatted".
# Example: Start-Pause 2 6 Create-ADUser @("Bob", "Nilsen", "35")
Function Repeat-Function([int]$secondsPerCheck, [int]$secondsUntilTimeOut, $function, [string[]]$functionArgs) {
	$Global:ErrorActionPreference = "Stop"
	$counter = 0
	while($counter -lt $secondsUntilTimeOut) {
        try {
			Write-Output "Trying function.."
		    (Get-Item "function:$function").ScriptBlock.Invoke($functionArgs)
			break
        } catch {
			$numRemainingAttempts = ($secondsUntilTimeOut - $counter) / $secondsPerCheck
			Write-Output $_
            Write-output "Retrying function call, $($numRemainingAttempts) attempts left..."
            Start-Sleep -Seconds $secondsPerCheck
        }
        $counter += $secondsPerCheck
	}
	$Global:ErrorActionPreference = "Continue"
}

# Renames the file or folder in the provided path
# with the provided suffix.
# Used to "reset" cached and similar files and folders.
Function Add-NameSuffix($path, $suffix) {
	$newName = $path.Split("\")[-1] + $suffix
	Rename-Item -Path $path -NewName $newName
}

# UTF8BOM is supposed to handle norwegian special characters
# but this does not seem to be the case. This function is used
# as a work around to set the proper characters æ, ø and å.
Function Invoke-EncodingWorkaround($html) {
    $temp = $html.Replace("Ã¸", "ø").
                  Replace("Ã¥", "å").
                  Replace("Ã¦", "æ")
    return $temp
}


# Sends a HTTP POST request to the Eika Admin login page.
# Receives a HTTP session object where you can access
# inlogged functions.
# @Requires access to Eika Admin
Function Invoke-EikaAdminWebRequestLogin($userId, $password) {
    return Invoke-WebRequest -Uri "https://intra.eika.no/wad/j_spring_security_check" `
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
                  -Body "j_username=$($userId)&j_password=$($password)"
}

# Sends a HTTP POST request to start a HTTP session
# Receives a HTTP GET request containing the table
# consisting of all the access rights.
# @Requires access to ESS Drift Confluence
# @Returns a json object
Function Invoke-ConfluenceWebRequestAccessControlList($userId, $password, $confluenceTitle) {
    $request = Invoke-WebRequest -Uri "https://confluence.intra.eika.no/dologin.action" `
    -SessionVariable session `
    -Method "POST" `
    -Headers @{ `
               "Cache-Control"="max-age=0"; `
               "Origin"="https://confluence.intra.eika.no"; `
               "Upgrade-Insecure-Requests"="1"; `
               "User-Agent"="Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36"; `
               "Accept"="text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3"; `
               "Referer"="https://confluence.intra.eika.no/login.action"; `
               "Accept-Encoding"="gzip, deflate, br"; `
               "Accept-Language"="en-US,en;q=0.9"; `
               "Cookie"="_ga=GA1.2.666718609.1559905434;BIGipServerpool_confluance=126883244.39455.0000;mywork.tab.tasks=false;JSESSIONID=2A965B66810000192BCCE5C8C861AAC9"`
               } `
               -ContentType "application/x-www-form-urlencoded" `
               -Body "os_username=$($userId)&os_password=$($password)&os_cookie=true&login=Log+in&os_destination="
    
    $jsonRequest = Invoke-WebRequest -Uri "https://confluence.intra.eika.no/rest/api/content?spaceKey=ESSDRSE&title=$($confluenceTitle)&expand=space,body.view" `
          -WebSession $session `
          -Method "GET" `
          -Headers @{ `
                     "Cache-Control"="max-age=0"; `
                     "Upgrade-Insecure-Requests"="1"; `
                     "User-Agent"="Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36"; `
                     "Accept"="text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3"; `
                     "Accept-Encoding"="gzip, deflate, br"; `
                     "Accept-Language"="en-US,en;q=0.9"; `
                     "Cookie"="_ga=GA1.2.666718609.1559905434;BIGipServerpool_confluance=126883244.39455.0000; mywork.tab.tasks=false;JSESSIONID=B01D853427CB33DBD6E83FD105ADEF47;seraph.confluence=116064840%3A0b5eb067146b18e58664911de4640033203aa2d9" `
                     }
    return $jsonRequest
}

# Filters out irrelevant elements and objects 
# from the provided JSON object and returns the result
Function Get-ConfluenceWebRequestHtmlElements($jsonRequest) {
    return $jsonRequest.Content |
                ConvertFrom-Json | 
                Select-Object -ExpandProperty results |
                Select-Object -ExpandProperty body | 
                Select-Object -ExpandProperty view |
                Select-Object -ExpandProperty value

}

# Filters out HTML-related tags from the input and 
# returns an array list of access keys and values
Function Get-ConfluenceWebRequestAccessControlData($htmlPage) {
    
    $regexTableBody = "<tbody>(.*?)</tbody>"
    $htmlTable = ($htmlPage | select-string -pattern $regexTableBody).Matches.Value
    
    $regexTableRow = "<tr>(.*?)</tr>"
    $tableRows = ($htmlTable | Select-String -Pattern $regexTableRow -AllMatches).Matches.Value

   
    $regexKeyContents   = "<tr><th colspan=`"5`" class=`"confluenceTh`">(.*?)</th></tr>"
    $regexValueContents = "<td colspan=`"1`" class=`"confluenceTd`">(.*?)</td>"
    
    [System.Collections.ArrayList]$accessRights = @()
    $keyNum                                     = 1
    
    # For each row in the confluence table
    foreach($row in $tableRows) {
        $keyRow    = ($row | Select-String -Pattern $regexKeyContents).Matches
        $valuesRow = ($row | Select-String -Pattern $regexValueContents -AllMatches).Matches
    
        # If confluence header row, extract the key value from the row 
        if(($keyNum -ne 0) -and ($keyRow.Count -eq 1)) {
            $keyNum++
            $key = $keyRow.Value.Replace("<tr><th colspan=`"5`" class=`"confluenceTh`">", "").
                                 Replace("</th></tr>", "")
            $accessRights.add("K: $($key)") | Out-Null

        # If confluence value row, extract the values from the row
        } elseif(($keyNum -ne 0) -and ($keyRow.Count -eq 0)) {
            $valueNum = 0
            # For each column in the value row
            foreach($rowValue in $valuesRow.Value) {
                $value = $rowValue.Replace("<td colspan=`"1`" class=`"confluenceTd`">", "").
                                   Replace("</td>", "").
                                   Replace("<br/>", "")
                # If value contains something
                if($value.Length -gt 0) {
                    # Append coloumn information
                    switch($valueNum) {
                        0 { $value = "(0) " + $value } # Always empty
                        1 { $value = "(1) " + $value } # AD-Group
                        2 { $value = "(2) " + $value } # extensionAttributeId
                        3 { $value = "(3) " + $value } # extensionAttributeValue
                        4 { $value = "(4) " + $value } # SMTP
                    }
                $accessRights.add("V: $($value)") | Out-Null
                }
                # Each column corresponds to value 1-5
                $valueNum++
            }
        }
    }
    return $accessRights
}

# Separates and groups together keys and values correlating
# to each other.
# Calls Set-ConfluenceWebRequestAccessRights() to set the access rights.
# @Requires Import-ExchangeModule
Function Get-ConfluenceWebRequestKeyValues($list, $newUserId, $accessInput) {
    $keyPrefix        = "K: "
    $valuePrefix      = "V: "
    for($ii=0; $ii -lt $list.Length; $ii++) {
        # If key
        if($list[$ii].StartsWith($keyPrefix)) {
            $keyName = $list[$ii].Substring($keyPrefix.Length)
            $jj = $ii + 1
            [System.Collections.ArrayList]$accessValues = @()
            while($list[$jj] -ne $null -and $list[$jj].StartsWith($valuePrefix)) {
                $keyValue  = $list[$jj].Substring($valuePrefix.Length)
                $accessValues.Add($keyValue) | Out-Null
                $jj++
            }
            Set-ConfluenceWebRequestAccessRights $keyname $accessValues $accessInput $newUserId
            $accessValues.Clear()
        }
    }
}

# Sets the provided access rights provided from a Confluence-page and outputs
# each of the given accesses.
# @Requires Import-ExchangeModule
Function Set-ConfluenceWebRequestAccessRights($key, $value, $accessRightInput, $newUserId) {    
    $valuePrefix = "(X)"
    # If user needs the access
    if($accessRightInput[$key] -eq "ja") {
        $extensionAttributeKeyName = ""
        # For each value required to give the access
        $value.Foreach({
            $currentValue = $_.Substring($valuePrefix.Length).Trim()
            $colNumber    = $_.SubString(0, $valuePrefix.Length)
            # Call functions with provided values
            switch($colNumber) {
                "(0)" {
                    # Not used
                }
                "(1)" {
					# AD-Groups
                    Add-ADGroupMember -Identity $currentValue -Members $newUserId
                    Write-Output "AD-Gruppe: $($currentValue)"
                }
                "(2)" {
                    # AD Object property key
                    # The attribute key is stored for the next iteration
                    # the next iteration will always be the attribute value
                    $extensionAttributeKeyName = "extensionAttribute$($currentValue)"
                 }
                "(3)" {
                    # AD Object property name
                    if($extensionAttributeKeyName.Length -gt 0) {
                      Set-ADAttribute $newUserId $extensionAttributeKeyName $currentValue
                      Write-Output "$($extensionAttributeKeyName) = $($currentValue)"
                      $extensionAttributeKeyName = ""
                    }
                 }
                "(4)" {
                    # SMTP 
                    Set-FullAccessToSharedMailbox $currentValue $newUserId $true
                    Write-Output "Send As og Full Access: $($currentValue)"
                 } 
            }
        })
    }
}

# Sends a HTTP POST request to the Eika Admin login page.
# Receives a HTTP session object where you can access
# inlogged functions.
# @Requires access to Eika Admin
Function Invoke-EikaAdminWebRequestEncryptedSSN($userId, $password, $ssn) {
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
              -Body "j_username=$($userId)&j_password=$($password)"

    $r = Invoke-WebRequest -Uri "https://intra.eika.no/wad/ssn/Encrypt.action" `
              -WebSession $session `
              -Method "POST" `
              -Headers @{`
                    "Cache-Control"="max-age=0"; `
                    "Origin"="https://intra.eika.no"; `
                    "Upgrade-Insecure-Requests"="1"; `
                    "User-Agent"="Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36"; `
                    "Accept"="text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3"; `
                    "Referer"="https://intra.eika.no/wad/ssn/Read"; `
                    "Accept-Encoding"="gzip, deflate, br"; `
                    "Accept-Language"="nb-NO,nb;q=0.9,no;q=0.8,nn;q=0.7,en-US;q=0.6,en;q=0.5"; `
                    "Cookie"="JSESSIONID=D26007BD062B71BA13A96F86CF9A3BBF; `
                              _ga=GA1.2.1163989884.1562150487; `
                              _gcl_au=1.1.1727308635.1562825593; `
                              InternalJointSession=edb1f5962f362bf3cc8fdedd0923d5847795afd5c8d7333d3b3cab7229e865d8; BID=2050"`
                    }`
              -ContentType "application/x-www-form-urlencoded" `
              -Body "ssn=$($ssn)"
    return $r.Content
}

# Filters out the encrypted social security number from
# the results of the HTTP POST request to Eika Admin.
# @Return encrypted social security number as a string
Function Get-EikaAdminWebRequestEncryptedSSNValue($html) {
    $htmlPage     = $html -replace '\s', ''
    $regexHtmlSSN = "<divclass=`"well`">(.*?)</div>"
    $htmlSSN      = ($htmlPage | Select-String -Pattern $regexHtmlSSN).Matches.Value
    $regexSSN     = "<b>(.*?)</b>"
    $ssnTemp1     = ($htmlSSN | Select-String -Pattern $regexSSN).Matches.Value
    $htmlStartTag = "<b>"
    $htmlEndTag   = "</b>"
    $ssnTemp2     = $ssnTemp1.Substring($htmlStartTag.Length)
    $ssn          = $ssnTemp2.Substring(0, $ssnTemp2.Length - ($htmlEndTag.Length))
    return $ssn
}

# Takes an HTML element. Checks if this element exists 
# in the DOM and waits 200ms if it does not. Repeats until element
# is found or a total of 10 seconds passes.
Function WaitForDOM($comObject, $htmlElement) {
    $msLimit = 10000
    $ms = 0
    While($comObject.document.getElementsByTagName("body")[0].outerHTML -notmatch $htmlElement -and $ms -le $msLimit) {
        Write-Output $htmlElement
        Start-Sleep -m 200
        $ms += 200
    }
}

# Opens up Internet Explorer and navigates to the page content
# where one can add new agents to Competella Queues for EKS.
Function Invoke-OpenCompetellaQueueHandlingEKS($essUsername, $essPassword) {
    # Open Internet Explorer and display it to the user
    $ie = New-Object -ComObject "InternetExplorer.Application" -ErrorAction 0
    $ie.Navigate("http://competellapool102/WebManagement/")
    $ie.visible = $true

    # Log in
    $waitingElement = '<button class="btn btn-lg competellaBlue btn-block" type="submit">Sign in</button>'
    WaitForDom $ie $waitingElement
    $ie.Document.getElementById("UserName").Value = $essUsername
    $ie.Document.getElementById("Password").Value = $essPassword
    $ie.Document.getElementsByTagName("form")[0].submit()

    # Open Configurations
    $waitingElement = '<small>Copyright © Competella AB 2019</small>'
    WaitForDom $ie $waitingElement
    $ie.Navigate("http://competellapool102/WebManagement/AgentServiceConfigs/Home")

    # Mark Agent Service
    $waitingElement = '<tfoot>'
    WaitForDom $ie $waitingElement
    $ie.Document.getElementsByTagName("table")[0].children[2].children[0].children[0].click()

    # Navigate to Agent Groups
    $waitingElement = '<span class="glyphicon glyphicon-cog"></span>'
    WaitForDom $ie $waitingElement
    $ie.Navigate("http://competellapool102/WebManagement/AgentServiceConfigs/AgentGroups")

    # Choose Bank
    $waitingElement = 'Forsikring ENG'
    WaitForDom $ie $waitingElement
    $ie.document.getElementsByTagName("table")[0].children[2].children[1].children[0].click()

    # Click Member Agents
    $waitingElement = '<label class="alert alert-info ng-binding">Bank</label>'
    WaitForDom $ie $waitingElement
    $ie.document.getElementsByTagName("ul")[8].children[1].children[0].click()

    # Click Add new member
    $waitingElement = 'Add new member'
    WaitForDom $ie $waitingElement
    $ie.document.getElementsByTagName("form")[0].children[0].children[0].children[0].click()
}

# Extracts existing script execution statistics
# from the scripts statistics file. Adds the provided
# values to the existing values and writes the new
# result to the satistics file.
Function Add-TimeSaveData($secondsSaved, $scriptName) {
    $file_statistics = "$($file_local_folder)\statistikk_do_not_delete.txt"
    $rowScript  = "Script:"
    $rowSec     = "Seconds:"
    $rowTimes   = "Times:"
    $lineNumber = -1 # used with array indexes starting at 0
    $numSec     = 0
    $numTimes   = 0
    $found      = $false

    $content = Get-Content $file_statistics
    foreach($line in $content) {
        $lineNumber++
        $dataType  = $line.Split(' ')[0]
        $dataValue = $line.Substring($dataType.Length).Trim()

        # Set found flag if reached correct script section
        if($dataType -eq $rowScript -and $dataValue -eq $scriptName) {
            $found = $true
        }

        # If at correct script section
        if($found) {
            # line containing number of seconds
            if($dataType -eq $rowSec) {
                $numSec    = [int]$dataValue
                $newNumSec = $numSec + $secondsSaved
                $content[$lineNumber] = $content[$lineNumber].Replace($numSec.ToString(), $newNumSec.ToString())
                $content | Set-Content $file_statistics
            # line containing number of times
            } elseif($dataType -eq $rowTimes) {
                $numTimes    = [int]$dataValue
                $newNumTimes = $numTimes  + 1
                $content[$lineNumber] = $content[$lineNumber].Replace($numTimes.ToString(), $newNumTimes.ToString())
                $content | Set-Content $file_statistics
            }
        }

        # If done updating script section
        if($found -eq $true -and $line -eq "") {
            $found = $false
            break
        }

     }
 }