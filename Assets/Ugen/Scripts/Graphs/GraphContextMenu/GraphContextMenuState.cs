using UnityEngine;

namespace Ugen.Graphs.GraphContextMenu
{
    public readonly struct GraphContextMenuState
    {
        public bool IsVisible { get; }
        public Vector2 Position { get; }

        public GraphContextMenuState(bool isVisible, Vector2 position)
        {
            IsVisible = isVisible;
            Position = position;
        }

        public static GraphContextMenuState Hidden => new(false, Vector2.zero);
    }
}
