using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs.Manipulators
{
    public class PanManipulator : Manipulator
    {
        bool _isPanning;
        readonly Subject<Vector2> _onPanDelta= new();
        public Observable<Vector2> OnPanDelta() => _onPanDelta;

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            target.RegisterCallback<PointerUpEvent>(OnPointerUp);
            target.RegisterCallback<PointerCaptureOutEvent>(OnPointerCaptureOut);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
            target.UnregisterCallback<PointerCaptureOutEvent>(OnPointerCaptureOut);
        }

        void OnPointerDown(PointerDownEvent evt)
        {
            // 左クリックかつ背景上でのみパンを開始
            if (evt.button != 0) return;

            _isPanning = true;
            target.CapturePointer(evt.pointerId);
            evt.StopPropagation();
        }

        void OnPointerMove(PointerMoveEvent evt)
        {
            if (!_isPanning) return;

            _onPanDelta.OnNext(evt.deltaPosition);
            evt.StopPropagation();
        }

        void OnPointerUp(PointerUpEvent evt)
        {
            if (!_isPanning) return;

            _isPanning = false;
            target.ReleasePointer(evt.pointerId);
            evt.StopPropagation();
        }

        void OnPointerCaptureOut(PointerCaptureOutEvent evt)
        {
            _isPanning = false;
        }
    }
}
