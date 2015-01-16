Option Explicit

'Visual Basic (all versions) have two types of proceedures... Subs and Functions.  Functions return values, and subs do not.
'in Languages like C, ++, C#, Java we just have a function and if it has no return value we specify it as a function that
'returns void

msgBox IsWeekday(date)

'------------------------------------------------------------------------
Function IsWeekDay(dateValue)

	dim day
	day = Weekday(date)

	if day >= vbMonday and day <=vbFriday  Then
		IsWeekDay = True
	ElseIF day = vbSaturday or day = vbSunday Then
		IsWeekDay = False
	Else
		msgbox "there really shouldn't be anything else..."
	End if
	
End Function