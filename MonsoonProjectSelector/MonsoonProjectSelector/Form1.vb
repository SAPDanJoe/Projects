Public Class MainForm

    Private Sub ApplyButton_Click(sender As Object, e As EventArgs) Handles ApplyButton.Click
        'This is where the environmental variables will be checked and applied using
        'SetEnvironmentVariable(strVar, strVal) and SetEnvironmentVariable(strVar, strVal)
        If ProjectNameComboBox.SelectedText = "New" Then
            'when clicking apply on an unsaved profile, call the save button action
            'before applying the settings to the workstation
            SaveButton.PerformClick()
        End If

    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

    End Sub
End Class
