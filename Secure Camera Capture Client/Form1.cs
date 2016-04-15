using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Secure_Camera_Capture_Client
{
    public partial class Form1 : Form
    {
        private JsonObject jO;
        private String currentImageName;
        private bool TreeDrawn = false;
        private String G_URI;
        private String G_myParameters;
        private String mostRecentPictureName = "";
        private Image GlobalImage;
        private String GlobalKey;
        private TreeNode selectedNode;
        private List<TreeNode> treeNodeList = new List<TreeNode>();
        private string myLoginParameters;
        private string GLOBALIPADDRESS;

        public Form1()
        {
            InitializeComponent();
            GLOBALIPADDRESS = getIpFromFile(); //Also starts the other form:)

            treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            treeView1.DrawNode += new DrawTreeNodeEventHandler(treeView1_DrawNode);
            
        }

        public byte login(String username, String password)
        {
            //Start the login in script, getting all the data
            string URI = "https://" + GLOBALIPADDRESS + "/login.php";
            string myParameters = "username=" + username + "&password=" + password;
            myLoginParameters = myParameters;

            try {
                ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    string HtmlResult = wc.UploadString(URI, myParameters);
#if DEBUG
                    Console.WriteLine(HtmlResult);
#endif
                    if (HtmlResult.Substring(0, 1) == "0")
                    {
                        GlobalKey = HtmlResult.Substring(1, HtmlResult.Length - 1).Split(',')[0];
                        string jsonString = HtmlResult.Substring(GlobalKey.Length+2);// HtmlResult.Substring(1, HtmlResult.Length - 1);
                        JSONParser jsp = new JSONParser(jsonString);
                        jO = jsp.jO;
                        return 0;
                    }
                    else
                        return 1;
                }
            } catch
            {
                MessageBox.Show("Unable to connect to Remote Server", "Connection Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 2;
            }
        }

        public byte loginRefresh()
        {
            logout();
            //Start the login in script, getting all the data
            string URI = "https://" + GLOBALIPADDRESS + "/login.php";
            JSONParser jsp_1 = new JSONParser("");

            ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    string HtmlResult = wc.UploadString(URI, myLoginParameters);
#if DEBUG
                    Console.WriteLine(HtmlResult);
#endif
                    if (HtmlResult.Substring(0, 1) == "0")
                    {
                        treeView1 = new TreeView();
                        jO = null;
                        jO = jsp_1.jO;
                        GlobalKey = HtmlResult.Substring(1, HtmlResult.Length - 1).Split(',')[0];
                        string jsonString = HtmlResult.Substring(GlobalKey.Length + 2);// HtmlResult.Substring(1, HtmlResult.Length - 1);
                        JSONParser jsp = new JSONParser(jsonString);
                        jO = jsp.jO;
                        TreeDrawn = false;
                        return 0;
                    }
                    else
                        return 1;
                }
            }
            catch
            {
                MessageBox.Show("Unable to connect to Remote Server", "Connection Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 2;
            }
        }

        public byte registerAccount(String username, String password, String regNumber)
        {
            string URI = "https://"+ GLOBALIPADDRESS+"/login.php";
            string myParameters = "username=" + username + "&password=" + password + "&number=" + regNumber;

            ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    string HtmlResult = wc.UploadString(URI, myParameters);
#if DEBUG
                    Console.WriteLine(HtmlResult);
#endif
                    if (HtmlResult.Substring(0, 1) == "0")
                    {
                        return 0;
                    }
                    else
                        return 1;
                }
            }
            catch
            {
                MessageBox.Show("Unable to connect to Remote Server", "Connection Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 2;
            }
        }

        private Boolean logout( )
        {
            //Start the login in script, getting all the data
            string URI = "https://" + GLOBALIPADDRESS + "/logout.php";
            string myParameters = "key=" + GlobalKey;

            try
            {
                ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    string HtmlResult = wc.UploadString(URI, myParameters);
#if DEBUG
                    Console.WriteLine(HtmlResult);
#endif
                    if (HtmlResult.Substring(0, 1) == "0")
                    {
                        return false;
                    }
                    else
                    {   
                        MessageBox.Show("Unable to connect to Remote Server", "Connection Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                }
            }
            catch
            {               
                MessageBox.Show("Unable to connect to Remote Server", "Connection Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }

        private string getIpFromFile()
        {
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string iniFile = directory + "\\config.ini";
            if(File.Exists(iniFile))
            {
                string configFile = System.IO.File.ReadAllText(iniFile);
                string ip = Regex.Match(configFile, "(?=<ip>)(.*?)(?=</ip>)").ToString().Substring(4);

                Form2 subForm = new Form2(this);
                this.TopLevel = false;
                subForm.TopMost = true;
                subForm.Show();

                return ip;
            }
            else
            {
                Form5 ipForm = new Form5(this, false);
                this.TopLevel = false;
                ipForm.TopMost = true;
                ipForm.Show();
                return "";
            }
        }

        public void setIP(string IP, bool showLoginForm)
        {
            GLOBALIPADDRESS = IP;
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string iniFile = directory + "\\config.ini";
            System.IO.File.WriteAllText(iniFile, "<ip>" + IP + "</ip>");

            if (showLoginForm)
            {
                Form2 subForm = new Form2(this);
                this.TopLevel = false;
                subForm.TopMost = true;
                subForm.Show();
            }            
        }

        public static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (error == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }
#if DEBUG
            Console.WriteLine("X509Certificate [{0}] Policy Error: '{1}'",
                cert.Subject,
                error.ToString());
#endif
            return true;
        }

        public void getPicture(string pictureName)
        {
            if (pictureName == "") return;

            string URI = "https://" + GLOBALIPADDRESS + "/serve.php";
            string myParameters = "picture=" + pictureName  + "&type=0" + "&key=" + GlobalKey;
            //Set Gloabls
            G_URI = URI;
            G_myParameters = myParameters;
            if (pictureName != mostRecentPictureName)
            {
                mostRecentPictureName = pictureName;
                Form4 loading = new Form4(URI, myParameters, this);
                loading.downloadPicture();
                if (loading.ShowDialog(this) == DialogResult.OK)
                {
                    //do processing
                    this.Enabled = true;                    
                }
                else
                {
                    //do processing
                    MessageBox.Show("Unable to download picture", "Download Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Enabled = true;
                }
            }
        }

        public void setImage(Image i)
        {
            GlobalImage = i;
            pictureBox1.Image = GlobalImage;
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
                    getPicture(imageName);
                    
                } catch ( Exception ex )
                {
#if DEBUG
                    Console.WriteLine(ex.ToString());
#endif
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
            if (!TreeDrawn)
            {
                int yearCount = jO.year.Count();
                List<TreeNode> yearNodeList = new List<TreeNode>();
                for (int i = 0; i < yearCount; i++)
                {
                    var year = jO.year.ElementAt(i);
                    int monthCount = year.months.Count();
                    List<TreeNode> monthNodeList = new List<TreeNode>();
                    for (int j = 0; j < monthCount; j++)
                    {
                        var month = year.months.ElementAt(j);
                        int dayCount = month.days.Count();
                        List<TreeNode> dayNodeList = new List<TreeNode>();
                        for (int k = 0; k < dayCount; k++)
                        {
                            var day = month.days.ElementAt(k);
                            int hourCount = day.hours.Count();
                            List<TreeNode> hourNodeList = new List<TreeNode>();
                            for (int l = 0; l < hourCount; l++)
                            {
                                var hour = day.hours.ElementAt(l);
                                int imageCount = hour.images.Count();
                                List<TreeNode> imageNodeList = new List<TreeNode>();
                                for (int m = 0; m < imageCount; m++)
                                {
                                    TreeNode tn = new TreeNode(formatDateAndTime(hour.images.ElementAt(m).date_taken));
                                    tn.Tag = hour.images.ElementAt(m).file_name;
                                    imageNodeList.Add(tn);
                                    treeNodeList.Add(tn);
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
                for (int i = 0; i < yearNodeList.Count(); i++)
                {
                    treeView1.Nodes.Add(yearNodeList.ElementAt(i));
                }
                TreeDrawn = true;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView tv = (TreeView)sender;
            //tv.SelectedNode.BackColor = Color.DarkOrange;
            tv.HideSelection = false;
            selectedNode = e.Node;
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

        private void navigateLeft()
        {
            //Left Button
            //get index of current node
            if (treeNodeList.Count > 0)
            {
                int index = 0;
                for (int i = 0; i < treeNodeList.Count; i++)
                {
                    if (selectedNode == treeNodeList[i])
                    {
                        index = i;
                        break;
                    }
                }

                if (index != 0)
                {
                    treeNodeList[index].Toggle();
                    treeNodeList[index - 1].Toggle();
                    treeView1.SelectedNode = treeNodeList[index - 1];
                }
            }
        }

        private void navigateRight()
        {
            //Right button
            //get index of current node
            if (treeNodeList.Count > 0)
            {
                int index = 0;
                for (int i = 0; i < treeNodeList.Count; i++)
                {
                    if (selectedNode == treeNodeList[i])
                    {
                        index = i;
                        break;
                    }
                }

                if (index != treeNodeList.Count - 1)
                {
                    treeNodeList[index].Toggle();
                    treeNodeList[index + 1].Toggle();
                    treeView1.SelectedNode = treeNodeList[index + 1];
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
            {
                navigateLeft();
                return true;
            }
            if (keyData == Keys.Right)
            {
                navigateRight();
                return true;
            }
            // etc..
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (currentImageName != "" && GlobalImage != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Save Downloaded Image";
                sfd.Filter = "Image files (*.jpg)|*.jpg|All files (*.*)|*.*";
                sfd.FilterIndex = 1;
                sfd.RestoreDirectory = true;
                //sfd.FileName = currentImageName;
                sfd.FileName = Regex.Replace(currentImageName, @":+", "_").ToString();
                ImageFormat format = ImageFormat.Jpeg;

                Bitmap bp = new Bitmap(GlobalImage);
                Graphics g = Graphics.FromImage(bp);
                g.DrawImage(GlobalImage, new Point(0,0));
                g.Dispose();     

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    bp.Save(sfd.FileName, format);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if( logout() )
            {
                e.Cancel = true;
            }
    }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            navigateRight();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            navigateLeft();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            this.Enabled = false;
            byte ret = loginRefresh();
            if (ret == 0)
            {
                Cursor.Current = Cursors.Default;
                this.Enabled = true;
            }
            else if(ret == 1)
            {
                Cursor.Current = Cursors.Default;
                //Handle the error
                //TODO
                this.Enabled = true;
                MessageBox.Show("Unable to Refresh the List", "Refresh Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
            this.Enabled = true;
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 ipForm = new Form5(this, true);
            ipForm.Show();
        }
    }
}
