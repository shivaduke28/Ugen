using System.Collections.Generic;
using Ugen.Behaviours;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Ugen.Serialization;
using Ugen.UI.Nodes;
using UnityEngine.Assertions;

namespace Ugen.Editor.GraphView
{
    public class UgenGraphView : UnityEditor.Experimental.GraphView.GraphView
    {
        readonly Dictionary<string, UgenNodeView> _nodeViews = new();
        readonly Dictionary<string, UgenEdgeView> _edgeViews = new();
        readonly List<UgenBehaviour> _behaviours = new();
        readonly List<UgenUIElement> _uiElements = new();

        public List<UgenBehaviour> Behaviours => _behaviours;
        public List<UgenUIElement> UIElements => _uiElements;
        UgenGraphData _currentGraph;

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
            if (styleSheet != null) styleSheets.Add(styleSheet);

            var searchWindowProvider = ScriptableObject.CreateInstance<CreateNodeSearchWindowProvider>();
            searchWindowProvider.Initialize(this, _currentGraph);

            nodeCreationRequest += context =>
            {
                if (_currentGraph != null) searchWindowProvider.Initialize(this, _currentGraph);

                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindowProvider);
            };
        }

        UgenNodeView CreateNodeView(UgenNodeData node)
        {
            UgenNodeView nodeView = node switch
            {
                // Create specialized view for UgenBehaviourNode
                UgenBehaviourNodeData behaviourNode => new UgenBehaviourNodeView(behaviourNode, _currentGraph),
                UgenUIElementNodeData uiElementNode => new UgenUIElementNodeView(uiElementNode, _uiElements),
                _ => new UgenNodeView(node)
            };

            nodeView.SetPosition(new Rect(node.Position, Vector2.zero));

            AddElement(nodeView);
            _nodeViews[node.Id] = nodeView;
            return nodeView;
        }

        public void AddNodeView(UgenNodeView nodeView)
        {
            AddElement(nodeView);
            _nodeViews[nodeView.NodeId] = nodeView;
        }

        public void AddEdgeView(UgenEdgeView edgeView)
        {
            AddElement(edgeView);
            _edgeViews[edgeView.EdgeId] = edgeView;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node)
                    // Check if one is input and other is output
                    if (startPort.direction != port.direction)
                        // Check type compatibility
                        if (startPort.portType == port.portType)
                            compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        public void ClearGraph()
        {
            edges.ForEach(RemoveElement);
            nodes.ForEach(RemoveElement);

            _nodeViews.Clear();
        }

        public UgenGraphData ExportToGraph()
        {
            var nodeDatas = new List<UgenNodeData>();
            var edgeDatas = new List<EdgeData>();
            // Save nodes with updated positions
            foreach (var nodeView in _nodeViews.Values)
            {
                var node = nodeView.Node;
                node.Position = nodeView.GetPosition().position;
                nodeDatas.Add(node);
            }

            foreach (var edge in _edgeViews.Values)
            {
                var outputNode = (edge.output.node as UgenNodeView)?.Node;
                var inputNode = (edge.input.node as UgenNodeView)?.Node;

                if (outputNode != null && inputNode != null)
                {
                    if (edge.output.userData is PortData outputPort && edge.input.userData is PortData inputPort)
                    {
                        var edgeId = edge.EdgeId;

                        edgeDatas.Add(new EdgeData(
                            edgeId,
                            inputNode.Id,
                            inputPort.Index,
                            outputNode.Id,
                            outputPort.Index));
                    }
                }
                else
                {
                    Debug.LogWarning($"Edge {edge.EdgeId} has invalid nodes. Output: {outputNode?.Id}, Input: {inputNode?.Id}");
                }
            }

            return new UgenGraphData(_currentGraph.Behaviours, _currentGraph.VisualTreeAsset, nodeDatas.ToArray(), edgeDatas.ToArray());
        }

        public void LoadFromGraph(UgenGraphData graph)
        {
            Assert.IsNotNull(graph);
            ClearGraph();
            _currentGraph = graph;

            _behaviours.Clear();
            _uiElements.Clear();

            _behaviours.AddRange(graph.Behaviours);
            var ve = new VisualElement();
            graph.VisualTreeAsset.CloneTree(ve);
            _uiElements.AddRange(ve.Query<UgenUIElement>().ToList());

            // Create node views
            foreach (var node in graph.Nodes) CreateNodeView(node);

            // Create connections
            foreach (var ugenEdge in graph.Edges)
            {
                var sourceView = _nodeViews.GetValueOrDefault(ugenEdge.OutputNodeId);
                var targetView = _nodeViews.GetValueOrDefault(ugenEdge.InputNodeId);

                if (sourceView != null && targetView != null)
                {
                    var outputPort = sourceView.GetOutputPort(ugenEdge.OutputPortIndex);
                    var inputPort = targetView.GetInputPort(ugenEdge.InputPortIndex);

                    if (outputPort != null && inputPort != null)
                    {
                        var edge = new UgenEdgeView(ugenEdge.Id)
                        {
                            output = outputPort,
                            input = inputPort,
                        };
                        outputPort.Connect(edge);
                        inputPort.Connect(edge);
                        AddEdgeView(edge);
                    }
                }
            }
        }
    }
}
