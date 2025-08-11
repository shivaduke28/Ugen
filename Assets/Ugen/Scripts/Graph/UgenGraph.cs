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
        [SerializeField] List<NodeBehaviourBinding> bindings = new();

        public IReadOnlyList<UgenNode> Nodes => nodes;
        public IReadOnlyList<UgenEdge> Edges => edges;
        public IReadOnlyList<NodeBehaviourBinding> Bindings => bindings;

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

        public void AddBinding(NodeBehaviourBinding binding)
        {
            if (binding == null) return;
            bindings.Add(binding);
        }

        public string CreateBinding(UgenBehaviour behaviour)
        {
            if (behaviour == null) return null;

            var binding = new NodeBehaviourBinding
            {
                Behaviour = behaviour
            };

            bindings.Add(binding);
            return binding.BindingId;
        }

        public void RemoveBinding(string bindingId)
        {
            if (string.IsNullOrEmpty(bindingId)) return;
            bindings.RemoveAll(b => b.BindingId == bindingId);
        }

        public UgenBehaviour GetBoundBehaviourByBindingId(string bindingId)
        {
            var binding = bindings.Find(b => b.BindingId == bindingId);
            return binding?.Behaviour;
        }

        public UgenNode GetNode(string nodeId)
        {
            return nodes.Find(n => n.NodeId == nodeId);
        }

        public void Clear()
        {
            nodes.Clear();
            edges.Clear();
            bindings.Clear();
        }
    }

    [Serializable]
    public class NodeBehaviourBinding
    {
        [SerializeField] string bindingId;
        [SerializeField] UgenBehaviour behaviour;

        public string BindingId
        {
            get => bindingId;
            set => bindingId = value;
        }

        public UgenBehaviour Behaviour
        {
            get => behaviour;
            set => behaviour = value;
        }

        public NodeBehaviourBinding()
        {
            bindingId = Guid.NewGuid().ToString();
        }
    }
}
