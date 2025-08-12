using System;
using UnityEngine;

namespace Ugen.Serialization
{
    [Serializable]
    public class Graph
    {
        [SerializeReference, SerializeReferenceSelector]
        Node[] nodes;

        [SerializeField] Edge[] edges;

        public Node[] Nodes => nodes;
        public Edge[] Edges => edges;

        public Graph(Node[] nodes, Edge[] edges)
        {
            this.nodes = nodes;
            this.edges = edges;
        }
    }
}
