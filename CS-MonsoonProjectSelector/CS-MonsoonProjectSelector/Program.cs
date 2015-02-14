using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
