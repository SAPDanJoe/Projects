using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Automation;
using System.Xml.XPath;

namespace CS_MonsoonProjectSelector
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MonsoonSettingsMainForm());
        }

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
                        pathString = rootName + "/" + level1 + attribCurrent + "/" + level3 + "/" + level5 + attribSelected;
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
                        pathString = rootName + "/" + level1 + attribCurrent + "/" + level3 + "/" + level4 + attribs;
                        break;
                    case 5:
                        pathString = rootName + "/" + level1 + attribCurrent + "/" + level3 + "/" + level5 + attribs;
                        break;
                    case 6:
                        pathString = rootName + "/" + level1 + attribCurrent + "/" + level3 + "/" + level5 + attribSelected + "/" + level6 + attribs;
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

        static void makeXML(FileInfo path)
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
                xdoc = XMLDoc(xdoc, 2, controlName, true);
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
            xdoc = XMLDoc(xdoc, 5, "some other org", true, "name");

            //generate some level 6 elements
            for (int i = 0; i < 5; i++)
            {
                bool test = i == 2 ? true : false;
                string num = (i + 1).ToString();
                string projectName = "project_" + num + "_name";

                xdoc = XMLDoc(xdoc, 6);
                xdoc = XMLDoc(xdoc, 6, "ProjectNameComboBox", true);
                xdoc = XMLDoc(xdoc, 6, projectName, true, "name");

                //set one of the projects to be 'selected'
                if (test)
                {
                    xdoc = XMLDoc(xdoc, 6, "1", true);
                }
            }

            //in the selected project add some relevant data
            xdoc = XMLDoc(xdoc, 7, @"https://ec2-us-west.api.monsoon.mo.sap.corp:443");
            xdoc = XMLDoc(xdoc, 7, "EC2UrlTextBox", true);
            xdoc = XMLDoc(xdoc, 7, @"STgzNzYzMzo6MTc3OTc%3D%0A");
            xdoc = XMLDoc(xdoc, 7, "AccessKeyTextBox", true);
            xdoc = XMLDoc(xdoc, 7, @"hRfAb%2FmOz6Phg%2B%2B73%2BwuQhMmqz%2BmSAHg%2FZ%2FyR1Ch4b4%3D%0A");
            xdoc = XMLDoc(xdoc, 7, "SecretKeyTextBox", true);
            //store the xml file to disk
            xdoc.Save(path.ToString());
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
        public static XDocument XMLDoc(XDocument doc, int level, string value = null, bool attrib = false, string customName = null)
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

        public static FileInfo settings = initializeConfig();
       
        /// <summary>
        /// Check if there is an existing config file, and if not creates one.
        /// </summary>
        /// <returns>Returns the file as a fileinfo object.</returns>
        public static FileInfo initializeConfig ()
        {   
            //string components of path to confilg file
            string appdata = System.Environment.GetEnvironmentVariable("APPDATA");
            string configPath = @"\SAPIT\Monsoon\";
            string configFile = "Config.xml";

            //convert strings into FileInfo Objects
            System.IO.DirectoryInfo configDir = new System.IO.DirectoryInfo(appdata + configPath);
            System.IO.FileInfo config = new System.IO.FileInfo(configDir.ToString() + configFile);

            if (!configDir.Exists) //Test to see if the directory exists
            {   //directory not present, therfore file is not present, create a both
                configDir.Create();
               // generateNewConfig(config);
                makeXML(config);
            }
            else if (!config.Exists)
            {   //directory is present, but file is not, create the file
                //generateNewConfig(config);
                makeXML(config);
            }
            return config;
        }

        /// <summary>
        /// This generates a new empty XML settings file, and logs the date/time when the file was initialized.
        /// </summary>
        /// <param name="configFile">The FileInfo for the destination file.</param>
        public static void generateNewConfig(FileInfo configFile)
        {
            XDocument xconfig = new XDocument(
                new XElement("configFile",
                    new XAttribute("created",
                        DateTime.Now.ToString()
                    ),
                    new XElement("settings",
                        new XAttribute("id","current"),
                        new XAttribute("created",DateTime.Now.ToString())
                    )
                )
            );
            xconfig.Save(configFile.ToString());
        }

        /// <summary>
        /// This will load the settings from the form into the environment based on the provided mode, then launch a command window. NOTE: some of these settings are environment variables, and some are files
        /// </summary>
        /// <param name="config">This XElement is the XML configuration element</param>
        /// <param name="mode">The intended scope for the settings. {Process, User, Machine}</param>
        public static void loadEnvironment(XElement config, EnvironmentVariableTarget mode)
        {
            //environmental Variables
            //!!!!!Chef
            //!!!!!!!!Variables
            addEnv(                 (string)config.Element("ChefRootTextBox"), mode);
            addEnv(                 (string)config.Element("ChefEmbeddedBinTextBox"), mode);
            addEnv(                 (string)config.Element("MinGWBinTextBox"), mode, "beginning");
            addEnv("RI_DEVKIT",     (string)config.Element("DevkitBinTextBox"), mode);
            addEnv("KITCHEN_LOG",   (string)config.Element("KitchentLogLevelComboBox"), mode); //might be a little more complicated to set the combo boxes...

            //!!!!!git
            //!!!!!!!!Variables
            addEnv("GIT_SSH",       (string)config.Element("GitSSHTextBox"), mode);
            addEnv("HOME", Environment.GetEnvironmentVariable("USERPROFILE"), mode);
            //!!!!!git
            //!!!!!!!!Configuration actions
            System.Diagnostics.Process.Start("git","config --global user.name \"" +
                    (string)config.Element("GitFirstNameTextBox") +
                    (string)config.Element("GitFirstNameTextBox") + "\"");
            System.Diagnostics.Process.Start("git","config --global user.email \"" +
                    (string)config.Element("GitEmailAddressTextBox") + "\"");
            System.Diagnostics.Process.Start("git","config --global color.ui true");
            System.Diagnostics.Process.Start("git","config --global http.sslVerify false");

            //!!!!!Monsoon SSH Keys
            //!!!!!!!!ID_RSA files

            //this will backup the existing files on the 1st run
            string sshPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\.ssh\";
            string sshFile = Environment.GetEnvironmentVariable("USERPROFILE") + @"\.ssh\id_rsa";
            string sshBackup = Environment.GetEnvironmentVariable("USERPROFILE") + @"\.ssh\backup\";

            bool sshPathExist = Directory.Exists(sshPath);
            bool sshFileExist = File.Exists(sshFile);
            bool sshBackupExist = Directory.Exists(sshBackup);

            //Create a backup if .ssh\id_rsa exists and .ssh\backup does not
            if (sshFileExist & !sshBackupExist)
            {
                Directory.CreateDirectory(sshBackup);

                //now backup the exiting private key                
                File.Move(
                    sshFile,
                    sshBackup + @"id_rsa_BAK_" + DateTime.Now.ToString("yyyy-MM-dd_hhmmss"));  
   
                //now backup the existing public key, after checking existence
                string pubKey = sshFile + @".pub";
                if (File.Exists(pubKey))
                {
		            File.Move(
                        pubKey,
                        sshBackup + @"id_rsa.pub_BAK_" + DateTime.Now.ToString("yyyy-MM-dd_hhmmss"));
                }

                //Also backup the putty key if it exists
                string putKey = sshFile + @".ppk";
                if (File.Exists(putKey))
                {
		            File.Move(
                        putKey,
                        sshBackup + @"id_rsa.ppk_BAK_" + DateTime.Now.ToString("yyyy-MM-dd_hhmmss"));
                }
            }
                
            //create the new files

            //private key
            string ID_RSA = (string)config.Element("PrivateKeyTextBox");
            File.WriteAllText(sshFile,ID_RSA);

            //public key
            string ID_RSA_PUB = (string)config.Element("PublicKeyTextBox");
            File.WriteAllText(sshFile +".pub", ID_RSA_PUB);

            //puTTY Key
            string ppkFile = Environment.GetEnvironmentVariable("USERPROFILE") + @"\.ssh\id_rsa.ppk";            
            
            string puTTYgenPath = (string)config.Element("puTTYgenTextBox");
            if (File.Exists(puTTYgenPath))
            {
                if (System.IO.File.Exists(ppkFile))
                {   //the file already exists, so delete it first
                    System.IO.File.Delete(ppkFile);
                }                
                
                Process PPKgenerator = new Process();
                PPKgenerator.StartInfo.FileName = puTTYgenPath;
                PPKgenerator.StartInfo.Arguments = sshFile;
                PPKgenerator.Start();

                int PPKid = PPKgenerator.Id;

                Debug.Write("Clicking the OK button...");
                clickButton(PPKid, "OK");

                Debug.Write("Clicking the Save Key button...");
                clickButton(PPKid, "Save private key");

                Debug.Write("Clicking the Yes button...");
                clickButton(PPKid, "Yes", "PuTTYgen Warning");

                Debug.Write("Writing the destination path...");
                enterText(PPKid, ppkFile,@"Save private key as:");

                Debug.Write("Clicking the Save file button...");
                clickButton(PPKid, "Save", @"Save private key as:");

                //wait for the file to be written to the system
                while (!System.IO.File.Exists(ppkFile))
                {
                    System.Threading.Thread.Sleep(50);
                }

                //close the window
                PPKgenerator.Kill();
            }
            else
            {
                Debug.Write("The PuTTY key generator does not appear to be installed.  A PuTTY keyfile has not been generated.");
            }


            //!!!!!AWS
            //!!!!!!!!Variables
            addEnv("AWS_ORGANIZATION",(string)config.Element("OrgTextBox"), mode);
            addEnv("AWS_PROJECT",   (string)config.Element("ProjectNameComboBox"), mode);
            addEnv("AWS_ACCESS_KEY",(string)config.Element("AccessKeyTextBox"), mode);
            addEnv("AWS_SECRET_KEY",(string)config.Element("SecretKeyTextBox"), mode);
            addEnv("AWS_SSH_KEY_ID",(string)config.Element("KeyIDTextBox"), mode);

            //!!!!!EC2
            //!!!!!!!!Variables
            addEnv("EC2_HOME",      (string)config.Element("EC2HomeTextBox"), mode);
            addEnv("CLASSPATH",     (string)config.Element("EC2HomeTextBox")+"\\lib", mode);
            addEnv(                 (string)config.Element("EC2HomeTextBox") + "\\bin", mode);
            addEnv("EC2_SSH_KEY",   Environment.GetEnvironmentVariable("USERPROFILE") + "\\.ssh\\id_rsa", mode);
            addEnv("EC2_URL",       (string)config.Element("EC2UrlTextBox"), mode);
            

            //!!!!!FOG
            //!!!!!!!!FogFile: ~/.fog
            string[] fogFile= {
                                  "default:",
                                  "  aws_access_key_id: " + (string)config.Element("AccessKeyTextBox"),
                                  "  aws_secret_access_key: " + (string)config.Element("SecretKeyTextBox"),
                                  "  host: monsoon.mo.sap.corp",
                                  "  path: /api/ec2"
                              };
            System.IO.File.WriteAllLines(Environment.GetEnvironmentVariable("USERPROFILE").ToString() + @"\.fog",fogFile);


            //!!!!!GEM
            //!!!!!!!!Variables
            addEnv("GEM_PATH",      (string)config.Element("GEMPathTextBox"), mode);
            //!!!!!GEM
            //!!!!!!!!GEMRC file: ~/.gemrc
            
            //The data from the XML file is from a multiline TextBox
            //We'll need this in seperate strings to add the lines to the file
            //fist load the whole element into a string
            string lines = (string)config.Element("GEMSourcesTextBox").ToString();

            //now take off the leading and training XML tags
            lines = lines.Substring(19);
            lines = lines.Substring(0,lines.Length - 20);

            //now split the lines into an aray seperated by the line breaks
            //Also create a new string where the 
            string[] linesArray = lines.Split(new string[] {"\r\n","\n"},StringSplitOptions.None);
            string newLines = string.Empty;

            //itterate through the array and add each non-blank line to the file
            foreach (string line in linesArray)
            {
                if (line != string.Empty)
                {
                    newLines = newLines + "- " + lines + Environment.NewLine;
                }
            }

            //for (int i = 0; i < linesArray.Length; i++)
            //{
            //    if (linesArray[i] != string.Empty)
            //    {
            //        newLines = newLines + "- " + linesArray[i] + Environment.NewLine;                                       
            //    }
            //}

            string line1 = (string)config.Element("GEMSourcesTextBox").ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)[0].Substring(19);
            string[] gemRCFile = {
                                     "---",
                                     "http_proxy: :no_proxy",
                                     ":bulk_threshold: 1000",
                                     ":update_sources: true",
                                     ":backtrace: false",
                                     ":verbose: true",
                                     ":sources:",
                                     newLines,
                                     ":benchmark: false",
                                     "gem: --no-http-proxy --no-ri --no-rdoc"
                                 };
            System.IO.File.WriteAllLines(Environment.GetEnvironmentVariable("USERPROFILE").ToString() + @"\.gemrc",gemRCFile);

            //!!!!!kitchen-Monsoon
            //!!!!!!!!Variables
            addEnv(                 (string)config.Element("VagrantEmbeddedBinTextBox"), mode);
                // seem to be missing ...\Vagrant\bin

            //!!!!!Vagrant
            //!!!!!!!!Variables
            addEnv(                 (string)config.Element("VagrantEmbeddedTextBox"), mode);


            addEnv(                 (string)config.Element("PublicKeyTextBox"), mode);
            addEnv(                 (string)config.Element("PrivateKeyTextBox"), mode);


            //!!!!!Informational
            addEnv("Mo_Configured", DateTime.Now.ToString(), mode);
        }

        #region Environment
        //All of the handlers for the environmental variables and local setting files

        /// <summary>
        /// Adds value to the PATH, in the specified order.
        /// </summary>
        /// <param name="value">The value to add to the path</param>
        /// <param name="scope">[Optional] The variable scope {*Process, User, Machine}</param>
        /// <param name="order">[Optional] Where to add the value {beginning, *end}(</param>
        public static void addEnv(string value, EnvironmentVariableTarget scope = EnvironmentVariableTarget.Process, string order = "end")
        {   
            string currentPath = Environment.GetEnvironmentVariable("PATH");
            string newPath;

            switch (order)
            {
                case "beginning":
                    newPath = value + ";" + currentPath;
                    break;

                default:
                    newPath = currentPath + ";" + value;
                    break;
            }
            Environment.SetEnvironmentVariable("PATH", newPath, scope);
        }

        /// <summary>
        /// Adds and environmental variable in the specified scope, overriding a previous variable in the given scope.
        /// </summary>
        /// <param name="varName">The variable name to be set.</param>
        /// <param name="value">The value to assign the variable.</param>
        /// <param name="scope">Specifies the scope for the variable (user, system, etc.)</param>
        public static void addEnv(string varName, string value, EnvironmentVariableTarget scope)
        {   
            Environment.SetEnvironmentVariable(varName, value, scope);
        }
        #endregion

        #region UIAutomation
        //Contains the code for the UI automation that the PPK generation requires

        /// <summary>
        /// Clicks a button on a main or subwindow of a windows form.  Use Inspect 
        /// or UISpy to get the element names (windows and buttons).
        /// </summary>
        /// <param name="ProcID">The process ID of the target application</param>
        /// <param name="ButtonName">The name of the button to click</param>
        /// <param name="subWindowName">[optional] The name of the subwindow</param>
        static void clickButton(int ProcID, string ButtonName, string subWindowName = null)
        {
            //binds to the desktop (root element of all windows)
            Debug.Write("Binding to the root desktop element." + Environment.NewLine);
            AutomationElement root = AutomationElement.RootElement;

            Debug.Write("Setting PropertyCondition UIAProcID." + Environment.NewLine);
            PropertyCondition UIAProcID = new PropertyCondition(
                AutomationElement.ProcessIdProperty, ProcID);

            Debug.Write("Setting AutomationElement Window." + Environment.NewLine);
            AutomationElement Window = waitForElement(root,TreeScope.Children, UIAProcID);

            AutomationElement sub = null;
            if (subWindowName != null)
            {
                Debug.Write("Setting PropertyCondition subWindow ." + Environment.NewLine);
                PropertyCondition subWindow = new PropertyCondition(
                    AutomationElement.NameProperty, subWindowName);

                Debug.Write("Setting the ActionElement sub to subWindow." + Environment.NewLine);
                sub = waitForElement(Window,TreeScope.Children, subWindow);
            }
            else
            {
                Debug.Write("No sub-window specified, Setting the ActionElement sub to Window." + Environment.NewLine);
                sub = Window;
            }

            Debug.Write("Setting PropertyCondition buttonName." + Environment.NewLine);
            PropertyCondition buttonName = new PropertyCondition(
                AutomationElement.NameProperty, ButtonName);

            Debug.Write("Setting the button element." + Environment.NewLine);
            //AutomationElement button = waitForElement(sub, TreeScope.Children, condition);
            AutomationElement button = waitForElement(sub, TreeScope.Children, buttonName);

            Debug.Write("Setting the button invocation action." + Environment.NewLine);
            InvokePattern doClick = (InvokePattern)button.GetCurrentPattern(InvokePattern.Pattern);


            Debug.Write("Invoking the button action (click)." + Environment.NewLine);
            System.Threading.ThreadStart invokeModal = new System.Threading.ThreadStart(doClick.Invoke);
            System.Threading.Thread modal = new System.Threading.Thread(invokeModal);
            modal.Start();

            Debug.Write("The button has been clicked." + Environment.NewLine);
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
            //binds to the desktop (root element of all windows)
            Debug.Write("Binding to the root desktop element." + Environment.NewLine);
            AutomationElement root = AutomationElement.RootElement;

            Debug.Write("Setting PropertyCondition UIAProcID." + Environment.NewLine);
            PropertyCondition UIAProcID = new PropertyCondition(
                AutomationElement.ProcessIdProperty, ProcID);

            Debug.Write("Setting AutomationElement Window." + Environment.NewLine);
            AutomationElement Window = waitForElement(root, TreeScope.Children, UIAProcID);

            AutomationElement sub = null;
            if (subWindowName != null)
            {
                Debug.Write("Setting PropertyCondition subWindow ." + Environment.NewLine);
                PropertyCondition subWindow = new PropertyCondition(
                    AutomationElement.NameProperty, subWindowName);

                Debug.Write("Setting the ActionElement sub to subWindow." + Environment.NewLine);       
                sub = waitForElement(Window,TreeScope.Children, subWindow);
            }
            else
            {
                Debug.Write("No sub-window specified, Setting the ActionElement sub to Window." + Environment.NewLine);
                sub = Window;
            }

            Debug.Write("Setting PropertyCondition fieldType." + Environment.NewLine);
            PropertyCondition fieldType = new PropertyCondition(
                AutomationElement.LocalizedControlTypeProperty, "combo box");

            Debug.Write("Setting the field element." + Environment.NewLine);
            AutomationElement field = waitForElement(sub,TreeScope.Descendants, fieldType);

            object valuePattern = null;

            if (field.TryGetCurrentPattern(
                ValuePattern.Pattern, out valuePattern))
            {
                Debug.Write("Writing text to the path name." + Environment.NewLine);
                field.SetFocus();
                ((ValuePattern)valuePattern).SetValue(text);
            }
            else
            {
                Debug.Write("field does not support VlauePattern, use SendKeys" + Environment.NewLine);
            }
        }

        /// <summary>
        /// Searches for an Element inside the specified root with that matches the given condition.  Returns and AutomationElement.
        /// </summary>
        /// <param name="root">The root element in which to search</param>
        /// <param name="scope">This is the level at which to search.</param>
        /// <param name="condition">The UIA condition to use as a filter.</param>
        /// <returns></returns>
        static AutomationElement waitForElement(AutomationElement root, TreeScope scope, PropertyCondition condition)
        {
            while (root.FindFirst(scope, condition) == null)
            {
                Thread.Sleep(50);
            }

            AutomationElement resultElement = root.FindFirst(scope, condition);
            return resultElement;
        }

        #endregion

        static void auto()
        {//this was my scratch code for the automation
            
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
    }
}
