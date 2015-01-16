$ADS_UF_SCRIPT                                   = 1         # 0x1
$ADS_UF_ACCOUNTDISABLE                           = 2         # 0x2 
$ADS_UF_HOMEDIR_REQUIRED                         = 8         # 0x8 
$ADS_UF_LOCKOUT                                  = 16        # 0x10
$ADS_UF_PASSWD_NOTREQD                           = 32        # 0x20
$ADS_UF_PASSWD_CANT_CHANGE                       = 64        # 0x40
$ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED          = 128       # 0x80
$ADS_UF_TEMP_DUPLICATE_ACCOUNT                   = 256       # 0x100
$ADS_UF_NORMAL_ACCOUNT                           = 512       # 0x200
$ADS_UF_INTERDOMAIN_TRUST_ACCOUNT                = 2048      # 0x800
$ADS_UF_WORKSTATION_TRUST_ACCOUNT                = 4096      # 0x1000
$ADS_UF_SERVER_TRUST_ACCOUNT                     = 8192      # 0x2000
$ADS_UF_DONT_EXPIRE_PASSWD                       = 65536     # 0x10000
$ADS_UF_MNS_LOGON_ACCOUNT                        = 131072    # 0x20000
$ADS_UF_SMARTCARD_REQUIRED                       = 262144    # 0x40000
$ADS_UF_TRUSTED_FOR_DELEGATION                   = 524288    # 0x80000
$ADS_UF_NOT_DELEGATED                            = 1048576   # 0x100000
$ADS_UF_USE_DES_KEY_ONLY                         = 2097152   # 0x200000
$ADS_UF_DONT_REQUIRE_PREAUTH                     = 4194304   # 0x400000
$ADS_UF_PASSWORD_EXPIRED                         = 8388608   # 0x800000
$ADS_UF_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION   = 16777216  # 0x1000000
function Set-PinnedApplication 
{ 
<#  
.SYNOPSIS  
This function are used to pin and unpin programs from the taskbar and Start-menu in Windows 7 and Windows Server 2008 R2 
.DESCRIPTION  
The function have to parameteres which are mandatory: 
Action: PinToTaskbar, PinToStartMenu, UnPinFromTaskbar, UnPinFromStartMenu 
FilePath: The path to the program to perform the action on 
.EXAMPLE 
Set-PinnedApplication -Action PinToTaskbar -FilePath "C:\WINDOWS\system32\notepad.exe" 
.EXAMPLE 
Set-PinnedApplication -Action UnPinFromTaskbar -FilePath "C:\WINDOWS\system32\notepad.exe" 
.EXAMPLE 
Set-PinnedApplication -Action PinToStartMenu -FilePath "C:\WINDOWS\system32\notepad.exe" 
.EXAMPLE 
Set-PinnedApplication -Action UnPinFromStartMenu -FilePath "C:\WINDOWS\system32\notepad.exe" 
#>  
       [CmdletBinding()] 
       param( 
      [Parameter(Mandatory=$true)][string]$Action,  
      [Parameter(Mandatory=$true)][string]$FilePath 
       ) 
       if(-not (test-path $FilePath)) {  
           throw "FilePath does not exist."   
    } 
    
       function InvokeVerb { 
           param([string]$FilePath,$verb) 
        $verb = $verb.Replace("&","") 
        $path= split-path $FilePath 
        $shell=new-object -com "Shell.Application"  
        $folder=$shell.Namespace($path)    
        $item = $folder.Parsename((split-path $FilePath -leaf)) 
        $itemVerb = $item.Verbs() | ? {$_.Name.Replace("&","") -eq $verb} 
        if($itemVerb -eq $null){ 
            throw "Verb $verb not found."             
        } else { 
            $itemVerb.DoIt() 
        } 
            
       } 
    function GetVerb { 
        param([int]$verbId) 
        try { 
            $t = [type]"CosmosKey.Util.MuiHelper" 
        } catch { 
            $def = [Text.StringBuilder]"" 
            [void]$def.AppendLine('[DllImport("user32.dll")]') 
            [void]$def.AppendLine('public static extern int LoadString(IntPtr h,uint id, System.Text.StringBuilder sb,int maxBuffer);') 
            [void]$def.AppendLine('[DllImport("kernel32.dll")]') 
            [void]$def.AppendLine('public static extern IntPtr LoadLibrary(string s);') 
            add-type -MemberDefinition $def.ToString() -name MuiHelper -namespace CosmosKey.Util             
        } 
        if($global:CosmosKey_Utils_MuiHelper_Shell32 -eq $null){         
            $global:CosmosKey_Utils_MuiHelper_Shell32 = [CosmosKey.Util.MuiHelper]::LoadLibrary("shell32.dll") 
        } 
        $maxVerbLength=255 
        $verbBuilder = new-object Text.StringBuilder "",$maxVerbLength 
        [void][CosmosKey.Util.MuiHelper]::LoadString($CosmosKey_Utils_MuiHelper_Shell32,$verbId,$verbBuilder,$maxVerbLength) 
        return $verbBuilder.ToString() 
    } 
 
    $verbs = @{  
        "PintoStartMenu"=5381 
        "UnpinfromStartMenu"=5382 
        "PintoTaskbar"=5386 
        "UnpinfromTaskbar"=5387 
    } 
        
    if($verbs.$Action -eq $null){ 
           Throw "Action $action not supported`nSupported actions are:`n`tPintoStartMenu`n`tUnpinfromStartMenu`n`tPintoTaskbar`n`tUnpinfromTaskbar" 
    } 
    InvokeVerb -FilePath $FilePath -Verb $(GetVerb -VerbId $verbs.$action) 
} 
 
