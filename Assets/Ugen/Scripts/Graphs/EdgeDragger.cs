using System;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public sealed class EdgePreviewEndPoints : IEdgeEndPoints
    {
        readonly ReactiveProperty<Vector2> _startPosition;
        readonly ReactiveProperty<Vector2> _endPosition;
        public ReadOnlyReactiveProperty<Vector2> StartPosition => _startPosition;

        public ReadOnlyReactiveProperty<Vector2> EndPosition => _endPosition;

        public EdgePreviewEndPoints(
            ReactiveProperty<Vector2> startPosition = null,
            ReactiveProperty<Vector2> endPosition = null)
        {
            _startPosition = startPosition ?? new ReactiveProperty<Vector2>();
            _endPosition = endPosition ?? new ReactiveProperty<Vector2>();
        }

        public void SetStartPosition(Vector2 position)
        {
            _startPosition.Value = position;
        }

        public void SetEndPosition(Vector2 position)
        {
            _endPosition.Value = position;
        }
    }

    public class EdgeDragger : MouseManipulator
    {
        bool _isActive;
        readonly Subject<Vector2> _onStart = new();
        readonly Subject<Vector2> _onMoveDelta = new();
        readonly Subject<Vector2> _onMove = new();
        readonly Subject<Vector2> _onEnd = new();
        public Observable<Vector2> OnStart() => _onStart;
        public Observable<Vector2> OnMoveDelta() => _onMoveDelta;
        public Observable<Vector2> OnMove() => _onMove;
        public Observable<Vector2> OnEnd() => _onEnd;

        readonly EdgeCreationRequest _request;
        readonly IEdgeEndPoints _endPoints;
        readonly EdgeCreator _edgeCreator;
        IDisposable _previewEdgeDisposable;

        public EdgeDragger(EdgeCreationRequest request, IEdgeEndPoints endPoints, EdgeCreator edgeCreator)
        {
            _isActive = false;
            _request = request;
            _endPoints = endPoints;
            _edgeCreator = edgeCreator;

            activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.LeftMouse
            });
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        void OnMouseDown(MouseDownEvent evt)
        {
            if (!CanStartManipulation(evt))
                return;

            _isActive = true;
            target.CaptureMouse();
            evt.StopPropagation();
            _onStart.OnNext(evt.mousePosition);
            _previewEdgeDisposable = _edgeCreator.CreatePreviewEdge(_endPoints);
        }

        void OnMouseMove(MouseMoveEvent evt)
        {
            if (!_isActive)
                return;

            _onMoveDelta.OnNext(evt.mouseDelta);
            _onMove.OnNext(evt.mousePosition);
            evt.StopPropagation();
        }

        void OnMouseUp(MouseUpEvent evt)
        {
            if (!_isActive)
                return;
            _previewEdgeDisposable?.Dispose();
            _previewEdgeDisposable = null;

            _onEnd.OnNext(evt.mousePosition);
            _isActive = false;
            target.ReleaseMouse();
            var pick = target.panel.Pick(evt.mousePosition);
            if (pick is PortPickerView picker)
            {
                picker.TryCreateEdge(_request);
            }
        }
    }
}
