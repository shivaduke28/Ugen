using System;
using UnityEngine;

namespace Ugen.Serialization
{
    [Serializable]
    public sealed class EdgeData
    {
        [SerializeField] string id;
        [SerializeField] string inputNodeId;
        [SerializeField] int inputPortIndex;
        [SerializeField] string outputNodeId;
        [SerializeField] int outputPortIndex;

        public string Id => id;
        public string InputNodeId => inputNodeId;
        public int InputPortIndex => inputPortIndex;
        public string OutputNodeId => outputNodeId;
        public int OutputPortIndex => outputPortIndex;

        public EdgeData(string id, string inputNodeId, int inputPortIndex, string outputNodeId, int outputPortIndex)
        {
            this.id = id;
            this.inputNodeId = inputNodeId;
            this.inputPortIndex = inputPortIndex;
            this.outputNodeId = outputNodeId;
            this.outputPortIndex = outputPortIndex;
        }
    }
}
