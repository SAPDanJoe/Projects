param(
[string] $strName = $env:username
)

$strFilter = "(&(objectCategory=User)(samAccountName=$strName))"

$objSearcher = New-Object System.DirectoryServices.DirectorySearcher
$objSearcher.Filter = $strFilter

$objPath = $objSearcher.FindOne()
$objUser = $objPath.GetDirectoryEntry()
if (-not  $objUser.uidNumber -eq "" ) {
"Congrats! $strName has Linux logon rights."
}
else {
"In order to enable $strName for Linux login please open a request using the following page:"
"https://ifp.wdf.sap.corp/itform/zitform.htm?formname=_LBS_LINUX1"
}
