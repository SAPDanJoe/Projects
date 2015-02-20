using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public static System.IO.FileInfo settings = initializeConfig();
       
        public static System.IO.FileInfo initializeConfig ()
        {   
            //Check if there is an existing config file
            //if not, create one
            //return the file as a fileinfo object

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
                generateNewConfig(config);
            }
            else if (!config.Exists)
            {   //directory is present, but file is not, create the file
                generateNewConfig(config);
            }
            return config;
        }

        public static void generateNewConfig(System.IO.FileInfo configFile)
        {
            //This generates a new empty XML settings file, and 
            //logs the date/time when the file was initialized.
           
            System.Xml.Linq.XDocument xconfig = new System.Xml.Linq.XDocument(
                new System.Xml.Linq.XElement("configFile",
                    new System.Xml.Linq.XAttribute("created",
                        System.DateTime.Now.ToString()
                    ),
                    new System.Xml.Linq.XElement("settings",
                        new System.Xml.Linq.XAttribute("id","current"),
                        new System.Xml.Linq.XAttribute("created",System.DateTime.Now.ToString())
                    )
                )
            );

            xconfig.Save(configFile.ToString());
        }

        public static void loadSettings(XElement config, EnvironmentVariableTarget mode)
        {
            //this will load the settings based on the provided mode, and launch a command window
            //note that some of these settings are environment variables, and some are files

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
            System.Diagnostics.Process.Start("git","config --global color.ui true ");
            System.Diagnostics.Process.Start("git","config --global http.sslVerify false");

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
            
            //Prep the multiline XML value
            string lines = (string)config.Element("GEMSourcesTextBox").ToString();
            lines = lines.Substring(19);
            lines = lines.Substring(0,lines.Length - 20);
            string[] linesArray = lines.Split(new string[] {"\r\n","\n"},StringSplitOptions.None);
            string newLines = string.Empty;

            for (int i = 0; i < linesArray.Length; i++)
            {
                if (linesArray[i] != string.Empty)
                {
                    newLines = newLines + "- " + linesArray[i] + Environment.NewLine;                                       
                }
            }

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

        public static void addEnv(string value, EnvironmentVariableTarget scope = EnvironmentVariableTarget.Process, string order = "end")
        {   /// <summary>
            /// Adds a value to the PATH using the provided:
            /// [string]value {The value to add to the path}
            /// [EnvironmentVariableTarget]scope {*Process, User, Machine}
            /// [string]order {beginning, *end}
            /// *defaults
            /// </summanry>
                      
            
            //Adds a vaue to the system path
            //get current contents of the PATH
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

        public static void addEnv(string varName, string value, EnvironmentVariableTarget scope)
        {   /// <summary>
            /// Adds an env Variable using the provided:
            /// [string]name {The name of the var to add}
            /// [string]value {The variable value to add}
            /// [EnvironmentVariableTarget]scope {Process, User, Machine}
            /// NOTE: this will override a previous variable in the given scope
            /// </summanry>
            Environment.SetEnvironmentVariable(varName, value, scope);
        }

    }
}
