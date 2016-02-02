using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Secure_Camera_Capture_Client
{
    public partial class Form1 : Form
    {
        private JsonObject jO;
        public Form1()
        {
            //DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
            //jsonSerializer.ReadObject(response.GetResponseStream());

            JSONParser jsp = new JSONParser("");
            jO = jsp.jO;

            InitializeComponent();

            treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            treeView1.DrawNode += new DrawTreeNodeEventHandler(treeView1_DrawNode);
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
                                TreeNode tn = new TreeNode(hour.images.ElementAt(m).date_taken);
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
    }
}
