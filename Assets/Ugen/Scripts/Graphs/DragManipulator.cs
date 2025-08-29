using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public class DragManipulator : MouseManipulator
    {
        readonly Action<Vector2> _onMove;
        bool _isActive;

        public DragManipulator(Action<Vector2> onMove)
        {
            _onMove = onMove;
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

            _onMove?.Invoke(evt.mouseDelta);
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