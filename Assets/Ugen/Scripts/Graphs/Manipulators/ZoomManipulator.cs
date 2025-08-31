using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs.Manipulators
{
    public class ZoomManipulator : Manipulator
    {
        readonly VisualElement _translation;
        float _currentScale = 1.0f;
        const float MinScale = 0.1f;
        const float MaxScale = 3.0f;
        const float ZoomSpeed = 0.1f;

        public ZoomManipulator(VisualElement translation)
        {
            _translation = translation;
        }

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
            // マウスホイールのdeltaを取得
            var delta = evt.delta.y;
            if (Mathf.Abs(delta) < 0.01f) return;

            // ズーム倍率を計算
            var scaleFactor = 1.0f - (delta * ZoomSpeed * 0.01f);
            var newScale = Mathf.Clamp(_currentScale * scaleFactor, MinScale, MaxScale);

            if (Mathf.Abs(newScale - _currentScale) < 0.001f) return;

            // スケールを更新
            _currentScale = newScale;
            _translation.style.scale = new Scale(new Vector3(_currentScale, _currentScale, 1));

            evt.StopPropagation();
        }
    }
}
