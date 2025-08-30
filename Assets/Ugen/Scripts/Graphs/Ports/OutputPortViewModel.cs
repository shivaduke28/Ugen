namespace Ugen.Graphs.Ports
{
    public class OutputPortViewModel : PortViewModel
    {
        public override PortDirection Direction => PortDirection.Output;
        public override EdgeCreationRequest EdgeCreationRequest { get; }
        public EdgeCreator EdgeCreator { get; }

        public OutputPortViewModel(NodeId nodeId, int index, string name, EdgeCreator edgeCreator) : base(nodeId, index, name)
        {
            EdgeCreator = edgeCreator;
            EdgeCreationRequest = new EdgeCreationRequest(nodeId, index, PortDirection.Output);
        }
    }
}
