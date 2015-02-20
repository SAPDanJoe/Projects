using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ITdirect_Processor_Toolkit
{
    public partial class mainForm : Form
    {
        //static strings for building the link
        public static string baseURL = "https://itdirect.wdf.sap.corp/sap/bc/bsp/sap/crm_ui_start/default.htm?saprole=ZSOLMANPRO&crm-object-type=AIC_OB_INCIDENT&crm-object-action=D&PROCESS_TYPE=Z";
        public static string categoryHeader = "&CAT_ID=";
        public static string descriptionHeader = "&DESCRIPTION=";

        public mainForm()
        {
            InitializeComponent();}

        private void buttonHandler(object sender, EventArgs e)
        {
            //cast object as a button
            Button clickedButton = (Button)sender;

            //create string to store the link as we build it
            string link = baseURL;

            //create some strings to store the various parts of the link
            string ticketType = string.Empty;
            string ticketCategory = string.Empty;
            string ticketDescription = string.Empty;

            //determine the ticket type by seaqrching the button text
            if (clickedButton.Text.Contains("IMIS"))
            {
                ticketType = "INC";
            }
            else if (clickedButton.Text.Contains("SRIS"))
            {
                ticketType = "SER";
            }
            else
            {
                throw new Exception("Something went wrong building the link: Could not parse the ticketType from " + clickedButton.Text);
            }

            //set the ticket category = to the button's text
            ticketCategory = clickedButton.Text;

            //set the description to begin with '[Walk-In]'
            //note that %5B = '[' and %5D = ']'
            ticketDescription = @"%5BWalk-In%5D";
            if (AcademyButton.Checked)
            {
                ticketDescription += @"%20%5BAcademy%5D";
            }

            //build the link
            link = link + ticketType + categoryHeader + ticketCategory + descriptionHeader + ticketDescription;

            //go to the link
            System.Diagnostics.Process linkFollower = new System.Diagnostics.Process();
            linkFollower.StartInfo.Arguments = link;
            linkFollower.StartInfo.FileName = "IExplore.exe";
            linkFollower.Start();
        }
    }
}
