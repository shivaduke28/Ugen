using System;

namespace Ugen.Graphs.ContextMenu
{
    public readonly struct ContextMenuItemState : IEquatable<ContextMenuItemState>
    {
        public readonly string Label;
        public readonly bool IsEnabled;
        public readonly Action OnClick;

        public ContextMenuItemState(string label, bool isEnabled, Action onClick)
        {
            Label = label;
            IsEnabled = isEnabled;
            OnClick = onClick;
        }

        public bool Equals(ContextMenuItemState other)
        {
            return Label == other.Label && IsEnabled == other.IsEnabled && Equals(OnClick, other.OnClick);
        }

        public override bool Equals(object obj)
        {
            return obj is ContextMenuItemState other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Label, IsEnabled, OnClick);
        }

        public static bool operator ==(ContextMenuItemState left, ContextMenuItemState right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ContextMenuItemState left, ContextMenuItemState right)
        {
            return !left.Equals(right);
        }
    }
}