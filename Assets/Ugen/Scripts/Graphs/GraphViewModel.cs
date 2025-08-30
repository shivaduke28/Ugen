using System;
using System.Collections.Generic;
using ObservableCollections;
using R3;
using Ugen.Graphs.Ports;
using UnityEngine;

namespace Ugen.Graphs
{
    public class GraphViewModel
    {
        readonly ObservableDictionary<NodeId, NodeViewModel> _nodes = new();
        readonly ObservableDictionary<EdgeId, EdgeViewModel> _edges = new();
        readonly ObservableDictionary<EdgeId, IEdgeEndPoints> _previewEdges = new();
        readonly EdgeCreator _edgeCreator;

        public IReadOnlyObservableDictionary<NodeId, NodeViewModel> Nodes => _nodes;
        public IReadOnlyObservableDictionary<EdgeId, EdgeViewModel> Edges => _edges;
        public IReadOnlyObservableDictionary<EdgeId, IEdgeEndPoints> PreviewEdges => _previewEdges;

        public GraphViewModel()
        {
            _edgeCreator = new EdgeCreator(this);
        }

        public void AddTestData()
        {
            // サンプルノードを作成
            for (var i = 0; i < 4; i++)
            {
                var nodeId = NodeId.New();
                var inputPorts = new[]
                {
                    new InputPortViewModel(nodeId, 0, $"Input {i * 2}", _edgeCreator),
                    new InputPortViewModel(nodeId, 1, $"Input {i * 2 + 1}", _edgeCreator)
                };

                var outputPorts = new[]
                {
                    new OutputPortViewModel(nodeId, 0, $"Output {i}", _edgeCreator)
                };

                var nodeViewModel = new NodeViewModel(nodeId, $"Node {nodeId}", inputPorts, outputPorts);

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
                TryCreateEdge(nodeIds[0], 0, nodeIds[1], 0);

                if (nodeIds.Count >= 3)
                {
                    TryCreateEdge(nodeIds[1], 0, nodeIds[2], 0);
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

        public void RemoveNode(NodeId nodeId)
        {
            // TODO: nodeからつながっているedgeを削除する
            _nodes.Remove(nodeId);
        }

        public void RemoveEdge(EdgeId edgeId)
        {
            _edges.Remove(edgeId);
        }

        public bool TryCreateEdge(NodeId outputNodeId, int outputPortIndex, NodeId inputNodeId, int inputPortIndex)
        {
            if (outputNodeId == inputNodeId) return false;
            if (!_nodes.TryGetValue(outputNodeId, out var outputNode)) return false;
            if (!_nodes.TryGetValue(inputNodeId, out var inputNode)) return false;

            if (outputNode.OutputPorts.Length <= outputPortIndex ||
                inputNode.InputPorts.Length <= inputPortIndex)
                return false;

            var outputPort = outputNode.OutputPorts[outputPortIndex];
            var inputPort = inputNode.InputPorts[inputPortIndex];

            var edgeViewModel = new EdgeViewModel(outputPort, inputPort);
            AddEdge(edgeViewModel);
            return true;
        }

        public IDisposable CreatePreviewEdge(IEdgeEndPoints endPoints)
        {
            var id = EdgeId.New();
            _previewEdges.Add(id, endPoints);
            return Disposable.Create(() => _previewEdges.Remove(id));
        }

        public NodeViewModel CreateNode(string name, int inputPortCount, int outputPortCount)
        {
            var nodeId = NodeId.New();
            var inputPorts = new InputPortViewModel[inputPortCount];
            for (var i = 0; i < inputPortCount; i++)
            {
                inputPorts[i] = new InputPortViewModel(nodeId, i, $"Input {i}", _edgeCreator);
            }

            var outputPorts = new OutputPortViewModel[outputPortCount];
            for (var i = 0; i < outputPortCount; i++)
            {
                outputPorts[i] = new OutputPortViewModel(nodeId, i, $"Output {i}", _edgeCreator);
            }

            var node = new NodeViewModel(nodeId, name, inputPorts, outputPorts);
            AddNode(node);
            return node;
        }
    }
}
