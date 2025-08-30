namespace Ugen.Graphs.Ports
{
    public class InputPortViewModel : PortViewModel
    {
        public override PortDirection Direction => PortDirection.Input;
        public override PortData PortData { get; }
        public EdgeCreator EdgeCreator { get; }

        public InputPortViewModel(NodeId nodeId, int index, string name, EdgeCreator edgeCreator) : base(nodeId, index, name)
        {
            EdgeCreator = edgeCreator;
            PortData = new PortData(nodeId, index, PortDirection.Input);
        }
    }
}
