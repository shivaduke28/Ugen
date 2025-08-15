using System;
using Ugen.Attributes;
using Ugen.Behaviours;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Serialization
{
    [Serializable]
    public class UgenGraphData
    {
        [SerializeField] UgenBehaviour[] _behaviours;
        [SerializeField] VisualTreeAsset _visualTreeAsset;

        [SerializeReference, SerializeReferenceSelector,]
        UgenNodeData[] _nodes;

        [SerializeField] EdgeData[] _edges;

        public UgenBehaviour[] Behaviours => _behaviours;
        public VisualTreeAsset VisualTreeAsset => _visualTreeAsset;
        public UgenNodeData[] Nodes => _nodes;
        public EdgeData[] Edges => _edges;

        public UgenGraphData(UgenBehaviour[] behaviours, VisualTreeAsset visualTreeAsset,   UgenNodeData[] nodes, EdgeData[] edges)
        {
            _behaviours = behaviours;
            _visualTreeAsset = visualTreeAsset;
            _nodes = nodes;
            _edges = edges;
        }
    }
}
