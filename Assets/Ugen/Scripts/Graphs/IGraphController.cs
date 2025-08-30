using UnityEngine;

namespace Ugen.Graphs
{
    public interface IGraphController
    {
        bool CreateEdge(NodeId outputNodeId, int outputPortIndex, NodeId inputNodeId, int inputPortIndex);
        bool RemoveEdge(EdgeId edgeId);
        bool RemoveNode(NodeId nodeId);
        void ShowNodeContextMenu(NodeId nodeId, Vector2 position);
    }
}
