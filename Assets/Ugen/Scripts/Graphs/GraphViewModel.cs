using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;
using Ugen.Graphs.ContextMenu;
using Ugen.Graphs.Edges;
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
        readonly ReactiveProperty<GraphTransform> _transform = new(GraphTransform.Default);

        public IReadOnlyObservableDictionary<NodeId, NodeViewModel> Nodes => _nodes;
        public IReadOnlyObservableDictionary<EdgeId, EdgeViewModel> Edges => _edges;
        public IReadOnlyObservableDictionary<EdgeId, IEdgeEndPoints> PreviewEdges => _previewEdges;
        public ReadOnlyReactiveProperty<GraphTransform> Transform => _transform;
        public ContextMenuViewModel<NodeId> NodeContextMenu { get; }
        public ContextMenuViewModel<Vector2> GraphContextMenu { get; }
        public ContextMenuViewModel<EdgeId> EdgeContextMenu { get; }

        public GraphViewModel()
        {
            NodeContextMenu = new ContextMenuViewModel<NodeId>(new[]
            {
                new ContextMenuItemViewModel(new ContextMenuItemState("Delete", true, RemoveContextNode)),
            });

            GraphContextMenu = new ContextMenuViewModel<Vector2>(new[]
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
            var nodeGraphPosition = GraphContextMenu.Value;
            AddNode(node, nodeGraphPosition);
            GraphContextMenu.Hide();
        }

        public NodeViewModel AddNode(Node node, Vector2 nodeGraphPosition)
        {
            var vm = new NodeViewModel(node, this);
            vm.SetPosition(nodeGraphPosition);
            _nodes.Add(node.Id, vm);
            return vm;
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

        public Vector2 ConvertPanelToGraph(Vector2 panelPosition)
        {
            var trans = _transform.Value;
            return (panelPosition - trans.Position) * (1.0f / trans.Zoom);
        }

        public void ShowGraphContextMenu(Vector2 panelPosition)
        {
            GraphContextMenu.Show(panelPosition, ConvertPanelToGraph(panelPosition));
        }

        public void ShowEdgeContextMenu(EdgeId edgeId, Vector2 panelPosition)
        {
            EdgeContextMenu.Show(panelPosition, edgeId);
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

            var edgeViewModel = new EdgeViewModel(outputPort, inputPort);
            AddEdge(edgeViewModel);
            return true;
        }

        public void TransformGraph(GraphTransform transform)
        {
            _transform.Value = transform;
        }

        public IDisposable CreatePreviewEdge(IEdgeEndPoints endPoints)
        {
            var id = EdgeId.New();
            _previewEdges.Add(id, endPoints);
            return Disposable.Create(() => _previewEdges.Remove(id));
        }
    }
}
