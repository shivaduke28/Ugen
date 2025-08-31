using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public class ZoomManipulator : Manipulator
    {
        readonly VisualElement _nodeLayer;
        float _currentScale = 1.0f;
        const float MinScale = 0.1f;
        const float MaxScale = 3.0f;
        const float ZoomSpeed = 0.1f;
        
        public ZoomManipulator(VisualElement nodeLayer)
        {
            _nodeLayer = nodeLayer;
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
            
            // マウス位置を中心にズーム
            var mousePosition = target.WorldToLocal(evt.mousePosition);
            
            // 現在のtranslate値を取得
            var currentTranslate = _nodeLayer.style.translate.value;
            var currentPosition = new Vector2(currentTranslate.x.value, currentTranslate.y.value);
            
            // ズームの中心点を基準に位置を調整
            var scaleRatio = newScale / _currentScale;
            var newPosition = mousePosition + (currentPosition - mousePosition) * scaleRatio;
            
            // スケールと位置を更新
            _currentScale = newScale;
            
            // nodeLayerのみスケールと位置を更新（edgeLayerは動かさない）
            _nodeLayer.style.scale = new Scale(new Vector3(_currentScale, _currentScale, 1));
            _nodeLayer.style.translate = new Translate(newPosition.x, newPosition.y);
            
            evt.StopPropagation();
        }
    }
}
