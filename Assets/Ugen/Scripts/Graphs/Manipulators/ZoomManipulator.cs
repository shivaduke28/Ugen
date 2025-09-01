using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs.Manipulators
{
    public class ZoomManipulator : Manipulator
    {
        readonly Subject<float> _onZoomDelta = new();
        const float ZoomSpeed = 0.1f;

        public Observable<float> OnZoomDelta() => _onZoomDelta;

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

            var scaleFactor = 1.0f - delta * ZoomSpeed * 0.01f;

            _onZoomDelta.OnNext(scaleFactor);
            evt.StopPropagation();
        }
    }
}
