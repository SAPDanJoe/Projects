namespace ITdirect_Processor_Toolkit
{
    partial class mainForm
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
            this.IMIS_WS_Button = new System.Windows.Forms.Button();
            this.SRIS_WS_Button = new System.Windows.Forms.Button();
            this.SRIS_MS_Button = new System.Windows.Forms.Button();
            this.IMIS_MS_Button = new System.Windows.Forms.Button();
            this.IM_Label = new System.Windows.Forms.Label();
            this.SR_Label = new System.Windows.Forms.Label();
            this.WS_Label = new System.Windows.Forms.Label();
            this.MS_Label = new System.Windows.Forms.Label();
            this.Title_Label = new System.Windows.Forms.Label();
            this.AcademyButton = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // IMIS_WS_Button
            // 
            this.IMIS_WS_Button.Location = new System.Drawing.Point(89, 61);
            this.IMIS_WS_Button.Name = "IMIS_WS_Button";
            this.IMIS_WS_Button.Size = new System.Drawing.Size(156, 23);
            this.IMIS_WS_Button.TabIndex = 0;
            this.IMIS_WS_Button.Text = "IMIS_OTHER_WIN_CTFWS";
            this.IMIS_WS_Button.UseVisualStyleBackColor = true;
            this.IMIS_WS_Button.Click += new System.EventHandler(this.buttonHandler);
            // 
            // SRIS_WS_Button
            // 
            this.SRIS_WS_Button.Location = new System.Drawing.Point(89, 90);
            this.SRIS_WS_Button.Name = "SRIS_WS_Button";
            this.SRIS_WS_Button.Size = new System.Drawing.Size(156, 23);
            this.SRIS_WS_Button.TabIndex = 1;
            this.SRIS_WS_Button.Text = "SRIS_OTHER_WIN_CTFWS";
            this.SRIS_WS_Button.UseVisualStyleBackColor = true;
            this.SRIS_WS_Button.Click += new System.EventHandler(this.buttonHandler);
            // 
            // SRIS_MS_Button
            // 
            this.SRIS_MS_Button.Location = new System.Drawing.Point(251, 90);
            this.SRIS_MS_Button.Name = "SRIS_MS_Button";
            this.SRIS_MS_Button.Size = new System.Drawing.Size(156, 23);
            this.SRIS_MS_Button.TabIndex = 2;
            this.SRIS_MS_Button.Text = "SRIS_OTHER_WIN_CTFMS";
            this.SRIS_MS_Button.UseVisualStyleBackColor = true;
            this.SRIS_MS_Button.Click += new System.EventHandler(this.buttonHandler);
            // 
            // IMIS_MS_Button
            // 
            this.IMIS_MS_Button.Location = new System.Drawing.Point(251, 61);
            this.IMIS_MS_Button.Name = "IMIS_MS_Button";
            this.IMIS_MS_Button.Size = new System.Drawing.Size(156, 23);
            this.IMIS_MS_Button.TabIndex = 3;
            this.IMIS_MS_Button.Text = "IMIS_OTHER_WIN_CTFMS";
            this.IMIS_MS_Button.UseVisualStyleBackColor = true;
            this.IMIS_MS_Button.Click += new System.EventHandler(this.buttonHandler);
            // 
            // IM_Label
            // 
            this.IM_Label.AutoSize = true;
            this.IM_Label.Location = new System.Drawing.Point(12, 66);
            this.IM_Label.Name = "IM_Label";
            this.IM_Label.Size = new System.Drawing.Size(71, 13);
            this.IM_Label.TabIndex = 4;
            this.IM_Label.Text = "Incidents - IM";
            // 
            // SR_Label
            // 
            this.SR_Label.AutoSize = true;
            this.SR_Label.Location = new System.Drawing.Point(11, 95);
            this.SR_Label.Name = "SR_Label";
            this.SR_Label.Size = new System.Drawing.Size(72, 13);
            this.SR_Label.TabIndex = 5;
            this.SR_Label.Text = "Services - SR";
            // 
            // WS_Label
            // 
            this.WS_Label.AutoSize = true;
            this.WS_Label.Location = new System.Drawing.Point(131, 45);
            this.WS_Label.Name = "WS_Label";
            this.WS_Label.Size = new System.Drawing.Size(79, 13);
            this.WS_Label.TabIndex = 6;
            this.WS_Label.Text = "Computer - WS";
            // 
            // MS_Label
            // 
            this.MS_Label.AutoSize = true;
            this.MS_Label.Location = new System.Drawing.Point(298, 45);
            this.MS_Label.Name = "MS_Label";
            this.MS_Label.Size = new System.Drawing.Size(63, 13);
            this.MS_Label.TabIndex = 7;
            this.MS_Label.Text = "Mobile - MS";
            // 
            // Title_Label
            // 
            this.Title_Label.AutoSize = true;
            this.Title_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title_Label.Location = new System.Drawing.Point(12, 9);
            this.Title_Label.Name = "Title_Label";
            this.Title_Label.Size = new System.Drawing.Size(388, 20);
            this.Title_Label.TabIndex = 8;
            this.Title_Label.Text = "Click a button below to open a new [Walk-In] ticket.";
            this.Title_Label.UseMnemonic = false;
            // 
            // AcademyButton
            // 
            this.AcademyButton.AutoSize = true;
            this.AcademyButton.Location = new System.Drawing.Point(13, 33);
            this.AcademyButton.Name = "AcademyButton";
            this.AcademyButton.Size = new System.Drawing.Size(109, 17);
            this.AcademyButton.TabIndex = 9;
            this.AcademyButton.TabStop = true;
            this.AcademyButton.Text = "Academy Support";
            this.AcademyButton.UseVisualStyleBackColor = true;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 125);
            this.Controls.Add(this.AcademyButton);
            this.Controls.Add(this.Title_Label);
            this.Controls.Add(this.MS_Label);
            this.Controls.Add(this.WS_Label);
            this.Controls.Add(this.SR_Label);
            this.Controls.Add(this.IM_Label);
            this.Controls.Add(this.IMIS_MS_Button);
            this.Controls.Add(this.SRIS_MS_Button);
            this.Controls.Add(this.SRIS_WS_Button);
            this.Controls.Add(this.IMIS_WS_Button);
            this.Name = "mainForm";
            this.Text = "ITdirect Processor Toolbox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button IMIS_WS_Button;
        private System.Windows.Forms.Button SRIS_WS_Button;
        private System.Windows.Forms.Button SRIS_MS_Button;
        private System.Windows.Forms.Button IMIS_MS_Button;
        private System.Windows.Forms.Label IM_Label;
        private System.Windows.Forms.Label SR_Label;
        private System.Windows.Forms.Label WS_Label;
        private System.Windows.Forms.Label MS_Label;
        private System.Windows.Forms.Label Title_Label;
        private System.Windows.Forms.RadioButton AcademyButton;
    }
}

