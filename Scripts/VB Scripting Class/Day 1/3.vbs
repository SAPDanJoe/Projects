'introducing Message Box options

result = msgBox("click a button",8,"Fun with messages")

Select Case result
	Case 6
		msgbox("you clicked yes")
	Case 7
		msgbox("you clicked no")
	Case 2
		msgbox("you clicked cancel")
	Case Else
		msgbox("Unhandled result: " + result)
End Select

'3 - yes no cancel
'1 - ok cancel
'+16 - Turns message into error
'+32 - blue question mark
