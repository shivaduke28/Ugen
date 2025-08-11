using System;
using UnityEngine;

namespace Ugen.Graph
{
    [Serializable]
    public sealed class UgenEdge
    {
        [SerializeField] string edgeId;
        [SerializeField] string sourceNodeId;
        [SerializeField] int sourcePortIndex;
        [SerializeField] string targetNodeId;
        [SerializeField] int targetPortIndex;

        public string EdgeId
        {
            get => edgeId;
            set => edgeId = value;
        }

        public string SourceNodeId
        {
            get => sourceNodeId;
            set => sourceNodeId = value;
        }

        public int SourcePortIndex
        {
            get => sourcePortIndex;
            set => sourcePortIndex = value;
        }

        public string TargetNodeId
        {
            get => targetNodeId;
            set => targetNodeId = value;
        }

        public int TargetPortIndex
        {
            get => targetPortIndex;
            set => targetPortIndex = value;
        }

        public UgenEdge()
        {
            edgeId = Guid.NewGuid().ToString();
        }

        public UgenEdge(string sourceNode, int sourcePort, string targetNode, int targetPort)
        {
            edgeId = Guid.NewGuid().ToString();
            sourceNodeId = sourceNode;
            sourcePortIndex = sourcePort;
            targetNodeId = targetNode;
            targetPortIndex = targetPort;
        }
    }
}