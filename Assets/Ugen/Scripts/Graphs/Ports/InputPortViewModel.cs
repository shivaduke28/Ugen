using R3;
using Ugen.Graphs.Nodes;
using UnityEngine;

namespace Ugen.Graphs.Ports
{
    public class InputPortViewModel : PortViewModel
    {
        public PortData PortData { get; }

        readonly EdgePreviewEndPoints _edgePreviewEndPoints;
        readonly SerialDisposable _previewEdgeDisposable = new();
        readonly IGraphController _graphController;
        readonly ReactiveProperty<Vector2> _outputPosition = new();
        readonly ReactiveProperty<Vector2> _inputPosition = new();
        public ReadOnlyReactiveProperty<Vector2> InputPosition => _inputPosition;

        public InputPortViewModel(NodeId nodeId, int index, string name, IGraphController graphController) : base(nodeId, name)
        {
            _graphController = graphController;
            PortData = new PortData(nodeId, index, PortDirection.Input);
            _edgePreviewEndPoints = new EdgePreviewEndPoints(_outputPosition, _inputPosition);
        }

        public void UpdateOutputPosition(Vector2 panelPosition)
        {
            _outputPosition.Value = _graphController.ConvertPanelToGraph(panelPosition);
        }

        public void UpdateInputPosition(Vector2 panelPosition)
        {
            _inputPosition.Value = _graphController.ConvertPanelToGraph(panelPosition);
        }

        public void StartPreviewEdge(Vector2 panelPosition)
        {
            UpdateOutputPosition(panelPosition);
            _previewEdgeDisposable.Disposable = _graphController
                .CreatePreviewEdge(_edgePreviewEndPoints);
        }

        public void StopPreviewEdge(PortData? otherPortData)
        {
            _previewEdgeDisposable.Disposable = null;
            if (otherPortData.HasValue)
            {
                var other = otherPortData.Value;
                _graphController.CreateEdge(other.NodeId, other.Index, PortData.NodeId, PortData.Index);
            }
        }
    }
}
