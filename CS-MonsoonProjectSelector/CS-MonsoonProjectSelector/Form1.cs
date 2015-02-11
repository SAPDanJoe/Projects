using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS_MonsoonProjectSelector
{
    public partial class MonsoonSettingsMainForm : Form
    {
        public MonsoonSettingsMainForm()
        {
            InitializeComponent();
        }
        //The code below handles the UI behaviors like link creation and changing, labels, Folder browsing, etc. 
        private void DashboardLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://monsoon.mo.sap.corp/organizations/sandbox");
        }

        private void UserIDLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://monsoon.mo.sap.corp/users/my_profile");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(
                "https://monsoon.mo.sap.corp/users/"+
                UserIDComboBox.Text+
                "/keys"
                );
        }

        private void ProjectSettingsLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(
                "https://monsoon.mo.sap.corp/organizations/"+
                UserIDComboBox.Text+
                "/projects/"+
                ProjectNameComboBox.Text
                );
        }

        private void ProjectNameComboBox_Leave(object sender, EventArgs e)
        {
            if ((UserIDComboBox.Text != "") && (ProjectNameComboBox.Text != ""))
            {
                this.ProjectSettingsLink.Visible = true;
            }
            else
            {
                this.ProjectSettingsLink.Visible = false;
            }
        }

        private void UserIDComboBox_Leave(object sender, EventArgs e)
        {
            if (UserIDComboBox.Text != "")
            {
                this.MonsoonKeysLink.Visible = true;
                if (ProjectNameComboBox.Text != "")
                {
                    this.ProjectSettingsLink.Visible = true;
                }
            }
            else
            {
                this.ProjectSettingsLink.Visible = false;
                this.MonsoonKeysLink.Visible = false;
            }
        }

        private void FolderBrowser(object sender, EventArgs e)
        {
            TextBox SenderBox = sender as TextBox;
            if (SenderBox.Text != "")//if the text box is not empty
            {
                //set the selected path to the text box's current contents (incase of accidental entry)
                FolderBrowserDialog.SelectedPath = SenderBox.Text;
            }
            if (FolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                SenderBox.Text = FolderBrowserDialog.SelectedPath;
            }
           
        }
        private void FileBrowser(object sender, EventArgs e)
        {   //basically the same as the folder browser above, but for selecting specific files
            TextBox SenderBox = sender as TextBox;
            if (SenderBox.Text != "")//if the text box is not empty
            {
                //set the selected path to the text box's current contents (incase of accidental entry)
                FileBrowserDialog.FileName = SenderBox.Text;
            }
            if (FileBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                SenderBox.Text = FileBrowserDialog.FileName;
            }
        }
    }
}
