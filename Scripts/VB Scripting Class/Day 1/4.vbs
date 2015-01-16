'introducing constans.  this extends the last example by using
'built in constants to make the code readble

result = msgBox("click a button",VBYesNoCancel,"Fun with messages")

Select Case result
	Case vbYes
		msgbox("you clicked yes")
	Case vbNo
		msgbox("you clicked no")
	Case vbCancel
		msgbox("you clicked cancel")
	Case Else
		msgbox(result)
End Select

'Google VBScript Built in constants