Option Explicit
'Arrays can be dynamically resized by using redim
'by default, all the data in the array when it is recreated is lost
'To preserve the existing data while you redim it, user the PRESERVE keyword after REDIM

Dim People ()	'Creates an array with no positions

Dim Msg, i
Dim SizeOfArray
msg = "Names: " + vbCRLF

Do
	SizeofArray = inputBox("Enter the number of players: ", "Determine Player count", 0)
Loop Until SizeOfArray >= 0

msgBox "You chose " + SizeOfArray  + " players."

redim People(SizeOfArray - 1)

msgbox "Array bounds are: Lbound(" +Cstr(lbound(people)) + "), Ubound(" + Cstr(ubound(People)) + ")."