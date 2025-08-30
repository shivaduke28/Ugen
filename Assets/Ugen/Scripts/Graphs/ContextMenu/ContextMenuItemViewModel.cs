using R3;

namespace Ugen.Graphs.ContextMenu
{
    public sealed class ContextMenuItemViewModel
    {
        readonly ReactiveProperty<ContextMenuItemState> _state = new();
        public ReadOnlyReactiveProperty<ContextMenuItemState> State => _state;

        public ContextMenuItemViewModel(ContextMenuItemState state)
        {
            _state.Value = state;
        }
    }
}
