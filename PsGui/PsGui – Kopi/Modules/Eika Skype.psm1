<#
    A Powershell module for Skype related functions.
#>

# Imports the Skype Powershell module.
# @Requires Set-GlobalConfigVariables()
Function Import-SkypeModule($adminUser, $adminPw) {
	$credentials = Get-AdminCredentials $adminUser $adminPw
	try {
		Enter-PSSession -ComputerName $skype_server_uri -Credential $credentials -Authentication CredSSP
        Import-module lync
	} catch {
		Write-Error "Failed to import the Skype module!"
		# Throw terminates the script and outputs a detailed exception.
		throw $_
	}
}


# Enables skype for the supplied user ID
# @Requires Set-GlobalConfigVariables()
Function Enable-Skype($userId) {
    $skypeId     = ($skype_id_prefix + $userId)
    
    Enable-CsUser -Identity $skypeId `
                  -RegistrarPool $skype_registrar_pool `
                  -SipAddressType Emailaddress
    Start-Sleep 20
	Set-SkypePolicies $skypeId
	# Obs! Set-SkypeFix is an AD-fix, thus not in terra
	Set-SkypeFix $userId
}

# Sets the external access and conferencing policies to
# the provided skype user id.
# @Requires Set-GlobalConfigVariables()
Function Set-SkypePolicies($skypeId) {
	Grant-CsExternalAccessPolicy -Identity $skypeId `
                             	 -PolicyName $skype_external_access_policy
    Grant-CsConferencingPolicy -Identity $skypeId `
                               -PolicyName $skype_conferencing_policy
}