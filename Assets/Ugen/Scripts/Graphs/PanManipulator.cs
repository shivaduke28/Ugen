using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public sealed class PanManipulator : Manipulator
    {
        Vector2 _startMousePosition;
        Vector2 _startPanPosition;
        bool _isPanning;
        readonly VisualElement _nodeLayer;

        public PanManipulator(VisualElement nodeLayer)
        {
            _nodeLayer = nodeLayer;
        }

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
            if (evt.target != target && evt.target != _nodeLayer) return;

            _isPanning = true;
            _startMousePosition = new Vector2(evt.position.x, evt.position.y);

            // 現在のtranslate値を取得
            var currentTranslate = _nodeLayer.style.translate.value;
            _startPanPosition = new Vector2(currentTranslate.x.value, currentTranslate.y.value);

            target.CapturePointer(evt.pointerId);
            evt.StopPropagation();
        }

        void OnPointerMove(PointerMoveEvent evt)
        {
            if (!_isPanning) return;

            var currentPosition = new Vector2(evt.position.x, evt.position.y);
            var delta = currentPosition - _startMousePosition;
            var newPosition = _startPanPosition + delta;

            _nodeLayer.style.translate = new Translate(newPosition.x, newPosition.y);

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
