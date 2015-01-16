Public Class WebForm2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ProcLabel.Visible = False
    End Sub

    Protected Sub LoadData_Click(sender As Object, e As EventArgs) Handles LoadData.Click
        debugText.Text = "You Clicked the button!"
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
            Button3.Text = "Show Results"
        Else
            'GridView1.DataBind()
            Panel2.Visible = True
            Button3.Text = "Hide Results"
        End If
    End Sub

    Protected Sub Upload_Click(sender As Object, e As EventArgs) Handles Upload.Click
        If FileUpload1.FileName.ToString.Contains("tickets.xlsx") Then 'if there is a file to upload, upload it
            If System.IO.File.Exists("D:\inetpub\wwwroot\ITdirect\upload\tickets.xlsx") Then 'if the file already exists, delete it to make room for the new one
                System.IO.File.Delete("D:\inetpub\wwwroot\ITdirect\upload\tickets.xlsx")
            End If
            Try
                FileUpload1.SaveAs("D:\inetpub\wwwroot\ITdirect\upload\" & _
                   FileUpload1.FileName)
                Label1.Text = "Upload Complete! <br> File name: " & _
                   FileUpload1.PostedFile.FileName & "<br>" & _
                   "File Size: " & _
                   FileUpload1.PostedFile.ContentLength & " kb"
                '    uploadFailed = False
            Catch ex As Exception
                Label1.Text = "ERROR: " & ex.Message.ToString()
                '    uploadFailed = True
            End Try

        Else
            Label1.Text = "The file that you specified is not vailid. <br>Please select 'tickets.xlsx'"
        End If
        RegularExpressionValidator1.Visible = False
    End Sub

    Protected Sub RunProcedure_Click(sender As Object, e As EventArgs) Handles RunProcedure.Click
        Dim cs As String
        Dim strCommand As String

        ProcLabel.Text = ". ."

        cs = "Persist Security Info=False;Integrated Security=true;Server=OAKD00465226A;Database=Import;User Id=user;Password=kPdnpZn0603;"
        strCommand = "Execute HuntBuildingCodes"

        Dim connection As New System.Data.SqlClient.SqlConnection(cs)
        Dim command As New SqlClient.SqlCommand(strCommand, connection)
        ProcLabel.Visible = True

        Try
            connection.Open()
            Dim rowsAffected As Integer = command.ExecuteNonQuery()
            ProcLabel.Text = "Processing Complete. " ' & rowsAffected & " blank buildings were identified."
        Catch ex As Exception
            ProcLabel.Text = "Error Occurred: " & ex.Message.ToString
        End Try

    End Sub
End Class