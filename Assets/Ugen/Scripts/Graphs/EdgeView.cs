using System;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public class EdgeView : VisualElement, IDisposable
    {
        readonly CompositeDisposable _disposable = new();
        readonly EdgeId _edgeId;
        readonly IGraphController _graphController;

        Vector2 _startPos;
        Vector2 _endPos;

        // EdgeViewModel用のコンストラクタ
        public EdgeView(EdgeViewModel edgeViewModel)
        {
            style.position = Position.Absolute;
            style.left = 0;
            style.top = 0;
            style.right = 0;
            style.bottom = 0;

            _edgeId = edgeViewModel.Id;
            _graphController = edgeViewModel.GraphController;

            generateVisualContent += OnGenerateVisualContent;

            // StartPositionとEndPositionの変更を購読して再描画
            edgeViewModel.OutputPosition.Subscribe(pos =>
            {
                _startPos = pos;
                MarkDirtyRepaint();
            }).AddTo(_disposable);

            edgeViewModel.InputPosition.Subscribe(pos =>
            {
                _endPos = pos;
                MarkDirtyRepaint();
            }).AddTo(_disposable);

            // 右クリックでコンテキストメニューを表示
            RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.button == 1) // 右クリック
                {
                    evt.StopPropagation();
                    var localPosition = this.parent.WorldToLocal(evt.position);
                    _graphController.ShowEdgeContextMenu(_edgeId, localPosition);
                }
            });
        }

        // プレビューエッジ用のコンストラクタ
        public EdgeView(IEdgeEndPoints edgeEndPoints)
        {
            style.position = Position.Absolute;
            style.left = 0;
            style.top = 0;
            style.right = 0;
            style.bottom = 0;

            _edgeId = EdgeId.Invalid;
            _graphController = null;

            generateVisualContent += OnGenerateVisualContent;

            // StartPositionとEndPositionの変更を購読して再描画
            edgeEndPoints.OutputPosition.Subscribe(pos =>
            {
                _startPos = pos;
                MarkDirtyRepaint();
            }).AddTo(_disposable);

            edgeEndPoints.InputPosition.Subscribe(pos =>
            {
                _endPos = pos;
                MarkDirtyRepaint();
            }).AddTo(_disposable);
        }

        public override bool ContainsPoint(Vector2 localPoint)
        {
            const float threshold = 10f;
            const int sampleCount = 20;

            var minDistance = float.MaxValue;

            for (var i = 0; i <= sampleCount; i++)
            {
                var t = i / (float)sampleCount;
                var pointOnCurve = CalculateBezierPoint(t);
                var distance = Vector2.Distance(localPoint, pointOnCurve);
                minDistance = Mathf.Min(minDistance, distance);

                if (minDistance <= threshold)
                    return true;
            }

            return minDistance <= threshold;
        }

        Vector2 CalculateBezierPoint(float t)
        {
            // ベジェ曲線のコントロールポイントを計算（OnGenerateVisualContentと同じロジック）
            var distance = Vector2.Distance(_startPos, _endPos);
            var controlPointOffset = Mathf.Min(distance * 0.5f, 100f);
            var controlPoint1 = new Vector2(_startPos.x + controlPointOffset, _startPos.y);
            var controlPoint2 = new Vector2(_endPos.x - controlPointOffset, _endPos.y);

            // 3次ベジェ曲線の計算
            var oneMinusT = 1f - t;
            var oneMinusTSquared = oneMinusT * oneMinusT;
            var oneMinusTCubed = oneMinusTSquared * oneMinusT;
            var tSquared = t * t;
            var tCubed = tSquared * t;

            return oneMinusTCubed * _startPos +
                   3f * oneMinusTSquared * t * controlPoint1 +
                   3f * oneMinusT * tSquared * controlPoint2 +
                   tCubed * _endPos;
        }

        void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            if (_startPos == _endPos)
                return;

            var painter = mgc.painter2D;
            painter.strokeColor = Color.white;
            painter.lineWidth = 1.0f;

            painter.BeginPath();
            painter.MoveTo(_startPos);

            // ベジェ曲線のコントロールポイントを計算
            var distance = Vector2.Distance(_startPos, _endPos);
            var controlPointOffset = Mathf.Min(distance * 0.5f, 100f);

            var controlPoint1 = new Vector2(_startPos.x + controlPointOffset, _startPos.y);
            var controlPoint2 = new Vector2(_endPos.x - controlPointOffset, _endPos.y);

            painter.BezierCurveTo(controlPoint1, controlPoint2, _endPos);
            painter.Stroke();
        }

        public void Dispose()
        {
            _disposable.Dispose();
            generateVisualContent -= OnGenerateVisualContent;
        }
    }
}
