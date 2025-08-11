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
                UgenBehaviour behaviour = null;
                
                // For UgenBehaviourNode, try to get behaviour by bindingId
                if (node is UgenBehaviourNode behaviourNode && !string.IsNullOrEmpty(behaviourNode.BindingId))
                {
                    behaviour = graph.GetBoundBehaviourByBindingId(behaviourNode.BindingId);
                }
                
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
            foreach (var edge in graph.Edges)
            {
                EstablishConnection(edge);
            }

            Debug.Log($"Graph executed: {graph.Nodes.Count} nodes, {graph.Edges.Count} edges");
        }

        void EstablishConnection(UgenEdge edge)
        {
            if (!runtimeBehaviours.TryGetValue(edge.SourceNodeId, out var sourceBehaviour))
            {
                Debug.LogWarning($"Source behaviour not found for node: {edge.SourceNodeId}");
                return;
            }

            if (!runtimeBehaviours.TryGetValue(edge.TargetNodeId, out var targetBehaviour))
            {
                Debug.LogWarning($"Target behaviour not found for node: {edge.TargetNodeId}");
                return;
            }

            sourceBehaviour.ConnectTo(
                edge.SourcePortIndex,
                targetBehaviour,
                edge.TargetPortIndex
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
                UgenBehaviourNode node = behaviour switch
                {
                    UgenSlider => new SliderNode(),
                    UgenYawRotator => new YawRotatorNode(),
                    _ => null
                };

                if (node != null)
                {
                    // Use GameObject name as node ID for easy identification
                    node.NodeId = behaviour.gameObject.name;
                    
                    // Create binding and set the bindingId on the node
                    var binding = new NodeBehaviourBinding
                    {
                        Behaviour = behaviour
                    };
                    node.BindingId = binding.BindingId;
                    
                    graph.AddNode(node);
                    graph.AddBinding(binding);

                    Debug.Log($"Collected {behaviour.GetType().Name} as node '{node.NodeId}' with binding '{node.BindingId}'");
                }
            }

            Debug.Log($"Collected {graph.Nodes.Count} behaviours from scene");
        }
    }
}
