using R3;
using UnityEngine;

namespace Ugen.Graphs.Ports
{
    public sealed class EdgePreviewEndPoints : IEdgeEndPoints
    {
        readonly ReactiveProperty<Vector2> _startPosition;
        readonly ReactiveProperty<Vector2> _endPosition;
        public ReadOnlyReactiveProperty<Vector2> StartPosition => _startPosition;

        public ReadOnlyReactiveProperty<Vector2> EndPosition => _endPosition;

        public EdgePreviewEndPoints(
            ReactiveProperty<Vector2> startPosition = null,
            ReactiveProperty<Vector2> endPosition = null)
        {
            _startPosition = startPosition ?? new ReactiveProperty<Vector2>();
            _endPosition = endPosition ?? new ReactiveProperty<Vector2>();
        }

        public void SetStartPosition(Vector2 position)
        {
            _startPosition.Value = position;
        }

        public void SetEndPosition(Vector2 position)
        {
            _endPosition.Value = position;
        }
    }
}