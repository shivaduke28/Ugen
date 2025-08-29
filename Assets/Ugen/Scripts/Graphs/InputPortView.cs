using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public sealed class InputPortView
    {
        readonly VisualElement _root;
        readonly Label _nameLabel;
        readonly VisualElement _connector;

        public InputPortView(VisualElement container)
        {
            _root = container.Q<VisualElement>("input-port");
            _nameLabel = _root.Q<Label>("name");
            _connector = _root.Q<VisualElement>("connector");
        }
    }
}
