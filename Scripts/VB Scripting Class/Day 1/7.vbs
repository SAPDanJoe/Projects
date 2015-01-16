Option Explicit
dim x
x=10

'The following syntax doesn't need and ENDif because the body of the if
'statement is on one line:

if x = 10 then msgbox x

'The following syntax requires an end if statement because the body of the 
'if block is on a seperate line as the if itself:

if x=10 then
	msgbox x
end if