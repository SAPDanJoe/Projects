<#
.SYNOPSIS
    This script will backup and then clear the current user's Outlook cache files.

.DESCRIPTION
    This script will backup and then clear the current user's Outlook cache files.  Minimal user input is required, and minimal user output is provided to achieve the highest level of simplicity.
    
    The script is layed out as follows:
        1) This help section
        2) Background functions that provide the scripts functionality
        3) Global variable declaration
        4) Function calls to navigate the workflow
    
    The functional order will follow this basic workflow:
        1) Check command parameters for optional functions
        2) Identify running processes that may lock any of the cache files
        3) Stop those processes
        4) Create a working directory
            (if one exists, this may be an indication that there is a problem)
        5) Save list of files in Outlook cache
        6) Move files from Outlook cache to working directory
            (if cache folder is not empty, this may be an indication of a locked file)
        7) Create a backup folder in the Outlook cache directory
        8) Move the items from the working directory to the backup directory
        9) Compare list of items in the cache directory from 5 to the current contents of the backup directory. (warn if not matched)
       10) Process any options specified at the command line
       11) Restart the process that were closed in the beginning
       
.PARAMETER Mode
    Used to define the operation mode of the script.  Available options are:
        *SaveCache
            Preserves the AutoCorrect and Offline Addressbook data by copying the RoamCache folder back into the Outlook cache.
        *
    Default Mode is SaveRoamCache

.PARAMETER Extension


.INPUTS


.OUTPUTS


.EXAMPLE


.EXAMPLE


.LINK


.LINK

#>
param(
    [ValidateSet('SaveCache','that')]
    [string]
    $Mode = 'SaveCache'
,
    [parameter(Mandatory=$false)]
    [alias("PD")]
    [switch]
    $persistData=$false
)

#Enable message boxes
Add-Type –AssemblyName System.Windows.Forms

#Global variables
$runTime = get-date <#I have decided to format this seperately, just incase I need to call up the time that the script was launched for a log or something.#>
$DateTimeDIR = Get-Date $runTime -Format 'dd-MM-yyy hhmm'
$workingDir = "$env:temp\ostBackup" + $DateTimeDIR
$outlookDir = "$env:LOCALAPPDATA\Microsoft\Outlook"
$backupDir = $outlookDir + "\ostBackup" + $DateTimeDIR
$suspectprocesses = "OUTLOOK", "communicator", "UcMapi", "eaclient"

#Supporting functions

function Use-RunAs 
{    
    # Check if script is running as Adminstrator and if not use RunAs 
    # Use Check Switch to check if admin 
    # Function credit to MS Script Center (http://gallery.technet.microsoft.com/scriptcenter/63fd1c0d-da57-4fb4-9645-ea52fc4f1dfb)
     
    param([Switch]$Check) 
     
    $IsAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()` 
        ).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator") 
         
    if ($Check) { return $IsAdmin }     
 
    if ($MyInvocation.ScriptName -ne "") 
    {  
        if (-not $IsAdmin)  
        {  
            try 
            {  
                $arg = "-file `"$($MyInvocation.ScriptName)`"" 
                Start-Process "$psHome\powershell.exe" -Verb Runas -ArgumentList $arg -ErrorAction 'stop'  
            } 
            catch 
            { 
                Write-Warning "Error - Failed to restart script with runas"  
                break               
            } 
            exit # Quit this session of powershell 
        }  
    }  
    else  
    {  
        Write-Warning "Error - Script must be saved as a .ps1 file first"  
        break  
    }  
} 

function makeWorkingDir()
{
    #create a temp folder for the outlook profile backup (or warn if the folder already exists)
    if(!(Test-Path $workingDir)){
        New-Item $workingDir -ItemType Directory
    }
    else {
        $Response=([System.Windows.Forms.MessageBox]::Show("It looks like this script may have been started, but did not complete.`nThe working directory:`n$workingDir `nalready exists.  How would you like to proceed?`n`nAbort: Stop running the script`nRetry: I have fixed the problem and am ready to try again`nIgnore: This error is overlooked and the rest of the script runs","Ooops!","AbortRetryIgnore","Warning"))
        if($Response -eq 'Abort'){exit}
        elseif ($Response -eq 'Retry'){makeWorkingDir}
        elseif ($Response -eq 'Ignore'){break}
        else{
            ([System.Windows.Forms.MessageBox]::Show('Script error: the error response: $Response was unhandled.'))
            exit
            }
     }
}

