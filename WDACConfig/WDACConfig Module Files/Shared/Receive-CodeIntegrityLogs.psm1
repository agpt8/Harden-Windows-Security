Function Receive-CodeIntegrityLogs {
    <#
    .SYNOPSIS
        A high-performance function that:
        Retrieves the Code Integrity Operational logs and App Locker logs
        Fixes the paths to the files that are being logged
        Separates events based on their type: Audit or Blocked
        Separates events based on their file paths: existing or deleted
        For Code Integrity logs: Finds correlated events with ID 3089 and adds them to the main event (IDs 3076 and 3077)
        For App Locker logs: Finds correlated events with ID 8038 and adds them to the main event (IDs 8028 and 8029)
        Replaces many numbers in the logs with user-friendly strings
        Performs precise de-duplication of the logs so that the output will always have unique logs
        Then processes the output based on different criteria
    .PARAMETER Date
        The date from which the logs should be collected. If not specified, all logs will be collected.
        It accepts empty strings, nulls, and whitespace and they are treated as not specified.
    .PARAMETER Type
        The type of logs to be collected. Audit or Blocked. The default value is 'Audit'
    .PARAMETER PostProcessing
        How to process the output for different scenarios
        OnlyExisting: Returns only the logs of files that exist on the disk
        OnlyDeleted: Returns only the hash details of files that do not exist on the disk
        Separate: Returns the file paths of files that exist on the disk and the hash details of files that do not exist on the disk, separately in a nested object
    .PARAMETER PolicyNames
        The names of the policies to filter the logs by
    .PARAMETER Category
        The category of logs to be collected. Code Integrity, AppLocker, or All. The default value is 'All'
    .PARAMETER LogSource
        The source of the logs. EVTXFiles or LocalLogs. The default value is 'LocalLogs'
    .PARAMETER EVTXFilePaths
        The file paths of the EVTX files to collect the logs from. It accepts an array of FileInfo objects
    .INPUTS
        System.String
        System.String[]
        System.IO.FileInfo[]
    .OUTPUTS
        System.Collections.Hashtable
    .NOTES
        The extra functionalities for post processing such as Separated output and Deleted outputs have been commented
        out because they are not used anymore by the module.
    #>
    [CmdletBinding()]
    [OutputType([System.Collections.Hashtable])]
    param(
        [AllowEmptyString()]
        [AllowNull()]
        [Parameter(Mandatory = $false)]
        [System.String]$Date,

        [ValidateSet('Audit', 'Blocked')]
        [Parameter(Mandatory = $false)]
        [System.String]$Type = 'Audit',

        # [ValidateSet('OnlyExisting', 'OnlyDeleted' , 'Separate')]
        [ValidateSet('OnlyExisting')]
        [parameter(mandatory = $false)]
        [System.String]$PostProcessing,

        [AllowEmptyString()]
        [AllowNull()]
        [parameter(mandatory = $false)]
        [System.String[]]$PolicyNames,

        [ValidateSet('CodeIntegrity', 'AppLocker', 'All')]
        [Parameter(mandatory = $false)][System.String]$Category = 'All',

        [ValidateSet('EVTXFiles', 'LocalLogs')]
        [Parameter(Mandatory = $false)][System.String]$LogSource = 'LocalLogs',

        [Parameter(Mandatory = $false)][System.IO.FileInfo[]]$EVTXFilePaths
    )
    Begin {
        . "$([WDACConfig.GlobalVars]::ModuleRootPath)\CoreExt\PSDefaultParameterValues.ps1"

        # Importing the required sub-modules
        Import-Module -FullyQualifiedName "$([WDACConfig.GlobalVars]::ModuleRootPath)\Shared\Get-GlobalRootDrives.psm1" -Force

        #Region Application Control event tags intelligence

        # Requested and Validated Signing Level Mappings: https://learn.microsoft.com/en-us/windows/security/application-security/application-control/windows-defender-application-control/operations/event-tag-explanations#requested-and-validated-signing-level
        [System.Collections.Hashtable]$ReqValSigningLevels = @{
            0us  = "Signing level hasn't yet been checked"
            1us  = 'File is unsigned or has no signature that passes the active policies'
            2us  = 'Trusted by Windows Defender Application Control policy'
            3us  = 'Developer signed code'
            4us  = 'Authenticode signed'
            5us  = 'Microsoft Store signed app PPL (Protected Process Light)'
            6us  = 'Microsoft Store-signed'
            7us  = 'Signed by an Antimalware vendor whose product is using AMPPL'
            8us  = 'Microsoft signed'
            11us = 'Only used for signing of the .NET NGEN compiler'
            12us = 'Windows signed'
            14us = 'Windows Trusted Computing Base signed'
        }

        # SignatureType Mappings: https://learn.microsoft.com/en-us/windows/security/application-security/application-control/windows-defender-application-control/operations/event-tag-explanations#signaturetype
        [System.Collections.Hashtable]$SignatureTypeTable = @{
            0us = "Unsigned or verification hasn't been attempted"
            1us = 'Embedded signature'
            2us = 'Cached signature; presence of a CI EA means the file was previously verified'
            3us = 'Cached catalog verified via Catalog Database or searching catalog directly'
            4us = 'Uncached catalog verified via Catalog Database or searching catalog directly'
            5us = 'Successfully verified using an EA that informs CI that catalog to try first'
            6us = 'AppX / MSIX package catalog verified'
            7us = 'File was verified'
        }

        # VerificationError mappings: https://learn.microsoft.com/en-us/windows/security/application-security/application-control/windows-defender-application-control/operations/event-tag-explanations#verificationerror
        [System.Collections.Hashtable]$VerificationErrorTable = @{
            0us  =	'Successfully verified signature.'
            1us  =	'File has an invalid hash.'
            2us  =	'File contains shared writable sections.'
            3us  =	"File isn't signed."
            4us  =	'Revoked signature.'
            5us  =	'Expired signature.'
            6us  =	"File is signed using a weak hashing algorithm, which doesn't meet the minimum policy."
            7us  =	'Invalid root certificate.'
            8us  =	'Signature was unable to be validated; generic error.'
            9us  =	'Signing time not trusted.'
            10us =	'The file must be signed using page hashes for this scenario.'
            11us =	'Page hash mismatch.'
            12us =	'Not valid for a PPL (Protected Process Light).'
            13us =	'Not valid for a PP (Protected Process).'
            14us =	'The signature is missing the required ARM processor EKU.'
            15us =	'Failed WHQL check.'
            16us =	'Default policy signing level not met.'
            17us =	"Custom policy signing level not met; returned when signature doesn't validate against an SBCP-defined set of certs."
            18us =	'Custom signing level not met; returned if signature fails to match CISigners in UMCI.'
            19us =	'Binary is revoked based on its file hash.'
            20us =	"SHA1 cert hash's timestamp is missing or after valid cutoff as defined by Weak Crypto Policy."
            21us =	'Failed to pass Windows Defender Application Control policy.'
            22us =	'Not Isolated User Mode (IUM) signed; indicates an attempt to load a standard Windows binary into a virtualization-based security (VBS) trustlet.'
            23us =	"Invalid image hash. This error can indicate file corruption or a problem with the file's signature. Signatures using elliptic curve cryptography (ECC), such as ECDSA, return this VerificationError."
            24us =	'Flight root not allowed; indicates trying to run flight-signed code on production OS.'
            25us =	'Anti-cheat policy violation.'
            26us =	'Explicitly denied by WDAC policy.'
            27us =	'The signing chain appears to be tampered / invalid.'
            28us =	'Resource page hash mismatch.'
        }

        #EndRegion Application Control event tags intelligence

        Function Test-NotEmpty ($Data) {
            <#
            .SYNOPSIS
                Tests if the data is not null, empty, or whitespace
            #>
            if ((-NOT ([System.String]::IsNullOrWhiteSpace($Data)))) {
                if ($Data.count -ge 1) {
                    return $true
                }
                else {
                    return $false
                }
            }
            else {
                return $false
            }
        }

        # Validate the date provided if it's not null or empty or whitespace
        if (-NOT ([System.String]::IsNullOrWhiteSpace($Date))) {
            if (-NOT ([System.DateTime]::TryParse($Date, [ref]$Date))) {
                Throw 'The date provided is not in a valid DateTime type.'
            }
        }

        #Region Global Root Drive Fix
        Try {
            # Set a flag indicating that the alternative drive letter mapping method is not necessary unless the primary method fails
            [System.Boolean]$AlternativeDriveLetterFix = $false

            # Get the local disks mappings
            [PSCustomObject[]]$DriveLettersGlobalRootFix = Get-GlobalRootDrives
        }
        catch {
            Write-Verbose -Verbose -Message 'Receive-CodeIntegrityLogs: Could not get the drive mappings from the system using the primary method, trying the alternative method now'

            # Set the flag to true indicating the alternative method is being used
            $AlternativeDriveLetterFix = $true
        }

        # Create a hashtable of partition numbers and their associated drive letters
        [System.Collections.Hashtable]$DriveLetterMappings = @{}

        # Get all partitions and filter out the ones that don't have a drive letter and then add them to the hashtable with the partition number as the key and the drive letter as the value
        foreach ($Drive in (Get-Partition | Where-Object -FilterScript { $_.DriveLetter })) {
            $DriveLetterMappings[[System.String]$Drive.PartitionNumber] = [System.String]$Drive.DriveLetter
        }
        #Endregion Global Root Drive Fix

        if ($Category -in 'All', 'CodeIntegrity') {
            Try {
                Write-Verbose -Message 'Receive-CodeIntegrityLogs: Collecting the Code Integrity Operational logs'
                switch ($LogSource) {
                    'EVTXFiles' {
                        # Get all of the Code Integrity logs from the specified EVTX files
                        [System.Diagnostics.Eventing.Reader.EventLogRecord[]]$CiRawEventLogs = Get-WinEvent -FilterHashtable @{Path = $EVTXFilePaths; ID = '3076', '3077', '3089' }
                    }
                    'LocalLogs' {
                        # Get all of the Code Integrity logs from the local machine
                        [System.Diagnostics.Eventing.Reader.EventLogRecord[]]$CiRawEventLogs = Get-WinEvent -FilterHashtable @{LogName = 'Microsoft-Windows-CodeIntegrity/Operational' }
                    }
                }
            }
            catch {
                Write-Verbose -Message "Receive-CodeIntegrityLogs: Could not collect the Code Integrity Operational logs, the number of logs collected is $($CiRawEventLogs.Count)"
            }

            [Microsoft.PowerShell.Commands.GroupInfo[]]$CiGroupedEvents = $CiRawEventLogs | Group-Object -Property ActivityId
            Write-Verbose -Message "Receive-CodeIntegrityLogs: Grouped the Code Integrity logs by ActivityId. The total number of groups is $($CiGroupedEvents.Count) and the total number of logs in the groups is $($CiGroupedEvents.Group.Count)"
        }
        else {
            Write-Verbose -Message 'Receive-CodeIntegrityLogs: Skipping the collection of the Code Integrity logs'
        }

        if ($Category -in 'All', 'AppLocker') {
            Try {
                Write-Verbose -Message 'Receive-CodeIntegrityLogs: Collecting the AppLocker logs'
                switch ($LogSource) {
                    'EVTXFiles' {
                        # Get all of the AppLocker logs from the specified EVTX files
                        [System.Diagnostics.Eventing.Reader.EventLogRecord[]]$AppLockerRawEventLogs = Get-WinEvent -FilterHashtable @{Path = $EVTXFilePaths; ID = '8028', '8029', '8038' }
                    }
                    'LocalLogs' {
                        # Get all of the AppLocker logs from the local machine
                        [System.Diagnostics.Eventing.Reader.EventLogRecord[]]$AppLockerRawEventLogs = Get-WinEvent -FilterHashtable @{LogName = 'Microsoft-Windows-AppLocker/MSI and Script' }
                    }
                }
            }
            catch {
                Write-Verbose -Message "Receive-CodeIntegrityLogs: Could not collect the AppLocker logs, the number of logs collected is $($AppLockerRawEventLogs.Count)"
            }

            [Microsoft.PowerShell.Commands.GroupInfo[]]$AppLockerGroupedEvents = $AppLockerRawEventLogs | Group-Object -Property ActivityId
            Write-Verbose -Message "Receive-CodeIntegrityLogs: Grouped the AppLocker logs by ActivityId. The total number of groups is $($AppLockerGroupedEvents.Count) and the total number of logs in the groups is $($AppLockerGroupedEvents.Group.Count)"
        }
        else {
            Write-Verbose -Message 'Receive-CodeIntegrityLogs: Skipping the collection of the AppLocker logs'
        }

        # Add Code Integrity and AppLocker logs to a single array based on the selected category
        $AccumulatedGroupedEvents = ($Category -eq 'All') ? ($CiGroupedEvents + $AppLockerGroupedEvents) : (($Category -eq 'CodeIntegrity') ? $CiGroupedEvents : $AppLockerGroupedEvents)

        # Create a list of HashTables to store the event log data
        $EventPackageCollection = New-Object -TypeName 'System.Collections.Generic.List[System.Collections.Hashtable]'

        # Loop over each group of logs to identify Audit/Block events and also gather correlated events of each main event
        Foreach ($RawLogGroup in $AccumulatedGroupedEvents) {

            # Process Audit events
            if (($RawLogGroup.Group.Id -contains '3076') -or ($RawLogGroup.Group.Id -contains '8028')) {

                # Finding the main event in the group - If there are more than 1, selecting the first one because that means the same event was triggered by multiple deployed policies
                [System.Diagnostics.Eventing.Reader.EventLogRecord]$AuditTemp = $RawLogGroup.Group.Where({ $_.Id -in '3076', '8028' }) | Select-Object -First 1

                # If the main event is older than the specified date, skip it
                if (-NOT ([System.String]::IsNullOrWhiteSpace($Date))) {
                    if ($AuditTemp.TimeCreated -lt $Date) {
                        continue
                    }
                }

                # Create a local hashtable to store the main event and the correlated events
                [System.Collections.Hashtable]$LocalAuditEventPackageCollections = @{}

                $LocalAuditEventPackageCollections['MainEventData'] = $AuditTemp
                $LocalAuditEventPackageCollections['CorrelatedEventsData'] = $RawLogGroup.Group.Where({ $_.Id -in '3089', '8038' })
                $LocalAuditEventPackageCollections['Type'] = 'Audit'

                # Add the main event along with the correlated events as a nested hashtable to the list
                $EventPackageCollection.Add($LocalAuditEventPackageCollections)
            }

            # Process Blocked events
            if (($RawLogGroup.Group.Id -contains '3077') -or ($RawLogGroup.Group.Id -contains '8029')) {

                # Finding the main event in the group - If there are more than 1, selecting the first one because that means the same event was triggered by multiple deployed policies
                [System.Diagnostics.Eventing.Reader.EventLogRecord]$BlockedTemp = $RawLogGroup.Group.Where({ $_.Id -in '3077', '8029' }) | Select-Object -First 1

                # If the main event is older than the specified date, skip it
                if (-NOT ([System.String]::IsNullOrWhiteSpace($Date))) {
                    if ($BlockedTemp.TimeCreated -lt $Date) {
                        continue
                    }
                }

                # Create a local hashtable to store the main event and the correlated events
                [System.Collections.Hashtable]$LocalBlockedEventPackageCollections = @{}

                $LocalBlockedEventPackageCollections['MainEventData'] = $BlockedTemp
                $LocalBlockedEventPackageCollections['CorrelatedEventsData'] = $RawLogGroup.Group.Where({ $_.Id -in '3089', '8038' })
                $LocalBlockedEventPackageCollections['Type'] = 'Blocked'

                # Add the main event along with the correlated events as a nested hashtable to the list
                $EventPackageCollection.Add($LocalBlockedEventPackageCollections)
            }
        }

        # Hashtable that contains the entire output
        [System.Collections.Hashtable]$Output = @{
            # all the logs without post-processing
            All      = @{
                Audit   = @{}
                Blocked = @{}
            }
            # only the logs of files that exist on the disk
            Existing = @{
                Audit   = @{}
                Blocked = @{}
            }
            <#
            # only the hash details of files no longer on the disk
            Deleted   = @{
                Audit   = @{}
                Blocked = @{}
            }
            # FilePaths of files on the disk and hash details of files not on the disk
            Separated = @{
                Audit   = @{
                    AvailableFilesPaths = [System.Collections.Generic.HashSet[System.String]] @()
                    DeletedFileHashes   = @{}
                }
                Blocked = @{
                    AvailableFilesPaths = [System.Collections.Generic.HashSet[System.String]] @()
                    DeletedFileHashes   = @{}
                }
            }
            #>
        }

        # Making the hashtable thread-safe by synchronizing it and allowing the Foreach-Object -Parallel to write back data to it safely in real time with $Using scope modifier
        # ForEach-Object -Parallel is Thread Session so the scriptblock inside of it can modify parent scope variables since they are references instead of independent copies
        # https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_remote_variables#other-situations-where-the-using-scope-modifier-is-needed
        # https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_scopes#the-using-scope-modifier
        $Output = [System.Collections.Hashtable]::Synchronized($Output)
    }

    Process {

        if ($EventPackageCollection.count -eq 0) {
            Write-Verbose -Message 'Receive-CodeIntegrityLogs: No logs were collected'
            return
        }

        # Split the main hashtable into 5 arrays to run the main loop in parallel
        # https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.chunk
        $SplitArrays = [System.Linq.Enumerable]::Chunk($EventPackageCollection, [System.Math]::Ceiling($EventPackageCollection.Count / 5))

        # Running the main loop in parallel
        $SplitArrays | ForEach-Object -Parallel {

            # Making the parent scope variables available in the parallel child scope as references

            # Only variable modified from within the thread session
            $Output = $using:Output

            # Variables that are not modified from within the thread session
            $DriveLettersGlobalRootFix = $using:DriveLettersGlobalRootFix
            $ReqValSigningLevels = $using:ReqValSigningLevels
            $SignatureTypeTable = $using:SignatureTypeTable
            $VerificationErrorTable = $using:VerificationErrorTable
            $AlternativeDriveLetterFix = $using:AlternativeDriveLetterFix
            $DriveLetterMappings = $using:DriveLetterMappings
            $LogSource = $using:LogSource

            # Set the VerbosePreference to the parent scope's value
            $VerbosePreference = $using:VerbosePreference

            # Loop over each event package in the collection
            foreach ($EventPackage in $_.GetEnumerator()) {

                # Extract the main event data
                [System.Diagnostics.Eventing.Reader.EventLogRecord]$Event = $EventPackage.MainEventData

                # Convert the main event data to XML object
                $Xml = [System.Xml.XmlDocument]$Event.ToXml()

                if ($null -eq $Xml.event.EventData.data) {
                    Write-Verbose -Message "Receive-CodeIntegrityLogs: Skipping Main event data for: $($Log['File Name'])"
                    continue
                }

                # Place each main event data in a hashtable
                [System.Collections.Hashtable]$Log = @{}
                foreach ($Item in $Xml.event.EventData.data) {
                    $Log[$Item.Name] = $Item.'#text'
                }

                # Add the TimeCreated property to the $Log hashtable
                $Log['TimeCreated'] = $Event.TimeCreated
                # Add the ActivityId property to the $Log hashtable
                $Log['ActivityId'] = $Event.ActivityId
                # Add the UserId property to the $Log hashtable
                $Log['UserId'] = $Event.UserId
                # Add the ProviderName property to the $Log hashtable
                $Log['ProviderName'] = $Event.ProviderName

                # Filter the logs based on the policy that generated them
                if (-NOT ([System.String]::IsNullOrWhiteSpace($PolicyNames))) {
                    if ($Log.PolicyName -notin $PolicyNames) {
                        continue
                    }
                }

                # Define the regex pattern for the device path
                [System.Text.RegularExpressions.Regex]$Pattern = '\\Device\\HarddiskVolume(?<HardDiskVolumeNumber>\d+)\\(?<RemainingPath>.*)$'

                # These are the properties that are different in AppLocker so they need to be manually set to be compliant with the expected output of this function
                if ($Log['ProviderName'] -eq 'Microsoft-Windows-AppLocker') {

                    # Replace File Name property with the FilePath property and then remove the FilePath property
                    $Log['File Name'] = $Log['FilePath']
                    $Log.Remove('FilePath')

                    $Log['SHA256 Hash'] = $Log['Sha256Hash']
                    $Log.Remove('Sha256Hash')

                    $Log['SHA1 Hash'] = $Log['Sha1Hash']
                    $Log.Remove('Sha1Hash')
                }

                # replace the device path with the drive letter if it matches the pattern
                # Only if the log source is local logs
                if (($LogSource -eq 'LocalLogs') -and ($Log['File Name'] -match $Pattern)) {

                    # Use the primary method to fix the drive letter mappings
                    if ($AlternativeDriveLetterFix -eq $false) {

                        [System.UInt32]$HardDiskVolumeNumber = $Matches['HardDiskVolumeNumber']
                        [System.String]$RemainingPath = $Matches['RemainingPath']
                        [PSCustomObject]$GetLetter = $DriveLettersGlobalRootFix.Where({ $_.DevicePath -eq "\Device\HarddiskVolume$HardDiskVolumeNumber" })
                        [System.IO.FileInfo]$UsablePath = "$($GetLetter.DriveLetter)$RemainingPath"
                        $Log['File Name'] = $Log['File Name'] -replace $Pattern, $UsablePath
                    }
                    # Use the alternative method to fix the drive letter mappings
                    else {
                        $Log['File Name'] = $Log['File Name'] -replace "\\Device\\HarddiskVolume$($Matches['HardDiskVolumeNumber'])", "$($DriveLetterMappings[$Matches['HardDiskVolumeNumber']]):"
                    }
                }
                # sometimes the file name begins with System32 so we prepend the Windows directory to create a full resolvable path
                # https://learn.microsoft.com/en-us/dotnet/api/system.string.startswith
                elseif ($Log['File Name'].StartsWith('System32', $true, [System.Globalization.CultureInfo]::InvariantCulture)) {
                    $Log['File Name'] = Join-Path -Path $Env:WinDir -ChildPath ($Log['File Name'])
                }

                # Replace these numbers in the logs with user-friendly strings that represent the signature level at which the code was verified
                $Log['Requested Signing Level'] = $ReqValSigningLevels[[System.UInt16]$Log['Requested Signing Level']]
                $Log['Validated Signing Level'] = $ReqValSigningLevels[[System.UInt16]$Log['Validated Signing Level']]

                # Replace the SI Signing Scenario numbers with a user-friendly string
                $Log['SI Signing Scenario'] = $Log['SI Signing Scenario'] -eq '0' ? 'Kernel-Mode' : 'User-Mode'

                # if the log source is local logs
                if ($LogSource -eq 'LocalLogs') {

                    # Translate the SID to a UserName if it's not null
                    if ($null -ne $Log.UserId) {
                        Try {
                            [System.Security.Principal.SecurityIdentifier]$ObjSID = New-Object -TypeName System.Security.Principal.SecurityIdentifier($Log.UserId)
                            $Log.UserId = [System.String]($ObjSID.Translate([System.Security.Principal.NTAccount])).Value
                        }
                        Catch {
                            Write-Verbose -Message "Receive-CodeIntegrityLogs: Could not translate the SID $($Log.UserId) to a username for the Activity ID $($Log['ActivityId']) for the file $($Log['File Name'])"
                        }
                    }
                    else {
                        Write-Verbose -Message "Receive-CodeIntegrityLogs: The UserId property is null for the Activity ID $($Log['ActivityId']) for the file $($Log['File Name'])"
                    }
                }

                # If there are correlated events, then process them
                if ($null -ne $EventPackage.CorrelatedEventsData) {

                    # A hashtable for storing the correlated logs
                    [System.Collections.Hashtable]$CorrelatedLogs = @{}

                    # Store the unique publisher name in HashSet
                    $Publishers = [System.Collections.Generic.HashSet[System.String]]@()

                    # Looping over each correlated event data
                    # There are more than 1 if the file has multiple signers/publishers
                    foreach ($CorrelatedEvent in $EventPackage.CorrelatedEventsData) {

                        # Convert the main event data to XML object
                        $XmlCorrelated = [System.Xml.XmlDocument]$CorrelatedEvent.ToXml()

                        if ($null -eq $XmlCorrelated.event.EventData.data) {
                            Write-Verbose -Message "Receive-CodeIntegrityLogs: Skipping Publisher check for: '$($Log['File Name'])' due to missing correlated event data"
                            continue
                        }

                        # Place each event data in a hashtable
                        [System.Collections.Hashtable]$CorrelatedLog = @{}
                        foreach ($Item in $XmlCorrelated.event.EventData.data) {
                            $CorrelatedLog[$Item.Name] = $Item.'#text'
                        }

                        # Skip signers that don't have PublisherTBSHash (aka LeafCertificate TBS Hash)
                        # They have "Unknown" as their IssuerName and PublisherName too
                        if ($null -eq $CorrelatedLog.PublisherTBSHash) {
                            Continue
                        }

                        # Replace the properties with their user-friendly strings
                        $CorrelatedLog.SignatureType = $SignatureTypeTable[[System.UInt16]$CorrelatedLog.SignatureType]
                        $CorrelatedLog.ValidatedSigningLevel = $ReqValSigningLevels[[System.UInt16]$CorrelatedLog.ValidatedSigningLevel]
                        $CorrelatedLog.VerificationError = $VerificationErrorTable[[System.UInt16]$CorrelatedLog.VerificationError]

                        # Create a unique key for each Publisher
                        [System.String]$PublisherKey = $CorrelatedLog.PublisherTBSHash + '|' +
                        $CorrelatedLog.PublisherName + '|' +
                        $CorrelatedLog.IssuerTBSHash + '|' +
                        $CorrelatedLog.IssuerName

                        # Add the Correlated Log to the array of Correlated Logs if it doesn't already exist there
                        if (-NOT $CorrelatedLogs.ContainsKey($PublisherKey)) {
                            $CorrelatedLogs[$PublisherKey] = $CorrelatedLog
                        }

                        # Add the unique publisher name to the array of Publishers if it doesn't already exist there
                        if (-NOT $Publishers.Contains($CorrelatedLog.PublisherName)) {
                            [System.Void]$Publishers.Add($CorrelatedLog.PublisherName)
                        }
                    }

                    Write-Debug -Message "Receive-CodeIntegrityLogs: The number of unique publishers in the correlated events is $($Publishers.Count)"
                    $Log['Publishers'] = $Publishers

                    # Add a new property to detect whether this log is signed or not
                    # Primarily used by the Build-SignerAndHashObjects Function and for Evtx log sources
                    $Log['SignatureStatus'] = $Publishers.Count -ge 1 ? 'Signed' : 'Unsigned'

                    Write-Debug -Message "Receive-CodeIntegrityLogs: The number of correlated events is $($CorrelatedLogs.Count)"
                    $Log['SignerInfo'] = $CorrelatedLogs
                }

                # Add the Type property to the log object
                $Log['Type'] = $EventPackage.Type

                #Region Post-processing for the logs

                # Creating a unique string key for the current log
                # The key ending up being too long doesn't matter and doesn't affect the performance
                # Since all keys are hashed in a hashtable
                [System.String]$UniqueLogKey = $Log['File Name'] + '|' +
                $Log.ProductName + '|' +
                $Log.FileVersion + '|' +
                $Log.OriginalFileName + '|' +
                $Log.FileDescription + '|' +
                $Log.InternalName + '|' +
                $Log.PackageFamilyName + '|' +
                $Log.Publishers + '|' +
                $Log['SHA256 Hash'] + '|' +
                $Log['SHA256 Flat Hash']

                try {

                    # Using the SyncRoot property to lock the $Output hashtable during the check-and-add sequence, making it atomic and thread-safe
                    # This ensures that only one thread at a time can execute the code within the try block, thus preventing race conditions
                    [System.Threading.Monitor]::Enter($using:Output.SyncRoot)

                    if ($Log.Type -eq 'Audit') {

                        # Add the log to the output hashtable if it has Audit type and doesn't already exist there
                        if (-NOT $Output.All.Audit.ContainsKey($UniqueLogKey)) {
                            $Output.All.Audit[$UniqueLogKey] = $Log
                        }

                        # If the file the log is referring to is currently on the disk
                        if ([System.IO.File]::Exists($Log['File Name'])) {

                            if (-NOT $Output.Existing.Audit.ContainsKey($UniqueLogKey)) {
                                $Output.Existing.Audit[$UniqueLogKey] = $Log
                            }

                            <#
                                if (-NOT $Output.Separated.Audit.AvailableFilesPaths.Contains($Log['File Name'])) {
                                    [System.Void]$Output.Separated.Audit.AvailableFilesPaths.Add($Log['File Name'])
                                }
                                #>
                        }
                        <#
                            # If the file is not currently on the disk, extract its hashes from the log
                            else {
                                $TempDeletedOutputAudit = $Log | Select-Object -Property FileVersion, 'File Name', PolicyGUID, 'SHA256 Hash', 'SHA256 Flat Hash', 'SHA1 Hash', 'SHA1 Flat Hash'

                                if (-NOT $Output.Deleted.Audit.ContainsKey($UniqueLogKey)) {
                                    $Output.Deleted.Audit[$UniqueLogKey] = $TempDeletedOutputAudit
                                }

                                if (-NOT $Output.Separated.Audit.DeletedFileHashes.Contains($UniqueLogKey)) {
                                    $Output.Separated.Audit.DeletedFileHashes[$UniqueLogKey] = $TempDeletedOutputAudit
                                }
                            }
                            #>
                    }

                    elseif ($Log.Type -eq 'Blocked') {

                        # Add the log to the output hashtable if it has Blocked type and doesn't already exist there
                        if (-NOT $Output.All.Blocked.ContainsKey($UniqueLogKey)) {
                            $Output.All.Blocked[$UniqueLogKey] = $Log
                        }

                        # If the file the log is referring to is currently on the disk
                        if ([System.IO.File]::Exists($Log['File Name'])) {

                            if (-NOT $Output.Existing.Blocked.ContainsKey($UniqueLogKey)) {
                                $Output.Existing.Blocked[$UniqueLogKey] = $Log
                            }

                            <#
                                if (-NOT $Output.Separated.Blocked.AvailableFilesPaths.Contains($Log['File Name'])) {
                                    [System.Void]$Output.Separated.Blocked.AvailableFilesPaths.Add($Log['File Name'])
                                }
                                #>
                        }
                        <#
                            # If the file is not currently on the disk, extract its hashes from the log
                            else {
                                $TempDeletedOutputBlocked = $Log | Select-Object -Property FileVersion, 'File Name', PolicyGUID, 'SHA256 Hash', 'SHA256 Flat Hash', 'SHA1 Hash', 'SHA1 Flat Hash'

                                if (-NOT $Output.Deleted.Blocked.ContainsKey($UniqueLogKey)) {
                                    $Output.Deleted.Blocked[$UniqueLogKey] = $TempDeletedOutputBlocked
                                }

                                if (-NOT $Output.Separated.Blocked.DeletedFileHashes.Contains($UniqueLogKey)) {
                                    $Output.Separated.Blocked.DeletedFileHashes[$UniqueLogKey] = $TempDeletedOutputBlocked
                                }
                            }
                            #>
                    }
                    #Endregion Post-processing for the logs

                }
                catch {
                    Throw $_
                }
                # Always ensures the lock is released
                finally {
                    [System.Threading.Monitor]::Exit($using:Output.SyncRoot)
                }
            }
        } -ThrottleLimit 5
    }

    End {
        # Assigning null to the variables that are empty since users of this function need null values for empty variables
        if (-NOT (Test-NotEmpty -Data $Output.All.Audit)) { $Output.All.Audit = $null }
        if (-NOT (Test-NotEmpty -Data $Output.All.Blocked)) { $Output.All.Blocked = $null }
        if (-NOT (Test-NotEmpty -Data $Output.Existing.Audit)) { $Output.Existing.Audit = $null }
        if (-NOT (Test-NotEmpty -Data $Output.Existing.Blocked)) { $Output.Existing.Blocked = $null }
        #    if (-NOT (Test-NotEmpty -Data $Output.Deleted.Audit)) { $Output.Deleted.Audit = $null }
        #    if (-NOT (Test-NotEmpty -Data $Output.Deleted.Blocked)) { $Output.Deleted.Blocked = $null }
        #    if (-NOT (Test-NotEmpty -Data $Output.Separated.Audit.AvailableFilesPaths)) { $Output.Separated.Audit.AvailableFilesPaths = $null }
        #    if (-NOT (Test-NotEmpty -Data $Output.Separated.Audit.DeletedFileHashes)) { $Output.Separated.Audit.DeletedFileHashes = $null }
        #    if (-NOT (Test-NotEmpty -Data $Output.Separated.Blocked.AvailableFilesPaths)) { $Output.Separated.Blocked.AvailableFilesPaths = $null }
        #    if (-NOT (Test-NotEmpty -Data $Output.Separated.Blocked.DeletedFileHashes)) { $Output.Separated.Blocked.DeletedFileHashes = $null }

        Switch ($PostProcessing) {
            <#
            'Separate' {
                if ($Type -eq 'Audit') {
                    Write-Verbose -Message "Receive-CodeIntegrityLogs: Returning $($Output.Separated.Audit.AvailableFilesPaths.Count) Audit Code Integrity logs for files on the disk and $($Output.Separated.Audit.DeletedFileHashes.Count) for the files not on the disk."
                    Return $Output.Separated.Audit
                }
                else {
                    Write-Verbose -Message "Receive-CodeIntegrityLogs: Returning $($Output.Separated.Blocked.AvailableFilesPaths.Count) Blocked Code Integrity logs for files on the disk and $($Output.Separated.Blocked.DeletedFileHashes.Count) for the files not on the disk."
                    Return $Output.Separated.Blocked
                }
            }
            #>
            'OnlyExisting' {
                if ($Type -eq 'Audit') {
                    Write-Verbose -Message "Receive-CodeIntegrityLogs: Returning $($Output.Existing.Audit.Values.Count) Audit Code Integrity logs for files on the disk."
                    Return $Output.Existing.Audit.Values
                }
                else {
                    Write-Verbose -Message "Receive-CodeIntegrityLogs: Returning $($Output.Existing.Blocked.Values.Count) Blocked Code Integrity logs for files on the disk."
                    Return $Output.Existing.Blocked.Values
                }
            }
            <#
            'OnlyDeleted' {
                if ($Type -eq 'Audit') {
                    Write-Verbose -Message "Receive-CodeIntegrityLogs: Returning $($Output.Deleted.Audit.Values.Count) Audit Code Integrity logs for files not on the disk."
                    Return $Output.Deleted.Audit.Values
                }
                else {
                    Write-Verbose -Message "Receive-CodeIntegrityLogs: Returning $($Output.Deleted.Blocked.Values.Count) Blocked Code Integrity logs for files not on the disk."
                    Return $Output.Deleted.Blocked.Values
                }
            }
            #>
            Default {
                if ($Type -eq 'Audit') {
                    Write-Verbose -Message "Receive-CodeIntegrityLogs: Returning $($Output.All.Audit.Values.Count) Audit Code Integrity logs."
                    Return $Output.All.Audit.Values
                }
                else {
                    Write-Verbose -Message "Receive-CodeIntegrityLogs: Returning $($Output.All.Blocked.Values.Count) Blocked Code Integrity logs."
                    Return $Output.All.Blocked.Values
                }
            }
        }
    }
}
Export-ModuleMember -Function 'Receive-CodeIntegrityLogs'
