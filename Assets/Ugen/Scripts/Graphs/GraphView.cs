using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public class GraphView
    {
        readonly VisualElement _root;
        readonly VisualElement _nodeLayer;

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
                var nodeView = new NodeView(nodeElement, nodeViewModel);

                // NodeViewをnodeLayerに追加して位置を調整
                _nodeLayer.Add(nodeView.Root);

                // ノードの位置を設定（横に並べる）
                var xOffset = i * 250;
                var yOffset = (i % 2) * 150; // ジグザグ配置
                nodeView.Root.style.left = xOffset;
                nodeView.Root.style.top = yOffset;
            }
        }
    }
}
