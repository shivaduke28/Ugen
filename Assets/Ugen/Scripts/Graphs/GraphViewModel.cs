using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;
using Ugen.Graphs.ContextMenu;
using Ugen.Graphs.Nodes;
using Ugen.Graphs.Ports;
using UnityEngine;

namespace Ugen.Graphs
{
    public sealed class GraphViewModel : IGraphController
    {
        readonly ObservableDictionary<NodeId, NodeViewModel> _nodes = new();
        readonly ObservableDictionary<EdgeId, EdgeViewModel> _edges = new();
        readonly ObservableDictionary<EdgeId, IEdgeEndPoints> _previewEdges = new();

        public IReadOnlyObservableDictionary<NodeId, NodeViewModel> Nodes => _nodes;
        public IReadOnlyObservableDictionary<EdgeId, EdgeViewModel> Edges => _edges;
        public IReadOnlyObservableDictionary<EdgeId, IEdgeEndPoints> PreviewEdges => _previewEdges;
        public ContextMenuViewModel<NodeId> NodeContextMenu { get; }
        public ContextMenuViewModel GraphContextMenu { get; }
        public ContextMenuViewModel<EdgeId> EdgeContextMenu { get; }

        public GraphViewModel()
        {
            NodeContextMenu = new ContextMenuViewModel<NodeId>(new[]
            {
                new ContextMenuItemViewModel(new ContextMenuItemState("Delete", true, RemoveContextNode)),
            });

            GraphContextMenu = new ContextMenuViewModel(new[]
            {
                new ContextMenuItemViewModel(new ContextMenuItemState("Float", true, () => CreateNode(id => new FloatNode(id)))),
                new ContextMenuItemViewModel(new ContextMenuItemState("Vector3", true, () => CreateNode(id => new Vector3Node(id)))),
                new ContextMenuItemViewModel(new ContextMenuItemState("Update", true, () => CreateNode(id => new UpdateNode(id)))),
                new ContextMenuItemViewModel(new ContextMenuItemState("Add Force", true, () => CreateNode(id => new AddForceNode(id)))),
            });

            EdgeContextMenu = new ContextMenuViewModel<EdgeId>(new[]
            {
                new ContextMenuItemViewModel(new ContextMenuItemState("Delete", true, RemoveContextEdge)),
            });
        }

        void RemoveContextNode()
        {
            RemoveNode(NodeContextMenu.Value);
            NodeContextMenu.Hide();
        }

        void RemoveContextEdge()
        {
            RemoveEdge(EdgeContextMenu.Value);
            EdgeContextMenu.Hide();
        }

        void CreateNode(Func<NodeId, Node> factory)
        {
            var node = factory(NodeId.New());
            var nodeViewModel = new NodeViewModel(node, this);
            nodeViewModel.SetPosition(GraphContextMenu.State.CurrentValue.Position);
            AddNode(nodeViewModel);
            GraphContextMenu.Hide();
        }

        public void AddTestData()
        {
            var nodes = new Node[]
            {
                new FloatNode(NodeId.New()),
                new FloatNode(NodeId.New()),
                new FloatNode(NodeId.New()),
                new Vector3Node(NodeId.New()),
            };

            foreach (var node in nodes)
            {
                var vm = new NodeViewModel(node, this);
                AddNode(vm);
            }

            var nodeIds = nodes.Select(x => x.Id).ToList();

            if (nodeIds.Count >= 2)
            {
                CreateEdge(nodeIds[0], 0, nodeIds[1], 0);

                if (nodeIds.Count >= 3)
                {
                    CreateEdge(nodeIds[1], 0, nodeIds[2], 0);
                }
            }
        }

        public void AddNode(NodeViewModel node)
        {
            _nodes.Add(node.Id, node);
        }

        public void AddEdge(EdgeViewModel edge)
        {
            _edges.Add(edge.Id, edge);
        }

        public bool RemoveNode(NodeId nodeId)
        {
            var edgesToRemove = new List<EdgeId>();
            foreach (var (edgeId, edge) in _edges)
            {
                if (edge.OutputPort.NodeId == nodeId || edge.InputPort.NodeId == nodeId)
                {
                    edgesToRemove.Add(edgeId);
                }
            }

            foreach (var edgeId in edgesToRemove)
            {
                RemoveEdge(edgeId);
            }

            return _nodes.Remove(nodeId);
        }

        void IGraphController.ShowNodeContextMenu(NodeId nodeId, Vector2 position)
        {
            NodeContextMenu.Show(position, nodeId);
        }

        public void ShowGraphContextMenu(Vector2 position)
        {
            GraphContextMenu.Show(position);
        }

        public void ShowEdgeContextMenu(EdgeId edgeId, Vector2 position)
        {
            EdgeContextMenu.Show(position, edgeId);
        }

        public bool RemoveEdge(EdgeId edgeId)
        {
            return _edges.Remove(edgeId);
        }

        public bool CreateEdge(NodeId outputNodeId, int outputPortIndex, NodeId inputNodeId, int inputPortIndex)
        {
            if (outputNodeId == inputNodeId) return false;
            if (!_nodes.TryGetValue(outputNodeId, out var outputNode)) return false;
            if (!_nodes.TryGetValue(inputNodeId, out var inputNode)) return false;

            if (!outputNode.TryGetOutputPort(outputPortIndex, out var outputPort)) return false;
            if (!inputNode.TryGetInputPort(inputPortIndex, out var inputPort)) return false;

            var edgeViewModel = new EdgeViewModel(outputPort, inputPort, this);
            AddEdge(edgeViewModel);
            return true;
        }

        public bool TryCreateEdge(NodeId outputNodeId, int outputPortIndex, NodeId inputNodeId, int inputPortIndex)
        {
            return CreateEdge(outputNodeId, outputPortIndex, inputNodeId, inputPortIndex);
        }

        public IDisposable CreatePreviewEdge(IEdgeEndPoints endPoints)
        {
            var id = EdgeId.New();
            _previewEdges.Add(id, endPoints);
            return Disposable.Create(() => _previewEdges.Remove(id));
        }
    }
}
