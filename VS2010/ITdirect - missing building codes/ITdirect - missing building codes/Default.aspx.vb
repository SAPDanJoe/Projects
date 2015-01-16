Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If uploadForm.Visible = False Then
            uploadForm.Visible = True
            DataForm.Visible = False
        Else
            uploadForm.Visible = False
            DataForm.Visible = True
        End If
        DebugTXT.Text = "Upload Form Visible: " & uploadForm.Visible.ToString & _
        " | Data Form Visible: " & DataForm.Visible.ToString
    End Sub

End Class