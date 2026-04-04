using System.Collections.Generic;

namespace Astar
{
    public class Graph
    {
        private Dictionary<(int, int), Node> nodes;

        public Dictionary<(int, int), Node> Nodes { get => nodes; private set => nodes = value; }

        public Graph()
        {
            nodes = new Dictionary<(int, int), Node>();
        }

        public Node AddNode(int x, int y)
        {
            Node node = new Node(x, y);
            Nodes.Add((x, y), node);
            return node;
        }
        public void AddNodes((int x, int y)[] coords)
        {
            foreach ((int x, int y) coord in coords)
            {
                Nodes.Add((coord.x, coord.y), new Node(coord.x, coord.y));
            }
        }

        public void RemoveNode(int x, int y)
        {
            nodes.Remove((x, y));
        }
        public void ClearNodes()
        {
            nodes.Clear();
        }

        public void AddEdge(float length, Node fromNode, Node toNode, float weight = 1)
        {
            new Edge(fromNode, toNode, length, weight);
            new Edge(toNode, fromNode, length, weight);
        }
        public void AddEdge(Node fromNode, Node toNode, float weight = 1)
        {
            new Edge(fromNode, toNode, weight);
            new Edge(toNode, fromNode, weight);
        }

        public bool TryAddEdge(float length, Node fromNode, (int, int) coord, float weight = 1)
        {
            if (Nodes.TryGetValue(coord, out Node toNode))
            {
                AddEdge(length, fromNode, toNode, weight);
                return true;
            }
            return false;
        }
        public bool TryAddEdge(Node fromNode, (int, int) coord, float weight = 1)
        {
            if (Nodes.TryGetValue(coord, out Node toNode))
            {
                AddEdge(fromNode, toNode, weight);
                return true;
            }
            return false;
        }

        public void AddDirectionalEdge(float length, Node fromNode, Node toNode, float weight = 1)
        {
            new Edge(fromNode, toNode, length, weight);
        }
        public void AddDirectionalEdge(Node fromNode, Node toNode, float weight = 1)
        {
            new Edge(fromNode, toNode, weight);
        }




        public void DFS(Node startNode)
        {
            LinkedList<Node> parents = new LinkedList<Node>();
            Stack<Edge> stack = new Stack<Edge>();
            stack.Push(new Edge(startNode, startNode, 0));

            while (stack.Count != 0)
            {
                Edge currentEdge = stack.Pop();
                if (parents.Contains(currentEdge.ToNode))
                {
                    continue;
                }
                foreach (Edge edge in currentEdge.ToNode.Edges)
                {
                    stack.Push(edge);
                }

                parents.AddLast(currentEdge.ToNode);
            }
        }
        public LinkedList<Node> DFS(Node startNode, Node slutNode)
        {
            LinkedList<Node> parents = new LinkedList<Node>();
            Stack<Edge> stack = new Stack<Edge>();
            stack.Push(new Edge(startNode, startNode, 0));

            while (stack.Count != 0)
            {
                Edge currentEdge = stack.Pop();
                if (currentEdge.ToNode == slutNode)
                {
                    parents.AddLast(currentEdge.ToNode);
                    break;
                }

                if (parents.Contains(currentEdge.ToNode))
                {
                    continue;
                }

                foreach (Edge edge in currentEdge.ToNode.Edges)
                {
                    stack.Push(edge);
                }
                parents.AddLast(currentEdge.ToNode);
            }
            return parents;
        }


