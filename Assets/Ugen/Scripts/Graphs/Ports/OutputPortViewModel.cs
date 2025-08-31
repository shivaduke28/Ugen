using R3;
using Ugen.Graphs.Nodes;
using UnityEngine;

namespace Ugen.Graphs.Ports
{
    public class OutputPortViewModel : PortViewModel
    {
        public override PortDirection Direction => PortDirection.Output;
        public override PortData PortData { get; }

        readonly EdgePreviewEndPoints _edgePreviewEndPoints;
        readonly SerialDisposable _previewEdgeDisposable = new();
        readonly IGraphController _graphController;

        public OutputPortViewModel(NodeId nodeId, int index, string name, IGraphController graphController) : base(nodeId, index, name)
        {
            _graphController = graphController;
            PortData = new PortData(nodeId, index, PortDirection.Output);
            _edgePreviewEndPoints = new EdgePreviewEndPoints(outputPosition: ConnectorWorldPosition);
        }

        public void UpdatePreviewOtherEnd(Vector2 point)
        {
            _edgePreviewEndPoints.UpdateInputPosition(point);
        }

        public void StartPreviewEdge(Vector2 point)
        {
            _edgePreviewEndPoints.UpdateInputPosition(point);
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
