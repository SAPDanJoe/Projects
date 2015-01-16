 # ============================================================================================== 
 # 
 #
 # NAME: Add-PinnedApplication.ps1 
 # 
 # AUTHOR: Ragnar Harper , Crayon 
 #         Kristian Svantorp, Microsoft
 # DATE  : 20.04.2009 
 # 
 # COMMENT: 
 # Locale supported: Norwegian, English, Swedish, Danish, Finnish
 # 
 #    http://blog.crayon.no/blogs/ragnar 
 #    http://blogs.technet.com/kristian
 # 
 # Changelog:
 # KS: Added multilanguage support, automated. Added Nordic verbs 
 # KS: Changed path splitting
 # KS: Added some error-handling, test-path
 # KS: Added -s parameter for pinning to start menu instead
 #
 #
 # Todo:
 # KS: Get the verbs in the languages listed above
 #  : Add error handling
 #  : Use env:path instead if not given full path
 #  : Add switch to pin to start-menu instead. default taskbar, param for startmenu
 # ============================================================================================== 
 <# 
 $PinVerbTask = @{}
 $PinverbTask['En-US'] ="Pin to Taskbar" 
 $PinverbTask['Nb-No'] ="Fest til oppgavelinjen"
 $PinverbTask['Sv-se'] = "Fäst i Aktivitetsfältet"
 $PinverbTask['Da-dk'] = "Fastgør til proceslinje"
 $PinVerbTask['Fi-fi'] = "Kiinnitä tehtäväpalkkiin"
   
   
  $PinVerbStart = @{}
  $PinverbStart['En-US'] ="Pin to Start Menu" 
  $PinverbStart['Nb-No'] ="Fest til Start-menyen"
  $PinverbStart['Sv-se'] = "Fäst på Start-menyn"
  $PinverbStart['Da-dk'] = "Fastgør til menuen Start"
  $PinverbStart['Fi-fi'] = "Kiinnitä Käynnistä-valikkoon"
    
   ##add the rest
    
   ##get culture
   $culture = (get-culture).Name
    
   if($args.count -lt 1) 
   { 
       Write-Host "`nWrong number of arguments." 
       Write-Host "usage: Add-ToTaskbar.ps1 [program to pin] [-s]" 
       Write-Host "example: Add-ToTaskBar.ps1 c:\windows\system32\calc.exe" 
       Write-Host "Parameter -s pins program to start menu instead"
       Write-Host "You must include folderpath to program `n" 
   } 
   else 
   { 
       $file=$args[0]     
       #Check to see if path actually exists
       if(-not (test-path $file)) { write-host "`nPath does not exist.`n $file `nExiting... `n";  exit  }
          
       #this should only be done if a full path is given    
       $path= split-path $file
       #Create shell envir
       $shell=new-object -com "Shell.Application" 
       $folder=$shell.Namespace($path)   
       $item = $folder.Parsename((split-path $file -leaf))
      
       $verbs=$item.Verbs()    
       if($args[1] -eq "-s") {foreach($v in $verbs){if($v.Name.Replace("&","") -match $PinVerbStart[$culture]){$v.DoIt()}}}
       else { foreach($v in $verbs){if($v.Name.Replace("&","") -match $PinVerbTask[$culture]){$v.DoIt()}} }
   }
   #>

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
Set-PinnedApplication -Action UnpinFromTaskbar -FilePath "C:\WINDOWS\system32\notepad.exe" 
.EXAMPLE 
Set-PinnedApplication -Action PinToStartMenu -FilePath "C:\WINDOWS\system32\notepad.exe" 
.EXAMPLE 
Set-PinnedApplication -Action UnPinFromStartMenu -FilePath "C:\WINDOWS\system32\notepad.exe" 
#>  
#       [CmdletBinding()] 
       param( 
      [Parameter(Mandatory=$true)][string]$Action,  
      [Parameter(Mandatory=$true)][string]$FilePath 
       ) 
    #var Definition(s)  
    $verbs = @{  
        "PintoStartMenu"=5381 
        "UnpinfromStartMenu"=5382 
        "PintoTaskbar"=5386 
        "UnpinfromTaskbar"=5387 
    }

    #function Definition(s)
    function InvokeVerb { 
        param([string]$FilePath,$verb) 
        Write-Host "Verb Invocation Initiated with parameters `$FilePath= $FilePath and `$verb= $verb"
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
        Write-Host "GetVerb Initiated with parameter `$verbId= $verbId"
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
    
    ##ENTRY function Set-PinnedApplication

    #Check Parameters --> $FilePath
    Write-Host "Checking Parameter: `$FilePath = $FilePath"
    if(-not (test-path $FilePath)) {  
           throw "FilePath does not exist."   
    } 
    Write-Host "File path validated: $FilePath"
 
    #Check Parameters --> $Action
    Write-Host "Checking Parameter: `$Action= $Action"    
    if($verbs.$Action -eq $null){ 
           Throw "Action $Action not supported`nSupported actions are:`n`tPintoStartMenu`n`tUnpinfromStartMenu`n`tPintoTaskbar`n`tUnpinfromTaskbar" 
    } 
    Write-Host "Verb validated for `$Action: $verbs.$action"

    #Main statement calls the subfunctions InvokeVerb and GetVerb
    InvokeVerb -FilePath $FilePath -Verb $(GetVerb -VerbId $verbs.$action) 
} 