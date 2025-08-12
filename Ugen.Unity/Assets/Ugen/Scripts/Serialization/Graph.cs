using System;
using Ugen.Attributes;
using UnityEngine;

namespace Ugen.Serialization
{
    [Serializable]
    public class Graph
    {
        [SerializeReference, SerializeReferenceSelector]
        UgenNodeData[] nodes;

        [SerializeField] Edge[] edges;

        public UgenNodeData[] Nodes => nodes;
        public Edge[] Edges => edges;

        public Graph(UgenNodeData[] nodes, Edge[] edges)
        {
            this.nodes = nodes;
            this.edges = edges;
        }
    }
}
