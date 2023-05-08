using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DijkstrasAlgorithmWPF.Model;

namespace DijkstrasAlgorithmWPF.Helper
{
    public static class GridHelper
    {
        /// <summary>
        /// Initializes a Grid object with the given dimensions, start node, and end node.
        /// </summary>
        /// <param name="width">The width of the grid.</param>
        /// <param name="height">The height of the grid.</param>
        /// <param name="start">(x, y) coordinates of the start node.</param>
        /// <param name="end">(x, y) coordinates of the end node.</param>
        /// <returns>A Grid object with the specified parameters.</returns>
        public static Grid InitializeGrid(int width, int height, (int x, int y) start, (int x, int y) end)
        {
            Grid grid = new Grid(width, height);

            grid.StartNode = grid.Nodes[start.x, start.y];
            grid.EndNode = grid.Nodes[end.x, end.y];
            grid.StartNode.Distance = 0;

            return grid;
        }

        /// <summary>
        /// Adds an obstacle to the specified grid at the given (x, y) coordinates.
        /// </summary>
        /// <param name="grid">The Grid object where the obstacle should be added.</param>
        /// <param name="x">The x-coordinate of the obstacle.</param>
        /// <param name="y">The y-coordinate of the obstacle.</param>
        public static void AddObstacle(Grid grid, int x, int y)
        {
            grid.Nodes[x, y].IsObstacle = true;
        }

        /// <summary>
        /// Retrieves the unvisited and non-obstacle neighbors of the given node within the grid.
        /// </summary>
        /// <param name="grid">The Grid object containing the node.</param>
        /// <param name="node">The Node object for which neighbors should be retrieved.</param>
        /// <returns>A List of neighboring Node objects that are unvisited and not obstacles.</returns>
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

                    if (!neighbor.IsObstacle && neighbor.State == NodeState.Unvisited)
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }

            return neighbors;
        }

        /// <summary>
        /// Builds a path from the end node to the start node by following parent pointers.
        /// </summary>
        /// <param name="endNode">The end Node object from which the path should be built.</param>
        /// <returns>A List of Node objects representing the path from the end node to the start node.</returns>
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
