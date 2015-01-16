'Option Explicit

Dim myNumber
Dim myString

'msgbox myNymber + myString		'WORKS: Produces 0

'msgbox 13 + 19					'WORKS: Produces 32

'msgbox "1" + "1"				'WORKS: Produces "11"

'msgbox 1 + "23" + 2			'WORKS: produces 26, because 23 was coerced from string to number
								'The attempt to use the + symbol check to see if all arguments have matching data types
								'and if not, if all arguments match the data type of the first argument
								'msgbox as a function cannot take a number, only a string, so after the convertion of data types occurs along with the addition, the sum is then converted to a string so the msgbox can display it.


myNumber = 100
myString = "You entered: "

'msgbox myString + myNumber		'FAILS dues to type mismatch
msgbox myString + CStr(myNumber)'WORKS: after explicit conversion to make both arguments the same data type (string)

'Cstr is a built in convertion funcion that converts its argument to a String.  See also cint or search vbscript conversion funtions
