	Dim objNet, getComputer 
	Set objNet = WScript.CreateObject("WScript.Network") 
	getComputer = objNet.ComputerName 
wscript.echo getComputer 