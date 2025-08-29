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
        readonly VisualElement _connector;
        readonly OutputPortViewModel _outputPortViewModel;

        public OutputPortView(VisualElement container)
        {
            _root = container.Q<VisualElement>("output-port");
            _nameLabel = _root.Q<Label>("name");
            _connector = _root.Q<VisualElement>("connector");
        }

        public IDisposable Bind(OutputPortViewModel port)
        {
            _nameLabel.text = port.Name;
            return Observable.EveryValueChanged(this, view => view.GetConnectorWorldPosition())
                .Subscribe(pos => port.ConnectorWorldPosition.Value = pos);
        }


        public Vector2 GetConnectorWorldPosition()
        {
            var worldBound = _connector.worldBound;
            return worldBound.center;
        }
    }
}
