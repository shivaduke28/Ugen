using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public class NodeView
    {
        public VisualElement Root => _root;
        readonly VisualElement _root;
        readonly Label _nameLabel;
        readonly VisualElement _inputPortContainer;
        readonly VisualElement _outputPortContainer;
        readonly List<InputPortView> _inputPortViews = new();
        readonly List<OutputPortView> _outputPortViews = new();

        public IDisposable Bind(NodeViewModel nodeViewModel)
        {
            // ノード名を設定
            _nameLabel.text = nodeViewModel.Name;

            foreach (var inputPort in nodeViewModel.InputPorts)
            {
                var portElement = VisualElementFactory.Instance.CreateInputPort();
                _inputPortContainer.Add(portElement);

                var inputPortView = new InputPortView(portElement, inputPort);
                _inputPortViews.Add(inputPortView);
            }

            foreach (var outputPort in nodeViewModel.OutputPorts)
            {
                var portElement = VisualElementFactory.Instance.CreateOutputPort();
                _outputPortContainer.Add(portElement);

                var outputPortView = new OutputPortView(portElement, outputPort);
                _outputPortViews.Add(outputPortView);
            }

            return nodeViewModel.Position.Subscribe(SetPosition);
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
    }
}
