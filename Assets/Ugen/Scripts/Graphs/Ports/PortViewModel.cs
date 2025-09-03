using Ugen.Graphs.Nodes;

namespace Ugen.Graphs.Ports
{
    public abstract class PortViewModel
    {
        public NodeId NodeId { get; }
        public string Name { get; }

        protected PortViewModel(NodeId nodeId, string name)
        {
            NodeId = nodeId;
            Name = name;
        }
    }
}
