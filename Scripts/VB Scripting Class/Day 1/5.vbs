Option Explicit

'a function is generic term for a named block of code that we call by
'name to perform some task.  msgbox and inputbox are built in functions.


Select Case Weekday(date)
	Case vbSunday
		msgbox "Sunday"
	Case vbMonday
		msgbox "Monday"
	Case vbTuesday
		msgbox "Tuesday"
	Case vbWednesday
		msgbox "Wednesday"
	Case vbThursday
		msgbox "Thursday"
	Case vbFriday
		msgbox "Friday"		
	Case vbSaturday
		msgbox "Saturday"
End Select
		
'Option Explicit requires all variables and constants to be declared
'before they are used