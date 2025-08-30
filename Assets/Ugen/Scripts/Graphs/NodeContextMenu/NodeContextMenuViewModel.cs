using System.Collections.Generic;
using R3;
using Ugen.Graphs.ContextMenu;
using UnityEngine;

namespace Ugen.Graphs.NodeContextMenu
{
    public class NodeContextMenuViewModel
    {
        readonly IGraphController _graphController;
        readonly ReactiveProperty<NodeContextMenuState> _state = new();

        public NodeContextMenuViewModel(IGraphController graphController)
        {
            _graphController = graphController;
        }

        public ReadOnlyReactiveProperty<NodeContextMenuState> State => _state;

        public void Show(NodeId nodeId, Vector2 position)
        {
            _state.Value = new NodeContextMenuState(nodeId, position, true);
        }

        public void Hide()
        {
            _state.Value = NodeContextMenuState.Invisible;
        }

        public List<ContextMenuItemViewModel> GetMenuItems()
        {
            var items = new List<ContextMenuItemViewModel>();

            // グラフコンテキストメニューの項目を定義
            items.Add(new ContextMenuItemViewModel(new ContextMenuItemState("Delete", true, DeleteNode)));

            return items;
        }


        void DeleteNode()
        {
            if (_state.Value.IsVisible)
            {
                _graphController.RemoveNode(_state.Value.NodeId);
            }

            Hide();
        }
    }
}
