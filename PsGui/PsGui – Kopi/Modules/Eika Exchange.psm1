<#
    A Powershell module for Exchange related functions.
#>

# Imports the Exchange Powershell module.
# @Requires Set-GlobalConfigVariables()
Function Import-ExchangeModule {
	try {
		$session = New-PSSession -ConfigurationName Microsoft.Exchange `
								-ConnectionUri $exch_server_uri `
								-Authentication Kerberos `
								-AllowRedirection
	} catch {
		Write-Error "Failed to import the Exchange module!"
		# Throw terminates the script and outputs a detailed exception.
		throw $_
	}
    Import-Module (Import-PSSession -Session $session -AllowClobber) -Global
}

# Enables mailbox of the supplied user and sets 
# the primary SMTP to the users primary e-mail address.
# @Requires Import-ExchangeModule
Function Enable-UserMailbox($userId, $userMail) {
	Enable-Mailbox -Identity $userId `
				   -Alias $userId `
				   -PrimarySmtpAddress $userMail `
				   -ErrorAction Stop
	Set-Mailbox -Identity $userId -EmailAddresses @{add="SIP:$userMail"}
}

# Creates a new shared mailbox with the provided arguments.
# @Requires Set-GlobalConfigVariables()
Function Enable-SharedMailbox($displayName, $samAccountName, $smtp, $ouName) {
	New-Mailbox -Shared `
	            -Name $displayName `
				-DisplayName $displayName `
				-Alias $samAccountName `
				-OrganizationalUnit $ouName `
				-UserPrincipalName "$($samAccountName)$($ad_principal_name_suffix)" `
				-PrimarySmtpAddress $smtp
}

# Sets the standard properties used on shared mailboxes.
Function Set-SharedMailboxProperties($samAccountName, $extAttr11) {
	Set-Mailbox -Identity $samAccountName `
            -CustomAttribute11 $extAttr11 `
            -MessageCopyForSentAsEnabled $true `
            -MessageCopyForSendOnBehalfEnabled $true
}

# Sets Send As and Full Access rights to the provided mailbox
# with the provided user Id.
Function Set-FullAccessToSharedMailbox($mailboxAddress, $userId, $enableAutoMapping) {
	Add-MailboxPermission -Identity $mailboxAddress `
						   -User $userId `
						   -AccessRights "FullAccess" `
						   -Automapping $enableAutoMapping
	Get-Mailbox -Identity $mailboxAddress | Add-ADPermission -User $userId `
															 -AccessRights "ExtendedRight" `
															 -ExtendedRights "Send As"
}

# Sends a mail to the provided address, from the provided 
# address with the provided message. The message body
# must be encapsulated in <html> tags </html> where 
# <br> represents a new line
# @Requires Set-GlobalConfigVariables()
Function Send-Email($fromAddr, $toAddr, $subject, $message) {
	Send-MailMessage -To $toAddr `
					 -From $fromAddr `
					 -Subject $subject `
					 -SmtpServer $exch_smtp_server `
					 -Encoding $exch_mail_encoding `
					 -BodyAsHtml `
					 -Body $message
}

# Gets the exchange logs with the provided parameters for filtering. 
# senderOrReceiver value options: "sender", "receiver"
# date format: mm/dd/yyyy
Function Get-ExchangeLogs($mail, $senderOrReceiver, $startDate, $endDate, $messageSubject) {
	$servers = Get-ExchangeServer
	foreach($server in $servers) { 
		if($senderOrReceiver -eq "sender") {
			return (Get-MessageTrackingLog -Sender $mail `
									-Start $startDate `
									-End $endDate `
									-MessageSubject $messageSubject)
		} elseif($senderOrReceiver -eq "receiver") {
			return (Get-MessageTrackingLog -Recipients $mail `
									-Start $startDate `
									-End $endDate `
									-MessageSubject $messageSubject)
		}
	}
}

# Sets the primary SMTP address for the provided user.
Function Set-PrimarySmtpAddress($userId, $email) {
	Set-Mailbox $userId -PrimarySmtpAddress $email -EmailAddressPolicyEnabled $false
	#EmailAddressPolicyEnabled tilsvarer avhukningsboks i GUI
}

# Sets the HiddenFromAddressListsEnabled flag to true for the
# provided user.
Function Set-HiddenFromAddressLists($userId) {
	Set-Mailbox -Identity $userId -HiddenFromAddressListsEnabled $true
}

# Sets auto reply on the provided mailbox. The given date formats must 
# match "mm/dd/yy hh:mm:ss" or "mm/ddyy". Date formats can also contain
# an empty space, this indicates unlimited length.
# The provided message shall not contain any HTML tags.
Function Set-MailboxAutoReply($userId, $message, $dateTimeStart, $dateTimeEnd) {
	# If unlimited time span
	if(($dateTimeStart.Length -eq 1 -and $dateTimeStart -eq " ") `
	    -and ($dateTimeEnd.Length -eq 1 -and $dateTimeEnd -eq " ")) {
		Set-MailboxAutoReplyConfiguration $userId `
			-AutoReplyState Enabled `
			-ExternalMessage $message `
			-InternalMessage $message
	# If defined time span
	} elseif($dateTimeStart.length -ge 1 -and $dateTimeEnd.length -ge 1) {
		Set-MailboxAutoReplyConfiguration $userId `
			-AutoReplyState Scheduled `
			-StartTime $dateTimeStart `
			-EndTime $dateTimeEnd `
			-ExternalMessage $message `
			-InternalMessage $message
	}
}

# Provides full access rights to the provided shared e-mail account.
# The full access rights are given to the provided group.
Function Set-MailboxPermissionFullAccess($accountName, $groupName) {
	Add-MailboxPermission -Identity $accountName -AccessRights "FullAccess" -User $groupName
}