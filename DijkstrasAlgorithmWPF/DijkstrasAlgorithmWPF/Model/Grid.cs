using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstrasAlgorithmWPF.Model
{
    public class Grid
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Node[,] Nodes { get; set; }
        public Node StartNode { get; set; }
        public Node EndNode { get; set; }
    }
}
