using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using Ugen.Behaviours;
using Ugen.Graph.Nodes;

namespace Ugen.Graph
{
    public sealed class UgenManager : MonoBehaviour
    {
        [SerializeField] UgenGraph graph = new();
        [SerializeField] bool autoExecuteOnStart = true;

        readonly CompositeDisposable graphDisposable = new();

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
            var nodes = new Dictionary<string, UgenNode>();
            foreach (var node in graph.Nodes)
            {
                if (node is IInitializable initializable)
                {
                    initializable.Initialize();
                }

                if (node is IDisposable dis)
                {
                    graphDisposable.Add(dis);
                }

                nodes[node.NodeId] = node;
            }

            foreach (var edge in graph.Edges)
            {
                nodes[edge.OutputNodeId].OutputPorts[edge.OutputPortIndex].ConnectTo(
                    nodes[edge.InputNodeId].InputPorts[edge.InputPortIndex], graphDisposable);
            }

            Debug.Log($"Graph executed: {graph.Nodes.Count} nodes, {graph.Edges.Count} edges");
        }


        public void CollectBehavioursFromScene()
        {
            graph.ClearNodeAndEdges();

            var behaviours = FindObjectsByType<UgenBehaviour>(FindObjectsSortMode.None);
            foreach (var behaviour in behaviours)
            {
                graph.AddBehaviour(behaviour);
                UgenBehaviourNode node;
                switch (behaviour)
                {
                    case UgenSlider ugenSlider:
                        var sliderNode = new SliderNode();
                        sliderNode.SetBehaviour(ugenSlider);
                        node = sliderNode;
                        break;
                    case UgenYawRotator rotator:
                        var rotatorNode = new YawRotatorNode();
                        rotatorNode.SetBehaviour(rotator);
                        node = rotatorNode;
                        break;
                    default:
                        node = null;
                        break;
                }

                if (node != null)
                {
                    graph.AddNode(node);
                    Debug.Log($"Collected {behaviour.GetType().Name} as node '{node.NodeId}'");
                }
            }

            Debug.Log($"Collected {graph.Nodes.Count} behaviours from scene");
        }
    }
}
