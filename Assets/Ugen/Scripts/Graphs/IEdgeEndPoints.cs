using R3;
using UnityEngine;

namespace Ugen.Graphs
{
    public interface IEdgeEndPoints
    {
        ReadOnlyReactiveProperty<Vector2> StartPosition { get; }
        ReadOnlyReactiveProperty<Vector2> EndPosition { get; }
    }
}