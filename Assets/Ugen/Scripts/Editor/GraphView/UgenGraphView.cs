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
            currentGraph = graph;
            graph.ClearNodeAndEdges();

            // Save nodes
            foreach (var nodeView in nodeViews.Values)
            {
                var node = nodeView.Node;
                node.Position = nodeView.GetPosition().position;
                graph.AddNode(node);
            }

            // Save connections
            edges.ForEach(edge =>
            {
                var outputNode = (edge.output.node as UgenNodeView)?.Node;
                var inputNode = (edge.input.node as UgenNodeView)?.Node;

                if (outputNode != null && inputNode != null)
                {
                    if (edge.output.userData is UgenPort outputPort && edge.input.userData is UgenPort inputPort)
                    {
                        graph.AddEdge(new UgenEdge
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
            currentGraph = graph;

            // Create node views
            foreach (var node in graph.Nodes)
            {
                CreateNodeView(node);
            }

            // Create connections
            foreach (var ugenEdge in graph.Edges)
            {
                var sourceView = nodeViews.GetValueOrDefault(ugenEdge.SourceNodeId);
                var targetView = nodeViews.GetValueOrDefault(ugenEdge.TargetNodeId);

                if (sourceView != null && targetView != null)
                {
                    var outputPort = sourceView.GetOutputPort(ugenEdge.SourcePortIndex);
                    var inputPort = targetView.GetInputPort(ugenEdge.TargetPortIndex);

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
