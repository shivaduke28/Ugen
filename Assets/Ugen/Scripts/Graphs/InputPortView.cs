using System;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public sealed class InputPortView
    {
        readonly VisualElement _root;
        readonly Label _nameLabel;
        readonly VisualElement _connector;
        readonly InputPortViewModel _inputPort;

        public InputPortView(VisualElement container)
        {
            _root = container.Q<VisualElement>("input-port");
            _nameLabel = _root.Q<Label>("name");
            _connector = _root.Q<VisualElement>("connector");
        }

        public IDisposable Bind(InputPortViewModel inputPort)
        {
            _nameLabel.text = inputPort.Name;
            return Observable.EveryValueChanged(this, view => view.GetConnectorWorldPosition())
                .Subscribe(pos => inputPort.ConnectorWorldPosition.Value = pos);
        }

        public Vector2 GetConnectorWorldPosition()
        {
            var worldBound = _connector.worldBound;
            return worldBound.center;
        }
    }
}
