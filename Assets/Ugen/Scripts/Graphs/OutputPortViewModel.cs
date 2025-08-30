namespace Ugen.Graphs
{
    public class OutputPortViewModel : PortViewModel
    {
        public override PortDirection Direction => PortDirection.Output;
        public override EdgeCreationRequest EdgeCreationRequest { get; }
        readonly EdgeCreator _edgeCreator;

        public OutputPortViewModel(NodeId nodeId, int index, string name, EdgeCreator edgeCreator) : base(nodeId, index, name)
        {
            _edgeCreator = edgeCreator;
            EdgeCreationRequest = new EdgeCreationRequest(nodeId, index, PortDirection.Output);
        }

        public void TryCreateEdge(NodeId outputNodeId, int outputPortIndex)
        {
            _edgeCreator.TryCreateEdge(outputNodeId, outputPortIndex, NodeId, Index);
        }
    }
}
