using System;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs.Ports
{
    public class EdgePreviewDragger : MouseManipulator
    {
        bool _isActive;
        readonly Subject<Vector2> _onStart = new();
        readonly Subject<Vector2> _onMove = new();
        readonly Subject<PortData?> _onEnd = new();
        public Observable<Vector2> OnStart() => _onStart;
        public Observable<Vector2> OnMove() => _onMove;
        public Observable<PortData?> OnEnd() => _onEnd;

        IDisposable _previewEdgeDisposable;

        public EdgePreviewDragger()
        {
            _isActive = false;

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
        }

        void OnMouseMove(MouseMoveEvent evt)
        {
            if (!_isActive)
                return;

            _onMove.OnNext(evt.mousePosition);
            evt.StopPropagation();
        }

        void OnMouseUp(MouseUpEvent evt)
        {
            if (!_isActive)
                return;
            _previewEdgeDisposable?.Dispose();
            _previewEdgeDisposable = null;

            PortData? portData = null;
            if (target.panel.Pick(evt.mousePosition) is PortPickerView picker)
            {
                portData = picker.PortData;
            }

            _onEnd.OnNext(portData);
            _isActive = false;
            target.ReleaseMouse();
        }
    }
}
