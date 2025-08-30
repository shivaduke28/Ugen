using System.Collections.Generic;
using R3;
using Ugen.Graphs.ContextMenu;
using UnityEngine;

namespace Ugen.Graphs.EdgeContextMenu
{
    public sealed class EdgeContextMenuViewModel
    {
        readonly IGraphController _graphController;
        readonly ReactiveProperty<EdgeContextMenuState> _state = new();

        public EdgeContextMenuViewModel(IGraphController graphController)
        {
            _graphController = graphController;
        }

        public ReadOnlyReactiveProperty<EdgeContextMenuState> State => _state;

        public void Show(EdgeId edgeId, Vector2 position)
        {
            _state.Value = new EdgeContextMenuState(edgeId, position, true);
        }

        public void Hide()
        {
            _state.Value = EdgeContextMenuState.Invisible;
        }

        public List<ContextMenuItemViewModel> GetMenuItems()
        {
            var items = new List<ContextMenuItemViewModel>();
            
            // エッジコンテキストメニューの項目を定義
            items.Add(new ContextMenuItemViewModel(new ContextMenuItemState("Delete Edge", true, DeleteEdge)));
            
            return items;
        }

        void DeleteEdge()
        {
            if (_state.Value.IsVisible)
            {
                _graphController.RemoveEdge(_state.Value.EdgeId);
            }
            Hide();
        }
    }
}
