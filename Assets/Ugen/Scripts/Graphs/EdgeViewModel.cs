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
        public IGraphController GraphController { get; }
        public ReadOnlyReactiveProperty<Vector2> StartPosition => OutputPort.ConnectorWorldPosition;
        public ReadOnlyReactiveProperty<Vector2> EndPosition => InputPort.ConnectorWorldPosition;

        public EdgeViewModel(OutputPortViewModel outputPort, InputPortViewModel inputPort, IGraphController graphController)
        {
            Id = EdgeId.New();
            OutputPort = outputPort;
            InputPort = inputPort;
            GraphController = graphController;
        }
    }
}
