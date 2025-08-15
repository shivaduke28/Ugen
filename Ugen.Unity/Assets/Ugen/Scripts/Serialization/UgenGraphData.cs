using System;
using Ugen.Attributes;
using Ugen.Behaviours;
using UnityEngine;

namespace Ugen.Serialization
{
    [Serializable]
    public class UgenGraphData
    {
        [SerializeField] UgenBehaviour[] behaviours;
        [SerializeReference, SerializeReferenceSelector]
        UgenNodeData[] nodes;

        [SerializeField] EdgeData[] edges;

        public UgenBehaviour[] Behaviours => behaviours;
        public UgenNodeData[] Nodes => nodes;
        public EdgeData[] Edges => edges;

        public UgenGraphData(UgenBehaviour[] behaviours,  UgenNodeData[] nodes, EdgeData[] edges)
        {
            this.behaviours = behaviours;
            this.nodes = nodes;
            this.edges = edges;
        }
    }
}
