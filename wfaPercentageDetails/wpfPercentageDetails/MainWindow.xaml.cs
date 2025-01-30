using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using libClassDraws;

namespace wpfPercentageDetails
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TreeNode rootNode;
        private double ei;
        private double es;
        private double x_htrih;
        private double sigma;
        private CMyDraw graph = new CMyDraw();
        private int h;
        private int w;
        private bool pflag_scale = true;
        private bool shiftB = false;
        private bool ctrl = true;
        System.Drawing.Color color;
        public MainWindow()
        {
            InitializeComponent();
            FellTree();
            this.SizeChanged += Form1_SizeChanged;
            treeView1.KeyDown += Form1_KeyDown;
            buAdd.Click += InitGraph;
            buClear.Click += ClearGraph;
            treeView1.NodeMouseClick += NodeMouseClick;
            pictureBox1.MouseWheel += PictureBox1_MouseWheel;
            h = (int)pictureBox1.Height;
            w = (int)pictureBox1.Width;
        }
        private void PictureBox1_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (shiftB)
            {
                graph.SetShift(e.Delta);
                Draw();
            }
            if (ctrl)
            {
                graph.SetScale(e.Delta);
                Draw();
            }
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.)
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
            pflag_scale = true;
        }

        private void NodeMouseClick(object? sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Проверяем, является ли узел дочерним
                if (e.Node.Parent != null)
                {
                    graph.FlagСhange(e.Node.Index);
                    Draw();
                    //MessageBox.Show($"Вы нажали на дочерний узел: {e.Node.Text} { e.Node.Index}", "Информация");
                }
            }
        }

        public void FellTree()
        {
            rootNode = new TreeNode("Графики: ");
            treeView1.Nodes.Add(rootNode);

            // Раскрываем корневой узел
            rootNode.Expand();
        }

        public void AddTree(double pei, double pes, double px_htrih, double psigma)
        {
            TreeNode childNode = new TreeNode($"График: {pei}x{pes}x{px_htrih}x{psigma}");
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
                    // Получаем выбранный цвет
                    color = colorDialog1.Color;
                    AddTree(ei, es, x_htrih, sigma);
                    graph.AddGraph(ei, es, x_htrih, sigma, color);
                    Draw();
                    if (pflag_scale)
                    {
                        graph.SetFlag(false);
                        pflag_scale = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Есть незаполненное поле!");
            }
        }
    }
}