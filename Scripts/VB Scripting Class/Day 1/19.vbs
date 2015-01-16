Option Explicit
ReDim People (0)	'redim requires initial size
Dim SizeOfArray
Dim i
Dim msg

SetPlayerCount()
SetPlayerNames()
GetPlayerNames()

'------------------------------------------------------------------

Sub SetPlayerCount()
	Do
		SizeofArray = inputBox("Enter the number of players: ", "Determine Player count", 0)
	Loop Until SizeOfArray >= 0

	msgBox "You chose " + SizeOfArray  + " players."

	'resize the array based on what was entered by the user
	redim People(SizeOfArray - 1)
End Sub

'------------------------------------------------------------------

Sub SetPlayerNames()
	'populate array with names of players from user input
	for i = lbound(People) to ubound(People)
		people(i) = inputbox("Enter player name: ","Player Names",0)
	next
End Sub

'------------------------------------------------------------------

sub GetPlayerNames()
	msg = "Players: " + vbCRLF
	i = 0 ' reset i to 0 to make sure this starts running at zero
	for i = lbound(People) to ubound(People)
		msg = msg + people(i) + vbCRLF
	next
	msgbox msg
End Sub