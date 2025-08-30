namespace Ugen.Graphs.Ports
{
    public class InputPortViewModel : PortViewModel
    {
        public override PortDirection Direction => PortDirection.Input;
        public override EdgeCreationRequest EdgeCreationRequest { get; }
        public EdgeCreator EdgeCreator { get; }

        public InputPortViewModel(NodeId nodeId, int index, string name, EdgeCreator edgeCreator) : base(nodeId, index, name)
        {
            EdgeCreator = edgeCreator;
            EdgeCreationRequest = new EdgeCreationRequest(nodeId, index, PortDirection.Input);
        }
    }
}
