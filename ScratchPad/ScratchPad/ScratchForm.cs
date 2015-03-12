﻿using System;
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
                int level = int.Parse(box.Tag.ToString());
                uiValue = box.Text.ToString();
                docValue = (xdoc.XPathSelectElement(Program.Path.Access(level)) == null) ? string.Empty : xdoc.XPathSelectElement(Program.Path.Access(level,"[@selected = '1']")).Attribute("name").Value.ToString();
                bool existsInDoc = (xdoc.XPathSelectElement(Program.Path.Access(level, "[@name = '" + box.Text.ToString() + "']")) != null) ? true : false;

                if (!existsInDoc)
                {   //if the uiValue in the Combobox is not yet in the document, add it, reload the box's items, and select the newly added item

                    //store uiValue in xdoc
                    xdoc = Program.XMLDoc(xdoc, level);
                    xdoc = Program.XMLDoc(xdoc, level, box.Name.ToString(), true);
                    xdoc = Program.XMLDoc(xdoc, level, uiValue, true, "name");

                    //add new item to box list
                    box.Items.Add(box.Text);

                    //select new value in combobox
                    box.SelectedIndex = box.Items.Count - 1;
                }
                
                if (uiValue != docValue)
                {
                    //if the value in the UI has deviated from that in the document, fire selection changed event
                    comboSelectionChanged(box, new EventArgs());
                    exchangeSelections(box);
                }
            }
            else if (sender is TextBox)
            {
                TextBox box = (TextBox)sender;
                int level = int.Parse(box.Tag.ToString());
                uiValue = box.Text.ToString();
                docValue = (xdoc.XPathSelectElement(Program.Path.Access(level)) == null) ? string.Empty : xdoc.XPathSelectElement(Program.Path.Access(level)).Value.ToString();

                if (uiValue != docValue)
                {
                    //store uiValue in xdoc
                    xdoc = Program.XMLDoc(xdoc, level, uiValue);
                    xdoc = Program.XMLDoc(xdoc, level, box.Name.ToString(), true);
                }
                System.Threading.Thread.Sleep(1);
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
                int tag = int.Parse(box.Tag.ToString());                
                string boxPath = Program.Path.Access(tag,
                    "[@controlName = '" + box.Name.ToString() + "']");

                switch (box.GetType().ToString())
                {
                    case "System.Windows.Forms.TextBox":
                        box.Text = (xdoc.XPathSelectElement(boxPath) != null) ?
                            xdoc.XPathSelectElement(boxPath).Value.ToString() :
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
            //get the box's level
            int level = int.Parse(cBox.Tag.ToString());
            
            //get the parent path for this combobox
            string parent = Program.Path.AddNew(level);

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
            int level = int.Parse(cBox.Tag.ToString());
            //get the old and newly selected elements

            //this is the item still 'selected' in the xdoc
            XElement docSelection = xdoc.XPathSelectElement(Program.Path.Access(level,"[@selected = '1']"));

            //this is the item currently selected in the UI
            XElement uiSelection = xdoc.XPathSelectElement(Program.Path.Access(level,"[@name = '" + cBox.SelectedItem.ToString() + "']"));  

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

        /// <summary>
        /// gets XPathto add a control item
        /// </summary>
        /// <param name="pathItem">The Control whose setting path you want to add</param>
        /// <returns>XPath string</returns>
        private string addSettingPath(Control pathItem)
        {
            string path = string.Empty;
            string itemTag = (pathItem.Tag != null) ?
                pathItem.Tag.ToString() :
                string.Empty;
            //string pathSuffix = "[@controlName = '" + pathItem.Name.ToString() + "']";

            switch (itemTag)
            {
                case "monsoon":
                    path = (pathItem.Name.ToString() == "OrgComboBox") ?
                        monsoon + "/organization" :
                        monsoon + "/moSetting";
                    break;
                case "organization":
                    path = currentOrganization + "/project";
                    break;
                case "project":
                    path = currentProject;
                    break;
                default:
                    path = currentSettings + "/machineSetting";
                    break;
            }
            return path;
        }

        #endregion

    }
}
