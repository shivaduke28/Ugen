using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using Ugen.Behaviours;
using Ugen.Graph.Nodes;
using Ugen.Serialization;
using UnityEngine.Assertions;

namespace Ugen.Graph
{
    public sealed class UgenManager : MonoBehaviour
    {
        [SerializeField] UgenGraphData graphData;
        [SerializeField] bool autoExecuteOnStart = true;

        readonly CompositeDisposable graphDisposable = new();
        public UgenGraphData GraphData => graphData;

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
            foreach (var nodeData in graphData.Nodes)
            {
                UgenNode node = null;
                switch (nodeData)
                {
                    case AddNodeData addNodeData:
                        node = new AddNode(addNodeData.Id);
                        break;
                    case UgenBehaviourNodeData behaviourNodeData:
                        if (behaviourNodeData.Behaviour is { } behaviour)
                        {
                            node = new UgenBehaviourNode(nodeData.Id, behaviour);
                        }

                        break;
                }

                if (node is IInitializable initializable)
                {
                    initializable.Initialize();
                }

                if (node is IDisposable dis)
                {
                    graphDisposable.Add(dis);
                }

                if (node != null)
                {
                    nodes[node.NodeId] = node;
                }
            }

            foreach (var edge in graphData.Edges)
            {
                nodes[edge.OutputNodeId].OutputPorts[edge.OutputPortIndex].ConnectTo(
                    nodes[edge.InputNodeId].InputPorts[edge.InputPortIndex], graphDisposable);
            }

            Debug.Log($"Graph executed: {graphData.Nodes.Length} nodes, {graphData.Edges.Length} edges");
        }


        public void CollectBehavioursFromScene()
        {
            var behaviours = FindObjectsByType<UgenBehaviour>(FindObjectsSortMode.None);
            graphData = new UgenGraphData(behaviours, graphData.Nodes, graphData.Edges);
        }

        public void ClearGraph()
        {
            CollectBehavioursFromScene();
            graphData = new UgenGraphData(graphData.Behaviours, Array.Empty<UgenNodeData>(), Array.Empty<EdgeData>());
        }

        public void SaveGraph(UgenGraphData newGraphData)
        {
            Assert.IsFalse(Application.isPlaying, "Cannot save graph while in play mode");
            graphData = newGraphData;
        }
    }
}
