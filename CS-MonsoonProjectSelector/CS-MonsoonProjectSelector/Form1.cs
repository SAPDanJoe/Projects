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
            KeyIDTextBox.Text = (string)current.Element("KeyIDTextBox");
            AccessKeyTextBox.Text = (string)current.Element("AccessKeyTextBox");
            OrgTextBox.Text = (string)current.Element("OrgTextBox");
            SecretKeyTextBox.Text = (string)current.Element("SecretKeyTextBox");
            PublicKeyTextBox.Text = (string)current.Element("PublicKeyTextBox");
            PrivateKeyTextBox.Text = (string)current.Element("PrivateKeyTextBox");
            DevkitBinTextBox.Text = (string)current.Element("DevkitBinTextBox");
            MinGWBinTextBox.Text = (string)current.Element("MinGWBinTextBox");
            ChefEmbeddedBinTextBox.Text = (string)current.Element("ChefEmbeddedBinTextBox");
            ChefRootTextBox.Text = (string)current.Element("ChefRootTextBox");
            GitSSHTextBox.Text = (string)current.Element("GitSSHTextBox");
            GitEmailAddressTextBox.Text = (string)current.Element("GvalueailAddressTextBox");
            GitFirstNameTextBox.Text = (string)current.Element("GitFirstNameTextBox");
            GitLastNameTextBox.Text = (string)current.Element("GitLastNameTextBox");
            GEMPathTextBox.Text = (string)current.Element("GEMPathTextBox");
            GEMSourcesTextBox.Text = (string)current.Element("GEMSourcesTextBox");
            EC2HomeTextBox.Text = (string)current.Element("EC2HomeTextBox");
            EC2UrlTextBox.Text = (string)current.Element("EC2UrlTextBox");
            VagrantEmbeddedTextBox.Text = (string)current.Element("VagrantEmbeddedTextBox");
            VagrantEmbeddedBinTextBox.Text = (string)current.Element("VagrantEmbeddedBinTextBox");
            
            //loading values into combo boxes must be done differently
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //Also need to ADD CODE for getting and setting the selected item inthe combo Box
            //This can be refactored~
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            //load item values from the XML into collections
            var ProjectNameComboBoxitems = from xmlData in current.Descendants("ProjectNameComboBox")
                        select new 
                        {
                            value = (string)xmlData
                        };
            var KitchentLogLevelComboBoxitems = from xmlData in current.Descendants("KitchentLogLevelComboBox")
                        select new 
                        {
                            value = (string)xmlData
                        };
            var UserIDComboBoxitems = from xmlData in current.Descendants("UserIDComboBox")
                        select new
                        {
                            value = (string)xmlData
                        };
            
            //itterate the collections and add items
            int i = 0;
            foreach (var item in ProjectNameComboBoxitems)
            {
                ProjectNameComboBox.Items[i] = item;
                i++;
            }

            i = 0;
            foreach (var item in KitchentLogLevelComboBoxitems)
            {
                KitchentLogLevelComboBox.Items[i] = item;
                i++;
            }

            i = 0;
            foreach (var item in UserIDComboBoxitems)
            {
                UserIDComboBox.Items[i] = item;
                i++;
            }

            //set selected items
            ProjectNameComboBox.SelectedText = (string)current.XPathSelectElement("ProjectNameComboBox[@selected = '1']");
            KitchentLogLevelComboBox.SelectedText = (string)current.XPathSelectElement("KitchentLogLevelComboBox[@selected = '1']");
            UserIDComboBox.SelectedText = (string)current.XPathSelectElement("UserIDComboBox[@selected = '1']");
            
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
            xconfig.Add(
                new XElement("settings",
                    new XAttribute("id", "current"),
                    new XAttribute("created",DateTime.Now)
                    )
                );
            
            //write UI values to the document
            xconfig = valueWriter(KeyIDTextBox, xconfig);
            xconfig = valueWriter(AccessKeyTextBox, xconfig);
            xconfig = valueWriter(OrgTextBox, xconfig);
            xconfig = valueWriter(ProjectNameComboBox, xconfig);
            xconfig = valueWriter(SecretKeyTextBox, xconfig);
            xconfig = valueWriter(PublicKeyTextBox, xconfig);
            xconfig = valueWriter(PrivateKeyTextBox, xconfig);
            xconfig = valueWriter(UserIDComboBox, xconfig);
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
            //select the current node
            XElement current = doc.XPathSelectElement("configFile/settings[@id = 'current']");
            
            //add the box's name and value as an element to the doc
            current.Add(
                new XElement(tBox.Name.ToString(),tBox.Text.ToString()
                    )
                );
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
            cBoxElement.Value = "test";
            foreach (var item in cBox.Items)
	        {
                cBoxElement.Value = item.ToString();

                if (cBox.SelectedText.ToString() == item.ToString())
                { //this is the currently selected item in the form
                    cBoxElement.Add(new XAttribute("selected", "1"));
                }
	        }
            return doc;
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
