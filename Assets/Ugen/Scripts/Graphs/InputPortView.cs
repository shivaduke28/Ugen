using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public sealed class InputPortView
    {
        readonly VisualElement _root;
        readonly Label _nameLabel;
        readonly VisualElement _connector;
        readonly InputPortViewModel _inputPort;

        public InputPortViewModel InputPort => _inputPort;

        public InputPortView(VisualElement container, InputPortViewModel inputPort)
        {
            _root = container.Q<VisualElement>("input-port");
            _nameLabel = _root.Q<Label>("name");
            _connector = _root.Q<VisualElement>("connector");
            _inputPort = inputPort;

            _nameLabel.text = inputPort.Name;
        }
    }
}
