using System.Collections.Generic;
using UnityEngine;
using Ugen.Behaviours;
using Ugen.Graph.Nodes;

namespace Ugen.Graph
{
    public sealed class UgenManager : MonoBehaviour
    {
        [SerializeField] UgenGraph graph = new();
        [SerializeField] bool autoExecuteOnStart = true;

        readonly Dictionary<string, UgenBehaviour> runtimeBehaviours = new();

        public UgenGraph Graph => graph;

        void Start()
        {
            if (autoExecuteOnStart)
            {
                ExecuteGraph();
            }
        }

        void ExecuteGraph()
        {
            // Clear previous runtime behaviours
            ClearRuntimeBehaviours();

            // Create behaviours for each node
            foreach (var node in graph.Nodes)
            {
                var behaviour = graph.GetBoundBehaviour(node.NodeId);
                if (behaviour != null)
                {
                    // Use existing bound behaviour
                    runtimeBehaviours[node.NodeId] = behaviour;
                }
                else
                {
                    // Try to create behaviour automatically (for demo purposes)
                    behaviour = TryCreateBehaviour(node);
                    if (behaviour != null)
                    {
                        runtimeBehaviours[node.NodeId] = behaviour;
                    }
                }
            }

            // Establish connections
            foreach (var connection in graph.Connections)
            {
                EstablishConnection(connection);
            }

            Debug.Log($"Graph executed: {graph.Nodes.Count} nodes, {graph.Connections.Count} connections");
        }

        void EstablishConnection(UgenConnection connection)
        {
            if (!runtimeBehaviours.TryGetValue(connection.SourceNodeId, out var sourceBehaviour))
            {
                Debug.LogWarning($"Source behaviour not found for node: {connection.SourceNodeId}");
                return;
            }

            if (!runtimeBehaviours.TryGetValue(connection.TargetNodeId, out var targetBehaviour))
            {
                Debug.LogWarning($"Target behaviour not found for node: {connection.TargetNodeId}");
                return;
            }

            sourceBehaviour.ConnectTo(
                connection.SourcePortIndex,
                targetBehaviour,
                connection.TargetPortIndex
            );
        }

        UgenBehaviour TryCreateBehaviour(UgenNode node)
        {
            switch (node)
            {
                case SliderNode:
                {
                    // For slider, we need to find or create a UI element
                    var slider = FindFirstObjectByType<UgenSlider>();
                    if (slider != null)
                    {
                        return slider;
                    }

                    Debug.LogWarning("No UgenSlider found in scene");
                    return null;
                }
                case YawRotatorNode:
                {
                    // For rotator, we can add it to any GameObject
                    var go = new GameObject($"YawRotator_{node.NodeId}");
                    go.transform.SetParent(transform);
                    return go.AddComponent<UgenYawRotator>();
                }
                default:
                    return null;
            }
        }

        void ClearRuntimeBehaviours()
        {
            // Clean up dynamically created behaviours
            foreach (var kvp in runtimeBehaviours)
            {
                var behaviour = kvp.Value;
                if (behaviour != null && behaviour.gameObject.name.StartsWith("YawRotator_"))
                {
                    Destroy(behaviour.gameObject);
                }
            }

            runtimeBehaviours.Clear();
        }

        void OnDestroy()
        {
            ClearRuntimeBehaviours();
        }

        public void CollectBehavioursFromScene()
        {
            graph.Clear();

            var behaviours = FindObjectsByType<UgenBehaviour>(FindObjectsSortMode.None);
            foreach (var behaviour in behaviours)
            {
                // Create appropriate node based on behaviour type
                UgenNode node = behaviour switch
                {
                    UgenSlider => new SliderNode(),
                    UgenYawRotator => new YawRotatorNode(),
                    _ => null
                };

                if (node != null)
                {
                    // Use GameObject name as node ID for easy identification
                    node.NodeId = behaviour.gameObject.name;
                    graph.AddNode(node);
                    graph.BindNodeToBehaviour(node.NodeId, behaviour);

                    Debug.Log($"Collected {behaviour.GetType().Name} as node '{node.NodeId}'");
                }
            }

            Debug.Log($"Collected {graph.Nodes.Count} behaviours from scene");
        }
    }
}
