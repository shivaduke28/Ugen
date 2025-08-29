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

        public InputPortView(VisualElement container)
        {
            _root = container.Q<VisualElement>("input-port");
            _nameLabel = _root.Q<Label>("name");
            _connector = _root.Q<VisualElement>("connector");
        }

        public IDisposable Bind(InputPortViewModel inputPort)
        {
            _nameLabel.text = inputPort.Name;
            var disposable = new CompositeDisposable();
            Observable.EveryValueChanged(this, view => view.GetConnectorWorldPosition())
                .Subscribe(pos => inputPort.ConnectorWorldPosition.Value = pos)
                .AddTo(disposable);
            return disposable;
        }

        Vector2 GetConnectorWorldPosition() => _connector.worldBound.center;
    }
}
