#nullable enable
using System;
using UnityEngine;

namespace Ugen.Graphs.ContextMenu
{
    public readonly struct ContextMenuState : IEquatable<ContextMenuState>
    {
        public readonly bool IsVisible;
        public readonly Vector2 PanelPosition;

        public ContextMenuState(bool isVisible, Vector2 panelPosition)
        {
            IsVisible = isVisible;
            PanelPosition = panelPosition;
        }

        public bool Equals(ContextMenuState other)
        {
            return IsVisible == other.IsVisible && PanelPosition.Equals(other.PanelPosition);
        }

        public override bool Equals(object? obj)
        {
            return obj is ContextMenuState other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IsVisible, PanelPosition);
        }

        public static bool operator ==(ContextMenuState left, ContextMenuState right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ContextMenuState left, ContextMenuState right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"IsVisible: {IsVisible}, Position: {PanelPosition}";
        }
    }
}
