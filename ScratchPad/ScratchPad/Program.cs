using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

using System.Diagnostics;
using System.Windows.Automation;

using System.Xml.Linq;
using System.Xml.XPath;

using System.Data;


namespace ScratchPad
{
    class Program
    {        
        static void Main(string[] args)
        {
            makeXML();
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new ScratchForm());
        }

        #region XMLdata
        
        static void makeXML()
        {
            //base root name of the XML
            string rootParent = "configFile";

            //create a document
            XDocument xdoc = XMLDoc(rootParent);

            //add a 'settings' element node
            xdoc = XMLDoc(xdoc, rootParent, "settings");

            //reference to the newly added node
            string currentSettings = rootParent + "/settings";

            //add the "current" attribute
            xdoc = XMLDoc(xdoc, currentSettings, "current", "1", true);
            //add another settings node without the current attribute
            xdoc = XMLDoc(xdoc, rootParent, "settings", "These are some other non-current settings, without the \"current\" attribute.");

            //respecify the currentSettings to be the one with the "current" attribute
            currentSettings += "[@current = '1']";

            //generate some machine settings
            for (int i = 0; i < 10; i++)
            {
                xdoc = XMLDoc(xdoc, currentSettings, "machineSetting" + i.ToString(), @"C:\monsoon\chef\bin");
            }

            //defines the name of the next element, and the XPath where it will be found
            string moElementName = "monsoon";
            string moPath = currentSettings + "/" + moElementName;

            //defines the name of the next element, and the XPath where it will be found
            string orgElementName = "organization";
            string currentOrg = moPath + "/" + orgElementName;

            //add the mo element
            xdoc = XMLDoc(xdoc, currentSettings, moElementName);

            //add some mo settings
            xdoc = XMLDoc(xdoc, moPath, "SSH_Public_Key", @"ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQCjCLCr+dnXbKjguZ7okOJmwZnYzp8h2VWEMnnTMPyM/iL0A+EQebw2PzSFSZVtGQaAJ4rq5j5DD6dPOp7I6FrzVgWnie6RTUd7sqC+Uu1z/SoNOMIjPvzYvp8bmMi9lLK0S/zPq7fnTr00rLW6pRM0HoeH5IXqeoOfk0Zzz4qMcPpeR5j4Q2Snaq6KAu9xWiBfGkgWrmwYAc0ny8vqWlkGfqnFDhegZekW1v/g4s+NvMjFXb0ZJjB+u2zPe5MJmjSvE5PRqU3Tq953E0cpjPnie1H7bz5XBrFKEueXQ9mprfwe5aGKlggODlgEQMvVHXRDNiUtyRpnr8IWBzA2H1ZZ Generated key for I837633 (Dan Joe Lopez)");
            xdoc = XMLDoc(xdoc, moPath, "SSH_Private_Key", "A very private Key");

            //add the org element and an attribute indicating that it is selected (as for a comboBox)
            xdoc = XMLDoc(xdoc, moPath, orgElementName);
            xdoc = XMLDoc(xdoc, currentOrg, "name", Environment.GetEnvironmentVariable("USERNAME"), true);
            xdoc = XMLDoc(xdoc, currentOrg, "selected", "1", true);

            //update the selection string to point to the currently 'selected' XElement
            currentOrg += "[@selected = '1']";

            //generate some 'Project' elements
            for (int i = 0; i < 5; i++)
            {
                bool test = i == 2 ? true : false;
                string num = (i + 1).ToString();
                string projectName = "project_" + num + "_name";

                xdoc = XMLDoc(xdoc, currentOrg, "Project");
                xdoc = XMLDoc(xdoc, currentOrg + "/Project[last()]", "name", projectName, true);

                //set one of the projects to be 'selected'
                if (test)
                {
                    xdoc = XMLDoc(xdoc, currentOrg + "/Project[@name = '" + projectName + "']", "selected", "1", true);
                }
            }

            //define the path to the selected project in the selected org in the current settings...
            string currentProject = currentOrg + "/Project[@selected = '1']";

            //in the selected project add some relevant data
            xdoc = XMLDoc(xdoc, currentProject, "EC2_URL", @"https://ec2-us-west.api.monsoon.mo.sap.corp:443");
            xdoc = XMLDoc(xdoc, currentProject, "AWS_ACCESS_KEY", @"STgzNzYzMzo6MTc3OTc%3D%0A");
            xdoc = XMLDoc(xdoc, currentProject, "AWS_SECRET_KEY", @"hRfAb%2FmOz6Phg%2B%2B73%2BwuQhMmqz%2BmSAHg%2FZ%2FyR1Ch4b4%3D%0A");

            //store the xml file to disk
            xdoc.Save(Environment.GetEnvironmentVariable("USERPROFILE") + @"\downloads\scratch.xml");
        }

