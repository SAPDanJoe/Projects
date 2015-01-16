@echo off

::	This can be either SAPStart8 or Start8
Set VER=SAPStart8

::	Self extractor should have placed the following files in %TEMP%\SAP-IT\Start8\		::
::	File Name			|Description													::
::	--------------------|--------------------------------------------------------------	::
::	SAPStart.lnk		|Link file for the SAP logo'd version of the start button		::
::	SAPStart8.ico		|SAP logo'd Start Icon File (not optimized, single layer)		::
::	Start.lnk			|Link file for the unbrabded start button						::
::	Win8.ico			|Start Icon File (not optimized, single layer)					::
::	Win8Start.reg		|Reg file to add the link to the 1st position on the taskbar	:: 				

::	Copy temp files to a permenant dirctory
copy %TEMP%\SAP-IT\Start8\%VER%.* %appdata%\SAP\

::	Copy links to the Taskbar and the Start menu
copy %appdata%\SAP\%VER%.lnk "%appdata%\Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar\Start.lnk"
%TEMP%\SAP-IT\Start8\PinToStart.vbs "%appdata%\Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar\Start.lnk"

::	This will put the new Start Button in the 1st position of the taskbar.
::	!! NOTE: I have not added anything to position the button on the start screen !!
reg import %TEMP%\SAP-IT\Start8\Set_%VER%.reg

cls

:: Echo Windows explorer will now be restarted
:: pause
start /wait taskkill /f /im "explorer.exe"
explorer.exe