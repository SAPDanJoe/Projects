using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ProcessRestarter
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string configFilePath = Environment.GetEnvironmentVariable("APPDATA") + @"\SAPIT\ProcessRestarter\config.txt";

            if ( ! System.IO.File.Exists( configFilePath ))
            {
                this.EventLog.WriteEntry(
                    "The service {" 
                    + this.ServiceName + 
                    "}failed to start: No configuration file exists in {"
                    + configFilePath + 
                    "}.  Please check the file and permissions and try again.");
                this.Stop();
            }
            string config = System.IO.File.ReadLines(configFilePath).ToString();
            ProcessRestarter.Properties.Settings.Default.processName = config.Split(':')[0].ToString();
            ProcessRestarter.Properties.Settings.Default.processEXE = config.Split(':')[1].ToString();
        }

        protected override void OnStop()
        {
        }
    }
}
