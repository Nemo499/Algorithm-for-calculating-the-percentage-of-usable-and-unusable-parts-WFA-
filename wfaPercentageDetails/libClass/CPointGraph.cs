namespace libClass
{
    public class CPointGraph
    {
        public CPointGraph() 
        {
            x = 0; 
            y = 0;   
        }
        public CPointGraph(ref int px, ref int py)
        {
            x = px; 
            y = py;
        }
        public int x;
        public int y;
    }
}
