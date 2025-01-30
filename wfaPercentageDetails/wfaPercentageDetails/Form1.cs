using libClass;
using libClassDraws;
using System.Windows.Forms;


namespace wfaPercentageDetails
{
    public partial class Form1 : Form
    {
        private TreeNode rootNode;
        private double ei;
        private double es;
        private double x_htrih;
        private double sigma;
        private CMyDraw graph = new CMyDraw();
        private int h;
        private int w;
        private bool pflag_scale=true;
        private bool shiftB = false;
        private bool ctrl = true;
        Color color;
        public Form1()
        {
            InitializeComponent();
            FellTree();
            this.SizeChanged += Form1_SizeChanged;
            treeView1.KeyDown += Form1_KeyDown;
            buAdd.Click += InitGraph;
            buClear.Click += ClearGraph;
            treeView1.NodeMouseClick += NodeMouseClick;
            pictureBox1.MouseWheel += PictureBox1_MouseWheel;
            h = pictureBox1.Height;
            w = pictureBox1.Width;
            //graph.OnBitmapChakgd += Graph_OnBitmapChakgd;
            //graph.OnTextChakgd += Graph_OnTextChakgd;
        }

        private void PictureBox1_MouseWheel(object? sender, MouseEventArgs e)
        {
            if(shiftB)
            {
                graph.SetShift(e.Delta);
                Draw();
            }
            if(ctrl)
            {
                graph.SetScale(e.Delta);
                Draw();
            }
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    shiftB = false;
                    ctrl = true;
                    break;
                case Keys.S:
                    shiftB = true;
                    ctrl = false;
                    break;
            }
        }


        private void Form1_SizeChanged(object? sender, EventArgs e)
        {
            h = pictureBox1.Height;
            w = pictureBox1.Width;
            Draw();
        }

        private void Draw()
        {
            graph.DrawGraph(ref w, ref h);
            pictureBox1.Image = (Bitmap)graph.Bitmap.Clone();
            pictureBox1.Invalidate();
            textBox.Text = graph.ReturnStr();
        }

        private void ClearGraph(object? sender, EventArgs e)
        {
            rootNode.Nodes.Clear();
            graph.ClearGraph();
            Draw();
            graph.SetFlag(true);
            pflag_scale=true;
        }

        private void NodeMouseClick(object? sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // ���������, �������� �� ���� ��������
                if (e.Node.Parent != null)
                {
                    graph.Flag�hange(e.Node.Index);
                    Draw();
                    //MessageBox.Show($"�� ������ �� �������� ����: {e.Node.Text} { e.Node.Index}", "����������");
                }
            }
        }

        public void FellTree()
        {
            rootNode = new TreeNode("�������: ");
            treeView1.Nodes.Add(rootNode);

            // ���������� �������� ����
            rootNode.Expand();
        }

        public void AddTree(double pei, double pes, double px_htrih, double psigma)
        {
            TreeNode childNode = new TreeNode($"������: {pei}x{pes}x{px_htrih}x{psigma}");
            rootNode.Nodes.Add(childNode);
            rootNode.Expand();

        }

        private void InitGraph(object? sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Edei.Text) || !string.IsNullOrWhiteSpace(Edes.Text) || !string.IsNullOrWhiteSpace(Edx_htrih.Text) || !string.IsNullOrWhiteSpace(Edsigma.Text))
            {
                ei = Convert.ToDouble(Edei.Text);
                es = Convert.ToDouble(Edes.Text);
                x_htrih = Convert.ToDouble(Edx_htrih.Text);
                sigma = Convert.ToDouble(Edsigma.Text);
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    // �������� ��������� ����
                    color = colorDialog1.Color;
                    AddTree(ei, es, x_htrih, sigma);
                    graph.AddGraph(ei, es, x_htrih, sigma, color);
                    Draw();
                    if(pflag_scale)
                    {
                        graph.SetFlag(false);
                        pflag_scale = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("���� ������������� ����!");
            }
        }
    }
}
