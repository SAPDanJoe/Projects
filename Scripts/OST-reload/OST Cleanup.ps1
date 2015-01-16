
Stop-Process outlook.exe
set OLdataRoot="C:\Users\%username%\AppData\Local\Microsoft\Outlook"
md %OLdataRoot%\backup
copy %OLdataRoot%\*.ost %OLdataRoot%\backup\*.ost
del %OLdataRoot%\*.ost
Start-Process outlook.exe