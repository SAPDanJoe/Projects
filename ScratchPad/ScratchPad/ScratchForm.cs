using System;
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

namespace ScratchPad
{
    public partial class ScratchForm : Form
    {
        public ScratchForm()
        {
            InitializeComponent();
            System.Xml.Linq.XDocument xdoc = System.Xml.Linq.XDocument.Load(Environment.GetEnvironmentVariable("USERPROFILE") + @"\downloads\Scratch.xml");
            
            Binding data = new Binding("AWS_Access_KEYTextBox",xdoc.XPathSelectElement("configFile/settings[@current = '1']/monsoon/AWS_Access_KEY"),"Text");

        }
    }
}
