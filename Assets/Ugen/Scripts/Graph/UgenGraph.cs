using System;
using System.Collections.Generic;
using UnityEngine;
using Ugen.Behaviours;

namespace Ugen.Graph
{
    [Serializable]
    public sealed class UgenGraph
    {
        [SerializeReference, SerializeReferenceSelector]
        List<UgenNode> nodes = new();
        [SerializeField] List<UgenEdge> edges = new();
        [SerializeField] List<UgenBehaviour> behaviours = new();

        public IReadOnlyList<UgenNode> Nodes => nodes;
        public IReadOnlyList<UgenEdge> Edges => edges;
        public IReadOnlyList<UgenBehaviour> Behaviours => behaviours;

        public void AddNode(UgenNode node)
        {
            if (node == null) return;
            nodes.Add(node);
        }

        public void RemoveNode(UgenNode node)
        {
            if (node == null) return;

            // Remove edges related to this node
            edges.RemoveAll(e =>
                e.SourceNodeId == node.NodeId ||
                e.TargetNodeId == node.NodeId);

            // Note: We don't remove bindings here because multiple nodes can reference the same binding

            // Remove node
            nodes.Remove(node);
        }

        public void AddEdge(UgenEdge edge)
        {
            if (edge == null) return;

            // Validate edge
            var sourceNode = GetNode(edge.SourceNodeId);
            var targetNode = GetNode(edge.TargetNodeId);

            if (sourceNode == null || targetNode == null)
            {
                Debug.LogError("Invalid edge: node not found");
                return;
            }

            if (edge.SourcePortIndex >= sourceNode.OutputPorts.Count ||
                edge.TargetPortIndex >= targetNode.InputPorts.Count)
            {
                Debug.LogError("Invalid edge: port index out of range");
                return;
            }

            edges.Add(edge);
        }

        public void RemoveEdge(UgenEdge edge)
        {
            if (edge == null) return;
            edges.Remove(edge);
        }

        public UgenNode GetNode(string nodeId)
        {
            return nodes.Find(n => n.NodeId == nodeId);
        }

        public void AddBehaviour(UgenBehaviour behaviour)
        {
            behaviours.Add(behaviour);
        }

        public void ClearNodeAndEdges()
        {
            nodes.Clear();
            edges.Clear();
        }
        
        public void ClearEdges()
        {
            edges.Clear();
        }

        public void ClearBehaviours()
        {
            behaviours.Clear();
        }
    }
}