function Backup()
{
    <#Get a list of items in the outlook folder, before making any changes#>
    $before = Get-ChildItem $outlookDir
    "`nCurrent contents of outlook directory:`n"
    $before

    Move-Item $outlookDir\* $workingDir -force
    
    <#Get a list of items in the outlook folder after emptying it (to make sure it is empty)#>
    $empty = Get-ChildItem $outlookDir
    "`nContents of outlook directory after empty operation:`n"
    $empty

    if ($empty.Count -eq 0){

        <#If the folder was emptied successfully, create the backup folder in outlook and backup to it#>

        New-Item $backupDir -ItemType Directory
        Move-Item $workingDir\* $backupDir -Force
        Remove-Item $workingDir
    }
    else {
        
        <#Handle exception where the folder was not emptied successfully#>
        $Response=([System.Windows.Forms.MessageBox]::Show(
            "$outlookDir should be empty but it is not.`n" +
            "The folder contains " + $empty.Count + " files.`n `n" +
            "How would you like to proceed?","Ooops!","AbortRetryIgnore","Warning"))
        if($Response -eq 'Abort'){exit}
        elseif ($Response -eq 'Retry'){ostBackup}
        elseif ($Response -eq 'Ignore'){
            New-Item $backupDir -ItemType Directory
            Move-Item $workingDir\* $backupDir -Force
            Remove-Item $workingDir
        }
        else{
            ([System.Windows.Forms.MessageBox]::Show('Script error: the error response: $Response was unhandled.'))
            exit
           }
    }
     
    <#Now make sure that the number of files that were in the Outlook folder matches the number now in the backup folder#>
    $backedUp = Get-ChildItem $backupDir
    "`nContents of outlook backup directory after move operation:`n"
    $backedUp
    if(!($before.Count -eq $backedUp.Count)){
        <#Handle the exception where they do not match#>
        $Response=([System.Windows.Forms.MessageBox]::Show(
            "There was a problem backing up the outlook files.`n" +
            "$outlookDir contained " + $before.Count + " files when I started. `n" +
            "$outlookDir\ostBackup $scriptTime contains " + $backedUp.Count +" files.`n `n" +
            "Original Files:`n" +
            "$before `n `n" +
            "Backed-up Files: `n `n" +
            "$backedUp `n `n" +
            "How would you like to proceed?",
            "Ooops!",
            "AbortRetryIgnore",
            "Warning"))
        if($Response -eq 'Abort'){exit}
        elseif ($Response -eq 'Retry'){ostBackup}
        elseif ($Response -eq 'Ignore'){Break}
        else{
            ([System.Windows.Forms.MessageBox]::Show('Script error: the error response: $Response was unhandled.'))
            exit
            }
     }
}

function restartProc ()
{
    param(
       [parameter(mandatory=$true)]
       [string]
       $restarts
    )
    foreach ($restart in $restarts){
        Start-Process -FilePath $restart
        }
    exit
}

function restorePersistantData ()
{
    Copy-Item $backupDir\RoamCache $outlookDir -Recurse
    Copy-Item "$backupDir\Offline Address Books" $outlookDir -Recurse
}

function Get-User 
{
    param(
        $name = $env:USERNAME
    )
    $objSearcher = New-Object System.DirectoryServices.DirectorySearcher
    $objSearcher.Filter = "(&(objectCategory=User)(samAccountName=$name))"
    $objPath = $objSearcher.FindOne()
    $objUser = $objPath.GetDirectoryEntry()
    return $objUser
}

function Get-DirFiles
{
    param(
        [parameter(Mandatory=$true)]
        [string]
        $Dir
    )

    $dirfiles = Get-ChildItem $Dir -Force -Recurse
    $dirfiles = foreach($file in $dirfiles)
    {
        if($file.directory.fullname -Match ':\\')
        {
        $file.directory.fullname + "\" + $file.name
        }
    }
    return $dirfiles
}

function Unlock-Item
{
    param(
        
        $files
    )
    
    $liveProcesses = Get-Process
    $suspectprocesses = "OUTLOOK", "communicator", "UcMapi", "eaclient"
    foreach($file in $files)
    {
        foreach($process in $liveProcesses)
        {
            
            if($Process.Modules.FileName -Contains $file)
            {
                write-host $file + " is in use by: " +$process.Name -ForegroundColor Red
            }
            if($suspectprocesses -contains $process.name)
            {
                $process.Name + " is suspect, and alive with " + $Process.Modules.FileName.Count + " files in use."
                $process.Modules.Filename | foreach{write-host $_. -ForegroundColor DarkGray}
                $filesofinterest| foreach{write-host $_. -ForegroundColor Gray}
            }
        }
    }



<#  Process
    {
 #>     Get-Process | foreach
        {
	        $processVar = $_;$_.Modules | foreach
            {
		        if($_.FileName -eq $File)
                {
			        
                    <#Original script section, removed
                    $processVar.Name + " PID:" + $processVar.id
                    #>    
                }
		    }
<#	    }
#>  }
}

function Set-Timer
{
    $taskDate = Get-Date $scriptTime.AddDays(14) -Format mm/dd/yyyy
    schtasks /Create /SC ONCE /ST /SD $taskDate /TN "SAP-IT OST Recovery Cleanup" /TR "Powershell -command {Remove-Item $backupDir}"   
}

#Validate parameters, display help if invalid parameters are defined
<# This section was a work in progress, and is depreciated in lieu of the built-in parameter validation
user = Get-User
switch ($Mode)
{
    'SaveRoamCache'{}
    default {
    write-host Hello $user.givenName','`nOne of the parameters that you entered: $Mode `nwas not valid. Run Help for more information. -ForegroundColor DarkRed -BackgroundColor White
    }
}
#>


#stop processes that may be using outlook files
foreach ($process in $processes){
    $testproc = Get-Process -name $process -ErrorAction Continue
    if (!($testproc.Count -eq 0)){        
        $restarts += $testproc.Path
    }
    do {
        
        Stop-Process -name $process
    }
    while(Get-Process $process -ErrorAction SilentlyContinue)
}

$filesToBackup = Get-DirFiles -Dir $outlookDir 

makeWorkingDir
Backup
if ($Mode -eq 'SaveCache'){restoreRoamCache}
restartProc
if (!($persistData)){Set-Timer}