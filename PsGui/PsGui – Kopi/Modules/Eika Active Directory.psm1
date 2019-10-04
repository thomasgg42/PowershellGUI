<#
    A Powershell module for Active Directory related 
    functions.
#>

# Gets a PSCredential object for the provided
# user name and password.
Function Get-AdminCredentials($adminUser, $adminPw) {
	$pw          = ConvertTo-SecureString $adminPw -AsPlainText -Force
	$credentials = New-Object System.Management.Automation.PSCredential($adminUser, $pw)
	return $credentials
}

# Creates and returns a  Powershell user object with all necessary 
# information required to create a basic ADUser object in Eikanett and 
# Terra.local.
# @Requires Eika General-module
# @Requires Eika Skype-module
# @Requires Eika Exchange-module
# @Requires Set-GlobalConfigVariables()
# @Returns a hash table with user and domain properties.
Function Get-NewADUserData($similarUserId, $newGivenName, $newSurName, $newPhoneNum, $newMail, $userType, $userId = "") {
	$newUser 				= @{}
	$similarUser 			= Get-SimilarUserObject($similarUserId)
	$newUser.pwClearText    = Get-FriendlyPassword
    $newUser.eikaPassword   = ConvertTo-SecureString -String $newUser.pwClearText -AsPlainText -Force
	$newUser.terraPasssword = ConvertTo-SecureString -String $newUser.pwClearText -AsPlainText -Force
	$newUser.office 		= $similarUser.Office
	$newUser.department     = $similarUser.Department
	$newUser.givenName      = $newGivenName
    $newUser.surName        = $newSurName
    $newUser.phoneNumber    = $newPhoneNum
    $newUser.email          = $newMail
	$newUser.homeDrive      = $ad_homedrive_letter
	$newUser.description    = ""
	$newUser.displayName    = "$($newUser.givenName) $($newUser.surName)"

	$ouDep     = ($similarUser.CanonicalName -split "/")[4]
	$ouCompany = ($similarUser.CanonicalName -split "/")[3]
	$ouUsers   = ($similarUser.CanonicalName -split "/")[5]
	
	$newUser.eikanettOUPath  = $ad_eikanett_skala_oupath.Replace("AAAAA", $ouUsers).
												Replace("BBBBB", $ouDep).
												Replace("CCCCC", $ouCompany)

	# Adds type-specific attributes to the user object
	switch($userType.ToLower()) {
		"eiendom" {
			# Passes $newUser by reference, not requiring a return value
			Get-NewAktivEiendomADUserData $newUser $similarUser 
			break
		}
		"konsern" {
			# bruker ID
			Get-NewKonsernADUserData $newUser $userId $ouDep $ouCompany $ouUsers
		}
	}

    $newUser.homeDirectory  = $similarUser.HomeDirectory.Replace($similarUserId, $newUser.id)
	$newUser.principalName  = $newUser.id.ToString() + $ad_principal_name_suffix
	
	return $newUser
}

# Creates and returns a Powershell user object with necessary
# information required to create a basic AD-user object in Eikanett.
# @Requires Eika General-module
# @Requires Set-GlobalConfigVariables()
# @Returns a hash table with user and domain properties.
Function Get-NewKonsernUserDataFromTerra($terraUserObject) {
    $user                = @{}
    $user.id             = $terraUserObject.samAccountName
    $user.givenName      = $terraUserObject.givenName
    $user.surName        = $terraUserObject.surName
    $user.displayName    = $terraUserObject.displayName
    $user.phoneNumber    = $terraUserObject.mobile
    $user.office         = $terraUserObject.office
    $user.email          = $terraUserObject.mail
    $user.homeDrive      = $terraUserObject.homeDrive
	$user.department     = $terraUserObject.department
	$user.description    = $terraUserObject.description
    $user.principalName  = $user.id.ToString() + $ad_principal_name_suffix
	$user.eikaPassword   = ConvertTo-SecureString -String Get-FriendlyPassword -AsPlainText -Force
	
    $distinguishedNameCC = ($terraUserObject.CanonicalName -split "/")[2] # Konsern
    $distinguishedNameBB = ($terraUserObject.CanonicalName -split "/")[3] # KO-xxxx
    $distinguishedNameAA = ($terraUserObject.CanonicalName -split "/")[4] # Users

    $user.homeDirectory  = $ad_eikanett_skala_homedirectory.Replace("AAAAA", $user.id).
													 Replace("BBBBB", $distinguishedNameBB).
													 Replace("CCCCC", $distinguishedNameCC)

    $user.eikanettOUPath = $ad_eikanett_skala_oupath.Replace("AAAAA", $distinguishedNameAA).
                                              Replace("BBBBB",$distinguishedNameBB).
                                              Replace("CCCCC",$distinguishedNameCC)

    return $user
}

