@echo off

:GetRemMedia
	set numb=0
	for /L %%z in (1,1,1) do set Letter_%%z=
	FOR /F "skip=1 tokens=*" %%A in (
	'wmic volume get name^,DriveType'
	) do call :Sub2 %%A

	for /l %%n in (1,1,1) do (
	set /a numb+=1
	call echo %%Letter_%%n%%)
	set /a numb-=1
	echo %numb% drives were detected.
	goto :EOF

:Sub2
	if [%2]==[] goto :EOF
	if not [%1]==[2] goto :EOF
	set /a numb+=1
	(set Type_%numb%=%1)
	(set Letter_%numb%=%2)