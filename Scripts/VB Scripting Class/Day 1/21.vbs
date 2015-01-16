Option Explicit
dim myData
myData ="Bruce"

MySub myData
MySub2 myData

MySub2(myData)
'BUG:^^ if we call a function without a call statement, USING parens,and ONLY
'if you have a single parameter, any byref call is treated as a byval 


'Data when passed by ref just send a copy of the data, changes to data in sub do not affect 
'the original value of myData
Sub MySub2 (byref data)
	data = "Chuck"
	msgbox "Sub2 data:" + data		'
	msgbox "Sub2 myData:" + myData
End Sub

'Data when passed by val just send a copy of the data, changes to data in sub do not affect 
'the original value of myData
Sub MySub(byval data)
	data = "Chuck"
	msgbox "Sub data:" + data		'
	msgbox "Sub myData:" + MyData
End Sub