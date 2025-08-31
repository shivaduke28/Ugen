using R3;
using Ugen.Graphs.Ports;
using UnityEngine;

namespace Ugen.Graphs
{
    public class EdgeViewModel : IEdgeEndPoints
    {
        public EdgeId Id { get; }
        public OutputPortViewModel OutputPort { get; }
        public InputPortViewModel InputPort { get; }
        public ReadOnlyReactiveProperty<Vector2> OutputPanelPosition => OutputPort.ConnectorPanelPosition;
        public ReadOnlyReactiveProperty<Vector2> InputPanelPosition => InputPort.ConnectorPanelPosition;

        public EdgeViewModel(OutputPortViewModel outputPort, InputPortViewModel inputPort)
        {
            Id = EdgeId.New();
            OutputPort = outputPort;
            InputPort = inputPort;
        }
    }
}
