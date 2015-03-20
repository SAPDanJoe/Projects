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
        
            //LoadData();
        }


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

        #region Save entries to XML

        /// <summary>
        /// Main event handler for the TextChanged events of the Text and Combo boxes
        /// </summary>
        /// <param name="sender">Box in which the text changed.</param>
        /// <param name="e">Not currently impimented, use new EventArgs()</param>
        public void leaveBox(object sender, EventArgs e)
        {
            string uiValue = string.Empty;
            string docValue = string.Empty;

            var dbg = (Control)sender;
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
                    Debug.Write("The control's value is not in the document, adding..." + Environment.NewLine);
                    //store uiValue in xdoc
                    xdoc = Program.XMLDoc(xdoc, box, uiValue);
                    xdoc = Program.XMLDoc(xdoc, box, box.Name.ToString(), true);
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
                    GitLastNameTextBox_Leave();
                }
            }            
        }

        #endregion

        #region Populate Boxes from xdoc

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

                        leaveBox(box, new EventArgs());
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
        /// When a new selection is made in a combobox, this ensures that the change is reflected in the acompanying XDocument
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

        #endregion

        #region UI data interactions
        // This code handles loading data to the form
        // fields, and saving data from them

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

        public List<Default> listDefaults()
        {
            List<Default> defaults = new List<Default>();

            defaults.Add(new Default(UserIDTextBox, Environment.GetEnvironmentVariable("USERNAME").ToLower()));
            defaults.Add(new Default(OrgComboBox, (string)Environment.GetEnvironmentVariable("USERNAME").ToLower()));
            defaults.Add(new Default(DevkitBinTextBox, "\\chef\\embedded"));
            defaults.Add(new Default(MinGWBinTextBox, "\\chef\\embedded\\migwin\\bin"));
            defaults.Add(new Default(ChefEmbeddedBinTextBox, "\\chef\\embedded\\bin"));
            defaults.Add(new Default(ChefRootTextBox, "\\chef"));
            defaults.Add(new Default(GitSSHTextBox, "C:\\Program Files (x86)\\Git\\bin\\ssh.exe"));
            defaults.Add(new Default(GEMPathTextBox, "\\Vagrant\\embedded\\gems"));
            defaults.Add(new Default(GEMSourcesTextBox, "http://moo-repo.wdf.sap.corp:8080/geminabox/" + Environment.NewLine + "http://moo-repo.wdf.sap.corp:8080/rubygemsorg/"));
            defaults.Add(new Default(EC2HomeTextBox, "\\ec2-api-tools-1.6.9.0"));
            defaults.Add(new Default(EC2UrlTextBox, "https://ec2-us-west.api.monsoon.mo.sap.corp:443"));
            defaults.Add(new Default(VagrantEmbeddedTextBox, "\\Vagrant\\embedded"));
            defaults.Add(new Default(VagrantEmbeddedBinTextBox, "\\Vagrant\\embedded\\bin"));
            defaults.Add(new Default(KitchentLogLevelComboBox, "Default"));
            defaults.Add(new Default(puTTYgenTextBox, "C:\\Program Files (x86)\\PuTTY\\puttygen.exe"));
            defaults.Add(new Default(KeyIDTextBox, "Default"));
            defaults.Add(new Default(ProjectNameComboBox, "Enter a project name"));
            return defaults;
        }

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

        //private void LoadData()
        //{
        //    //load values from file into text boxes 
        //    readXMLtoForm(UserIDTextBox, fileSettings);
        //    readXMLtoForm(AccessKeyTextBox, fileSettings);
        //    readXMLtoForm(OrgComboBox, fileSettings);
        //    readXMLtoForm(SecretKeyTextBox, fileSettings);
        //    readXMLtoForm(PublicKeyTextBox, fileSettings);
        //    readXMLtoForm(PrivateKeyTextBox, fileSettings);
        //    readXMLtoForm(DevkitBinTextBox, fileSettings);
        //    readXMLtoForm(MinGWBinTextBox, fileSettings);
        //    readXMLtoForm(ChefEmbeddedBinTextBox, fileSettings);
        //    readXMLtoForm(ChefRootTextBox, fileSettings);
        //    readXMLtoForm(GitSSHTextBox, fileSettings);
        //    readXMLtoForm(GitEmailAddressTextBox, fileSettings);
        //    readXMLtoForm(GitFirstNameTextBox, fileSettings);
        //    readXMLtoForm(GitLastNameTextBox, fileSettings);
        //    readXMLtoForm(GEMPathTextBox, fileSettings);
        //    readXMLtoForm(GEMSourcesTextBox, fileSettings);
        //    readXMLtoForm(EC2HomeTextBox, fileSettings);
        //    readXMLtoForm(EC2UrlTextBox, fileSettings);
        //    readXMLtoForm(VagrantEmbeddedTextBox, fileSettings);
        //    readXMLtoForm(VagrantEmbeddedBinTextBox, fileSettings);
        //    readXMLtoForm(puTTYgenTextBox, fileSettings);
        //    readXMLtoForm(ProjectNameComboBox, fileSettings);
        //    //valueReader(KitchentLogLevelComboBox, fileSettings);  <-- This is not necessary, because adding values is not allowed.
        //    readXMLtoForm(KeyIDTextBox, fileSettings);

        //    //set selected items in combo boxes
        //    setSelections(ProjectNameComboBox, fileSettings);
        //    setSelections(KitchentLogLevelComboBox, fileSettings);  //but we do need to get the selected value from the XML File
        //    setSelections(OrgComboBox, fileSettings);
        //}

        private void SaveData(XDocument form = null)
        {
            //need to re-access the config file to ensure we have the most recent copy...

            XDocument latestFile = XDocument.Load(Program.settings.ToString());        
            XElement latestSettings = latestFile.XPathSelectElement(Program.xStructure.Access(1, "[@id = 'current']"));

            if (formChanged(xdoc, latestSettings))
            {
                //connect to the file
                //XDocument xFile = XDocument.Load(Program.settings.ToString());

                //connect to the the 'current' settings node in the file
                //XElement currentFileSettings = xdoc.XPathSelectElement("configFile/settings[@id = 'current']");

                //* 1) Make room in the file!!!!!!
                
                //start by taking the file's 'current' settings and giving them an ID number instead
                //connect to the document and count the number of settings already there
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
                MessageBox.Show(
                    "The configuration was saved successfully.", 
                    "Save Complete",
                    System.Windows.Forms.MessageBoxButtons.OK ,
                    System.Windows.Forms.MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("There were no configuration changes to save.", 
                    "Save Skipped",
                    System.Windows.Forms.MessageBoxButtons.OK ,
                    System.Windows.Forms.MessageBoxIcon.Information);
            }
        }

        //private XElement writeFormToXML(TextBox tBox, XElement current)
        //{
        //    if (tBox.Text != string.Empty)
        //    {   //add the box's name and value                  
        //        current.Add(
        //            new XElement(
        //                tBox.Name.ToString(),
        //                tBox.Text.ToString()
        //                )
        //            );
        //    }
        //    return current;
        //}

        //private XElement writeFormToXML(ComboBox cBox, XElement current)
        //{
        //    //add a new element in the node for the comboBox
        //    current.Add(new XElement(cBox.Name.ToString()), string.Empty);

        //    //select the newly added element
        //    XElement cBoxElement = current.Element(cBox.Name.ToString());

        //    foreach (var item in cBox.Items)
        //    {
        //        if ((string)cBox.SelectedItem == item.ToString())
        //        { //this is the currently selected item in the form

        //            cBoxElement.Add(
        //                new XElement("item", item.ToString(),
        //                new XAttribute("selected", "1")));
        //        }
        //        else
        //        {
        //            cBoxElement.Add(new XElement("item", item.ToString()));
        //        }
        //    }
        //    return current;
        //}

        //private void readXMLtoForm(TextBox cBox, XElement root)
        //{
        //    string xmlValue = (string)root.Element(cBox.Name.ToString());
        //    if ((xmlValue != string.Empty) && (xmlValue != null))
        //    {
        //        cBox.Text = xmlValue;
        //        cBox.ForeColor = System.Drawing.Color.Black;
        //    }
        //}

        //private void readXMLtoForm(ComboBox cBox, XElement root)
        //{
        //    XElement branch = root.XPathSelectElement(cBox.Name.ToString());
        //    if (branch != null)
        //    {
        //        IEnumerable<XElement> ComboBoxitems =
        //            from branchItems in branch.Descendants()
        //            select branchItems;

        //        if (cBox.Items.Count > 0)
        //        {   //if there are already items in the combo box, clear them
        //            cBox.Items.Clear();
        //        }

        //        foreach (XElement xmlData in ComboBoxitems)
        //        {
        //            cBox.Items.Add(xmlData.Value);
        //            cBox.ForeColor = System.Drawing.Color.Black;
        //        }
        //    }

        //}

        //private void setSelections(ComboBox cBox, XElement root)
        //{
        //    string value = (string)root.XPathSelectElement(cBox.Name.ToString() + "/item[@selected = '1']");
        //    if ((value != string.Empty) && (value != null))
        //    {
        //        cBox.SelectedItem = value;
        //    }
        //}

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


        //Individual actions: Other
        private void SaveAllButton_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void GitLastNameTextBox_Leave()
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
            editArgs.FileName = "iexplore.exe";
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
            Program.loadEnvironment(currentConfig, mode);

            //open the command prompt
            if (commandWindow)
            {
                System.Diagnostics.Process.Start("cmd");
            }
        }

        //private void ComboBox_Leave(object sender, EventArgs e)
        //{
        //    //this *will* hold the code to add newly entered text into a
        //    //combobox list, and then select the newly entered item.
        //    ComboBox cBox = (ComboBox)sender;

        //    if (cBox.Text.ToString() != string.Empty)
        //    {   //There is text in this combo box

        //        if (!cBox.Items.Contains(cBox.Text))
        //        {   //The text is not in the item list, so add it
        //            cBox.Items.Add(cBox.Text);
        //            Debug.Write("{" + cBox.Name + "}: added {" + cBox.Text + "} to item list. \n");
        //            //and select it
        //            cBox.SelectedIndex = cBox.FindString(cBox.Text);
        //            // cBox.SelectedItem = cBox.Items.Count - 1;
        //        }
        //    }
        //    else
        //    {
        //        //there is no text int he combo box, this shouldn't be an option
        //        Debug.Write("{" + cBox.Name + "} was empty.\n");
        //    }

        //    //when leaving the userID box, run the mooLink checker
        //    if (cBox.Name == "UserIDComboBox")
        //    {
        //        checkMooKeysLink();
        //    }
        //    Debug.Write("{" + cBox.Name + "}: selected item is {" + cBox.SelectedItem + "}.\n");
        //    Debug.Write("{" + cBox.Name + "}: selected index is {" + cBox.SelectedIndex + "}.\n");
        //}

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

        #endregion

        #region Helpers
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

        ///// <summary>
        ///// Collects all the data stored in the form in the specidied XML format.
        ///// </summary>
        ///// <returns>Returns and XDocument representing the data currently on the form.</returns>
        //private XDocument xmlFormData()
        //{   
        //    XDocument newForm = new XDocument(
        //        new XElement("settings",
        //            new XAttribute("id", "current")
        //            )
        //        );

        //    //select that single settings current element
        //    XElement formContents = newForm.XPathSelectElement("settings[@id = 'current']");

        //    //load the form data into the new element
        //    formContents = writeFormToXML(KeyIDTextBox, formContents);
        //    formContents = writeFormToXML(PublicKeyTextBox, formContents);
        //    formContents = writeFormToXML(PrivateKeyTextBox, formContents);
        //    formContents = writeFormToXML(OrgComboBox, formContents);
        //    formContents = writeFormToXML(ProjectNameComboBox, formContents);
        //    formContents = writeFormToXML(UserIDTextBox, formContents);
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
        //    formContents = writeFormToXML(puTTYgenTextBox, formContents);            
        //    return newForm;
        //}

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
            XElement cleanedFormData = new XElement(xForm.XPathSelectElement(Program.xStructure.Access(1,"[@id = 'current']")));
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

    }
}
