using R3;
using Ugen.Graphs.Nodes;
using UnityEngine;

namespace Ugen.Graphs.Ports
{
    public class OutputPortViewModel : PortViewModel
    {
        public PortData PortData { get; }

        readonly EdgePreviewEndPoints _edgePreviewEndPoints;
        readonly SerialDisposable _previewEdgeDisposable = new();
        readonly IGraphController _graphController;
        public ReadOnlyReactiveProperty<Vector2> OutputPosition => _outputPosition;
        readonly ReactiveProperty<Vector2> _outputPosition = new();
        readonly ReactiveProperty<Vector2> _inputPosition = new();

        public OutputPortViewModel(NodeId nodeId, int index, string name, IGraphController graphController) : base(nodeId, name)
        {
            _graphController = graphController;
            PortData = new PortData(nodeId, index, PortDirection.Output);
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

        public void StartPreviewEdge(Vector2 point)
        {
            UpdateInputPosition(point);
            _previewEdgeDisposable.Disposable = _graphController
                .CreatePreviewEdge(_edgePreviewEndPoints);
        }

        public void StopPreviewEdge(PortData? otherPortData)
        {
            _previewEdgeDisposable.Disposable = null;
            if (otherPortData.HasValue)
            {
                var other = otherPortData.Value;
                _graphController.CreateEdge(PortData.NodeId, PortData.Index, other.NodeId, other.Index);
            }
        }
    }
}
