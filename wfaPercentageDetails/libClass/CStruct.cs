using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace libClass
{
    public class CStruct
    {
        public CStruct(ref double pei, ref double pes, ref double px_htrih, ref double psigma, ref Color pcolor)
        {
            ei = pei;
            es = pes;
            x_htrih = px_htrih;
            sigma = psigma;
            color = pcolor;
            mas_ei = new CPointGraph[2];
            mas_es = new CPointGraph[2];
            mas_y = new CPointGraph[2];
            mas_x_htrih = new CPointGraph[2];
            mas_ei[0] = new CPointGraph();
            mas_ei[1] = new CPointGraph();
            mas_es[0] = new CPointGraph();
            mas_es[1] = new CPointGraph();
            mas_y[0] = new CPointGraph();
            mas_y[1] = new CPointGraph();
            mas_x_htrih[0] = new CPointGraph();
            mas_x_htrih[1] = new CPointGraph();
        }
        public bool flag = false ;
        public Color color;
        public List<CPointGraph> points_graph = new();
        public List<CPointGraph> points_fail_dutaley = new();
        public List<CPointGraph> points_fixable_dutaley = new();
        public CPointGraph[] mas_ei;
        public CPointGraph[] mas_es;
        public CPointGraph[] mas_y;
        public CPointGraph[] mas_x_htrih;
        public string str { get; set; }
        public double ei { get; set; }
        public double es { get; set; }
        public double x_htrih { get; set; }
        public double sigma { get; set; }
    }
}
