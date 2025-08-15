using System;
using UnityEngine;

namespace Ugen.Serialization
{
    [Serializable]
    public sealed class EdgeData
    {
        [SerializeField] string _id;
        [SerializeField] string _inputNodeId;
        [SerializeField] int _inputPortIndex;
        [SerializeField] string _outputNodeId;
        [SerializeField] int _outputPortIndex;

        public string Id => _id;
        public string InputNodeId => _inputNodeId;
        public int InputPortIndex => _inputPortIndex;
        public string OutputNodeId => _outputNodeId;
        public int OutputPortIndex => _outputPortIndex;

        public EdgeData(string id, string inputNodeId, int inputPortIndex, string outputNodeId, int outputPortIndex)
        {
            _id = id;
            _inputNodeId = inputNodeId;
            _inputPortIndex = inputPortIndex;
            _outputNodeId = outputNodeId;
            _outputPortIndex = outputPortIndex;
        }
    }
}
