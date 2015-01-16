Option Explicit
on error resume next
dim number
number = 1/0
If err.number <> 0 then 
	WScript.Echo err.number & ": " & err.Description
end If
