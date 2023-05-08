# Dijkstra's and A* Algorithm Visualization

This is a simple WPF application to visualize Dijkstra's and A* pathfinding algorithms. The application allows users to interactively create obstacles, move the start and end nodes, and see the step-by-step process of finding the shortest path.

## Features

- Interactive grid with adjustable cell size and grid size
- Ability to place and remove obstacles
- Move start and end nodes with keyboard modifiers
- Choose between Dijkstra's and A* algorithms
- Step-by-step visualization of the search process
- Reset and clear obstacles buttons

## How to Run

1. Clone this repository or download it as a ZIP file and extract it.
2. Open the solution file (`DijkstrasAlgorithmWPF.sln`) in Visual Studio.
3. Build and run the project by pressing `F5` or by clicking the `Start` button in Visual Studio.

## Usage

- Left-click on a cell to place or remove an obstacle.
- Hold the `Left Shift` key and left-click on a cell to move the start node.
- Hold the `Left Ctrl` key and left-click on a cell to move the end node.
- Use the drop-down menu at the top to select the search algorithm (Dijkstra's or A*).
- Click the `Start` button to begin the search.
- Click the `Reset` button to clear the search results and start over.
- Click the `Clear Obstacles` button to remove all obstacles from the grid.

## Note on Unweighted Graph

It is important to mention that the current implementation of the grid and graph for this visualization is unweighted. This means that all edges connecting nodes have the same cost. In such cases, a simpler Breadth-First Search (BFS) algorithm could be used to achieve the same results with lower complexity. Dijkstra's and A* algorithms are particularly useful when dealing with weighted graphs, where the cost of traversing edges varies.

## Contributing

If you would like to contribute to this project, please feel free to fork the repository, make changes, and submit a pull request. All contributions are welcome!

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.
