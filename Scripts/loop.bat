

set /a itteration=0
:TheBeginning
%1
echo Itteration: %itteration%
set /a itteration=%itteration%+1
ping -w 5000 -n 1 1.1.1.1 2>&1>nul
set /a refresh=%2

:RefreshLoop
echo Reloading in... %refresh%
ping -w 1000 -n 1 1.1.1.1 2>&1>nul
set /a refresh=%refresh%-1
::cls
goto RefreshLoop

goto TheBeginning