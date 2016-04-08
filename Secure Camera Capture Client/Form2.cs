using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Secure_Camera_Capture_Client
{
    public partial class Form2 : Form
    {
        Form1 mainForm;
        bool normalClose = false;

        public Form2(Form1 mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.userNameBox.Text != string.Empty && this.passwordBox.Text != string.Empty )
            {
                string username = userNameBox.Lines[0];
                string password = passwordBox.Lines[0];

                Cursor.Current = Cursors.WaitCursor;
                this.Enabled = false;
                byte ret = mainForm.login(username, password);
                if (ret==0)
                {
                    Cursor.Current = Cursors.Default;
                    mainForm.TopLevel = true;
                    normalClose = true;
                    this.Close();
                } else if(ret == 0)
                {
                    Cursor.Current = Cursors.Default;
                    //Handle the error
                    //TODO
                    MessageBox.Show("Username or password incorrect", "Login Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Enabled = true;
                }
                Cursor.Current = Cursors.Default;
                this.Enabled = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 subForm = new Form3(mainForm);
            //Could split this into to if's 
            //TODO
            if (this.userNameBox.Text != string.Empty && this.passwordBox.Text != string.Empty)
            {
                string username = userNameBox.Lines[0];
                string password = passwordBox.Lines[0];
                subForm.inputData(username, password);
            }
            normalClose = true;
            this.Close();
            subForm.Show();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!normalClose) mainForm.Close();
        }
    }
}
