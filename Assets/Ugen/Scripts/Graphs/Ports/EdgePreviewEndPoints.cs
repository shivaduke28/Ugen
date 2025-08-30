using R3;
using UnityEngine;

namespace Ugen.Graphs.Ports
{
    public sealed class EdgePreviewEndPoints : IEdgeEndPoints
    {
        readonly ReactiveProperty<Vector2> _outputPosition;
        readonly ReactiveProperty<Vector2> _inputPosition;
        public ReadOnlyReactiveProperty<Vector2> OutputPosition => _outputPosition;

        public ReadOnlyReactiveProperty<Vector2> InputPosition => _inputPosition;

        public EdgePreviewEndPoints(
            ReactiveProperty<Vector2> outputPosition = null,
            ReactiveProperty<Vector2> inputPosition = null)
        {
            _outputPosition = outputPosition ?? new ReactiveProperty<Vector2>();
            _inputPosition = inputPosition ?? new ReactiveProperty<Vector2>();
        }

        public void UpdateOutputPosition(Vector2 position)
        {
            _outputPosition.Value = position;
        }

        public void UpdateInputPosition(Vector2 position)
        {
            _inputPosition.Value = position;
        }
    }
}
