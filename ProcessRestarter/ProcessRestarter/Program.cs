using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ProcessRestarter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Service1() 
            };
            ServiceBase.Run(ServicesToRun);

            string processName = ProcessRestarter.Properties.Settings.Default.processName;
            string processEXE = ProcessRestarter.Properties.Settings.Default.processEXE;
            Process[] namedProcesses = Process.GetProcessesByName(processName);
            
            if (namedProcesses.Count() == 0)
            {
                Program.EventLog.WriteEntry(
                    "The process named {" + processName + 
                    "} was not found to be running at {" + DateTime.Now + 
                    "}.  Attempting to restart the process from {" + processEXE + "}");
                Process.Start(processEXE);
            }

        }
    }
}
