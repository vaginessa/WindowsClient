﻿using System;
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
    public partial class Form3 : Form
    {
        Form1 mainForm;
        bool normalClose = false;

        public Form3(Form1 mainForm)
        {
           this.mainForm = mainForm;
            InitializeComponent();
        }

        public void inputData(string username, string password)
        {
            this.userNameBox.Text = username;
            this.passwordBox.Text = password;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void passwordBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.userNameBox.Text != string.Empty && this.passwordBox.Text != string.Empty && this.registrationNumberBox.Text != string.Empty)
            {
                string username = userNameBox.Lines[0];
                string password = passwordBox.Lines[0];
                string regNumber = registrationNumberBox.Lines[0];

                Cursor.Current = Cursors.WaitCursor;
                this.Enabled = false;
                byte ret = mainForm.registerAccount("user", "password", regNumber);
                if (ret == 0)
                {
                    ret = mainForm.registerAccount(username, password, regNumber);
                    if (ret == 0)
                    {
                        ret = mainForm.login(username, password);
                        if (ret == 0)
                        {
                            Cursor.Current = Cursors.Default;
                            mainForm.TopLevel = true;
                            normalClose = true;
                            this.Close();
                        }
                        else if(ret == 1)
                        {
                            Cursor.Current = Cursors.Default;
                            //Error, loging in the second time
                            MessageBox.Show("Unable to login with new credentials", "Login Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Enabled = true;
                        }
                    }
                    else if(ret == 1)
                    {
                        Cursor.Current = Cursors.Default;
                        //Error, loging in the second time
                        MessageBox.Show("Unable to create new account", "Account Creation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
                    }
                }
                else if(ret == 1)
                {
                    Cursor.Current = Cursors.Default;
                    //Error, registering accound.
                    //How to handle?
                    //TODO
                    MessageBox.Show("Registration number incorrect", "Account Creation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Enabled = true;
                }
                Cursor.Current = Cursors.Default;
                this.Enabled = true;
            } else
            {
                MessageBox.Show("Please Enter a Valid Username, Password, and Registration Number", "Not Valid Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
           if(!normalClose) mainForm.Close();
        }
   }
}
