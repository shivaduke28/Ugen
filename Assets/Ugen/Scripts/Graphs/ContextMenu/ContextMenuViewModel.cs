#nullable enable
using R3;
using UnityEngine;

namespace Ugen.Graphs.ContextMenu
{
    public class ContextMenuViewModel
    {
        protected readonly ReactiveProperty<ContextMenuState> _state = new();
        public ReadOnlyReactiveProperty<ContextMenuState> State => _state;
        public ContextMenuItemViewModel[] Items { get; }

        public ContextMenuViewModel(ContextMenuItemViewModel[] items)
        {
            Items = items;
        }

        public void Show(Vector2 position)
        {
            _state.Value = new ContextMenuState(true, position);
        }

        public void Hide()
        {
            _state.Value = new ContextMenuState(false, Vector2.zero);
        }
    }

    public class ContextMenuViewModel<T> : ContextMenuViewModel
    {
        public T? Value { get; private set; }

        public ContextMenuViewModel(ContextMenuItemViewModel[] items) : base(items)
        {
        }

        public void Show(Vector2 panelPosition, T value)
        {
            Value = value;
            _state.Value = new ContextMenuState(true, panelPosition);
        }
    }
}
