namespace Ugen.Graphs.Ports
{
    public class InputPortViewModel : PortViewModel
    {
        public override PortDirection Direction => PortDirection.Input;
        public override PortData PortData { get; }
        public IGraphController GraphController { get; }

        public InputPortViewModel(NodeId nodeId, int index, string name, IGraphController graphController) : base(nodeId, index, name)
        {
            GraphController = graphController;
            PortData = new PortData(nodeId, index, PortDirection.Input);
        }
    }
}
