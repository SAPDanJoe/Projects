Option Explicit

Dim People (3)	'This creates and array (a collecftion of values) of size 4.

Dim Msg, i
msg = "Names: " + vbCRLF

for i = lbound(People) to ubound(People)
	People(i) = inputbox("Enter name: ","Populate names list","Enter name here")
Next

for i = lbound(People) to ubound(People)
	msg = msg + People(i) + vbCRLF
Next

msgBox msg