        public void BFS(Node startNode)
        {
            LinkedList<Node> parents = new LinkedList<Node>();
            Queue<Edge> stack = new Queue<Edge>();
            stack.Enqueue(new Edge(startNode, startNode, 0));

            while (stack.Count != 0)
            {
                Edge currentEdge = stack.Dequeue();
                if (parents.Contains(currentEdge.ToNode))
                {
                    continue;
                }
                foreach (Edge edge in currentEdge.ToNode.Edges)
                {
                    stack.Enqueue(edge);
                }

                parents.AddLast(currentEdge.ToNode);
            }
        }
        public LinkedList<Node> BFS(Node startNode, Node slutNode)
        {
            LinkedList<Node> parents = new LinkedList<Node>();
            Queue<Edge> stack = new Queue<Edge>();
            stack.Enqueue(new Edge(startNode, startNode, 0));

            while (stack.Count != 0)
            {
                Edge currentEdge = stack.Dequeue();
                if (currentEdge.ToNode == slutNode)
                {
                    parents.AddLast(currentEdge.ToNode);
                    break;
                }

                if (parents.Contains(currentEdge.ToNode))
                {
                    continue;
                }

                foreach (Edge edge in currentEdge.ToNode.Edges)
                {
                    stack.Enqueue(edge);
                }
                parents.AddLast(currentEdge.ToNode);
            }
            return parents;
        }


        public LinkedList<Node> AStar(Node startNode, Node endNode, out bool successful)
        {
            //starting shit
            //        child, parent
            Dictionary<Node, Node> closedList = new Dictionary<Node, Node>() { { startNode, startNode } };
            Dictionary<Node, (Node parent, float currentDistance)> openList = new Dictionary<Node, (Node, float)>() { { startNode, (startNode, 0) } };
            Node currentNode = startNode;
            successful = true;

            while (currentNode != endNode)// if current node is the final node.
            {
                // for each edge
                foreach (Edge edge in currentNode.Edges)
                {
                    Node searchedNode = edge.ToNode;

                    //if not already selected path
                    if (!closedList.ContainsKey(searchedNode))
                    {
                        float alternativDistance = openList[currentNode].currentDistance + edge.WeightedLength;

                        //if searched node isn't already searched
                        if (!openList.ContainsKey(searchedNode))
                        {// add node
                            //          child            parent
                            openList.Add(searchedNode, (currentNode, alternativDistance));
                        }
                        //if searched node is pointing to a longer path
                        else if (openList[searchedNode].currentDistance > alternativDistance)
                        {// change parent
                            //          child            parent
                            openList[searchedNode] = (currentNode, alternativDistance);
                        }
                    }
                }

                // breaks if no path could be found
                if (openList.Count == 1)
                {
                    successful = false;
                    break;
                }

                // removes current node, so it's not chosen as next node
                openList.Remove(currentNode);


                //find out which one is closer to the goal (add to closed list)
                Node nextNode = default;
                float nextNodeDistance = float.MaxValue;
                foreach (KeyValuePair<Node, (Node parent, float distance)> pair in openList)
                {
                    float F = pair.Value.distance + pair.Key.DistanceTo(endNode);
                    if (F < nextNodeDistance)
                    {
                        nextNodeDistance = F;
                        nextNode = pair.Key;
                    }
                }


                closedList.Add(nextNode, openList[nextNode].parent);
                currentNode = nextNode;
            }


            LinkedList<Node> finalPath = new LinkedList<Node>();
            while (currentNode != startNode)
            {
                finalPath.AddFirst(currentNode);
                currentNode = closedList[currentNode];
            }
            finalPath.AddFirst(startNode);
            return finalPath;
        }

        public LinkedList<Node> AStar((int, int) startCoord, (int, int) endCoord, out bool successful)
        {
            if (Nodes.TryGetValue(startCoord, out Node startNode) & Nodes.TryGetValue(endCoord, out Node endNode))
            {
                return AStar(startNode, endNode, out successful);
            }
            successful = false;
            return default;
        }


