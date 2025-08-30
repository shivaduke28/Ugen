using System;
using UnityEngine;

namespace Ugen.Graphs.NodeContextMenu
{
    public readonly struct NodeContextMenuState : IEquatable<NodeContextMenuState>
    {
        public readonly NodeId NodeId;
        public readonly Vector2 Position;
        public readonly bool IsVisible;

        public static NodeContextMenuState Invisible => new();

        public NodeContextMenuState(NodeId nodeId, Vector2 position, bool isVisible)
        {
            NodeId = nodeId;
            Position = position;
            IsVisible = isVisible;
        }

        public bool Equals(NodeContextMenuState other)
        {
            return NodeId.Equals(other.NodeId) && Position.Equals(other.Position) && IsVisible == other.IsVisible;
        }

        public override bool Equals(object obj)
        {
            return obj is NodeContextMenuState other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NodeId, Position, IsVisible);
        }

        public static bool operator ==(NodeContextMenuState left, NodeContextMenuState right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NodeContextMenuState left, NodeContextMenuState right)
        {
            return !left.Equals(right);
        }
    }
}
