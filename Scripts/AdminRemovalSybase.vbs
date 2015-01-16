'==========================================================================
'
' VBScript Source File -- Created with SAPIEN Technologies PrimalScript 4.1
'
' NAME: 
'
' AUTHOR: I804641 , SAP
' DATE  : 4/30/2010
'
' COMMENT: 
'
'==========================================================================
Dim WshNetwork, TheUserName, wshshell, Account, LSCOSD
Set WshNetwork = CreateObject( "WScript.Network" )
Set wshshell = CreateObject("Wscript.Shell")
TheUserName = UCase(WshNetwork.UserName) 
TheComputerName = UCase(WshNetwork.ComputerName)
Set FSO = createobject("scripting.filesystemobject")


UserCheck



'######################### ^^CHANGE SCRIPT ABOVE^^ ##########################
'######################### Subs 'n Functions Below #########################
'############################## DO NOT CHANGE ##############################
Sub UserCheck

'On Error Resume Next
	Dim objLocalGroup, objUser, ODict
	' Bind to local Administrators group.
	Set objLocalGroup = GetObject("WinNT://" & TheComputerName & "/Administrators")
  For Each sAdmin In objLocalGroup.Members
    If UCase(sAdmin.Name) = UCase("sybaseimg") Then
       'MsgBox "Match: " & sAdmin.Name & " to sybaseimg"
       Run
    end If
  Next
End Sub


Sub Run
Account=InputBox ("Please Enter User ID","Sybase Image Account 1.0")
CheckAccount


End Sub

  
Sub CheckAccount
    If CheckExpression (Account, "^I[0-9]{6}$") Then 

		wshshell.Run "%comspec% /c net localgroup administrators "& Account &" /ADD", 0, True
		wshshell.Run "%comspec% /c net localgroup administrators GLOBAL\"& Account &" /ADD", 0, True
		wshshell.RegWrite "HKEY_LOCAL_MACHINE\SOFTWARE\SAP\IT Template\Images\Owner",Account, "REG_SZ"
		wshshell.RegWrite "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\RegisteredOwner",Account, "REG_SZ"
		wshshell.RegWrite "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SAP\IT Template\Images\Owner",Account, "REG_SZ"
		'wshshell.RegWrite "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lanmanserver\parameters" ,Account,"REG_SZ"
		CompDescription Account
		WshShell.Run "RunDll32.exe USER32.DLL,UpdatePerUserSystemParameters" ,1 ,True
		DeleteAccounts
		MsgBox "Finished Setting User Account. Please reboot to have it take effect.",,"SAP IT"
		
	ElseIf CheckExpression (Account, "^D[0-9]{6}$") Then 
		wshshell.Run "%comspec% /c net localgroup administrators "& Account &" /ADD", 0, True
		wshshell.Run "%comspec% /c net localgroup administrators GLOBAL\"& Account &" /ADD", 0, True
		wshshell.RegWrite "HKEY_LOCAL_MACHINE\SOFTWARE\SAP\IT Template\Images\Owner",Account, "REG_SZ"
		wshshell.RegWrite "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SAP\IT Template\Images\Owner",Account, "REG_SZ"
		wshshell.RegWrite "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\RegisteredOwner",Account, "REG_SZ"
		'wshshell.RegWrite "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lanmanserver\parameters" ,Account,"REG_SZ"
		CompDescription Account
		WshShell.Run "RunDll32.exe USER32.DLL,UpdatePerUserSystemParameters" ,1 ,True
		DeleteAccounts
		MsgBox "Finished Setting User Account. Please reboot to have it take effect.",,"SAP IT"
		
	ElseIf CheckExpression (Account, "^C[0-9]{7}$") Then 
		wshshell.Run "%comspec% /c net localgroup administrators "& Account &" /ADD", 0, True
		wshshell.Run "%comspec% /c net localgroup administrators GLOBAL\"& Account &" /ADD", 0, True
		wshshell.RegWrite "HKEY_LOCAL_MACHINE\SOFTWARE\SAP\IT Template\Images\Owner",Account, "REG_SZ"
		wshshell.RegWrite "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SAP\IT Template\Images\Owner",Account, "REG_SZ"
		wshshell.RegWrite "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\RegisteredOwner",Account, "REG_SZ"
		'wshshell.RegWrite "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lanmanserver\parameters" ,Account,"REG_SZ"
		CompDescription Account
		WshShell.Run "RunDll32.exe USER32.DLL,UpdatePerUserSystemParameters" ,1 ,True
		DeleteAccounts
		MsgBox "Finished Setting User Account. Click OK to reboot to have it take effect.",,"SAP IT"
		
		
	Else
		MsgBox "The Account information is incorrect Please try again!"
		Wscript.Quit
    End If
	WshShell.Run "C:\Windows\System32\shutdown.exe -r -t 0"
