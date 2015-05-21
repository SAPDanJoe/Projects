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
using System.Windows.Forms;


namespace ScratchPad
{
    class Program
    {        
        static void Main(string[] args)
        {
            
        }

        #region Recipe Generation

        /// <summary>
        /// Creates the chef code to write an Environmental Variable on a Windows client.
        /// If the Variable exits, its value is overwritten.
        /// </summary>
        /// <param name="recipeFile"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        static void writeEnv (string recipeFile, string key, string value)
        {
            string fileText = 
                "env "+ key + "do" + Environment.NewLine +
                "  attribute " + value + Environment.NewLine +
                "  action: create" + Environment.NewLine +
                "end" + Environment.NewLine;
            System.IO.File.AppendAllText(recipeFile, fileText);
        }

        static void appendEnv(string recipeFile, string key, string value, bool first = false)
        {
            string fileText = "varValue = ENV['" + key + "']" + Environment.NewLine;
            if (first)
            {
                fileText += "varValue = " + value + " + varValue" + Environment.NewLine;
            }
            else
            {
                fileText += "varValue = varValue + " + value + Environment.NewLine;
            }
            
            fileText += Environment.NewLine + 
                "env " + key + "do" + Environment.NewLine +
                "    attribute: varValue" + Environment.NewLine +
                "    action: create" + Environment.NewLine +
                "end" + Environment.NewLine;

            System.IO.File.AppendAllText(recipeFile, fileText);
        }

        #endregion

        #region XMLdata

        public static class Path
        {
            #region LevelNames            
            private static string rootName = "configFile";
            private static string level1 = "settings";
            private static string level2 = "thisMachine";
            private static string level3 = "monsoonGroup";
            private static string level4 = "monsoonSetting";
            private static string level5 = "organization";
            private static string level6 = "project";
            private static string level7 = "projectSetting";

            private static string curr = "current";
            private static string sel = "selected";
            private static string cont = "ControlName";
            
            private static string attribCurrent = "[@" + curr + " = '1']";
            private static string attribSelected = "[@" + sel + " = '1']";
            #endregion

            /// <summary>
            /// XPath string to which a NEW *element* can be added
            /// </summary>
            /// <param name="level">The level of the new item.</param>
            /// <returns>XPath formatted string of the parent of the specied level</returns>
            public static string AddNew(int level)
            {
                string pathString = string.Empty;

                switch (level)
                {
                    case 1:
                        pathString = rootName;
                        break;
                    case 2:
                        pathString = rootName + "/" + level1 + attribCurrent;
                        break;
                    case 3:
                        pathString = rootName + "/" + level1 + attribCurrent;
                        break;
                    case 4:
                        pathString = rootName + "/" + level1 + attribCurrent + "/" + level3;
                        break;
                    case 5:
                        pathString = rootName + "/" + level1 + attribCurrent + "/" + level3;
                        break;
                    case 6:
                        pathString = rootName + "/" + level1 + attribCurrent + "/" + level3 + "/"  + level5 + attribSelected;
                        break;
                    case 7:
                        pathString = rootName + "/" + level1 + attribCurrent + "/" + level3 + "/" + level5 + attribSelected + "/" + level6 + attribSelected;
                        break;
                    default:
                        Debug.Write("Error in Path.AddNew: the level {" + level.ToString() + "} was not recognized." + Environment.NewLine);
                        Debug.Write("Error in Path.AddNew: an error is expected accessing the xDoc." + Environment.NewLine);
                        break;
                }
                return pathString;
            }

            /// <summary>
            /// Gets the XPath of the element.
            /// </summary>
            /// <param name="level">The level at which the element should be found</param>
            /// <param name="additionAttributes">List&lt;String&gt; of additional attributes in XPath format ([@name = 'value']) used to identify the XElement.</param>
            /// <returns>XPath formatted string</returns>            
            public static string Access(int level, string additionAttributes = null)
            {
                string pathString = string.Empty;
                string attribs = additionAttributes;

                switch (level)
                {
                    case 0:
                        pathString = rootName + attribs;
                        break;
                    case 1:
                        pathString = rootName + "/" + level1 + attribs;
                        break;
                    case 2:
                        pathString = rootName + "/" + level1 + attribCurrent + "/" + level2 + attribs;
                        break;
                    case 3:
                        pathString = rootName + "/" + level1 + attribCurrent + "/" + level3 + attribs;
                        break;
                    case 4:
                        pathString = rootName + "/" + level1 + attribCurrent + "/" + level3 + "/" + level4  + attribs;
                        break;
                    case 5:
                        pathString = rootName + "/" + level1 + attribCurrent + "/" + level3 + "/" +  level5 + attribs;
                        break;
                    case 6:
                        pathString = rootName + "/" + level1 + attribCurrent + "/" + level3 + "/" +  level5 + attribSelected + "/" + level6 + attribs;
                        break;
                    case 7:
                        pathString = rootName + "/" + level1 + attribCurrent + "/" + level3 + "/" + level5 + attribSelected + "/" + level6 + attribSelected + "/" + level7 + attribs;
                        break;
                    default:
                        Debug.Write("Error in Path.Access: the level {" + level.ToString() + "} was not recognized." + Environment.NewLine);
                        Debug.Write("Error in Path.Access: an error is expected accessing the xDoc." + Environment.NewLine);
                        break;
                }
                return pathString;
            }
            
