namespace Ugen.Graphs.Ports
{
    public class OutputPortViewModel : PortViewModel
    {
        public override PortDirection Direction => PortDirection.Output;
        public override PortData PortData { get; }
        public IGraphController GraphController { get; }

        public OutputPortViewModel(NodeId nodeId, int index, string name, IGraphController graphController) : base(nodeId, index, name)
        {
            GraphController = graphController;
            PortData = new PortData(nodeId, index, PortDirection.Output);
        }
    }
}
