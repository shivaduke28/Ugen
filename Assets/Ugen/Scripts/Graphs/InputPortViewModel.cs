namespace Ugen.Graphs
{
    public class InputPortViewModel : PortViewModel
    {
        public override PortDirection Direction => PortDirection.Input;

        public InputPortViewModel(NodeId nodeId, int index, string name) : base(nodeId, index, name)
        {
        }
    }
}
