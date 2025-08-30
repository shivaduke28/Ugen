using System;
using System.Collections.Generic;
using ObservableCollections;
using R3;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public class GraphView : IDisposable
    {
        readonly VisualElement _root;
        readonly VisualElement _nodeLayer;
        readonly VisualElement _edgeLayer;
        readonly CompositeDisposable _disposables = new();
        readonly Dictionary<NodeId, NodeView> _nodeViews = new();
        readonly Dictionary<EdgeId, EdgeView> _edgeViews = new();
        readonly Dictionary<EdgeId, EdgeView> _previewEdgeViews = new();

        public GraphView(VisualElement container)
        {
            _root = container.Q<VisualElement>("graph");
            _nodeLayer = _root.Q<VisualElement>("node-layer");
            _edgeLayer = _root.Q<VisualElement>("edge-layer");
        }

        public IDisposable Bind(GraphViewModel graphViewModel)
        {
            var disposable = new CompositeDisposable();

            // ノードの追加を監視
            graphViewModel.Nodes.ObserveAdd().Subscribe(evt =>
            {
                var nodeId = evt.Value.Key;
                var nodeViewModel = evt.Value.Value;

                // NodeViewを作成
                var nodeElement = VisualElementFactory.Instance.CreateNode();
                var nodeView = new NodeView(nodeElement);

                // NodeViewをnodeLayerに追加
                _nodeLayer.Add(nodeView.Root);
                nodeView.Bind(nodeViewModel).AddTo(disposable);

                // NodeViewを管理
                _nodeViews[nodeId] = nodeView;
            }).AddTo(disposable);

            // ノードの削除を監視
            graphViewModel.Nodes.ObserveRemove().Subscribe(evt =>
            {
                var nodeId = evt.Value.Key;
                if (_nodeViews.Remove(nodeId, out var nodeView))
                {
                    _nodeLayer.Remove(nodeView.Root);
                    nodeView.Dispose();
                }
            }).AddTo(disposable);

            // エッジの追加を監視
            graphViewModel.Edges.ObserveAdd().Subscribe(evt =>
            {
                var edgeId = evt.Value.Key;
                var edgeViewModel = evt.Value.Value;

                // EdgeViewを作成
                var edgeView = new EdgeView(edgeViewModel);
                _edgeLayer.Add(edgeView);

                // EdgeViewを管理
                _edgeViews[edgeId] = edgeView;
            }).AddTo(disposable);

            // エッジの削除を監視
            graphViewModel.Edges.ObserveRemove().Subscribe(evt =>
            {
                var edgeId = evt.Value.Key;

                if (_edgeViews.Remove(edgeId, out var edgeView))
                {
                    _edgeLayer.Remove(edgeView);
                    edgeView.Dispose();
                }
            }).AddTo(disposable);

            graphViewModel.PreviewEdges.ObserveAdd().Subscribe(evt =>
            {
                var edgeId = evt.Value.Key;
                var endPoints = evt.Value.Value;

                // EdgeViewを作成
                var edgeView = new EdgeView(endPoints);
                _edgeLayer.Add(edgeView);

                _previewEdgeViews[edgeId] = edgeView;
            }).AddTo(disposable);

            // エッジの削除を監視
            graphViewModel.PreviewEdges.ObserveRemove().Subscribe(evt =>
            {
                var edgeId = evt.Value.Key;

                if (_previewEdgeViews.Remove(edgeId, out var edgeView))
                {
                    _edgeLayer.Remove(edgeView);
                    edgeView.Dispose();
                }
            }).AddTo(disposable);

            // 既存のノードとエッジを処理（初期化時）
            foreach (var kvp in graphViewModel.Nodes)
            {
                var nodeId = kvp.Key;
                var nodeViewModel = kvp.Value;

                var nodeElement = VisualElementFactory.Instance.CreateNode();
                var nodeView = new NodeView(nodeElement);
                _nodeLayer.Add(nodeView.Root);
                nodeView.Bind(nodeViewModel).AddTo(disposable);
                _nodeViews[nodeId] = nodeView;
            }

            foreach (var kvp in graphViewModel.Edges)
            {
                var edgeId = kvp.Key;
                var edgeViewModel = kvp.Value;

                var edgeView = new EdgeView(edgeViewModel);
                _edgeLayer.Add(edgeView);
                _edgeViews[edgeId] = edgeView;
            }

            return disposable;
        }

        public void Dispose()
        {
            // EdgeViewのDispose
            foreach (var edgeView in _edgeViews.Values)
            {
                edgeView.Dispose();
            }

            _edgeViews.Clear();
            foreach (var nodeView in _nodeViews.Values)
            {
                nodeView.Dispose();
            }

            _nodeViews.Clear();
            _disposables.Dispose();
        }
    }
}
