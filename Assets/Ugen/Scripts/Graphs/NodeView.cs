using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public class NodeView
    {
        public VisualElement Root => _root;
        readonly VisualElement _root;
        readonly Label _nameLabel;
        readonly VisualElement _inputPortContainer;
        readonly VisualElement _outputPortContainer;
        readonly InputPortView[] _inputPortViews;
        readonly OutputPortView[] _outputPortViews;

        public NodeView(VisualElement container, NodeViewModel nodeViewModel)
        {
            _root = container.Q<VisualElement>("node");
            _nameLabel = _root.Q<Label>("name");
            _inputPortContainer = _root.Q<VisualElement>("input-ports");
            _outputPortContainer = _root.Q<VisualElement>("output-ports");

            // ノード名を設定
            _nameLabel.text = nodeViewModel.Name;

            // InputPortViewを作成して初期化
            _inputPortViews = new InputPortView[nodeViewModel.InputPorts.Length];
            for (var i = 0; i < nodeViewModel.InputPorts.Length; i++)
            {
                var inputPort = nodeViewModel.InputPorts[i];
                var portElement = VisualElementFactory.Instance.CreateInputPort();
                _inputPortContainer.Add(portElement);

                var inputPortView = new InputPortView(portElement, inputPort);
                _inputPortViews[i] = inputPortView;
            }

            // OutputPortViewを作成して初期化
            _outputPortViews = new OutputPortView[nodeViewModel.OutputPorts.Length];
            for (var i = 0; i < nodeViewModel.OutputPorts.Length; i++)
            {
                var outputPort = nodeViewModel.OutputPorts[i];
                var portElement = VisualElementFactory.Instance.CreateOutputPort();
                _outputPortContainer.Add(portElement);

                var outputPortView = new OutputPortView(portElement, outputPort);
                _outputPortViews[i] = outputPortView;
            }
        }
    }
}
