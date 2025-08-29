namespace Ugen.Graphs
{
    public class OutputPortViewModel : PortViewModel
    {
        public override PortDirection Direction => PortDirection.Output;

        public OutputPortViewModel(NodeId nodeId, int index, string name) : base(nodeId, index, name)
        {
        }
    }
}
