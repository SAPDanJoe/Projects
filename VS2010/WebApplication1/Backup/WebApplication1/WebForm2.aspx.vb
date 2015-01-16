Public Class WebForm2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub



    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Panel1.Visible = True Then
            Panel1.Visible = False
            Button2.Text = "Show Panel 1"
        Else
            Panel1.Visible = True
            Button2.Text = "Hide Panel 1"
        End If
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Panel2.Visible = True Then
            Panel2.Visible = False
            Button3.Text = "Show Panel 2"
        Else
            Panel2.Visible = True
            Button3.Text = "Hide Panel 2"
        End If
    End Sub

    Protected Sub Upload_Click(sender As Object, e As EventArgs) Handles Upload.Click
        If FileUpload1.HasFile Then 'if there is a file to upload, upload it
            If System.IO.File.Exists("D:\inetpub\wwwroot\ITdirect\upload\tickets.xlsx") Then 'if the file already exists, delete it to make room for the new one
                System.IO.File.Delete("D:\inetpub\wwwroot\ITdirect\upload\tickets.xlsx")
            End If
            Try
                FileUpload1.SaveAs("D:\inetpub\wwwroot\ITdirect\upload\" & _
                   FileUpload1.FileName)
                Label1.Text = "File name: " & _
                   FileUpload1.PostedFile.FileName & "<br>" & _
                   "File Size: " & _
                   FileUpload1.PostedFile.ContentLength & " kb<br>" & _
                   "Content type: " & _
                   FileUpload1.PostedFile.ContentType
                '    uploadFailed = False
            Catch ex As Exception
                Label1.Text = "ERROR: " & ex.Message.ToString()
                '    uploadFailed = True
            End Try

        Else
            Label1.Text = "You have not specified a file."
        End If
    End Sub

End Class