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
        private Model.Grid _grid;
        private const int GridSize = 20;
        private const int CellSize = 25;
        private Action<Node> _onNodeProcessed;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGrid();
            _onNodeProcessed = UpdateUI;
        }

        /// <summary>
        /// Initializes the grid by creating a grid of nodes and drawing rectangles
        /// representing each node on the MyCanvas element. The grid dimensions are determined
        /// by the GridSize constant and each cell's dimensions are determined by the CellSize constant.
        /// This method also sets the appropriate event handlers for handling mouse input on each cell.
        /// After initializing the grid, the UI is updated to reflect the initial state of the grid.
        /// </summary>
        private void InitializeGrid()
        {
            MyCanvas.Children.Clear();

            MyCanvas.Width = CellSize * GridSize;
            MyCanvas.Height = CellSize * GridSize;

            _grid = GridHelper.InitializeGrid(GridSize, GridSize, (0, 0), (GridSize - 1, GridSize - 1));

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    var rect = new Rectangle
                    {
                        Width = CellSize,
                        Height = CellSize,
                        Fill = Brushes.White,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1
                    };
                    rect.MouseLeftButtonDown += OnCellMouseLeftButtonDown;
                    MyCanvas.Children.Add(rect);
                    Canvas.SetLeft(rect, i * CellSize);
                    Canvas.SetTop(rect, j * CellSize);
                }
            }
            UpdateAllUI();
        }

        /// <summary>
        /// Updates the visual appearance of a single node on the UI canvas based on its properties.
        /// The fill color of the rectangle representing the node is changed according to its type
        /// (start node, end node, obstacle, visited or unvisited).
        /// </summary>
        /// <param name="node">The Node object to be updated on the UI.</param>
        private void UpdateUI(Node node)
        {
            int x = node.X;
            int y = node.Y;
            var rect = MyCanvas.Children[y * GridSize + x] as Rectangle;

            if (node == _grid.StartNode)
            {
                rect.Fill = Brushes.Green;
            }
            else if (node == _grid.EndNode)
            {
                rect.Fill = Brushes.Red;
            }
            else if (node.IsObstacle)
            {
                rect.Fill = Brushes.Black;
            }
            else if (node.State == NodeState.Visited)
            {
                rect.Fill = Brushes.LightBlue;
            }
            else
            {
                rect.Fill = Brushes.White;
            }
        }

        /// <summary>
        /// Updates the UI to highlight the shortest path found by the search algorithm.
        /// The fill color of the rectangles representing the path nodes is set to yellow,
        /// excluding the start and end nodes.
        /// </summary>
        /// <param name="path">The list of Node objects representing the shortest path.</param>
        private void UpdatePathUI(List<Node> path)
        {
            foreach (Node node in path)
            {
                if (node == _grid.StartNode || node == _grid.EndNode)
                {
                    continue;
                }

                int x = node.X;
                int y = node.Y;
                var rect = MyCanvas.Children[y * GridSize + x] as Rectangle;
                rect.Fill = Brushes.Yellow;
            }
        }

        /// <summary>
        /// Updates the visual appearance of all nodes on the UI canvas based on their properties.
        /// Calls the UpdateUI method for each node in the grid.
        /// </summary>
        private void UpdateAllUI()
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    var node = _grid.Nodes[i, j];
                    UpdateUI(node);
                }
            }
        }

        /// <summary>
        /// Returns the appropriate Brush object based on the properties of the given node.
        /// </summary>
        /// <param name="node">The Node object for which to get the brush.</param>
        /// <returns>The Brush object corresponding to the node's type (start node, end node, obstacle, visited or unvisited).</returns>
        private Brush GetBrushForNode(Node node)
        {
            if (node == _grid.StartNode) return Brushes.Green;
            if (node == _grid.EndNode) return Brushes.Red;
            if (node.IsObstacle) return Brushes.Black;
            if (node.State == NodeState.Visited) return Brushes.LightBlue;
            return Brushes.White;
        }

        /// <summary>
        /// Handles the Start button click event. Resets the grid, runs the selected search algorithm
        /// (Dijkstra's or A*), and updates the UI to display the shortest path.
        /// </summary>
        /// <param name="sender">The sender object (Start button).</param>
        /// <param name="e">The RoutedEventArgs object containing the event data.</param>
        private async void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            _grid.Reset();
            UpdateAllUI();

            List<Node> shortestPath = null;
            if (AlgorithmComboBox.SelectedIndex == 0) // Dijkstra's Algorithm
            {
                shortestPath = await Dijkstras.Search(_grid, UpdateUI);
            }
            else if (AlgorithmComboBox.SelectedIndex == 1) // A* Algorithm
            {
                shortestPath = await AStar.Search(_grid, UpdateUI);
            }

            // Update the UI for the shortest path
            UpdatePathUI(shortestPath);
        }

        /// <summary>
        /// Handles the Reset button click event. Resets the grid and updates the UI.
        /// </summary>
        /// <param name="sender">The sender object (Reset button).</param>
        /// <param name="e">The RoutedEventArgs object containing the event data.</param>
        private void OnResetButtonClick(object sender, RoutedEventArgs e)
        {
            _grid.Reset();
            UpdateAllUI();
        }

        /// <summary>
        /// Handles the Clear Obstacles button click event. Resets the grid without keeping
        /// the obstacles and updates the UI.
        /// </summary>
        /// <param name="sender">The sender object (Clear Obstacles button).</param>
        /// <param name="e">The RoutedEventArgs object containing the event data.</param>
        private void OnClearObstaclesButtonClick(object sender, RoutedEventArgs e)
        {
            _grid.Reset(keepObstacles: false);
            UpdateAllUI();
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event for a cell. Updates the start node, end node, or obstacle
        /// state based on the current keyboard state (Shift or Ctrl key pressed) and updates the UI.
        /// </summary>
        /// <param name="sender">The sender object (cell Rectangle).</param>
        /// <param name="e">The MouseButtonEventArgs object containing the event data.</param>
        private void OnCellMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var rect = sender as Rectangle;
            int x = (int)(Canvas.GetLeft(rect) / CellSize);
            int y = (int)(Canvas.GetTop(rect) / CellSize);

            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                _grid.StartNode = _grid.Nodes[y, x]; 
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                _grid.EndNode = _grid.Nodes[y, x]; 
            }
            else
            {
                Node node = _grid.Nodes[y, x]; 
                node.IsObstacle = !node.IsObstacle;
            }
            UpdateAllUI();
        }
    }
}
