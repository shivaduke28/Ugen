#nullable enable
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs.Manipulators
{
    public class DragManipulator : MouseManipulator
    {
        bool _isActive;
        readonly Subject<Vector2> _onMoveDelta = new();
        public Observable<Vector2> OnMoveDelta() => _onMoveDelta;

        public DragManipulator()
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
        }

        void OnMouseMove(MouseMoveEvent evt)
        {
            if (!_isActive)
                return;

            _onMoveDelta.OnNext(evt.mouseDelta);
            evt.StopPropagation();
        }

        void OnMouseUp(MouseUpEvent evt)
        {
            if (!_isActive)
                return;

            _isActive = false;
            target.ReleaseMouse();
            evt.StopPropagation();
        }
    }
}