        /// <summary>
        /// Not even sure if this shit works ima be honest.
        /// Finds a path from <paramref name="startNode"/> to a <see cref="Node"/> that is a given <paramref name="distance"/> or farther away from the <paramref name="escapeNode"/> using the AStar algorithm.
        /// </summary>
        /// <param name="startNode">The starting <see cref="Node"/>.</param>
        /// <param name="escapeNode">The escaped <see cref="Node"/>.</param>
        /// <param name="distance">The target distance from the <paramref name="escapeNode"/>.</param>
        /// <param name="successful">Is true if the task succeeded.</param>
        /// <returns>A <see cref="LinkedList{Node}"/> containing the <see cref="Node"/>'s in the order they are travled through.</returns>
        public static LinkedList<Node> AwayStar(Node startNode, Node escapeNode, float distance, out bool successful)
        {
            //starting shit
            //        child, parent
            Dictionary<Node, Node> closedList = new Dictionary<Node, Node>() { { startNode, startNode } };
            Dictionary<Node, (Node parent, float currentDistance)> openList = new Dictionary<Node, (Node, float)>() { { startNode, (startNode, 0) } };
            Node currentNode = startNode;
            successful = true;

            while (currentNode.DistanceTo(escapeNode) <= distance)// if current node is the final node.
            {
                // for each edge
                foreach (Edge edge in currentNode.Edges)
                {
                    Node searchedNode = edge.ToNode;

                    //if not already selected path
                    if (!closedList.ContainsKey(searchedNode))
                    {
                        float alternativDistance = openList[currentNode].currentDistance + edge.WeightedLength;

                        //if searched node isn't already searched
                        if (!openList.ContainsKey(searchedNode))
                        {// add node
                            //          child            parent
                            openList.Add(searchedNode, (currentNode, alternativDistance));
                        }
                        //if searched node is pointing to a longer path
                        else if (openList[searchedNode].currentDistance > alternativDistance)
                        {// change parent
                            //          child            parent
                            openList[searchedNode] = (currentNode, alternativDistance);
                        }
                    }
                }

                // breaks if no path could be found
                if (openList.Count == 1)
                {
                    successful = false;
                    break;
                }

                // removes current node, so it's not chosen as next node
                openList.Remove(currentNode);


                //find out which one is closer to the goal (add to closed list)
                Node nextNode = default;
                float nextNodeDistance = float.MaxValue;
                foreach (KeyValuePair<Node, (Node parent, float distance)> pair in openList)
                {
                    float F = pair.Value.distance + (distance - pair.Key.DistanceTo(escapeNode));
                    if (F < nextNodeDistance)
                    {
                        nextNodeDistance = F;
                        nextNode = pair.Key;
                    }
                }


                closedList.Add(nextNode, openList[nextNode].parent);
                currentNode = nextNode;
            }


            LinkedList<Node> finalPath = new LinkedList<Node>();
            while (currentNode != startNode)
            {
                finalPath.AddFirst(currentNode);
                currentNode = closedList[currentNode];
            }
            finalPath.AddFirst(startNode);

            return finalPath;
        }

        /// <summary>
        /// Not even sure if this shit works ima be honest.
        /// Finds a path from <paramref name="startCoord"/> to a <see cref="Node"/> that is a given <paramref name="distance"/> or farther away from the <paramref name="escapeCoord"/> using the AStar algorithm.
        /// </summary>
        /// <param name="startCoord">The coordinates of the starting <see cref="Node"/>.</param>
        /// <param name="escapeCoord">The coordinates of the escaped <see cref="Node"/>.</param>
        /// <param name="distance">The target distance from the <paramref name="escapeCoord"/>.</param>
        /// <param name="successful">Is true if the task succeeded.</param>
        /// <returns>A <see cref="LinkedList{Node}"/> containing the <see cref="Node"/>'s in the order they are travled through.</returns>
        public LinkedList<Node> AwayStar((int, int) startCoord, (int, int) escapeCoord, float distance, out bool successful)
        {
            if (Nodes.TryGetValue(startCoord, out Node startNode) & Nodes.TryGetValue(escapeCoord, out Node escapeNode))
            {
                return AwayStar(startNode, escapeNode, distance, out successful);
            }
            successful = false;
            return default;
        }


    }
}
