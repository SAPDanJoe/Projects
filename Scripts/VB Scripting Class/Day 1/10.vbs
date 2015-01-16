Option Explicit

Dim myNumber
Dim i					'to be used as the itterator for a for loop
myNumber = 65

'msgbox Hex(mynumber)	'prints 65 in hexadecimal (41) = (4*16^1) + )1*16^0)

'msgbox Chr(myNumber)	'takes a value and produces the ASCII cahracter for that value

for i = 1 to 10
	msgbox i
	'i = i + 1 unnecessary because a for loop auto incriments its itterator
next
'at this point i is 11
