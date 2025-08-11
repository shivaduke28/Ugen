using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Ugen.Graph;
using UnityEngine.UIElements;

namespace Ugen.Editor.GraphView
{
    public class UgenNodeView : Node
    {
        public UgenNode Node { get; }

        readonly List<Port> inputPorts = new();
        readonly List<Port> outputPorts = new();

        public UgenNodeView(UgenNode node)
        {
            Node = node;
            title = node.NodeName;

            CreatePorts();
            // RefreshExpandedState();
            RefreshPorts();

            SetPosition(new Rect(node.Position, Vector2.zero));
        }

        void CreatePorts()
        {
            // Create input ports
            foreach (var port in Node.InputPorts)
            {
                var inputPort = CreatePort(port, Direction.Input);
                inputContainer.Add(inputPort);
                inputPorts.Add(inputPort);
            }

            // Create output ports
            foreach (var port in Node.OutputPorts)
            {
                var outputPort = CreatePort(port, Direction.Output);
                outputContainer.Add(outputPort);
                outputPorts.Add(outputPort);
            }
        }

        Port CreatePort(UgenPort portData, Direction direction)
        {
            var port = InstantiatePort(Orientation.Horizontal, direction,
                Port.Capacity.Multi, portData.ValueType);

            port.portName = portData.Name;
            port.userData = portData;

            // Add custom edge connector for creating UgenEdgeView
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

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Node.Position = newPos.position;
        }
    }
}