Export-ModuleMember Set-PinnedApplication
if ($env:USERNAME -eq "RACCOUNT") {


#get operator
$op = Get-Credential -Message "Please enter your SAP credentials (including GLOBAL\) to convert this to an Event Machine.  Example: GLOBAL\I654321"

#User definition
$userName = "EventUser"
$userPass = "1Event2Day!"
$userGroup = "Administrators"

#user creation
net user $userName $userPass /add

#user flags
$u = [adsi]"WinNT://$env:computername/$userName,user"
$u.invokeSet("userFlags", ($u.userFlags[0] -BOR $ADS_UF_PASSWD_CANT_CHANGE))
$u.commitChanges()
$u.invokeSet("userFlags", ($u.userFlags[0] -BOR $ADS_UF_DONT_EXPIRE_PASSWD))
$u.commitChanges() 

#user group
net localgroup $userGroup $userName /add

#user auto login
New-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon' -Name AutoAdminLogon -Value 1 
New-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon' -Name DefaultUserName -Value $userName 
New-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon' -Name DefaultPassword -Value $userPass

#make a copy of script in c:\scripts
New-Item -Path C:\ -Name Scripts -ItemType Directory
Copy-Item $Script:MyInvocation.MyCommand.Path -Destination C:\Scripts

#move computer from domain to workstation
Remove-Computer -UnjoinDomaincredential $op -Passthru -Restart -Force -WorkgroupName Events -Whatif

} elseif ($env:USERNAME -eq "EventUser"){
#Clean SCCM
Start-process \\ecs\Remediation\SCCM\ccmclean.exe -Wait

#Uninstall Apps
$appNames = "Symantec Encryption Desktop","1E Agent","1E Web"
foreach ($appName in $appNames) {
    $uninstall32 = gci "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall" | foreach { gp $_.PSPath } | ? { $_ -match $appName } | select UninstallString
    $uninstall64 = gci "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall" | foreach { gp $_.PSPath } | ? { $_ -match $appName } | select UninstallString

    if ($uninstall64) {
    $uninstall64 = $uninstall64.UninstallString -Replace "msiexec.exe","" -Replace "/I","" -Replace "/X",""
    $uninstall64 = $uninstall64.Trim()
    Write "Uninstalling..."
    start-process "msiexec.exe" -arg "/X $uninstall64 /qb" -Wait}
    if ($uninstall32) {
    $uninstall32 = $uninstall32.UninstallString -Replace "msiexec.exe","" -Replace "/I","" -Replace "/X",""
    $uninstall32 = $uninstall32.Trim()
    Write "Uninstalling..."
    start-process "msiexec.exe" -arg "/X $uninstall32 /qb" -Wait}
}

#Set Screensaver timeout to 599940
Set-ItemProperty -Path 'HKCU:\Control Panel\Desktop' -name 'ScreenSaveTimeOut' -value '599940'

#Pin Office apps to taskbar
$apps = "WINWORD.EXE","POWERPNT.EXE","EXCEL.EXE"
foreach ($app in $apps){
    $path = "C:\Program Files (x86)\Microsoft Office\Office15\"
    $app = $path + $app
    Set-PinnedApplication -Action PinToTaskbar -FilePath $app
}

#Unpin office communication apps
$apps = "OUTLOOK.EXE","LYNC.EXE"
foreach ($app in $apps){
    $path = "C:\Program Files (x86)\Microsoft Office\Office15\"
    $app = $path + $app
    Set-PinnedApplication -Action UnPinFromTaskbar -FilePath $app
}


} else {
#

#delete script

}

