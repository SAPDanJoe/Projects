Option Explicit

'Dim Person		'Create a scalar variable

Dim People (3)	'Every language EXCEPT VB would make an array of size 3 but the number here is not the size of the array its 
				'the maximum adressable position. This creates and array (a collecftion of values) of size 4.

People(0) = "Tammy"
People(1) = "Brian"
People(2) = "Alyssa"
People(3) = "Bruce"

Dim Msg, i
msg = "Names: " + vbCRLF

for i = 0 to 3
	Msg = Msg+People(i) + vbCRLF
Next
msgBox msg