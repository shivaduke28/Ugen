using R3;
using UnityEngine;

namespace Ugen.Graphs.Ports
{
    public class InputPortViewModel : PortViewModel
    {
        public override PortDirection Direction => PortDirection.Input;
        public override PortData PortData { get; }

        readonly EdgePreviewEndPoints _edgePreviewEndPoints;
        readonly SerialDisposable _previewEdgeDisposable = new();
        readonly IGraphController _graphController;

        public InputPortViewModel(NodeId nodeId, int index, string name, IGraphController graphController) : base(nodeId, index, name)
        {
            _graphController = graphController;
            PortData = new PortData(nodeId, index, PortDirection.Input);
            _edgePreviewEndPoints = new EdgePreviewEndPoints(inputPosition: ConnectorWorldPosition);
        }

        public void UpdatePreviewOtherEnd(Vector2 point)
        {
            _edgePreviewEndPoints.UpdateOutputPosition(point);
        }

        public void StartPreviewEdge(Vector2 point)
        {
            _edgePreviewEndPoints.UpdateOutputPosition(point);
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
