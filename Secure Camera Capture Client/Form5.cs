using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Secure_Camera_Capture_Client
{
    public partial class Form5 : Form
    {
        private Form1 mainForm;
        private bool showLoginForm;
        private bool deleteFileIfExist;
        public Form5(Form1 mainForm, bool deleteFileIfExist)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            showLoginForm = true;
            this.deleteFileIfExist = deleteFileIfExist;
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress testIP;
            if (IPAddress.TryParse(ipTextBox.Text, out testIP))
            {
                if (deleteFileIfExist)
                {
                    string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    string iniFile = directory + "\\config.ini";

                    if (File.Exists(iniFile))
                    {
                        File.Delete(iniFile);
                        showLoginForm = false;
                    }
                }

                this.mainForm.setIP(ipTextBox.Text, showLoginForm);
                if(!showLoginForm) { mainForm.loginRefresh(); }
                this.Close();
            } 
            else
            {
                MessageBox.Show("Format of IP Address Incorrect", "Incorrect IP Address",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }           
        }
    }
}
