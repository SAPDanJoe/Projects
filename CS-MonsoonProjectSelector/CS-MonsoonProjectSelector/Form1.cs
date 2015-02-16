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
            InitializeComponent();
            LoadData();
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
            //valueReader(KitchentLogLevelComboBox, current);  <-- This is not necessary, because add values is not allowed.
            valueReader(UserIDComboBox, current);
        
            //set selected items in combo boxes
            setSelections(ProjectNameComboBox, current);
            setSelections(KitchentLogLevelComboBox, current);
            setSelections(UserIDComboBox, current);
        }

        private void SaveData() 
        {
            //check if anything has changed

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
                if (cBox.SelectedItem.ToString() == item.ToString())
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

        private void valueReader(System.Windows.Forms.ComboBox cBox, XElement root)
        {            
            XElement branch = root.XPathSelectElement(cBox.Name.ToString());
            
            IEnumerable<XElement> ComboBoxitems =
                from branchItems in branch.Descendants()
                select branchItems;
            
            foreach (XElement xmlData in ComboBoxitems)
            {
                cBox.Items.Add(xmlData.Value);
            }
        }

        private void valueReader(System.Windows.Forms.TextBox cBox, XElement root)
        {
            cBox.Text = (string)root.Element(cBox.Name.ToString());
        }                

        private void setSelections(System.Windows.Forms.ComboBox cBox, XElement root) 
        {
            string value = (string)root.XPathSelectElement(cBox.Name.ToString() + "/item[@selected = '1']");
            cBox.SelectedItem = value;
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
    }
}
