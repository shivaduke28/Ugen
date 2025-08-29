namespace Ugen.Graphs
{
    public class NodeViewModel
    {
        public string Name { get; }
        public InputPortViewModel[] InputPorts { get; }
        public OutputPortViewModel[] OutputPorts { get; }

        public NodeViewModel(string name, InputPortViewModel[] inputPorts, OutputPortViewModel[] outputPorts)
        {
            Name = name;
            InputPorts = inputPorts;
            OutputPorts = outputPorts;
        }
    }
}
