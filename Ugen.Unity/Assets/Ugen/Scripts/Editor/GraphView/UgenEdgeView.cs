using System;
using UnityEditor.Experimental.GraphView;

namespace Ugen.Editor.GraphView
{
    public sealed class UgenEdgeView : Edge
    {
        public string EdgeId { get; }

        public UgenEdgeView()
        {
            EdgeId = Guid.NewGuid().ToString();
        }

        public UgenEdgeView(string edgeId)
        {
            EdgeId = edgeId ?? Guid.NewGuid().ToString();
        }
    }
}
