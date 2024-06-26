﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstrasAlgorithmWPF.Model
{
    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Distance { get; set; }
        public Node Parent { get; set; }
        public NodeState State { get; set; }
        public bool IsObstacle { get; set; }

        public Node(int x, int y)
        {
            X = x;
            Y = y;
            Distance = double.MaxValue;
            State = NodeState.Unvisited;
            IsObstacle = false;
        }
    }

    public enum NodeState
    {
        Unvisited,
        Visited
    }

}
