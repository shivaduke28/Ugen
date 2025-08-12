using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Ugen.Serialization;
using UnityEngine.UIElements;

namespace Ugen.Editor.GraphView
{
    public class UgenNodeView : Node
    {
        public UgenNodeData Node { get; }

        public string NodeId => Node.Id;
        public Vector2 Position => new(style.left.value.value, style.top.value.value);

        readonly List<Port> inputPorts = new();
        readonly List<Port> outputPorts = new();

        public UgenNodeView(UgenNodeData node)
        {
            this.Node = node;
            title = node.Name;

            CreatePorts(node);
            RefreshPorts();

            SetPosition(new Rect(node.Position, Vector2.zero));
        }

        void CreatePorts(UgenNodeData node)
        {
            // Create input ports
            foreach (var port in node.InputPorts)
            {
                var inputPort = CreateInputPort(port);
                inputContainer.Add(inputPort);
                inputPorts.Add(inputPort);
            }

            // Create output ports
            foreach (var port in node.OutputPorts)
            {
                var outputPort = CreateOutputPort(port);
                outputContainer.Add(outputPort);
                outputPorts.Add(outputPort);
            }
        }

        Port CreateInputPort(PortData portData)
        {
            var port = InstantiatePort(Orientation.Horizontal, Direction.Input,
                Port.Capacity.Multi, portData.ValueType);

            port.portName = portData.Name;
            port.userData = portData;

            port.AddManipulator(new EdgeConnector<UgenEdgeView>(new UgenEdgeConnectorListener()));

            return port;
        }

        Port CreateOutputPort(PortData portData)
        {
            var port = InstantiatePort(Orientation.Horizontal, Direction.Output,
                Port.Capacity.Multi, portData.ValueType);
            port.portName = portData.Name;
            port.userData = portData;

            port.AddManipulator(new EdgeConnector<UgenEdgeView>(new UgenEdgeConnectorListener()));
            return port;
        }


        public Port GetInputPort(int index)
        {
            return index >= 0 && index < inputPorts.Count ? inputPorts[index] : null;
        }

        public Port GetOutputPort(int index)
        {
            return index >= 0 && index < outputPorts.Count ? outputPorts[index] : null;
        }
    }
}
