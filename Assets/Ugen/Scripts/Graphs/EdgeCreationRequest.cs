namespace Ugen.Graphs
{
    public readonly struct EdgeCreationRequest
    {
        public readonly NodeId NodeId;
        public readonly int PortIndex;
        public readonly PortDirection Direction;

        public EdgeCreationRequest(NodeId nodeId, int portIndex, PortDirection direction)
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
