using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Secure_Camera_Capture_Client
{
    public partial class Form1 : Form
    {
        private JsonObject jO;
        private String currentImageName;

        public Form1()
        {
            //DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
            //jsonSerializer.ReadObject(response.GetResponseStream());

            JSONParser jsp = new JSONParser("");
            jO = jsp.jO;

            InitializeComponent();

            treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            treeView1.DrawNode += new DrawTreeNodeEventHandler(treeView1_DrawNode);

            Form2 subForm = new Form2(this);
            this.TopLevel = false;
            subForm.TopMost = true;
            subForm.Show();
        }

        public bool login(String username, String password)
        {
            //Start the login in script, getting all the data
            Console.WriteLine("U: " + username + " P: " + password);
            return true;
        }

        public bool registerAccount(String username, String password, String regNumber)
        {
            Console.WriteLine("U: " + username + " P: " + password + " R: " + regNumber);
            return true;
        }

        void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {

            if ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected)
            {
                Graphics g = e.Graphics;
                System.Drawing.Font treeFont = new System.Drawing.Font("Gill Sans MT", 14.0F, FontStyle.Regular);
                Color nodeForeColor = Color.FromArgb(49, 49, 49);//GetTreeNodeForeColor(e.Node, e.State);

                // fill node background
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(105, 105, 105)))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }

                // draw node text
                TextRenderer.DrawText(e.Graphics, e.Node.Text, treeFont, e.Bounds, nodeForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);

                //Thread out the download of the images.
                String imageName = (String)e.Node.Tag;
                currentImageName = Regex.Replace(e.Node.Text, @"/", "_").ToString();
                currentImageName = Regex.Replace(currentImageName, @"\s+", "-").ToString();
                try
                {
                    string ImagesDirectory =
                        Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "img");
                    pictureBox1.Image = Image.FromFile(ImagesDirectory + "\\" + imageName);
                    
                } catch
                {
                    //pictureBox1.Image = Image.FromFile(ImagesDirectory + "\\" + imageName);
                }
                
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private Font GetTreeNodeFont(TreeNode node)
        {
            Font nodeFont = node.NodeFont;
            if (nodeFont == null)
            {
                nodeFont = this.Font;
            }
            return nodeFont;
        }

        private Color GetTreeNodeForeColor(TreeNode node, TreeNodeStates nodeState)
        {
            Color nodeForeColor = Color.Empty;

            if ((nodeState & TreeNodeStates.Selected) == TreeNodeStates.Selected)
            {
                nodeForeColor = Color.FromKnownColor(KnownColor.HighlightText);
            }
            else
            {
                nodeForeColor = node.ForeColor;
                if (nodeForeColor == Color.Empty)
                {
                    nodeForeColor = this.ForeColor;
                }
            }

            return nodeForeColor;
        }

private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            int yearCount = jO.year.Count();
            List<TreeNode> yearNodeList = new List<TreeNode>();
            for (int i = 0; i < yearCount; i++)
            {
                var year = jO.year.ElementAt(i);
                int monthCount = year.months.Count();
                List<TreeNode> monthNodeList = new List<TreeNode>();
                for(int j = 0; j < monthCount; j++)
                {
                    var month = year.months.ElementAt(j);
                    int dayCount = month.days.Count();
                    List<TreeNode> dayNodeList = new List<TreeNode>();
                    for ( int k = 0; k < dayCount; k++)
                    {
                        var day = month.days.ElementAt(k);
                        int hourCount = day.hours.Count();
                        List<TreeNode> hourNodeList = new List<TreeNode>();
                        for ( int l = 0; l < hourCount; l++)
                        {
                            var hour = day.hours.ElementAt(l);
                            int imageCount = hour.images.Count();
                            List<TreeNode> imageNodeList = new List<TreeNode>();
                            for (int m = 0; m < imageCount; m++)
                            {
                                TreeNode tn = new TreeNode(formatDateAndTime(hour.images.ElementAt(m).date_taken));
                                tn.Tag = hour.images.ElementAt(m).file_name;
                                imageNodeList.Add(tn);
                            }
                            TreeNode h = new TreeNode(hour.hour.ToString(), imageNodeList.ToArray());
                            hourNodeList.Add(h);
                        }
                        TreeNode d = new TreeNode(day.day_name.ToString(), hourNodeList.ToArray());
                        dayNodeList.Add(d);
                    }
                    TreeNode mn = new TreeNode(getMonthString(month.month_name), dayNodeList.ToArray());
                    monthNodeList.Add(mn);
                }
                TreeNode y = new TreeNode(year.year_name.ToString(), monthNodeList.ToArray());
                yearNodeList.Add(y);
            }
            for(int i = 0; i < yearNodeList.Count(); i++)
            {
                treeView1.Nodes.Add(yearNodeList.ElementAt(i));
            }
            /*
            //
            // This is the first node in the view.
            //
            TreeNode treeNode = new TreeNode("2013");
            treeView1.Nodes.Add(treeNode);
            //
            // Another node following the first node.
            //
            treeNode = new TreeNode("2014");
            treeView1.Nodes.Add(treeNode);
            //
            // Create two child nodes and put them in an array.
            // ... Add the third node, and specify these as its children.
            //
            TreeNode node2 = new TreeNode("January");
            TreeNode node3 = new TreeNode("Febuary");
            TreeNode[] array = new TreeNode[] { node2, node3 };
            //
            // Final node.
            //
            treeNode = new TreeNode("2015", array);
            treeView1.Nodes.Add(treeNode);
            */
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView tv = (TreeView)sender;
            //tv.SelectedNode.BackColor = Color.DarkOrange;
            tv.HideSelection = false;
        }

        public string formatDateAndTime(string timeStamp)
        {
            //Example
            //2016 01    26  01   32
            //Year Month Day Hour Min

            //TODO An option in settings could be 12/24 time
            string year = timeStamp.Substring(0,4);
            string month = timeStamp.Substring(4, 2);
            string day = timeStamp.Substring(6, 2);
            string hour = timeStamp.Substring(8, 2);
            string min = timeStamp.Substring(10, 2);

            return hour + ":" + min + "  " + month + "/" + day + "/" + year;
        }

        public string getMonthString(int month)
        {
            switch (month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return "";
            }

        }

        private void pictureBox1_Click(object sender, System.EventArgs e)
        {
            //Generate some dialog about the image?
            //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, System.EventArgs e)
        {

        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Title = "Save Downloaded Image";
            saveFileDialog1.Filter = "Image files (*.jpg)|*.jpg|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = currentImageName;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    myStream.Close();
                }
            }
        }
    }
}
