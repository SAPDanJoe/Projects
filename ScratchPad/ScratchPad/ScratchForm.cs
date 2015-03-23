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
            List<Control> myTextBoxes = listControls(this, new TextBox());
            List<Control> myComboBoxes = listControls(this, new ComboBox());

            //populate the fields
            populate(myTextBoxes);
            populate(myComboBoxes);
            Debug.Write("******Intial loading of form is completed!**********" + Environment.NewLine);
        }

        /// <summary>
        /// Lists all the controls that are children of the specified control.
        /// </summary>
        /// <param name="parent">The control whose child ocontrols are to be retrieved.</param>
        /// <returns>List&lt;Control&gt;</returns>
        private List<Control> childrenOf(Control parent)
        {
            //initialize the result list
            List<Control> result = new List<Control>();

            //get the parameter's level
            int boxLevel = int.Parse(parent.Tag.ToString());
            int childLevel = new int();
            List<Control> formTextBoxes = listControls(this, new TextBox());

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
                    Debug.Write("The control {" + parent.Name + "} at level {" + boxLevel + "} does not have any children!");
                    break;
            }
            result = listControls(this, childLevel.ToString());

            return result;
        }

        /// <summary>
        /// Recurses through a root control and gets all child controls by Type.
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
        /// Recurses through a root control and gets all child controls by Control.Tag(string).
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
            
            var dbg = (Control)sender;
            Debug.Write("Leaving a {" + dbg.GetType().ToString() +"} named {" + dbg.Name.ToString() +"} at level {" + dbg.Tag +"} ."+ Environment.NewLine);

            if (sender is ComboBox)
            {
                ComboBox box = (ComboBox)sender;
                int level = int.Parse(box.Tag.ToString());
                uiValue = box.Text.ToString();
                docValue = (xdoc.XPathSelectElement(Program.Path.Access(level, "[@selected = '1']")) == null) ? string.Empty : xdoc.XPathSelectElement(Program.Path.Access(level, "[@selected = '1']")).Attribute("name").Value.ToString();
                bool existsInDoc = (xdoc.XPathSelectElement(Program.Path.Access(level, "[@name = '" + box.Text.ToString() + "']")) != null) ? true : false;

                if (!existsInDoc && !string.IsNullOrEmpty(uiValue))
                {   //if the uiValue in the Combobox is not yet in the document, add it
                    Debug.Write("The control's value {" + uiValue + "} is not in the document, adding..." + Environment.NewLine);

                    //store uiValue in xdoc
                    xdoc = Program.XMLDoc(xdoc, level);
                    xdoc = Program.XMLDoc(xdoc, level, box.Name.ToString(), true);
                    xdoc = Program.XMLDoc(xdoc, level, uiValue, true, "name");
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
                    Debug.Write("The form entered value {" + uiValue + "} does not match the selected document's value{" + docValue + "} , updating document to reflect the changes..." + Environment.NewLine);

                    //comboSelectionChanged(box, new EventArgs());
                    exchangeSelections(box);
                }
                Debug.Write("{" + box.Name + "has {" + childrenOf(box).Count + "} children to populate..." + Environment.NewLine);
                populate(childrenOf(box));
            }
            else if (sender is TextBox)
            {
                TextBox box = (TextBox)sender;
                int level = int.Parse(box.Tag.ToString());
                uiValue = box.Text.ToString();
                docValue = (xdoc.XPathSelectElement(Program.Path.Access(level)) == null) ? string.Empty : xdoc.XPathSelectElement(Program.Path.Access(level)).Value.ToString();

                if (uiValue != docValue)
                {
                    Debug.Write("The control's value is not in the document, adding..." + Environment.NewLine);
                    //store uiValue in xdoc
                    xdoc = Program.XMLDoc(xdoc, level, uiValue);
                    xdoc = Program.XMLDoc(xdoc, level, box.Name.ToString(), true);
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
            foreach (var box in boxes)
            {
                int tag = int.Parse(box.Tag.ToString());                
                string boxPath = Program.Path.Access(tag,
                    "[@ControlName = '" + box.Name.ToString() + "']");

                switch (box.GetType().ToString())
                {
                    case "System.Windows.Forms.TextBox":
                        box.Text = (xdoc.XPathSelectElement(boxPath) != null) ?
                            xdoc.XPathSelectElement(boxPath).Value.ToString() :
                            string.Empty;
                        leaveBox(box, new EventArgs());
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
            string parent = Program.Path.Access(level);

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
            leaveBox(cBox, new EventArgs());
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
            if (docSelection != null)
            {                
                docSelection.SetAttributeValue("selected", null);
            }
            uiSelection.SetAttributeValue("selected", 1);
        }

        #endregion

    }
}
