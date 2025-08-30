namespace Ugen.Graphs
{
    public class InputPortViewModel : PortViewModel
    {
        public override PortDirection Direction => PortDirection.Input;
        public override EdgeCreationRequest EdgeCreationRequest { get; }
        readonly EdgeCreator _edgeCreator;

        public InputPortViewModel(NodeId nodeId, int index, string name, EdgeCreator edgeCreator) : base(nodeId, index, name)
        {
            _edgeCreator = edgeCreator;
            EdgeCreationRequest = new EdgeCreationRequest(nodeId, index, PortDirection.Input);
        }

        public void TryCreateEdge(NodeId outputNodeId, int outputPortIndex)
        {
            _edgeCreator.TryCreateEdge(outputNodeId, outputPortIndex, NodeId, Index);
        }
    }
}
