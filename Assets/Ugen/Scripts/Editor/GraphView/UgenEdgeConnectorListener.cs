using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Ugen.Editor.GraphView
{
    public sealed class UgenEdgeConnectorListener : IEdgeConnectorListener
    {
        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
        }

        public void OnDrop(UnityEditor.Experimental.GraphView.GraphView graphView, Edge edge)
        {
            if (graphView is UgenGraphView ugenGraphView)
            {
                var ugenEdge = new UgenEdgeView
                {
                    output = edge.output,
                    input = edge.input
                };

                graphView.RemoveElement(edge);

                ugenEdge.output.Connect(ugenEdge);
                ugenEdge.input.Connect(ugenEdge);
                ugenGraphView.AddEdgeView(ugenEdge);
            }
        }
    }
}
