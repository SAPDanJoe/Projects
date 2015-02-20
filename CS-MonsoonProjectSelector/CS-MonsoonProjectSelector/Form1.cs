using System;
using System.Xml.Linq;
using System.Xml.XPath;
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
            InitializeComponent();  //designer code, don't modify
            LoadDefaults();
            LoadData();
        }
        public bool formChanged = false;

        #region UI data interactions
        // This code handles loading data to the form
        // fields, and saving data from them

        private void LoadDefaults() 
        {
            KeyIDTextBox.Text = "Default";
            OrgTextBox.Text = Environment.GetEnvironmentVariable("USERNAME").ToLower();
            DevkitBinTextBox.Text = "\\chef\\embedded";
            MinGWBinTextBox.Text = "\\chef\\embedded\\migwin\\bin";
            ChefEmbeddedBinTextBox.Text = "\\chef\\embedded\\bin";
            ChefRootTextBox.Text = "\\chef";
            GitSSHTextBox.Text = "C:\\Program Files (x86)\\Git\\bin\\ssh.exe";
            GEMPathTextBox.Text = "\\Vagrant\\embedded\\gems";
            GEMSourcesTextBox.Text = "http://moo-repo.wdf.sap.corp:8080/geminabox/" + Environment.NewLine + "http://moo-repo.wdf.sap.corp:8080/rubygemsorg/";
            EC2HomeTextBox.Text = "\\ec2-api-tools-1.6.9.0";
            EC2UrlTextBox.Text = "https://ec2-us-west.api.monsoon.mo.sap.corp:443";
            VagrantEmbeddedTextBox.Text = "\\Vagrant\\embedded";
            VagrantEmbeddedBinTextBox.Text = "\\Vagrant\\embedded\\bin";
            ProjectNameComboBox.Text = "";
            KitchentLogLevelComboBox.Text = "Default";
            UserIDComboBox.Items.Add(Environment.GetEnvironmentVariable("USERNAME").ToLower());
            UserIDComboBox.SelectedItem = (string)Environment.GetEnvironmentVariable("USERNAME").ToLower();
            
            //set all of these controls to gray to indicate default values
            KeyIDTextBox.ForeColor = System.Drawing.Color.Gray;
            OrgTextBox.ForeColor = System.Drawing.Color.Gray;
            DevkitBinTextBox.ForeColor = System.Drawing.Color.Gray;
            MinGWBinTextBox.ForeColor = System.Drawing.Color.Gray;
            ChefEmbeddedBinTextBox.ForeColor = System.Drawing.Color.Gray;
            ChefRootTextBox.ForeColor = System.Drawing.Color.Gray;
            GitSSHTextBox.ForeColor = System.Drawing.Color.Gray;
            GEMPathTextBox.ForeColor = System.Drawing.Color.Gray;
            GEMSourcesTextBox.ForeColor = System.Drawing.Color.Gray;
            EC2HomeTextBox.ForeColor = System.Drawing.Color.Gray;
            EC2UrlTextBox.ForeColor = System.Drawing.Color.Gray;
            VagrantEmbeddedTextBox.ForeColor = System.Drawing.Color.Gray;
            VagrantEmbeddedBinTextBox.ForeColor = System.Drawing.Color.Gray;
            ProjectNameComboBox.ForeColor = System.Drawing.Color.Gray;
            KitchentLogLevelComboBox.ForeColor = System.Drawing.Color.Gray;
            UserIDComboBox.ForeColor = System.Drawing.Color.Gray;
        }

        private void LoadData() 
        {
            XDocument xconfig = XDocument.Load(Program.settings.ToString());
            XElement current = xconfig.XPathSelectElement("configFile/settings[@id = 'current']");

            //load values into text boxes 
            valueReader(KeyIDTextBox, current);
            valueReader(AccessKeyTextBox, current);
            valueReader(OrgTextBox, current);
            valueReader(SecretKeyTextBox, current);
            valueReader(PublicKeyTextBox, current);
            valueReader(PrivateKeyTextBox, current);
            valueReader(DevkitBinTextBox, current);
            valueReader(MinGWBinTextBox, current);
            valueReader(ChefEmbeddedBinTextBox, current);
            valueReader(ChefRootTextBox, current);
            valueReader(GitSSHTextBox, current);
            valueReader(GitEmailAddressTextBox, current);
            valueReader(GitFirstNameTextBox, current);
            valueReader(GitLastNameTextBox, current);
            valueReader(GEMPathTextBox, current);
            valueReader(GEMSourcesTextBox, current);
            valueReader(EC2HomeTextBox, current);
            valueReader(EC2UrlTextBox, current);
            valueReader(VagrantEmbeddedTextBox, current);
            valueReader(VagrantEmbeddedBinTextBox, current);         
            valueReader(ProjectNameComboBox, current);
            //valueReader(KitchentLogLevelComboBox, current);  <-- This is not necessary, because adding values is not allowed.
            valueReader(UserIDComboBox, current);
        
            //set selected items in combo boxes
            setSelections(ProjectNameComboBox, current);
            setSelections(KitchentLogLevelComboBox, current);
            setSelections(UserIDComboBox, current);
        }

        private void SaveData() 
        {
            //check if anything has changed
            if (formChanged)
            {
                //load the XML settings file into an xDoc object
                XDocument xconfig = XDocument.Load(Program.settings.ToString());

                //make the currect node no lunger current
                XElement current = xconfig.XPathSelectElement("configFile/settings[@id = 'current']");
                int settings = xconfig.Root.Elements().Count();
                current.SetAttributeValue("id", settings + 2);
                current.Add(new XAttribute("replaced", DateTime.Now));

                //create a new 'current' node
                xconfig.Element("configFile").Add(
                    new XElement("settings",
                        new XAttribute("id", "current"),
                        new XAttribute("created",DateTime.Now.ToString())
                        )
                    );     
            
                //write UI values to the document
                xconfig = valueWriter(UserIDComboBox, xconfig);
                    xconfig = valueWriter(PublicKeyTextBox, xconfig);
                    xconfig = valueWriter(PrivateKeyTextBox, xconfig);
                    xconfig = valueWriter(OrgTextBox, xconfig);
                xconfig = valueWriter(ProjectNameComboBox, xconfig);
                    xconfig = valueWriter(KeyIDTextBox, xconfig);
                    xconfig = valueWriter(AccessKeyTextBox, xconfig);
                    xconfig = valueWriter(SecretKeyTextBox, xconfig);
                xconfig = valueWriter(DevkitBinTextBox, xconfig);
                xconfig = valueWriter(MinGWBinTextBox, xconfig);
                xconfig = valueWriter(ChefEmbeddedBinTextBox, xconfig);
                xconfig = valueWriter(ChefRootTextBox, xconfig);
                xconfig = valueWriter(KitchentLogLevelComboBox, xconfig);
                xconfig = valueWriter(GitSSHTextBox, xconfig);
                xconfig = valueWriter(GitEmailAddressTextBox, xconfig);
                xconfig = valueWriter(GitFirstNameTextBox, xconfig);
                xconfig = valueWriter(GitLastNameTextBox, xconfig);
                xconfig = valueWriter(GEMPathTextBox, xconfig);
                xconfig = valueWriter(GEMSourcesTextBox, xconfig);
                xconfig = valueWriter(EC2HomeTextBox, xconfig);
                xconfig = valueWriter(EC2UrlTextBox, xconfig);
                xconfig = valueWriter(VagrantEmbeddedTextBox, xconfig);
                xconfig = valueWriter(VagrantEmbeddedBinTextBox, xconfig);

                //save the xDoc back to the file
                xconfig.Save(Program.settings.ToString());               
            }
            formChanged = false;
        }

        private XDocument valueWriter(System.Windows.Forms.TextBox tBox, XDocument doc)
        {
            if (tBox.Text != string.Empty)
            {
                XElement parent = doc.XPathSelectElement("configFile/settings[@id = 'current']");
                //add the box's name and value as an element to the doc                    
                parent.Add(
                    new XElement(
                        tBox.Name.ToString(),
                        tBox.Text.ToString()
                        )
                    );
            }
            return doc;
        }

        private XDocument valueWriter(System.Windows.Forms.ComboBox cBox, XDocument doc)
        {
            //select the current node
            XElement current = doc.XPathSelectElement("configFile/settings[@id = 'current']");
            
            //add a new element in the node for the comboBox
            current.Add(new XElement(cBox.Name.ToString()),string.Empty);
            
            //select the newly added element
            XElement cBoxElement = current.Element(cBox.Name.ToString());

            foreach (var item in cBox.Items)
	        {
                if ((string)cBox.SelectedItem == item.ToString())
                { //this is the currently selected item in the form

                    cBoxElement.Add(
                        new XElement("item", item.ToString(),
                        new XAttribute("selected", "1")));
                }
                else
                {
                    cBoxElement.Add(new XElement("item", item.ToString()));
                }
	        }
            return doc;
        }

        private void valueReader(System.Windows.Forms.TextBox cBox, XElement root)
        {
            string xmlValue = (string)root.Element(cBox.Name.ToString());
            if ((xmlValue != string.Empty) && (xmlValue != null))
            {
                cBox.Text = xmlValue;
                cBox.ForeColor = System.Drawing.Color.Black;
            }
        }                

        private void valueReader(System.Windows.Forms.ComboBox cBox, XElement root)
        {            
            XElement branch = root.XPathSelectElement(cBox.Name.ToString());
            if (branch != null)
            {
                IEnumerable<XElement> ComboBoxitems =
                from branchItems in branch.Descendants()
                select branchItems;
            
                foreach (XElement xmlData in ComboBoxitems)
                {
                    cBox.Items.Add(xmlData.Value);
                }  
            }
            
        }

        private void setSelections(System.Windows.Forms.ComboBox cBox, XElement root) 
        {            
            string value = (string)root.XPathSelectElement(cBox.Name.ToString() + "/item[@selected = '1']");
            if ((value != string.Empty) && (value != null))
            {
                cBox.SelectedItem = value;                
            }
        }

        #endregion

        #region UI Validation and Automation
        //The code below handles the UI behaviors
        //and actions like link creation and 
        //changing, labels, Folder browsing, etc. 
        private void DashboardLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://monsoon.mo.sap.corp/organizations/sandbox");
        }

        private void UserIDLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://monsoon.mo.sap.corp/users/my_profile");
        }

        private void MonsoonKeysLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
            if (ProjectNameComboBox.Text.ToString() != string.Empty)
            {   //There is text in this combo box

                if (!this.ProjectNameComboBox.Items.Contains(this.ProjectNameComboBox.Text))
                {   //The text is not in the item list, so add it
		            this.ProjectNameComboBox.Items.Add(ProjectNameComboBox.Text);
                    //and select it
                    this.ProjectNameComboBox.SelectedItem = ProjectNameComboBox.Items.Count - 1;
	            }

                if ((UserIDComboBox.Text.ToString() != string.Empty))
                {   //The user ID combo box is populated
                    //show the settings link
                    this.ProjectSettingsLink.Visible = true;
                }

                else
                {   //The user ID combo box is not populated
                    //ensure the settings link is hidden
                    this.ProjectSettingsLink.Visible = false;
                } 
            }
            
        }

        private void UserIDComboBox_Leave(object sender, EventArgs e)
        {
            if (UserIDComboBox.Text != string.Empty)
            {   //There is text in this combo box
                if (!UserIDComboBox.Items.Contains(UserIDComboBox.Text))
                {   //The text is not in the item list, so add it
                    UserIDComboBox.Items.Add(UserIDComboBox.Text);
                }

                this.MonsoonKeysLink.Visible = true;
                if (ProjectNameComboBox.Text != "")
                {   //The project settings combo box is populated
                    //show the settings and monsoon links
                    this.ProjectSettingsLink.Visible = true;
                }
                else
                {   //The project settings combo box is not populated
                    //ensure the settings and monsoon links are hidden
                    this.ProjectSettingsLink.Visible = false;
                    this.MonsoonKeysLink.Visible = false;
                }
            }
        
        }

        private void FolderBrowser(object sender, EventArgs e)
        {

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

        private void SaveAllButton_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void GitLastNameTextBox_Leave(object sender, EventArgs e)
        {
            if (GitFirstNameTextBox.Text != string.Empty && GitLastNameTextBox.Text != string.Empty)
            {
                GitEmailAddressTextBox.Text =
                    GitFirstNameTextBox.Text +
                    "." +
                    GitLastNameTextBox.Text +
                    "@sap.com";
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            //save the current settings
            SaveData();

            //reload the file into a new document
            XDocument document = XDocument.Load(Program.settings.ToString());

            //load the current settings that we just saved as the working node
            XElement currentConfig = document.XPathSelectElement("configFile/settings[@id = 'current']");

            //setup a new variable to store the scope of the configuration
            EnvironmentVariableTarget mode = new EnvironmentVariableTarget();

            //setup a var to determin if the command window should be opened
            bool commandWindow = new bool();

            //check to see which button called the fucntion, and set the scope accordingly
            if (sender.ToString().Contains("User"))
            {
                mode  = EnvironmentVariableTarget.User;
                commandWindow = false;
            }
            else if (sender.ToString().Contains("Session"))
            {                
                mode  = EnvironmentVariableTarget.Process;
                commandWindow = true;
            }

            //run the setting loader
            Program.loadSettings(currentConfig, mode);

            //open the command prompt
            if (commandWindow)
            {
                System.Diagnostics.Process.Start("cmd");
            }
        }

        private void RestoreDefaultColor_Enter(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox box = (TextBox)sender;
                box.ForeColor = System.Drawing.Color.Black;
            }
            else if (sender is ComboBox)
            {
                ComboBox box = (ComboBox)sender;
                box.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void FormFeild_TextChanged(object sender, EventArgs e)
        {
            XElement current = XDocument.Load(Program.settings.ToString()).XPathSelectElement("configFile/settings[@id = 'current']");
            string boxName = string.Empty;
            string astring = string.Empty;
            if (sender is TextBox)
            {
                TextBox formBox = (TextBox)sender;
                boxName = formBox.Name;
                XElement xBox = current.XPathSelectElement(formBox.Name.ToString());
                astring = "formText | " + formBox.Text.ToString() +
                    " vs. xText | " + (string)xBox;
                if (formBox.Text.ToString() != (string)xBox)
                {
                    formChanged = true;
                    formBox.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    formChanged = false;                  
                }
            }
            else if (sender is ComboBox)
            {
                ComboBox formBox = (ComboBox)sender;
                boxName = formBox.Name;
                System.Timers.Timer wait = new System.Timers.Timer(5000);
                wait.Start();
                MessageBox.Show("done waiting");
                if (!formBox.Items.Contains(formBox.Text))
                {
                    formBox.Items.Add(formBox.Text);
                }

                string storedvalue = (string)current.XPathSelectElement(formBox.Name.ToString() + "/item[@selected = '1']");
                
               if ( ((string)formBox.SelectedItem != storedvalue) | ((string)formBox.Text != storedvalue) )   //trouble getting the logic right on this... how to definitively tell if the text change
                {
                    formChanged = true;
                    formBox.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    formChanged = false;
                }
            }

            label1.Text = "Form changed status: " + formChanged +
                Environment.NewLine + "Last changed item: " + boxName +
                Environment.NewLine + "Data: " + astring + "\r\n" +
                "UserIDComboBox.SelectedItem: " + UserIDComboBox.SelectedItem + "\r\n" +
                "UserIDComboBox.SelectedText: " + UserIDComboBox.SelectedText + "\r\n" +
                "UserIDComboBox.SelectedValue: " + UserIDComboBox.SelectedValue + "\r\n" +
                "UserIDComboBox.Text: " + UserIDComboBox.Text + "\r\n";
        }
        
        #endregion

        private void UserIDComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            MessageBox.Show("Selection chenge commited! \r\n" +
                "UserIDComboBox.SelectedItem: " + UserIDComboBox.SelectedItem + "\r\n" +
                "UserIDComboBox.SelectedText: " + UserIDComboBox.SelectedText + "\r\n" +
                "UserIDComboBox.SelectedValue: " + UserIDComboBox.SelectedValue + "\r\n" +
                "UserIDComboBox.Text: " + UserIDComboBox.Text + "\r\n");
        }
    }
}
