using R3;
using UnityEngine;

namespace Ugen.Graphs
{
    public interface IEdgeEndPoints
    {
        ReadOnlyReactiveProperty<Vector2> OutputPosition { get; }
        ReadOnlyReactiveProperty<Vector2> InputPosition { get; }
    }
}
