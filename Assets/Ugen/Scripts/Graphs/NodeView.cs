using System;
using System.Collections.Generic;
using R3;
using Ugen.Graphs.Ports;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public class NodeView : IDisposable
    {
        public VisualElement Root => _root;
        readonly VisualElement _root;
        readonly Label _nameLabel;
        readonly VisualElement _inputPortContainer;
        readonly VisualElement _outputPortContainer;
        readonly SerialDisposable _disposable = new();
        public List<InputPortView> InputPortViews { get; } = new();
        public List<OutputPortView> OutputPortViews { get; } = new();

        public IDisposable Bind(NodeViewModel nodeViewModel)
        {
            var disposable = new CompositeDisposable();
            _disposable.Disposable = disposable;
            _nameLabel.text = nodeViewModel.Name;

            var dragManipulator = new DragManipulator();
            _root.AddManipulator(dragManipulator);
            dragManipulator.OnMoveDelta().Subscribe(nodeViewModel.MoveDelta).AddTo(disposable);
            Disposable.Create(() => _root.RemoveManipulator(dragManipulator)).AddTo(disposable);

            foreach (var inputPort in nodeViewModel.InputPorts)
            {
                var portElement = VisualElementFactory.Instance.CreateInputPort();
                _inputPortContainer.Add(portElement);

                var inputPortView = new InputPortView(portElement);
                InputPortViews.Add(inputPortView);
                inputPortView.Bind(inputPort).AddTo(disposable);
            }

            foreach (var outputPort in nodeViewModel.OutputPorts)
            {
                var portElement = VisualElementFactory.Instance.CreateOutputPort();
                _outputPortContainer.Add(portElement);

                var outputPortView = new OutputPortView(portElement);
                OutputPortViews.Add(outputPortView);
                outputPortView.Bind(outputPort).AddTo(disposable);
            }

            nodeViewModel.Position.Subscribe(SetPosition).AddTo(disposable);
            return disposable;
        }

        void SetPosition(Vector2 position)
        {
            _root.style.left = position.x;
            _root.style.top = position.y;
        }

        public NodeView(VisualElement container)
        {
            _root = container.Q<VisualElement>("node");
            _nameLabel = _root.Q<Label>("name");
            _inputPortContainer = _root.Q<VisualElement>("input-ports");
            _outputPortContainer = _root.Q<VisualElement>("output-ports");
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
