Option Explicit

Dim money
Dim SomeWholeNumber
money = 1200.45
someWholeNumber = 10

msgbox someWholeNumber + money			'implicit conversion of whole number to floating point
msgbox CSng(someWholeNumber) + money	'explicit conversion of same whole number to float (better)

'with or without the Csng conversion function, SomeWholeNumber is cast in memopry to a Single precision float point value (fancy name for a nomber with something to the right of the decimal.)
