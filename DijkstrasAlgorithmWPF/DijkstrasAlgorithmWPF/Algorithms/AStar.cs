using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DijkstrasAlgorithmWPF.Helper;
using DijkstrasAlgorithmWPF.Model;

namespace DijkstrasAlgorithmWPF.Algorithms
{
    public class AStar
    {
        public static async Task<List<Node>> Search(Grid grid)
        {
            List<Node> unvisitedNodes = new List<Node>();

            foreach (Node node in grid.Nodes)
            {
                if (!node.IsObstacle)
                {
                    unvisitedNodes.Add(node);
                }
            }

            while (unvisitedNodes.Any())
            {
                Node currentNode = unvisitedNodes.OrderBy(n => n.Distance + Heuristic(n, grid.EndNode)).FirstOrDefault();

                if (currentNode is null)
                {
                    break;
                }

                if (currentNode == grid.EndNode)
                {
                    break;
                }

                List<Node> neighbors = GridHelper.GetNeighbors(grid, currentNode);

                foreach (Node neighbor in neighbors)
                {
                    double tentativeDistance = currentNode.Distance + 1;

                    if (tentativeDistance < neighbor.Distance)
                    {
                        neighbor.Distance = tentativeDistance;
                        neighbor.Parent = currentNode;
                    }
                }

                unvisitedNodes.Remove(currentNode);

                await Task.Delay(5);
            }

            return GridHelper.BuildPath(grid.EndNode);
        }

        private static double Heuristic(Node a, Node b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
}
