using libClass;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace libClassDraws
{
    public class CMyDraw
    {
        private CNormalDistributionFunction NDF;
        public delegate void CMyEvent(Bitmap newValua);
        public delegate void CMyEvent2(string newValua);
        public event CMyEvent OnBitmapChakgd;
        public event CMyEvent2 OnTextChakgd;
        private int Width;
        private int Height;
        private List<CStruct> book_Graph;
        private string str;
        private string str2;
        private double scale;
        private int shift;
        private int delta = 9;
        private Bitmap b;
        private Bitmap b2;
        private Graphics g;
        public Bitmap Bitmap 
        {
            get {  return b; }
        }
        public string Str
        {
            get => str2;
            set => OnTextChakgd?.Invoke(str2 = value);
        }
        public CMyDraw()
        {
            NDF = new CNormalDistributionFunction();
            book_Graph = new List<CStruct>();
        }
        public void AddGraph(double p_ei, double p_es, double px_htrih, double psigma, Color pcolor)
        {
            book_Graph.Add(new CStruct(ref p_ei, ref p_es, ref px_htrih, ref psigma, ref pcolor));
        }
        public void FlagСhange(int i)
        {
            book_Graph[i].flag = !book_Graph[i].flag;
        }
        public void ClearGraph()
        {
            book_Graph.Clear();
        }
        public string ReturnStr()
        {
            return str;
        }
        public void DrawGraph(ref int pWidth, ref int pHeight)
        {
            Width = pWidth; 
            Height = pHeight;
            b = new Bitmap(Width, Height);
            g = Graphics.FromImage(b);
            str = "";
            NDF.BuildGraph(ref book_Graph, ref pWidth, ref pHeight);
            for (int i = 0; i < book_Graph.Count; i++)
            {
                book_Graph[i].str = "";
                book_Graph[i].str += CNormalDistributionFunction.Percent_good_dutaley(
                    book_Graph[i].ei, book_Graph[i].es, book_Graph[i].x_htrih, book_Graph[i].sigma);
                book_Graph[i].str += CNormalDistributionFunction.Percent_fail_dutaley(
                    book_Graph[i].ei, book_Graph[i].es, book_Graph[i].x_htrih, book_Graph[i].sigma);
                book_Graph[i].str += CNormalDistributionFunction.Percent_fixable_dutaley(
                    book_Graph[i].ei, book_Graph[i].es, book_Graph[i].x_htrih, book_Graph[i].sigma);
                if (book_Graph[i].flag)
                {
                    str += $"График: {book_Graph[i].ei}x{book_Graph[i].es}x{book_Graph[i].x_htrih}x{book_Graph[i].sigma}" + "\n" + book_Graph[i].str + "\n";
                    for (int j = 1; j < book_Graph[i].points_graph.Count; j++)
                    {
                        g.DrawLine(new Pen(book_Graph[i].color), book_Graph[i].points_graph[j - 1].x, book_Graph[i].points_graph[j - 1].y, book_Graph[i].points_graph[j].x, book_Graph[i].points_graph[j].y);
                    }
                    Drawing_selected_area(ref book_Graph[i].points_fail_dutaley, ref book_Graph[i].color, true, Height);
                    Drawing_selected_area(ref book_Graph[i].points_fixable_dutaley, ref book_Graph[i].color, false, Height);
                    Draw_axis(ref book_Graph[i].mas_ei, ref book_Graph[i].color, "ei");
                    Draw_axis(ref book_Graph[i].mas_es, ref book_Graph[i].color, "es");
                    Draw_axis(ref book_Graph[i].mas_y, ref book_Graph[i].color, "y");
                    Draw_axis(ref book_Graph[i].mas_x_htrih, ref book_Graph[i].color, "x_htrih");
                }
            }
            //Str = str;
        }

        private void Draw_axis(ref CPointGraph[]mas, ref Color color, string pstr)
        {
            if (mas[0].x > delta && mas[0].x < Width - delta)
            {
                g.DrawString(pstr, new Font("Arial", 16), new SolidBrush(Color.Black), new Point(mas[0].x, Height / 2));
                Drawing_line(ref mas, color);
            }
        }

        public void SetShift(double p_shift)
        {
            NDF.SetShift(p_shift);
        }
        public void SetScale(double p_scale)
        {
            NDF.SetScale(p_scale);
        }
        public void SetFlag(bool flag)
        {
            NDF.flag_scale = flag;
        }
        private void Drawing_selected_area(ref List<CPointGraph> book, ref Color color, bool flag, int h)
        {
            if (book.Count() != 0)
            {
                int y = 0;
                int count = 0;
                if (flag)
                {
                    if (book[0].y / h * 100 < 95.0)
                    {
                        y = 1;
                    }
                }
                {
                    if (book[book.Count() - 1].y / h * 100 < 95.0)
                    {
                        y = 1;
                    }
                }
                count = book.Count() + 1 + y;
                using (Brush brush = new HatchBrush(HatchStyle.ForwardDiagonal, color, Color.Transparent))
                {
                    Point[] points = new Point[count];
                    for (int j = 0; j < book.Count() + 1; j++)
                    {
                        if (j == book.Count())
                        {
                            if (flag)
                            {
                                if (y == 1)
                                {
                                    points[j] = new Point(book[book.Count() - 1].x, h);
                                    points[j + 1] = new Point(book[0].x, h);
                                }
                                else
                                {
                                    points[j] = new Point(book[book.Count() - 1].x, book[0].y);
                                }
                            }
                            else
                            {
                                if (y == 1)
                                {
                                    points[j] = new Point(book[book.Count() - 1].x, h);
                                    points[j + 1] = new Point(book[0].x, h);
                                }
                                else
                                {
                                    points[j] = new Point(book[0].x, book[book.Count() - 1].y);
                                }

                            }
                        }
                        else
                        {
                            points[j] = new Point(book[j].x, book[j].y);
                        }
                    }
                    g.FillPolygon(brush, points);
                }
            }
        }
        private void Drawing_line( ref CPointGraph[] mas, Color color)
        {
            g.DrawLine(new Pen(color), mas[0].x, mas[0].y, mas[1].x, mas[1].y);
        }
    }
}
