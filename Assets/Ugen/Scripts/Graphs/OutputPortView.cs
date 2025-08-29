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

        public OutputPortView(VisualElement container)
        {
            _root = container.Q<VisualElement>("output-port");
            _nameLabel = _root.Q<Label>("name");
            _connector = _root.Q<VisualElement>("connector");
        }

        public IDisposable Bind(OutputPortViewModel port)
        {
            _nameLabel.text = port.Name;
            var disposable = new CompositeDisposable();
            Observable.EveryValueChanged(this, view => view.GetConnectorWorldPosition())
                .Subscribe(pos => port.ConnectorWorldPosition.Value = pos)
                .AddTo(disposable);
            return disposable;
        }

        Vector2 GetConnectorWorldPosition() => _connector.worldBound.center;
    }
}