            /// <summary>
            /// Gets the base name of an element.
            /// </summary>
            /// <param name="level">The level at which the element whose name is requested</param>
            /// <returns>string</returns>            
            public static string LevelName(int level, bool attrib = false, bool secondary = false)
            {
                string pathString = string.Empty;

                switch (level)
                {
                    case 0:
                        pathString = rootName;
                        break;
                    case 1:
                        pathString = attrib ? curr : level1;
                        break;
                    case 2:
                        pathString = attrib ? cont : level2;
                        break;
                    case 3:
                        pathString = level3;
                        break;
                    case 4:
                        pathString = attrib ? cont : level4;
                        break;
                    case 5:
                        pathString = 
                            (attrib && secondary) ? cont :
                            (attrib && !secondary) ? sel : level5;
                        break;
                    case 6:
                        pathString = 
                            (attrib && secondary) ? cont :
                            (attrib && !secondary) ? sel : level6;
                        break;
                    case 7:
                        pathString =
                            (attrib && secondary) ? cont : level7;
                        break;
                    default:
                        Debug.Write("Error in Path.LevelName: the level {" + level.ToString() + "} was not recognized." + Environment.NewLine);
                        Debug.Write("Error in Path.LevelName: an error is expected accessing the xDoc." + Environment.NewLine);
                        break;
                }
                return pathString;
            }
        }
        
        static void makeXML()
        {
            //create a document
            XDocument xdoc = XMLDoc();

            //add a 1st level element under the root
            xdoc = XMLDoc(xdoc, 1);

            //add the "current" attribute
            xdoc = XMLDoc(xdoc, 1, "1", true);

            //add another 1st level element under the root, without the current attribute
            xdoc = XMLDoc(xdoc, 1, "These are some other non-current settings, without the \"current\" attribute.");

            //generate some machine settings
            for (int i = 0; i < 10; i++)
            {
                string controlName = "someControl" + i.ToString(); //some randon name for a control
                xdoc = XMLDoc(xdoc, 2, @"C:\monsoon\chef\bin");
                xdoc = XMLDoc(xdoc, 2, controlName , true);
            }


            //add a container element at the 3rd level (which actually resides at the same level as the 2nd level elements)
            xdoc = XMLDoc(xdoc, 3);

            //add a 4th level element 
            xdoc = XMLDoc(xdoc, 4, 
                @"ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQCjCLCr+dnXbKjguZ7okOJmwZnYzp8h2VWEMnnTMPyM/iL0A+EQebw2PzSFSZVtGQaAJ4rq5j5DD6dPOp7I6FrzVgWnie6RTUd7sqC+Uu1z/SoNOMIjPvzYvp8bmMi9lLK0S/zPq7fnTr00rLW6pRM0HoeH5IXqeoOfk0Zzz4qMcPpeR5j4Q2Snaq6KAu9xWiBfGkgWrmwYAc0ny8vqWlkGfqnFDhegZekW1v/g4s+NvMjFXb0ZJjB+u2zPe5MJmjSvE5PRqU3Tq953E0cpjPnie1H7bz5XBrFKEueXQ9mprfwe5aGKlggODlgEQMvVHXRDNiUtyRpnr8IWBzA2H1ZZ Generated key for I837633 (Dan Joe Lopez)"
                );
            xdoc = XMLDoc(xdoc, 4, "publicKeyTextBox", true);

            //add another 4th level element 
            xdoc = XMLDoc(xdoc, 4, "A very private Key");
            xdoc = XMLDoc(xdoc, 4, "privateKeyTextBox", true);

            //add the 5th level element and an attribute indicating that it is selected (as for a comboBox)
            xdoc = XMLDoc(xdoc, 5);
            xdoc = XMLDoc(xdoc, 5, "OrgComboBox", true);
            xdoc = XMLDoc(xdoc, 5, Environment.GetEnvironmentVariable("USERNAME"), true, "name");
            xdoc = XMLDoc(xdoc, 5, "1", true);

            //add another level5 without the selected atribute
            xdoc = XMLDoc(xdoc, 5);
            xdoc = XMLDoc(xdoc, 5, "OrgComboBox", true);
            xdoc = XMLDoc(xdoc, 5, "some other org", true,"name");

            //generate some level 6 elements
            for (int i = 0; i < 5; i++)
            {
                bool test = i == 2 ? true : false;
                string num = (i + 1).ToString();
                string projectName = "project_" + num + "_name";

                xdoc = XMLDoc(xdoc, 6);
                xdoc = XMLDoc(xdoc, 6, "projectComboBox", true);
                xdoc = XMLDoc(xdoc, 6, projectName, true, "name");

                //set one of the projects to be 'selected'
                if (test)
                {
                    xdoc = XMLDoc(xdoc, 6, "1", true);
                }
            }
            
            //in the selected project add some relevant data
            xdoc = XMLDoc(xdoc, 7, @"https://ec2-us-west.api.monsoon.mo.sap.corp:443");
            xdoc = XMLDoc(xdoc, 7, "EC2_URLTextBox", true);
            xdoc = XMLDoc(xdoc, 7, @"STgzNzYzMzo6MTc3OTc%3D%0A");
            xdoc = XMLDoc(xdoc, 7, "AWS_Access_KEYTextBox", true);
            xdoc = XMLDoc(xdoc, 7, @"hRfAb%2FmOz6Phg%2B%2B73%2BwuQhMmqz%2BmSAHg%2FZ%2FyR1Ch4b4%3D%0A");
            xdoc = XMLDoc(xdoc, 7, "AWS_SECRET_KEYTextBox", true);
            //store the xml file to disk
            xdoc.Save(Environment.GetEnvironmentVariable("USERPROFILE") + @"\downloads\scratch.xml");
        }

