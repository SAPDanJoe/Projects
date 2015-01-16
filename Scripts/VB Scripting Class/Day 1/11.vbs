Option Explicit

Dim i
Dim Message

'Display letters of the alphabet
'for i = 65 to 90
'	Message = Message + chr(i) + ", "
'next
'msgbox Message

'Display all ASCII characters
for i = 30 to 255
	Message = Message + chr(i) + ", "
next

msgbox Message

'if you wanted to have a new line betwwen output values, concatenate vbCRLF
'or concatenate chr(10) or chr(13)