using R3;
using UnityEngine;

namespace Ugen.Graphs
{
    public abstract class PortViewModel
    {
        public NodeId NodeId { get; }
        public int Index { get; }
        public abstract PortDirection Direction { get; }
        public string Name { get; }
        public ReactiveProperty<Vector2> ConnectorWorldPosition { get; } = new();

        protected PortViewModel(NodeId nodeId, int index, string name)
        {
            NodeId = nodeId;
            Index = index;
            Name = name;
        }
    }
}