        static XDocument XMLDoc(string rootNode)
        {
            XDocument doc = new XDocument(new XElement(rootNode));
            return doc;
        }

        static XDocument XMLDoc(XDocument doc, string parent, string name, string value = null, bool attrib=false)
        {            
            //check for null parameters
            if (doc == null || name == null || parent == null)
            {
                throw new Exception("Null parameters are not valid!");
            }

            //check if the parent path is found in the document
            if (doc.XPathSelectElement(parent) == null)
            {
                throw new Exception("The parent path {" + parent + "} was not found in the XML document.");
            }

            //if the value is null (default) then add a valuless element
            if (value == null)
            {
                doc.XPathSelectElement(parent).Add(new XElement(name));
            }

            //if the value is not null, and attrib is true, add an element with a value
            else if (attrib)
            {
                doc.XPathSelectElement(parent).Add(new XAttribute(name, value));
            }
            //otherwise add the information as a new child element
            else
            {
                doc.XPathSelectElement(parent).Add(new XElement(name, value));
            }            
            
            return doc;
        }

        #endregion

        #region file encryption

        static void crypt()
        {
            //using System.Security.Cryptography;
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            RSAParameters keyPair = provider.ExportParameters(true);
            string keys = provider.ToXmlString(true);
            int i = 0;
        }

        #endregion

        #region UI Automation

        static void auto()
        {
            //using System.Diagnostics;
            //using System.Windows.Automation;
            //added references for Framework: UIAutomationClient and UIAutomationTypes

            //the goal is to launch puttygen with a specific private key, and save it in 
            //putty's ppk format without any user interaction
            
            //define some variables
            string puTTYgenPath =
                Environment.GetEnvironmentVariable("ProgramFiles(x86)") + 
                @"\putty\puttygen.exe"; //location of Puttygen
            string ID_RSA = 
                Environment.GetEnvironmentVariable("USERPROFILE") + 
                @"\.ssh\ID_RSA";        //location of file to convert
            
            //create a process
            System.Diagnostics.Process PPKgenerator = new System.Diagnostics.Process();
            
            //add start info to the process
            PPKgenerator.StartInfo.FileName = puTTYgenPath;
            PPKgenerator.StartInfo.Arguments = ID_RSA;

            //start the process
            PPKgenerator.Start();

            //get the process ID
            int procID = PPKgenerator.Id;

            Debug.Write("Clicking the OK button...");
            clickButton(procID, "OK");

            Debug.Write("Clicking the Save Key button...");
            clickButton(procID, "Save private key");

            Debug.Write("Clicking the Yes button...");
            clickButton(procID, "Yes", "PuTTYgen Warning");

            Debug.Write("Writing the destination path...");
            enterText(
                procID, 
                Environment.GetEnvironmentVariable("USERPROFILE") + @"\.ssh\id_rsa.ppk",
                @"Save private key as:");

            Debug.Write("Clicking the Save file button...");
            clickButton(procID, "Save", @"Save private key as:");

            while (!System.IO.File.Exists(Environment.GetEnvironmentVariable("USERPROFILE") + @"\.ssh\id_rsa.ppk"))
            {
                System.Threading.Thread.Sleep(50);
            }

            Process.GetProcessById(procID).Kill();
        }