# Returns a hash table containing all the properties and
# attributes found on an AD user object. The properties
# are given the same name as in AD.
# @Requires Set-GlobalConfigVariables()
# @Return hash table with user properties
Function Get-AdUserObject($userId, $server = "") {
	if($server -eq "") {
		$server = $ad_eikanett_fqdn
	}
	$adObj = Get-ADUser -Identity $userId -Server $server -Properties *
	return $adObj
}

# Creates a user AD object in Active Directory 
# in the Eikanett domain based on the supplied 
# local user object.
# @Requires Set-GlobalConfigVariables()
Function New-EikanettUserObject($user) {
	try {
		New-ADUser -Server (Get-ADDomainController -Server $ad_eikanett_fqdn).Hostname `
			-Name $user.id `
			-GivenName $user.givenName `
			-Surname $user.surName `
			-DisplayName $user.displayName `
			-MobilePhone $user.phoneNumber `
			-Office $user.office `
			-Department $user.department `
			-EmailAddress $user.email `
			-HomeDrive $user.homeDrive `
			-HomeDirectory $user.homeDirectory `
			-UserPrincipalName $user.principalName `
			-Description $user.description `
			-Path $user.eikanettOUPath `
			-AccountPassword $user.eikaPassword `
			-Enabled $true
	} catch {
		Write-Error "Failed to create the user object in the Eikanett domain."
		# Throw terminates the script and outputs a detailed exception.
		throw $_
	}
}

# Creates a user AD object in Active Directory 
# in the Terra domain based on the supplied local 
# user object.
# @Requires Set-GlobalConfigVariables()
Function New-TerraUserObject($user) {
	try {
		New-ADUser -Server (Get-ADDomainController -Server $ad_terra_fqdn).Hostname `
			-Name $user.id `
			-GivenName $user.givenName `
			-Surname $user.surName `
			-DisplayName $user.displayName `
			-MobilePhone $user.phoneNumber `
			-Office $user.office `
			-EmailAddress $user.email `
			-UserPrincipalName $user.principalName `
			-Path $user.terraOUPath `
			-AccountPassword $user.terraPasssword `
			-Enabled $false
	} catch {
		Write-Error "Failed to create the user object in the Terra domain."
		# Throw terminates the script and outputs a detailed exception.
		throw $_
	}
}

