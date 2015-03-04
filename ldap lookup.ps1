#Function Definitions
function preFlight {
    if (!(test-path variable:\Users) -or ($Users.Count -eq 0)) {
        cls
        echo "No user IDs were provided.","Please provide the user IDs as follows:","",'$Users="I123456","C5012345"',""
        break
       }
}
function ResetVars {
    Set-Variable -Name strMsg -Value "No Data Collected"
    #Setting the variable in the function to make this reusable
    #elevating the scope so that the definitions apply globally 
    Set-Variable -Name strFName -Value $strMsg -Scope 1
    Set-Variable -Name strLName -Value $strMsg -Scope 1
    Set-Variable -Name strCC -Value $strMsg -Scope 1
    Set-Variable -Name strRegion -Value $strMsg -Scope 1
    Set-Variable -Name objLastError -Scope 1
    Set-Variable -Name intErrors -Value 0 -Scope 1
}
function ldapLookup {
    param ($strName)
    if ($debug) {echo "Lookup initiated for $strName."}
    $strFilter = "(&(objectCategory=User)(samAccountName=$strName))"
    $objSearcher = New-Object System.DirectoryServices.DirectorySearcher
    $objSearcher.Filter = $strFilter
    try {
        $objPath = $objSearcher.FindOne()
        $objUser = $objPath.GetDirectoryEntry()

        $strFName = $objUser.givenName
        $strLName = $objUser.sn
        $strCC = $objUser.extensionAttribute2
        $strRegion = $objUser.extensionAttribute7
        $strResult += "$strName,$strFName,$strLName,$strCC,$strFName $strLName,$strRegion"
    }

    catch {
        $intErrors++
        if ($debug) {echo "This was error $intErrors"}
        $objLastError = $_.Exception
        if (!$omit) {ResetVars;$strResult += "$strName,$strFName,$strLName,$strCC,$strFName $strLName,$strRegion"}
        #break
    }    
    if ($debug) {echo "$strFName $strLName was found!"}
    if ($debug) {echo '$strResult updated:'}
    if ($debug) {echo $strResult}if ($debug) {echo "Exiting lookup application"}
    if ($debug) {echo ""}
}
#End function definitions

#Set Headers
$strResult = "sep=,","UserID,First Name,Last Name,Cost Center,Full Name,Region"

#Set Defaults
ResetVars

#Set operations switches
$debug = $false
#$omit = $true #omits entries where no data was found in LDAP
$omit = $false #includes entries where no data was found in LDAP

#if ndef $Users, error and exit
preFlight


foreach ($user in $Users) {
    #If a blank was passed, go to the next entry
    if ($debug) {echo "Selected user is $user."}
    if ($user -ne "") {
        if ($debug) {echo "User ID is not blank, calling lookup."}
        . ldapLookup $user
        }
    else {   
        $intErrors++ 
        if ($debug) {echo "User ID is blank. $intErrors errors encountered."}
        continue
        }
}

if ($intErrors -eq $Users.Count) {
    $users = $Users.Count
    echo "Of the $users User IDs provided, none were found in the LDAP.  Please check your network connection and try again."
    break
}
$strResult | Out-File ${env:Temp}"\a.csv"
start-process ${env:ProgramFiles(x86)}"\microsoft office\Office15\excel.exe" ${env:Temp}"\a.csv" -wait
del ${env:Temp}"\a.csv"