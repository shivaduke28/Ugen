namespace Ugen.Graphs.Ports
{
    public readonly struct PortData
    {
        public readonly NodeId NodeId;
        public readonly int PortIndex;
        public readonly PortDirection Direction;

        public PortData(NodeId nodeId, int portIndex, PortDirection direction)
        {
            NodeId = nodeId;
            PortIndex = portIndex;
            Direction = direction;
        }

        public override string ToString()
        {
            return $"({NodeId}, {PortIndex}, {Direction})";
        }
    }
}
