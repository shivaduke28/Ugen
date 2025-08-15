using System;
using Ugen.Attributes;
using Ugen.Behaviours;
using UnityEngine;

namespace Ugen.Serialization
{
    [Serializable]
    public class UgenGraphData
    {
        [SerializeField] UgenBehaviour[] _behaviours;

        [SerializeReference, SerializeReferenceSelector,]
        UgenNodeData[] _nodes;

        [SerializeField] EdgeData[] _edges;

        public UgenBehaviour[] Behaviours => _behaviours;
        public UgenNodeData[] Nodes => _nodes;
        public EdgeData[] Edges => _edges;

        public UgenGraphData(UgenBehaviour[] behaviours, UgenNodeData[] nodes, EdgeData[] edges)
        {
            _behaviours = behaviours;
            _nodes = nodes;
            _edges = edges;
        }
    }
}
