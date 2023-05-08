using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstrasAlgorithmWPF.Model
{
    public class Grid
    {
        public Node[,] Nodes { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Node StartNode { get; set; }
        public Node EndNode { get; set; }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            Nodes = new Node[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Nodes[x, y] = new Node(x, y);
                }
            }
        }

        public void Reset(bool keepObstacles = true)
        {
            foreach (Node node in Nodes)
            {
                node.Distance = double.MaxValue;
                node.Parent = null;
                node.State = NodeState.Unvisited;

                if (!keepObstacles)
                {
                    node.IsObstacle = false;
                }
            }
            StartNode.Distance = 0;
        }
    }

}
