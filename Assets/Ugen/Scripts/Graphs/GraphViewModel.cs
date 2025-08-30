using System;
using System.Collections.Generic;
using ObservableCollections;
using R3;
using Ugen.Graphs.EdgeContextMenu;
using Ugen.Graphs.GraphContextMenu;
using Ugen.Graphs.NodeContextMenu;
using Ugen.Graphs.Ports;
using UnityEngine;

namespace Ugen.Graphs
{
    public sealed class GraphViewModel : IGraphController
    {
        readonly ObservableDictionary<NodeId, NodeViewModel> _nodes = new();
        readonly ObservableDictionary<EdgeId, EdgeViewModel> _edges = new();
        readonly ObservableDictionary<EdgeId, IEdgeEndPoints> _previewEdges = new();

        public IReadOnlyObservableDictionary<NodeId, NodeViewModel> Nodes => _nodes;
        public IReadOnlyObservableDictionary<EdgeId, EdgeViewModel> Edges => _edges;
        public IReadOnlyObservableDictionary<EdgeId, IEdgeEndPoints> PreviewEdges => _previewEdges;
        public NodeContextMenuViewModel NodeContextMenu { get; }
        public GraphContextMenuViewModel GraphContextMenu { get; }
        public EdgeContextMenuViewModel EdgeContextMenu { get; }

        public GraphViewModel()
        {
            NodeContextMenu = new NodeContextMenuViewModel(this);
            GraphContextMenu = new GraphContextMenuViewModel(this);
            EdgeContextMenu = new EdgeContextMenuViewModel(this);
        }

        public void AddTestData()
        {
            // サンプルノードを作成
            for (var i = 0; i < 4; i++)
            {
                var nodeId = NodeId.New();
                var inputPorts = new[]
                {
                    new InputPortViewModel(nodeId, 0, $"Input {i * 2}", this),
                    new InputPortViewModel(nodeId, 1, $"Input {i * 2 + 1}", this)
                };

                var outputPorts = new[]
                {
                    new OutputPortViewModel(nodeId, 0, $"Output {i}", this)
                };

                var nodeViewModel = new NodeViewModel(nodeId, $"Node {nodeId}", inputPorts, outputPorts, this);

                // ノードの位置を設定（横に並べる）
                var xOffset = i * 250;
                var yOffset = (i % 2) * 150; // ジグザグ配置
                nodeViewModel.SetPosition(new Vector2(xOffset, yOffset));

                AddNode(nodeViewModel);
            }

            var nodeIds = new List<NodeId>();
            foreach (var kvp in _nodes)
            {
                nodeIds.Add(kvp.Key);
            }

            if (nodeIds.Count >= 2)
            {
                CreateEdge(nodeIds[0], 0, nodeIds[1], 0);

                if (nodeIds.Count >= 3)
                {
                    CreateEdge(nodeIds[1], 0, nodeIds[2], 0);
                }
            }
        }

        public void AddNode(NodeViewModel node)
        {
            _nodes.Add(node.Id, node);
        }

        public void AddEdge(EdgeViewModel edge)
        {
            _edges.Add(edge.Id, edge);
        }

        public bool RemoveNode(NodeId nodeId)
        {
            var edgesToRemove = new List<EdgeId>();
            foreach (var (edgeId, edge) in _edges)
            {
                if (edge.OutputPort.NodeId == nodeId || edge.InputPort.NodeId == nodeId)
                {
                    edgesToRemove.Add(edgeId);
                }
            }

            foreach (var edgeId in edgesToRemove)
            {
                RemoveEdge(edgeId);
            }

            return _nodes.Remove(nodeId);
        }

        void IGraphController.ShowNodeContextMenu(NodeId nodeId, Vector2 position)
        {
            NodeContextMenu.Show(nodeId, position);
        }

        public void ShowGraphContextMenu(Vector2 position)
        {
            GraphContextMenu.Show(position);
        }

        public void ShowEdgeContextMenu(EdgeId edgeId, Vector2 position)
        {
            EdgeContextMenu.Show(edgeId, position);
        }

        public bool RemoveEdge(EdgeId edgeId)
        {
            return _edges.Remove(edgeId);
        }

        public bool CreateEdge(NodeId outputNodeId, int outputPortIndex, NodeId inputNodeId, int inputPortIndex)
        {
            if (outputNodeId == inputNodeId) return false;
            if (!_nodes.TryGetValue(outputNodeId, out var outputNode)) return false;
            if (!_nodes.TryGetValue(inputNodeId, out var inputNode)) return false;

            if (!outputNode.TryGetOutputPort(outputPortIndex, out var outputPort)) return false;
            if (!inputNode.TryGetInputPort(inputPortIndex, out var inputPort)) return false;

            var edgeViewModel = new EdgeViewModel(outputPort, inputPort, this);
            AddEdge(edgeViewModel);
            return true;
        }

        public bool TryCreateEdge(NodeId outputNodeId, int outputPortIndex, NodeId inputNodeId, int inputPortIndex)
        {
            return CreateEdge(outputNodeId, outputPortIndex, inputNodeId, inputPortIndex);
        }

        public IDisposable CreatePreviewEdge(IEdgeEndPoints endPoints)
        {
            var id = EdgeId.New();
            _previewEdges.Add(id, endPoints);
            return Disposable.Create(() => _previewEdges.Remove(id));
        }
    }
}
