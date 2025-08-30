using UnityEngine;

namespace Ugen.Graphs.EdgeContextMenu
{
    public readonly struct EdgeContextMenuState
    {
        public EdgeId EdgeId { get; }
        public Vector2 Position { get; }
        public bool IsVisible { get; }

        public EdgeContextMenuState(EdgeId edgeId, Vector2 position, bool isVisible)
        {
            EdgeId = edgeId;
            Position = position;
            IsVisible = isVisible;
        }

        public static EdgeContextMenuState Invisible => new(EdgeId.Invalid, Vector2.zero, false);
    }
}