Set objShell = CreateObject("Shell.Application")
Set FSO = CreateObject("Scripting.FileSystemObject")
strPath = FSO.GetParentFolderName (WScript.ScriptFullName)
If FSO.FileExists(strPath & "\AdminRemovalSybase.VBS") Then
     objShell.ShellExecute "wscript.exe", _
        Chr(34) & strPath & "\AdminRemovalSybase.VBS" & Chr(34), "", "runas", 1
Else
     MsgBox "Script file AdminRemovalSybase.VBS not found"
End If 