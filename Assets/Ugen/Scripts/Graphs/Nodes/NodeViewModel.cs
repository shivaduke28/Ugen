using System.Linq;
using R3;
using Ugen.Graphs.Ports;
using UnityEngine;

namespace Ugen.Graphs.Nodes
{
    public class NodeViewModel
    {
        public NodeId Id { get; }
        public string Name { get; }
        public readonly ReactiveProperty<Vector2> Position = new();
        public InputPortViewModel[] InputPorts { get; }
        public OutputPortViewModel[] OutputPorts { get; }

        readonly IGraphController _graphController;

        public NodeViewModel(Node node,
            IGraphController graphController)
        {
            Id = node.Id;
            Name = node.Name;
            InputPorts = node.InputPorts.Select((x, i) => new InputPortViewModel(Id, i, x.Name, graphController)).ToArray();
            OutputPorts = node.OutputPorts.Select((x, i) => new OutputPortViewModel(Id, i, x.Name, graphController)).ToArray();
            _graphController = graphController;
        }

        public bool TryGetInputPort(int index, out InputPortViewModel port)
        {
            if (index < 0 || index >= InputPorts.Length)
            {
                port = null;
                return false;
            }

            port = InputPorts[index];
            return true;
        }

        public bool TryGetOutputPort(int index, out OutputPortViewModel port)
        {
            if (index < 0 || index >= OutputPorts.Length)
            {
                port = null;
                return false;
            }

            port = OutputPorts[index];
            return true;
        }


        public void SetPosition(Vector2 position)
        {
            Position.Value = position;
        }

        public void MoveDelta(Vector2 delta)
        {
            Position.Value += delta;
        }

        public void ShowMenuContext(Vector2 pos)
        {
            _graphController.ShowNodeContextMenu(Id, pos);
        }
    }
}
