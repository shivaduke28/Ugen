using System;
using R3;
using Ugen.Graphs.Manipulators;
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
            _connector.OnCenterPanelPositionChanged()
                .Subscribe(port.UpdateInputPosition)
                .AddTo(disposable);
            var edgeDragger = new PortPickerManipulator();
            edgeDragger.OnStartPanelPosition().Subscribe(port.StartPreviewEdge).AddTo(disposable);
            edgeDragger.OnMovePanelPosition()
                .Subscribe(port.UpdateOutputPosition)
                .AddTo(disposable);
            edgeDragger.OnEnd().Subscribe(port.StopPreviewEdge).AddTo(disposable);
            _portPicker.PortData = port.PortData;
            _portPicker.AddManipulator(edgeDragger);
            return disposable;
        }
    }
}
