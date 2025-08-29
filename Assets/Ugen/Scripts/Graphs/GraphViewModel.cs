using System.Collections.Generic;
using ObservableCollections;
using UnityEngine;

namespace Ugen.Graphs
{
    public class GraphViewModel
    {
        readonly ObservableDictionary<NodeId, NodeViewModel> _nodes = new();
        readonly ObservableDictionary<EdgeId, EdgeViewModel> _edges = new();

        public IReadOnlyObservableDictionary<NodeId, NodeViewModel> Nodes => _nodes;
        public IReadOnlyObservableDictionary<EdgeId, EdgeViewModel> Edges => _edges;

        public void AddTestData()
        {
            // サンプルノードを作成
            for (var i = 0; i < 4; i++)
            {
                var inputPorts = new[]
                {
                    new InputPortViewModel($"Input {i * 2}"),
                    new InputPortViewModel($"Input {i * 2 + 1}")
                };

                var outputPorts = new[]
                {
                    new OutputPortViewModel($"Output {i}")
                };

                var nodeViewModel = new NodeViewModel($"Node {i}", inputPorts, outputPorts);

                // ノードの位置を設定（横に並べる）
                var xOffset = i * 250;
                var yOffset = (i % 2) * 150; // ジグザグ配置
                nodeViewModel.SetPosition(new Vector2(xOffset, yOffset));

                AddNode(nodeViewModel);
            }

            // サンプルのEdgeを作成
            var nodeList = new List<NodeViewModel>();
            foreach (var kvp in _nodes)
            {
                nodeList.Add(kvp.Value);
            }

            if (nodeList.Count >= 2)
            {
                CreateEdge(nodeList[0], 0, nodeList[1], 0);

                if (nodeList.Count >= 3)
                {
                    CreateEdge(nodeList[1], 0, nodeList[2], 0);
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

        public void CreateEdge(NodeViewModel outputNode, int outputPortIndex, NodeViewModel inputNode, int inputPortIndex)
        {
            if (outputNode.OutputPorts.Length <= outputPortIndex ||
                inputNode.InputPorts.Length <= inputPortIndex)
                return;

            var outputPort = outputNode.OutputPorts[outputPortIndex];
            var inputPort = inputNode.InputPorts[inputPortIndex];

            var edgeViewModel = new EdgeViewModel(outputPort, inputPort);
            AddEdge(edgeViewModel);
        }

        public NodeViewModel CreateNode(string name, int inputPortCount, int outputPortCount)
        {
            var inputPorts = new InputPortViewModel[inputPortCount];
            for (var i = 0; i < inputPortCount; i++)
            {
                inputPorts[i] = new InputPortViewModel($"Input {i}");
            }

            var outputPorts = new OutputPortViewModel[outputPortCount];
            for (var i = 0; i < outputPortCount; i++)
            {
                outputPorts[i] = new OutputPortViewModel($"Output {i}");
            }

            var node = new NodeViewModel(name, inputPorts, outputPorts);
            AddNode(node);
            return node;
        }
    }
}
