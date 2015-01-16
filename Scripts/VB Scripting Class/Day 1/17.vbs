Option Explicit
'Introduces the ability to dynamically resize an array and preserve any existing data

reDim People (1)		'original declaration needs to be redim so the array can be resized later
People(0) = "Player01"
People(1) = "Player02"

redim Preserve People(2)'resize the array to 3 positions, keeping all existing data
msgbox People(0)		' proves loss of the data

redim People(5)			'resize the array to 6 positions, losing all existing data
msgbox People(0)		' proves loss of the data