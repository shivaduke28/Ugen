using R3;
using UnityEngine;

namespace Ugen.Graphs
{
    public class EdgeViewModel
    {
        OutputPortViewModel OutputPort { get; }
        InputPortViewModel InputPort { get; }
        public ReactiveProperty<Vector2> StartPosition => OutputPort.ConnectorWorldPosition;
        public ReactiveProperty<Vector2> EndPosition => InputPort.ConnectorWorldPosition;

        public EdgeViewModel(OutputPortViewModel outputPort, InputPortViewModel inputPort)
        {
            OutputPort = outputPort;
            InputPort = inputPort;
        }
    }
}
