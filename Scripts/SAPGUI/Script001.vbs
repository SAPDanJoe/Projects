If Not IsObject(application) Then
   Set SapGuiAuto  = GetObject("SAPGUI")
   Set application = SapGuiAuto.GetScriptingEngine
End If
If Not IsObject(connection) Then
   Set connection = application.Children(0)
End If
If Not IsObject(session) Then
   Set session    = connection.Children(0)
End If
If IsObject(WScript) Then
   WScript.ConnectObject session,     "on"
   WScript.ConnectObject application, "on"
End If
session.findById("wnd[0]").resizeWorkingPane 142,23,false
session.findById("wnd[0]/usr/cntlIMAGE_CONTAINER/shellcont/shell/shellcont[0]/shell").doubleClickNode "F00002"
session.findById("wnd[0]/usr/ctxtEQUI-EQUNR").text = "563015"
session.findById("wnd[0]/usr/ctxtEQUI-EQUNR").caretPosition = 6
session.findById("wnd[0]").sendVKey 0
session.findById("wnd[0]/usr/tabsTABSTRIP1/tabpUSR/ssubSUB:ZFI_EQMT_ASSET_MT:2000/ctxtPREL_PERNR_C").text = "i837633"
session.findById("wnd[0]/usr/tabsTABSTRIP1/tabpUSR/ssubSUB:ZFI_EQMT_ASSET_MT:2000/ctxtILOA-ABCKZ").text = "U"
session.findById("wnd[0]/usr/tabsTABSTRIP1/tabpUSR/ssubSUB:ZFI_EQMT_ASSET_MT:2000/ctxtILOA-ABCKZ").setFocus
session.findById("wnd[0]/usr/tabsTABSTRIP1/tabpUSR/ssubSUB:ZFI_EQMT_ASSET_MT:2000/ctxtILOA-ABCKZ").caretPosition = 1
session.findById("wnd[0]").sendVKey 11
session.findById("wnd[0]/tbar[0]/btn[12]").press
