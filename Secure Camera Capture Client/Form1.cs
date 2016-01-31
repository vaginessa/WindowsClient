using System.Drawing;
using System.Windows.Forms;

namespace Secure_Camera_Capture_Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
            //jsonSerializer.ReadObject(response.GetResponseStream());

            JSONParser jsp = new JSONParser("");

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
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView tv = (TreeView)sender;
            //tv.SelectedNode.BackColor = Color.DarkOrange;
            tv.HideSelection = false;
        }
    }
}
