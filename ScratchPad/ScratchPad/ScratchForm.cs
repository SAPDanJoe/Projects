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
        private static XDocument xdoc = XDocument.Load(Environment.GetEnvironmentVariable("USERPROFILE") + @"\downloads\Scratch.xml");

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

            //determine which combo level we have
            string myChildren = (cBox.Name.Contains("project")) ? "project" : "organization";
            
            //collect all the children of this item
            var children = listControls(this, myChildren);

            //populate the children
            populate(children);
            
            //if any of the chilbren are combo boxes, fire their selectionChanged events in order
            var childCombos = children.FindAll(delegate(Control box) { return box.GetType().ToString() == "System.Windows.Forms.ComboBox"; });

            //fire the selectionChanged event for the child combobox
            foreach (ComboBox childBox in childCombos)
            {
                comboSelectionChanged(childBox, new EventArgs());                    
            }

            //// 3) identify the sender by parent
            //if (cBox.Tag.ToString() == "monsoon")
            //{   //this is the organization combo box, whose parent is the monsoon level


            //}
            //else if (cBox.Tag.ToString() == "organization")
            //{   //the only combobox whose parent is 'organization' is the project combo

            //    //load the children of the new selection into the form
            //    //EC2_URL and AWS keys are children
            //    var children = listControls(this, "project");

            //    //populate the children
            //    populate(children);

            //    List<Control> projectChildren = new List<Control>();
            //    projectChildren.Add(EC2_URLTextBox);
            //    projectChildren.Add(AWS_Access_KEYTextBox);
            //    projectChildren.Add(AWS_SECRET_KEYTextBox);
                
            //    //populate the children
            //    populate(projectChildren);
            //}
            //else
            //{   //I messed something up, because the combobox name is invalid
            //    Debug.Write("Unreachable code encountered: The combobox name {" + cBox.Name.ToString() + "} is not valid!");
            //    return;
            //}            
        }

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

    }
}
