using R3;
using UnityEngine;

namespace Ugen.Graphs
{
    public interface IEdgeEndPoints
    {
        /// <summary>
        /// グラフ座標
        /// </summary>
        ReadOnlyReactiveProperty<Vector2> OutputPosition { get; }

        /// <summary>
        /// グラフ座標
        /// </summary>
        ReadOnlyReactiveProperty<Vector2> InputPosition { get; }
    }
}
