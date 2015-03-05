using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

using System.Diagnostics;
using System.Windows.Automation;

namespace ScratchPad
{
    class Program
    {
        static void Main(string[] args)
        {
            XDocument myDoc = XMLStore("configFile");

            myDoc = XMLStore("name", "value", myDoc);

        }

        #region Hierarchal XML

        public static XDocument createXML(string rootName)
        {
            XDocument doc = new XDocument();
            doc.Add(new XElement(rootName));
            return doc;
        }

        public static XDocument XMLStore(string name, string value = null, XDocument doc = null, string parent = "configFile", bool attrib = false)
        {
            if (value == null)
            {
                doc = new XDocument();
                doc.Add(new XElement(name));
            }
            else if (attrib)
            {
                doc.XPathSelectElement(parent).Add(new XAttribute(name, value));
            }
            else
            {
                doc.XPathSelectElement(parent).Add(new XElement(name, value));
            }

            return doc;
        }


        #endregion

        #region File Cryptogrophy
        static void crypt()
        {
            //using System.Security.Cryptography;
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            RSAParameters keyPair = provider.ExportParameters(true);
            string keys = provider.ToXmlString(true);
            int i = 0;
        }
        #endregion

        #region UIAutomation testing

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


