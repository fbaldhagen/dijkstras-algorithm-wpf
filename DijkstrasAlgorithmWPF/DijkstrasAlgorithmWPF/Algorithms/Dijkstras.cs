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
    public class Dijkstras
    {
        public static async Task<List<Node>> Search(Grid grid, Action<Node> onNodeProcessed)
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
                Node currentNode = unvisitedNodes.OrderBy(n => n.Distance).FirstOrDefault();

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

                currentNode.State = NodeState.Visited;
                onNodeProcessed(currentNode); // Update the UI for the current node
                await Task.Delay(5); // Add a delay to visualize the algorithm steps

                unvisitedNodes.Remove(currentNode);
            }
            return GridHelper.BuildPath(grid.EndNode);
        }
    }
}
