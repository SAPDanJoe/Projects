using System;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
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
            Debug.Write("");
        }

        #region Private form variable definitions
        private XElement fileSettings = XDocument.Load(Program.settings.ToString()).XPathSelectElement("configFile/settings[@id = 'current']");
        #endregion


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
            KitchentLogLevelComboBox.ForeColor = System.Drawing.Color.Gray;
            UserIDComboBox.ForeColor = System.Drawing.Color.Gray;
        }

        private void LoadData()
        {
            //load values from file into text boxes 
            readXMLtoForm(KeyIDTextBox, fileSettings);
            readXMLtoForm(AccessKeyTextBox, fileSettings);
            readXMLtoForm(OrgTextBox, fileSettings);
            readXMLtoForm(SecretKeyTextBox, fileSettings);
            readXMLtoForm(PublicKeyTextBox, fileSettings);
            readXMLtoForm(PrivateKeyTextBox, fileSettings);
            readXMLtoForm(DevkitBinTextBox, fileSettings);
            readXMLtoForm(MinGWBinTextBox, fileSettings);
            readXMLtoForm(ChefEmbeddedBinTextBox, fileSettings);
            readXMLtoForm(ChefRootTextBox, fileSettings);
            readXMLtoForm(GitSSHTextBox, fileSettings);
            readXMLtoForm(GitEmailAddressTextBox, fileSettings);
            readXMLtoForm(GitFirstNameTextBox, fileSettings);
            readXMLtoForm(GitLastNameTextBox, fileSettings);
            readXMLtoForm(GEMPathTextBox, fileSettings);
            readXMLtoForm(GEMSourcesTextBox, fileSettings);
            readXMLtoForm(EC2HomeTextBox, fileSettings);
            readXMLtoForm(EC2UrlTextBox, fileSettings);
            readXMLtoForm(VagrantEmbeddedTextBox, fileSettings);
            readXMLtoForm(VagrantEmbeddedBinTextBox, fileSettings);
            readXMLtoForm(ProjectNameComboBox, fileSettings);
            //valueReader(KitchentLogLevelComboBox, fileSettings);  <-- This is not necessary, because adding values is not allowed.
            readXMLtoForm(UserIDComboBox, fileSettings);

            //set selected items in combo boxes
            setSelections(ProjectNameComboBox, fileSettings);
            setSelections(KitchentLogLevelComboBox, fileSettings);
            setSelections(UserIDComboBox, fileSettings);
        }

        private void SaveData(XDocument form = null)
        {
            XDocument formData = xmlFormData();

            if (formChanged(formData, fileSettings))
            {
                //connect to the file
                XDocument xFile = XDocument.Load(Program.settings.ToString());

                //connect to the the 'current' settings node in the file
                XElement currentFileSettings = xFile.XPathSelectElement("configFile/settings[@id = 'current']");

                //start by taking the file's 'current' settings and giving them an ID number instead
                //connect to the document and count the number of settings already there
                int settings = xFile.Root.Elements().Count();

                //modify attributes: 'current' --> last
                currentFileSettings.SetAttributeValue("id", settings);
                currentFileSettings.Add(new XAttribute("replaced", DateTime.Now));

                //add the date attribute to the form's settings
                formData.XPathSelectElement("settings[@id = 'current']").Add(
                    new XAttribute("created", DateTime.Now.ToString())
                    );

                //add a the new 'current' element to the file                    
                xFile.Element("configFile").Add(formData.XPathSelectElement("settings[@id = 'current']"));

                //save the changes back to the document
                xFile.Save(Program.settings.ToString());
            }
            
            //if (form == null)
            //{   //formData is null on the 1st pass.
            //    //save data from the form into an XElement

            //    //create a new document whose structure matches that
            //    //of a single setting element
            //    XDocument newForm = new XDocument(
            //        new XElement("settings",
            //            new XAttribute("id", "current")
            //            )
            //        );

            //    //select that single settings current element
            //    XElement formContents = newForm.XPathSelectElement("settings[@id = 'current']");

            //    //load the form data into the new element
            //    formContents = writeFormToXML(UserIDComboBox, formContents);
            //    formContents = writeFormToXML(PublicKeyTextBox, formContents);
            //    formContents = writeFormToXML(PrivateKeyTextBox, formContents);
            //    formContents = writeFormToXML(OrgTextBox, formContents);
            //    formContents = writeFormToXML(ProjectNameComboBox, formContents);
            //    formContents = writeFormToXML(KeyIDTextBox, formContents);
            //    formContents = writeFormToXML(AccessKeyTextBox, formContents);
            //    formContents = writeFormToXML(SecretKeyTextBox, formContents);
            //    formContents = writeFormToXML(DevkitBinTextBox, formContents);
            //    formContents = writeFormToXML(MinGWBinTextBox, formContents);
            //    formContents = writeFormToXML(ChefEmbeddedBinTextBox, formContents);
            //    formContents = writeFormToXML(ChefRootTextBox, formContents);
            //    formContents = writeFormToXML(KitchentLogLevelComboBox, formContents);
            //    formContents = writeFormToXML(GitSSHTextBox, formContents);
            //    formContents = writeFormToXML(GitEmailAddressTextBox, formContents);
            //    formContents = writeFormToXML(GitFirstNameTextBox, formContents);
            //    formContents = writeFormToXML(GitLastNameTextBox, formContents);
            //    formContents = writeFormToXML(GEMPathTextBox, formContents);
            //    formContents = writeFormToXML(GEMSourcesTextBox, formContents);
            //    formContents = writeFormToXML(EC2HomeTextBox, formContents);
            //    formContents = writeFormToXML(EC2UrlTextBox, formContents);
            //    formContents = writeFormToXML(VagrantEmbeddedTextBox, formContents);
            //    formContents = writeFormToXML(VagrantEmbeddedBinTextBox, formContents);

            //    //pass back the contents to the 2nd part of the function
            //    SaveData(newForm);

            //}
            //else
            //{   //formData is not null on the 2nd pass.
            //    //check if the form data varies from stored data

            //    //load up the XML file
            //    XDocument xFile = XDocument.Load(Program.settings.ToString());

            //    //connect to the 'current' Settings node in the file
            //    XElement currentFileSettings = xFile.XPathSelectElement("configFile/settings[@id = 'current']");

            //    //sanitize both for comparision by removing attribute from 'settings'
            //    //must store sanitization in a new element to avooid damage to the original structures

            //    XElement cleanedFileSettings = new XElement(currentFileSettings);
            //    cleanedFileSettings.RemoveAttributes();
            //    XElement cleanedFormData = new XElement(form.XPathSelectElement("settings[@id = 'current']"));
            //    cleanedFormData.RemoveAttributes();

            //    if (XNode.DeepEquals(cleanedFormData, cleanedFileSettings))
            //    {   //no variance detected, no action required
            //        //reserving this space in the condition to present debugging data if needed
            //        Debug.Write("No changes were found when comparing the form data to the stored data.  Save request ignored.");
            //    }
            //    else
            //    {   //variance detected, add current formData to file and save 

            //        Debug.Write("Changes were found when comparing the form data to the stored data.  Saving new settings to form.");

            //        //start by taking the file's 'current' settings and giving them an ID number
            //        //connect to the document and count the number of settings already there
            //        int settings = xFile.Root.Elements().Count();

            //        //modify attributes: 'current' --> last + 1
            //        currentFileSettings.SetAttributeValue("id", settings);
            //        currentFileSettings.Add(new XAttribute("replaced", DateTime.Now));

            //        //add the date attribute to the form's settings
            //        form.XPathSelectElement("settings[@id = 'current']").Add(
            //            new XAttribute("created", DateTime.Now.ToString())
            //            );

            //        //add a the new 'current' element to the file                    
            //        xFile.Element("configFile").Add(form.XPathSelectElement("settings[@id = 'current']"));

            //        //save the changes back to the document
            //        xFile.Save(Program.settings.ToString());
            //    }
//            }
        }

        private XDocument xmlFormData()
        {   
            XDocument newForm = new XDocument(
                new XElement("settings",
                    new XAttribute("id", "current")
                    )
                );

            //select that single settings current element
            XElement formContents = newForm.XPathSelectElement("settings[@id = 'current']");

            //load the form data into the new element
            formContents = writeFormToXML(UserIDComboBox, formContents);
            formContents = writeFormToXML(PublicKeyTextBox, formContents);
            formContents = writeFormToXML(PrivateKeyTextBox, formContents);
            formContents = writeFormToXML(OrgTextBox, formContents);
            formContents = writeFormToXML(ProjectNameComboBox, formContents);
            formContents = writeFormToXML(KeyIDTextBox, formContents);
            formContents = writeFormToXML(AccessKeyTextBox, formContents);
            formContents = writeFormToXML(SecretKeyTextBox, formContents);
            formContents = writeFormToXML(DevkitBinTextBox, formContents);
            formContents = writeFormToXML(MinGWBinTextBox, formContents);
            formContents = writeFormToXML(ChefEmbeddedBinTextBox, formContents);
            formContents = writeFormToXML(ChefRootTextBox, formContents);
            formContents = writeFormToXML(KitchentLogLevelComboBox, formContents);
            formContents = writeFormToXML(GitSSHTextBox, formContents);
            formContents = writeFormToXML(GitEmailAddressTextBox, formContents);
            formContents = writeFormToXML(GitFirstNameTextBox, formContents);
            formContents = writeFormToXML(GitLastNameTextBox, formContents);
            formContents = writeFormToXML(GEMPathTextBox, formContents);
            formContents = writeFormToXML(GEMSourcesTextBox, formContents);
            formContents = writeFormToXML(EC2HomeTextBox, formContents);
            formContents = writeFormToXML(EC2UrlTextBox, formContents);
            formContents = writeFormToXML(VagrantEmbeddedTextBox, formContents);
            formContents = writeFormToXML(VagrantEmbeddedBinTextBox, formContents);
            return newForm;
        }

        private bool formChanged(XDocument xForm, XElement xFileSettings)
        {
            //sanitize both for comparision by removing attribute from 'settings'
            //must store sanitization in a new element to avooid damage to the original structures

            XElement cleanedFileSettings = new XElement(xFileSettings);
            cleanedFileSettings.RemoveAttributes();
            XElement cleanedFormData = new XElement(xForm.XPathSelectElement("settings[@id = 'current']"));
            cleanedFormData.RemoveAttributes();

            if (XNode.DeepEquals(cleanedFormData, cleanedFileSettings))
            {   //no variance detected, no action required
                //reserving this space in the condition to present debugging data if needed
                Debug.Write("formChanged: No changes were found.\n");
                return false;
            }
            else
            { //variance detected, return true
                Debug.Write("formChanged: The data does not match.\n");
                return true;                
            }
        }

        private XElement writeFormToXML(TextBox tBox, XElement current)
        {
            if (tBox.Text != string.Empty)
            {   //add the box's name and value                  
                current.Add(
                    new XElement(
                        tBox.Name.ToString(),
                        tBox.Text.ToString()
                        )
                    );
            }
            return current;
        }

        private XElement writeFormToXML(ComboBox cBox, XElement current)
        {
            //add a new element in the node for the comboBox
            current.Add(new XElement(cBox.Name.ToString()), string.Empty);

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
            return current;
        }

        private void readXMLtoForm(TextBox cBox, XElement root)
        {
            string xmlValue = (string)root.Element(cBox.Name.ToString());
            if ((xmlValue != string.Empty) && (xmlValue != null))
            {
                cBox.Text = xmlValue;
                cBox.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void readXMLtoForm(ComboBox cBox, XElement root)
        {
            XElement branch = root.XPathSelectElement(cBox.Name.ToString());
            if (branch != null)
            {
                IEnumerable<XElement> ComboBoxitems =
                    from branchItems in branch.Descendants()
                    select branchItems;

                if (cBox.Items.Count > 0)
                {   //if there are already items in the combo box, clear them
                    cBox.Items.Clear();
                }

                foreach (XElement xmlData in ComboBoxitems)
                {
                    cBox.Items.Add(xmlData.Value);
                    cBox.ForeColor = System.Drawing.Color.Black;
                }
            }

        }

        private void setSelections(ComboBox cBox, XElement root)
        {
            string value = (string)root.XPathSelectElement(cBox.Name.ToString() + "/item[@selected = '1']");
            if ((value != string.Empty) && (value != null))
            {
                cBox.SelectedItem = value;
            }
        }

        #endregion


        #region UI Validation and Automation
        //The code below handles the direct form actions
        //and actions like link creation and 
        //changing, labels, Folder browsing, etc. 

        //Indivudual actions: Links
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
                "https://monsoon.mo.sap.corp/users/" +
                UserIDComboBox.Text +
                "/keys"
                );
        }

        private void ProjectSettingsLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(
                "https://monsoon.mo.sap.corp/organizations/" +
                UserIDComboBox.Text +
                "/projects/" +
                ProjectNameComboBox.Text
                );
        }

        //Individual actions: Other
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

        private void openConfig(object sender, EventArgs e)
        {
            ProcessStartInfo editArgs = new ProcessStartInfo();
            editArgs.FileName = "notepad.exe";
            editArgs.Arguments = Program.settings.ToString();
            Process.Start(editArgs);
        }


        //Reusued actions
        private void LoadButton_Click(object sender, EventArgs e)
        {
            //save the current settings
            SaveData();
            XElement currentConfig = fileSettings;

            //setup a new variable to store the scope of the configuration
            EnvironmentVariableTarget mode = new EnvironmentVariableTarget();

            //setup a var to determine if the command window should be opened
            bool commandWindow = new bool();

            //check to see which button called the fucntion, and set the scope accordingly
            if (sender.ToString().Contains("User"))
            {
                mode = EnvironmentVariableTarget.User;
                commandWindow = false;
            }
            else if (sender.ToString().Contains("Session"))
            {
                mode = EnvironmentVariableTarget.Process;
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

        private void ComboBox_Leave(object sender, EventArgs e)
        {
            //this *will* hold the code to add newly entered text into a
            //combobox list, and then select the newly entered item.
            ComboBox cBox = (ComboBox)sender;

            if (cBox.Text.ToString() != string.Empty)
            {   //There is text in this combo box

                if (!cBox.Items.Contains(cBox.Text))
                {   //The text is not in the item list, so add it
                    cBox.Items.Add(cBox.Text);
                    Debug.Write("{" + cBox.Name + "}: added {" + cBox.Text + "} to item list. \n");
                    //and select it
                    cBox.SelectedIndex = cBox.FindString(cBox.Text);
                    // cBox.SelectedItem = cBox.Items.Count - 1;
                }
            }
            else
            {
                //there is no text int he combo box, this shouldn't be an option
                Debug.Write("{" + cBox.Name + "} was empty.\n");
            }

            //when leaving the userID box, run the mooLink checker
            if (cBox.Name == "UserIDComboBox")
            {
                checkMooKeysLink(sender, e);
            }
            Debug.Write("{" + cBox.Name + "}: selected item is {" + cBox.SelectedItem + "}.\n");
            Debug.Write("{" + cBox.Name + "}: selected index is {" + cBox.SelectedIndex + "}.\n");
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

        private void checkMooKeysLink(object sender, EventArgs e)
        {
            //cast sender as combobox
            ComboBox cBox = (ComboBox)sender;

            Debug.Write("{" + cBox.Name + "} currently has text : {" + cBox.Text + "}.\n");
            if (cBox.Text == string.Empty | cBox.Text == null)
            {
                MonsoonKeysLink.Visible = false;
            }
            else
            {
                MonsoonKeysLink.Visible = true;
            }
        }

        #endregion

        #region formHelpers
        //These are all helper methods for the form
        //they handle any tasks that are not directly
        //initiated by a form action        

        private void entryToItems(ComboBox cBox, string entry)
        {
            if (cBox.Items.Contains(entry))
            {
                Debug.Write(cBox.Name + " contains {" + entry + "}\n");
                cBox.SelectedText = entry;
                Debug.Write(cBox.Name + " now has a selected index of {" + cBox.SelectedIndex + "} with a value of {" + cBox.SelectedValue + "}.\n");
            }
            else
            {
                Debug.Write(cBox.Name + " is missing {" + entry + "}\n");
                cBox.Items.Add(entry);
                entryToItems(cBox, entry);
            }
        }

        private void ffBrowser(object sender, EventArgs e)
        {
            //cast sender as a textbox
            TextBox tBox = (TextBox)sender;
            string selected = null;

            if (tBox.Tag.ToString().Equals("Folder"))
            {                
                FileBrowserDialog.CheckFileExists = false;
                string defaultFilename = "Select this folder";
                FileBrowserDialog.FileName = defaultFilename;
            }
            else
            {
                FileBrowserDialog.CheckFileExists = true;
                string defaultFilename = null;
                FileBrowserDialog.FileName = defaultFilename;
                if (tBox.Text != "")//if the text box is not empty
                {
                    //set the selected path to the text box's current contents (incase of accidental entry)
                    string test = tBox.Tag.ToString().Equals("Folder") ? "Folder" : "File";
                    MessageBox.Show("Test: " + test);
                    FileBrowserDialog.FileName = tBox.Text;
                }
            }
            if (FileBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                //Sanitize the folder selection
                //Remove everything after the last '\'
                string folder = FileBrowserDialog.FileName;
                int index = folder.LastIndexOf(@"\");
                Debug.Write("The selected folder path was: {" + folder + "}.\n");
                Debug.Write("The index of the last \\: {" + index + "}.\n");
                if (index > 0)
                    folder = folder.Substring(0, index);

                 tBox.Text = tBox.Tag.ToString().Equals("Folder") ?
                     folder :
                     FileBrowserDialog.FileName;
            }
        }

        #endregion

    }
}