# Copies all the AD-groups from one user to another.
# Outputs an error to the console if a group cannot be copied.
# @Requires Set-GlobalConfigVariables()
Function Copy-EikanettUserGroups($fromUserId, $toUserId, $fromServer = "") {
	if($fromServer -eq "") {
		$fromServer = $ad_eikanett_fqdn
	}
    $groups = (Get-ADPrincipalGroupMembership -Server $fromServer -Identity $fromUserId)
    foreach($group in $groups) {
		try {
			Add-ADGroupMember -Identity $group.SamAccountName `
							  -Members $toUserId
		} catch {
			Write-Error "Could not copy $($group). Likely due to lack of permissions."
		}
    }
}

# Sets a new AD user password to the provided user 
# with the provided password in the provided domain.
# @Requires Set-GlobalConfigVariables()
Function Set-NewPassword($userId, $password, $domain) {
	$domainHostname = ""
	if($domain.ToLower() -eq "eikanett") {
		$domainHostname = (Get-ADDomainController -Server $ad_eikanett_fqdn).Hostname
	} elseif($domain.ToLower() -eq "terra") {
		$domainHostname = (Get-ADDomainController -Server $ad_terra_fqdn).Hostname
	}
	
	Set-ADAccountPassword -Identity $userId `
						  -Server $domainHostname `
						  -Reset `
						  -NewPassword (ConvertTo-SecureString -AsPlainText $password -Force)
}

# Gets the provided AD user's phone number.
# @Returns user's phone number as a string.
Function Get-PhoneNumber($userId) {
	return (Get-ADUser -identity $userId -properties Mobile).Mobile
}

# Gets the Security Identifier (SID) belonging
# to the given AD user object.
# @Returns the user SID as a string.
Function Get-UserSID($userID) {
	$SID = (Get-ADUser -Identity $userID -Properties SID).sid.value
	return $SID
}

# Sets the provided attribute key with the provided attribute value
# on the provided user object.
Function Set-ADAttribute($userId, $attributeName, $attributeValue) {
	$existingValue = (Get-ADUser -Identity $userId -Properties $attributeName).$attributeName
	if($existingValue.Length -le 0) {
		Set-ADUser -Identity $userId `
				   -Add @{ $attributeName = $attributeValue }
	} elseif($existingValue.Length -ge 1) {
		Set-ADUser -Identity $userId `
				   -Replace @{ $attributeName = $attributeValue }
	} elseif($existingValue.Length -eq 1 -and $existingValue -eq " ") {
		Set-ADUser -Identity $userId `
				   -Clear $attributeName
	}
}

# Runs the skype fix
Function Set-SkypeFix($userId) {
	$userSid = Get-UserSID $userId
	Set-ADUser -Identity $userId `
	           -Replace @{'msRTCSIP-OriginatorSID'=$userSid} `
			   -Server (Get-ADDomainController -Server $ad_terra_fqdn).Hostname
}

# Fixes "Kan ikke legge til en eller flere brukere i tilgangslisten
# for mappen. Kan ikke gi rettigheter på serveren til brukere som ikke
# er lokale." on user mailboxes.
Function Set-CalendarSharingFix($userId) {
	# If not working, use Set-Mailbox -Type Regular
	$accessControlListAbleUserMailbox = "1073741824"
	Set-ADUser -Identity $userId `
	-Replace @{'msExchRecipientDisplayType'=$accessControlListAbleUserMailbox}
}

# Gets the user type from the AD-user's canonical name.
# @Returns a string with the user type. Ex: Konsern/Bank/eiendom
Function Get-UserType($userId) {
	$canonical = (Get-ADUser -Identity $userId -Properties CanonicalName).CanonicalName
	return $canonical.Split("/")[3]
}

# Gets the PI-number belonging to a bank user.
# @Returns a string with the PI-number to the given bank user.
Function Get-BankPINumber($userId) {
	return (((Get-ADUser -Identity $userId -Properties CanonicalName).CanonicalName).Split("/")[4]).SubString(2)
}

######## HELPER FUNCTIONS ########

# Returns the next available Aktiv Eiendom-ID with the
# prefix ad_aktiv_account_prefix concatenated with a number 
# in the interval [ad_aktiv_account_start_number -
# ad_aktiv_account_end_number].
# If all user IDs from ad_aktiv_account_start_number
# to ad_aktiv_account_end_number are taken, 0 is returned.
# @Requires Set-GlobalConfigVariables()
# @Returns a string, ex: H905020.
Function Get-NextAvailableEiendomUserId {
    $accountNumber = [int]$ad_aktiv_account_start_number
    $id = 0
    while($accountNumber -le $ad_aktiv_account_end_number) {
       $id = ($ad_aktiv_account_prefix) + $accountNumber++
       $usr = (Get-ADUser -Filter "SamAccountName -eq '$id'")
       if(!$usr) {
         break
       }
    }
    return $id
}

# Gets an AD user object with properties relevant
# to creating a new user based on this user.
# @Returns an ADUser object.
Function Get-SimilarUserObject($userID) {
    $similarUser = Get-ADUser -Identity $userID `
                               -Properties Department, HomeDirectory, `
                                           Office, GivenName, SurName, `
										   DisplayName, DistinguishedName, `
										   CanonicalName
    return $similarUser
}

# Gets the Security Identifier (SID) belonging
# to the given AD user object.
# @Returns the user SID as a string.
Function Get-UserSID($userID) {
	$SID = (Get-ADUser -Identity $userID -Properties SID).sid.value
	return $SID
}

# Sets Aktiv Eiendom specific attributes to the provided user object
# by reference.
# @Requires Set-GlobalConfigVariables()
Function Get-NewAktivEiendomADUserData($newUser, $similarUser) {
	$newUser.id = Get-NextAvailableEiendomUserId
	$newUser.userOU = $ad_aktiv_dep_prefix + ($similarUser.Department.ToString())
	$newUser.terraOUPath = $ad_terra_skala_oupath.Replace("AAAAA", "Users").
												  Replace("BBBBB", "TM").
												  Replace("CCCCC", "Eiendom")
	$newUser.extAttr11   = $ad_aktiv_ext11
	
	# Webmegler requires a different Terra-pw (31.01.19)
	$newUser.terraPasssword = ConvertTo-SecureString -String Get-FriendlyPassword -AsPlainText -Force
}

# Sets Konsern specific attributes to the provided user object
# by reference.
# @Requires Set-GlobalConfigVariables()
Function Get-NewKonsernADUserData($newUser, $userId, $ouDep, $ouCompany, $ouUsers) {
    # Realised child functions can access all parent variables
    # using arguments anyway to clarify.
	$newUser.id = $userId
	$newUser.terraOUPath = $ad_terra_skala_oupath.Replace("AAAAA", $ouUsers).
												  Replace("BBBBB", $ouDep).
												  Replace("CCCCC", $ouCompany)
}