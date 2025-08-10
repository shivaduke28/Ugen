using System;
using System.Collections.Generic;
using UnityEngine;
using Ugen.Behaviours;

namespace Ugen.Graph
{
    [Serializable]
    public sealed class UgenGraph
    {
        [SerializeField] List<SerializedUgenNode> serializedNodes = new();
        [SerializeField] List<UgenConnection> connections = new();
        [SerializeField] List<NodeBehaviourBinding> bindings = new();

        List<UgenNode> runtimeNodes;

        public IReadOnlyList<UgenNode> Nodes
        {
            get
            {
                if (runtimeNodes == null)
                {
                    DeserializeNodes();
                }
                return runtimeNodes;
            }
        }

        public IReadOnlyList<UgenConnection> Connections => connections;
        public IReadOnlyList<NodeBehaviourBinding> Bindings => bindings;

        void DeserializeNodes()
        {
            runtimeNodes = new List<UgenNode>();
            foreach (var serializedNode in serializedNodes)
            {
                var node = serializedNode.ToUgenNode();
                if (node != null)
                {
                    runtimeNodes.Add(node);
                }
            }
        }

        void SerializeNodes()
        {
            serializedNodes.Clear();
            if (runtimeNodes != null)
            {
                foreach (var node in runtimeNodes)
                {
                    serializedNodes.Add(new SerializedUgenNode(node));
                }
            }
        }

        public void AddNode(UgenNode node)
        {
            if (node == null) return;

            if (runtimeNodes == null)
            {
                DeserializeNodes();
            }

            runtimeNodes.Add(node);
            SerializeNodes();
        }

        public void RemoveNode(UgenNode node)
        {
            if (node == null) return;

            if (runtimeNodes == null)
            {
                DeserializeNodes();
            }

            // Remove connections related to this node
            connections.RemoveAll(c =>
                c.SourceNodeId == node.NodeId ||
                c.TargetNodeId == node.NodeId);

            // Remove bindings
            bindings.RemoveAll(b => b.NodeId == node.NodeId);

            // Remove node
            runtimeNodes.Remove(node);
            SerializeNodes();
        }

        public void AddConnection(UgenConnection connection)
        {
            if (connection == null) return;

            // Validate connection
            var sourceNode = GetNode(connection.SourceNodeId);
            var targetNode = GetNode(connection.TargetNodeId);

            if (sourceNode == null || targetNode == null)
            {
                Debug.LogError("Invalid connection: node not found");
                return;
            }

            if (connection.SourcePortIndex >= sourceNode.OutputPorts.Count ||
                connection.TargetPortIndex >= targetNode.InputPorts.Count)
            {
                Debug.LogError("Invalid connection: port index out of range");
                return;
            }

            connections.Add(connection);
        }

        public void RemoveConnection(UgenConnection connection)
        {
            if (connection == null) return;
            connections.Remove(connection);
        }

        public void BindNodeToBehaviour(string nodeId, UgenBehaviour behaviour)
        {
            if (string.IsNullOrEmpty(nodeId) || behaviour == null) return;

            // Remove existing binding for this node
            bindings.RemoveAll(b => b.NodeId == nodeId);

            // Add new binding
            bindings.Add(new NodeBehaviourBinding
            {
                NodeId = nodeId,
                Behaviour = behaviour
            });
        }

        public UgenBehaviour GetBoundBehaviour(string nodeId)
        {
            var binding = bindings.Find(b => b.NodeId == nodeId);
            return binding?.Behaviour;
        }

        public UgenNode GetNode(string nodeId)
        {
            if (runtimeNodes == null)
            {
                DeserializeNodes();
            }
            return runtimeNodes.Find(n => n.NodeId == nodeId);
        }

        public void Clear()
        {
            serializedNodes.Clear();
            runtimeNodes?.Clear();
            connections.Clear();
            bindings.Clear();
        }
    }

    [Serializable]
    public class NodeBehaviourBinding
    {
        [SerializeField] string nodeId;
        [SerializeField] UgenBehaviour behaviour;

        public string NodeId
        {
            get => nodeId;
            set => nodeId = value;
        }

        public UgenBehaviour Behaviour
        {
            get => behaviour;
            set => behaviour = value;
        }
    }
}