End Sub

Sub DeleteAccounts
  		wshshell.Run "%comspec% /c net localgroup administrators SAP_ALL\sybaseimg /DEL", 0, True
		wshshell.Run "%comspec% /c net localgroup administrators GLOBAL\sybaseimg /DEL", 0, True
		CheckRegistryForProfile  "C:\Documents and Settings\sybaseimg"
		CheckRegistryForProfile  "C:\Documents and Settings\sybaseimg"
		CheckRegistryForProfile "C:\Users\sybaseimg"
		CheckRegistryForProfile "C:\Users\sybaseimg"
		
		DeleteProfile
		'DeleteScript
  End Sub 

Sub DeleteProfile
'On Error Resume Next
UserProfile = "c:\Users"


'MsgBox UserProfile & "_" & WshShell.SpecialFolders(0)
	If FSO.FolderExists (Userprofile & "\sybaseimg")Then
		MsgBox Userprofile & "\sybaseimg"
		wshShell.Run "%COMSPEC% /c Echo Y| cacls "& Userprofile & "\sybaseimg" & " /t /c /g everyone:F ", 0, True
		wshShell.Run "%COMSPEC% /c RD "& Chr(34) & Userprofile & "\sybaseimg" & Chr(34) & " /s /q", 0, True
		wshShell.Run "%COMSPEC% /c setEcho Y| cacls "& Userprofile & "\sybaseimg" & " /t /c /g everyone:F ", 0, True
		wshShell.Run "%COMSPEC% /c RD "& Chr(34) & Userprofile & "\sybaseimg" & Chr(34) & " /s /q", 0, True
	Else 
		'nothing
	End If
End Sub

 Sub CompDescription(TheUserName)
 Const HKEY_LOCAL_MACHINE = &H80000002
strComputer = "."
Set objRegistry = GetObject ("winmgmts:\\" & strComputer & "\root\default:StdRegProv")
strKeyPath = "System\CurrentControlSet\Services\lanmanserver\parameters"
strValueName = "srvcomment"
strDescription = TheUserName
objRegistry.SetStringValue HKEY_LOCAL_MACHINE, strKeyPath, strValueName, strDescription
 End Sub

    
Function CheckExpression(test, TestPattern)
	Set objRegExpr = New regexp
	objRegExpr.Pattern = TestPattern
	objRegExpr.Global = True
	objRegExpr.IgnoreCase = True
	Set colMatches = objRegExpr.Execute(test)
	For Each objMatch In colMatches
		CheckExpression = True
	Next
	Set colMatches = Nothing
	Set objRegExpr = Nothing
End Function

Sub CheckRegistryForProfile(ProfileImagePath)
Dim KeyToDelete
On Error Resume Next
  Const HKLM = &H80000002 'HKEY_LOCAL_MACHINE
  Set oRegistry = GetObject("winmgmts:{impersonationLevel=impersonate}!\\./root/default:StdRegProv")
  sBaseKey = "SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList\"
  iRC = oRegistry.EnumKey(HKLM, sBaseKey, arSubKeys)
  For Each sKey In arSubKeys
   iRC = oRegistry.GetStringValue( HKLM, sBaseKey & sKey, "ProfileImagePath", sValue)
    If UCase(Svalue) = Ucase(ProfileImagePath) Then
	  	 	'MsgBox UCase(Svalue)&" = "& Ucase(ProfileImagePath)
	  	 	KeyToDelete = sBaseKey & sKey 
	   	Else
	   	    'Do nothing
	   		'MsgBox UCase(Svalue)&" <> "& Ucase(ProfileImagePath)
	   	End If
  Next
  
  If KeyToDelete <> "" Then
  	DeleteRegKey KeyToDelete
  Else
  	'Do Nothing
  End If
 End Sub
 
  
 Sub DeleteRegKey (KeyToDelete)
 Const HKLM = &H80000002 'HKEY_LOCAL_MACHINE
 Set oReg = GetObject("winmgmts:{impersonationLevel=impersonate}!\\./root/default:StdRegProv")
 oReg.DeleteKey HKLM, KeyToDelete
 Set oReg = Nothing
 End Sub
 
 