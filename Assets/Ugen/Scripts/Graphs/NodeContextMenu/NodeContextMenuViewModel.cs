using R3;
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

        public void Delete()
        {
            _graphController.RemoveNode(_state.Value.NodeId);
            Hide();
        }

        public void Hide()
        {
            _state.Value = NodeContextMenuState.Invisible;
        }
    }
}
