using System;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public class GraphView : IDisposable
    {
        readonly VisualElement _root;
        readonly VisualElement _nodeLayer;
        readonly CompositeDisposable _disposables = new();

        public GraphView(VisualElement container)
        {
            _root = container.Q<VisualElement>("graph");
            _nodeLayer = _root.Q<VisualElement>("node-layer");

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

                // NodeViewを作成
                var nodeElement = VisualElementFactory.Instance.CreateNode();
                var nodeView = new NodeView(nodeElement);

                // NodeViewをnodeLayerに追加して位置を調整
                _nodeLayer.Add(nodeView.Root);
                nodeView.Bind(nodeViewModel).AddTo(_disposables);

                // ノードの位置を設定（横に並べる）
                var xOffset = i * 250;
                var yOffset = (i % 2) * 150; // ジグザグ配置
                nodeViewModel.SetPosition(new Vector2(xOffset, yOffset));
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
