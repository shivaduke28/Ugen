using System;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs.Edges
{
    public class EdgeView : VisualElement, IDisposable
    {
        readonly CompositeDisposable _disposable = new();
        readonly Subject<Vector2> _onClickPanelPosition = new();
        public Observable<Vector2> OnClickPanelPosition() => _onClickPanelPosition;

        Vector2 _startPosition;
        Vector2 _endPosition;
        const float Offset = 8f;
        const float Padding = 20f;
        bool _isHovered;

        public EdgeView()
        {
            style.position = Position.Absolute;
            pickingMode = PickingMode.Position;

            generateVisualContent += OnGenerateVisualContent;

            RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.button == 1) // 右クリック
                {
                    evt.StopPropagation();
                    _onClickPanelPosition.OnNext(evt.position);
                }
            });

            // マウスホバーイベントの登録
            RegisterCallback<PointerEnterEvent>(_ =>
            {
                _isHovered = true;
                MarkDirtyRepaint();
            });

            RegisterCallback<PointerLeaveEvent>(_ =>
            {
                _isHovered = false;
                MarkDirtyRepaint();
            });
        }

        public void Bind(IEdgeEndPoints edgeViewModel)
        {
            // StartPositionとEndPositionの変更を購読して再描画
            edgeViewModel.OutputPosition.Subscribe(position =>
            {
                _startPosition = position;
                UpdateBounds();
                MarkDirtyRepaint();
            }).AddTo(_disposable);

            edgeViewModel.InputPosition.Subscribe(position =>
            {
                _endPosition = position;
                UpdateBounds();
                MarkDirtyRepaint();
            }).AddTo(_disposable);
        }

        void UpdateBounds()
        {
            var startLocal = _startPosition;
            var endLocal = _endPosition;

            // Z字型の4つの点を考慮した境界矩形を計算
            var p2 = new Vector2(startLocal.x + Offset, startLocal.y);
            var p3 = new Vector2(endLocal.x - Offset, endLocal.y);

            // 最小・最大座標を計算
            var minX = Mathf.Min(startLocal.x, p2.x, p3.x, endLocal.x) - Padding;
            var maxX = Mathf.Max(startLocal.x, p2.x, p3.x, endLocal.x) + Padding;
            var minY = Mathf.Min(startLocal.y, p2.y, p3.y, endLocal.y) - Padding;
            var maxY = Mathf.Max(startLocal.y, p2.y, p3.y, endLocal.y) + Padding;

            // VisualElementの位置とサイズを設定
            style.left = minX;
            style.top = minY;
            style.width = maxX - minX;
            style.height = maxY - minY;
        }

        public override bool ContainsPoint(Vector2 localPoint)
        {
            const float threshold = 10f;

            var parentStartLocal = _startPosition;
            var parentEndLocal = _endPosition;

            var startLocal = new Vector2(
                parentStartLocal.x - style.left.value.value,
                parentStartLocal.y - style.top.value.value
            );
            var endLocal = new Vector2(
                parentEndLocal.x - style.left.value.value,
                parentEndLocal.y - style.top.value.value
            );

            var p2 = new Vector2(startLocal.x + Offset, startLocal.y);
            var p3 = new Vector2(endLocal.x - Offset, endLocal.y);
            var distance = DistanceToLineSegment(localPoint, p2, p3);
            return distance <= threshold;
        }

        static float DistanceToLineSegment(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
        {
            var lineVec = lineEnd - lineStart;
            var pointVec = point - lineStart;
            var lineLength = lineVec.sqrMagnitude;

            if (lineLength == 0)
                return Vector2.Distance(point, lineStart);

            var t = Mathf.Clamp01(Vector2.Dot(pointVec, lineVec) / lineLength);
            var projection = lineStart + t * lineVec;
            return Vector2.Distance(point, projection);
        }

        void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            if (_startPosition == _endPosition)
                return;

            if (parent == null)
                return;

            var parentStartLocal = _startPosition;
            var parentEndLocal = _endPosition;

            // このEdgeViewのローカル座標系に変換
            var startLocal = new Vector2(
                parentStartLocal.x - style.left.value.value,
                parentStartLocal.y - style.top.value.value
            );
            var endLocal = new Vector2(
                parentEndLocal.x - style.left.value.value,
                parentEndLocal.y - style.top.value.value
            );

            var painter = mgc.painter2D;
            painter.strokeColor = _isHovered ? new Color(0.5f, 1f, 1f) : Color.white;
            painter.lineWidth = 1.0f;

            painter.BeginPath();
            painter.MoveTo(startLocal);

            // Z字型のパス
            var p2 = new Vector2(startLocal.x + Offset, startLocal.y);
            var p3 = new Vector2(endLocal.x - Offset, endLocal.y);

            painter.LineTo(p2);
            painter.LineTo(p3);
            painter.LineTo(endLocal);
            painter.Stroke();
        }

        public void Dispose()
        {
            _onClickPanelPosition.Dispose();
            _disposable.Dispose();
            generateVisualContent -= OnGenerateVisualContent;
        }
    }
}
