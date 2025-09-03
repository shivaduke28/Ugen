using R3;
using Ugen.Graphs.Ports;
using UnityEngine;

namespace Ugen.Graphs.Edges
{
    public class EdgeViewModel : IEdgeEndPoints
    {
        public EdgeId Id { get; }
        public OutputPortViewModel OutputPort { get; }
        public InputPortViewModel InputPort { get; }
        public ReadOnlyReactiveProperty<Vector2> OutputPosition => OutputPort.OutputPosition;
        public ReadOnlyReactiveProperty<Vector2> InputPosition => InputPort.InputPosition;

        public EdgeViewModel(OutputPortViewModel outputPort, InputPortViewModel inputPort)
        {
            Id = EdgeId.New();
            OutputPort = outputPort;
            InputPort = inputPort;
        }
    }
}
