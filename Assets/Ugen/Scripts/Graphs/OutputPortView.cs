using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public sealed class OutputPortView
    {
        readonly VisualElement _root;
        readonly Label _nameLabel;
        readonly VisualElement _connector;

        public OutputPortView(VisualElement container)
        {
            _root = container.Q<VisualElement>("output-port");
            _nameLabel = _root.Q<Label>("name");
            _connector = _root.Q<VisualElement>("connector");
        }
    }
}