        /// <summary>
        /// Creates a new XML Doc with the specified Root Node
        /// </summary>
        /// <param name="rootNode">Name of the root node. default is the conde configured Program.Path.rootName</param>
        /// <returns>XDocument</returns>
        static XDocument XMLDoc(string rootNode = null)
        {
            rootNode = (rootNode == null) ? Program.Path.AddNew(1) : rootNode;
            XDocument doc = new XDocument(new XElement(rootNode));
            return doc;
        }

        /// <summary>
        /// Adds Elements or atributes to an XML Document
        /// </summary>
        /// <param name="doc">The XDocument to which the data should be added</param>
        /// <param name="parent">The XPath to the parent of the element upon which changes are to be made.</param>
        /// <param name="name">The string name of the element or attribute.</param>
        /// <param name="value">The (optional)string value of the element or attribute.</param>
        /// <param name="attrib">a(optional) bool indicating if this should be an attribute of an existing element.</param>
        /// <returns>A new XDocument based on the document that you supplied, with the specified changes.</returns>
        public static XDocument XMLDoc(XDocument doc, int level, string value = null, bool attrib=false, string customName = null)
        {
            //check for null parameters
            if (doc == null || level == null)
            {
                Debug.Write("Null parameters are not valid!");
                return null;
            }

            //setup a string for the name of the element or attribute
            string name = Path.LevelName(level, attrib);

            if (attrib && value != "1" && customName == null)
            {
                name = Path.LevelName(level, attrib, true);
            }

            if (customName != null)
            {
                name = customName;
            }

            //setup a srting name for the path to the parent element
            string parent = attrib ? Path.Access(level) : Path.AddNew(level);

            //need to add a [last()] identifier to the path if multiples are found
            if (doc.XPathSelectElements(parent) != null && doc.XPathSelectElements(parent).Count() > 1)
            {
                parent += "[last()]";
            }


            //check if the parent path is found in the document
            if (doc.XPathSelectElement(parent) == null)
            {
                Debug.Write("The parent path {" + parent + "} was not found in the XML document.");
                return null;
            }

            //if the value is null (default) then add a valuless element
            if (value == null)
            {
                doc.XPathSelectElement(parent).Add(new XElement(name));
            }

            //if the value is not null, and attrib is true, add an attribute with a value
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
            System.Threading.Thread.Sleep(1);
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