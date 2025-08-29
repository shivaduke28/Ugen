using R3;
using UnityEngine;

namespace Ugen.Graphs
{
    public class NodeViewModel
    {
        public NodeId Id { get; }
        public string Name { get; }
        public readonly ReactiveProperty<Vector2> Position = new();
        public InputPortViewModel[] InputPorts { get; }
        public OutputPortViewModel[] OutputPorts { get; }

        public NodeViewModel(NodeId id, string name, InputPortViewModel[] inputPorts, OutputPortViewModel[] outputPorts)
        {
            Id = id;
            Name = name;
            InputPorts = inputPorts;
            OutputPorts = outputPorts;
        }

        public void SetPosition(Vector2 position)
        {
            Position.Value = position;
        }

        public void MoveDelta(Vector2 delta)
        {
            Position.Value += delta;
        }
    }
}
