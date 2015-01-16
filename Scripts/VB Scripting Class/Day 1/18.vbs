Option Explicit

Dim People ()
Dim SizeOfArray, i, msg
msg = "Players: " + vbCRLF

Do
	SizeofArray = inputBox("Enter the number of players: ", "Determine Player count", 0)
Loop Until SizeOfArray >= 0

msgBox "You chose " + SizeOfArray  + " players."

redim People(SizeOfArray - 1)

for i = lbound(People) to ubound(People)
	people(i) = inputbox("Enter player name: ","Player Names",0)
next

for i = lbound(People) to ubound(People)
	msg = msg + people(i) + vbCRLF
next
msgbox msg