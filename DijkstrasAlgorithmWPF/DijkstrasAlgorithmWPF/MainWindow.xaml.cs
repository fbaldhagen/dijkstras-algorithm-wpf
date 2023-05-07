using DijkstrasAlgorithmWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using DijkstrasAlgorithmWPF.Helper;
using DijkstrasAlgorithmWPF.Algorithms;
using System.Windows.Input;

namespace DijkstrasAlgorithmWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DijkstrasAlgorithmWPF.Model.Grid grid;
        private Canvas canvas;
        private readonly int cellSize = 20; // Adjust the cell size as needed

        private Node latestStart;
        private Node latestEnd;

        public MainWindow()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Initialize the Grid and Canvas
            grid = GridHelper.InitializeGrid(20, 20, (0, 0), (10, 9)); // Adjust the grid size, start, and end positions as needed
            latestEnd = grid.EndNode;
            latestStart = grid.StartNode;
            AddObstacles(grid);

            canvas = new Canvas
            {
                Width = grid.Width * cellSize,
                Height = grid.Height * cellSize
            };
            MyGrid.Children.Add(canvas); 

            // Draw the initial grid cells
            foreach (Node node in grid.Nodes)
            {
                UpdateNodeUI(node, canvas);
            }

            canvas.MouseDown += OnCanvasMouseDown;

        }

        private void AddObstacles(DijkstrasAlgorithmWPF.Model.Grid grid)
        {
            GridHelper.AddObstacle(grid, 9, 10);
            GridHelper.AddObstacle(grid, 10, 11);
            GridHelper.AddObstacle(grid, 11, 12);
            GridHelper.AddObstacle(grid, 12, 13);
            GridHelper.AddObstacle(grid, 13, 14);
            GridHelper.AddObstacle(grid, 14, 15);


            GridHelper.AddObstacle(grid, 9, 9);
            GridHelper.AddObstacle(grid, 9, 8);
            GridHelper.AddObstacle(grid, 10, 7);
            GridHelper.AddObstacle(grid, 11, 6);
            GridHelper.AddObstacle(grid, 12, 5);
            GridHelper.AddObstacle(grid, 13, 4);
            GridHelper.AddObstacle(grid, 14, 3);
        }

        private void UpdateNodeUI(Node node, Canvas canvas)
        {
            Rectangle rect = new Rectangle
            {
                Width = cellSize,
                Height = cellSize,
                Stroke = Brushes.Black, 
                StrokeThickness = 1 
            };

            switch (node.State)
            {
                case NodeState.Unvisited:
                    rect.Fill = Brushes.White;
                    break;
                case NodeState.Visited:
                    rect.Fill = Brushes.Gray;
                    break;
                case NodeState.Path:
                    rect.Fill = Brushes.Orange;
                    break;
            }

            if (node == grid.StartNode)
            {
                rect.Fill = Brushes.Green;
            }
            else if (node == grid.EndNode)
            {
                rect.Fill = Brushes.Red;
            }
            else if (node.IsObstacle)
            {
                rect.Fill = Brushes.Black;
            }

            Canvas.SetLeft(rect, node.X * cellSize);
            Canvas.SetTop(rect, node.Y * cellSize);
            canvas.Children.Add(rect);
        }



        private async void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            List<Node> path = null;

            if (selectedAlgorithm == AlgorithmType.Dijkstra)
            {
                path = await Dijkstras.Search(grid, node => UpdateNodeUI(node, canvas));
            }
            else if (selectedAlgorithm == AlgorithmType.AStar)
            {
                
                path = await AStar.Search(grid, node => UpdateNodeUI(node, canvas));
            }

            if (path != null)
            {
                // Update the UI for the final path
                foreach (Node node in path)
                {
                    node.State = NodeState.Path;
                    UpdateNodeUI(node, canvas);
                }
            }
        }


        private void OnResetButtonClick(object sender, RoutedEventArgs e)
        {
            ResetGrid();
        }

        private void ResetGrid()
        {
            // Clear the canvas
            canvas.Children.Clear();

            // Iterate through each node in the grid
            foreach (Node node in grid.Nodes)
            {
                if (node != grid.StartNode && node != grid.EndNode && !node.IsObstacle)
                {
                    // Set the state of the node to Unvisited
                    node.State = NodeState.Unvisited;
                    // Reset distance value if it's not the start node
                    if (node != grid.StartNode)
                    {
                        node.Distance = double.MaxValue;
                    }
                }
            }

            // Draw the updated grid cells
            foreach (Node node in grid.Nodes)
            {
                UpdateNodeUI(node, canvas);
            }
        }


        private void OnCanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int x = (int)(e.GetPosition(canvas).X / cellSize);
                int y = (int)(e.GetPosition(canvas).Y / cellSize);

                Node clickedNode = grid.Nodes[x, y];

                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    // Update the UI for the previous start node
                    Node oldStart = grid.StartNode;
                    oldStart.Distance = double.MaxValue;
                    oldStart.State = NodeState.Unvisited;
                    grid.StartNode = clickedNode;
                    grid.StartNode.Distance = 0;
                    latestStart = grid.StartNode;
                    UpdateNodeUI(oldStart, canvas);
                    UpdateNodeUI(grid.StartNode, canvas);

                }
                else if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    // Update the UI for the previous end node
                    UpdateNodeUI(grid.EndNode, canvas);

                    Node oldEnd = grid.EndNode;
                    grid.EndNode = clickedNode;
                    latestEnd = grid.EndNode;
                    UpdateNodeUI(oldEnd, canvas);
                    UpdateNodeUI(clickedNode, canvas);
                }
                else
                {
                    // Toggle obstacle
                    clickedNode.IsObstacle = !clickedNode.IsObstacle;
                }

                // Update the UI for the clicked node
                UpdateNodeUI(clickedNode, canvas);
            }
        }

        public enum AlgorithmType
        {
            Dijkstra,
            AStar
        }

        private AlgorithmType selectedAlgorithm = AlgorithmType.Dijkstra;

        private void AlgorithmComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlgorithmComboBox.SelectedIndex == 0)
            {
                selectedAlgorithm = AlgorithmType.Dijkstra;
            }
            else if (AlgorithmComboBox.SelectedIndex == 1)
            {
                selectedAlgorithm = AlgorithmType.AStar;
            }
        }

        private void OnClearObstaclesButtonClick(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            // Reset the grid
            grid = GridHelper.InitializeGrid(20, 20, (latestStart.X, latestStart.Y), (latestEnd.X, latestEnd.Y));
            foreach (Node node in grid.Nodes)
            {
                UpdateNodeUI(node, canvas);
            }
        }
    }
}
