Set objShell = WScript.CreateObject("Wscript.Shell")
objShell.Run "Calc.exe"
Do Until Success = True
	Success = objShell.AppActivate ("Calculator")
	wscript.sleep 1
loop
objshell.SendKeys "2"
objshell.SendKeys "{+}"
objshell.SendKeys "2"
objshell.SendKeys "{ENTER}"