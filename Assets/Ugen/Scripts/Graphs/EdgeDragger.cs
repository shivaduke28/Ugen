using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
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

        public EdgeDragger(EdgeCreationRequest request)
        {
            _isActive = false;
            _request = request;

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
            _onMove.OnNext(evt.mousePosition);
            evt.StopPropagation();
        }

        void OnMouseUp(MouseUpEvent evt)
        {
            if (!_isActive)
                return;

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
