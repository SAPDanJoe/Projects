<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.FormTabs = New System.Windows.Forms.TabControl()
        Me.ProjectTab = New System.Windows.Forms.TabPage()
        Me.MonsoonTab = New System.Windows.Forms.TabPage()
        Me.ApplyButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.RevertButton = New System.Windows.Forms.Button()
        Me.ProjectNameLabel = New System.Windows.Forms.Label()
        Me.AccessKeyLabel = New System.Windows.Forms.Label()
        Me.SecretKeyLabel = New System.Windows.Forms.Label()
        Me.ProjectNameComboBox = New System.Windows.Forms.ComboBox()
        Me.AccessKeyTextBox = New System.Windows.Forms.TextBox()
        Me.SecretKeyTextBox = New System.Windows.Forms.TextBox()
        Me.WorkstationSettingsTab = New System.Windows.Forms.TabPage()
        Me.FormTabs.SuspendLayout()
        Me.ProjectTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'FormTabs
        '
        Me.FormTabs.Controls.Add(Me.ProjectTab)
        Me.FormTabs.Controls.Add(Me.MonsoonTab)
        Me.FormTabs.Controls.Add(Me.WorkstationSettingsTab)
        Me.FormTabs.Enabled = False
        Me.FormTabs.Location = New System.Drawing.Point(13, 13)
        Me.FormTabs.Name = "FormTabs"
        Me.FormTabs.SelectedIndex = 0
        Me.FormTabs.Size = New System.Drawing.Size(300, 236)
        Me.FormTabs.TabIndex = 0
        '
        'ProjectTab
        '
        Me.ProjectTab.Controls.Add(Me.SecretKeyTextBox)
        Me.ProjectTab.Controls.Add(Me.AccessKeyTextBox)
        Me.ProjectTab.Controls.Add(Me.ProjectNameComboBox)
        Me.ProjectTab.Controls.Add(Me.SecretKeyLabel)
        Me.ProjectTab.Controls.Add(Me.AccessKeyLabel)
        Me.ProjectTab.Controls.Add(Me.ProjectNameLabel)
        Me.ProjectTab.Controls.Add(Me.RevertButton)
        Me.ProjectTab.Controls.Add(Me.SaveButton)
        Me.ProjectTab.Controls.Add(Me.ApplyButton)
        Me.ProjectTab.Location = New System.Drawing.Point(4, 22)
        Me.ProjectTab.Name = "ProjectTab"
        Me.ProjectTab.Padding = New System.Windows.Forms.Padding(3)
        Me.ProjectTab.Size = New System.Drawing.Size(292, 210)
        Me.ProjectTab.TabIndex = 0
        Me.ProjectTab.Text = "Project Settings"
        Me.ProjectTab.UseVisualStyleBackColor = True
        '
        'MonsoonTab
        '
        Me.MonsoonTab.Location = New System.Drawing.Point(4, 22)
        Me.MonsoonTab.Name = "MonsoonTab"
        Me.MonsoonTab.Padding = New System.Windows.Forms.Padding(3)
        Me.MonsoonTab.Size = New System.Drawing.Size(292, 210)
        Me.MonsoonTab.TabIndex = 1
        Me.MonsoonTab.Text = "Monsoon Settings"
        Me.MonsoonTab.UseVisualStyleBackColor = True
        '
        'ApplyButton
        '
        Me.ApplyButton.Location = New System.Drawing.Point(7, 181)
        Me.ApplyButton.Name = "ApplyButton"
        Me.ApplyButton.Size = New System.Drawing.Size(75, 23)
        Me.ApplyButton.TabIndex = 0
        Me.ApplyButton.Text = "Apply"
        Me.ApplyButton.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveButton.Location = New System.Drawing.Point(89, 181)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(75, 23)
        Me.SaveButton.TabIndex = 1
        Me.SaveButton.Text = "Save"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'RevertButton
        '
        Me.RevertButton.Location = New System.Drawing.Point(170, 181)
        Me.RevertButton.Name = "RevertButton"
        Me.RevertButton.Size = New System.Drawing.Size(75, 23)
        Me.RevertButton.TabIndex = 2
        Me.RevertButton.Text = "Revert"
        Me.RevertButton.UseVisualStyleBackColor = True
        '
        'ProjectNameLabel
        '
        Me.ProjectNameLabel.AutoSize = True
        Me.ProjectNameLabel.Location = New System.Drawing.Point(7, 28)
        Me.ProjectNameLabel.Name = "ProjectNameLabel"
        Me.ProjectNameLabel.Size = New System.Drawing.Size(71, 13)
        Me.ProjectNameLabel.TabIndex = 3
        Me.ProjectNameLabel.Text = "Project Name"
        '
        'AccessKeyLabel
        '
        Me.AccessKeyLabel.AutoSize = True
        Me.AccessKeyLabel.Location = New System.Drawing.Point(7, 56)
        Me.AccessKeyLabel.Name = "AccessKeyLabel"
        Me.AccessKeyLabel.Size = New System.Drawing.Size(107, 13)
        Me.AccessKeyLabel.TabIndex = 4
        Me.AccessKeyLabel.Text = "AWS_ACCESS_KEY"
        '
        'SecretKeyLabel
        '
        Me.SecretKeyLabel.AutoSize = True
        Me.SecretKeyLabel.Location = New System.Drawing.Point(7, 82)
        Me.SecretKeyLabel.Name = "SecretKeyLabel"
        Me.SecretKeyLabel.Size = New System.Drawing.Size(108, 13)
        Me.SecretKeyLabel.TabIndex = 5
        Me.SecretKeyLabel.Text = "AWS_SECRET_KEY"
        '
        'ProjectNameComboBox
        '
        Me.ProjectNameComboBox.FormattingEnabled = True
        Me.ProjectNameComboBox.Location = New System.Drawing.Point(100, 25)
        Me.ProjectNameComboBox.Name = "ProjectNameComboBox"
        Me.ProjectNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.ProjectNameComboBox.TabIndex = 6
        '
        'AccessKeyTextBox
        '
        Me.AccessKeyTextBox.Location = New System.Drawing.Point(120, 53)
        Me.AccessKeyTextBox.Name = "AccessKeyTextBox"
        Me.AccessKeyTextBox.Size = New System.Drawing.Size(100, 20)
        Me.AccessKeyTextBox.TabIndex = 7
        '
        'SecretKeyTextBox
        '
        Me.SecretKeyTextBox.Location = New System.Drawing.Point(120, 79)
        Me.SecretKeyTextBox.Name = "SecretKeyTextBox"
        Me.SecretKeyTextBox.Size = New System.Drawing.Size(100, 20)
        Me.SecretKeyTextBox.TabIndex = 8
        '
        'WorkstationSettingsTab
        '
        Me.WorkstationSettingsTab.Location = New System.Drawing.Point(4, 22)
        Me.WorkstationSettingsTab.Name = "WorkstationSettingsTab"
        Me.WorkstationSettingsTab.Padding = New System.Windows.Forms.Padding(3)
        Me.WorkstationSettingsTab.Size = New System.Drawing.Size(292, 210)
        Me.WorkstationSettingsTab.TabIndex = 2
        Me.WorkstationSettingsTab.Text = "Workstation Settings"
        Me.WorkstationSettingsTab.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(325, 261)
        Me.Controls.Add(Me.FormTabs)
        Me.Name = "MainForm"
        Me.Text = "Monsoon Workstation Settings Tool"
        Me.FormTabs.ResumeLayout(False)
        Me.ProjectTab.ResumeLayout(False)
        Me.ProjectTab.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents FormTabs As System.Windows.Forms.TabControl
    Friend WithEvents ProjectTab As System.Windows.Forms.TabPage
    Friend WithEvents MonsoonTab As System.Windows.Forms.TabPage
    Friend WithEvents SecretKeyTextBox As System.Windows.Forms.TextBox
    Friend WithEvents AccessKeyTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ProjectNameComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents SecretKeyLabel As System.Windows.Forms.Label
    Friend WithEvents AccessKeyLabel As System.Windows.Forms.Label
    Friend WithEvents ProjectNameLabel As System.Windows.Forms.Label
    Friend WithEvents RevertButton As System.Windows.Forms.Button
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents ApplyButton As System.Windows.Forms.Button
    Friend WithEvents WorkstationSettingsTab As System.Windows.Forms.TabPage

End Class
