namespace Ugen.Graphs.Ports
{
    public readonly struct PortData
    {
        public readonly NodeId NodeId;
        public readonly int Index;
        public readonly PortDirection Direction;

        public PortData(NodeId nodeId, int index, PortDirection direction)
        {
            NodeId = nodeId;
            Index = index;
            Direction = direction;
        }

        public override string ToString()
        {
            return $"(NodeId:{NodeId}, Index:{Index}, Direction:{Direction})";
        }
    }
}
