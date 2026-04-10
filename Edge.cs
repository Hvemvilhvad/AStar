
namespace Astar
{
    public class Edge
    {
        private Node fromNode;
        private Node toNode;
        private float length;
        private float weight;

        public Node FromNode { get => fromNode; private set => fromNode = value; }
        public Node ToNode { get => toNode; private set => toNode = value; }
        public float Length { get => length; private set => length = value; }
        public float Weight { get => weight; private set => weight = value; }
        public float WeightedLength { get => length * weight; }

        public Edge(Node fromNode, Node toNode, float length, float weight)
        {
            FromNode = fromNode;
            FromNode.AddEdge(this);
            ToNode = toNode;
            Length = length;
            Weight = weight;
        }
        public Edge(Node fromNode, Node toNode, float weight) : this(fromNode, toNode, fromNode.DistanceTo(toNode), weight)
        {

        }

        public override string ToString()
        {
            return $"{FromNode} -> {ToNode} | {Length}";
        }
    }

}
