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
                var inputPort = CreateInputPort(port);
                inputContainer.Add(inputPort);
                inputPorts.Add(inputPort);
            }

            // Create output ports
            foreach (var port in Node.OutputPorts)
            {
                var outputPort = CreateOutputPort(port);
                outputContainer.Add(outputPort);
                outputPorts.Add(outputPort);
            }
        }

        Port CreateInputPort(IUgenInput portData)
        {
            var port = InstantiatePort(Orientation.Horizontal, Direction.Input,
                Port.Capacity.Multi, portData.ValueType);

            port.portName = portData.Name;
            port.userData = portData;

            port.AddManipulator(new EdgeConnector<UgenEdgeView>(new UgenEdgeConnectorListener()));

            return port;
        }

        Port CreateOutputPort(IUgenOutput portData)
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

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Node.Position = newPos.position;
        }
    }
}
