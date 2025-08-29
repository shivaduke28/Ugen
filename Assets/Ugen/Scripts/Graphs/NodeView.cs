using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public class InputPort
    {
        public string Name { get; }

        public InputPort(string name)
        {
            Name = name;
        }
    }

    public class OutputPort
    {
        public string Name { get; }

        public OutputPort(string name)
        {
            Name = name;
        }
    }

    public class Node
    {
        public string Name { get; }
        public InputPort[] InputPorts { get; }
        public OutputPort[] OutputPorts { get; }

        public Node(string name, InputPort[] inputPorts, OutputPort[] outputPorts)
        {
            Name = name;
            InputPorts = inputPorts;
            OutputPorts = outputPorts;
        }
    }

    public class NodeView
    {
        readonly VisualElement _root;
        readonly Label _nameLabel;
        readonly VisualElement _inputPortContainer;
        readonly VisualElement _outputPortContainer;

        public NodeView(VisualElement container, Node node)
        {
            _root = container.Q<VisualElement>("node");
            _nameLabel = _root.Q<Label>("name");
            _inputPortContainer = _root.Q<VisualElement>("input-ports");
            _outputPortContainer = _root.Q<VisualElement>("output-ports");
        }
    }
}
