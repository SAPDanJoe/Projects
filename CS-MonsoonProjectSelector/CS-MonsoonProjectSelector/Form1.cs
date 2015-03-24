using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS_MonsoonProjectSelector
{
    public partial class MonsoonSettingsMainForm : Form
    {
        #region Private form resource definitions
        //this is the settings file as loaded with program entry
        private static XDocument file = XDocument.Load(Program.settings.ToString());        
        private static XElement fileSettings = file.XPathSelectElement(Program.xStructure.Access(1, "[@id = 'current']"));

        //this is a working replica of the data in the user form
        private XDocument xdoc = Program.XMLDoc();
        #endregion

        public MonsoonSettingsMainForm()
        {
            Debug.Write("***********Initial loading of form has begun!***********" + Environment.NewLine);

            InitializeComponent();  //designer code, don't modify

            //copy current settings from the file into the working doc
            xdoc.XPathSelectElement(Program.xStructure.AddNew(1)).Add(fileSettings);

            //enumerate populable fields
            Debug.Write("Getting a list of all the TextBoxes..." + Environment.NewLine);
            List<Control> myBoxes = listControls(this, new TextBox());
            Debug.Write("Adding all of the ComboBoxes..." + Environment.NewLine);
            myBoxes.AddRange(listControls(this, new ComboBox()));

            //populate the fields
            Debug.Write("Populating the boxes with data from the XML..." + Environment.NewLine);
            populate(myBoxes);
            Debug.Write("Loading default values for all the blank defaultable boxes." + Environment.NewLine);
            initializDefaults();
            //LoadDefaults();
            Debug.Write("**********Intial loading of form is completed!**********" + Environment.NewLine);
        }

        #region FormInformation

        /// <summary>
        /// Recurses through a root control and gets all child controls by Type.
        /// </summary>
        /// <param name="root">The root control from which to begin recursing. Usually "this", but could also be a groupBox, tabPane, etc.</param>
        /// <param name="type">The type of control to find.  You can put any control to match here, or use "new TextBox", for example.</param>
        /// <returns>List&lt;Control&gt;</returns>
        public List<Control> listControls(Control root, Control type)
        {
            Debug.Write("ENTER METHOD: listControls ( Control, Control )" + Environment.NewLine);
            Debug.Write("root {" + root.Name.ToString() + "}." + Environment.NewLine);
            Debug.Write("type {" + type.GetType().ToString() + "}." + Environment.NewLine);


            Debug.Write("Initializing a new empty List<Control>." + Environment.NewLine);
            List<Control> theList = new List<Control>();


            Debug.Write("Iterate through each {" + root.Controls.Count.ToString() + "} controls in  {" + root.Name + "} and add the {" + type.GetType().ToString() + "} to the List." + Environment.NewLine);
            foreach (Control control in root.Controls)
            {
                Debug.Write("Current control is {" + control.Name + "}." + Environment.NewLine);
                if (control.GetType() == type.GetType())
                {
                    Debug.Write("Adding {" + control.Name + "} to the list." + Environment.NewLine);
                    theList.Add(control);
                }
                else if (control.HasChildren)
                {
                    Debug.Write("Getting children of {" + control.Name + "}." + Environment.NewLine);
                    theList.InsertRange(theList.Count(), listControls(control, type));
                }
            }
            Debug.Write("LEAVE METHOD: listControls ( Control, Control )" + Environment.NewLine + Environment.NewLine);
            return theList;
        }

        /// <summary>
        /// Recurses through a root control and gets all child controls by Control.Tag(string).
        /// </summary>
        /// <param name="root">The root control from which to begin recursing. Usually "this", but could also be a groupBox, tabPane, etc.</param>
        /// <param name="tag">A string to which the control's tag will be matched.</param>
        /// <returns>List&lt;Control&gt;</returns>
        public List<Control> listControls(Control root, string tag)
        {
            Debug.Write("ENTER METHOD: listControls ( Control, string )" + Environment.NewLine);
            Debug.Write("root {" + root.Name.ToString() + "}." + Environment.NewLine);
            Debug.Write("tag {" + tag.GetType().ToString() + "}." + Environment.NewLine);


            Debug.Write("Initializing a new empty List<Control>." + Environment.NewLine);
            List<Control> theList = new List<Control>();

            foreach (Control control in root.Controls)
            {
                Debug.Write("Current control is {" + control.Name + "}." + Environment.NewLine);
                if (control.Tag != null && control.Tag.ToString() == tag)
                {
                    Debug.Write("Adding {" + control.Name + "} to the list." + Environment.NewLine);
                    theList.Add(control);
                }
                else if (control.HasChildren)
                {
                    Debug.Write("Getting children of {" + control.Name + "}." + Environment.NewLine);
                    theList.InsertRange(theList.Count(), listControls(control, tag));
                }
            }
            Debug.Write("LEAVE METHOD: listControls ( Control, string )" + Environment.NewLine + Environment.NewLine);
            return theList;
        }


        /// <summary>
        /// Lists all the controls that are children of the specified control.
        /// </summary>
        /// <param name="parent">The control whose child ocontrols are to be retrieved.</param>
        /// <returns>List&lt;Control&gt;</returns>
        private List<Control> childrenOf(Control parent)
        {
            Debug.Write("ENTER METHOD: childrenOf ( Control )" + Environment.NewLine);
            Debug.Write("parent {" + parent.Name.ToString() + "}." + Environment.NewLine);

            Debug.Write("Initializing a new empty List<Control>." + Environment.NewLine);
            //initialize the result list
            List<Control> result = new List<Control>();

            //get the parameter's level
            int boxLevel = int.Parse(parent.Tag.ToString());
            Debug.Write("{" + parent.Name.ToString() + "} is in level {" + boxLevel + "}." + Environment.NewLine);

            //List<Control> formTextBoxes = listControls(this, new TextBox());
            //Debug.Write("{" + parent.Name.ToString() + "} is in level {" + boxLevel + "} and has {" + formTextBoxes.Count.ToString() + "} textBox child(ren)." + Environment.NewLine);

            int childLevel = new int();

            switch (boxLevel)
            {
                case 1:
                    childLevel = 2;
                    break;
                case 3:
                    childLevel = 4;
                    break;
                case 4:
                    childLevel = 5;
                    break;
                case 5:
                    childLevel = 6;
                    break;
                case 6:
                    childLevel = 7;
                    break;
                default:
                    Debug.Write("The control {" + parent.Name + "} at level {" + boxLevel + "} does not have any children!" + Environment.NewLine);
                    break;
            }
            result = listControls(this, childLevel.ToString());
            Debug.Write("{" + parent.Name.ToString() + "} has  {" + result.Count.ToString() + "} in level {" + childLevel.ToString() + "}." + Environment.NewLine);

            Debug.Write("LEAVE METHOD: childrenOf ( Control )" + Environment.NewLine);
            return result;
        }

        #endregion

        #region DataDefinitions

        /// <summary>
        /// Defines the structure of a default value
        /// </summary>
        public class Default
        {
            public Control control { get; set; }
            public string value { get; set; }
            public System.Drawing.Color color { get; set; }
            public Default(Control cont, string val)
            {
                control = cont;
                value = val;
                color = System.Drawing.Color.Gray;
            }
        }

        /// <summary>
        /// Gets the default values for the form's text and combo boxes.
        /// </summary>
        /// <returns>List<Default> of default values.</Default></returns>
        public List<Default> listDefaults()
        {
            List<Default> defaults = new List<Default>();

            defaults.Add(new Default(ProjectNameComboBox, "Enter a project name"));
            defaults.Add(new Default(OrgComboBox, (string)Environment.GetEnvironmentVariable("USERNAME").ToLower()));
            defaults.Add(new Default(KitchentLogLevelComboBox, "Default"));
            defaults.Add(new Default(UserIDTextBox, Environment.GetEnvironmentVariable("USERNAME").ToLower()));
            defaults.Add(new Default(DevkitBinPathTextBox, "\\chef\\embedded"));
            defaults.Add(new Default(MinGWBinPathTextBox, "\\chef\\embedded\\migwin\\bin"));
            defaults.Add(new Default(ChefEmbeddedBinPathTextBox, "\\chef\\embedded\\bin"));
            defaults.Add(new Default(ChefRootPathTextBox, "\\chef"));
            defaults.Add(new Default(GitSSHPathTextBox, "C:\\Program Files (x86)\\Git\\bin\\ssh.exe"));
            defaults.Add(new Default(GEMPathTextBox, "\\Vagrant\\embedded\\gems"));
            defaults.Add(new Default(GEMSourcesTextBox, "http://moo-repo.wdf.sap.corp:8080/geminabox/" + Environment.NewLine + "http://moo-repo.wdf.sap.corp:8080/rubygemsorg/"));
            defaults.Add(new Default(EC2HomePathTextBox, "\\ec2-api-tools-1.6.9.0"));
            defaults.Add(new Default(EC2UrlTextBox, "https://ec2-us-west.api.monsoon.mo.sap.corp:443"));
            defaults.Add(new Default(VagrantEmbeddedPathTextBox, "\\Vagrant\\embedded"));
            defaults.Add(new Default(VagrantEmbeddedBinPathTextBox, "\\Vagrant\\embedded\\bin"));
            defaults.Add(new Default(puTTYgenPathTextBox, "C:\\Program Files (x86)\\PuTTY\\puttygen.exe"));
            defaults.Add(new Default(KeyIDTextBox, "Default"));
            return defaults;
        }
        
        #endregion

        #region UIDataInsertion

        /// <summary>
        /// Populates TextBoxes and ComboBoxes
        /// </summary>
        /// <param name="boxes">The list of boxes to populate</param>
        private void populate(List<Control> boxes)
        {
            Debug.Write("ENTER METHOD: populate( List<Control> )" + Environment.NewLine);
            Debug.Write("boxes {" + boxes.Count + "}." + Environment.NewLine);

            Debug.Write("Itterate through the List<Control>." + Environment.NewLine);
            foreach (var box in boxes)
            {
                Debug.Write("Unsubscribe {" + box.Name+ "}.Leave event." + Environment.NewLine);
                box.Leave -= leaveBox;

                int level = int.Parse(box.Tag.ToString());
                Debug.Write("{" + box.Name + "} is level {" + level + "}." + Environment.NewLine);
                
                string boxPath = Program.xStructure.Access(level,
                    "[@ControlName = '" + box.Name.ToString() + "']");
                Debug.Write("The box can be accessed int he XML document by the following XPath: +" + Environment.NewLine + "{" + boxPath + "}." + Environment.NewLine);

                switch (box.GetType().ToString())
                {
                    case "System.Windows.Forms.TextBox":
                        Debug.Write("This is a TextBox." + Environment.NewLine);
                        box.Text = (xdoc.XPathSelectElement(boxPath) != null) ?
                            xdoc.XPathSelectElement(boxPath).Value.ToString() :
                            string.Empty;
                        Debug.Write("The box's text was set to {" + box.Text + "}. (if empty, xdoc.XPathSelectElement({" + boxPath + "}) == null)" + Environment.NewLine);

                        //leaveBox(box, new EventArgs());
                        break;
                    case "System.Windows.Forms.ComboBox":
                        Debug.Write("This is a ComboBox." + Environment.NewLine);
                        ComboBox cBox = (ComboBox)box;
                        Debug.Write("Calling ComboBox population Method." + Environment.NewLine);
                        populate(cBox);
                        break;
                    default:
                        Debug.Write("Control Type : {" + boxes.GetType().ToString() + "} does not have a defined population method." + Environment.NewLine);
                        break;
                }

                Debug.Write("Resubscribe {" + box.Name + "}.Leave event." + Environment.NewLine);
                box.Leave += leaveBox;

                Debug.Write("LEAVE METHOD: populate( List<Control> )" + Environment.NewLine);
            }
        }

        /// <summary>
        /// Helper function that overloads the population method to handle a single ComboBox
        /// </summary>
        /// <param name="cBox">The Combobox to be populated from the xdoc</param>
        private void populate(ComboBox cBox, string value = null)
        {
            Debug.Write("ENTER METHOD: populate( Control, string )" + Environment.NewLine);
            Debug.Write("cBox {" + cBox.Name.ToString() + "}" + Environment.NewLine);

            string checkedValue = value == null ? string.Empty : value;
            Debug.Write("checkedValue {" + checkedValue + "}" + Environment.NewLine);


            //unsubscribe from the selected index changed event
            Debug.Write("Unsubscribe {" + cBox.Name + "}.SelectedIndexChanged event." + Environment.NewLine);
            cBox.SelectedIndexChanged -= leaveBox;
            
            //get the box's level
            int level = int.Parse(cBox.Tag.ToString());
            Debug.Write("{" + cBox.Name + "}is level {" + level + "}" + Environment.NewLine);

            //get the parent path for this combobox
            string parent = Program.xStructure.Access(level);
            Debug.Write("The box's XPath access string is {" + parent + "}." + Environment.NewLine);

            if (level > 3)
            {
                Debug.Write("Clear all items, and entered text from {" + cBox.Name+ "}." + Environment.NewLine);
                //Clear the ComboBox and get ready to load data into it
                cBox.Items.Clear();
                cBox.Text = null;

                if (value != null)
                {   //a single value was provided
                    Debug.Write("Adding a single static value {" + value + "} to ComboBox.Items..." + Environment.NewLine);
                    cBox.Items.Add(value);
                    cBox.SelectedIndex = cBox.Items.Count - 1;
                    Debug.Write("Set SelectedIndex to  {" + (cBox.Items.Count - 1).ToString() + "} with a value of {" + cBox.Items[cBox.Items.Count - 1].ToString() + "}" + Environment.NewLine);
                }
                else
                {
                    Debug.Write("Adding an XML value list to ComboBox.Items..." + Environment.NewLine);
                    foreach (XElement item in xdoc.XPathSelectElements(parent))
                    {
                        //add each item to the Combobox item list
                        Debug.Write("Adding a value {" + value + "} from XML to ComboBox.Items..." + Environment.NewLine);
                        cBox.Items.Add(item.Attribute("name").Value.ToString());
                        if (item.Attribute("selected") != null && item.Attribute("selected").Value.ToString() == "1")
                        {
                            cBox.SelectedIndex = cBox.Items.Count - 1;
                            Debug.Write("Set SelectedIndex to  {" + (cBox.Items.Count - 1).ToString() + "} with a value of {" + cBox.Items[cBox.Items.Count - 1].ToString() + "}" + Environment.NewLine);
                        }
                    }
                }
                if (childrenOf(cBox) != null)
                {
                    Debug.Write("This ComboBox has {" + childrenOf(cBox).Count.ToString() + "} child(ren), populate them..." + Environment.NewLine);
                    populate(childrenOf(cBox));
                }
                //leaveBox(cBox, new EventArgs());
            }
            else
            {
                cBox.SelectedValue = xdoc.XPathSelectElement(parent + "[@ControlName = '" + cBox.Name + "']") != null ? xdoc.XPathSelectElement(parent + "[@ControlName = '" + cBox.Name + "']").Attribute("name").Value.ToString() : 
                    "Default";
                Debug.Write("ComboBox {" + cBox.Name + "} is below level 4, and not eligible for modification, only set SelectedItem {" + cBox.SelectedValue + "} ." + Environment.NewLine);
            }
            //resubscribe to the selected index changed event
            Debug.Write("Resubscribe {" + cBox.Name + "}.SelectedIndexChanged event." + Environment.NewLine);
            cBox.SelectedIndexChanged += leaveBox;

            Debug.Write("LEAVE METHOD: populate( Control, string )" + Environment.NewLine);
        }

        /// <summary>
        /// Inserts the default form data into the corresponding form fields.
        /// </summary>
        private void initializDefaults()
        {
            List<Default> defaultables = listDefaults();
            foreach (Default defaultable in defaultables)
            {
                if (string.IsNullOrEmpty(defaultable.control.Text))
                {
                    defaultable.control.ForeColor = defaultable.color;

                    switch (defaultable.control.GetType().ToString())
                    {
                        case "System.Windows.Forms.TextBox":
                            defaultable.control.Text = defaultable.value;
                            //leaveBox(defaultable.control, new EventArgs());
                            break;
                        case "System.Windows.Forms.ComboBox":
                            ComboBox cBox = (ComboBox)defaultable.control;
                            populate(cBox, defaultable.value);
                            break;
                        default:
                            Debug.Write("Control Type : {" + defaultable.control.GetType().ToString() + "} does not have a defined population method." + Environment.NewLine);
                            break;
                    }
                }
            }
        }

        #endregion

        
        #region EventHandlers:FocusChange
        
        /// <summary>
        /// Event Handler that compares the content in the UI with the content in the working settings, and updates the settings if necessary.
        /// </summary>
        /// <param name="sender">Box in which the text changed.</param>
        /// <param name="e">Not currently impimented, use new EventArgs()</param>
        public void leaveBox(object sender, EventArgs e)
        {
            string uiValue = string.Empty;
            string docValue = string.Empty;

            Control dbg = (Control)sender;
            Debug.Write("Leaving a {" + dbg.GetType().ToString() + "} named {" + dbg.Name.ToString() + "} at level {" + dbg.Tag + "} ." + Environment.NewLine);

            if (sender is ComboBox)
            {
                ComboBox box = (ComboBox)sender;
                int level = int.Parse(box.Tag.ToString());
                uiValue = box.Text.ToString();
                docValue = (xdoc.XPathSelectElement(Program.xStructure.Access(level, "[@selected = '1']")) == null) ? string.Empty : xdoc.XPathSelectElement(Program.xStructure.Access(level, "[@selected = '1']")).Attribute("name").Value.ToString();
                bool existsInDoc = (xdoc.XPathSelectElement(Program.xStructure.Access(level, "[@name = '" + box.Text.ToString() + "']")) != null) ? true : false;

                if (!existsInDoc && !string.IsNullOrEmpty(uiValue))
                {   //if the uiValue in the Combobox is not yet in the document, add it
                    Debug.Write("The control's value {" + uiValue + "} is not in the document, adding..." + Environment.NewLine);

                    //store uiValue in xdoc
                    xdoc = Program.XMLDoc(xdoc, box);
                    xdoc = Program.XMLDoc(xdoc, box, box.Name.ToString(), true);
                    xdoc = Program.XMLDoc(xdoc, box, uiValue, true, "name");
                }

                if (!box.Items.Contains(uiValue) && !string.IsNullOrEmpty(uiValue))
                {   //if the uiValue is not already in the combobox's list...
                    Debug.Write("The control's value {" + uiValue + "} is not in the list, adding and selecting..." + Environment.NewLine);

                    //add new item to box list
                    box.Items.Add(box.Text);

                    //select new value in combobox
                    box.SelectedIndex = box.Items.Count - 1;
                }
                if (uiValue != docValue)
                {
                    //if the value in the UI has deviated from that in the document, fire selection changed event
                    Debug.Write("The form entered value {" + uiValue + "} does not match the selected document's value {" + docValue + "}, updating document to reflect the changes..." + Environment.NewLine);

                    //comboSelectionChanged(box, new EventArgs());
                    exchangeSelections(box);
                }
                else
                {
                    Debug.Write("{" + box.Name + "} had no changes to commit." + Environment.NewLine);
                }
                //Debug.Write("{" + box.Name + "} has {" + childrenOf(box).Count + "} children to populate..." + Environment.NewLine);
                //populate(childrenOf(box));

                if (box == ProjectNameComboBox)
                {
                    checkLink(box, ProjectSettingsLink);
                }
            }
            else if (sender is TextBox)
            {
                TextBox box = (TextBox)sender;
                int level = int.Parse(box.Tag.ToString());
                uiValue = box.Text.ToString();
                docValue = (xdoc.XPathSelectElement(Program.xStructure.Access(level,"[@ControlName = '" + box.Name.ToString() + "']")) == null) ? 
                    string.Empty : xdoc.XPathSelectElement(Program.xStructure.Access(level,"[@ControlName = '" + box.Name.ToString() + "']")).Value.ToString();

                    if (uiValue != docValue && !string.IsNullOrEmpty(uiValue))
                {
                    if (string.IsNullOrEmpty(docValue))
                    {
                        Debug.Write("The control's value is not in the document, adding..." + Environment.NewLine);
                        //store uiValue in xdoc
                        xdoc = Program.XMLDoc(xdoc, box, uiValue);
                        xdoc = Program.XMLDoc(xdoc, box, box.Name.ToString(), true);
                    }
                    else
                    {
                        Debug.Write("The control's value is not current in the document, updating..." + Environment.NewLine);
                        //store uiValue in xdoc
                        xdoc.XPathSelectElement(Program.xStructure.Access(level, "[@ControlName = '" + box.Name.ToString() + "']")).Value = uiValue;
                    }
                   
                }
                else
                {
                    Debug.Write("{" + box.Name + "} had no changes to commit." + Environment.NewLine);
                }

                //toggle link visibility and autofill
                if (box == UserIDTextBox)
                {
                    checkLink(box, MonsoonKeysLink);
                }
                else if (box == KeyIDTextBox)
                {
                    checkLink(box, ProjectSettingsLink);
                }
                else if (box == GitLastNameTextBox || box == GitFirstNameTextBox)
                {
                    autoFillEmailAddress();
                }
            }            
        }

        /// <summary>
        /// Indirect event handler called by leaveBox after the other operations are performed.  Combines the 1st and last anme into an SAP standard email address if they are non-blank.
        /// </summary>
        private void autoFillEmailAddress()
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

        /// <summary>
        /// Indirect event handler called by leaveBox.  When a new selection is made in a combobox, this ensures that the change is reflected in the acompanying XDocument.
        /// </summary>
        /// <param name="cBox">The ComboBox where the selection has been changed.</param>
        private void exchangeSelections(ComboBox cBox)
        {
            int level = int.Parse(cBox.Tag.ToString());
            //get the old and newly selected elements

            //this is the item still 'selected' in the xdoc
            XElement docSelection = xdoc.XPathSelectElement(Program.xStructure.Access(level, "[@selected = '1']"));

            //this is the item currently selected in the UI
            XElement uiSelection = xdoc.XPathSelectElement(Program.xStructure.Access(level, "[@name = '" + cBox.SelectedItem.ToString() + "']"));

            //remove the previous element's "seleted" attribute, and add it to the newly selected attribute
            if (docSelection != null)
            {
                docSelection.SetAttributeValue("selected", null);
            }
            if (uiSelection != null)
            {
                uiSelection.SetAttributeValue("selected", 1);
            }
            if (childrenOf(cBox).Count > 0)
            {
                populate(childrenOf(cBox));
            }
        }

        /// <summary>
        /// Changes the text color from Gray to Black, to indicate that a value is no longer default.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RestoreDefaultColor_Enter(object sender, EventArgs e)
        {
            Control box = (Control)sender;
            if (box.ForeColor == System.Drawing.Color.Gray)
            {                
                box.ForeColor = System.Drawing.Color.Black;
            }
        }
        #endregion

        #region EventHandlers:Clicks
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
                UserIDTextBox.Text +
                "/keys"
                );
        }

        private void ProjectSettingsLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(
                "https://monsoon.mo.sap.corp/organizations/" +
                OrgComboBox.Text +
                "/projects/" +
                ProjectNameComboBox.Text
                );
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            //save the current settings
            SaveData();

            //check if all the data is valid
            if (!dataVerified())
            {
                return;
            }

            //get a new conenction to the file and the current settings
            XElement currentConfig = XDocument.Load(Program.settings.ToString()).XPathSelectElement(Program.xStructure.Access(1, "[@id = 'current']"));

            //setup a new variable to store the scope of the configuration
            EnvironmentVariableTarget mode =
                sender.ToString().Contains("User") ?
                EnvironmentVariableTarget.User :
                EnvironmentVariableTarget.Process;

            //check to see which button called the fucntion, and set the scope accordingly
            bool commandWindow =
                sender.ToString().Contains("User") ?
                false :
                true;

            //run the setting loader
            Program.loadEnvironment(currentConfig, mode);

            //open the command prompt
            if (commandWindow)
            {
                System.Diagnostics.Process.Start("cmd");
            }
        }

        /// <summary>
        /// Called on doubleclick of a path or file input box, opend the file browser to select a folder or file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ffBrowser(object sender, EventArgs e)
        {
            //cast sender as a textbox
            TextBox tBox = (TextBox)sender;
            bool isFolder = false;

            if (!(tBox == GitSSHPathTextBox || tBox == puTTYgenPathTextBox))
            {
                FileBrowserDialog.CheckFileExists = false;
                string defaultFilename = "Select this folder";
                FileBrowserDialog.FileName = defaultFilename;
                isFolder = true;
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

                tBox.Text = isFolder ?
                    folder :
                    FileBrowserDialog.FileName;

                Debug.Write("Folder? " + isFolder.ToString() + Environment.NewLine +
                    "folder name is " + folder.ToString() + Environment.NewLine);
            }
        }

        /// <summary>
        /// Handles the buttonClick event for the open Config.xml file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openConfig(object sender, EventArgs e)
        {
            ProcessStartInfo editArgs = new ProcessStartInfo();
            editArgs.FileName = "iexplore.exe";
            editArgs.Arguments = Program.settings.ToString();
            Process.Start(editArgs);
        }  
      
        /// <summary>
        /// Calls the SaveData method with the contextualResponse option enabled. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAllButton_Click(object sender, EventArgs e)
        {
            SaveData(true);
        }
        #endregion


        #region Checks

        /// <summary>
        /// Checks to ensure that all of the boxes are filled in, and contain valid paths where applicable.
        /// </summary>
        /// <returns>bool: True if there are no invalid paths or empty boxes.</returns>
        private bool dataVerified()
        {
            //enumerate populable fields
            List<Control> boxes = listControls(this, new TextBox());
            boxes.AddRange(listControls(this, new ComboBox()));

            foreach (Control box in boxes)
            {
                if (!(box == level0TextBox || box == level1TextBox || box == level3TextBox))
                {
                    if (string.IsNullOrEmpty(box.Text))
                    {
                        MessageBox.Show(
                            box.Name.ToString() + " was empty." + Environment.NewLine +
                                "You must complete all of the settings before launching a session." + Environment.NewLine + Environment.NewLine +
                                "Please check the form and try again.",
                            "Error in " + box.Name.ToString(),
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Error);
                        return false;
                    }
                    if (box.Name.Contains("Path") || box.Name.Contains("Bin"))
                    {
                        if (System.IO.File.Exists(box.Text) || System.IO.Directory.Exists(box.Text))
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show(
                                 "The path {" + box.Text + "} could not be accessed.",
                                 "Error in " + box.Name.ToString(),
                                 System.Windows.Forms.MessageBoxButtons.OK,
                                 System.Windows.Forms.MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Checks to see a linkLabel should be invisible, as when text components of the link are not yet entered into the form.
        /// </summary>
        /// <param name="box">Control: The box that contains data that is a part of the link.</param>
        /// <param name="link">LinkLabel: The label on which visibility should be toggled.</param>
        private void checkLink(Control box, LinkLabel link)
        {
            Debug.Write("{" + box.Name + "} currently has text : {" + box.Text + "}.\n");
            if (string.IsNullOrEmpty(box.Text))
            {
                link.Visible = false;
            }
            else
            {
                link.Visible = true;
            }
        }
        
        /// <summary>
        /// Checks to see if the data on the form is different from the data in the XML Settings.
        /// </summary>
        /// <param name="xForm">XDocument representing the Form</param>
        /// <param name="xFileSettings">XElement representing the current settings in the file.</param>
        /// <returns>Returns bool</returns>
        /// 
        private bool formChanged(XDocument xForm, XElement xFileSettings)
        {
            //sanitize both for comparision by removing attribute from 'settings'
            //must store sanitization in a new element to avoid damage to the original structures

            XElement cleanedFileSettings = new XElement(xFileSettings);
            cleanedFileSettings.RemoveAttributes();
            XElement cleanedFormData = new XElement(xForm.XPathSelectElement(Program.xStructure.Access(1, "[@id = 'current']")));
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

        #endregion

       
        /// <summary>
        /// Saves the data in the form to a new XElement in the file.
        /// </summary>
        /// <param name="contextualResponse">bool: provides a message box to confirm completion or failure.</param>
        private void SaveData(bool contextualResponse = false)
        {
            //need to re-access the config file to ensure we have the most recent copy...

            XDocument latestFile = XDocument.Load(Program.settings.ToString());        
            XElement latestSettings = latestFile.XPathSelectElement(Program.xStructure.Access(1, "[@id = 'current']"));

            if (formChanged(xdoc, latestSettings))
            {
                //start by taking the file's 'current' settings and giving them an ID number instead
                //count the number of settings already in the file
                int settingID = latestFile.Root.Elements().Count() - 1;

                //modify attributes: 'current' --> last
                latestSettings.SetAttributeValue("id", settingID);
                latestSettings.Add(new XAttribute("replaced", DateTime.Now.ToString()));


                //* 2) Update the form data to be compliant with the format of the document
                //add the date attribute to the form's settings
                if ( xdoc.XPathSelectElement(Program.xStructure.Access(1, "[@id = 'current']")).Attribute("created") == null)
                {
                    xdoc.XPathSelectElement(Program.xStructure.Access(1, "[@id = 'current']")).Add(new XAttribute("created",DateTime.Now.ToString()));
                }
                else
                {
                    xdoc.XPathSelectElement(Program.xStructure.Access(1, "[@id = 'current']")).Attribute("created").Value = DateTime.Now.ToString();
                }

                //* 3) Put the form data in the file
                //add a the new 'current' element to the file                    
                latestFile.Element("configFile").Add(xdoc.XPathSelectElement(Program.xStructure.Access(1,"[@id = 'current']")));

                //* 4) save the file to disk
                //save the changes back to the document
                latestFile.Save(Program.settings.ToString());
                if (contextualResponse)
                {   
                    MessageBox.Show(
                        "The configuration was saved successfully.", 
                        "Save Complete",
                        System.Windows.Forms.MessageBoxButtons.OK ,
                        System.Windows.Forms.MessageBoxIcon.Information);
                }
            }
            else if (contextualResponse)
            {
                MessageBox.Show("There were no configuration changes to save.", 
                    "Save Skipped",
                    System.Windows.Forms.MessageBoxButtons.OK ,
                    System.Windows.Forms.MessageBoxIcon.Information);
            }
        }

    }
}
