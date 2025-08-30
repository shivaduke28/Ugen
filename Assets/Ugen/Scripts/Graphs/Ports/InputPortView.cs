using System;
using R3;
using UnityEngine.UIElements;

namespace Ugen.Graphs.Ports
{
    public sealed class InputPortView
    {
        readonly VisualElement _root;
        readonly Label _nameLabel;
        readonly PortConnectorView _connector;
        readonly PortPickerView _portPicker;

        public InputPortView(VisualElement container)
        {
            _root = container.Q<VisualElement>("input-port");
            _nameLabel = _root.Q<Label>("name");
            _connector = _root.Q<PortConnectorView>();
            _portPicker = _root.Q<PortPickerView>();
        }

        public IDisposable Bind(InputPortViewModel port)
        {
            _nameLabel.text = port.Name;
            var disposable = new CompositeDisposable();
            _connector.OnCenterWorldPositionChanged()
                .Subscribe(pos => port.ConnectorWorldPosition.Value = pos)
                .AddTo(disposable);
            var endPoints = new EdgePreviewEndPoints(inputPosition: port.ConnectorWorldPosition);
            var edgeDragger = new EdgePreviewDragger(port.PortData, endPoints, port.GraphController);

            edgeDragger.OnStart().Merge(edgeDragger.OnMove())
                .Subscribe(pos => endPoints.UpdateOutputPosition(pos))
                .AddTo(disposable);
            _portPicker.AddManipulator(edgeDragger);
            _portPicker.OnEdgeCreationRequested()
                .Where(req => req.Direction == PortDirection.Output)
                .Subscribe(req => port.GraphController.CreateEdge(req.NodeId, req.PortIndex, port.NodeId, port.Index))
                .AddTo(disposable);
            return disposable;
        }
    }
}
