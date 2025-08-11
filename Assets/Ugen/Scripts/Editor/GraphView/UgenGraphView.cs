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
        readonly Dictionary<string, UgenEdgeView> edgeViews = new();
        UgenGraph currentGraph;

        public UgenGraphView()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            // Load stylesheet
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Ugen/Scripts/Editor/GraphView/UgenGraphView.uss");
            if (styleSheet != null)
            {
                styleSheets.Add(styleSheet);
            }

            var searchWindowProvider = ScriptableObject.CreateInstance<CreateNodeSearchWindowProvider>();
            searchWindowProvider.Initialize(this, currentGraph);

            nodeCreationRequest += context =>
            {
                if (currentGraph != null)
                {
                    searchWindowProvider.Initialize(this, currentGraph);
                }
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindowProvider);
            };
        }

        UgenNodeView CreateNodeView(UgenNode node)
        {
            UgenNodeView nodeView;

            // Create specialized view for UgenBehaviourNode
            if (node is UgenBehaviourNode behaviourNode)
            {
                nodeView = new UgenBehaviourNodeView(behaviourNode, currentGraph);
            }
            else
            {
                nodeView = new UgenNodeView(node);
            }

            nodeView.SetPosition(new Rect(node.Position, Vector2.zero));

            AddElement(nodeView);
            nodeViews[node.NodeId] = nodeView;
            return nodeView;
        }

        public void AddNodeView(UgenNodeView nodeView)
        {
            AddElement(nodeView);
            nodeViews[nodeView.Node.NodeId] = nodeView;
        }

        public void AddEdgeView(UgenEdgeView edgeView)
        {
            AddElement(edgeView);
            edgeViews[edgeView.EdgeId] = edgeView;
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
            edges.ForEach(RemoveElement);
            nodes.ForEach(RemoveElement);

            nodeViews.Clear();
        }

        public void SaveToGraph(UgenGraph graph)
        {
            currentGraph = graph;
            graph.ClearNodeAndEdges();

            // Save nodes with updated positions
            foreach (var nodeView in nodeViews.Values)
            {
                var node = nodeView.Node;
                node.Position = nodeView.GetPosition().position;
                graph.AddNode(node);
            }

            foreach(var edge in edgeViews.Values)
            {
                var outputNode = (edge.output.node as UgenNodeView)?.Node;
                var inputNode = (edge.input.node as UgenNodeView)?.Node;

                if (outputNode != null && inputNode != null)
                {
                    if (edge.output.userData is IUgenOutput outputPort && edge.input.userData is IUgenInput inputPort)
                    {
                        var edgeId = edge.EdgeId;

                        graph.AddEdge(new UgenEdge
                        {
                            EdgeId = edgeId,
                            OutputNodeId = outputNode.NodeId,
                            OutputPortIndex = outputPort.Index,
                            InputNodeId = inputNode.NodeId,
                            InputPortIndex = inputPort.Index
                        });
                    }
                }
            }
        }

        public void LoadFromGraph(UgenGraph graph)
        {
            ClearGraph();
            currentGraph = graph;

            // Create node views
            foreach (var node in graph.Nodes)
            {
                CreateNodeView(node);
            }

            // Create connections
            foreach (var ugenEdge in graph.Edges)
            {
                var sourceView = nodeViews.GetValueOrDefault(ugenEdge.OutputNodeId);
                var targetView = nodeViews.GetValueOrDefault(ugenEdge.InputNodeId);

                if (sourceView != null && targetView != null)
                {
                    var outputPort = sourceView.GetOutputPort(ugenEdge.OutputPortIndex);
                    var inputPort = targetView.GetInputPort(ugenEdge.InputPortIndex);

                    if (outputPort != null && inputPort != null)
                    {
                        // Create UgenEdgeView with existing EdgeId
                        var edge = new UgenEdgeView(ugenEdge.EdgeId);
                        edge.output = outputPort;
                        edge.input = inputPort;
                        outputPort.Connect(edge);
                        inputPort.Connect(edge);
                        AddElement(edge);
                    }
                }
            }
        }
    }
}
