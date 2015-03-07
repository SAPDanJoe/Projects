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
        //some itmes that need to be available to all the methods
        private static XDocument xdoc = XDocument.Load(Environment.GetEnvironmentVariable("USERPROFILE") + @"\downloads\Scratch.xml");

        //define XPath strings
        private static string root = "configFile";
        private static string currentSettings = root + "/settings[@current = '1']";
        private static string monsoon = currentSettings + "/monsoon";
        private static string organizations = monsoon + "/organization";
        private static string currentOrganization = monsoon + "/organization[@selected = '1']";
        private static string projects = currentOrganization + "/Project";
        private static string currentProject = currentOrganization + "/Project[@selected = '1']";
        
        public ScratchForm()
        {
            InitializeComponent();

            //load the 1st two textboxes with the XML data
            publicKeyTextBox.Text = xdoc.XPathSelectElement(monsoon + "/SSH_Public_Key").Value.ToString();
            privateKeyTextBox.Text = xdoc.XPathSelectElement(monsoon + "/SSH_Private_Key").Value.ToString();

            //load the comboBoxes with the XML data
            OrgComboBox.Items.Clear();
            foreach (XElement item in xdoc.XPathSelectElements(organizations))
            {
                OrgComboBox.Items.Add(item.Attribute("name").Value.ToString());
                if (item.Attribute("selected") != null && item.Attribute("selected").Value.ToString() == "1")
                {
                    OrgComboBox.SelectedItem = item.Attribute("name").Value.ToString();
                }
            }

            projectComboBox.Items.Clear();
            foreach (XElement item in xdoc.XPathSelectElements(projects))
            {
                projectComboBox.Items.Add(item.Attribute("name").Value.ToString());
                if (item.Attribute("selected") != null && item.Attribute("selected").Value.ToString() == "1")
                {
                    projectComboBox.SelectedItem = item.Attribute("name").Value.ToString();
                }
            }

            //load the current project's items
            EC2_URLTextBox.Text = xdoc.XPathSelectElement(currentProject + "/EC2_URL").Value.ToString();
            AWS_Access_KEYTextBox.Text = xdoc.XPathSelectElement(currentProject + "/AWS_ACCESS_KEY").Value.ToString();
            AWS_SECRET_KEYTextBox.Text = xdoc.XPathSelectElement(currentProject + "/AWS_SECRET_KEY").Value.ToString();
            
        }

        public void comboSelectionChanged(object sender, EventArgs e)
        {
            //when the selection changes...
            // 1) cast the sender as a comboBox
            ComboBox cBox = (ComboBox)sender;

            // 2) identify the sender
            if (cBox.Name.ToString() == "OrgComboBox")
            {   //this is the organization combo box                
                
                //get the old and newly selected elements
                XElement was = xdoc.XPathSelectElement(currentOrganization);
                XElement now = xdoc.XPathSelectElement(organizations + "[@name = '" + cBox.SelectedItem.ToString() + "']");

                //remove the previous element's "seleted" attribute, and add it to the newly selected attribute
                was.SetAttributeValue("selected", null);
                now.SetAttributeValue("selected", 1);

                //load the children of the new selection into the form
                //child of the organization is the project combo box
                projectComboBox.Items.Clear();
                projectComboBox.Text = null;
                string selected = null;
                foreach (XElement item in xdoc.XPathSelectElements(projects))
                {
                    projectComboBox.Items.Add(item.Attribute("name").Value.ToString());
                    if (item.Attribute("selected") != null && item.Attribute("selected").Value.ToString() == "1")
                    {
                        selected = item.Attribute("name").Value.ToString();
                    }
                }
                //need to set the selection last, because this will (hopefully) fire the selection changed event on the child
                if (selected != null)
                {
                    projectComboBox.SelectedItem = selected;
                }
                else
                {
                    projectComboBox.SelectedIndex = -1;
                }
                comboSelectionChanged(projectComboBox, new EventArgs());
            }
            else if (cBox.Name.ToString() == "projectComboBox")
            {   //this is the project name combo box               

                //check if the box has a selected item...
                if (cBox.SelectedIndex != -1)
                {
                    //get the old and newly selected elements
                    XElement was = xdoc.XPathSelectElement(currentProject);
                    XElement now = xdoc.XPathSelectElement(projects + "[@name = '" + cBox.SelectedItem.ToString() + "']");

                    //remove the previous element's "seleted" attribute, and add it to the newly selected attribute
                    was.SetAttributeValue("selected", null);
                    now.SetAttributeValue("selected", 1);
                }


                //load the children of the new selection into the form
                //EC2_URL and AWS keys are children
                EC2_URLTextBox.Text =
                    xdoc.XPathSelectElement(currentProject + "/EC2_URL") == null ? null : xdoc.XPathSelectElement(currentProject + "/EC2_URL").Value.ToString();
                AWS_Access_KEYTextBox.Text =
                    xdoc.XPathSelectElement(currentProject + "/AWS_ACCESS_KEY") == null ? null : xdoc.XPathSelectElement(currentProject + "/AWS_ACCESS_KEY").Value.ToString();
                AWS_SECRET_KEYTextBox.Text =
                    xdoc.XPathSelectElement(currentProject + "/AWS_SECRET_KEY") == null ? null : xdoc.XPathSelectElement(currentProject + "/AWS_SECRET_KEY").Value.ToString();
            }
            else
            {   //I messed something up, because the combobox name is invalid
                Debug.Write("Unreachable code encountered: The combobox name {" + cBox.Name.ToString() + "} is not valid!");
                return;
            }
            
        }
    }
}
