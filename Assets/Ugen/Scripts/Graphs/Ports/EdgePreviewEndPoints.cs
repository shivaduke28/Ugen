using R3;
using UnityEngine;

namespace Ugen.Graphs.Ports
{
    public sealed class EdgePreviewEndPoints : IEdgeEndPoints
    {
        public ReadOnlyReactiveProperty<Vector2> OutputPosition { get; }

        public ReadOnlyReactiveProperty<Vector2> InputPosition { get; }

        public EdgePreviewEndPoints(
            ReactiveProperty<Vector2> outputPosition,
            ReactiveProperty<Vector2> inputPosition)
        {
            OutputPosition = outputPosition;
            InputPosition = inputPosition;
        }
    }
}
