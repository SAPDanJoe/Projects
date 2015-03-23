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
            this.components = new System.ComponentModel.Container();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.programBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.programBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // OrgComboBox
            // 
            this.OrgComboBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.OrgComboBox.FormattingEnabled = true;
            this.OrgComboBox.Location = new System.Drawing.Point(126, 83);
            this.OrgComboBox.Name = "OrgComboBox";
            this.OrgComboBox.Size = new System.Drawing.Size(121, 21);
            this.OrgComboBox.TabIndex = 3;
            this.OrgComboBox.Tag = "5";
            this.OrgComboBox.Leave += new System.EventHandler(this.leaveBox);
            // 
            // projectComboBox
            // 
            this.projectComboBox.FormattingEnabled = true;
            this.projectComboBox.Location = new System.Drawing.Point(126, 110);
            this.projectComboBox.Name = "projectComboBox";
            this.projectComboBox.Size = new System.Drawing.Size(121, 21);
            this.projectComboBox.TabIndex = 4;
            this.projectComboBox.Tag = "6";
            this.projectComboBox.Leave += new System.EventHandler(this.leaveBox);
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
            this.publicKeyTextBox.TabIndex = 1;
            this.publicKeyTextBox.Tag = "4";
            this.publicKeyTextBox.Leave += new System.EventHandler(this.leaveBox);
            // 
            // privateKeyTextBox
            // 
            this.privateKeyTextBox.Location = new System.Drawing.Point(126, 57);
            this.privateKeyTextBox.Name = "privateKeyTextBox";
            this.privateKeyTextBox.Size = new System.Drawing.Size(100, 20);
            this.privateKeyTextBox.TabIndex = 2;
            this.privateKeyTextBox.Tag = "4";
            this.privateKeyTextBox.Leave += new System.EventHandler(this.leaveBox);
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
            this.AWS_Access_KEYTextBox.TabIndex = 6;
            this.AWS_Access_KEYTextBox.Tag = "7";
            this.AWS_Access_KEYTextBox.Leave += new System.EventHandler(this.leaveBox);
            // 
            // EC2_URLTextBox
            // 
            this.EC2_URLTextBox.Location = new System.Drawing.Point(126, 137);
            this.EC2_URLTextBox.Name = "EC2_URLTextBox";
            this.EC2_URLTextBox.Size = new System.Drawing.Size(100, 20);
            this.EC2_URLTextBox.TabIndex = 5;
            this.EC2_URLTextBox.Tag = "7";
            this.EC2_URLTextBox.Leave += new System.EventHandler(this.leaveBox);
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
            this.AWS_SECRET_KEYTextBox.TabIndex = 7;
            this.AWS_SECRET_KEYTextBox.Tag = "7";
            this.AWS_SECRET_KEYTextBox.Leave += new System.EventHandler(this.leaveBox);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(306, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(47, 44);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Tag = "4";
            this.textBox1.Leave += new System.EventHandler(this.leaveBox);
            // 
            // programBindingSource
            // 
            this.programBindingSource.DataSource = typeof(ScratchPad.Program);
            // 
            // ScratchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 255);
            this.Controls.Add(this.groupBox1);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.programBindingSource)).EndInit();
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
        private System.Windows.Forms.BindingSource programBindingSource;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
    }
}