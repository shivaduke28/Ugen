using System;
using UnityEngine;

namespace Ugen.Graph
{
    [Serializable]
    public sealed class UgenEdge
    {
        [SerializeField] string edgeId;
        [SerializeField] string outputNodeId;
        [SerializeField] int outputPortIndex;
        [SerializeField] string inputNodeId;
        [SerializeField] int inputPortIndex;

        public string EdgeId
        {
            get => edgeId;
            set => edgeId = value;
        }

        public string OutputNodeId
        {
            get => outputNodeId;
            set => outputNodeId = value;
        }

        public int OutputPortIndex
        {
            get => outputPortIndex;
            set => outputPortIndex = value;
        }

        public string InputNodeId
        {
            get => inputNodeId;
            set => inputNodeId = value;
        }

        public int InputPortIndex
        {
            get => inputPortIndex;
            set => inputPortIndex = value;
        }

        public UgenEdge()
        {
            edgeId = Guid.NewGuid().ToString();
        }

        public UgenEdge(string outputNode, int outputPort, string inputNode, int inputPort)
        {
            edgeId = Guid.NewGuid().ToString();
            outputNodeId = outputNode;
            outputPortIndex = outputPort;
            inputNodeId = inputNode;
            inputPortIndex = inputPort;
        }
    }
}
