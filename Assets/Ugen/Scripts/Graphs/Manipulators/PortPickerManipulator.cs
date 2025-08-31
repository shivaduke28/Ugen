using R3;
using Ugen.Graphs.Ports;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs.Manipulators
{
    public class PortPickerManipulator : MouseManipulator
    {
        bool _isActive;
        readonly Subject<Vector2> _onStartPanelPosition = new();
        readonly Subject<Vector2> _onMovePanelPosition = new();
        readonly Subject<PortData?> _onEnd = new();
        public Observable<Vector2> OnStartPanelPosition() => _onStartPanelPosition;
        public Observable<Vector2> OnMovePanelPosition() => _onMovePanelPosition;
        public Observable<PortData?> OnEnd() => _onEnd;

        public PortPickerManipulator()
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
            var panelPosition = evt.mousePosition;
            _onStartPanelPosition.OnNext(panelPosition);
        }

        void OnMouseMove(MouseMoveEvent evt)
        {
            if (!_isActive)
                return;

            var worldPos = evt.mousePosition;
            _onMovePanelPosition.OnNext(worldPos);
            evt.StopPropagation();
        }

        void OnMouseUp(MouseUpEvent evt)
        {
            if (!_isActive)
                return;

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
