using System;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public sealed class OutputPortView
    {
        readonly VisualElement _root;
        readonly Label _nameLabel;
        readonly PortConnectorView _connector;
        readonly PortPickerView _portPicker;

        public OutputPortView(VisualElement container)
        {
            _root = container.Q<VisualElement>("output-port");
            _nameLabel = _root.Q<Label>("name");
            _connector = _root.Q<PortConnectorView>();
            _portPicker = _root.Q<PortPickerView>();
        }

        public IDisposable Bind(OutputPortViewModel port)
        {
            _nameLabel.text = port.Name;
            var disposable = new CompositeDisposable();
            _connector.OnCenterWorldPositionChanged()
                .Subscribe(pos => port.ConnectorWorldPosition.Value = pos)
                .AddTo(disposable);
            var endPoints = new EdgePreviewEndPoints(startPosition: port.ConnectorWorldPosition);
            var edgeDragger = new EdgeDragger(port.EdgeCreationRequest, endPoints, port.EdgeCreator);
            edgeDragger.OnStart().Merge(edgeDragger.OnMove())
                .Subscribe(pos => endPoints.SetEndPosition(pos))
                .AddTo(disposable);
            _portPicker.AddManipulator(edgeDragger);
            _portPicker.OnEdgeCreationRequested()
                .Where(req => req.Direction == PortDirection.Input)
                .Subscribe(req => port.EdgeCreator.TryCreateEdge(port.NodeId, port.Index, req.NodeId, req.PortIndex))
                .AddTo(disposable);
            return disposable;
        }
    }
}
