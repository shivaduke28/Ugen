using System.Collections.Generic;
using R3;
using Ugen.Graphs.ContextMenu;
using Ugen.Graphs.Ports;
using UnityEngine;

namespace Ugen.Graphs.GraphContextMenu
{
    public sealed class GraphContextMenuViewModel
    {
        readonly ReactiveProperty<GraphContextMenuState> _state = new(GraphContextMenuState.Hidden);
        readonly IGraphController _graphController;

        public ReadOnlyReactiveProperty<GraphContextMenuState> State => _state;

        public GraphContextMenuViewModel(IGraphController graphController)
        {
            _graphController = graphController;
        }

        public void Show(Vector2 position)
        {
            _state.Value = new GraphContextMenuState(true, position);
        }

        public void Hide()
        {
            _state.Value = GraphContextMenuState.Hidden;
        }

        public List<ContextMenuItemViewModel> GetMenuItems()
        {
            var items = new List<ContextMenuItemViewModel>();

            // グラフコンテキストメニューの項目を定義
            items.Add(new ContextMenuItemViewModel(new ContextMenuItemState("Create Float Node", true, () => CreateNode("Float"))));

            return items;
        }

        void CreateNode(string nodeType)
        {
            if (_state.Value.IsVisible)
            {
                var position = _state.Value.Position;
                CreateNodeAtPosition(nodeType, position);
            }

            Hide();
        }

        void CreateNodeAtPosition(string nodeType, Vector2 position)
        {
            var nodeId = NodeId.New();

            var inputPorts = new[]
            {
                new InputPortViewModel(nodeId, 0, "Input 0", _graphController),
                new InputPortViewModel(nodeId, 1, "Input 1", _graphController)
            };

            var outputPorts = new[]
            {
                new OutputPortViewModel(nodeId, 0, "Output", _graphController)
            };

            var nodeViewModel = new NodeViewModel(nodeId, $"{nodeType} Node", inputPorts, outputPorts, _graphController);
            nodeViewModel.SetPosition(position);

            _graphController.AddNode(nodeViewModel);
        }
    }
}
