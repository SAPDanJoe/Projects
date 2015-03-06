namespace ScratchPad
{
    partial class ScratchForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OrgComboBox = new System.Windows.Forms.ComboBox();
            this.projectComboBox = new System.Windows.Forms.ComboBox();
            this.settingsLabel = new System.Windows.Forms.Label();
            this.publicKeyLabel = new System.Windows.Forms.Label();
            this.privateKeyLabel = new System.Windows.Forms.Label();
            this.publicKeyTextBox = new System.Windows.Forms.TextBox();
            this.privateKeyTextBox = new System.Windows.Forms.TextBox();
            this.orgLabel = new System.Windows.Forms.Label();
            this.projectLabel = new System.Windows.Forms.Label();
            this.AWS_Access_KEYTextBox = new System.Windows.Forms.TextBox();
            this.EC2_URLTextBox = new System.Windows.Forms.TextBox();
            this.AWS_ACCESS_KEYLabel = new System.Windows.Forms.Label();
            this.EC2_URLLabel = new System.Windows.Forms.Label();
            this.AWS_SECRET_KEYTextBox = new System.Windows.Forms.TextBox();
            this.AWS_SECRET_KEYLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OrgComboBox
            // 
            this.OrgComboBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.OrgComboBox.FormattingEnabled = true;
            this.OrgComboBox.Location = new System.Drawing.Point(126, 83);
            this.OrgComboBox.Name = "OrgComboBox";
            this.OrgComboBox.Size = new System.Drawing.Size(121, 21);
            this.OrgComboBox.TabIndex = 0;
            // 
            // projectComboBox
            // 
            this.projectComboBox.FormattingEnabled = true;
            this.projectComboBox.Location = new System.Drawing.Point(126, 110);
            this.projectComboBox.Name = "projectComboBox";
            this.projectComboBox.Size = new System.Drawing.Size(121, 21);
            this.projectComboBox.TabIndex = 1;
            // 
            // settingsLabel
            // 
            this.settingsLabel.AutoSize = true;
            this.settingsLabel.Location = new System.Drawing.Point(12, 9);
            this.settingsLabel.Name = "settingsLabel";
            this.settingsLabel.Size = new System.Drawing.Size(92, 13);
            this.settingsLabel.TabIndex = 2;
            this.settingsLabel.Text = "Monsoon Settings";
            // 
            // publicKeyLabel
            // 
            this.publicKeyLabel.AutoSize = true;
            this.publicKeyLabel.Location = new System.Drawing.Point(12, 33);
            this.publicKeyLabel.Name = "publicKeyLabel";
            this.publicKeyLabel.Size = new System.Drawing.Size(88, 13);
            this.publicKeyLabel.TabIndex = 3;
            this.publicKeyLabel.Text = "SSH_Public_Key";
            // 
            // privateKeyLabel
            // 
            this.privateKeyLabel.AutoSize = true;
            this.privateKeyLabel.Location = new System.Drawing.Point(12, 60);
            this.privateKeyLabel.Name = "privateKeyLabel";
            this.privateKeyLabel.Size = new System.Drawing.Size(92, 13);
            this.privateKeyLabel.TabIndex = 4;
            this.privateKeyLabel.Text = "SSH_Private_Key";
            // 
            // publicKeyTextBox
            // 
            this.publicKeyTextBox.Location = new System.Drawing.Point(126, 31);
            this.publicKeyTextBox.Name = "publicKeyTextBox";
            this.publicKeyTextBox.Size = new System.Drawing.Size(100, 20);
            this.publicKeyTextBox.TabIndex = 5;
            // 
            // privateKeyTextBox
            // 
            this.privateKeyTextBox.Location = new System.Drawing.Point(126, 57);
            this.privateKeyTextBox.Name = "privateKeyTextBox";
            this.privateKeyTextBox.Size = new System.Drawing.Size(100, 20);
            this.privateKeyTextBox.TabIndex = 6;
            // 
            // orgLabel
            // 
            this.orgLabel.AutoSize = true;
            this.orgLabel.Location = new System.Drawing.Point(12, 85);
            this.orgLabel.Name = "orgLabel";
            this.orgLabel.Size = new System.Drawing.Size(66, 13);
            this.orgLabel.TabIndex = 7;
            this.orgLabel.Text = "Organization";
            // 
            // projectLabel
            // 
            this.projectLabel.AutoSize = true;
            this.projectLabel.Location = new System.Drawing.Point(12, 112);
            this.projectLabel.Name = "projectLabel";
            this.projectLabel.Size = new System.Drawing.Size(40, 13);
            this.projectLabel.TabIndex = 8;
            this.projectLabel.Text = "Project";
            // 
            // AWS_Access_KEYTextBox
            // 
            this.AWS_Access_KEYTextBox.Location = new System.Drawing.Point(126, 163);
            this.AWS_Access_KEYTextBox.Name = "AWS_Access_KEYTextBox";
            this.AWS_Access_KEYTextBox.Size = new System.Drawing.Size(100, 20);
            this.AWS_Access_KEYTextBox.TabIndex = 12;
            // 
            // EC2_URLTextBox
            // 
            this.EC2_URLTextBox.Location = new System.Drawing.Point(126, 137);
            this.EC2_URLTextBox.Name = "EC2_URLTextBox";
            this.EC2_URLTextBox.Size = new System.Drawing.Size(100, 20);
            this.EC2_URLTextBox.TabIndex = 11;
            // 
            // AWS_ACCESS_KEYLabel
            // 
            this.AWS_ACCESS_KEYLabel.AutoSize = true;
            this.AWS_ACCESS_KEYLabel.Location = new System.Drawing.Point(12, 166);
            this.AWS_ACCESS_KEYLabel.Name = "AWS_ACCESS_KEYLabel";
            this.AWS_ACCESS_KEYLabel.Size = new System.Drawing.Size(107, 13);
            this.AWS_ACCESS_KEYLabel.TabIndex = 10;
            this.AWS_ACCESS_KEYLabel.Text = "AWS_ACCESS_KEY";
            // 
            // EC2_URLLabel
            // 
            this.EC2_URLLabel.AutoSize = true;
            this.EC2_URLLabel.Location = new System.Drawing.Point(12, 139);
            this.EC2_URLLabel.Name = "EC2_URLLabel";
            this.EC2_URLLabel.Size = new System.Drawing.Size(55, 13);
            this.EC2_URLLabel.TabIndex = 9;
            this.EC2_URLLabel.Text = "EC2_URL";
            // 
            // AWS_SECRET_KEYTextBox
            // 
            this.AWS_SECRET_KEYTextBox.Location = new System.Drawing.Point(126, 189);
            this.AWS_SECRET_KEYTextBox.Name = "AWS_SECRET_KEYTextBox";
            this.AWS_SECRET_KEYTextBox.Size = new System.Drawing.Size(100, 20);
            this.AWS_SECRET_KEYTextBox.TabIndex = 14;
            // 
            // AWS_SECRET_KEYLabel
            // 
            this.AWS_SECRET_KEYLabel.AutoSize = true;
            this.AWS_SECRET_KEYLabel.Location = new System.Drawing.Point(12, 192);
            this.AWS_SECRET_KEYLabel.Name = "AWS_SECRET_KEYLabel";
            this.AWS_SECRET_KEYLabel.Size = new System.Drawing.Size(108, 13);
            this.AWS_SECRET_KEYLabel.TabIndex = 13;
            this.AWS_SECRET_KEYLabel.Text = "AWS_SECRET_KEY";
            // 
            // ScratchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 255);
            this.Controls.Add(this.AWS_SECRET_KEYTextBox);
            this.Controls.Add(this.AWS_SECRET_KEYLabel);
            this.Controls.Add(this.AWS_Access_KEYTextBox);
            this.Controls.Add(this.EC2_URLTextBox);
            this.Controls.Add(this.AWS_ACCESS_KEYLabel);
            this.Controls.Add(this.EC2_URLLabel);
            this.Controls.Add(this.projectLabel);
            this.Controls.Add(this.orgLabel);
            this.Controls.Add(this.privateKeyTextBox);
            this.Controls.Add(this.publicKeyTextBox);
            this.Controls.Add(this.privateKeyLabel);
            this.Controls.Add(this.publicKeyLabel);
            this.Controls.Add(this.settingsLabel);
            this.Controls.Add(this.projectComboBox);
            this.Controls.Add(this.OrgComboBox);
            this.Name = "ScratchForm";
            this.Text = "ScratchForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox OrgComboBox;
        private System.Windows.Forms.ComboBox projectComboBox;
        private System.Windows.Forms.Label settingsLabel;
        private System.Windows.Forms.Label publicKeyLabel;
        private System.Windows.Forms.Label privateKeyLabel;
        private System.Windows.Forms.TextBox publicKeyTextBox;
        private System.Windows.Forms.TextBox privateKeyTextBox;
        private System.Windows.Forms.Label orgLabel;
        private System.Windows.Forms.Label projectLabel;
        private System.Windows.Forms.TextBox AWS_Access_KEYTextBox;
        private System.Windows.Forms.TextBox EC2_URLTextBox;
        private System.Windows.Forms.Label AWS_ACCESS_KEYLabel;
        private System.Windows.Forms.Label EC2_URLLabel;
        private System.Windows.Forms.TextBox AWS_SECRET_KEYTextBox;
        private System.Windows.Forms.Label AWS_SECRET_KEYLabel;
    }
}