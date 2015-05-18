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
        //the very first thing that we do is to check for a config file
        //and if one is missing we create one.
        public static FileInfo settings = initializeConfig();

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

        /// <summary>
        /// Check if there is an existing config file, and if not creates one.
        /// </summary>
        /// <returns>Returns the file as a fileinfo object.</returns>
        public static FileInfo initializeConfig ()
        {
            Debug.Write("ENTER METHOD: initializeConfig ()" + Environment.NewLine);
            
            //string components of path to confilg file
            string appdata = System.Environment.GetEnvironmentVariable("APPDATA");
            string configPath = @"\SAPIT\Monsoon\";
            string configFile = "Config.xml";

            //convert strings into FileInfo Objects
            System.IO.DirectoryInfo configDir = new System.IO.DirectoryInfo(appdata + configPath);
            System.IO.FileInfo config = new System.IO.FileInfo(configDir.ToString() + configFile);

            if (!configDir.Exists) //Test to see if the directory exists
            {   //directory not present, therfore file is not present, create a both
                Debug.Write("The directory {" + configDir.ToString() + "} does not exist, creating it..." + Environment.NewLine);

                configDir.Create();
                initializeConfig();
            }
            else if (!config.Exists)
            {   //directory is present, but file is not, create the file
                Debug.Write("The config file is not present in the directory, creating it..." + Environment.NewLine);
            
                makeXML(config);
            }
            Debug.Write("LEAVE METHOD: initializeConfig ()" + Environment.NewLine);
            return config;
        }

        #region XMLdata

        /// <summary>
        /// Centrally defines the structure of the setting document
        /// </summary>
        public static class xStructure
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

            private static string setID = "id";
            private static string sel = "selected";
            private static string cont = "ControlName";

            private static string attribCurrent = "[@" + setID + " = 'current']";
            private static string attribSelected = "[@" + sel + " = '1']";
            #endregion

            /// <summary>
            /// XPath string to which a NEW *element* can be added
            /// </summary>
            /// <param name="level">The level of the new item.</param>
            /// <returns>XPath formatted string of the parent of the specied level</returns>
            public static string AddNew(int level)
            {
                Debug.Write("xStructure.AddNew ( int ) :Enter" + Environment.NewLine);
                Debug.Write(
                    "level:     " + level.ToString() + Environment.NewLine);

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
                Debug.Write("The XPath to add a new element or attribute at level {" + level.ToString() + "} is:" + Environment.NewLine +
                    "{" + pathString + "}." + Environment.NewLine);
                Debug.Write("xStructure.AddNew ( int ) :Exit" + Environment.NewLine);
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
                Debug.Write("xStructure.Access ( int, [string] ) :Enter" + Environment.NewLine);
                Debug.Write(
                    "level:                 " + level.ToString() + Environment.NewLine +
                    "additionAttributes:    " + ((string.IsNullOrEmpty(additionAttributes)) ? string.Empty : additionAttributes) + Environment.NewLine);

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

                Debug.Write("The XPath to access and element or attribute at level {" + level.ToString() + "} is:" + Environment.NewLine +
                    "{" + pathString + "}." + Environment.NewLine);
                Debug.Write("xStructure.Access ( int, [string] ) :Exit" + Environment.NewLine);
                return pathString;
            }

            /// <summary>
            /// Gets the base name of an element.
            /// </summary>
            /// <param name="level">The level at which the element whose name is requested</param>
            /// <returns>string</returns>            
            public static string LevelName(int level, bool attrib = false, bool secondary = false)
            {
                Debug.Write("xStructure.LevelName ( int, [bool], [bool] ) :Enter" + Environment.NewLine);
                Debug.Write(
                    "level:     " + level.ToString() + Environment.NewLine +
                    "attrib:    " + attrib.ToString() + Environment.NewLine +
                    "secondary: " + secondary.ToString() + Environment.NewLine);

                string pathString = string.Empty;

                switch (level)
                {
                    case 0:
                        pathString = rootName;
                        break;
                    case 1:
                        pathString = attrib ? setID : level1;
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
                        Debug.Write("The level {" + level.ToString() + "} was not recognized." + Environment.NewLine);
                        Debug.Write("An error is expected accessing the xDoc." + Environment.NewLine);
                        break;
                }

                Debug.Write("LevelName is {" + pathString.ToString() + "}" + Environment.NewLine);
                Debug.Write("xStructure.LevelName ( int, [bool], [bool] ) :Exit" + Environment.NewLine);
                return pathString;
            }
        }

        /// <summary>
        /// Creates a new XML file in the specified location.
        /// </summary>
        /// <param name="path">FileInfo: The location of the desired document.</param>
        static void makeXML(FileInfo path)
        {
            Debug.Write("makeXML( FileInfo ): Enter" + Environment.NewLine);
   
            //create a shell document
            Debug.Write("Creating a new XML Doc..." + Environment.NewLine);
            XDocument xdoc = XMLDoc();

            //create a level 1 container
            xdoc.XPathSelectElement(xStructure.AddNew(1)).Add(new XElement(xStructure.LevelName(1)));
            xdoc.XPathSelectElement(xStructure.Access(1)).Add(new XAttribute(xStructure.LevelName(1, true),"current"));

            //create a level 3 container
            xdoc.XPathSelectElement(xStructure.AddNew(3)).Add(new XElement(xStructure.LevelName(3)));
            
            //create a level 4 container
            xdoc.XPathSelectElement(xStructure.AddNew(4)).Add(new XElement(xStructure.LevelName(4)));

            ////create a level 5 container
            //xdoc.XPathSelectElement(xStructure.AddNew(5)).Add(new XElement(xStructure.LevelName(5)));
            //xdoc.XPathSelectElement(xStructure.Access(5)).Add(new XAttribute(xStructure.LevelName(5, true), "1"));

            //store the xml file to disk
            Debug.Write("File creation complete.  Saving file to  {" + path.ToString() + "}..." + Environment.NewLine);

            xdoc.Save(path.ToString());

            Debug.Write("makeXML ( FileInfo ): Exit" + Environment.NewLine);
        }

        /// <summary>
        /// Creates a new XML Doc with the specified Root Node
        /// </summary>
        /// <param name="rootNode">Name of the root node. default is the conde configured Program.Path.rootName</param>
        /// <returns>XDocument</returns>
        public static XDocument XMLDoc(string rootNode = null)
        {
            Debug.Write("XMLDoc ( string ): Enter" + Environment.NewLine);
            rootNode = (rootNode == null) ? Program.xStructure.AddNew(1) : rootNode;
            Debug.Write("The rootNade has been set to {" + rootNode + "}, creating document..." + Environment.NewLine);
            XDocument doc = new XDocument(new XElement(rootNode));
            Debug.Write("Document created, returning..." + Environment.NewLine);
            Debug.Write("XMLDoc ( string ): Exit" + Environment.NewLine);
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
        public static XDocument XMLDoc(XDocument doc, Control box, string value = null, bool attrib = false, string customName = null)
        {
            int level = int.Parse(box.Tag.ToString());

            Debug.Write("XMLDoc ( XDocument, int, [string], [bool], [string]): Enter" + Environment.NewLine);

            bool customNameIsNull = string.IsNullOrEmpty(customName);
            string cleanValue = value == null ? string.Empty : value;
            string cleanCustomName = customNameIsNull ? string.Empty : customName;

            Debug.Write(
                "level:     " + level.ToString() + Environment.NewLine +
                "value:     " + cleanValue.ToString() + Environment.NewLine +
                "attrib:    " + attrib.ToString() + Environment.NewLine +
                "customName:" + cleanCustomName.ToString() + Environment.NewLine);

            //check for null parameters
            if (doc == null || level == null)
            {
                Debug.Write("XDocument doc and int level are required, but were passed null!");
                return null;
            }

            //determine the Lookup level for the attribute (primary or secondary)
            bool secondaryAtrib = new bool();
            secondaryAtrib = (attrib && customNameIsNull && !(value == "current" || value == "1")) ?
                true :
                false;
            Debug.Write("secondaryAttrib is set to {" + secondaryAtrib.ToString() + "}" + Environment.NewLine);

            //setup a string for the name of the element or attribute         
            string name = (!customNameIsNull) ?
                cleanCustomName :
                xStructure.LevelName(level, attrib, secondaryAtrib);
            Debug.Write("element/attribute 'name' is set to {" + name.ToString() + "}" + Environment.NewLine);

            //setup a srting name for the path to the parent element
            string parent = attrib ? xStructure.Access(level) : xStructure.AddNew(level);
            Debug.Write("parent path is set to {" + parent.ToString() + "}" + Environment.NewLine);

            //need to add a [last()] identifier to the path if multiples are found
            if (doc.XPathSelectElements(parent) != null && doc.XPathSelectElements(parent).Count() > 1)
            {
                Debug.Write("multiple elements were found in {" + parent.ToString() + "}, appending identified '[last()]'..." + Environment.NewLine);
                parent += "[last()]";
            }


            //check if the parent path is found in the document
            if (doc.XPathSelectElement(parent) == null)
            {
                Debug.Write("The parent path {" + parent + "} was not found in the XML document.");
                if (box.Text != "Enter a project name")
                {
                throw new Exception("The xPath {" + parent + "} could not be found in the configuration file!" + Environment.NewLine
                    + "If this is your first time using the tool, please try entering your settings in order.");
                }
                else
                {
                    return doc;
                }                    
            }

            //if the value is null (default) then add a valuless element
            if (value == null)
            {
                Debug.Write("No value was specified, adding empty element with name {" + name.ToString() + "}" + Environment.NewLine);
                doc.XPathSelectElement(parent).Add(new XElement(name));
            }

            //if the value is not null, and attrib is true, add an attribute with a value
            else if (attrib)
            {
                if (doc.XPathSelectElement(parent).Attribute(name) != null)
                {
                    doc.XPathSelectElement(parent).Attribute(name).Value = value;
                }else
                {
                    Debug.Write("adding attribute  {" + name.ToString() + "} with value {" + value.ToString() + "}" + Environment.NewLine);
                    doc.XPathSelectElement(parent).Add(new XAttribute(name, value));
                }
                
            }
            //otherwise add the information as a child element
            else
            {
                if (doc.XPathSelectElement(parent + "/" + name.ToString()) != null && doc.XPathSelectElement(parent + "/" + name.ToString()).Attributes().Count() == 0)
                {   //The element is already present, changing value
                    Debug.Write("Changing value {" + value.ToString() + "} to existing element {" + name.ToString() + "}." + Environment.NewLine);
                    doc.XPathSelectElement(parent + "/" + name.ToString()).Value = value;
                }
                else
                {
                    Debug.Write("adding child element  {" + name.ToString() + "} with value {" + value.ToString() + "}" + Environment.NewLine);
                    doc.XPathSelectElement(parent).Add(new XElement(name, value));
                }
            }

            Debug.Write("Competed writing document." + Environment.NewLine);
            Debug.Write("XMLDoc ( XDocument, int, [string], [bool], [string]): Exit" + Environment.NewLine);
            return doc;
        }

        /// <summary>
        /// Finds an Element by its attribute\value pair.
        /// </summary>
        /// <param name="root">XElement: The root where the search is performed (the dataset).</param>
        /// <param name="attributeValue">string: The value of the attribute.</param>
        /// <param name="attributeName">string:[optional] The  value of the attribute name.  [DEFAULT = "ControlName"]</param>
        /// <returns>string: The value of the located element.</returns>
        public static string getElementByAttribute(XElement root, string attributeValue, string attributeName = "ControlName")
        {
            XElement result = null;
            try
            {
                result = root.Descendants().Where(
                    element => (string)element.Attribute(attributeName) == attributeValue
                    ).Single();
            }
            catch (Exception)
            {

                IEnumerable<XElement> results = (IEnumerable<XElement>)root.Descendants().Where(
                    element => (string)element.Attribute(attributeName) == attributeValue
                    && (string)element.Attribute("selected") == "1"
                    );
                
                result = root.Descendants().Where(
                    element => (string)element.Attribute(attributeName) == attributeValue
                    && (string)element.Attribute("selected") == "1"
                    ).Single();
            }
            
                
            return result.Value.ToString();
        }

        #endregion
        
        /// <summary>
        /// This will load the settings from the form into the environment based on the provided mode, then launch a command window. NOTE: some of these settings are environment variables, and some are files.
        /// </summary>
        /// <param name="config">XElement: The XML configuration element to be loaded.</param>
        /// <param name="mode">The intended scope for the settings. {Process, User, Machine}</param>
        public static void loadEnvironment(XElement config, EnvironmentVariableTarget mode)
        {
            //environmental Variables
            //!!!!!Chef
            //!!!!!!!!Variables
            addEnv(                 getElementByAttribute(config, "ChefRootPathTextBox"), mode);
            addEnv(                 getElementByAttribute(config, "ChefEmbeddedBinPathTextBox"), mode);
            addEnv(                 getElementByAttribute(config, "MinGWBinPathTextBox"), mode, "beginning");
            addEnv("RI_DEVKIT",     getElementByAttribute(config, "DevkitBinPathTextBox"), mode);
            addEnv("KITCHEN_LOG",   getElementByAttribute(config, "KitchentLogLevelComboBox"), mode); //might be a little more complicated to set the combo boxes...

            //!!!!!git
            //!!!!!!!!Variables
            addEnv("GIT_SSH",       getElementByAttribute(config, "GitSSHPathTextBox"), mode);
            addEnv("HOME", Environment.GetEnvironmentVariable("USERPROFILE"), mode);
            //!!!!!git
            //!!!!!!!!Configuration actions
            System.Diagnostics.Process.Start("git","config --global user.name \"" +
                    getElementByAttribute(config, "GitFirstNameTextBox") +
                    getElementByAttribute(config, "GitLastNameTextBox") + "\"");
            System.Diagnostics.Process.Start("git","config --global user.email \"" +
                    getElementByAttribute(config, "GitEmailAddressTextBox") + "\"");
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
            if (sshFileExist && !sshBackupExist)
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
                
            //create the new files if the private key has changed
            string ID_RSA = getElementByAttribute(config, "PrivateKeyTextBox");

            if (!File.Exists(sshFile) || File.ReadAllText(sshFile) != ID_RSA)
            {
                //private key
                File.WriteAllText(sshFile, ID_RSA);

                //public key
                string ID_RSA_PUB = getElementByAttribute(config, "PublicKeyTextBox");
                File.WriteAllText(sshFile + ".pub", ID_RSA_PUB);

                //puTTY Key
                string ppkFile = Environment.GetEnvironmentVariable("USERPROFILE") + @"\.ssh\id_rsa.ppk";

                string puTTYgenPath = getElementByAttribute(config, "puTTYgenPathTextBox");
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

                    Debug.Write(@"Clicking the ""OK"" button..." + Environment.NewLine);
                    clickButton(PPKid, "OK");

                    Debug.Write(@"Clicking the ""Save Private Key"" button..." + Environment.NewLine);
                    clickButton(PPKid, "Save private key");

                    Debug.Write(@"Clicking the ""Yes"" button..." + Environment.NewLine);
                    clickButton(PPKid, "Yes", "PuTTYgen Warning");

                    Debug.Write("Writing the destination path {" + ppkFile.ToString() + "}." + Environment.NewLine);
                    enterText(PPKid, ppkFile, @"Save private key as:");

                    Debug.Write(@"Clicking the ""Save file"" button..." + Environment.NewLine);
                    clickButton(PPKid, "Save", @"Save private key as:");

                    //wait for the file to be written to the system
                    while (!System.IO.File.Exists(ppkFile))
                    {
                        Debug.Write(@"Waiting for the file to be written..." + Environment.NewLine);
                        System.Threading.Thread.Sleep(50);
                    }

                    //close the window
                    PPKgenerator.Kill();
                }
                else
                {
                    Debug.Write("The PuTTY key generator does not appear to be installed.  A PuTTY keyfile has not been generated.");
                }

            }

            

            //!!!!!AWS
            //!!!!!!!!Variables
            addEnv("AWS_ORGANIZATION",getElementByAttribute(config, "OrgComboBox"), mode);
            addEnv("AWS_PROJECT",   getElementByAttribute(config, "ProjectNameComboBox"), mode);
            addEnv("AWS_ACCESS_KEY",getElementByAttribute(config, "AccessKeyTextBox"), mode);
            addEnv("AWS_SECRET_KEY",getElementByAttribute(config, "SecretKeyTextBox"), mode);
            addEnv("AWS_SSH_KEY_ID",getElementByAttribute(config, "KeyIDTextBox"), mode);

            //!!!!!EC2
            //!!!!!!!!Variables
            addEnv("EC2_HOME",      getElementByAttribute(config, "EC2HomePathTextBox"), mode);
            addEnv("CLASSPATH",     getElementByAttribute(config, "EC2HomePathTextBox") + "\\lib", mode);
            addEnv(                 getElementByAttribute(config, "EC2HomePathTextBox") + "\\bin", mode);
            addEnv("EC2_SSH_KEY",   Environment.GetEnvironmentVariable("USERPROFILE") + "\\.ssh\\id_rsa", mode);
            addEnv("EC2_URL",       getElementByAttribute(config, "EC2UrlTextBox"), mode);
            

            //!!!!!FOG
            //!!!!!!!!FogFile: ~/.fog
            string[] fogFile= {
                                  "default:",
                                  "  aws_access_key_id: " + getElementByAttribute(config, "AccessKeyTextBox"),
                                  "  aws_secret_access_key: " + getElementByAttribute(config, "SecretKeyTextBox"),
                                  "  host: monsoon.mo.sap.corp",
                                  "  path: /api/ec2"
                              };
            System.IO.File.WriteAllLines(Environment.GetEnvironmentVariable("USERPROFILE").ToString() + @"\.fog",fogFile);


            //!!!!!GEM
            //!!!!!!!!Variables
            addEnv("GEM_PATH",      getElementByAttribute(config, "GEMPathTextBox"), mode);
            //!!!!!GEM
            //!!!!!!!!GEMRC file: ~/.gemrc
            
            //The data from the XML file is from a multiline TextBox
            //We'll need this in seperate strings to add the lines to the file
            //fist load the whole element into a string
            string lines = getElementByAttribute(config, "GEMSourcesTextBox").ToString();

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

            string line1 = getElementByAttribute(config, "GEMSourcesTextBox").ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)[0].Substring(19);
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
            addEnv(                 getElementByAttribute(config, "VagrantEmbeddedBinPathTextBox"), mode);
                // seem to be missing ...\Vagrant\bin

            //!!!!!Vagrant
            //!!!!!!!!Variables
            addEnv(                 getElementByAttribute(config, "VagrantEmbeddedPathTextBox"), mode);


            addEnv(                 getElementByAttribute(config, "PublicKeyTextBox"), mode);
            addEnv(                 getElementByAttribute(config, "PrivateKeyTextBox"), mode);


            //!!!!!Informational
            addEnv("Mo_Configured", DateTime.Now.ToString(), mode);
        }


        #region SetEnvironmentalVariables
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

        #region PPKGeneratorUIAutomation
        //Contains the code for the UI automation that the PPK generation requires

        /// <summary>
        /// Clicks a button on a main or subwindow of a windows form.  Use Inspect 
        /// or UISpy to get the element names (windows and buttons).
        /// </summary>
        /// <param name="ProcID">The process ID of the target application</param>
        /// <param name="ButtonName">The name of the button to click</param>
        /// <param name="subWindowName">[optional] The name of the subwindow</param>
        public static void clickButton(int ProcID, string ButtonName, string subWindowName = null)
        {
            //binds to the desktop (root element of all windows)
            Debug.Write(ProcID.ToString() + "    Binding to the root desktop element." + Environment.NewLine);
            AutomationElement desktop = AutomationElement.RootElement;

            Debug.Write(ProcID.ToString() + "    Setting PropertyCondition UIAProcID." + Environment.NewLine);
            PropertyCondition UIAProcID = new PropertyCondition(
                AutomationElement.ProcessIdProperty, ProcID);

            Debug.Write(ProcID.ToString() + "    Setting AutomationElement Window." + Environment.NewLine);
            AutomationElement Window = waitForElement(desktop,TreeScope.Children, UIAProcID);

            AutomationElement sub = null;
            if (subWindowName != null)
            {
                Debug.Write(ProcID.ToString() + "    Setting PropertyCondition subWindow ." + Environment.NewLine);
                PropertyCondition subWindow = new PropertyCondition(
                    AutomationElement.NameProperty, subWindowName);

                Debug.Write(ProcID.ToString() + "    Setting the ActionElement sub to subWindow." + Environment.NewLine);
                sub = waitForElement(Window,TreeScope.Children, subWindow);
            }
            else
            {
                Debug.Write(ProcID.ToString() + "    No sub-window specified, Setting the ActionElement sub to Window." + Environment.NewLine);
                sub = Window;
            }

            Debug.Write(ProcID.ToString() + "    Setting PropertyCondition buttonName." + Environment.NewLine);
            PropertyCondition buttonName = new PropertyCondition(
                AutomationElement.NameProperty, ButtonName);

            Debug.Write(ProcID.ToString() + "    Setting the button element." + Environment.NewLine);
            //AutomationElement button = waitForElement(sub, TreeScope.Children, condition);
            AutomationElement button = waitForElement(sub, TreeScope.Children, buttonName);

            Debug.Write(ProcID.ToString() + "    Setting the button invocation action." + Environment.NewLine);
            InvokePattern doClick = (InvokePattern)button.GetCurrentPattern(InvokePattern.Pattern);


            Debug.Write(ProcID.ToString() + "    Invoking the button action (click)." + Environment.NewLine);
            System.Threading.ThreadStart invokeModal = new System.Threading.ThreadStart(doClick.Invoke);
            System.Threading.Thread modal = new System.Threading.Thread(invokeModal);
            modal.Start();

            Debug.Write(ProcID.ToString() + "    The button has been clicked." + Environment.NewLine);
            System.Threading.Thread.Sleep(25);
        }

        /// <summary>
        /// Enters text in a a main or subwindow combo entry box.  Use Inspect 
        /// or UISpy to get the main and sub window names.
        /// </summary>
        /// <param name="ProcID">The process ID of the target application</param>
        /// <param name="text">The text to be entered</param>
        /// <param name="subWindowName">[optional] The name of the subwindow</param>
        public static void enterText(int ProcID, string text, string subWindowName = null)
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
                Debug.Write("The element was not yet found, sleeping thread for 50ms..." + Environment.NewLine);
                Thread.Sleep(50);
            }

            AutomationElement resultElement = root.FindFirst(scope, condition);
            return resultElement;
        }

        #endregion

    }
}
