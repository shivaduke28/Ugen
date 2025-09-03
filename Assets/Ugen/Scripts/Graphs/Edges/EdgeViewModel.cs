using System;
using R3;
using Ugen.Graphs.Ports;
using UnityEngine;

namespace Ugen.Graphs.Edges
{
    public class EdgeViewModel : IEdgeEndPoints, IDisposable
    {
        public EdgeId Id { get; }
        public OutputPortViewModel OutputPort { get; }
        public InputPortViewModel InputPort { get; }
        public ReadOnlyReactiveProperty<Vector2> OutputPosition => OutputPort.OutputPosition;
        public ReadOnlyReactiveProperty<Vector2> InputPosition => InputPort.InputPosition;
        readonly IEdge _edge;

        public EdgeViewModel(IEdge edge, OutputPortViewModel outputPort, InputPortViewModel inputPort)
        {
            _edge = edge;
            Id = EdgeId.New();
            OutputPort = outputPort;
            InputPort = inputPort;
        }

        public void Dispose()
        {
            _edge.Dispose();
        }
    }
}
