using System;
using System.Collections.Generic;
using ObservableCollections;
using R3;
using Ugen.Graphs.ContextMenu;
using Ugen.Graphs.Nodes;
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
        readonly PanManipulator _panManipulator;
        ContextMenuView _nodeContextMenuView;
        ContextMenuView _graphContextMenuView;
        ContextMenuView _edgeContextMenuView;

        public GraphView(VisualElement container)
        {
            _root = container.Q<VisualElement>("graph");
            _nodeLayer = _root.Q<VisualElement>("node-layer");
            _edgeLayer = _root.Q<VisualElement>("edge-layer");

            // パン機能を追加
            _panManipulator = new PanManipulator(_nodeLayer);
            _root.AddManipulator(_panManipulator);
        }

        public IDisposable Bind(GraphViewModel graphViewModel)
        {
            var disposable = new CompositeDisposable();

            // ノードコンテキストメニューを作成
            var nodeContextMenuElement = VisualElementFactory.Instance.CreateContextMenu();
            _nodeContextMenuView = new ContextMenuView(nodeContextMenuElement, graphViewModel.NodeContextMenu);
            _root.Add(_nodeContextMenuView.Root);
            Disposable.Create(() => _nodeContextMenuView.Dispose()).AddTo(disposable);

            // グラフコンテキストメニューを作成
            var graphContextMenuElement = VisualElementFactory.Instance.CreateContextMenu();
            _graphContextMenuView = new ContextMenuView(graphContextMenuElement, graphViewModel.GraphContextMenu);
            _root.Add(_graphContextMenuView.Root);
            Disposable.Create(() => _graphContextMenuView.Dispose()).AddTo(disposable);

            // エッジコンテキストメニューを作成
            var edgeContextMenuElement = VisualElementFactory.Instance.CreateContextMenu();
            _edgeContextMenuView = new ContextMenuView(edgeContextMenuElement, graphViewModel.EdgeContextMenu);
            _root.Add(_edgeContextMenuView.Root);
            Disposable.Create(() => _edgeContextMenuView.Dispose()).AddTo(disposable);

            // 背景クリックでメニューを閉じる
            _root.RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.button == 0) // 左クリック
                {
                    graphViewModel.NodeContextMenu.Hide();
                    graphViewModel.GraphContextMenu.Hide();
                    graphViewModel.EdgeContextMenu.Hide();
                }
                else if (evt.button == 1) // 右クリック
                {
                    // 他のUI要素上でない場合のみグラフコンテキストメニューを表示
                    if (evt.target == _root || evt.target == _nodeLayer || evt.target == _edgeLayer)
                    {
                        evt.StopPropagation();
                        var localPosition = _root.WorldToLocal(evt.position);
                        graphViewModel.ShowGraphContextMenu(localPosition);
                    }
                }
            });

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
            foreach (var (nodeId, nodeViewModel) in graphViewModel.Nodes)
            {
                var nodeElement = VisualElementFactory.Instance.CreateNode();
                var nodeView = new NodeView(nodeElement);

                _nodeLayer.Add(nodeView.Root);
                nodeView.Bind(nodeViewModel).AddTo(disposable);
                _nodeViews[nodeId] = nodeView;
            }

            foreach (var (edgeId, edgeViewModel) in graphViewModel.Edges)
            {
                var edgeView = new EdgeView(edgeViewModel);
                _edgeLayer.Add(edgeView);
                _edgeViews[edgeId] = edgeView;
            }

            return disposable;
        }

        public void Dispose()
        {
            // PanManipulatorを削除
            _root.RemoveManipulator(_panManipulator);

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