        /// <summary>
        /// Clicks a button on a main or subwindow of a windows form.  Use Inspect 
        /// or UISpy to get the element names (windows and buttons).
        /// </summary>
        /// <param name="ProcID">The process ID of the target application</param>
        /// <param name="ButtonName">The name of the button to click</param>
        /// <param name="subWindowName">[optional] The name of the subwindow</param>
        static void clickButton(int ProcID, string ButtonName, string subWindowName = null)
        {
            AutomationElement root = AutomationElement.RootElement;
            Debug.Write("Root has been set." + Environment.NewLine);
            
            //System.Threading.Thread.Sleep(500);

            PropertyCondition UIAProcID = new PropertyCondition(
                AutomationElement.ProcessIdProperty, ProcID);

            Debug.Write("UIAProcID has been set." + Environment.NewLine);

            AutomationElement Window = root.FindFirst(TreeScope.Children, UIAProcID);

            Debug.Write("Window has been set." + Environment.NewLine);

            AutomationElement sub = null;
            if (subWindowName != null)
            {
                PropertyCondition subWindow = new PropertyCondition(
                    AutomationElement.NameProperty, subWindowName);

                Debug.Write("subWindow has been set." + Environment.NewLine);

                sub = Window.FindFirst(TreeScope.Children, subWindow);

                Debug.Write("sub has been set." + Environment.NewLine);

            }
            else
            {
                sub = Window;
            }

            PropertyCondition buttonName = new PropertyCondition(
                AutomationElement.NameProperty, ButtonName);

            Debug.Write("buttonName has been set." + Environment.NewLine);

            PropertyCondition buttonType = new PropertyCondition(
                AutomationElement.LocalizedControlTypeProperty, "button");

            Debug.Write("ButtonType has been set." + Environment.NewLine);

            AndCondition condition = new AndCondition(buttonName, buttonType);

            Debug.Write("Condition has been set." + Environment.NewLine);

            AutomationElement button = sub.FindFirst(TreeScope.Children, condition);

            Debug.Write("button has been set." + Environment.NewLine);

            InvokePattern doClick = (InvokePattern)button.GetCurrentPattern(InvokePattern.Pattern);

            Debug.Write("doClick has been set." + Environment.NewLine);
            Debug.Write("Before action: { doClick.Invoke() } with window {" + Window.Current.Name + "} and button: {" + button.Current.Name + "}." + Environment.NewLine);

            System.Threading.ThreadStart invokeModal = new System.Threading.ThreadStart(doClick.Invoke);
            System.Threading.Thread modal = new System.Threading.Thread(invokeModal);
            modal.Start();
           // doClick.Invoke();
            Debug.Write("Action exited: { doClick.Invoke() }." + Environment.NewLine);

            //CacheRequest cache = new CacheRequest();
            //cache.Add(AutomationElement.)
        }

        /// <summary>
        /// Enters text in a a main or subwindow combo entry box.  Use Inspect 
        /// or UISpy to get the main and sub window names.
        /// </summary>
        /// <param name="ProcID">The process ID of the target application</param>
        /// <param name="text">The text to be entered</param>
        /// <param name="subWindowName">[optional] The name of the subwindow</param>
        static void enterText(int ProcID, string text, string subWindowName = null)
        {
            AutomationElement root = AutomationElement.RootElement;
            Debug.Write("Root has been set." + Environment.NewLine);

            System.Threading.Thread.Sleep(500);

            PropertyCondition UIAProcID = new PropertyCondition(
                AutomationElement.ProcessIdProperty, ProcID);

            Debug.Write("UIAProcID has been set." + Environment.NewLine);

            AutomationElement Window = root.FindFirst(TreeScope.Children, UIAProcID);

            Debug.Write("Window has been set." + Environment.NewLine);

            AutomationElement sub = null;
            if (subWindowName != null)
            {
                PropertyCondition subWindow = new PropertyCondition(
                    AutomationElement.NameProperty, subWindowName);

                Debug.Write("subWindow has been set." + Environment.NewLine);

                sub = Window.FindFirst(TreeScope.Children, subWindow);

                Debug.Write("sub has been set." + Environment.NewLine);

                string test = sub.Current.Name;
                string tes1 = (string)Window.FindFirst(TreeScope.Children, subWindow).Current.Name;
                string tes2 = (string)Window.FindFirst(TreeScope.Children, new PropertyCondition(
                    AutomationElement.NameProperty, @"Save private key as:")).Current.Name;

            }
            else
            {
                sub = Window;
            }
            
            PropertyCondition fieldType = new PropertyCondition(
                AutomationElement.LocalizedControlTypeProperty, "combo box");

            Debug.Write("fieldType has been set." + Environment.NewLine);

            AutomationElement field = sub.FindFirst(TreeScope.Descendants, fieldType);            
            
            Debug.Write("field has been set." + Environment.NewLine);


            object valuePattern = null;

            if (field.TryGetCurrentPattern(
                ValuePattern.Pattern, out valuePattern))
            {
                field.SetFocus();
                ((ValuePattern)valuePattern).SetValue(text);
                Debug.Write("text has been set!" + Environment.NewLine);
            }
            else
            {
                Debug.Write("field does not support VlauePattern, use SendKeys" + Environment.NewLine);
            }

        }
        
        #endregion
    }
}