using R3;
using Ugen.Graphs.Nodes;
using UnityEngine;

namespace Ugen.Graphs.Ports
{
    public abstract class PortViewModel
    {
        public NodeId NodeId { get; }
        public string Name { get; }
        public ReactiveProperty<Vector2> ConnectorPanelPosition { get; } = new();

        protected PortViewModel(NodeId nodeId, string name)
        {
            NodeId = nodeId;
            Name = name;
        }
    }
}
