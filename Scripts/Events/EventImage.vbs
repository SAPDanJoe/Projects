'Sets up an event image:
'Detect Runlevel by checking where script is launched
'If 1st run, place script in c:\scripts
'	Add Local User
'	Set local user as Admin
'	Remove PC from domain
'	Set AutoLogin for LocalUser
'	Reboot
'Else
'	Set screensaver timout to 9999
'	Remove Software
'		SCCM
'		Symantec Encryption Desktop
'		1E Agent
'		1E web wake
'	Taskbar
'		Unpin Outlook and Lync
'		Pin Word, Excel, PowerPoint
'		

addUser "EventUser","1Event2Day!"
addAdmin "EventUser"
'Dim Stuff
Dim ServerName,UserName,Password,GroupName
Dim container,user,Flags,Group

Function getComputer()
'Get computer name 
	Dim objNet
	Set objNet = WScript.CreateObject("WScript.Network") 
	getComputer = objNet.ComputerName 
	Set objNet = Nothing 
End Function

Function addUser(strUser, strPassword)
	'get local machine name
	strComputer = "."
	
	'create a connection to the local machine's account service
	Set colAccounts = GetObject("WinNT://" & strComputer & "")
	
	'create a user object with the username passed to the function
	Set objUser = colAccounts.Create("user", strUser)
	
	'store the password in the object
	objUser.SetPassword strPassword
	
	'Create the user with the username and password
	objUser.SetInfo
	
	'Sets Change Pass at Next Login and Password Never Expires
	Set User = GetObject("WinNT://" & strComputer & "/" & strUser & ",User")
		Flags = User.Get("UserFlags")
'		User.Put "PasswordExpired", 0	'0 = NOT SET User must change pass at next login 1 = SET User must change pass at next login
'		User.Put "UserFlags", Flags XOR &H10000
		User.Put "UserFlags", &H0040        'User Cannot Change password
		User.SetInfo
	set User = nothing
End Function

Function addAdmin(strUser)
	'Puts User in Group
	Dim strComputer, User, Group
	strComputer = getComputer()
	Set User = GetObject("WinNT://" & strComputer & "/" & strUser & ",User")
	Set Group = GetObject("WinNT://" & strComputer & "/Administrators,Group")
		Group.Add(User.ADsPath)
		Group.SetInfo
	set User = nothing
	set Group = nothing
End Function


''Puts User in Group
'Set User = GetObject("WinNT://" & ServerName & "/" & UserName & ",User")
'Set Group = GetObject("WinNT://" & ServerName & "/" & GroupName & ",Group")
'	Group.Add(User.ADsPath)
'	Group.SetInfo
'set User = nothing
'set Group = nothing

'wscript.echo "The User:" & UserName & " has been added to GroupName:" & GroupName & " on Server:" & ServerName & "."
'objStdIn.close
'objStdOut.close
'wscript.quit
'	
Function getComputer()
'Get computer name 
	Dim objNet
	Set objNet = WScript.CreateObject("WScript.Network") 
	getComputer = objNet.ComputerName 
	Set objNet = Nothing 
End Function