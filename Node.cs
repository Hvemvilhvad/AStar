using System;
using System.Collections.Generic;

namespace Astar
{
    public class Node
    {
        private int x;
        private int y;
        private List<Edge> edges;

        public int X { get => x; }
        public int Y { get => y; }
        public List<Edge> Edges { get => edges; private set => edges = value; }

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
            edges = new List<Edge>(8);
        }


        public void AddEdge(Edge edge)
        {
            Edges.Add(edge);
        }

        public float DistanceTo(Node other)
        {
            return (float)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

    }
}
