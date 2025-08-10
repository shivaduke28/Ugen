using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Ugen.Graph;
using Ugen.Graph.Nodes;

namespace Ugen.Editor.GraphView
{
    public class UgenGraphView : UnityEditor.Experimental.GraphView.GraphView
    {
        readonly Dictionary<string, UgenNodeView> nodeViews = new();

        public UgenGraphView()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            // Setup node creation
            nodeCreationRequest = context => ShowNodeCreationMenu(context.screenMousePosition);
        }

        void ShowNodeCreationMenu(Vector2 mousePosition)
        {
            var menu = new GenericMenu();
            var localMousePosition = contentViewContainer.WorldToLocal(mousePosition);

            menu.AddItem(new GUIContent("Add Node/Slider"), false, () => 
            {
                var node = new SliderNode();
                node.Position = localMousePosition;
                CreateNodeView(node);
            });
            
            menu.AddItem(new GUIContent("Add Node/Yaw Rotator"), false, () => 
            {
                var node = new YawRotatorNode();
                node.Position = localMousePosition;
                CreateNodeView(node);
            });

            menu.ShowAsContext();
        }

        UgenNodeView CreateNodeView(UgenNode node)
        {
            var nodeView = new UgenNodeView(node);
            nodeView.SetPosition(new Rect(node.Position, Vector2.zero));

            AddElement(nodeView);
            nodeViews[node.NodeId] = nodeView;
            return nodeView;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node)
                {
                    // Check if one is input and other is output
                    if (startPort.direction != port.direction)
                    {
                        // Check type compatibility
                        if (startPort.portType == port.portType)
                        {
                            compatiblePorts.Add(port);
                        }
                    }
                }
            });

            return compatiblePorts;
        }

        public void ClearGraph()
        {
            // Delete all edges
            edges.ForEach(edge => RemoveElement(edge));

            // Delete all nodes
            nodes.ForEach(node => RemoveElement(node));

            nodeViews.Clear();
        }

        public void SaveToGraph(UgenGraph graph)
        {
            // Store existing bindings before clearing
            var existingBindings = new Dictionary<string, Ugen.Behaviours.UgenBehaviour>();
            foreach (var binding in graph.Bindings)
            {
                existingBindings[binding.NodeId] = binding.Behaviour;
            }
            
            graph.Clear();

            // Save nodes
            foreach (var nodeView in nodeViews.Values)
            {
                var node = nodeView.Node;
                node.Position = nodeView.GetPosition().position;
                graph.AddNode(node);
                
                // Restore binding if it existed
                if (existingBindings.TryGetValue(node.NodeId, out var behaviour))
                {
                    graph.BindNodeToBehaviour(node.NodeId, behaviour);
                }
            }

            // Save connections
            edges.ForEach(edge =>
            {
                var outputNode = (edge.output.node as UgenNodeView)?.Node;
                var inputNode = (edge.input.node as UgenNodeView)?.Node;

                if (outputNode != null && inputNode != null)
                {
                    var outputPort = edge.output.userData as UgenPort;
                    var inputPort = edge.input.userData as UgenPort;

                    if (outputPort != null && inputPort != null)
                    {
                        graph.AddConnection(new UgenConnection
                        {
                            SourceNodeId = outputNode.NodeId,
                            SourcePortIndex = outputPort.Index,
                            TargetNodeId = inputNode.NodeId,
                            TargetPortIndex = inputPort.Index
                        });
                    }
                }
            });
        }

        public void LoadFromGraph(UgenGraph graph)
        {
            ClearGraph();

            // Create node views
            foreach (var node in graph.Nodes)
            {
                CreateNodeView(node);
            }

            // Create connections
            foreach (var connection in graph.Connections)
            {
                var sourceView = nodeViews.GetValueOrDefault(connection.SourceNodeId);
                var targetView = nodeViews.GetValueOrDefault(connection.TargetNodeId);

                if (sourceView != null && targetView != null)
                {
                    var outputPort = sourceView.GetOutputPort(connection.SourcePortIndex);
                    var inputPort = targetView.GetInputPort(connection.TargetPortIndex);

                    if (outputPort != null && inputPort != null)
                    {
                        var edge = outputPort.ConnectTo(inputPort);
                        AddElement(edge);
                    }
                }
            }
        }
    }
}
