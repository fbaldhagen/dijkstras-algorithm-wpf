using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DijkstrasAlgorithmWPF.Model;

namespace DijkstrasAlgorithmWPF.Helper
{
    public class GridHelper
    {
        public static Grid InitializeGrid(int width, int height, (int x, int y) start, (int x, int y) end)
        {
            Grid grid = new Grid
            {
                Width = width,
                Height = height,
                Nodes = new Node[width, height]
            };

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid.Nodes[x, y] = new Node { X = x, Y = y, Distance = double.MaxValue };
                }
            }

            grid.StartNode = grid.Nodes[start.x, start.y];
            grid.EndNode = grid.Nodes[end.x, end.y];
            grid.StartNode.Distance = 0;

            return grid;
        }

        public static void AddObstacle(Grid grid, int x, int y)
        {
            grid.Nodes[x, y].IsObstacle = true;
        }

        public static List<Node> GetNeighbors(Grid grid, Node node)
        {
            List<Node> neighbors = new List<Node>();

            int[] dx = { -1, 0, 1, 0 };
            int[] dy = { 0, 1, 0, -1 };

            for (int i = 0; i < 4; i++)
            {
                int newX = node.X + dx[i];
                int newY = node.Y + dy[i];

                if (newX >= 0 && newX < grid.Width && newY >= 0 && newY < grid.Height)
                {
                    Node neighbor = grid.Nodes[newX, newY];

                    if (!neighbor.IsObstacle)
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }

            return neighbors;
        }

        public static List<Node> BuildPath(Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode.Parent != null)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Reverse();

            return path;
        }
    }
}
