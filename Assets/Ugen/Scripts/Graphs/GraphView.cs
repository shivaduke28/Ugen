using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public class GraphView : IDisposable
    {
        readonly VisualElement _root;
        readonly VisualElement _nodeLayer;
        readonly VisualElement _edgeLayer;
        readonly CompositeDisposable _disposables = new();
        readonly List<NodeViewModel> _nodeViewModels = new();
        readonly List<NodeView> _nodeViews = new();
        readonly List<EdgeView> _edgeViews = new();

        public GraphView(VisualElement container)
        {
            _root = container.Q<VisualElement>("graph");
            _nodeLayer = _root.Q<VisualElement>("node-layer");
            _edgeLayer = _root.Q<VisualElement>("edge-layer");

            for (var i = 0; i < 4; i++)
            {
                // Node View Modelを作成
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
                _nodeViewModels.Add(nodeViewModel);

                // NodeViewを作成
                var nodeElement = VisualElementFactory.Instance.CreateNode();
                var nodeView = new NodeView(nodeElement);

                // NodeViewをnodeLayerに追加して位置を調整
                _nodeLayer.Add(nodeView.Root);
                nodeView.Bind(nodeViewModel).AddTo(_disposables);
                _nodeViews.Add(nodeView);

                // ノードの位置を設定（横に並べる）
                var xOffset = i * 250;
                var yOffset = (i % 2) * 150; // ジグザグ配置
                nodeViewModel.SetPosition(new Vector2(xOffset, yOffset));
            }

            // サンプルのEdgeを作成（Node 0のOutput 0 → Node 1のInput 0）
            if (_nodeViews.Count >= 2)
            {
                CreateEdge(_nodeViewModels[0], 0, _nodeViewModels[1], 0);

                // Node 1のOutput 0 → Node 2のInput 0
                if (_nodeViews.Count >= 3)
                {
                    CreateEdge(_nodeViewModels[1], 0, _nodeViewModels[2], 0);
                }
            }
        }

        void CreateEdge(NodeViewModel outputNode, int outputPortIndex, NodeViewModel inputNode, int inputPortIndex)
        {
            if (outputNode.OutputPorts.Length <= outputPortIndex ||
                inputNode.InputPorts.Length <= inputPortIndex)
                return;

            var outputPort = outputNode.OutputPorts[outputPortIndex];
            var inputPort = inputNode.InputPorts[inputPortIndex];

            var edgeViewModel = new EdgeViewModel(
                outputPort,
                inputPort);

            var edgeView = new EdgeView(edgeViewModel);
            _edgeLayer.Add(edgeView);
            _edgeViews.Add(edgeView);
        }

        public void Dispose()
        {
            foreach (var edgeView in _edgeViews)
            {
                edgeView.Dispose();
            }

            _disposables.Dispose();
        }
    }
}
