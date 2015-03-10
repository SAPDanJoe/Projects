using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Xml.Linq;
using System.Xml.XPath;

using System.Diagnostics;

namespace ScratchPad
{
    public partial class ScratchForm : Form
    {
        //some itmes that need to be available to the whole form
        private XDocument xdoc = XDocument.Load(Environment.GetEnvironmentVariable("USERPROFILE") + @"\downloads\Scratch.xml");

        //define XPath strings
        private static string root = "configFile";
        private static string currentSettings = root + "/settings[@current = '1']";
        private static string monsoon = currentSettings + "/monsoon";
        private static string organizations = monsoon + "/organization";
        private static string currentOrganization = monsoon + "/organization[@selected = '1']";
        private static string projects = currentOrganization + "/project";
        private static string currentProject = currentOrganization + "/project[@selected = '1']";
        
        public ScratchForm()
        {
            InitializeComponent();

            //enumerate populable fields
            var myTextBoxes = listControls(this, new TextBox());
            var myComboBoxes = listControls(this, new ComboBox());

            //populate the fields
            populate(myTextBoxes);
            populate(myComboBoxes);
        }

        /// <summary>
        /// Event handler for the ComboBox.SelectionChanged event
        /// </summary>
        /// <param name="sender">The comboBox where the selection was changed</param>
        /// <param name="e">Not currently impimented, use new EventArgs()</param>
        public void comboSelectionChanged(object sender, EventArgs e)
        {
            //when the selection changes...
            // 1) cast the sender as a comboBox
            ComboBox cBox = (ComboBox)sender;
            
            // 2)puts the new selection in the Xdocument, and removes 'seleceted' from the item that is no longer selected
            if (cBox.SelectedIndex != -1)
            {   //this happens regardless of which box is selected
                exchangeSelections(cBox);
            }

            // 3) determine which combo level we have
            string myChildren = (cBox.Name.Contains("project")) ? "project" : "organization";
            
            // 3) collect all the children of this item
            var children = listControls(this, myChildren);

            // 4) populate the children
            populate(children);
            
            // * if any of the chilbren are combo boxes...
            var childCombos = children.FindAll(delegate(Control box) { return box.GetType().ToString() == "System.Windows.Forms.ComboBox"; });

            //fire the selectionChanged event for the child combobox
            foreach (ComboBox childBox in childCombos)
            {
                comboSelectionChanged(childBox, new EventArgs());                    
            }
        }

        /// <summary>
        /// Recurses through a root control and gets all child controls.
        /// </summary>
        /// <param name="root">The root control from which to begin recursing. Usually "this", but could also be a groupBox, tabPane, etc.</param>
        /// <param name="type">The type of control to find.  You can put any control to match here, or use "new TextBox", for example.</param>
        /// <returns>List&lt;Control&gt;</returns>
        public List<Control> listControls(Control root, Control type)
        {
            List<Control> theList = new List<Control>();

            foreach (Control control in root.Controls)
            {
                if (control.GetType() == type.GetType())
                {
                    theList.Add(control);
                }
                else if (control.HasChildren)
                {
                    theList.InsertRange(theList.Count(), listControls(control, type));
                }
            }            
            return theList;
        }

        /// <summary>
        /// Recurses through a root control and gets all child controls.
        /// </summary>
        /// <param name="root">The root control from which to begin recursing. Usually "this", but could also be a groupBox, tabPane, etc.</param>
        /// <param name="tag">A string to which the control's tag will be matched.</param>
        /// <returns>List&lt;Control&gt;</returns>
        public List<Control> listControls(Control root, string tag)
        {
            List<Control> theList = new List<Control>();

            foreach (Control control in root.Controls)
            {
                if (control.Tag != null && control.Tag.ToString() == tag)
                {
                    theList.Add(control);
                }
                else if (control.HasChildren)
                {
                    theList.InsertRange(theList.Count(), listControls(control, tag));
                }
            }
            return theList;
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

            if (sender is ComboBox)
            {
                ComboBox box = (ComboBox)sender;
                uiValue = box.SelectedItem.ToString();
                docValue = xdoc.XPathSelectElement(settingPath(box)).Attribute("name").Value.ToString();
                if (uiValue != docValue)
                {
                    //store uiValue in xdoc
                    xdoc = Program.XMLDoc(xdoc, settingPath(box), "name", uiValue, true);                
                    xdoc = Program.XMLDoc(xdoc, settingPath(box) + "[last()]", "controlName", box.Name.ToString() , true);

                    //select new value in combobox
                    box.SelectedIndex = box.Items.Count - 1;

                    //fire selection changed event
                    comboSelectionChanged(box, new EventArgs());
                }
            }
            else if (sender is TextBox)
            {
                TextBox box = (TextBox)sender;
                uiValue = box.SelectedText.ToString();
                docValue = (xdoc.XPathSelectElement(settingPath(box)) == null) ? string.Empty : xdoc.XPathSelectElement(settingPath(box)).Value.ToString();

                if (uiValue != docValue)
                {
                    //store uiValue in xdoc
                    xdoc = Program.XMLDoc(xdoc, settingPath(box), "name", uiValue);
                    xdoc = Program.XMLDoc(xdoc, settingPath(box) + "[last()]", "controlName", box.Name.ToString(), true);
                }
            }

        }

