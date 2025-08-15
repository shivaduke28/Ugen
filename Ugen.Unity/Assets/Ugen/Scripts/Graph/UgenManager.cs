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
        [SerializeField] UgenGraphData _graphData;
        [SerializeField] bool _autoExecuteOnStart = true;

        readonly CompositeDisposable graphDisposable = new();
        public UgenGraphData GraphData => _graphData;

        void Start()
        {
            if (_autoExecuteOnStart) ExecuteGraph();
        }

        void ExecuteGraph()
        {
            var nodes = new Dictionary<string, UgenNode>();
            foreach (var nodeData in _graphData.Nodes)
            {
                UgenNode node = null;
                switch (nodeData)
                {
                    case AddNodeData addNodeData:
                        node = new AddNode(addNodeData.Id);
                        break;
                    case UgenBehaviourNodeData behaviourNodeData:
                        if (behaviourNodeData.Behaviour is { } behaviour) node = new UgenBehaviourNode(nodeData.Id, behaviour);

                        break;
                }

                if (node is IInitializable initializable) initializable.Initialize();

                if (node is IDisposable dis) graphDisposable.Add(dis);

                if (node != null) nodes[node.NodeId] = node;
            }

            foreach (var edge in _graphData.Edges)
                nodes[edge.OutputNodeId].OutputPorts[edge.OutputPortIndex].ConnectTo(
                    nodes[edge.InputNodeId].InputPorts[edge.InputPortIndex], graphDisposable);

            Debug.Log($"Graph executed: {_graphData.Nodes.Length} nodes, {_graphData.Edges.Length} edges");
        }


        public void CollectBehavioursFromScene()
        {
            var behaviours = FindObjectsByType<UgenBehaviour>(FindObjectsSortMode.None);
            _graphData = new UgenGraphData(behaviours, _graphData.Nodes, _graphData.Edges);
        }

        public void ClearGraph()
        {
            CollectBehavioursFromScene();
            _graphData = new UgenGraphData(_graphData.Behaviours, Array.Empty<UgenNodeData>(), Array.Empty<EdgeData>());
        }

        public void SaveGraph(UgenGraphData newGraphData)
        {
            Assert.IsFalse(Application.isPlaying, "Cannot save graph while in play mode");
            _graphData = newGraphData;
        }
    }
}
