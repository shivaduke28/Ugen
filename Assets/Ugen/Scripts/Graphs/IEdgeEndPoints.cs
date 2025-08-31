using R3;
using UnityEngine;

namespace Ugen.Graphs
{
    public interface IEdgeEndPoints
    {
        ReadOnlyReactiveProperty<Vector2> OutputPanelPosition { get; }
        ReadOnlyReactiveProperty<Vector2> InputPanelPosition { get; }
    }
}
