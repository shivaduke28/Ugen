using System;
using R3;
using Ugen.Graphs.Manipulators;
using UnityEngine.UIElements;

namespace Ugen.Graphs.Ports
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
            _connector.OnCenterPanelPositionChanged()
                .Subscribe(port.UpdateOutputPosition)
                .AddTo(disposable);
            var edgeDragger = new PortPickerManipulator();
            edgeDragger.OnStartPanelPosition().Subscribe(port.StartPreviewEdge).AddTo(disposable);
            edgeDragger.OnMovePanelPosition()
                .Subscribe(port.UpdateInputPosition)
                .AddTo(disposable);
            edgeDragger.OnEnd().Subscribe(port.StopPreviewEdge).AddTo(disposable);
            _portPicker.PortData = port.PortData;
            _portPicker.AddManipulator(edgeDragger);
            return disposable;
        }
    }
}
