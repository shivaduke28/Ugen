using R3;
using UnityEngine;

namespace Ugen.Graphs
{
    public interface IEdgeEndPoints
    {
        ReadOnlyReactiveProperty<Vector2> StartPosition { get; }
        ReadOnlyReactiveProperty<Vector2> EndPosition { get; }
    }

    public class EdgeViewModel : IEdgeEndPoints
    {
        public EdgeId Id { get; }
        OutputPortViewModel OutputPort { get; }
        InputPortViewModel InputPort { get; }
        public ReadOnlyReactiveProperty<Vector2> StartPosition => OutputPort.ConnectorWorldPosition;
        public ReadOnlyReactiveProperty<Vector2> EndPosition => InputPort.ConnectorWorldPosition;

        public EdgeViewModel(OutputPortViewModel outputPort, InputPortViewModel inputPort)
        {
            Id = EdgeId.New();
            OutputPort = outputPort;
            InputPort = inputPort;
        }
    }
}
