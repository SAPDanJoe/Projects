Option Explicit

dim day
day = Weekday(date)

if day >= vbMonday and day <=vbFriday  Then
	msgbox "Weekday"
ElseIF day = vbSaturday or day = vbSunday Then
	msgbox "Weekend"
Else
	msgbox "there really shouldn't be anything else..."
End if

'with else it statements it keeps checking even adter it finds a match
'so if there are 200 choices, it does 200 checks.  A select case statement
'makes a single check always.
