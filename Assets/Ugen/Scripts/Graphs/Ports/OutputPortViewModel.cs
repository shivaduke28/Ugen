namespace Ugen.Graphs.Ports
{
    public class OutputPortViewModel : PortViewModel
    {
        public override PortDirection Direction => PortDirection.Output;
        public override PortData PortData { get; }
        public EdgeCreator EdgeCreator { get; }

        public OutputPortViewModel(NodeId nodeId, int index, string name, EdgeCreator edgeCreator) : base(nodeId, index, name)
        {
            EdgeCreator = edgeCreator;
            PortData = new PortData(nodeId, index, PortDirection.Output);
        }
    }
}
