using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs.Manipulators
{
    public class ZoomManipulator : Manipulator
    {
        readonly Subject<(Vector2 panelPosition, float zoomDelta)> _onZoomDelta = new();
        const float ZoomSpeed = 0.1f;

        public Observable<(Vector2 panelPosition, float zoomDelta)> OnZoomDelta() => _onZoomDelta;

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<WheelEvent>(OnWheel);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<WheelEvent>(OnWheel);
        }

        void OnWheel(WheelEvent evt)
        {
            var delta = evt.delta.y;
            if (Mathf.Abs(delta) < 0.01f) return;

            var scaleFactor = 1.0f - delta * ZoomSpeed * 0.05f;

            _onZoomDelta.OnNext((evt.mousePosition, scaleFactor));
            evt.StopPropagation();
        }
    }
}
