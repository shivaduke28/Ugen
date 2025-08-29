using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public sealed class OutputPortView
    {
        readonly VisualElement _root;
        readonly Label _nameLabel;
        readonly VisualElement _connector;
        readonly OutputPortViewModel _outputPortViewModel;

        public OutputPortViewModel OutputPort => _outputPortViewModel;

        public OutputPortView(VisualElement container, OutputPortViewModel outputPort)
        {
            _root = container.Q<VisualElement>("output-port");
            _nameLabel = _root.Q<Label>("name");
            _connector = _root.Q<VisualElement>("connector");
            _outputPortViewModel = outputPort;

            _nameLabel.text = outputPort.Name;
        }
    }
}
