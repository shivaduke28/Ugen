using System;
using R3;
using UnityEngine.UIElements;

namespace Ugen.Graphs
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
            var edgeDragger = new EdgeDragger(port.EdgeCreationRequest);
            _portPicker.AddManipulator(edgeDragger);
            _portPicker.OnEdgeCreationRequested()
                .Subscribe(req => port.TryCreateEdge(req.NodeId, req.PortIndex))
                .AddTo(disposable);
            return disposable;
        }
    }
}
