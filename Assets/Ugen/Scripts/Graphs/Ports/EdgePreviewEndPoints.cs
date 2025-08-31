using R3;
using UnityEngine;

namespace Ugen.Graphs.Ports
{
    public sealed class EdgePreviewEndPoints : IEdgeEndPoints
    {
        readonly ReactiveProperty<Vector2> _outputPanelPosition;
        readonly ReactiveProperty<Vector2> _inputPanelPosition;
        public ReadOnlyReactiveProperty<Vector2> OutputPanelPosition => _outputPanelPosition;

        public ReadOnlyReactiveProperty<Vector2> InputPanelPosition => _inputPanelPosition;

        public EdgePreviewEndPoints(
            ReactiveProperty<Vector2> outputPosition = null,
            ReactiveProperty<Vector2> inputPosition = null)
        {
            _outputPanelPosition = outputPosition ?? new ReactiveProperty<Vector2>();
            _inputPanelPosition = inputPosition ?? new ReactiveProperty<Vector2>();
        }

        public void UpdateOutputPosition(Vector2 position)
        {
            _outputPanelPosition.Value = position;
        }

        public void UpdateInputPosition(Vector2 position)
        {
            _inputPanelPosition.Value = position;
        }
    }
}