        ///// <summary>
        ///// Main event handler for the TextChanged events of the Combo boxes
        ///// </summary>
        ///// <param name="sender">Box in which the text changed.</param>
        ///// <param name="e">Not currently impimented, use new EventArgs()</param>
        //public void leaveComboBox(ComboBox box)
        //{
            
        //}

        ///// <summary>
        ///// Main event handler for the TextChanged events of the Text boxes
        ///// </summary>
        ///// <param name="sender">Box in which the text changed.</param>
        ///// <param name="e">Not currently impimented, use new EventArgs()</param>
        //public void leaveTextBox(TextBox box)
        //{
        //    string path = settingPath(box);
        //    string storedValue = ;
        //    string uiValue = 
        //    if (true)
        //    {
                
        //    }
        //}



        #endregion

        #region Populate Boxes from xdoc

        /// <summary>
        /// Populates TextBoxes and ComboBoxes
        /// </summary>
        /// <param name="boxes">The list of boxes to populate</param>
        private void populate(List<Control> boxes)
        {
            foreach (var box in boxes)
            {
                switch (box.GetType().ToString())
                {
                    case "System.Windows.Forms.TextBox":
                        box.Text = (xdoc.XPathSelectElement(settingPath(box)) != null) ?
                            xdoc.XPathSelectElement(settingPath(box)).Value.ToString() :
                            string.Empty;
                        break;
                    case "System.Windows.Forms.ComboBox":
                        ComboBox cBox = (ComboBox)box;
                        populate(cBox);
                        break;
                    default:
                        Debug.Write("Control Type : {" + boxes.GetType().ToString() + "} does not have a defined population method." + Environment.NewLine);
                        break;
                }                
            }
        }

        /// <summary>
        /// Helper function that overloads the population method to handle a single ComboBox
        /// </summary>
        /// <param name="cBox">The Combobox to be populated from the xdoc</param>
        private void populate(ComboBox cBox)
        {
            //check if handling the project or organization comboBox
            string parent = cBox.Name.ToString().Contains("project") ? projects : organizations;

            //child of the organization is the project combo box
            cBox.Items.Clear();
            cBox.Text = null;
            string selected = string.Empty;

            foreach (XElement item in xdoc.XPathSelectElements(parent))
            {
                //add each item to the Combobox item list
                cBox.Items.Add(item.Attribute("name").Value.ToString());
                if (item.Attribute("selected") != null && item.Attribute("selected").Value.ToString() == "1")
                {   
                    selected = item.Attribute("name").Value.ToString();
                    cBox.SelectedIndex = cBox.Items.Count - 1;
                }
            }
        }

        /// <summary>
        /// When a new selection is made in a combobox, this ensures that the change is reflected in the acompanying XDocument
        /// </summary>
        /// <param name="cBox">The ComboBox where the selection has been changed.</param>
        private void exchangeSelections(ComboBox cBox)
        {
            string basePath = (cBox.Name.Contains("project")) ? projects : organizations;
            string baseElement = (cBox.Name.Contains("project")) ? currentProject : currentOrganization;
            
            //get the old and newly selected elements

            //this is the item still 'selected' in the xdoc
            XElement docSelection = xdoc.XPathSelectElement(baseElement);

            //this is the item currently selected in the UI
            XElement uiSelection = xdoc.XPathSelectElement(basePath + "[@name = '" + cBox.SelectedItem.ToString() + "']");  

            //remove the previous element's "seleted" attribute, and add it to the newly selected attribute
            docSelection.SetAttributeValue("selected", null);
            uiSelection.SetAttributeValue("selected", 1);
        }

        /// <summary>
        /// gets XPath of a control item
        /// </summary>
        /// <param name="pathItem">The Control whose setting path you want to retrieve</param>
        /// <returns>XPath string</returns>
        private string settingPath(Control pathItem)
        {
            string path = string.Empty;
            string itemTag = (pathItem.Tag != null) ?
                pathItem.Tag.ToString() :
                string.Empty;
            string pathSuffix = "[@controlName = '" + pathItem.Name.ToString() + "']";
            switch (itemTag)
            {
                case "monsoon":
                    path = (pathItem.Name.ToString() == "OrgComboBox") ?
                        monsoon + "/organization" + pathSuffix :
                        monsoon + "/moSetting" + pathSuffix;
                    break;
                case "organization":
                    path = currentOrganization + "/project" + pathSuffix;
                    break;
                case "project":
                    path = currentProject + "/projectSetting" + pathSuffix;
                    break;
                default:
                    path = currentSettings + "/machineSetting" + pathSuffix;
                    break;
            }
            return path;
        }

        #endregion
    }
}
