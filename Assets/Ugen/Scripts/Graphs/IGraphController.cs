using System;
using UnityEngine;

namespace Ugen.Graphs
{
    public interface IGraphController
    {
        void AddNode(NodeViewModel node);
        bool CreateEdge(NodeId outputNodeId, int outputPortIndex, NodeId inputNodeId, int inputPortIndex);
        IDisposable CreatePreviewEdge(IEdgeEndPoints endPoints);
        bool RemoveEdge(EdgeId edgeId);
        bool RemoveNode(NodeId nodeId);
        void ShowNodeContextMenu(NodeId nodeId, Vector2 position);
    }
}
