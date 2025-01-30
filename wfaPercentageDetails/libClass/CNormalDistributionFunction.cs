using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace libClass
{
    public class CNormalDistributionFunction
    {
        public CNormalDistributionFunction() { }
        private double ei { get; set; }
        private double es { get; set; }
        private double x_htrih { get; set; }
        double sigma { get; set; }
        private double scale;
        private const double e = 2.71828 ;
        private const double pi = 3.14 ;
        private int shift;
        public bool flag_scale = true;
        public void BuildGraph(ref List<CStruct> book_Graph, ref int w, ref int h)
        {
            for (int i = 0 ; i < book_Graph.Count; i++)
            { 
                book_Graph[i].points_graph.Clear();
                book_Graph[i].points_fixable_dutaley.Clear();
                book_Graph[i].points_fail_dutaley.Clear();
                ei = book_Graph[i].ei * 1000;
                es = book_Graph[i].es * 1000;
                x_htrih = book_Graph[i].x_htrih * 1000;
                sigma = book_Graph[i].sigma * 1000;
                if (i == 0 && flag_scale)
                {
                    scale = w / (6 * sigma);
                    flag_scale = false;
                    shift = ((int)((x_htrih - 3 * sigma) * scale));
                }
                x_htrih *= scale;
                ei *= scale;
                es *= scale;
                sigma *= scale;

                Integral(ref book_Graph, ref i, ref w, ref h);
            }
        }
        public static string Percent_fixable_dutaley(double p_ei, double p_es, double p_x_htrih, double p_sigma)
        {
            string str1;
            string str = "Процент исправимого брака: ";
            double t1 = (p_es - p_x_htrih) / p_sigma;
            double t2 = (p_x_htrih + 3 * p_sigma - p_x_htrih) / p_sigma;
            double k2 = (t2 > 0 ? 1 : -1);
            double k1 = (t1 > 0 ? 1 : -1);
            t2 *= k2;
            t1 *= k1;
            double a = Integral_percent(ref t2) * k2 - Integral_percent(ref t1) * k1;
            str1 = (a * 100).ToString();
            str += str1 + "\n";
            return str;
        }
        public static string Percent_fail_dutaley(double p_ei, double p_es, double p_x_htrih, double p_sigma)
        {
            string str1;
            string str = "Процент неисправимого брака: ";
            double t1 = (p_x_htrih - 3 * p_sigma - p_x_htrih) / p_sigma;
            double t2 = (p_ei - p_x_htrih) / p_sigma;
            double k2 = (t2 > 0 ? 1 : -1);
            double k1 = (t1 > 0 ? 1 : -1);
            t2 *= k2;
            t1 *= k1;
            double a = Integral_percent(ref t2) * k2 - Integral_percent(ref t1) * k1;
            str1 = (a * 100).ToString();
            str += str1 + "\n";
            return str;
        }
        public static string Percent_good_dutaley(double p_ei, double p_es, double p_x_htrih, double p_sigma)
        {
            string str1;
            string str = "Процент годных деталей: ";
            double t1 = (p_ei - p_x_htrih) / p_sigma;
            double t2 = (p_es - p_x_htrih) / p_sigma;
            double k2 = (t2 > 0 ? 1 : -1);
            double k1 = (t1 > 0 ? 1 : -1);
            t2 *= k2;
            t1 *= k1;
            double a = Integral_percent(ref t2) * k2 - Integral_percent(ref t1) * k1;
            str1 = (a * 100).ToString();
            str += str1 + "\n";
            return str;
        }
        private void Integral(ref List<CStruct> book_Graph, ref int z, ref int w, ref int h)
        {
            int buf = 0;
            for (int j= 0 ; j <= w; j++)
	        {
                double fi = 0.0;
                int p;
                fi += Math.Pow(e, -Math.Pow(((j + shift) - x_htrih), 2) / (2 * Math.Pow(sigma, 2)));
                fi *= 1 / (sigma * Math.Sqrt(2 * pi)) + h /** 0.95*/ ;
                fi *= -1;
                fi += h;
                p = (int)fi;
                book_Graph[z].points_graph.Add(new CPointGraph(ref j, ref p));
                if (j + shift <= ei)
                {
                    book_Graph[z].points_fail_dutaley.Add(new CPointGraph(ref j, ref p));
                }
                if (j + shift >= es)
                {
                    book_Graph[z].points_fixable_dutaley.Add(new CPointGraph(ref j, ref p));
                }
                if (j + shift == (int)ei)
                {
                    buf = 0;
                    book_Graph[z].mas_ei[0] = new CPointGraph(ref j, ref buf);
                    book_Graph[z].mas_ei[1] = new CPointGraph(ref j, ref h);
                }
                if (j + shift == (int)es)
                {
                    buf = 0;
                    book_Graph[z].mas_es[0] = new CPointGraph(ref j, ref buf);
                    book_Graph[z].mas_es[1] = new CPointGraph(ref j, ref h);
                }
                if (j + shift == 0)
                {
                    buf = 0;
                    book_Graph[z].mas_y[0] = new CPointGraph(ref j, ref buf);
                    book_Graph[z].mas_y[1] = new CPointGraph(ref j, ref h);
                }
                if (j + shift == (int)x_htrih)
                {
                    buf = 0;
                    book_Graph[z].mas_x_htrih[0] = new CPointGraph(ref j, ref buf);
                    book_Graph[z].mas_x_htrih[1] = new CPointGraph(ref j, ref h);
                }
            }
        }
        private static double Integral_percent(ref double t)
        {
            double e_s = 2.71828;
            double fi = 0.0;
            double h = t / 10000;
            for (double i = 0.0 ; i <= t; i += h)
	{
                fi += Math.Pow(e_s, -(Math.Pow(i, 2.0) / 2.0));
            }
            fi *= h;
            fi *= 1.0 / (Math.Sqrt(2.0 * 3.14));
            return fi;
        }
        public void SetShift(double p_shift)
        {
            shift += (int)(p_shift * 0.05);
        }
        public void SetScale(double p_scale)
        {
            scale += p_scale * 0.01;
        }
    }

}
