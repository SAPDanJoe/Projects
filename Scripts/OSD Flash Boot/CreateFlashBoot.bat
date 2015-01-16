@echo off
cls

::Handle passed parameters
set /a ParamCount=0
set /a ISOCount=0
set OSDISO=
set Mode=
set ErrorText=
set Abort=

:ParamLoop
	if %1.==. goto ExitParamLoop
	set /a ParamCount+=%ParamCount%
	::Evaluate the current parameter here
	::Check if parameter is an ISO or Mode
	echo %1 | findstr /i /r ".*\.iso" > nul	
	if errorlevel 1 (
		::Not an ISO, must be a Mode
		goto SetMODE
	) else (
		::Is an ISO
		set /a ISOCount+=%ISOCount%
		goto SetISO
	)

:SetISO
	set OSDISO=%1
	SHIFT
	goto ParamLoop

:SetMODE
	set Mode=Multiple Drives
	SHIFT
	goto ParamLoop

:ExitParamLoop

	:NoSetIso
		::If no ISO was set by parameter (above), use the ISO in the current directory.
		if %OSDISO%.==. (
			dir *.iso /b 2>&1 nul
			cls
			if errorlevel 1 (
				goto ISOERROR
			) else (
				for /f %%i in ('dir *.iso /b') do (
					set OSDISO=%%i
					set /a ISOCount+=%ISOCount%				
				)
			)
		goto ExitParamLoop				
		)

	:NoModeSet
		::If no Mode has been set by parameter (above), use the default (Single Flashdrive mode).
		if %Mode%.==. (
			set Mode=Single Drive
			goto ExitParamLoop
		)
	
:ISOERROR
	::If more that 1 ISO, or no ISOs were passed, notify and prompt
	if %ISOCount%==0 (
		::No ISO
		set ErrorText=FATAL ERROR: No ISO was passed by parameter, nor was one present in the local folder.
	) else ( if %ISOCount%==1 (
			::Single ISO, no error
			goto TheEnd
		) else (
			::More than 1 ISO, warning
			set ErrorText=%ISOCount% ISOs were specified. %OSDISO% is selected and will be used.
		)
	)
	echo %errortext%
	set /p Abort=Would you like to cancel? Yes/No
	Call :Qestion_Abort %Abort%
	
:MediaError
::If more that 1 Flash drive is connected
	echo FATAL ERROR: %1 removable drives are currently attached.  PLease ensure there is only 1 volume attached.
	set /p Abort=Would you like to retry? Yes/No
	echo %Abort% | findstr /i /r "n" > nul
	Call :Qestion_Abort %Abort%

:Question_Abort
	echo %1 | findstr /i /r "n" > nul
	if errorlevel 1 (
		cls
		goto TheEnd
	)
	goto EOF
	
:TheBegining
	echo.
	echo The source ISO is:	%OSDISO%
	echo The mode is:		%Mode%
	echo.


set numb=0
for /F ('Call :GetRemMedia %numb%')

::Handle initial settings
set ThisTemp=%temp%\SAP-IT
set OSDDir=%ThisTemp%\OSD
set DestDrive=

:GetRemMedia
	set passed
	for /L %%z in (1,1,1) do set Letter_%%z=
	FOR /F "skip=1 tokens=*" %%A in (
	'wmic volume get name^,DriveType'
	) do call :Sub2 %%A
	
	for /l %%n in (1,1,1) do (
		set /a passed+=1
		if not %%Letter_%%n%%.==. (
			set DestDrive=%%Letter_%%n%%)
	)
	set /a passed-=1
call :MediaCheck %passed%

:MediaCheck
	if %1.==0. (
		set flash=discon
		goto :EOF
	) else (
		if %1.==1. 	(
			set flash=con
			goto :EOF
		)
	)
	call :MediaError %1
	goto :EOF

if not exist %ThisTemp% goto MakeIT
goto FillIT
goto RunIT
set /p More=Please insert the next drive.  Press Y to continue or N to exit. 
rmdir /S /Q %thisTemp%

:MakeIT
	goto RunIT
	md %ThisTemp%

:CleanIT
	rmdir %ThisTemp% /S /Q
	set Clean=No
	del %ThisTemp%\PartScript.scr
	goto TheEnd

:FillIT
	::Exctract the ISO into the temp folder
	start /wait "%programfiles%\winRAR\winRAR.exe" x %OSDISO% %OSDDir%\
	echo SELECT DISK 1 >>%ThisTemp%\PartScript.scr
	echo CLEAN>>%ThisTemp%\PartScript.scr
	echo CREATE PART PRIMARY>>%ThisTemp%\PartScript.scr
	echo ACTIVE>>%ThisTemp%\PartScript.scr
	echo FORMAT FS=NTFS QUICK>>%ThisTemp%\PartScript.scr
	echo EXIT>>%ThisTemp%\PartScript.scr
:RunIt
	diskpart /S %ThisTemp%\PartScript.scr	
	xcopy %OSDDir%\* E:\ /e /v /h /k /j
	::check run mode to image another drive
	if %mode%.=Single Drive. goto :TheEnd else goto :RunIt

:TheEnd
	echo Process complete.
	::all drives have been imaged, check if cleanup has been done.
	echo Cleaning up.
	pause
	goto :EOF
	
:Sub2
	if [%2]==[] goto :EOF
	if not [%1]==[2] goto :EOF
	set /a numb+=1
	(set Type_%numb%=%1)
	(set Letter_%numb%=%2)
	