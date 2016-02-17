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
            /*if (this.textBox1.Text != string.Empty)
            {

                mainForm.listBox1.Items.Clear();

                string[] stringsEntered = textBox1.Lines;

                for (int count = 0; count < stringsEntered.Length; count++)
                {

                    mainForm.listBox1.Items.Add(stringsEntered[count]);

                }

            }*/

            mainForm.TopLevel = true;
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
